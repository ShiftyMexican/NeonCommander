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
using TextureAtlas;
#endregion

namespace TestGameOne
{
    public class ConceptPlayer
    {
        Vector2 m_facingDirection;
        Vector2 m_shootingDirection;
        Vector2 m_position;
        Vector2 m_dimensions;
        Vector2 m_origin;
        Vector2 m_crosshairPosition;
        List<ConceptBullets> m_bulletList;
        Circle m_collisionRadius;
        float m_rotationAngle;
        float m_spriteScale;
        float m_shootingAngle;
        //float m_rotationSpeed;
        float m_shootCooldown;
        Texture2D t;

        public ConceptPlayer()
        {
            m_facingDirection = new Vector2(0, -1);
            m_shootingDirection = new Vector2(0, -1);
            m_position = new Vector2(600, 600);
            m_crosshairPosition = new Vector2(601, 601);
            m_rotationAngle = 0.0f;
            m_shootingAngle = 0.0f;
            m_spriteScale = 0.05f;
            //m_rotationSpeed = 0.1f;
            m_shootCooldown = 0.05f;
            m_dimensions = new Vector2(1024, 1024) * m_spriteScale;
            m_origin = (m_dimensions / 2.0f) / m_spriteScale;
            m_collisionRadius = new Circle(m_position, m_dimensions.X/2);

            t = new Texture2D(AIE.GameStateManager.Game.GraphicsDevice, 1, 1);
            t.SetData(new[] { Color.White });
            m_bulletList = new List<ConceptBullets>();
        }

        public List<ConceptBullets> GetBulletList()
        {
            return m_bulletList;
        }

        public void Update(GameTime gT)
        {
            m_shootCooldown -= (float)gT.ElapsedGameTime.TotalSeconds;

            #region GAMEPAD MOVEMENT
            if (GamePad.GetState(PlayerIndex.One).IsConnected)
            {
                Vector2 movementVector = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left;
                movementVector.Y *= -1;

                if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X != 0 || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y != 0)
                {
                    m_facingDirection = movementVector;
                
                }

                if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X != 0 || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y != 0)
                {
                    m_shootingDirection = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right;
                    m_shootingDirection.Y *= -1;
                    m_shootingDirection.Normalize();

                    m_shootingAngle = MathFunctions.AngleBetween360Degrees(m_position, m_position + m_shootingDirection);

                    if ( m_shootCooldown <= 0)
                    {
                        m_bulletList.Add(new ConceptBullets(m_position, m_shootingAngle, m_shootingDirection));
                        m_shootCooldown = 0.05f;
                    }
                }

                m_position += movementVector * (float)gT.ElapsedGameTime.TotalSeconds * 450;

                m_rotationAngle = MathFunctions.AngleBetween360Degrees(m_position, m_position + m_facingDirection);

            }
            #endregion

            #region KEYBOARD AND MOUSE MOVEMENT
            else
            {
                Vector2 newMovementVector = new Vector2(0, 0);

                if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    newMovementVector.Y -= 0.75f;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    newMovementVector.Y += 0.75f;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    newMovementVector.X -= 0.75f;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    newMovementVector.X += 0.75f;
                }

                if (newMovementVector.X != 0 || newMovementVector.Y != 0)
                {
                    m_facingDirection = newMovementVector;
                }

                if (Mouse.GetState().LeftButton == ButtonState.Pressed && m_shootCooldown <= 0)
                {
                    m_shootingDirection = m_crosshairPosition - m_position;
                    m_shootingDirection.Normalize();

                    m_shootingAngle = MathFunctions.AngleBetween360Degrees(m_position, m_position + m_shootingDirection);
                    

                    m_bulletList.Add(new ConceptBullets(m_position, m_shootingAngle, m_shootingDirection));
                    m_shootCooldown = 0.05f;

                }


                m_position += newMovementVector * (float)gT.ElapsedGameTime.TotalSeconds * 450;

                m_crosshairPosition = Globals.m_mousePosition;

                m_rotationAngle = MathFunctions.AngleBetween360Degrees(m_position, m_position + m_facingDirection);
            }
            #endregion
                      

            for (int iter = 0; iter < m_bulletList.Count(); iter++)
            {
                m_bulletList[iter].Update(gT);

                if (m_bulletList[iter].GetIfDead())
                {
                    m_bulletList.RemoveAt(iter);
                }
            }

            m_collisionRadius.m_center = m_position;
        }

        public Vector2 GetPosition()
        {
            return m_position;
        }

        public Circle GetCircle()
        {
            return m_collisionRadius;
        }

        public void ExplodeBullet(int bulletNumber)
        {
            m_bulletList[bulletNumber].Explode();
        }

        //public void RemoveBullet()
        //{
        //
        //}

        public void Draw(Texture2D playerSprite, Texture2D crosshairSprite, Texture2D bulletSprite, SpriteBatch sb)
        {
            
            //int bw = 2;

            //sb.Draw(t, new Rectangle((int)(m_collisionRadius.m_center.X + m_collisionRadius.m_radius), (int)(m_collisionRadius.m_center.Y - m_collisionRadius.m_radius), bw, 128), Color.Red); // Left
            //sb.Draw(t, new Rectangle((int)(m_collisionRadius.m_center.X - m_collisionRadius.m_radius), (int)(m_collisionRadius.m_center.Y - m_collisionRadius.m_radius), bw, 128), Color.Red); // Right
            //sb.Draw(t, new Rectangle((int)(m_collisionRadius.m_center.X - m_collisionRadius.m_radius), (int)(m_collisionRadius.m_center.Y + m_collisionRadius.m_radius), 128, bw), Color.Red); // Top
            //sb.Draw(t, new Rectangle((int)(m_collisionRadius.m_center.X - m_collisionRadius.m_radius), (int)(m_collisionRadius.m_center.Y - m_collisionRadius.m_radius), 128, bw), Color.Red);
            //sb.DrawString(Globals.m_defaultFont, "m_rotationAngle: " + m_rotationAngle.ToString(), new Vector2(10, 500), Color.White);
            //sb.DrawString(Globals.m_defaultFont, "Right Stick X: " + m_shootingDirection.X.ToString() + "\nRight Stick Y: " + m_shootingDirection.Y.ToString(), new Vector2(10, 100), Color.White);
            for (int iter = 0; iter < m_bulletList.Count(); iter++)
            {
                m_bulletList[iter].Draw(bulletSprite, sb);
            }

            sb.Draw(playerSprite, m_position, new Rectangle(0, 0, 1024, 1024), Color.White, m_rotationAngle, m_origin, m_spriteScale, SpriteEffects.None, 1.0f);

            if (!GamePad.GetState(PlayerIndex.One).IsConnected)
            {
                sb.Draw(crosshairSprite, m_crosshairPosition, new Rectangle(0, 0, 32, 32), Color.White, 0.0f, new Vector2(16, 16), 1.0f, SpriteEffects.None, 1);
            }
        }
    }

    public class ConceptBullets
    {
        Vector2 m_facingDirection;
        Vector2 m_position;
        Vector2 m_dimensions;
        Vector2 m_origin;
        Circle m_collisionRadius;
        float m_rotationAngle;
        float m_spriteScale;
        float m_speed;
        float m_deathCooldown;
        bool m_dying;
        bool m_dead;

        Texture2D m_line;
        Particles.ParticleSystem m_deathExplosion;

        Texture2D t;

        
        public ConceptBullets(Vector2 position, float rotation, Vector2 facingDir)
        {
            m_position = position;
            m_rotationAngle = rotation;
            m_facingDirection = facingDir;
            //m_facingDirection = new Vector2((float)Math.Sin(m_rotationAngle), -(float)Math.Cos(m_rotationAngle));
            m_spriteScale = 1f;
            m_dimensions = new Vector2(8, 8) * m_spriteScale;
            m_origin = (m_dimensions / 2.0f) / m_spriteScale;
            m_collisionRadius = new Circle(m_position, m_dimensions.X / 2);
            m_speed = 950.0f;
            m_dying = false;
            m_dead = false;

            #region PARTICLE SYSTEM
            m_line = AIE.GameStateManager.Game.Content.Load<Texture2D>("Images/line");
            m_deathExplosion = Particles.ParticleManager.CreateParticleSystem(m_line);

            m_deathExplosion.rate = 0.01f;
            m_deathExplosion.startColor = Color.White;
            m_deathExplosion.endColor = Color.Blue;
            m_deathExplosion.normColor = 0.5f;

            m_deathExplosion.spawnArea = 3f;
            m_deathExplosion.spawnAngle = 6.28f;

            m_deathExplosion.lifeTime = new Particles.Range(0.4f, 1.2f);
            m_deathCooldown = 2.0f;

            m_deathExplosion.startSpeed = new Particles.Range(10.0f, 100.0f);
            m_deathExplosion.endSpeed = new Particles.Range(0.0f, 0.0f);
            #endregion


            t = new Texture2D(AIE.GameStateManager.Game.GraphicsDevice, 1, 1);
            t.SetData(new[] { Color.White });
        }

        public void Update(GameTime gT)
        {
            if (!m_dying)
            {
                m_position += m_facingDirection * (float)gT.ElapsedGameTime.TotalSeconds * m_speed;

                m_collisionRadius.m_center = m_position;
            }

            else
            {
                m_deathExplosion.Update((float)gT.ElapsedGameTime.TotalSeconds);
                m_deathCooldown -= (float)gT.ElapsedGameTime.TotalSeconds;

                if (m_deathCooldown <= 0)
                {
                    m_dead = true;
                }
            }

            if (m_position.X < 0 || m_position.X > Globals.m_windowWidth || m_position.Y < 0 || m_position.Y > Globals.m_windowHeight)
            {
                Explode();
            }
        }

        public Vector2 GetPosition()
        {
            return m_position;
        }

        public Circle GetCircle()
        {
            return m_collisionRadius;
        }

        public bool GetIfDead()
        {
            return m_dead;
        }

        public bool GetIfDying()
        {
            return m_dying;
        }

        public void Explode()
        {
            if (!m_dying)
            {
                m_dying = true;
                m_deathExplosion.position = m_position;
                m_deathExplosion.Emit(200);
            }
        }

        public void Draw(Texture2D bulletSprite, SpriteBatch sb)
        {
            if( !m_dying )
            sb.Draw(bulletSprite, m_position, new Rectangle(0, 0, 8, 8), Color.Violet, m_rotationAngle, m_origin, m_spriteScale, SpriteEffects.None, 1.0f);
            //int bw = 2;

            //sb.Draw(t, new Rectangle((int)(m_collisionRadius.m_center.X + m_collisionRadius.m_radius), (int)(m_collisionRadius.m_center.Y - m_collisionRadius.m_radius), bw, 128), Color.Red); // Left
            //sb.Draw(t, new Rectangle((int)(m_collisionRadius.m_center.X - m_collisionRadius.m_radius), (int)(m_collisionRadius.m_center.Y - m_collisionRadius.m_radius), bw, 128), Color.Red); // Right
            //sb.Draw(t, new Rectangle((int)(m_collisionRadius.m_center.X - m_collisionRadius.m_radius), (int)(m_collisionRadius.m_center.Y + m_collisionRadius.m_radius), 128, bw), Color.Red); // Top
            //sb.Draw(t, new Rectangle((int)(m_collisionRadius.m_center.X - m_collisionRadius.m_radius), (int)(m_collisionRadius.m_center.Y - m_collisionRadius.m_radius), 128, bw), Color.Red);
        }
    }
        
    public class ConceptMissileEnemy
    {
        Vector2 m_facingDirection;
        Vector2 m_position;
        Vector2 m_dimensions;
        Vector2 m_origin;
        Vector2 m_seekPosition;
        Circle m_collisionRadius;
        Circle m_detectionRadius;
        float m_rotationAngle;
        float m_spriteScale;
        float m_speed;
        float m_deathCooldown;
        bool m_seeking;
        bool m_dying;
        bool m_dead;
        Texture2D t;
        Texture2D m_line;
        Particles.ParticleSystem m_deathExplosion;
        Random rng;
        
        public ConceptMissileEnemy(Vector2 position)
        {
            m_facingDirection = new Vector2();
            rng = Globals.m_rng;
            m_position = position;
            m_rotationAngle = 0.0f;
            m_spriteScale = 0.25f;
            m_dimensions = new Vector2(74, 70) * m_spriteScale;
            m_origin = (m_dimensions / 2.0f) / m_spriteScale;
            m_seekPosition = new Vector2(0, 0);
            m_seeking = false;
            m_speed = 150.0f;
            m_detectionRadius = new Circle(m_position, 128);
            m_collisionRadius = new Circle(m_position, m_dimensions.X / 2);
            m_dying = false;
            m_dead = false;

            #region PARTICLE SYSTEM
            m_line = AIE.GameStateManager.Game.Content.Load<Texture2D>("Images/line");
            m_deathExplosion = Particles.ParticleManager.CreateParticleSystem(m_line);
            
            m_deathExplosion.rate = 0.01f;
            m_deathExplosion.startColor = Color.White;
            m_deathExplosion.endColor = Color.Red;
            m_deathExplosion.normColor = 0.5f;

            m_deathExplosion.spawnArea = 3f;
            m_deathExplosion.spawnAngle = 6.28f;
                        
            m_deathExplosion.lifeTime = new Particles.Range(0.4f, 1.2f);
            m_deathCooldown = 2.0f;            

            m_deathExplosion.startSpeed = new Particles.Range(10.0f, 300.0f);
            m_deathExplosion.endSpeed = new Particles.Range(0.0f, 0.0f);
            #endregion

            t = new Texture2D(AIE.GameStateManager.Game.GraphicsDevice, 1, 1);
            t.SetData(new[] { Color.White });
        }

        public void Update(GameTime gT)
        {
            if (m_seeking)
            {
                m_position += m_facingDirection * (float)gT.ElapsedGameTime.TotalSeconds * m_speed;
            }

            else
            {
                m_rotationAngle += 0.02f;
                m_facingDirection = new Vector2((float)Math.Sin(m_rotationAngle), -(float)Math.Cos(m_rotationAngle));
                m_position += m_facingDirection * (float)gT.ElapsedGameTime.TotalSeconds * m_speed;
            }

            if (m_rotationAngle >= Math.PI * 2)
                m_rotationAngle = 0;

            if (m_rotationAngle <= -(Math.PI * 2))
                m_rotationAngle = 0;

            if (m_dying)
            {
                m_deathExplosion.Update((float)gT.ElapsedGameTime.TotalSeconds);
                m_deathCooldown -= (float)gT.ElapsedGameTime.TotalSeconds;

                if (m_deathCooldown <= 0)
                {
                    m_dead = true;
                }
            }

            m_detectionRadius.m_center = m_position;
            m_collisionRadius.m_center = m_position;
        }

        public Vector2 GetPosition()
        {
            return m_position;
        }

        public Circle GetCollisionCircle()
        {
            return m_collisionRadius;
        }

        public bool GetIfDying()
        {
            return m_dying;
        }

        public Circle GetDetectionCircle()
        {
            return m_detectionRadius;
        }

        public bool IsDead()
        {
            return m_dead;
        }

        /// <summary>
        /// Checks if the player/object is in the line of sight of the missile enemy.
        /// Line of sight is currently 180 degrees in front of the missile.
        /// </summary>
        /// <param name="position">The position of the object you want to check against</param>
        /// <returns></returns>
        public bool IsInLineOfSight(Vector2 position)
        {
            #region old code
            /*            
             * float angle = MathFunctions.AngleBetween360Degrees(m_position, position);
            float rotationInDegrees = (((m_rotationAngle / 2) * 180) / (float)Math.PI);
            float rightSideAngle = 90 + rotationInDegrees;
            float leftSideAngle = 270 + rotationInDegrees;
            int quadrant = 1;
                       
            // Quadrant 2
            if (angle > 90 && angle <= 180)
            {
                leftSideAngle = rotationInDegrees - 90;
                quadrant = 2;
            }

            //Quadrant 3
            if (angle > 180 && angle <= 270)
            {
                leftSideAngle = rotationInDegrees - 90;
                quadrant = 3;
            }

            //Quadrant 4
            if (angle > 270 && angle <= 360)
            {
                leftSideAngle = rotationInDegrees - 90;
                rightSideAngle = rotationInDegrees - 270;
                quadrant = 4;
            }
            
            //angle -= m_rotationAngle;
            angle = (float)(angle * 180 / Math.PI);

            if ((angle <= rightSideAngle && angle <= leftSideAngle) && !m_seeking && quadrant == 1)
            {
               angle = angle * (float)Math.PI / 180;
               m_rotationAngle = angle;
               m_facingDirection = new Vector2((float)Math.Sin(m_rotationAngle), -(float)Math.Cos(m_rotationAngle));
               m_seekPosition = position;
               m_seeking = true;
               m_speed = 400.0f;
               return true;
            }

            else if ((angle <= rightSideAngle && angle >= leftSideAngle) && !m_seeking)
            {
                angle = angle * (float)Math.PI / 180;
                m_rotationAngle = angle;
                m_facingDirection = new Vector2((float)Math.Sin(m_rotationAngle), -(float)Math.Cos(m_rotationAngle));
                m_seekPosition = position;
                m_seeking = true;
                m_speed = 400.0f;
                return true;
            }

            else if ((angle <= rightSideAngle || angle >= leftSideAngle) && !m_seeking && quadrant == 4)
            {
                angle = angle * (float)Math.PI / 180;
                m_rotationAngle = angle;
                m_facingDirection = new Vector2((float)Math.Sin(m_rotationAngle), -(float)Math.Cos(m_rotationAngle));
                m_seekPosition = position;
                m_seeking = true;
                m_speed = 400.0f;
                return true;
            }
             */
#endregion 
            float targetAngle = MathFunctions.AngleBetween360Degrees(m_position, position);;
            Vector2 targetDirection = new Vector2((float)Math.Sin(targetAngle), -(float)Math.Cos(targetAngle));
            float angle = Vector2.Dot(m_facingDirection, targetDirection);
            
            if (angle > 0.65 && !m_seeking)
            {
                m_rotationAngle = targetAngle;
                m_facingDirection = targetDirection;
                m_seekPosition = position;
                m_seeking = true;
                m_speed = 400.0f;
                return true;
            }


            else
            {
                return false;
            }
        }
        
        public void Draw(Texture2D enemySprite, SpriteBatch sb, int stringOffset)
        {
            if (!m_dying)
            {
                sb.Draw(enemySprite, m_position, new Rectangle(0, 0, 74, 70), Color.Red, m_rotationAngle, m_origin, m_spriteScale, SpriteEffects.None, 1.0f);
            }

            //sb.DrawString(Globals.m_defaultFont, "Direction X: " + m_facingDirection.X.ToString() + "\nDirection Y: " + m_facingDirection.Y.ToString(), new Vector2(10, 250), Color.White);
            //sb.DrawString(Globals.m_defaultFont, "m_rotationAngle: " + m_rotationAngle.ToString(), new Vector2(10, stringOffset), Color.White);

            //int bw = 2;

            //sb.Draw(t, new Rectangle((int)(m_collisionRadius.m_center.X + m_collisionRadius.m_radius), (int)(m_collisionRadius.m_center.Y - m_collisionRadius.m_radius), bw, 128), Color.Red); // Left
            //sb.Draw(t, new Rectangle((int)(m_collisionRadius.m_center.X - m_collisionRadius.m_radius), (int)(m_collisionRadius.m_center.Y - m_collisionRadius.m_radius), bw, 128), Color.Red); // Right
            //sb.Draw(t, new Rectangle((int)(m_collisionRadius.m_center.X - m_collisionRadius.m_radius), (int)(m_collisionRadius.m_center.Y + m_collisionRadius.m_radius), 128, bw), Color.Red); // Top
            //sb.Draw(t, new Rectangle((int)(m_collisionRadius.m_center.X - m_collisionRadius.m_radius), (int)(m_collisionRadius.m_center.Y - m_collisionRadius.m_radius), 128, bw), Color.Red);

        }

        public void Explode()
        {            
            if (!m_dying)
            {
                m_dying = true;
                m_deathExplosion.position = m_position;
                m_deathExplosion.Emit(200);
            }
        }
    }
    
    public class ConceptSeekerEnemy
    {
        Vector2 m_facingDirection;
        Vector2 m_position;
        Vector2 m_dimensions;
        Vector2 m_origin;
        Vector2 m_seekPosition;
        Circle m_collisionRadius;
        float m_rotationAngle;
        float m_spriteScale;
        float m_speed;
        bool m_seeking;
        Texture2D t;

        public ConceptSeekerEnemy(Vector2 position)
        {
            m_facingDirection = new Vector2();
            m_position = position;
            m_rotationAngle = 0.0f;
            m_spriteScale = 0.25f;
            m_dimensions = new Vector2(210, 180) * m_spriteScale;
            m_origin = (m_dimensions / 2.0f) / m_spriteScale;
            m_seekPosition = new Vector2(0, 0);
            m_seeking = false;
            m_speed = 250.0f;
            m_collisionRadius = new Circle(m_position, 256);

            t = new Texture2D(AIE.GameStateManager.Game.GraphicsDevice, 1, 1);
            t.SetData(new[] { Color.White });
        }

        public void Update(GameTime gT)
        {
            if (m_seeking)
            {
                m_rotationAngle = MathFunctions.AngleBetween360Degrees(m_position, m_seekPosition);

                m_facingDirection = new Vector2((float)Math.Sin(m_rotationAngle), -(float)Math.Cos(m_rotationAngle));
                m_position += m_facingDirection * (float)gT.ElapsedGameTime.TotalSeconds * m_speed;
            }

            else
            {

            }

            m_collisionRadius.m_center = m_position;
        }

        public Vector2 GetPosition()
        {
            return m_position;
        }

        public void SetSeekPosition(Vector2 newPosition)
        {
            m_seekPosition = newPosition;
        }

        public Circle GetCircle()
        {
            return m_collisionRadius;
        }

        public void StartSeeking(Vector2 playerPosition)
        {
            if (!m_seeking)
            {
                m_seekPosition = playerPosition;
                m_seeking = true;
                m_rotationAngle = MathFunctions.AngleBetween360Degrees(m_position, m_seekPosition);

                m_facingDirection = new Vector2((float)Math.Sin(m_rotationAngle), -(float)Math.Cos(m_rotationAngle));
            }
        }

        public bool GetSeeking()
        {
            return m_seeking;
        }

        public void Draw(Texture2D playerSprite, SpriteBatch sb)
        {
            sb.Draw(playerSprite, m_position, new Rectangle(0, 0, 210, 180), Color.Blue, m_rotationAngle, m_origin, m_spriteScale, SpriteEffects.None, 1.0f);

            //int bw = 2;
            
            //sb.Draw(t, new Rectangle((int)(m_collisionRadius.m_center.X + m_collisionRadius.m_radius), (int)(m_collisionRadius.m_center.Y - m_collisionRadius.m_radius), bw, 128), Color.Red); // Left
            //sb.Draw(t, new Rectangle((int)(m_collisionRadius.m_center.X - m_collisionRadius.m_radius), (int)(m_collisionRadius.m_center.Y - m_collisionRadius.m_radius), bw, 128), Color.Red); // Right
            //sb.Draw(t, new Rectangle((int)(m_collisionRadius.m_center.X - m_collisionRadius.m_radius), (int)(m_collisionRadius.m_center.Y + m_collisionRadius.m_radius), 128, bw), Color.Red); // Top
            //sb.Draw(t, new Rectangle((int)(m_collisionRadius.m_center.X - m_collisionRadius.m_radius), (int)(m_collisionRadius.m_center.Y - m_collisionRadius.m_radius), 128, bw), Color.Red);

        }
    }
        
    public class Concept : AIE.IGameState
    {
        Texture2D m_backgroundSprite;
        Texture2D m_playerSprite;
        Texture2D m_crosshairSprite;
        Texture2D m_lineSprite;
        Texture2D m_enemySprite;
        ConceptPlayer m_playerOne;
        List<ConceptMissileEnemy> m_missleEnemyList;
        List<ConceptSeekerEnemy> m_seekerEnemyList;
        Particles.ParticleSystem ps;

        public Concept()         : base()
        {            
            ps = Particles.ParticleManager.CreateParticleSystem(Particles.ParticleManager.defaultParticle);
            m_backgroundSprite = content.Load<Texture2D>("Images/levelbackground");
            m_playerSprite = content.Load<Texture2D>("Images/playergreen");
            m_enemySprite = content.Load<Texture2D>("Images/littleship");
            m_crosshairSprite = content.Load<Texture2D>("Images/crosshair");
            m_lineSprite = content.Load<Texture2D>("Images/line");

            m_playerOne = new ConceptPlayer();

            m_missleEnemyList = new List<ConceptMissileEnemy>();
            m_missleEnemyList.Add(new ConceptMissileEnemy(new Vector2(100, 200)));
            m_missleEnemyList.Add(new ConceptMissileEnemy(new Vector2(700, 250)));
            m_missleEnemyList.Add(new ConceptMissileEnemy(new Vector2(300, 450)));

            m_seekerEnemyList = new List<ConceptSeekerEnemy>();
            m_seekerEnemyList.Add(new ConceptSeekerEnemy(new Vector2(110, 250)));
            m_seekerEnemyList.Add(new ConceptSeekerEnemy(new Vector2(210, 150)));
            m_seekerEnemyList.Add(new ConceptSeekerEnemy(new Vector2(710, 400)));
            m_seekerEnemyList.Add(new ConceptSeekerEnemy(new Vector2(510, 350)));

        }

        public override void Update(GameTime gT)
        {
            m_playerOne.Update(gT);

            for (int iter = 0; iter < m_missleEnemyList.Count(); iter++)
            {                
                if (m_missleEnemyList[iter].IsDead())
                {
                    m_missleEnemyList.RemoveAt(iter);
                    m_missleEnemyList.Add(new ConceptMissileEnemy(new Vector2(Globals.m_rng.Next(150, 800), Globals.m_rng.Next(100, 450))));
                    m_missleEnemyList.Add(new ConceptMissileEnemy(new Vector2(Globals.m_rng.Next(150, 800), Globals.m_rng.Next(100, 450))));
                }

                else m_missleEnemyList[iter].Update(gT);
            }

            for (int iter = 0; iter < m_seekerEnemyList.Count(); iter++)
            {
                if (m_seekerEnemyList[iter].GetSeeking())
                {
                    m_seekerEnemyList[iter].SetSeekPosition(m_playerOne.GetPosition());
                }

                m_seekerEnemyList[iter].Update(gT);

                if (m_playerOne.GetCircle().Intersects(m_seekerEnemyList[iter].GetCircle()))
                {
                    m_seekerEnemyList[iter].StartSeeking(m_playerOne.GetPosition());
                }
            }

            #region COLLISIONS
            for (int iter = 0; iter < m_missleEnemyList.Count(); iter++)
            {
                if (m_playerOne.GetCircle().Intersects(m_missleEnemyList[iter].GetDetectionCircle()))
                {
                    m_missleEnemyList[iter].IsInLineOfSight(m_playerOne.GetPosition());
                }

                List<ConceptBullets> bulletList = m_playerOne.GetBulletList();

                for (int bulletIter = 0; bulletIter < bulletList.Count(); bulletIter++)
                {
                    if (bulletList[bulletIter].GetCircle().Intersects(m_missleEnemyList[iter].GetCollisionCircle()) && !bulletList[bulletIter].GetIfDying() && !m_missleEnemyList[iter].GetIfDying())
                    {
                        m_missleEnemyList[iter].Explode();
                        m_playerOne.ExplodeBullet(bulletIter);
                    }
                }
                
                //move to update
                if (m_missleEnemyList[iter].GetPosition().X >= Globals.m_windowWidth || m_missleEnemyList[iter].GetPosition().X <= 0 || m_missleEnemyList[iter].GetPosition().Y >= Globals.m_windowHeight || m_missleEnemyList[iter].GetPosition().Y <= 0)
                {
                    m_missleEnemyList[iter].Explode();
                }
            }            

            

            #endregion

            #region KEYBOARDINPUT
            if (Keyboard.GetState().IsKeyDown(Keys.J))
                AIE.GameStateManager.SetFreezeDraw(true);

            if (Keyboard.GetState().IsKeyDown(Keys.L))
                AIE.GameStateManager.SetFreezeDraw(false);


            if (Keyboard.GetState().IsKeyDown(Keys.I))
            {
                AIE.GameStateManager.SetFreezeUpdate(true);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.K))
            {
                AIE.GameStateManager.SetFreezeUpdate(false);
            }
            #endregion
        }

        public override void Draw(GameTime gT, SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(m_backgroundSprite, new Rectangle(0, 0, Globals.m_windowWidth, Globals.m_windowHeight), Color.White);
            m_playerOne.Draw(m_playerSprite, m_crosshairSprite, m_lineSprite, sb);
            int offset = 10;

            for (int iter = 0; iter < m_missleEnemyList.Count(); iter++)
            {
                m_missleEnemyList[iter].Draw(m_enemySprite, sb, offset);
                offset += 50;
            }

            for (int iter = 0; iter < m_seekerEnemyList.Count(); iter++)
            {
                m_seekerEnemyList[iter].Draw(m_playerSprite, sb);
            }

            sb.End();
        }
    }
}
