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
    public class MultiShooterEnemy
    {

        public enum EnemyStates
        {
            SPAWNING,
            SPINNING,
            SHOOTING,
            WAITING,
        }


        #region MISSILE_ENEMY_VARIABLES

        Vector2 m_facingDirection;
        public Vector2 m_position;
        //Vector2 m_velocity;
        private Vector2 m_dimensions;
        private Vector2 m_origin;

        public Circle m_collisionRadius;
        private Circle m_detectionRadius;

        private float m_rotationAngle;
        private float m_bulletRotationAngle;
        private float m_enemyDirChange;
        private float m_spriteScale;

        public EnemyStates m_state = EnemyStates.SPAWNING;
        private float m_spinningTime;
        private float m_shootTime;
        private float m_waitTime;
        private float m_stateTime;
        public float m_shootCoolDown;

        public bool m_slowMotion { get; set; }

        private float m_colorlerp;
        
        private Texture2D m_multiShooterEnemyTexture;
        //private Texture2D m_bulletTexture;
        #endregion

        /// <summary>
        /// Constructor for the Multi-shooter Enemy.
        /// </summary>
        /// <param name="position"></param>
        public MultiShooterEnemy(Vector2 position, Texture2D multiShooterEnemyTexture, Texture2D bulletTexture)
        {
            m_multiShooterEnemyTexture = multiShooterEnemyTexture;
            m_facingDirection = new Vector2(0, -1);
            m_position = position;
            m_spriteScale = 0.35f;
            m_dimensions = new Vector2(64, 64) * m_spriteScale;
            m_origin = (m_dimensions / 2.0f) / m_spriteScale;

            m_detectionRadius = new Circle(m_position, 50);
            m_collisionRadius = new Circle(m_position, m_dimensions.X / 2);

            m_rotationAngle = 0.0f;
            m_bulletRotationAngle = 0.0f;
            m_enemyDirChange = 5.0f;

            m_waitTime = 1.0f;
            m_spinningTime = 1.0f;
            m_shootTime = 3.0f;
            m_shootCoolDown = 1.5f;

            m_colorlerp = 0.0f;

            m_slowMotion = false;

        }

        /// <summary>
        /// Update function for the multi-shooter enemy.
        /// </summary>
        /// <param name="gT"></param>
        public void Update(GameTime gT)
        {
            m_enemyDirChange -= (float)gT.ElapsedGameTime.TotalSeconds;
            m_stateTime += (float)gT.ElapsedGameTime.TotalSeconds;

            m_bulletRotationAngle = m_rotationAngle;

            switch (m_state)
            {
                case EnemyStates.SPAWNING: UpdateSpawningState(gT); break;
                case EnemyStates.WAITING: UpdateWaitingState(gT); break;
                case EnemyStates.SPINNING: UpdateSpinningState(gT); break;
                case EnemyStates.SHOOTING: UpdateShootingState(gT); break;
                default: /* not in a valid state */ break;
            }

            if (m_slowMotion == true)
            {
                ChangeState(EnemyStates.WAITING);
            }

            m_detectionRadius.m_center = m_position;
            m_collisionRadius.m_center = m_position;
        }

        /// <summary>
        /// Draw Function for the Misile Enemy.
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(m_multiShooterEnemyTexture, m_position, new Rectangle(0, 0, 64, 64), Color.Lerp(Color.Transparent, Color.White, m_colorlerp), m_rotationAngle, m_origin, m_spriteScale, SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Update for the spawning state of the multi shooter enemy.
        /// </summary>
        /// <param name="gT"></param>
        void UpdateSpawningState(GameTime gT)
        {
            m_colorlerp += 0.02f;

            if (m_colorlerp >= 1.0f)
            {
                ChangeState(EnemyStates.SPINNING);
            }
        }

        /// <summary>
        /// Update for the waiting state of the MultiShooter enemy
        /// </summary>
        /// <param name="gT"></param>
        void UpdateWaitingState(GameTime gT)
        {
            m_facingDirection = new Vector2((float)Math.Sin(m_rotationAngle), -(float)Math.Cos(m_rotationAngle));

            if (m_slowMotion == false)
            {
                if (m_stateTime > m_waitTime)
                {
                    ChangeState(EnemyStates.SHOOTING);
                    m_shootCoolDown = 1.0f;
                }
            }
        }

        /// <summary>
        /// Update for the spinning state of the MultiShooter enemy 
        /// </summary>
        /// <param name="gT"></param>
        void UpdateSpinningState(GameTime gT)
        {
            m_rotationAngle += 100 * (float)gT.ElapsedGameTime.TotalSeconds;

            if (m_stateTime > m_spinningTime)
            {
                ChangeState(EnemyStates.WAITING);
            }
        }

        /// <summary>
        /// Update for the shooting state of the MultiShooter enemy
        /// </summary>
        /// <param name="gT"></param>
        void UpdateShootingState(GameTime gT)
        {
            m_shootCoolDown -= (float)gT.ElapsedGameTime.TotalSeconds;

            if (m_stateTime > m_shootTime)
            {
                ChangeState(EnemyStates.SPINNING);
            }
        }

        /// <summary>
        /// Function for the switching of states.
        /// </summary>
        /// <param name="state"></param>
        void ChangeState(EnemyStates state)
        {
            m_stateTime = 0;
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
    }
}
