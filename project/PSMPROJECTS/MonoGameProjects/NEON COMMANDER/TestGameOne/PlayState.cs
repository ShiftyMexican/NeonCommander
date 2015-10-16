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
    public class PlayState :    AIE.IGameState
    {

        private Player m_player1;
        private Shield m_shield;

        public List<Bullet> m_bulletList { get; set; }
        public List<Mine> m_mineList { get; set; }

        private SoundEffect m_slowMotionEffection;
        private SoundEffect m_enemyExplosion;
        private List<SoundEffectInstance> m_enemyExplosionList;

        #region TEXTURES
        private Texture2D m_playerBulletTexture;
        private Texture2D m_levelBackgroundTexture;
        private Texture2D m_missleEnemyTexture;
        private Texture2D m_seekerEnemyTexture;
        private Texture2D m_seekerEnemyRotatingBladeTexture;
        private Texture2D m_mineEnemyTexture;
        private Texture2D m_mineTexture;
        private Texture2D m_multiDirectionalEnemyTexture;
        private Texture2D m_enemyBullet;
        private Texture2D m_red3SpriteSheet;
        private Texture2D m_blue3SpriteSheet;
        private Texture2D m_green3SpriteSheet;
        private Texture2D m_yellow0SpriteSheet;
        private Texture2D m_bombPowerUpTexture;
        private Texture2D m_bottomBoundary;
        private Texture2D m_topBoundary;
        private Texture2D m_leftBoundary;
        private Texture2D m_rightBoundary;
        private Texture2D m_whiteBoundary;
        private Texture2D m_topHUD;
        private Texture2D m_multiplierNotLit;
        private Texture2D m_multiplierLit;
        private Texture2D m_whiteBorder;
        private Texture2D m_bombFlash;
        #endregion

        public int m_score;
        private Vector2 m_scorePosition;
        public int m_multiplier;
        private int m_subMultiplier;
        private bool m_gameOver;

        private float m_lerpBombTimer;
        private float m_missileEnemyCD;
        private float m_multiEnemyCD;
        private float m_powerupTimer;
        private float m_powerupSpwan;
        private float m_shieldTime;
        private float m_borderLerp;
        private bool m_borderRed;
        private bool m_shieldActive;
        public bool m_powerupUsed { get; set; }
        private int m_timeSpeedUp;
        private int m_fireRateIncreased;
        private int m_enemySpawnX;
        private int m_enemySpawnY;
        private int m_seekerSpawnX;
        private int m_seekerSpawnY;
        private int m_multiSpawnX;
        private int m_multiSpawnY;
        private int m_mineSpawnX;
        private int m_mineSpawnY;
        private float m_lerpTimer;
        private Vector2 m_timerPosition;
        private bool m_peakHit;
        private float m_bombTimer;
        private bool m_multifireDraw;
        private bool m_bombDraw;
        private bool m_speedUpDraw;
        private bool m_firerateUpDraw;
        private float m_slowMotionTimer;
        private bool m_bombExplosion;
        private bool m_peakBombHit;

        private float m_multiplierDrop;
        private float m_multiplierDropTimer;

        private bool m_gameBegin;
        private float m_fadeInHUD;
        private float m_gameBeginTimer;

        #region LISTS
        public List<MissleEnemy> m_missileEnemyList { get; set; }
        public List<SeekerEnemy> m_seekerEnemyList { get; set; }
        public List<MultiShooterEnemy> m_multiShooterList { get; set; }
        public List<MineEnemy> m_mineEnemyList { get; set; }
        private List<PowerUp> m_powerupList;
        private List<TextureAtlas.AnimatedSprite> m_redAnimation;
        private List<TextureAtlas.AnimatedSprite> m_greenAnimation;
        private List<TextureAtlas.AnimatedSprite> m_blueAnimation;
        private List<TextureAtlas.AnimatedSprite> m_yellowAnimation;
        #endregion

        /// <summary>
        /// Constructor for the playState
        /// </summary>
        public PlayState() : base()
        {
            m_player1 = new Player();
            m_gameOver = false;

            Globals.m_gameCamera.Zoom = 0.35f;
            m_score = 0;
            m_multiplier = 1;
            m_subMultiplier = 0;
            m_scorePosition = new Vector2(0, 0);

            m_slowMotionEffection = content.Load<SoundEffect>("Sounds/SlowMotion");
            m_enemyExplosion = content.Load<SoundEffect>("Sounds/EnemyDeath");
            m_enemyExplosionList = new List<SoundEffectInstance>();

            for (int i = 0; i < 10; i++)
            {
                m_enemyExplosionList.Add(m_enemyExplosion.CreateInstance());
            }


            m_borderLerp = 0.0f;
            m_borderRed = false;

            m_lerpTimer = 0.0f;
            m_bombTimer = 0.0f;
            m_timerPosition = new Vector2(0, 0);
            m_peakHit = false;
            m_multifireDraw = false;
            m_bombDraw = false;
            m_speedUpDraw = false;
            m_firerateUpDraw = false;
            m_bombExplosion = false;
            m_peakBombHit = false;

            m_slowMotionTimer = 0.0f;
            m_multiplierDrop = 20.0f;
            m_multiplierDropTimer = 1.0f;

            m_enemySpawnX = 0;
            m_enemySpawnY = 0;
            m_seekerSpawnX = 0;
            m_seekerSpawnY = 0;
            m_multiSpawnX = 0;
            m_multiSpawnY = 0;
            m_mineSpawnX = 0;
            m_mineSpawnY = 0;

            m_bulletList = new List<Bullet>();
            m_mineList = new List<Mine>();

            m_gameBegin = false;
            m_gameBeginTimer = 5.0f;
            m_fadeInHUD = 0.0f;

            m_lerpBombTimer = 0.0f;
            

            #region LOADING_CONTENT

            m_levelBackgroundTexture = content.Load<Texture2D>("Images/levelbackground");
            m_bottomBoundary = content.Load<Texture2D>("Images/Border");
            m_topBoundary = content.Load<Texture2D>("Images/TopBorder");
            m_leftBoundary = content.Load<Texture2D>("Images/LeftBorder");
            m_rightBoundary = content.Load<Texture2D>("Images/RightBorder");
            m_whiteBoundary = content.Load<Texture2D>("Images/WhiteRectangleBoundry");
            m_playerBulletTexture = content.Load<Texture2D>("Images/PlayerBullet");
            m_missleEnemyTexture = content.Load<Texture2D>("Images/MissileEnemy");
            m_seekerEnemyTexture = content.Load<Texture2D>("Images/SeekerEnemy");
            m_seekerEnemyRotatingBladeTexture = content.Load<Texture2D>("Images/SeekerEnemyRotatingBlades");
            m_mineEnemyTexture = content.Load<Texture2D>("Images/MineEnemy");
            m_mineTexture = content.Load<Texture2D>("Images/Mine");
            m_topHUD = content.Load<Texture2D>("Images/TopHUD");
            m_whiteBorder = content.Load<Texture2D>("Images/WhiteRectangleBoundry");

            m_multiDirectionalEnemyTexture = content.Load<Texture2D>("Images/DirectionalShooter");
            m_enemyBullet = content.Load<Texture2D>("Images/EnemyBullet");

            m_red3SpriteSheet = content.Load<Texture2D>("Images/red3");
            m_green3SpriteSheet = content.Load<Texture2D>("Images/green3");
            m_blue3SpriteSheet = content.Load<Texture2D>("Images/blue3");
            m_yellow0SpriteSheet = content.Load<Texture2D>("Images/yellow0");

            m_bombPowerUpTexture = content.Load<Texture2D>("Images/EnemyBullet");
            m_multiplierNotLit = content.Load<Texture2D>("Images/MultiplierNotLit");
            m_multiplierLit = content.Load<Texture2D>("Images/MultiplierLit");
            m_bombFlash = content.Load<Texture2D>("Images/BombFlash");

            #endregion

            #region ANIMATION_LISTS

            m_redAnimation = new List<TextureAtlas.AnimatedSprite>();
            m_greenAnimation = new List<TextureAtlas.AnimatedSprite>();
            m_blueAnimation = new List<TextureAtlas.AnimatedSprite>();
            m_yellowAnimation = new List<TextureAtlas.AnimatedSprite>();

            for (int i = 0; i < 10; i++)
            {
                // PLEASE READ
                // ALL EXPLOSIONS CURRENTLY USE THE RED EXPLOSION SPRITESHEET
                // TO CHANGE THIS, LOAD IN A DIFFERENT SPRITESHEET AND REPLACE "m_red3SpriteSheet" WITH THAT 2D TEXTURE

                m_redAnimation.Add(new TextureAtlas.AnimatedSprite(m_red3SpriteSheet, 5, 16, 80, 2, 1.5f));
                m_greenAnimation.Add(new TextureAtlas.AnimatedSprite(m_green3SpriteSheet, 5, 16, 80, 2, 1.5f));
                m_blueAnimation.Add(new TextureAtlas.AnimatedSprite(m_blue3SpriteSheet, 5, 16, 80, 2, 1.5f));
                m_yellowAnimation.Add(new TextureAtlas.AnimatedSprite(m_yellow0SpriteSheet, 5, 16, 80, 2, 1.5f));

                m_redAnimation[i].m_finished = true;
                m_greenAnimation[i].m_finished = true;
                m_blueAnimation[i].m_finished = true;
                m_yellowAnimation[i].m_finished = true;
            }

            #endregion

            #region ENEMY_RELATED

            m_missileEnemyList = new List<MissleEnemy>();
            m_seekerEnemyList = new List<SeekerEnemy>();
            m_multiShooterList = new List<MultiShooterEnemy>();
            m_mineEnemyList = new List<MineEnemy>();

            m_missileEnemyCD = 2.0f;
            m_multiEnemyCD = 4.0f;

            #endregion

            #region POWERUP_RELATED

            m_powerupList = new List<PowerUp>();

            m_powerupTimer = 16.0f;
            m_powerupSpwan = 10.0f;
            m_shieldTime = 0.0f;
            m_timeSpeedUp = 0;
            m_fireRateIncreased = 0;
            m_powerupUsed = false;
            m_shieldActive = false;

            #endregion

        }

        /// <summary>
        /// Update for the PlayState.
        /// </summary>
        /// <param name="gT"></param>
        public override void Update(GameTime gT)
        {
            m_missileEnemyCD -= (float)gT.ElapsedGameTime.TotalSeconds;
            m_powerupSpwan -= (float)gT.ElapsedGameTime.TotalSeconds;
            m_shieldTime -= (float)gT.ElapsedGameTime.TotalSeconds;
            m_bombTimer -= (float)gT.ElapsedGameTime.TotalSeconds;
            m_gameBeginTimer -= (float)gT.ElapsedGameTime.TotalSeconds;
            m_slowMotionTimer -= (float)gT.ElapsedGameTime.TotalSeconds;
            m_multiplierDrop -= (float)gT.ElapsedGameTime.TotalSeconds;
            m_multiplierDropTimer -= (float)gT.ElapsedGameTime.TotalSeconds;
            m_multiEnemyCD -= (float)gT.ElapsedGameTime.TotalSeconds;

            if (m_peakBombHit == true && m_lerpBombTimer > 0.0f)
            {
                m_lerpBombTimer -= (float)gT.ElapsedGameTime.TotalSeconds * 0.3f;
            }
            else if (m_lerpBombTimer <= 0)
            {
                m_peakBombHit = false;
            }

            m_player1.Update(gT);
            m_scorePosition = new Vector2(m_player1.GetPosition().X, m_player1.GetPosition().Y - 250);
            m_timerPosition = new Vector2(m_player1.GetPosition().X, m_player1.GetPosition().Y + 200);

            BorderColourLerp();

            if (InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.Start) || InputManager.InputManager.IsKeyJustPressed(Keys.Space))
            {
                AIE.GameStateManager.SetFreezeUpdate(true);
                AIE.GameStateManager.SetState("PAUSE", new PauseState());
                AIE.GameStateManager.PushState("PAUSE");
            }

            if (Globals.m_gameCamera.Zoom < 1.0f)
            {
                Globals.m_gameCamera.Zoom += 0.005f;
            }

            if (Globals.m_gameCamera.Zoom >= 1.0f)
            {
                m_fadeInHUD += 0.01f;
            }

            if (m_bombTimer > 0.0f)
            {

                if (m_peakHit == false)
                    m_lerpTimer += 0.005f;


                if (m_lerpTimer >= 0.7f)
                    m_peakHit = true;

                if (m_peakHit == true)
                    m_lerpTimer -= 0.01f;
            }
            else if (m_bombTimer <= 0.0f)
            {
                m_lerpTimer = 0.0f;
                m_peakHit = false;
                m_multifireDraw = false;
                m_bombDraw = false;
                m_speedUpDraw = false;
                m_firerateUpDraw = false;
            }

            if (m_gameBeginTimer <= 0)
            {
                m_gameBegin = true;
            }


            if (m_gameBegin == true)
            {
                #region ENEMY_UPDATES

                for (int iter = 0; iter < m_missileEnemyList.Count; iter++)
                {
                    m_missileEnemyList[iter].Update(gT);
                }

                for (int iter = 0; iter < m_multiShooterList.Count; iter++)
                {
                    m_multiShooterList[iter].Update(gT);

                    if (m_multiShooterList[iter].m_shootCoolDown < 0)
                    {

                        for (float i = 0; i < Math.PI * 2; i += (float)Math.PI / 4)
                        {
                            Vector2 dir = new Vector2((float)Math.Sin(i), -(float)Math.Cos(i));
                            m_bulletList.Add(new Bullet(m_multiShooterList[iter].m_position, i, dir));
                        }


                        m_multiShooterList[iter].m_shootCoolDown = 3.0f;
                    }
                }

                for (int iter = 0; iter < m_bulletList.Count(); iter++)
                {
                    m_bulletList[iter].Update(gT);

                    if (m_bulletList[iter].GetPosition().X < 0 || m_bulletList[iter].GetPosition().Y < 0 || m_bulletList[iter].GetPosition().X > 1260 || m_bulletList[iter].GetPosition().Y > 1260)
                    {
                        m_bulletList.RemoveAt(iter);
                    }

                }

                for (int iter = 0; iter < m_mineEnemyList.Count; iter++)
                {
                    m_mineEnemyList[iter].Update(gT);
                    if (m_mineEnemyList[iter].m_addMineCountdown <= 0)
                    {
                        m_mineList.Add(new Mine(m_mineEnemyList[iter].m_position, m_mineEnemyList[iter].m_rotationAngle, m_mineEnemyList[iter].m_spriteScale, m_mineTexture));
                        m_mineEnemyList[iter].m_addMineCountdown = 2.0f;
                    }
                }

                for (int iter = 0; iter < m_mineList.Count; iter++)
                {
                    m_mineList[iter].Update(gT);
                    if (m_mineList[iter].GetExploded() == true)
                    {
                        m_mineList.RemoveAt(iter);
                    }
                }

                // Attempt at getting flocking to work
                foreach (SeekerEnemy seeker in m_seekerEnemyList)
                {
                    seeker.Update(gT);
                    foreach (SeekerEnemy otherSeeker in m_seekerEnemyList)
                    {
                        if (seeker.GetCollisionCircle().Intersects(otherSeeker.GetCollisionCircle()))
                        {
                            seeker.m_seekingPosition = new Vector2(0, 0);
                        }
                    }
                }
                #endregion

                #region ANIMATION UPDATES
                for (int iter = 0; iter < m_redAnimation.Count; iter++)
                {
                    m_redAnimation[iter].Update();
                }

                for (int iter = 0; iter < m_greenAnimation.Count; iter++)
                {
                    m_greenAnimation[iter].Update();
                }

                for (int iter = 0; iter < m_blueAnimation.Count; iter++)
                {
                    m_blueAnimation[iter].Update();
                }

                for (int iter = 0; iter < m_yellowAnimation.Count; iter++)
                {
                    m_yellowAnimation[iter].Update();
                }
                #endregion

                #region POWERUPS_UPDATES

                if (m_powerupSpwan <= 0)
                {
                    m_powerupList.Add(new PowerUp(new Vector2(Globals.m_rng.Next(50, 1200), Globals.m_rng.Next(50, 1200)), m_bombPowerUpTexture, Globals.m_rng.Next(1, 6), this, m_player1));

                    m_powerupTimer = 10.0f;
                    m_powerupSpwan = 10.0f;
                }

                m_powerupTimer -= (float)gT.ElapsedGameTime.TotalSeconds;
                


                if (m_powerupTimer > 0.0f)
                {
                    for (int iter = 0; iter < m_powerupList.Count; iter++)
                    {
                        m_powerupList[iter].Update(gT);

                        if (m_player1.GetCollisionCircle().Intersects(m_powerupList[iter].GetCollisionCircle()))
                        {
                            m_powerupList[iter].UsePowerup();

                            if (m_powerupList[iter].m_powerupID == 1)
                            {
                                m_slowMotionTimer = 5.0f;
                                SlowMotion(gT);
                                m_bombTimer = 5.0f;
                                m_bombDraw = true;
                                m_powerupList.Clear();
                                m_bombExplosion = true;
                                m_score += 500 * m_multiplier;

                            }
                            else if (m_powerupList[iter].m_powerupID == 2)
                            {
                                m_shield = new Shield(m_player1.GetPosition(), m_powerupList[iter], m_player1);
                                m_powerupList.Clear();
                                m_bombTimer = 5.0f;
                                m_shieldTime = 10.0f;
                            }
                            else if (m_powerupList[iter].m_powerupID == 3)
                            {
                                m_player1.m_powerupMultifire = true;
                                m_player1.m_powerupMultifireCD = 10.0f;
                                m_bombTimer = 5.0f;
                                m_multifireDraw = true;

                                m_powerupList.Clear();
                            }
                            else if (m_powerupList[iter].m_powerupID == 4)
                            {
                                if (m_timeSpeedUp < 5)
                                {
                                    m_player1.m_speed = m_player1.m_speed * 1.1f;
                                    m_powerupList.Clear();
                                    m_bombTimer = 5.0f;
                                    m_timeSpeedUp += 1;
                                    m_speedUpDraw = true;
                                }
                                else
                                    m_powerupList[iter].m_powerupID = Globals.m_rng.Next(1, 4);
                            }
                            else if (m_powerupList[iter].m_powerupID == 5)
                            {
                                if (m_fireRateIncreased < 5)
                                {
                                    m_player1.m_shootCooldown = m_player1.m_shootCooldown * 0.9f;
                                    m_powerupList.Clear();
                                    m_bombTimer = 5.0f;
                                    m_fireRateIncreased += 1;
                                    m_firerateUpDraw = true;
                                }
                                else
                                    m_powerupList[iter].m_powerupID = Globals.m_rng.Next(1, 5);
                            }
                        }                        
                    }
                }

                if (m_slowMotionTimer <= 3.0f && m_bombExplosion == true)
                {

                    if (m_peakBombHit == false)
                        m_lerpBombTimer += 1f;

                    if (m_lerpBombTimer >= 0.95f)
                        m_peakBombHit = true;

                    m_slowMotionEffection.Play();
                    KillAllEnemies();
                    m_bombExplosion = false;
                }

                else if (m_powerupTimer <= 0.0f)
                {
                    m_powerupList.Clear();
                }

                if (m_shieldTime > 0)
                {
                    m_shield.Update(gT);
                    m_shieldActive = true;
                }
                else if (m_shieldTime <= 0)
                {
                    m_shieldActive = false;
                    m_shield = null;
                }

                #endregion

                if (m_slowMotionTimer <= 0)
                {
                    #region ENEMY_SPAWNING

                    if (m_missileEnemyCD <= 0)
                    {
                        // Missile Enemy Spawn
                        m_enemySpawnX = Globals.m_rng.Next((int)m_player1.GetPosition().X - 400, (int)m_player1.GetPosition().X + 400);
                        m_enemySpawnY = Globals.m_rng.Next((int)m_player1.GetPosition().Y - 400, (int)m_player1.GetPosition().Y + 400);

                        if (m_enemySpawnX < m_player1.GetCollisionCircle().m_radius + 200 || m_enemySpawnY < m_player1.GetCollisionCircle().m_radius + 200)
                        {
                            m_enemySpawnX = Globals.m_rng.Next((int)m_player1.GetPosition().X - 400, (int)m_player1.GetPosition().X + 400);
                            m_enemySpawnY = Globals.m_rng.Next((int)m_player1.GetPosition().Y - 400, (int)m_player1.GetPosition().Y + 400);
                        }
                        else if (m_enemySpawnX > m_player1.GetCollisionCircle().m_radius + 200 || m_enemySpawnY > m_player1.GetCollisionCircle().m_radius + 200)
                        {
                            m_missileEnemyList.Add(new MissleEnemy(new Vector2(m_enemySpawnX, m_enemySpawnY), m_missleEnemyTexture));
                        }

                        m_missileEnemyCD = 1f;

                        // Seeker Enemy Spawn
                        m_seekerSpawnX = Globals.m_rng.Next((int)m_player1.GetPosition().X - 400, (int)m_player1.GetPosition().X + 400);
                        m_seekerSpawnY = Globals.m_rng.Next((int)m_player1.GetPosition().Y - 400, (int)m_player1.GetPosition().Y + 400);

                        if (m_seekerSpawnX < m_player1.GetCollisionCircle().m_radius + 200 || m_seekerSpawnY < m_player1.GetCollisionCircle().m_radius + 200)
                        {
                            m_seekerSpawnX = Globals.m_rng.Next((int)m_player1.GetPosition().X - 400, (int)m_player1.GetPosition().X + 400);
                            m_seekerSpawnY = Globals.m_rng.Next((int)m_player1.GetPosition().Y - 400, (int)m_player1.GetPosition().Y + 400);

                        }
                        else if (m_seekerSpawnX > m_player1.GetCollisionCircle().m_radius + 200 || m_seekerSpawnY > m_player1.GetCollisionCircle().m_radius + 200)
                        {
                            m_seekerEnemyList.Add(new SeekerEnemy(new Vector2(m_seekerSpawnX, m_seekerSpawnY), m_seekerEnemyTexture, m_seekerEnemyRotatingBladeTexture, m_player1));
                        }


                        if (m_multiEnemyCD <= 0.0f)
                        {
                            // Multi Shooter Spawn 
                            m_multiSpawnX = Globals.m_rng.Next(0, 1260);
                            m_multiSpawnY = Globals.m_rng.Next(0, 1260);

                            if (m_multiSpawnX < m_player1.GetCollisionCircle().m_radius + 200 || m_multiSpawnY < m_player1.GetCollisionCircle().m_radius + 200)
                            {
                                m_multiSpawnX = Globals.m_rng.Next((int)m_player1.GetPosition().X - 400, (int)m_player1.GetPosition().X + 400);
                                m_multiSpawnY = Globals.m_rng.Next((int)m_player1.GetPosition().Y - 400, (int)m_player1.GetPosition().Y + 400);
                            }
                            else if (m_multiSpawnX > m_player1.GetCollisionCircle().m_radius + 200 || m_multiSpawnY > m_player1.GetCollisionCircle().m_radius + 200)
                            {
                                m_multiShooterList.Add(new MultiShooterEnemy(new Vector2(m_multiSpawnX, m_multiSpawnY), m_multiDirectionalEnemyTexture, m_enemyBullet));
                            }

                            m_multiEnemyCD = 4.0f;
                        }
                   

                        // Mine Enemy Spawn
                        m_mineSpawnX = Globals.m_rng.Next((int)m_player1.GetPosition().X - 400, (int)m_player1.GetPosition().X + 400);
                        m_mineSpawnY = Globals.m_rng.Next((int)m_player1.GetPosition().Y - 400, (int)m_player1.GetPosition().Y + 400);

                        if (m_mineSpawnX < m_player1.GetCollisionCircle().m_radius + 200 || m_mineSpawnY < m_player1.GetCollisionCircle().m_radius + 200)
                        {
                            m_mineSpawnX = Globals.m_rng.Next((int)m_player1.GetPosition().X - 400, (int)m_player1.GetPosition().X + 400);
                            m_mineSpawnY = Globals.m_rng.Next((int)m_player1.GetPosition().Y - 400, (int)m_player1.GetPosition().Y + 400);
                        }
                        else if (m_mineSpawnX > m_player1.GetCollisionCircle().m_radius + 200 || m_mineSpawnY > m_player1.GetCollisionCircle().m_radius + 200)
                        {
                            m_mineEnemyList.Add(new MineEnemy(new Vector2(m_mineSpawnX, m_mineSpawnY), m_player1, m_mineEnemyTexture, m_mineTexture));
                        }
                       
                    }

                    #endregion
                }

                #region MISSILE ENEMY COLLISION
                //LOOPS THROUGH ALL MISSILE ENEMIES
                for (int missileIter = 0; missileIter < m_missileEnemyList.Count; missileIter++)
                {
                    if (m_missileEnemyList[missileIter].m_state == MissleEnemy.EnemyStates.ATTACKING)
                    {
                        if (m_shieldActive == true)
                        {
                            // Checks to see if an enemy hit the shield
                            if (m_missileEnemyList[missileIter].GetCollisionCircle().Intersects(m_shield.GetCollisionCircle()))
                            {
                                //LOOPS THROUGH ANIMATION LIST TO FIND A FREE ANIMATION
                                for (int animationIter = 0; animationIter < m_redAnimation.Count; animationIter++)
                                {
                                    if (m_redAnimation[animationIter].m_finished)
                                    {
                                        m_redAnimation[animationIter].m_finished = false;
                                        m_redAnimation[animationIter].m_position = m_missileEnemyList[missileIter].GetPosition();
                                        break;
                                    }
                                }

                                //LOOPS THROUGH EXPLOSION SOUND LIST TO FIND A FREE SOUND
                                for (int i = 0; i < 10; i++)
                                {
                                    if (m_enemyExplosionList[i].State == SoundState.Stopped)
                                    {
                                        m_enemyExplosionList[i].Volume = 0.1f * Globals.m_volume;
                                        m_enemyExplosionList[i].Play();
                                        break;
                                    }
                                }

                                m_missileEnemyList.RemoveAt(missileIter);
                                m_score += 100 * (int)m_multiplier;
                                m_subMultiplier += 1;
                                m_multiplierDrop = 5.0f;
                                break;
                            }
                        }

                        //CHECKS IF THEY COLLIDE WITH THE PLAYER
                        if (m_player1.m_collisionCircle.Intersects(m_missileEnemyList[missileIter].GetCollisionCircle()))
                        {
                            m_gameOver = true;
                        }


                        //CHECKS IF THEY SEE THE PLAYER
                        if (m_player1.GetCollisionCircle().Intersects(m_missileEnemyList[missileIter].GetDetectionCircle()))
                        {
                            m_missileEnemyList[missileIter].IsInLineOfSight(m_player1.GetPosition());
                        }

                        //LOOPS THROUGH ALL PLAYERS BULLETS
                        for (int bulletIter = 0; bulletIter < m_player1.GetBulletList().Count; bulletIter++)
                        {
                            //CHECKS IF BULLET HITS THE ENEMY
                            if (m_player1.GetBulletList()[bulletIter].GetCircle().Intersects(m_missileEnemyList[missileIter].GetCollisionCircle()))
                            {
                                //LOOPS THROUGH ANIMATION LIST TO FIND A FREE ANIMATION
                                for (int animationIter = 0; animationIter < m_redAnimation.Count; animationIter++)
                                {
                                    if (m_redAnimation[animationIter].m_finished)
                                    {
                                        m_redAnimation[animationIter].m_finished = false;
                                        m_redAnimation[animationIter].m_position = m_missileEnemyList[missileIter].GetPosition();
                                        break;
                                    }
                                }

                                //LOOPS THROUGH EXPLOSION SOUND LIST TO FIND A FREE SOUND
                                for (int i = 0; i < 10; i++)
                                {
                                    if (m_enemyExplosionList[i].State == SoundState.Stopped)
                                    {
                                        m_enemyExplosionList[i].Volume = 0.1f * Globals.m_volume;
                                        m_enemyExplosionList[i].Play();
                                        break;
                                    }
                                }

                                m_missileEnemyList.RemoveAt(missileIter);
                                m_player1.GetBulletList().RemoveAt(bulletIter);
                                m_score += 100 * (int)m_multiplier;
                                m_subMultiplier += 1;
                                m_multiplierDrop = 5.0f;
                                break;
                            }
                        }
                    }
                }
                #endregion

                #region SEEKER ENEMY COLLISION
                //LOOPS THROUGH ALL MISSILE ENEMIES
                for (int seekerIter = 0; seekerIter < m_seekerEnemyList.Count; seekerIter++)
                {
                    if (m_seekerEnemyList[seekerIter].m_state != SeekerEnemy.EnemyStates.SPAWNING)
                    {
                        if (m_shieldActive == true)
                        {
                            // Checks to see if an enemy hit the shield
                            if (m_seekerEnemyList[seekerIter].GetCollisionCircle().Intersects(m_shield.GetCollisionCircle()))
                            {
                                //LOOPS THROUGH ANIMATION LIST TO FIND A FREE ANIMATION
                                for (int animationIter = 0; animationIter < m_blueAnimation.Count; animationIter++)
                                {
                                    if (m_blueAnimation[animationIter].m_finished)
                                    {
                                        m_blueAnimation[animationIter].m_finished = false;
                                        m_blueAnimation[animationIter].m_position = m_seekerEnemyList[seekerIter].GetPosition();
                                        break;
                                    }
                                }

                                //LOOPS THROUGH EXPLOSION SOUND LIST TO FIND A FREE SOUND
                                for (int i = 0; i < 10; i++)
                                {
                                    if (m_enemyExplosionList[i].State == SoundState.Stopped)
                                    {
                                        m_enemyExplosionList[i].Volume = 0.1f * Globals.m_volume;
                                        m_enemyExplosionList[i].Play();
                                        break;
                                    }
                                }

                                m_seekerEnemyList.RemoveAt(seekerIter);
                                m_score += 150 * (int)m_multiplier;
                                m_subMultiplier += 1;
                                m_multiplierDrop = 5.0f;
                                break;
                            }
                        }

                        //CHECKS IF THEY COLLIDE WITH THE PLAYER
                        else if (m_player1.m_collisionCircle.Intersects(m_seekerEnemyList[seekerIter].GetCollisionCircle()))
                        {
                            m_gameOver = true;
                        }

                        //LOOPS THROUGH ALL PLAYERS BULLETS
                        for (int bulletIter = 0; bulletIter < m_player1.GetBulletList().Count; bulletIter++)
                        {
                            //CHECKS IF BULLET HITS THE ENEMY
                            if (m_player1.GetBulletList()[bulletIter].GetCircle().Intersects(m_seekerEnemyList[seekerIter].GetCollisionCircle()))
                            {
                                //LOOPS THROUGH ANIMATION LIST TO FIND A FREE ANIMATION
                                for (int animationIter = 0; animationIter < m_blueAnimation.Count; animationIter++)
                                {
                                    if (m_blueAnimation[animationIter].m_finished)
                                    {
                                        m_blueAnimation[animationIter].m_finished = false;
                                        m_blueAnimation[animationIter].m_position = m_seekerEnemyList[seekerIter].GetPosition();
                                        break;
                                    }
                                }

                                //LOOPS THROUGH EXPLOSION SOUND LIST TO FIND A FREE SOUND
                                for (int i = 0; i < 10; i++)
                                {
                                    if (m_enemyExplosionList[i].State == SoundState.Stopped)
                                    {
                                        m_enemyExplosionList[i].Volume = 0.1f * Globals.m_volume;
                                        m_enemyExplosionList[i].Play();
                                        break;
                                    }
                                }

                                m_seekerEnemyList.RemoveAt(seekerIter);
                                m_player1.GetBulletList().RemoveAt(bulletIter);
                                m_score += 150 * (int)m_multiplier;
                                m_subMultiplier += 1;
                                m_multiplierDrop = 5.0f;
                                break;
                            }
                        }
                    }
                }

                #endregion

                #region SHOOTER ENEMY COLLISION
                //LOOPS THROUGH ALL MISSILE ENEMIES
                for (int shooterIter = 0; shooterIter < m_multiShooterList.Count; shooterIter++)
                {
                    if (m_multiShooterList[shooterIter].m_state != MultiShooterEnemy.EnemyStates.SPAWNING)
                    {
                        if (m_shieldActive == true)
                        {
                            // Checks to see if an enemy hit the shield
                            if (m_multiShooterList[shooterIter].GetCollisionCircle().Intersects(m_shield.GetCollisionCircle()))
                            {
                                //LOOPS THROUGH ANIMATION LIST TO FIND A FREE ANIMATION
                                for (int animationIter = 0; animationIter < m_greenAnimation.Count; animationIter++)
                                {
                                    if (m_greenAnimation[animationIter].m_finished)
                                    {
                                        m_greenAnimation[animationIter].m_finished = false;
                                        m_greenAnimation[animationIter].m_position = m_multiShooterList[shooterIter].GetPosition();
                                        break;
                                    }
                                }

                                //LOOPS THROUGH EXPLOSION SOUND LIST TO FIND A FREE SOUND
                                for (int i = 0; i < 10; i++)
                                {
                                    if (m_enemyExplosionList[i].State == SoundState.Stopped)
                                    {
                                        m_enemyExplosionList[i].Volume = 0.1f * Globals.m_volume;
                                        m_enemyExplosionList[i].Play();
                                        break;
                                    }
                                }

                                m_multiShooterList.RemoveAt(shooterIter);
                                m_score += 100 * (int)m_multiplier;
                                m_subMultiplier += 1;
                                m_multiplierDrop = 5.0f;
                                break;
                            }
                        }

                        //CHECKS IF THEY COLLIDE WITH THE PLAYER
                        if (m_player1.m_collisionCircle.Intersects(m_multiShooterList[shooterIter].GetCollisionCircle()))
                        {
                            m_gameOver = true;
                        }



                        //LOOPS THROUGH ALL PLAYERS BULLETS
                        for (int bulletIter = 0; bulletIter < m_player1.GetBulletList().Count; bulletIter++)
                        {
                            //CHECKS IF BULLET HITS THE ENEMY
                            if (m_player1.GetBulletList()[bulletIter].GetCircle().Intersects(m_multiShooterList[shooterIter].GetCollisionCircle()))
                            {
                                //LOOPS THROUGH ANIMATION LIST TO FIND A FREE ANIMATION
                                for (int animationIter = 0; animationIter < m_greenAnimation.Count; animationIter++)
                                {
                                    if (m_greenAnimation[animationIter].m_finished)
                                    {
                                        m_greenAnimation[animationIter].m_finished = false;
                                        m_greenAnimation[animationIter].m_position = m_multiShooterList[shooterIter].GetPosition();
                                        break;
                                    }
                                }

                                //LOOPS THROUGH EXPLOSION SOUND LIST TO FIND A FREE SOUND
                                for (int i = 0; i < 10; i++)
                                {
                                    if (m_enemyExplosionList[i].State == SoundState.Stopped)
                                    {
                                        m_enemyExplosionList[i].Volume = 0.1f * Globals.m_volume;
                                        m_enemyExplosionList[i].Play();
                                        break;
                                    }
                                }

                                m_multiShooterList.RemoveAt(shooterIter);
                                m_score += 100 * (int)m_multiplier;
                                m_subMultiplier += 1;
                                m_multiplierDrop = 5.0f;
                                break;
                            }
                        }
                    }
                }


                #endregion

                #region MINE ENEMY COLLISION
                //LOOPS THROUGH ALL MISSILE ENEMIES
                for (int mineIter = 0; mineIter < m_mineEnemyList.Count; mineIter++)
                {
                    if (m_mineEnemyList[mineIter].m_state != MineEnemy.EnemyStates.SPAWNING)
                    {
                        if (m_shieldActive == true)
                        {
                            // Checks to see if an enemy hit the shield
                            if (m_mineEnemyList[mineIter].GetCollisionCircle().Intersects(m_shield.GetCollisionCircle()))
                            {
                                //LOOPS THROUGH ANIMATION LIST TO FIND A FREE ANIMATION
                                for (int animationIter = 0; animationIter < m_yellowAnimation.Count; animationIter++)
                                {
                                    if (m_yellowAnimation[animationIter].m_finished)
                                    {
                                        m_yellowAnimation[animationIter].m_finished = false;
                                        m_yellowAnimation[animationIter].m_position = m_mineEnemyList[mineIter].GetPosition();
                                        break;
                                    }
                                }


                                //LOOPS THROUGH EXPLOSION SOUND LIST TO FIND A FREE SOUND
                                for (int i = 0; i < 10; i++)
                                {
                                    if (m_enemyExplosionList[i].State == SoundState.Stopped)
                                    {
                                        m_enemyExplosionList[i].Volume = 0.1f * Globals.m_volume;
                                        m_enemyExplosionList[i].Play();
                                        break;
                                    }
                                }


                                m_mineEnemyList.RemoveAt(mineIter);
                                m_score += 100 * (int)m_multiplier;
                                m_subMultiplier += 1;
                                m_multiplierDrop = 5.0f;
                                break;
                            }
                        }

                        //CHECKS IF THEY COLLIDE WITH THE PLAYER
                        else if (m_player1.m_collisionCircle.Intersects(m_mineEnemyList[mineIter].GetCollisionCircle()))
                        {
                            m_gameOver = true;
                        }


                        //LOOPS THROUGH ALL PLAYERS BULLETS
                        for (int bulletIter = 0; bulletIter < m_player1.GetBulletList().Count; bulletIter++)
                        {
                            //CHECKS IF BULLET HITS THE ENEMY
                            if (m_player1.GetBulletList()[bulletIter].GetCircle().Intersects(m_mineEnemyList[mineIter].GetCollisionCircle()))
                            {
                                //LOOPS THROUGH ANIMATION LIST TO FIND A FREE ANIMATION
                                for (int animationIter = 0; animationIter < m_yellowAnimation.Count; animationIter++)
                                {
                                    if (m_yellowAnimation[animationIter].m_finished)
                                    {
                                        m_yellowAnimation[animationIter].m_finished = false;
                                        m_yellowAnimation[animationIter].m_position = m_mineEnemyList[mineIter].GetPosition();
                                        break;
                                    }
                                }

                                //LOOPS THROUGH EXPLOSION SOUND LIST TO FIND A FREE SOUND
                                for (int i = 0; i < 10; i++)
                                {
                                    if (m_enemyExplosionList[i].State == SoundState.Stopped)
                                    {
                                        m_enemyExplosionList[i].Volume = 0.1f * Globals.m_volume;
                                        m_enemyExplosionList[i].Play();
                                        break;
                                    }
                                }

                                m_mineEnemyList.RemoveAt(mineIter);
                                m_score += 100 * (int)m_multiplier;
                                m_subMultiplier += 1;
                                m_multiplierDrop = 5.0f;
                                break;
                            }
                        }
                    }
                }


                #endregion

                #region BULLET AND MINE COLLISIONS WITH SHIELD

                for (int m_bulletIter = 0; m_bulletIter < m_bulletList.Count; m_bulletIter++)
                {
                    if (m_shieldActive == true)
                    {
                        // Checks to see if an enemy hit the shield
                        if (m_bulletList[m_bulletIter].GetCircle().Intersects(m_shield.GetCollisionCircle()))
                        {
                            //LOOPS THROUGH ANIMATION LIST TO FIND A FREE ANIMATION
                            for (int animationIter = 0; animationIter < m_greenAnimation.Count; animationIter++)
                            {
                                if (m_greenAnimation[animationIter].m_finished)
                                {
                                    m_greenAnimation[animationIter].m_finished = false;
                                    m_greenAnimation[animationIter].m_position = m_bulletList[m_bulletIter].GetPosition();
                                    break;
                                }
                            }

                            m_bulletList.RemoveAt(m_bulletIter);
                            break;
                        }
                    }

                    //CHECKS IF THEY COLLIDE WITH THE PLAYER
                    else if (m_player1.m_collisionCircle.Intersects(m_bulletList[m_bulletIter].GetCircle()))
                    {
                        m_gameOver = true;
                    }

                }

                for (int m_mineIter = 0; m_mineIter < m_mineList.Count; m_mineIter++)
                {
                    if (m_shieldActive == true)
                    {
                        // Checks to see if an enemy hit the shield
                        if (m_mineList[m_mineIter].GetCollisionCircle().Intersects(m_shield.GetCollisionCircle()))
                        {
                            //LOOPS THROUGH ANIMATION LIST TO FIND A FREE ANIMATION
                            for (int animationIter = 0; animationIter < m_redAnimation.Count; animationIter++)
                            {
                                if (m_redAnimation[animationIter].m_finished)
                                {
                                    m_redAnimation[animationIter].m_finished = false;
                                    m_redAnimation[animationIter].m_position = m_mineList[m_mineIter].GetPosition();
                                    break;
                                }
                            }

                            //LOOPS THROUGH EXPLOSION SOUND LIST TO FIND A FREE SOUND
                            for (int i = 0; i < 10; i++)
                            {
                                if (m_enemyExplosionList[i].State == SoundState.Stopped)
                                {
                                    m_enemyExplosionList[i].Volume = 0.1f * Globals.m_volume;
                                    m_enemyExplosionList[i].Play();
                                    break;
                                }
                            }

                            m_mineList.RemoveAt(m_mineIter);
                            break;
                        }


                        //CHECKS IF THEY COLLIDE WITH THE PLAYER
                        if (m_player1.m_collisionCircle.Intersects(m_mineList[m_mineIter].GetCollisionCircle()))
                        {
                            m_gameOver = true;
                        }
                    }

                    for (int bulletIter = 0; bulletIter < m_player1.GetBulletList().Count; bulletIter++)
                    {
                        //CHECKS IF BULLET HITS THE ENEMY
                        if (m_player1.GetBulletList()[bulletIter].GetCircle().Intersects(m_mineList[m_mineIter].GetCollisionCircle()))
                        {
                            //LOOPS THROUGH ANIMATION LIST TO FIND A FREE ANIMATION
                            for (int animationIter = 0; animationIter < m_yellowAnimation.Count; animationIter++)
                            {
                                if (m_yellowAnimation[animationIter].m_finished)
                                {
                                    m_yellowAnimation[animationIter].m_finished = false;
                                    m_yellowAnimation[animationIter].m_position = m_mineList[m_mineIter].GetPosition();
                                    break;
                                }
                            }

                            //LOOPS THROUGH EXPLOSION SOUND LIST TO FIND A FREE SOUND
                            for (int i = 0; i < 10; i++)
                            {
                                if (m_enemyExplosionList[i].State == SoundState.Stopped)
                                {
                                    m_enemyExplosionList[i].Volume = 0.1f * Globals.m_volume;
                                    m_enemyExplosionList[i].Play();
                                    break;
                                }
                            }

                            m_mineList.RemoveAt(m_mineIter);
                            break;
                        }
                    }


                }

                for (int m_mineIter = 0; m_mineIter < m_mineList.Count; m_mineIter++)
                {
                    if (m_player1.GetCollisionCircle().Intersects(m_mineList[m_mineIter].GetCollisionCircle()))
                    {
                        m_gameOver = true;
                    }
                }
                
                #endregion
            }

            #region CAMERA_RELATED_UPDATES

            //Zoom In.
            if (Keyboard.GetState().IsKeyDown(Keys.PageUp))
                Globals.m_gameCamera.Zoom += 0.01f;

            //Zoom Out.
            if (Keyboard.GetState().IsKeyDown(Keys.PageDown))
                Globals.m_gameCamera.Zoom -= 0.01f;

            Globals.m_gameCamera.Position = m_player1.GetPosition();

            #endregion

            if (m_subMultiplier >= 10)
            {
                m_multiplier += 1;
                m_subMultiplier = 0;
            }

            if (m_subMultiplier > 0 && m_multiplierDropTimer <= 0)
            {
                m_subMultiplier -= 1;
                m_multiplierDropTimer = 1.5f;
                m_subMultiplier = Math.Max(m_subMultiplier, 0);
            }

            if (m_gameOver)
            {
                AIE.GameStateManager.SetState("POSTGAME", new PostGameState(m_score));
                AIE.GameStateManager.PopState();
                AIE.GameStateManager.PushState("POSTGAME");
            }

            if (m_multiplier > -1 && m_subMultiplier == 0 && m_multiplierDrop <= 0 && m_multiplierDropTimer <= 0)
            {
                m_multiplier -= 1;
                m_multiplierDropTimer = 1.5f;
                m_subMultiplier = 9;
            }

        }

        /// <summary>
        /// Draw Function for the PlayState.
        /// </summary>
        /// <param name="gT"></param>
        /// <param name="sb"></param>
        public override void Draw(GameTime gT, SpriteBatch sb)
        {
            sb.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Globals.m_gameCamera.Transform);

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    sb.Draw(m_levelBackgroundTexture, new Vector2(-600 + (x * 256), -300 + (y * 256)), Color.White);
                }             
            }

            sb.Draw(m_levelBackgroundTexture, new Vector2(-500, -500), Color.White);
            sb.Draw(m_bottomBoundary, new Rectangle(0, 1260, 1260, 200), Color.Lerp(Color.Blue, Color.Red, m_borderLerp));
            sb.Draw(m_topBoundary, new Rectangle(0, -200, 1260, 200),Color.Lerp(Color.Blue, Color.Red, m_borderLerp));
            sb.Draw(m_leftBoundary, new Rectangle(1260, 0, 200, 1260), Color.Lerp(Color.Blue, Color.Red, m_borderLerp));
            sb.Draw(m_rightBoundary, new Rectangle(-200, 0, 200, 1260), Color.Lerp(Color.Blue, Color.Red, m_borderLerp));
            sb.Draw(m_whiteBorder, new Rectangle(0, 0, 1260, 1260), Color.Lerp(Color.Blue, Color.Red, m_borderLerp));
            m_player1.Draw(m_playerBulletTexture, sb);

            if (m_shieldActive == true)
                m_shield.Draw(sb);
            else
                m_shield = null;

            #region ENEMY DRAWS
            for (int iter = 0; iter < m_missileEnemyList.Count; iter++)
            {
                m_missileEnemyList[iter].Draw(sb);
            }

            for (int iter = 0; iter < m_seekerEnemyList.Count; iter++)
            {
                m_seekerEnemyList[iter].Draw(sb);
            }

            for (int iter = 0; iter < m_multiShooterList.Count; iter++)
            {                
                m_multiShooterList[iter].Draw(sb);
            }

            for (int iter = 0; iter < m_mineEnemyList.Count; iter++)
            {
                m_mineEnemyList[iter].Draw(sb);
            }
            #endregion

            for (int iter = 0; iter < m_bulletList.Count(); iter++)
            {
                m_bulletList[iter].Draw(m_enemyBullet, sb);
            }

            for (int iter = 0; iter < m_mineList.Count; iter++)
            {
                m_mineList[iter].Draw(sb);
            }


            for (int iter = 0; iter < m_powerupList.Count; iter++)
            {
                m_powerupList[iter].Draw(sb);
            }

            #region ANIMATION DRAWS
            for (int iter = 0; iter < m_redAnimation.Count; iter++)
            {
                m_redAnimation[iter].Draw(sb);
            }

            for (int iter = 0; iter < m_greenAnimation.Count; iter++)
            {
                m_greenAnimation[iter].Draw(sb);
            }

            for (int iter = 0; iter < m_blueAnimation.Count; iter++)
            {
                m_blueAnimation[iter].Draw(sb);
            }

            for (int iter = 0; iter < m_yellowAnimation.Count; iter++)
            {
                m_yellowAnimation[iter].Draw(sb);
            }
            #endregion

            #region POWERUP_NAME_DRAWS
            if (m_bombDraw == true)
            {
                sb.DrawString(Globals.m_defaultFont, "BOMB", m_timerPosition, Color.Lerp(Color.Transparent, Color.White, m_lerpTimer), 0.0f, Globals.m_defaultFont.MeasureString("BOMB") / 2, 1.0f, SpriteEffects.None, 0.0f);
            }

            if (m_multifireDraw == true)
            {
                sb.DrawString(Globals.m_defaultFont, "MULTIFIRE", m_timerPosition, Color.Lerp(Color.Transparent, Color.White, m_lerpTimer), 0.0f, Globals.m_defaultFont.MeasureString("MULTIFIRE") / 2, 1.0f, SpriteEffects.None, 0.0f);
            }

            if (m_speedUpDraw == true)
            {
                sb.DrawString(Globals.m_defaultFont, "SPEED INCREASE", m_timerPosition, Color.Lerp(Color.Transparent, Color.White, m_lerpTimer), 0.0f, Globals.m_defaultFont.MeasureString("SPEED INCREASE") / 2, 1.0f, SpriteEffects.None, 0.0f);
            }

            if (m_firerateUpDraw == true)
            {
                sb.DrawString(Globals.m_defaultFont, "FIRERATE INCREASE", m_timerPosition, Color.Lerp(Color.Transparent, Color.White, m_lerpTimer), 0.0f, Globals.m_defaultFont.MeasureString("FIRERATE INCREASE") / 2, 1.0f, SpriteEffects.None, 0.0f);
            }
          
            #endregion

            #region HUD

            sb.Draw(m_topHUD, new Vector2(m_scorePosition.X - 480, m_scorePosition.Y - 22), Color.Lerp(Color.Transparent, Color.White, m_fadeInHUD));
            sb.DrawString(Globals.m_defaultFont, m_score.ToString(), m_scorePosition, Color.Lerp(Color.Transparent, Color.White, m_fadeInHUD), 0.0f, Globals.m_defaultFont.MeasureString(m_score.ToString()) / 2, 1.0f, SpriteEffects.None, 0.0f);
            sb.DrawString(Globals.m_defaultFont, "x" + m_multiplier.ToString(), new Vector2(m_scorePosition.X + 25, m_scorePosition.Y + 12), Color.Lerp(Color.Transparent, Color.White, m_fadeInHUD), 0.0f, new Vector2(0, 0), 0.6f, SpriteEffects.None, 0.0f);

            for (int x = -5; x < 5; x++)
            {
                if (x <= m_subMultiplier - 5)
                {
                    sb.Draw(m_multiplierLit, new Vector2((m_scorePosition.X + 12) + (x * 30), m_scorePosition.Y + 42), Color.Lerp(Color.Transparent, Color.Aqua, m_fadeInHUD));
                }

                else
                {
                    sb.Draw(m_multiplierNotLit, new Vector2((m_scorePosition.X + 12) + (x * 30), m_scorePosition.Y + 42), Color.Lerp(Color.Transparent, Color.Aqua, m_fadeInHUD));
                }
            }

            for (int x = 0; x < 5; x++)
            {
                if (x < m_timeSpeedUp)
                {
                    sb.Draw(m_multiplierLit, new Vector2((m_scorePosition.X - 450) + (x * 30), m_scorePosition.Y + 25), new Rectangle(0, 0, 16, 16), Color.Lerp(Color.Transparent, Color.Red, m_fadeInHUD), 0.0f, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0.0f);
                }

                else
                {
                    sb.Draw(m_multiplierNotLit, new Vector2((m_scorePosition.X - 450) + (x * 30), m_scorePosition.Y + 25), new Rectangle(0, 0, 16, 16), Color.Lerp(Color.Transparent, Color.Red, m_fadeInHUD), 0.0f, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0.0f);
                }
            }

            for (int x = 0; x < 5; x++)
            {
                if (x < m_fireRateIncreased)
                {
                    sb.Draw(m_multiplierLit, new Vector2((m_scorePosition.X + 300) + (x * 30), m_scorePosition.Y + 25), new Rectangle(0, 0, 16, 16), Color.Lerp(Color.Transparent, Color.Red, m_fadeInHUD), 0.0f, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0.0f);
                }

                else
                {
                    sb.Draw(m_multiplierNotLit, new Vector2((m_scorePosition.X + 300) + (x * 30), m_scorePosition.Y + 25), new Rectangle(0, 0, 16, 16), Color.Lerp(Color.Transparent, Color.Red, m_fadeInHUD), 0.0f, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0.0f);
                }
            }

            #endregion

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    sb.Draw(m_bombFlash, new Vector2(-600 + (x * 256), -300 + (y * 256)), Color.Lerp(Color.Transparent, Color.White, m_lerpBombTimer));
                }
            }

            sb.End();
        }

        public void BorderColourLerp()
        {

            if( m_borderRed == false)
                m_borderLerp += 0.002f;

            if (m_borderLerp >= 1.0f)
                m_borderRed = true;

            if (m_borderRed == true)
                m_borderLerp -= 0.002f;

            if (m_borderLerp <= 0.0f)
                m_borderRed = false;
        }

        public void SlowMotion(GameTime Gt)
        {
            if (m_slowMotionTimer > 0)
            {
                foreach (SeekerEnemy seekerEnemy in m_seekerEnemyList)
                {
                    seekerEnemy.m_speed = 1.0f;
                    seekerEnemy.m_slowMotion = true;
                }
                foreach (Bullet bullet in m_bulletList)
                {
                    bullet.m_speed = 30.0f;
                }
                foreach (MissleEnemy missileEnemy in m_missileEnemyList)
                {
                    missileEnemy.m_slowMotion = true;
                }
                foreach (MultiShooterEnemy multiShooter in m_multiShooterList)
                {
                    multiShooter.m_slowMotion = true;
                }
                foreach (MineEnemy mineEnemy in m_mineEnemyList)
                {
                    mineEnemy.m_slowMotion = true;
                }
            }
            else
            {
                return;
            }
        }

        void KillAllEnemies()
        {
            m_mineEnemyList.Clear();
            m_missileEnemyList.Clear();
            m_multiShooterList.Clear();
            m_seekerEnemyList.Clear();
            m_bulletList.Clear();
            m_mineList.Clear();

            m_powerupUsed = true;
        }


    }
}
