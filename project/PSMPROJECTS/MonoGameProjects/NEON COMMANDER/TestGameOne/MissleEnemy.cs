#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
using TestGameOne;
#endregion

namespace TestGameOne
{
    public class MissleEnemy
    {

        public enum EnemyStates
        {
            SPAWNING,
            ATTACKING,
        }

        #region MISSILE_ENEMY_VARIABLES
        Vector2 m_facingDirection;
        Vector2 m_position;
        Vector2 m_headingPosition;
        Vector2 m_seekPosition;
        Vector2 m_missileEnemyRoam;
        Vector2 m_velocity;
        Vector2 m_force;
        Vector2 m_acceleration;
        private Vector2 m_dimensions;
        private Vector2 m_origin;

        private Circle m_collisionRadius;
        private Circle m_detectionRadius;
        private float m_rotationAngle;
        private float m_maxVelocity;
        private float m_enemyDirChange;
        private float m_spriteScale;
        private float m_lerpColour;
        public float m_speed { get; set; }

        public bool m_seeking;
        bool m_reachedTarget;
        public bool m_slowMotion { get; set; }

        private Texture2D m_missileEnemyTexture;

        public EnemyStates m_state = EnemyStates.SPAWNING;
        #endregion

        /// <summary>
        /// Constructor for the Missle Enemy.
        /// </summary>
        /// <param name="position"></param>
        public MissleEnemy(Vector2 position, Texture2D missileEnemyTexture)
        {
            m_missileEnemyTexture = missileEnemyTexture;
            m_facingDirection = new Vector2();
            m_position = position;
            m_spriteScale = 0.35f;
            m_dimensions = new Vector2(74, 70) * m_spriteScale;
            m_origin = (m_dimensions / 2.0f) / m_spriteScale;

            m_maxVelocity = 0.0f;

            m_detectionRadius = new Circle(m_position, 100);
            m_collisionRadius = new Circle(m_position, m_dimensions.X / 2 * m_spriteScale);

            m_seeking = false;
            m_reachedTarget = false;
            m_slowMotion = false;

            m_enemyDirChange = 2.0f;
            m_speed = 200.0f;
        }

        /// <summary>
        /// Update function for the missile enemy.
        /// </summary>
        /// <param name="gT"></param>
        public void Update(GameTime gT)
        {
            m_enemyDirChange -= (float)gT.ElapsedGameTime.TotalSeconds;

            if (m_slowMotion == true)
            {
                m_maxVelocity = 15.0f;
                m_seeking = true;
             
            }
            else
            {
                m_maxVelocity = 200.0f;
            }
           
            switch (m_state)
            {
                case EnemyStates.SPAWNING: UpdateSpawning(gT); break;
                case EnemyStates.ATTACKING: UpdateAttacking(gT); break;
            }
        }
        
        /// <summary>
        /// Draw Function for the Missile Enemy.
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(m_missileEnemyTexture, m_position, new Rectangle(0, 0, (int)(m_dimensions.X / m_spriteScale), (int)(m_dimensions.Y / m_spriteScale)), Color.Lerp(Color.Transparent, Color.White, m_lerpColour), m_rotationAngle, m_origin, m_spriteScale, SpriteEffects.None, 0.0f);            
        }

        /// <summary>
        /// Checks if the player/object is in the line of sight of the missile enemy.
        /// Line of sight is currently 180 degrees in front of the missile.
        /// </summary>
        /// <param name="position">The position of the object you want to check against</param>
        /// <returns></returns>
        public bool IsInLineOfSight(Vector2 position)
        {

            if (m_state == EnemyStates.ATTACKING)
            {
                float targetAngle = MathFunctions.AngleBetween360Degrees(m_position, position);
                m_facingDirection = new Vector2((float)Math.Sin(m_rotationAngle), -(float)Math.Cos(m_rotationAngle));
                Vector2 targetDirection = new Vector2((float)Math.Sin(targetAngle), -(float)Math.Cos(targetAngle));
                float angle = Vector2.Dot(m_facingDirection, targetDirection);

                if (angle > 0.75 && !m_seeking)
                {
                    m_rotationAngle = targetAngle;
                    m_facingDirection = targetDirection;
                    m_seekPosition = position;
                    m_seeking = true;
                    m_maxVelocity += 150f;
                    m_enemyDirChange = 1.0f;
                    return true;
                }

                else
                {
                    return false;
                }
            }

            return false;
        
        }

        /// <summary>
        /// Update for the Spawning state of the Missile enemy
        /// </summary>
        /// <param name="gT"></param>
        void UpdateSpawning(GameTime gT)
        {
            m_lerpColour += 0.02f;

            if (m_lerpColour >= 1.0f)
            {
                ChangeState(EnemyStates.ATTACKING);
            }
        }

        /// <summary>
        /// Update for the Attacking state of the Missile enemy
        /// </summary>
        /// <param name="gT"></param>
        void UpdateAttacking(GameTime gT)
        {
            //If not seeking, then it will wander.
            if (!m_seeking)
            {
                if (m_enemyDirChange <= 0 || (m_missileEnemyRoam - m_position).Length() < 16.0f)
                {
                    m_missileEnemyRoam = new Vector2(Globals.m_rng.Next(0, Globals.m_gameWidth), Globals.m_rng.Next(0, Globals.m_gameHeight));
                }

                m_headingPosition = m_position + m_velocity;
                m_force = GetForce(m_missileEnemyRoam);
                m_acceleration = m_force * (float)gT.ElapsedGameTime.TotalSeconds * m_speed;
                m_velocity += m_acceleration * (float)gT.ElapsedGameTime.TotalSeconds;
                if (m_velocity.X != 0 || m_velocity.Y != 0)
                {
                    m_velocity.Normalize();
                }

                m_velocity *= m_maxVelocity;

                m_position += m_velocity * (float)gT.ElapsedGameTime.TotalSeconds;

                m_rotationAngle = MathFunctions.AngleBetween360Degrees(m_position, m_headingPosition);
            }

            //If seeking and hasn't reached close to the players position yet
            else if (m_seeking && !m_reachedTarget)
            {
                if (Vector2.Distance(m_position, m_seekPosition) < 32f && m_enemyDirChange <= 0)
                {
                    m_reachedTarget = true;
                    return;
                }
                m_headingPosition = m_position + m_velocity;
                m_force = GetForce(m_seekPosition);
                m_acceleration = m_force * (float)gT.ElapsedGameTime.TotalSeconds * m_speed;
                m_velocity += m_acceleration * (float)gT.ElapsedGameTime.TotalSeconds;
                if (m_velocity.X != 0 || m_velocity.Y != 0)
                {
                    m_velocity.Normalize();
                }

                m_velocity *= m_maxVelocity;

                m_position += m_velocity * (float)gT.ElapsedGameTime.TotalSeconds;

                m_rotationAngle = MathFunctions.AngleBetween360Degrees(m_position, m_headingPosition);
            }

            else if (m_seeking && m_reachedTarget)
            {
                m_position += m_facingDirection * (float)gT.ElapsedGameTime.TotalSeconds * m_maxVelocity;
            }

            m_detectionRadius.m_center = m_position;
            m_collisionRadius.m_center = m_position;
        }

        void ChangeState(EnemyStates state)
        {
            m_state = state;
        }

        /// <summary>
        /// Returns the position of the enemy.
        /// </summary>
        /// <returns></returns>
        public Vector2 GetPosition()
        {
            return m_position;
        }

        /// <summary>
        /// Returns the collision radius for the enemy.
        /// </summary>
        /// <returns></returns>
        public Circle GetCollisionCircle()
        {
            return m_collisionRadius;
        }             

        /// <summary>
        /// Returns the detection radius.
        /// </summary>
        /// <returns></returns>
        public Circle GetDetectionCircle()
        {
            return m_detectionRadius;
        }

        /// <summary>
        /// Returns the force to move the missile enemy from it's current heading position (position + velocity)
        /// Towards the target position passed in.
        /// </summary>
        /// <param name="targetPosition">The position that you want the missile to move towards</param>
        /// <returns></returns>
        Vector2 GetForce(Vector2 targetPosition)
        {
            Vector2 force;
            
            force = targetPosition - m_headingPosition;

            if (m_velocity.X != 0 || m_velocity.Y != 0)
            {
                force.Normalize();
            }

            return force * 100;
        }
    }
}
