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
    public class MineEnemy
    {

        public enum EnemyStates
        {
            SPAWNING,
            ATTACKING,
        }

        #region MISSILE_ENEMY_VARIABLES
        public Vector2 m_position;
        private Vector2 m_headingPosition;
        private Vector2 m_wanderPosition;
        private Vector2 m_velocity;
        private Vector2 m_force;
        private Vector2 m_acceleration;
        private Player m_pPlayer;
        private Vector2 m_dimensions;
        private Vector2 m_origin;

        List<Mine> m_mineList;

        Circle m_collisionRadius;
        Circle m_detectionRadius;
        public float m_rotationAngle;
        private float m_maxVelocity;
        private float m_directionChange;
        public float m_addMineCountdown;
        public float m_spriteScale;
        private float m_lerpColour;

        //Too high turning speed leads to jittery movement, too low leads to very large, round turns
        private float m_turningSpeed;

        private Texture2D m_mineEnemyTexture;
        private Texture2D m_mineTexture;

        public bool m_slowMotion { get; set; }

        public EnemyStates m_state = EnemyStates.SPAWNING;

        #endregion

        /// <summary>
        /// Constructor for the Missle Enemy.
        /// </summary>
        /// <param name="position"></param>
        public MineEnemy(Vector2 position, Player playerOne, Texture2D mineSpawnerTexture, Texture2D mineTexture)
        {
            m_mineEnemyTexture = mineSpawnerTexture;
            m_mineTexture = mineTexture;
            m_pPlayer = playerOne;
            m_position = position;
            m_spriteScale = 0.35f;
            m_dimensions = new Vector2(128, 128) * m_spriteScale;
            m_origin = (m_dimensions / 2.0f) / m_spriteScale;

            m_maxVelocity = 0.0f;
            m_turningSpeed = 150.0f;
            m_wanderPosition = new Vector2();

            m_detectionRadius = new Circle(m_position, 100);
            m_collisionRadius = new Circle(m_position, m_dimensions.X / 2 * m_spriteScale);
            
            m_directionChange = 2.0f;
            m_addMineCountdown = 2.0f;

            m_lerpColour = 0.0f;

            m_mineList = new List<Mine>();

            m_slowMotion = false;
        }

        /// <summary>
        /// Update function for the missile enemy.
        /// </summary>
        /// <param name="gT"></param>
        public void Update(GameTime gT)
        {
            m_directionChange -= (float)gT.ElapsedGameTime.TotalSeconds;
            m_addMineCountdown -= (float)gT.ElapsedGameTime.TotalSeconds;

            if (m_slowMotion == true)
            {
                m_maxVelocity = 20.0f;
                Flee(gT, m_pPlayer.GetPosition());
            }
            else
            {
                m_maxVelocity = 100.0f;
            }

            if (m_slowMotion == false)
            {
                switch (m_state)
                {
                    case EnemyStates.SPAWNING: UpdateSpawning(gT); break;
                    case EnemyStates.ATTACKING: UpdateAttacking(gT); break;
                }
            }  
        }

        /// <summary>
        /// Draw Function for the Missile Enemy.
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < m_mineList.Count; i++)
            {
                m_mineList[i].Draw(sb);
            }

            sb.Draw(m_mineEnemyTexture, m_position, new Rectangle(0, 0, (int)(m_dimensions.X / m_spriteScale), (int)(m_dimensions.Y / m_spriteScale)), Color.Lerp(Color.Transparent, Color.White, m_lerpColour), m_rotationAngle, m_origin, m_spriteScale, SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Update for the Spawning state of the Mine enemy
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
        /// Update for the Attacking state of the Min enemy
        /// </summary>
        /// <param name="gT"></param>
        void UpdateAttacking(GameTime gT)
        {
            if (Vector2.Distance(m_position, m_pPlayer.GetPosition()) < 150f)
            {
                Flee(gT, m_pPlayer.GetPosition());
            }

            else
            {
                if (m_directionChange <= 0 || (m_wanderPosition - m_position).Length() < 16.0f)
                {
                    m_wanderPosition = new Vector2(Globals.m_rng.Next(0, Globals.m_gameWidth), Globals.m_rng.Next(0, Globals.m_gameHeight));
                }

                Wander(gT, m_wanderPosition);
            }

            for (int i = 0; i < m_mineList.Count; i++)
            {
                if (m_mineList[i].GetExploded())
                {
                    m_mineList.RemoveAt(i);
                }

                m_mineList[i].Update(gT);
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

        public List<Mine> GetMineList()
        {
            return m_mineList;
        }

        /// <summary>
        /// A basic flee function using forces. 
        /// </summary>
        /// <param name="gT">GameTime, used instead of delta time</param>
        /// <param name="fleePosition">Where the entity should flee from</param>
        public void Flee(GameTime gT, Vector2 fleePosition)
        {
            m_headingPosition = m_position + m_velocity;
            m_force = GetForce(fleePosition) * -1f;
            m_acceleration = m_force * (float)gT.ElapsedGameTime.TotalSeconds * m_turningSpeed;
            m_velocity += m_acceleration * (float)gT.ElapsedGameTime.TotalSeconds;
            if (m_velocity.X != 0 || m_velocity.Y != 0)
            {
                m_velocity.Normalize();
            }

            m_velocity *= m_maxVelocity;

            m_position += m_velocity * (float)gT.ElapsedGameTime.TotalSeconds;

            m_rotationAngle = MathFunctions.AngleBetween360Degrees(m_position, m_headingPosition);
        }

        /// <summary>
        /// A basic wander function using forces. 
        /// </summary>
        /// <param name="gT">GameTime, used instead of delta time</param>
        /// <param name="seekingPosition">Where the entity should wander to</param>
        public void Wander(GameTime gT, Vector2 seekingPosition)
        {
            m_headingPosition = m_position + m_velocity;
            m_force = GetForce(seekingPosition);
            m_acceleration = m_force * (float)gT.ElapsedGameTime.TotalSeconds * m_turningSpeed;
            m_velocity += m_acceleration * (float)gT.ElapsedGameTime.TotalSeconds;
            if (m_velocity.X != 0 || m_velocity.Y != 0)
            {
                m_velocity.Normalize();
            }

            m_velocity *= m_maxVelocity;

            m_position += m_velocity * (float)gT.ElapsedGameTime.TotalSeconds;

            m_rotationAngle = MathFunctions.AngleBetween360Degrees(m_position, m_headingPosition);
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

    public class Mine
    {
        Vector2 m_position;
        Texture2D m_mineTexture;
        float m_lifeTime;
        float m_explosionRadius;
        float m_spriteScale;
        float m_rotation;
        public bool m_explode;
        //float m_rotation;
        //float m_spriteScale;
        Circle m_explosionCircle;

        public Mine(Vector2 position, float rot, float scale, Texture2D mineTexture)
        {
            m_position = position;
            m_rotation = rot;
            m_spriteScale = scale;
            m_mineTexture = mineTexture;
            m_lifeTime = 5.0f;
            m_explosionRadius = 20.0f;
            m_explosionCircle = new Circle(m_position, m_explosionRadius);
            m_explode = false;
        }

        public void Update(GameTime gT)
        {
            m_lifeTime -= (float)gT.ElapsedGameTime.TotalSeconds;
            if (m_lifeTime <= 0)
            {
                m_explode = true;
            }
        }

        /// <summary>
        /// Returns the m_explode variable. Used to explode and kill enemies/player.
        /// </summary>
        /// <returns></returns>
        public bool GetExploded()
        {
            return m_explode;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(m_mineTexture, m_position, new Rectangle(0, 0, 40, 40), Color.White, m_rotation, new Vector2( 20, 20), m_spriteScale, SpriteEffects.None, 0.0f);
        }

        public Circle GetCollisionCircle()
        {
            return m_explosionCircle;
        }

        public Vector2 GetPosition()
        {
            return m_position;
        }

    }
}
