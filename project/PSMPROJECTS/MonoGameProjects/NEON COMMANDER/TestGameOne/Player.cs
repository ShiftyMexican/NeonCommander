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

    public class Player
    {
        #region PLAYER VARIABLES
        private Texture2D m_playerSprite;
        private Texture2D m_playerGlowSprite;
        private Texture2D m_crosshairSprite;

        private SoundEffect m_shootingSound;

        private List<SoundEffectInstance> m_shootingSoundInstanceList;
        
        Vector2 m_facingDirection;
        Vector2 m_shootingDirection;
        Vector2 m_position;
        Vector2 m_crosshairPosition;        
        Vector2 m_velocity;

        private Vector2 m_dimensions;
        private Vector2 m_origin;

        List<Bullet> m_bulletList;
        private bool m_loadedOptions;
        OptionsObject m_options; 
        Color m_playerColour;

        private Rectangle m_playerSource;
        public Circle m_collisionCircle;

        private float m_rotationAngle;
        private float m_shootingAngle;
        public float m_shootCooldown;
        private float m_textureScale;
        private float m_decelerationTime;

        public bool m_powerupMultifire;

        public float m_speed;
        private float m_shootingVolume;
        private float m_shootingSoundCooldown;
        public float m_powerupMultifireCD { get; set; }

        #endregion

        /// <summary>
        /// Default Constructor for the player class.
        /// </summary>
        public Player()
            : base()
        {
            m_playerSprite = AIE.GameStateManager.Game.Content.Load<Texture2D>("Images/playerNoGlow");
            m_playerGlowSprite = AIE.GameStateManager.Game.Content.Load<Texture2D>("Images/playerGlow");
            m_crosshairSprite = AIE.GameStateManager.Game.Content.Load<Texture2D>("Images/RoundCrosshair");
            
            m_shootingSound = AIE.GameStateManager.Game.Content.Load<SoundEffect>("Sounds/Laser007");
            m_shootingSoundInstanceList = new List<SoundEffectInstance>();

            m_options = new OptionsObject();

            m_shootingVolume = 0.05f;

            m_playerColour = Color.White;

            for (int i = 0; i < 15; i++)
            {
                m_shootingSoundInstanceList.Add(m_shootingSound.CreateInstance());
                m_shootingSoundInstanceList[i].Volume = m_shootingVolume * Globals.m_volume;
            }

            m_shootingSoundCooldown = 0.1f;

            m_facingDirection = new Vector2(0, -1);
            m_shootingDirection = new Vector2(0, -1);
            m_position = new Vector2(630, 630);
            m_crosshairPosition = Globals.m_mousePosition;
            m_textureScale = 0.35f;            
            m_dimensions = new Vector2(64, 64) * m_textureScale;
            m_origin = new Vector2(m_dimensions.X / 2.0f, m_dimensions.Y / 2.0f) / m_textureScale;
            m_playerSource = new Rectangle(0, 0, (int)(m_dimensions.X / m_textureScale), (int)(m_dimensions.Y / m_textureScale));
            m_speed = 300.0f;

            m_bulletList = new List<Bullet>();
            m_loadedOptions = false;

            m_collisionCircle = new Circle(m_position, m_origin.X * m_textureScale);

            m_rotationAngle = 0.0f;
            m_shootingAngle = 0.0f;
            m_shootCooldown = 0.1f;
            m_decelerationTime = 0.0f;

            m_powerupMultifire = false;
            m_powerupMultifireCD = 0.0f;

        }

        /// <summary>
        /// Update function for the Player class.
        /// </summary>
        /// <param name="gT">GameTime, used to calculate Delta Time</param>
        public void Update(GameTime gT)
        {
            m_shootCooldown -= (float)gT.ElapsedGameTime.TotalSeconds;
            m_shootingSoundCooldown -= (float)gT.ElapsedGameTime.TotalSeconds;
            m_powerupMultifireCD = m_powerupMultifireCD - (float)gT.ElapsedGameTime.TotalSeconds;

            if (!m_loadedOptions)
            {
                m_options.LoadOptions();
                m_playerColour = new Color(m_options.m_red, m_options.m_green, m_options.m_blue);
                m_loadedOptions = true;
            }

            
            m_crosshairPosition.X = Globals.m_mousePosition.X + m_position.X - Globals.m_windowWidth / 2;
            m_crosshairPosition.Y = Globals.m_mousePosition.Y + m_position.Y - Globals.m_windowHeight / 2;
            
            
            #region GAMEPAD 
            if (GamePad.GetState(PlayerIndex.One).IsConnected)
            {
                Vector2 movementVector = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left;
                movementVector.Y *= -1;

                //Checks if the left thumbstick is being moved in any direction.
                //If it isn't then the direction the player is facing will not change.
                if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X != 0 || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y != 0)
                {

                    if (m_position.X < Globals.m_gameWidth - 20 && m_position.X > 20 && m_position.Y < Globals.m_gameHeight - 20 && m_position.Y > 20)
                    {
                        m_facingDirection = movementVector;

                        m_velocity = movementVector * (float)gT.ElapsedGameTime.TotalSeconds * m_speed;
                        m_position += m_velocity;

                        m_decelerationTime = 0f;
                    }
                    else
                    {
                        m_position -= m_velocity;
                    }
                }

                //If the thumbstick is not being moved then this will occur.
                else
                {

                    if (m_position.X < Globals.m_gameWidth - 20 && m_position.X > 20 && m_position.Y < Globals.m_gameHeight - 20 && m_position.Y > 20)
                    {
                        if (m_decelerationTime < 1)
                        {
                            m_decelerationTime += (float)gT.ElapsedGameTime.TotalSeconds / 10;

                            if (m_decelerationTime >= 1)
                            {
                                m_decelerationTime = 1;
                            }
                        }


                        m_velocity = Vector2.Lerp(m_velocity, new Vector2(0, 0), m_decelerationTime);
                        m_position += m_velocity;
                    }
                }

                if (m_powerupMultifire == false)
                {
                    if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X != 0 || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y != 0)
                    {
                        m_shootingDirection = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right;
                        m_shootingDirection.Y *= -1;
                        m_shootingDirection.Normalize();

                        m_shootingAngle = MathFunctions.AngleBetween360Degrees(m_position, m_position + m_shootingDirection);

                        if (m_shootCooldown <= 0)
                        {
                            m_bulletList.Add(new Bullet(m_position, m_shootingAngle, m_shootingDirection));
                            m_shootCooldown = 0.1f;

                            if (m_shootingSoundCooldown <= 0)
                            {
                                for (int i = 0; i < m_shootingSoundInstanceList.Count(); i++)
                                {
                                    if (m_shootingSoundInstanceList[i].State == SoundState.Stopped)
                                    {
                                        m_shootingSoundInstanceList[i].Volume = m_shootingVolume * Globals.m_volume;
                                        m_shootingSoundInstanceList[i].Play();
                                        break;
                                    }
                                }

                                m_shootingSoundCooldown = 0.1f;
                            }
                        }
                    }
                }
                else if (m_powerupMultifire == true)
                {
                    if (m_powerupMultifireCD > 0.0f)
                    {
                        if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X != 0 || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y != 0)
                        {
                            if (m_shootCooldown <= 0)
                            {
                                for (float i = 0; i < Math.PI * 2; i += (float)Math.PI / 6)
                                {
                                    Vector2 dir = new Vector2((float)Math.Sin(i), -(float)Math.Cos(i));
                                    m_bulletList.Add(new Bullet(m_position, i, dir));
                                }

                                m_shootCooldown = 0.1f;
                                
                                if (m_shootingSoundCooldown <= 0)
                                {
                                    for (int i = 0; i < m_shootingSoundInstanceList.Count(); i++)
                                    {
                                        if (m_shootingSoundInstanceList[i].State == SoundState.Stopped)
                                        {
                                            m_shootingSoundInstanceList[i].Volume = m_shootingVolume * Globals.m_volume;
                                            m_shootingSoundInstanceList[i].Play();
                                            break;
                                        }
                                    }

                                    m_shootingSoundCooldown = 0.1f;
                                }
                            }
                        }
                    }
                    else if (m_powerupMultifireCD <= 0)
                    {
                        m_powerupMultifire = false;
                    }

                }

                m_rotationAngle = MathFunctions.AngleBetween360Degrees(m_position, m_position + m_facingDirection);

                
            }
#endregion
                
            #region MOUSE SHOOTING
            else
            {           
                if (m_powerupMultifire == false)
                {
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed && m_shootCooldown <= 0)
                    {
                        Vector2 shootingDir = m_crosshairPosition - m_position;
                        shootingDir.Normalize();

                        m_shootingAngle = MathFunctions.AngleBetween360Degrees(m_position, m_position + shootingDir);
                        
                        if (m_shootCooldown <= 0)
                        {
                            m_bulletList.Add(new Bullet(m_position, m_shootingAngle, shootingDir));
                            m_shootCooldown = 0.1f;

                            if (m_shootingSoundCooldown <= 0)
                            {
                                for (int i = 0; i < m_shootingSoundInstanceList.Count(); i++)
                                {
                                    if (m_shootingSoundInstanceList[i].State == SoundState.Stopped)
                                    {
                                        m_shootingSoundInstanceList[i].Volume = m_shootingVolume * Globals.m_volume;
                                        m_shootingSoundInstanceList[i].Play();
                                        break;
                                    }
                                }

                                m_shootingSoundCooldown = 0.1f;
                            }
                        }
                    }
                }

                else
                {
                    if (m_powerupMultifireCD > 0.0f)
                    {
                        if (Mouse.GetState().LeftButton == ButtonState.Pressed && m_shootCooldown <= 0)
                        {
                            if (m_shootCooldown <= 0)
                            {
                                for (float i = 0; i < Math.PI * 2; i += (float)Math.PI / 6)
                                {
                                    Vector2 dir = new Vector2((float)Math.Sin(i), -(float)Math.Cos(i));
                                    m_bulletList.Add(new Bullet(m_position, i, dir));
                                }

                                m_shootCooldown = 0.1f;
                                
                                if (m_shootingSoundCooldown <= 0)
                                {
                                    for (int i = 0; i < m_shootingSoundInstanceList.Count(); i++)
                                    {
                                        if (m_shootingSoundInstanceList[i].State == SoundState.Stopped)
                                        {
                                            m_shootingSoundInstanceList[i].Volume = m_shootingVolume * Globals.m_volume;
                                            m_shootingSoundInstanceList[i].Play();
                                            break;
                                        }
                                    }

                                    m_shootingSoundCooldown = 0.1f;
                                }
                            }
                        }
                    }

                    else if (m_powerupMultifireCD <= 0)
                    {
                        m_powerupMultifire = false;
                    }

                }
            }
            #endregion

            for (int iter = 0; iter < m_bulletList.Count(); iter++)
            {
                m_bulletList[iter].Update(gT);
                m_bulletList[iter].m_speed = this.m_speed * 2;

                if (m_bulletList[iter].GetPosition().X < 0 || m_bulletList[iter].GetPosition().Y < 0 || m_bulletList[iter].GetPosition().X > 1260 || m_bulletList[iter].GetPosition().Y > 1260)
                {
                    m_bulletList.RemoveAt(iter);
                }
            }

            #region KEYBOARD MOVEMENT
            if( !GamePad.GetState(PlayerIndex.One).IsConnected)
            {
                if (m_position.X < Globals.m_gameWidth - 20 && m_position.X > 20 && m_position.Y < Globals.m_gameHeight - 20 && m_position.Y > 20)
                {

                    Vector2 newMovementVector = new Vector2(0, 0);
                    newMovementVector.Y *= -1;

                    if (Keyboard.GetState().IsKeyDown(Keys.W))
                    {
                        newMovementVector.Y -= 1.0f;

                        if (Keyboard.GetState().IsKeyDown(Keys.A))
                        {
                            newMovementVector.X -= 1.0f;
                        }
                        else if (Keyboard.GetState().IsKeyDown(Keys.D))
                        {
                            newMovementVector.X += 1.0f;
                        }
                        
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.S))
                    {
                        newMovementVector.Y += 1.0f;

                        if (Keyboard.GetState().IsKeyDown(Keys.A))
                        {
                            newMovementVector.X -= 1.0f;
                        }
                        else if (Keyboard.GetState().IsKeyDown(Keys.D))
                        {
                            newMovementVector.X += 1.0f;
                        }

                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.A))
                    {
                        newMovementVector.X -= 1.0f;
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.D))
                    {
                        newMovementVector.X += 1.0f;
                    }
               


                    if (newMovementVector.X != 0 || newMovementVector.Y != 0)
                    {
                        m_facingDirection = newMovementVector;
                        m_velocity = newMovementVector * (float)gT.ElapsedGameTime.TotalSeconds * m_speed;
                        m_decelerationTime = 0f;
                        m_position += m_velocity;

                    }
                    else
                    {

                        if (m_position.X < Globals.m_gameWidth - 20 && m_position.X > 20 && m_position.Y < Globals.m_gameHeight - 20 && m_position.Y > 20)
                        {
                            if (m_decelerationTime < 1)
                            {
                                m_decelerationTime += (float)gT.ElapsedGameTime.TotalSeconds / 10;

                                if (m_decelerationTime >= 1)
                                {
                                    m_decelerationTime = 1;
                                }
                            }


                            m_velocity = Vector2.Lerp(m_velocity, new Vector2(0, 0), m_decelerationTime);
                            m_position += m_velocity;
                        }
                    }
                    
                    m_rotationAngle = MathFunctions.AngleBetween360Degrees(m_position, m_position + m_facingDirection);                    
                }

                else
                {
                    m_position -= m_velocity;
                }
            }
            #endregion

            m_collisionCircle.m_center = m_position;    
        }

        /// <summary>
        /// Returns the bullet list.
        /// </summary>
        /// <returns></returns>
        public List<Bullet> GetBulletList()
        {
            return m_bulletList;
        }

        /// <summary>
        /// Returns the players circle. This circle is used for collisions.
        /// </summary>
        /// <returns></returns>
        public Circle GetCollisionCircle()
        {
            return m_collisionCircle;
        }

        public Vector2 GetPosition()
        {
            return m_position;
        }

        public float GetRotation()
        {
            return m_rotationAngle;
        }

        /// <summary>
        /// Draw Function for the Player class.
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(Texture2D bulletSprite, SpriteBatch sb)
        {
            sb.Draw(m_playerGlowSprite, m_position, m_playerSource, m_playerColour, m_rotationAngle, m_origin, m_textureScale, SpriteEffects.None, 0.0f);                      
            sb.Draw(m_playerSprite, m_position, m_playerSource, Color.White, m_rotationAngle, m_origin, m_textureScale, SpriteEffects.None, 0.0f);

            if (!GamePad.GetState(PlayerIndex.One).IsConnected)
            {
                sb.Draw(m_crosshairSprite, m_crosshairPosition, new Rectangle(0, 0, 32, 32), Color.White, 0.0f, new Vector2(16, 16), 0.6f, SpriteEffects.None, 0.0f);
            }

            for (int iter = 0; iter < m_bulletList.Count(); iter++)
            {
                m_bulletList[iter].Draw(bulletSprite, sb);
            }
        }
    }
        

}
