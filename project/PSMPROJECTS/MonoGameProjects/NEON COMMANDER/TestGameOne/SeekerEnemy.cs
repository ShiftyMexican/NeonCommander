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
    public class SeekerEnemy
    {

        public enum EnemyStates
        {
            SPAWNING,
            SEEKING,
        }

        #region SEEKER_ENEMY_VARIABLES
        public Vector2 m_facingDirection;
        public Vector2 m_position;
        private Vector2 m_dimensions;
        private Vector2 m_origin;

        public Vector2 m_seekingPosition;
        public Vector2 m_headingPosition;
        public Vector2 m_velocity;
        public Vector2 m_force;
        public Vector2 m_acceleration;

        public bool m_flocking { get; set; }
        public bool m_slowMotion { get; set; }

        public Circle m_collisionRadius;

        private float m_spriteScale;
        private float m_maxVelocity;
        public  float m_rotationAngle;
        public float m_bladeRotation { get; set; }
        public float m_speed { get; set; }
        private float m_colorlerp;

        Player m_pPlayer;

        private Texture2D m_seekerEnemyTexture;
        private Texture2D m_seekerEnemyRotatingBladeTexture;

        public EnemyStates m_state = EnemyStates.SPAWNING;
        #endregion

        /// <summary>
        /// Constructor for the Seeker Enemy.
        /// </summary>
        /// <param name="position"></param>
        public SeekerEnemy(Vector2 position, Texture2D seekerEnemyTexture, Texture2D seekerEnemyRotatingBladeTexture, Player pPlayer)
        {
            m_seekerEnemyTexture = seekerEnemyTexture;
            m_seekerEnemyRotatingBladeTexture = seekerEnemyRotatingBladeTexture;
            m_facingDirection = new Vector2();
            m_position = position;
            m_spriteScale = 0.35f;
            m_dimensions = new Vector2(64, 64) * m_spriteScale;
            m_origin = (m_dimensions / 2.0f) / m_spriteScale;

            m_collisionRadius = new Circle(m_position, m_dimensions.X / 2);
            m_flocking = false;

            m_speed = 14.0f;
            m_bladeRotation = 0.0f;
            m_rotationAngle = 0.0f;

            m_seekingPosition = new Vector2(0, 0);

            m_pPlayer = pPlayer;

            m_maxVelocity = 20.0f;

            m_colorlerp = 0.0f;

            m_slowMotion = false;
        }

        /// <summary>
        /// Update function for the seeker enemy.
        /// </summary>
        /// <param name="gT"></param>
        public void Update(GameTime gT)
        {
            switch (m_state)
            {
                case EnemyStates.SPAWNING: UpdateSpawning(gT); break;
                case EnemyStates.SEEKING: UpdateSeeking(gT); break;
            }
        }

        /// <summary>
        /// Draw Function for the Misile Enemy.
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(m_seekerEnemyRotatingBladeTexture, m_position, new Rectangle(0, 0, 64, 64), Color.Lerp(Color.Transparent, Color.White, m_colorlerp), m_bladeRotation, m_origin, m_spriteScale + 0.015f, SpriteEffects.None, 0.0f);
            sb.Draw(m_seekerEnemyTexture, m_position, new Rectangle(0, 0, 64, 64), Color.Lerp(Color.Transparent, Color.White, m_colorlerp), m_rotationAngle, m_origin, m_spriteScale, SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Update for the Spawning state of the seeker enemy
        /// </summary>
        /// <param name="gT"></param>
        void UpdateSpawning(GameTime gT)
        {
            m_colorlerp += 0.02f;

            if (m_colorlerp >= 1.0f)
            {
                ChangeState(EnemyStates.SEEKING);
            }
        }

        /// <summary>
        /// Update for the Seeking of the seeker enemy
        /// </summary>
        /// <param name="gT"></param>
        void UpdateSeeking(GameTime gT)
        {
            if (!m_flocking)
                m_seekingPosition = m_pPlayer.GetPosition();

            if (m_rotationAngle >= Math.PI * 2)
                m_rotationAngle = 0;

            if (m_rotationAngle <= -(Math.PI * 2))
                m_rotationAngle = 0;

            if (m_slowMotion == true)
            {
                m_bladeRotation += 0.01f;
            }
            else
            {
                m_bladeRotation += 0.1f;
            }

            // Forces
            m_headingPosition = m_position + m_velocity;
            m_force = GetForce(m_seekingPosition);
            m_acceleration = m_force * (float)gT.ElapsedGameTime.TotalSeconds * 100;
            m_velocity += m_acceleration * (float)gT.ElapsedGameTime.TotalSeconds;
            if (m_velocity.X != 0 || m_velocity.Y != 0)
            {
                m_velocity.Normalize();
            }

            m_velocity *= m_maxVelocity;

            m_position += m_velocity * (float)gT.ElapsedGameTime.TotalSeconds * m_speed;

            m_rotationAngle = MathFunctions.AngleBetween360Degrees(m_position, m_headingPosition);
            m_facingDirection = new Vector2((float)Math.Sin(m_rotationAngle), -(float)Math.Cos(m_rotationAngle));
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
        /// Returns the force to move the missile enemy from it's current heading position (position + velocity)
        /// Towards the target position passed in.
        /// </summary>
        /// <param name="targetPosition">The position that you want the missile to move towards</param>
        /// <returns></returns>
        public Vector2 GetForce(Vector2 targetPosition)
        {
            Vector2 force;

            force = targetPosition - m_headingPosition;

            if (m_velocity.X != 0 || m_velocity.Y != 0)
            {
                force.Normalize();
            }

            return force * 500;
        }

    }
}
