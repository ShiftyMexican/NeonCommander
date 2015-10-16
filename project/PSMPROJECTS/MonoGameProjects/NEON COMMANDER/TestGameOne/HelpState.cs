using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
using TestGameOne;

namespace TestGameOne
{
    public class HelpState : AIE.IGameState
    {
        private Texture2D m_backgroundSprite;
        private OptionsObject m_options;

        private Texture2D m_titleTexture;
        private Texture2D m_buttonTexture;
        private Texture2D m_playerTexture;
        private Texture2D m_playerGlowTexture;

        private Texture2D m_indicator;
        private Vector2 m_indicatorLeft;
        private Vector2 m_indicatorRight;
        private int m_buttonSelected;

        private Color m_text;
        private Color m_playerColour;

        public float m_red { get; private set; }
        public float m_green { get; private set; }
        public float m_blue { get; private set; }
        public float m_volume { get; private set; }
        //public bool m_fullscreen { get; private set; }
        private bool m_imageJustOverlayed;

        private bool m_showInstructions;
        private bool m_showGamePadControls;
        private bool m_showKeyboardControls;
        private bool m_showVitaControls;
        private bool m_showEnemies;
        private bool m_showPowerups;

        private Button m_redUpButton;
        private Button m_redDownButton;
        private Button m_greenUpButton;
        private Button m_greenDownButton;
        private Button m_blueUpButton;
        private Button m_blueDownButton;
        private Button m_volumeUpButton;
        private Button m_volumeDownButton;
        //private Button m_fullscreenButton;
        private Button m_instructionsButton;
        private Button m_enemiesButton;
        private Button m_powerupsButton;
        private Button m_backButton;
        private Button m_controlsButton;

        private Texture2D m_enemiesInformation;
        private Texture2D m_pcControls;
        private Texture2D m_vitaControls;
        private Texture2D m_gamePadControls;
        private Texture2D m_powerupInformation;
        private Texture2D m_instructionsInformation;
        private Texture2D m_playerColourButtonTexture;
        private Texture2D m_instructionsButtonTexture;
        private Texture2D m_volumeButtonTexture;
        private Texture2D m_controlsButtonTexture;
        private Texture2D m_enemiesButtonTexture;
        //private Texture2D m_fullscreenButtonTexture;
        private Texture2D m_powerupsButtonTexture;
        private Texture2D m_backButtonTexture;

        private SoundEffect m_blipSound;


        public HelpState()
            : base()
        {
            m_backgroundSprite = content.Load<Texture2D>("Images/MenuBackground");
            m_titleTexture = content.Load<Texture2D>("Images/HelpAndOptionsTitle");
            m_buttonTexture = content.Load<Texture2D>("Images/arrows");
            m_playerTexture = content.Load<Texture2D>("Images/playerNoGlow");
            m_playerGlowTexture = content.Load<Texture2D>("Images/playerGlow");
            m_indicator = content.Load<Texture2D>("Images/topLeftSelector");

            m_playerColourButtonTexture = content.Load<Texture2D>("Images/PlayerColourText"); 
            m_instructionsButtonTexture = content.Load<Texture2D>("Images/InstructionsText");
            m_volumeButtonTexture = content.Load<Texture2D>("Images/VolumeText");
            m_controlsButtonTexture = content.Load<Texture2D>("Images/ControlsText");
            m_enemiesButtonTexture = content.Load<Texture2D>("Images/EnemiesText");
            //m_fullscreenButtonTexture = content.Load<Texture2D>("Images/FullscreenText");
            m_powerupsButtonTexture = content.Load<Texture2D>("Images/PowerupsText");
            m_backButtonTexture = content.Load<Texture2D>("Images/BackText");

            m_enemiesInformation = content.Load<Texture2D>("Images/Enemies");
            m_pcControls = content.Load<Texture2D>("Images/PCControls");
            m_vitaControls = content.Load<Texture2D>("Images/VitaControls");
            m_gamePadControls = content.Load<Texture2D>("Images/GamePadControls");
            m_powerupInformation = content.Load<Texture2D>("Images/PowerupsInformation");
            m_instructionsInformation = content.Load<Texture2D>("Images/Instructions");

            m_blipSound = content.Load<SoundEffect>("Sounds/Blip");
            m_options = new OptionsObject();
            m_options.LoadOptions();

            m_buttonSelected = 0;

            m_red = m_options.m_red;
            m_green = m_options.m_green;
            m_blue = m_options.m_blue;
            m_volume = m_options.m_volume;
            //m_fullscreen = m_options.m_fullscreen;
            m_imageJustOverlayed = false;

            m_showInstructions = false;
            m_showGamePadControls = false;
            m_showKeyboardControls = false;
            m_showVitaControls = false;
            m_showEnemies = false;
            m_showPowerups = false;
            
            m_text = Color.White;

            m_redUpButton = new Button(370, 190, 16, 16, m_buttonTexture);
            m_redDownButton = new Button(370, 280, 16, 16, m_buttonTexture);
            m_greenUpButton = new Button(410, 190, 16, 16, m_buttonTexture);
            m_greenDownButton = new Button(410, 280, 16, 16, m_buttonTexture);
            m_blueUpButton = new Button(450, 190, 16, 16, m_buttonTexture);
            m_blueDownButton = new Button(450, 280, 16, 16, m_buttonTexture);
            m_volumeUpButton = new Button(410, 335, 16, 16, m_buttonTexture);
            m_volumeDownButton = new Button(410, 405, 16, 16, m_buttonTexture);
           // m_fullscreenButton = new Button(420, 415, 16, 16, m_buttonTexture);
            m_instructionsButton = new Button(680, 175, 224, 40, m_instructionsButtonTexture);
            m_enemiesButton = new Button(675, 330, 155, 40, m_enemiesButtonTexture);
            m_powerupsButton = new Button(685, 415, 175, 40, m_powerupsButtonTexture);
            m_backButton = new Button(478, 498, 100, 40, m_backButtonTexture);
            m_controlsButton = new Button(680, 250, 155, 40, m_controlsButtonTexture);

            m_indicatorLeft.X = m_redUpButton.m_texturePosition.X - m_redUpButton.m_rectangle.Width / 2;
            m_indicatorLeft.Y = m_redUpButton.m_texturePosition.Y - 8;

            m_indicatorRight.X = m_redUpButton.m_texturePosition.X + m_redUpButton.m_rectangle.Width / 2;
            m_indicatorRight.Y = m_redUpButton.m_texturePosition.Y + 8;
        }

        public override void Update(GameTime gameTime)
        {
            

            #region BUTTON UPDATES
            m_redUpButton.Update(gameTime);
            m_redDownButton.Update(gameTime);
            m_greenUpButton.Update(gameTime);
            m_greenDownButton.Update(gameTime);
            m_blueUpButton.Update(gameTime);
            m_blueDownButton.Update(gameTime);
            m_volumeUpButton.Update(gameTime);
            m_volumeDownButton.Update(gameTime);
            //m_fullscreenButton.Update(gameTime);
            m_instructionsButton.Update(gameTime);
            m_enemiesButton.Update(gameTime);
            m_powerupsButton.Update(gameTime);
            m_backButton.Update(gameTime);
            m_controlsButton.Update(gameTime);
            #endregion
                       
            #region GAMEPAD/KEYBOARD RELATED
            //TO IMPLEMENT GAMEPAD STICKS
            //Up
            //GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0 && abs()GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > abs()GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X
            //

            #region LEFT
            if (InputManager.InputManager.IsKeyJustPressed(Keys.Left) || InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.DPadLeft) || (InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.LeftThumbstickLeft) && GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0 && Math.Abs(GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X) > Math.Abs(GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y)))
            {
                m_blipSound.Play(0.001f * Globals.m_volume, 0.4f, 0.4f);

                // Red up to Instructions
                if (m_buttonSelected == 0)
                {
                    m_buttonSelected = 3;

                    m_indicatorLeft.X = m_instructionsButton.m_texturePosition.X - m_instructionsButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_instructionsButton.m_texturePosition.Y - 16;

                    m_indicatorRight.X = m_instructionsButton.m_texturePosition.X + m_instructionsButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_instructionsButton.m_texturePosition.Y + 16;
                }

                // Green up to Red up
                else if (m_buttonSelected == 1)
                {
                    m_buttonSelected = 0;

                    m_indicatorLeft.X = m_redUpButton.m_texturePosition.X - m_redUpButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_redUpButton.m_texturePosition.Y - 8;

                    m_indicatorRight.X = m_redUpButton.m_texturePosition.X + m_redUpButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_redUpButton.m_texturePosition.Y + 8;
                }

                // Blue up to Green up
                else if (m_buttonSelected == 2)
                {
                    m_buttonSelected = 1;

                    m_indicatorLeft.X = m_greenUpButton.m_texturePosition.X - m_greenUpButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_greenUpButton.m_texturePosition.Y - 8;

                    m_indicatorRight.X = m_greenUpButton.m_texturePosition.X + m_greenUpButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_greenUpButton.m_texturePosition.Y + 8;
                }

                // Instructions to Blue up
                else if (m_buttonSelected == 3)
                {
                    m_buttonSelected = 2;

                    m_indicatorLeft.X = m_blueUpButton.m_texturePosition.X - m_blueUpButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_blueUpButton.m_texturePosition.Y - 8;

                    m_indicatorRight.X = m_blueUpButton.m_texturePosition.X + m_blueUpButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_blueUpButton.m_texturePosition.Y + 8;
                }

                // Red down to Controls
                else if (m_buttonSelected == 4)
                {
                    m_buttonSelected = 13;

                    m_indicatorLeft.X = m_controlsButton.m_texturePosition.X - m_controlsButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_controlsButton.m_texturePosition.Y - 16;

                    m_indicatorRight.X = m_controlsButton.m_texturePosition.X + m_controlsButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_controlsButton.m_texturePosition.Y + 16;
                }

                // Green down to Red down
                else if (m_buttonSelected == 5)
                {
                    m_buttonSelected = 4;

                    m_indicatorLeft.X = m_redDownButton.m_texturePosition.X - m_redDownButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_redDownButton.m_texturePosition.Y - 8;

                    m_indicatorRight.X = m_redDownButton.m_texturePosition.X + m_redDownButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_redDownButton.m_texturePosition.Y + 8;
                }

                // Blue down to Green down
                else if (m_buttonSelected == 6)
                {
                    m_buttonSelected = 5;

                    m_indicatorLeft.X = m_greenDownButton.m_texturePosition.X - m_greenDownButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_greenDownButton.m_texturePosition.Y - 8;

                    m_indicatorRight.X = m_greenDownButton.m_texturePosition.X + m_greenDownButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_greenDownButton.m_texturePosition.Y + 8;
                }

                // Volume up to Enemies
                else if (m_buttonSelected == 7)
                {
                    m_buttonSelected = 8;

                    m_indicatorLeft.X = m_enemiesButton.m_texturePosition.X - m_enemiesButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_enemiesButton.m_texturePosition.Y - 16;

                    m_indicatorRight.X = m_enemiesButton.m_texturePosition.X + m_enemiesButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_enemiesButton.m_texturePosition.Y + 16;
                }

                // Enemies to Volume up
                else if (m_buttonSelected == 8)
                {
                    m_buttonSelected = 7;

                    m_indicatorLeft.X = m_volumeUpButton.m_texturePosition.X - m_volumeUpButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_volumeUpButton.m_texturePosition.Y - 8;

                    m_indicatorRight.X = m_volumeUpButton.m_texturePosition.X + m_volumeUpButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_volumeUpButton.m_texturePosition.Y + 8;
                }

                // Volume down to Powerups
                else if (m_buttonSelected == 9)
                {
                    m_buttonSelected = 11;

                    m_indicatorLeft.X = m_powerupsButton.m_texturePosition.X - m_powerupsButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_powerupsButton.m_texturePosition.Y - 16;

                    m_indicatorRight.X = m_powerupsButton.m_texturePosition.X + m_powerupsButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_powerupsButton.m_texturePosition.Y + 16;
                }

                // Fullscreen to Powerups
               //else if (m_buttonSelected == 10)
               //{
               //    m_buttonSelected = 11;
               //
               //    m_indicatorLeft.X = m_powerupsButton.m_texturePosition.X - m_powerupsButton.m_rectangle.Width / 2;
               //    m_indicatorLeft.Y = m_powerupsButton.m_texturePosition.Y - 16;
               //
               //    m_indicatorRight.X = m_powerupsButton.m_texturePosition.X + m_powerupsButton.m_rectangle.Width / 2;
               //    m_indicatorRight.Y = m_powerupsButton.m_texturePosition.Y + 16;
               //}

                //Powerups to Volume down
                else if (m_buttonSelected == 11)
                {
                    m_buttonSelected = 9;

                    m_indicatorLeft.X = m_volumeDownButton.m_texturePosition.X - m_volumeDownButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_volumeDownButton.m_texturePosition.Y - 8;

                    m_indicatorRight.X = m_volumeDownButton.m_texturePosition.X + m_volumeDownButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_volumeDownButton.m_texturePosition.Y + 8;
                }

                //Powerups to Fullscreen
                //else if (m_buttonSelected == 11)
                //{
                //    m_buttonSelected = 10;
                //
                //    m_indicatorLeft.X = m_fullscreenButton.m_texturePosition.X - m_fullscreenButton.m_rectangle.Width / 2;
                //    m_indicatorLeft.Y = m_fullscreenButton.m_texturePosition.Y - 16;
                //
                //    m_indicatorRight.X = m_fullscreenButton.m_texturePosition.X + m_fullscreenButton.m_rectangle.Width / 2;
                //    m_indicatorRight.Y = m_fullscreenButton.m_texturePosition.Y + 16;
                //}
                
                //Controls to Blue down
                else if (m_buttonSelected == 13)
                {
                    m_buttonSelected = 6;

                    m_indicatorLeft.X = m_blueDownButton.m_texturePosition.X - m_blueDownButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_blueDownButton.m_texturePosition.Y - 8;

                    m_indicatorRight.X = m_blueDownButton.m_texturePosition.X + m_blueDownButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_blueDownButton.m_texturePosition.Y + 8;
                }

            }
            #endregion

            #region RIGHT
            if (InputManager.InputManager.IsKeyJustPressed(Keys.Right) || InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.DPadRight) || (InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.LeftThumbstickRight) && GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0 && Math.Abs(GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X) > Math.Abs(GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y)))
            {
                m_blipSound.Play(0.001f * Globals.m_volume, 0.4f, 0.4f);

                // Red to Green up
                if (m_buttonSelected == 0)
                {
                    m_buttonSelected = 1;

                    m_indicatorLeft.X = m_greenUpButton.m_texturePosition.X - m_greenUpButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_greenUpButton.m_texturePosition.Y - 8;

                    m_indicatorRight.X = m_greenUpButton.m_texturePosition.X + m_greenUpButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_greenUpButton.m_texturePosition.Y + 8;
                }

                // Green up to Blue up
                else if (m_buttonSelected == 1)
                {
                    m_buttonSelected = 2;

                    m_indicatorLeft.X = m_blueUpButton.m_texturePosition.X - m_blueUpButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_blueUpButton.m_texturePosition.Y - 8;

                    m_indicatorRight.X = m_blueUpButton.m_texturePosition.X + m_blueUpButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_blueUpButton.m_texturePosition.Y + 8;
                }

                // Blue up to Instructions
                else if (m_buttonSelected == 2)
                {
                    m_buttonSelected = 3;

                    m_indicatorLeft.X = m_instructionsButton.m_texturePosition.X - m_instructionsButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_instructionsButton.m_texturePosition.Y - 16;

                    m_indicatorRight.X = m_instructionsButton.m_texturePosition.X + m_instructionsButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_instructionsButton.m_texturePosition.Y + 16;
                }

                // Instructions to Red up
                else if (m_buttonSelected == 3)
                {
                    m_buttonSelected = 0;

                    m_indicatorLeft.X = m_redUpButton.m_texturePosition.X - m_redUpButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_redUpButton.m_texturePosition.Y - 8;

                    m_indicatorRight.X = m_redUpButton.m_texturePosition.X + m_redUpButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_redUpButton.m_texturePosition.Y + 8;
                }

                // Red down to Green down
                else if (m_buttonSelected == 4)
                {
                    m_buttonSelected = 5;

                    m_indicatorLeft.X = m_greenDownButton.m_texturePosition.X - m_greenDownButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_greenDownButton.m_texturePosition.Y - 8;

                    m_indicatorRight.X = m_greenDownButton.m_texturePosition.X + m_greenDownButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_greenDownButton.m_texturePosition.Y + 8;
                }

                // Green down to Blue down
                else if (m_buttonSelected == 5)
                {
                    m_buttonSelected = 6;

                    m_indicatorLeft.X = m_blueDownButton.m_texturePosition.X - m_blueDownButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_blueDownButton.m_texturePosition.Y - 8;

                    m_indicatorRight.X = m_blueDownButton.m_texturePosition.X + m_blueDownButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_blueDownButton.m_texturePosition.Y + 8;
                }

                // Blue down to Controls
                else if (m_buttonSelected == 6)
                {
                    m_buttonSelected = 13;

                    m_indicatorLeft.X = m_controlsButton.m_texturePosition.X - m_controlsButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_controlsButton.m_texturePosition.Y - 16;

                    m_indicatorRight.X = m_controlsButton.m_texturePosition.X + m_controlsButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_controlsButton.m_texturePosition.Y + 16;
                }

                // Volume up to Enemies
                else if (m_buttonSelected == 7)
                {
                    m_buttonSelected = 8;

                    m_indicatorLeft.X = m_enemiesButton.m_texturePosition.X - m_enemiesButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_enemiesButton.m_texturePosition.Y - 16;

                    m_indicatorRight.X = m_enemiesButton.m_texturePosition.X + m_enemiesButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_enemiesButton.m_texturePosition.Y + 16;
                }

                // Enemies to Volume up
                else if (m_buttonSelected == 8)
                {
                    m_buttonSelected = 7;

                    m_indicatorLeft.X = m_volumeUpButton.m_texturePosition.X - m_volumeUpButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_volumeUpButton.m_texturePosition.Y - 8;

                    m_indicatorRight.X = m_volumeUpButton.m_texturePosition.X + m_volumeUpButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_volumeUpButton.m_texturePosition.Y + 8;
                }

                // Volume down to Powerups
                else if (m_buttonSelected == 9)
                {
                    m_buttonSelected = 11;

                    m_indicatorLeft.X = m_powerupsButton.m_texturePosition.X - m_powerupsButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_powerupsButton.m_texturePosition.Y - 16;

                    m_indicatorRight.X = m_powerupsButton.m_texturePosition.X + m_powerupsButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_powerupsButton.m_texturePosition.Y + 16;
                }

                // Fullscreen to Powerups
                //else if (m_buttonSelected == 10)
                //{
                //    m_buttonSelected = 11;
                //
                //    m_indicatorLeft.X = m_powerupsButton.m_texturePosition.X - m_powerupsButton.m_rectangle.Width / 2;
                //    m_indicatorLeft.Y = m_powerupsButton.m_texturePosition.Y - 16;
                //
                //    m_indicatorRight.X = m_powerupsButton.m_texturePosition.X + m_powerupsButton.m_rectangle.Width / 2;
                //    m_indicatorRight.Y = m_powerupsButton.m_texturePosition.Y + 16;
                //}


                //Powerups to Volume Down
                else if (m_buttonSelected == 11)
                {
                    m_buttonSelected = 9;

                    m_indicatorLeft.X = m_volumeDownButton.m_texturePosition.X - m_volumeDownButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_volumeDownButton.m_texturePosition.Y - 8;

                    m_indicatorRight.X = m_volumeDownButton.m_texturePosition.X + m_volumeDownButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_volumeDownButton.m_texturePosition.Y + 8;
                }


                //Powerups to Fullscreen
                //else if (m_buttonSelected == 11)
                //{
                //    m_buttonSelected = 10;
                //
                //    m_indicatorLeft.X = m_fullscreenButton.m_texturePosition.X - m_fullscreenButton.m_rectangle.Width / 2;
                //    m_indicatorLeft.Y = m_fullscreenButton.m_texturePosition.Y - 8;
                //
                //    m_indicatorRight.X = m_fullscreenButton.m_texturePosition.X + m_fullscreenButton.m_rectangle.Width / 2;
                //    m_indicatorRight.Y = m_fullscreenButton.m_texturePosition.Y + 8;
                //}

                //Controls to Red down
                else if (m_buttonSelected == 13)
                {
                    m_buttonSelected = 4;

                    m_indicatorLeft.X = m_redDownButton.m_texturePosition.X - m_redDownButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_redDownButton.m_texturePosition.Y - 8;

                    m_indicatorRight.X = m_redDownButton.m_texturePosition.X + m_redDownButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_redDownButton.m_texturePosition.Y + 8;
                }
            }
            #endregion

            #region DOWN
            if (InputManager.InputManager.IsKeyJustPressed(Keys.Down) || InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.DPadDown) || (InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.LeftThumbstickDown) && GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < 0 && Math.Abs(GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y) > Math.Abs(GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X)))
            {
                m_blipSound.Play(0.001f * Globals.m_volume, 0.4f, 0.4f);

                // Red to Red down
                if (m_buttonSelected == 0)
                {
                    m_buttonSelected = 4;

                    m_indicatorLeft.X = m_redDownButton.m_texturePosition.X - m_redDownButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_redDownButton.m_texturePosition.Y - 8;

                    m_indicatorRight.X = m_redDownButton.m_texturePosition.X + m_redDownButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_redDownButton.m_texturePosition.Y + 8;
                }

                // Green up to Green down
                else if (m_buttonSelected == 1)
                {
                    m_buttonSelected = 5;

                    m_indicatorLeft.X = m_greenDownButton.m_texturePosition.X - m_greenDownButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_greenDownButton.m_texturePosition.Y - 8;

                    m_indicatorRight.X = m_greenDownButton.m_texturePosition.X + m_greenDownButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_greenDownButton.m_texturePosition.Y + 8;
                }

                // Blue up to Blue down
                else if (m_buttonSelected == 2)
                {
                    m_buttonSelected = 6;

                    m_indicatorLeft.X = m_blueDownButton.m_texturePosition.X - m_blueDownButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_blueDownButton.m_texturePosition.Y - 8;

                    m_indicatorRight.X = m_blueDownButton.m_texturePosition.X + m_blueDownButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_blueDownButton.m_texturePosition.Y + 8;
                }

                // Instructions to Controls
                else if (m_buttonSelected == 3)
                {
                    m_buttonSelected = 13;

                    m_indicatorLeft.X = m_controlsButton.m_texturePosition.X - m_controlsButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_controlsButton.m_texturePosition.Y - 16;

                    m_indicatorRight.X = m_controlsButton.m_texturePosition.X + m_controlsButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_controlsButton.m_texturePosition.Y + 16;
                }

                // Red down to Volume up
                else if (m_buttonSelected == 4)
                {
                    m_buttonSelected = 7;

                    m_indicatorLeft.X = m_volumeUpButton.m_texturePosition.X - m_volumeUpButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_volumeUpButton.m_texturePosition.Y - 8;

                    m_indicatorRight.X = m_volumeUpButton.m_texturePosition.X + m_volumeUpButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_volumeUpButton.m_texturePosition.Y + 8;
                }

                // Green down to Volume up
                else if (m_buttonSelected == 5)
                {
                    m_buttonSelected = 7;

                    m_indicatorLeft.X = m_volumeUpButton.m_texturePosition.X - m_volumeUpButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_volumeUpButton.m_texturePosition.Y - 8;

                    m_indicatorRight.X = m_volumeUpButton.m_texturePosition.X + m_volumeUpButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_volumeUpButton.m_texturePosition.Y + 8;
                }

                // Blue down to Volume up
                else if (m_buttonSelected == 6)
                {
                    m_buttonSelected = 7;

                    m_indicatorLeft.X = m_volumeUpButton.m_texturePosition.X - m_volumeUpButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_volumeUpButton.m_texturePosition.Y - 8;

                    m_indicatorRight.X = m_volumeUpButton.m_texturePosition.X + m_volumeUpButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_volumeUpButton.m_texturePosition.Y + 8;
                }

                // Volume up to Volume down
                else if (m_buttonSelected == 7)
                {
                    m_buttonSelected = 9;

                    m_indicatorLeft.X = m_volumeDownButton.m_texturePosition.X - m_volumeDownButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_volumeDownButton.m_texturePosition.Y - 8;

                    m_indicatorRight.X = m_volumeDownButton.m_texturePosition.X + m_volumeDownButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_volumeDownButton.m_texturePosition.Y + 8;
                }

                // Enemies to Powerups
                else if (m_buttonSelected == 8)
                {
                    m_buttonSelected = 11;

                    m_indicatorLeft.X = m_powerupsButton.m_texturePosition.X - m_powerupsButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_powerupsButton.m_texturePosition.Y - 16;

                    m_indicatorRight.X = m_powerupsButton.m_texturePosition.X + m_powerupsButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_powerupsButton.m_texturePosition.Y + 16;
                }

                //Volume down to Back
                else if (m_buttonSelected == 9)
                {
                    m_buttonSelected = 12;

                    m_indicatorLeft.X = m_backButton.m_texturePosition.X - m_backButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_backButton.m_texturePosition.Y - 16;

                    m_indicatorRight.X = m_backButton.m_texturePosition.X + m_backButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_backButton.m_texturePosition.Y + 16;
                }                    


                // Volume down to FullScreen
                //else if (m_buttonSelected == 9)
                //{
                //    m_buttonSelected = 10;
                //
                //    m_indicatorLeft.X = m_fullscreenButton.m_texturePosition.X - m_fullscreenButton.m_rectangle.Width / 2;
                //    m_indicatorLeft.Y = m_fullscreenButton.m_texturePosition.Y - 8;
                //
                //    m_indicatorRight.X = m_fullscreenButton.m_texturePosition.X + m_fullscreenButton.m_rectangle.Width / 2;
                //    m_indicatorRight.Y = m_fullscreenButton.m_texturePosition.Y + 8;
                //}

                // Fullscreen to Back
                //else if (m_buttonSelected == 10)
                //{
                //    m_buttonSelected = 12;
                //
                //    m_indicatorLeft.X = m_backButton.m_texturePosition.X - m_backButton.m_rectangle.Width / 2;
                //    m_indicatorLeft.Y = m_backButton.m_texturePosition.Y - 16;
                //
                //    m_indicatorRight.X = m_backButton.m_texturePosition.X + m_backButton.m_rectangle.Width / 2;
                //    m_indicatorRight.Y = m_backButton.m_texturePosition.Y + 16;
                //}

                //Powerups to Back
                else if (m_buttonSelected == 11)
                {
                    m_buttonSelected = 12;

                    m_indicatorLeft.X = m_backButton.m_texturePosition.X - m_backButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_backButton.m_texturePosition.Y - 16;

                    m_indicatorRight.X = m_backButton.m_texturePosition.X + m_backButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_backButton.m_texturePosition.Y + 16;
                }

                //Back to Red up
                else if (m_buttonSelected == 12)
                {
                    m_buttonSelected = 0;

                    m_indicatorLeft.X = m_redUpButton.m_texturePosition.X - m_redUpButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_redUpButton.m_texturePosition.Y - 8;

                    m_indicatorRight.X = m_redUpButton.m_texturePosition.X + m_redUpButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_redUpButton.m_texturePosition.Y + 8;
                }

                //Controls to Enemies
                else if (m_buttonSelected == 13)
                {
                    m_buttonSelected = 8;

                    m_indicatorLeft.X = m_enemiesButton.m_texturePosition.X - m_enemiesButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_enemiesButton.m_texturePosition.Y - 16;

                    m_indicatorRight.X = m_enemiesButton.m_texturePosition.X + m_enemiesButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_enemiesButton.m_texturePosition.Y + 16;
                }

            }
            #endregion

            #region UP
            if (InputManager.InputManager.IsKeyJustPressed(Keys.Up) || InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.DPadUp) || (InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.LeftThumbstickUp) && GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0 && Math.Abs(GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y) > Math.Abs(GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X)))
            {
                m_blipSound.Play(0.001f * Globals.m_volume, 0.4f, 0.4f);

                // Red up to Back
                if (m_buttonSelected == 0)
                {
                    m_buttonSelected = 12;

                    m_indicatorLeft.X = m_backButton.m_texturePosition.X - m_backButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_backButton.m_texturePosition.Y - 16;

                    m_indicatorRight.X = m_backButton.m_texturePosition.X + m_backButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_backButton.m_texturePosition.Y + 16;
                }

                // Green up to Back
                else if (m_buttonSelected == 1)
                {
                    m_buttonSelected = 12;

                    m_indicatorLeft.X = m_backButton.m_texturePosition.X - m_backButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_backButton.m_texturePosition.Y - 16;

                    m_indicatorRight.X = m_backButton.m_texturePosition.X + m_backButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_backButton.m_texturePosition.Y + 16;
                }

                // Blue up to Back
                else if (m_buttonSelected == 2)
                {
                    m_buttonSelected = 12;

                    m_indicatorLeft.X = m_backButton.m_texturePosition.X - m_backButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_backButton.m_texturePosition.Y - 16;

                    m_indicatorRight.X = m_backButton.m_texturePosition.X + m_backButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_backButton.m_texturePosition.Y + 16;
                }

                // Instructions to Back
                else if (m_buttonSelected == 3)
                {
                    m_buttonSelected = 12;

                    m_indicatorLeft.X = m_backButton.m_texturePosition.X - m_backButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_backButton.m_texturePosition.Y - 16;

                    m_indicatorRight.X = m_backButton.m_texturePosition.X + m_backButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_backButton.m_texturePosition.Y + 16;
                }

                // Red down to Red up
                else if (m_buttonSelected == 4)
                {
                    m_buttonSelected = 0;

                    m_indicatorLeft.X = m_redUpButton.m_texturePosition.X - m_redUpButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_redUpButton.m_texturePosition.Y - 8;

                    m_indicatorRight.X = m_redUpButton.m_texturePosition.X + m_redUpButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_redUpButton.m_texturePosition.Y + 8;
                }

                // Green down to Green up
                else if (m_buttonSelected == 5)
                {
                    m_buttonSelected = 1;

                    m_indicatorLeft.X = m_greenUpButton.m_texturePosition.X - m_greenUpButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_greenUpButton.m_texturePosition.Y - 8;

                    m_indicatorRight.X = m_greenUpButton.m_texturePosition.X + m_greenUpButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_greenUpButton.m_texturePosition.Y + 8;
                }

                // Blue down to Blue up
                else if (m_buttonSelected == 6)
                {
                    m_buttonSelected = 2;

                    m_indicatorLeft.X = m_blueUpButton.m_texturePosition.X - m_blueUpButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_blueUpButton.m_texturePosition.Y - 8;

                    m_indicatorRight.X = m_blueUpButton.m_texturePosition.X + m_blueUpButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_blueUpButton.m_texturePosition.Y + 8;
                }

                // Volume up to Green down
                else if (m_buttonSelected == 7)
                {
                    m_buttonSelected = 5;

                    m_indicatorLeft.X = m_greenDownButton.m_texturePosition.X - m_greenDownButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_greenDownButton.m_texturePosition.Y - 8;

                    m_indicatorRight.X = m_greenDownButton.m_texturePosition.X + m_greenDownButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_greenDownButton.m_texturePosition.Y + 8;
                }

                // Enemies to Controls
                else if (m_buttonSelected == 8)
                {
                    m_buttonSelected = 13;

                    m_indicatorLeft.X = m_controlsButton.m_texturePosition.X - m_controlsButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_controlsButton.m_texturePosition.Y - 16;

                    m_indicatorRight.X = m_controlsButton.m_texturePosition.X + m_controlsButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_controlsButton.m_texturePosition.Y + 16;
                }

                // Volume down to Volume up
                else if (m_buttonSelected == 9)
                {
                    m_buttonSelected = 7;

                    m_indicatorLeft.X = m_volumeUpButton.m_texturePosition.X - m_volumeUpButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_volumeUpButton.m_texturePosition.Y - 8;

                    m_indicatorRight.X = m_volumeUpButton.m_texturePosition.X + m_volumeUpButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_volumeUpButton.m_texturePosition.Y + 8;
                }

                // Fullscreen to Volume down
                else if (m_buttonSelected == 10)
                {
                    m_buttonSelected = 9;

                    m_indicatorLeft.X = m_volumeDownButton.m_texturePosition.X - m_volumeDownButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_volumeDownButton.m_texturePosition.Y - 8;

                    m_indicatorRight.X = m_volumeDownButton.m_texturePosition.X + m_volumeDownButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_volumeDownButton.m_texturePosition.Y + 8;
                }

                //Powerups to Enemies
                else if (m_buttonSelected == 11)
                {
                    m_buttonSelected = 8;

                    m_indicatorLeft.X = m_enemiesButton.m_texturePosition.X - m_enemiesButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_enemiesButton.m_texturePosition.Y - 16;

                    m_indicatorRight.X = m_enemiesButton.m_texturePosition.X + m_enemiesButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_enemiesButton.m_texturePosition.Y + 16;
                }

                //Back to Volume Down
                else if (m_buttonSelected == 12)
                {
                    m_buttonSelected = 9;

                    m_indicatorLeft.X = m_volumeDownButton.m_texturePosition.X - m_volumeDownButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_volumeDownButton.m_texturePosition.Y - 8;

                    m_indicatorRight.X = m_volumeDownButton.m_texturePosition.X + m_volumeDownButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_volumeDownButton.m_texturePosition.Y + 8;
                }

               //Back to Fullscreen
                // else if (m_buttonSelected == 12)
                // {
                //     m_buttonSelected = 10;
                //
                //     m_indicatorLeft.X = m_fullscreenButton.m_texturePosition.X - m_fullscreenButton.m_rectangle.Width / 2;
                //     m_indicatorLeft.Y = m_fullscreenButton.m_texturePosition.Y - 8;
                //
                //     m_indicatorRight.X = m_fullscreenButton.m_texturePosition.X + m_fullscreenButton.m_rectangle.Width / 2;
                //     m_indicatorRight.Y = m_fullscreenButton.m_texturePosition.Y + 8;
                // }

               //Controls to Instructions
                else if (m_buttonSelected == 13)
                {
                    m_buttonSelected = 3;

                    m_indicatorLeft.X = m_instructionsButton.m_texturePosition.X - m_instructionsButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_instructionsButton.m_texturePosition.Y - 16;

                    m_indicatorRight.X = m_instructionsButton.m_texturePosition.X + m_instructionsButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_instructionsButton.m_texturePosition.Y + 16;
                }
            }
            #endregion

            #region ENTER/A
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.B))
            {
                if (InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.A) || InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.B) || InputManager.InputManager.IsKeyJustPressed(Keys.Enter))
                {
                    m_blipSound.Play(0.001f * Globals.m_volume, 0.75f, 0.75f);
                }

                if (m_buttonSelected == 0)
                {
                    m_red += 0.01f;
                }

                if (m_buttonSelected == 1)
                {
                    m_green += 0.01f;
                }
                
                if (m_buttonSelected == 2)
                {
                    m_blue += 0.01f;
                }

                if (m_buttonSelected == 3 && (InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.A) || InputManager.InputManager.IsKeyJustPressed(Keys.Enter)))
                {
                    m_showInstructions = true;
                    m_imageJustOverlayed = true;
                    m_buttonSelected = 14;

                    m_indicatorLeft.X = m_backButton.m_texturePosition.X - m_backButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_backButton.m_texturePosition.Y - 16;

                    m_indicatorRight.X = m_backButton.m_texturePosition.X + m_backButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_backButton.m_texturePosition.Y + 16;
                }

                if (m_buttonSelected == 4)
                {
                    m_red -= 0.01f;
                }

                if (m_buttonSelected == 5)
                {
                    m_green -= 0.01f;
                }

                if (m_buttonSelected == 6)
                {
                    m_blue -= 0.01f;
                }

                if (m_buttonSelected == 7)
                {
                    m_options.m_volume += 0.01f;
                }

                if (m_buttonSelected == 8 && (InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.A) || InputManager.InputManager.IsKeyJustPressed(Keys.Enter)))
                {
                    m_showEnemies = true;
                    m_imageJustOverlayed = true;
                    m_buttonSelected = 14;

                    m_indicatorLeft.X = m_backButton.m_texturePosition.X - m_backButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_backButton.m_texturePosition.Y - 16;

                    m_indicatorRight.X = m_backButton.m_texturePosition.X + m_backButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_backButton.m_texturePosition.Y + 16;
                }

                if (m_buttonSelected == 9)
                {
                    m_options.m_volume -= 0.01f;
                }

                //if (m_buttonSelected == 10 && (InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.A) || InputManager.InputManager.IsKeyJustPressed(Keys.Enter)))
                //{
                //    m_fullscreen = !m_fullscreen;
                //}

                if (m_buttonSelected == 11 && (InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.A) || InputManager.InputManager.IsKeyJustPressed(Keys.Enter)))
                {
                    m_showPowerups = true;
                    m_imageJustOverlayed = true;
                    m_buttonSelected = 14;

                    m_indicatorLeft.X = m_backButton.m_texturePosition.X - m_backButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_backButton.m_texturePosition.Y - 16;

                    m_indicatorRight.X = m_backButton.m_texturePosition.X + m_backButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_backButton.m_texturePosition.Y + 16;
                }

                //BACK
                if (m_buttonSelected == 12 && (InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.A) || InputManager.InputManager.IsKeyJustPressed(Keys.Enter)))
                {
                    Globals.m_volume = m_options.m_volume;
                    m_options.SaveOptions();
                    //Globals.m_fullscreen = m_options.m_fullscreen;
                    AIE.GameStateManager.PopState();
                    AIE.GameStateManager.PushState("MENU");
                }

                //CONTROLS
                if (m_buttonSelected == 13 && (InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.A) || InputManager.InputManager.IsKeyJustPressed(Keys.Enter)))
                {
#if PSM 
                    m_showVitaControls = true;
                    m_imageJustOverlayed = true;
                    m_buttonSelected = 14;

                    m_indicatorLeft.X = m_backButton.m_texturePosition.X - m_backButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_backButton.m_texturePosition.Y - 16;

                    m_indicatorRight.X = m_backButton.m_texturePosition.X + m_backButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_backButton.m_texturePosition.Y + 16;
#else
                    if (GamePad.GetState(PlayerIndex.One).IsConnected)
                    {
                        m_showGamePadControls = true;
                        m_imageJustOverlayed = true;
                        m_buttonSelected = 14;

                        m_indicatorLeft.X = m_backButton.m_texturePosition.X - m_backButton.m_rectangle.Width / 2;
                        m_indicatorLeft.Y = m_backButton.m_texturePosition.Y - 16;

                        m_indicatorRight.X = m_backButton.m_texturePosition.X + m_backButton.m_rectangle.Width / 2;
                        m_indicatorRight.Y = m_backButton.m_texturePosition.Y + 16;
                    }

                    else
                    {
                        m_showKeyboardControls = true;
                        m_imageJustOverlayed = true;
                        m_buttonSelected = 14;

                        m_indicatorLeft.X = m_backButton.m_texturePosition.X - m_backButton.m_rectangle.Width / 2;
                        m_indicatorLeft.Y = m_backButton.m_texturePosition.Y - 16;

                        m_indicatorRight.X = m_backButton.m_texturePosition.X + m_backButton.m_rectangle.Width / 2;
                        m_indicatorRight.Y = m_backButton.m_texturePosition.Y + 16;
                    }
#endif 
                }
               
                //BACK BUTTON WHEN IMAGE IS OVERLAYED
                if (m_buttonSelected == 14 && (InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.B) || (InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.A) || InputManager.InputManager.IsKeyJustPressed(Keys.Enter)) && !m_imageJustOverlayed))
                {
                    if (m_showInstructions)
                    {
                        m_showInstructions = false;

                        m_buttonSelected = 3;

                        m_indicatorLeft.X = m_instructionsButton.m_texturePosition.X - m_instructionsButton.m_rectangle.Width / 2;
                        m_indicatorLeft.Y = m_instructionsButton.m_texturePosition.Y - 16;

                        m_indicatorRight.X = m_instructionsButton.m_texturePosition.X + m_instructionsButton.m_rectangle.Width / 2;
                        m_indicatorRight.Y = m_instructionsButton.m_texturePosition.Y + 16;
                    }

                    else if (m_showGamePadControls)
                    {
                        m_showGamePadControls = false;

                        m_buttonSelected = 13;

                        m_indicatorLeft.X = m_controlsButton.m_texturePosition.X - m_controlsButton.m_rectangle.Width / 2;
                        m_indicatorLeft.Y = m_controlsButton.m_texturePosition.Y - 16;

                        m_indicatorRight.X = m_controlsButton.m_texturePosition.X + m_controlsButton.m_rectangle.Width / 2;
                        m_indicatorRight.Y = m_controlsButton.m_texturePosition.Y + 16;
                    }

                    else if (m_showKeyboardControls)
                    {
                        m_showKeyboardControls = false;

                        m_buttonSelected = 13;

                        m_indicatorLeft.X = m_controlsButton.m_texturePosition.X - m_controlsButton.m_rectangle.Width / 2;
                        m_indicatorLeft.Y = m_controlsButton.m_texturePosition.Y - 16;

                        m_indicatorRight.X = m_controlsButton.m_texturePosition.X + m_controlsButton.m_rectangle.Width / 2;
                        m_indicatorRight.Y = m_controlsButton.m_texturePosition.Y + 16;
                    }

                    else if (m_showVitaControls)
                    {
                        m_showVitaControls = false;

                        m_buttonSelected = 13;

                        m_indicatorLeft.X = m_controlsButton.m_texturePosition.X - m_controlsButton.m_rectangle.Width / 2;
                        m_indicatorLeft.Y = m_controlsButton.m_texturePosition.Y - 16;

                        m_indicatorRight.X = m_controlsButton.m_texturePosition.X + m_controlsButton.m_rectangle.Width / 2;
                        m_indicatorRight.Y = m_controlsButton.m_texturePosition.Y + 16;
                    }

                    else if (m_showEnemies)
                    {
                        m_showEnemies = false;

                        m_buttonSelected = 8;

                        m_indicatorLeft.X = m_enemiesButton.m_texturePosition.X - m_enemiesButton.m_rectangle.Width / 2;
                        m_indicatorLeft.Y = m_enemiesButton.m_texturePosition.Y - 16;

                        m_indicatorRight.X = m_enemiesButton.m_texturePosition.X + m_enemiesButton.m_rectangle.Width / 2;
                        m_indicatorRight.Y = m_enemiesButton.m_texturePosition.Y + 16;
                    }

                    else if (m_showPowerups)
                    {
                        m_showPowerups = false;

                        m_buttonSelected = 11;

                        m_indicatorLeft.X = m_powerupsButton.m_texturePosition.X - m_powerupsButton.m_rectangle.Width / 2;
                        m_indicatorLeft.Y = m_powerupsButton.m_texturePosition.Y - 16;

                        m_indicatorRight.X = m_powerupsButton.m_texturePosition.X + m_powerupsButton.m_rectangle.Width / 2;
                        m_indicatorRight.Y = m_powerupsButton.m_texturePosition.Y + 16;
                    }

                }

                else if (InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.B) && m_buttonSelected != 14)
                {
                    m_blipSound.Play(0.001f, 0.4f, 0.4f);

                    m_buttonSelected = 0;

                    m_indicatorLeft.X = m_redUpButton.m_texturePosition.X - m_redUpButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_redUpButton.m_texturePosition.Y - 8;

                    m_indicatorRight.X = m_redUpButton.m_texturePosition.X + m_redUpButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_redUpButton.m_texturePosition.Y + 8;

                    Globals.m_volume = m_options.m_volume;
                    m_options.SaveOptions();
                    //Globals.m_fullscreen = m_options.m_fullscreen;
                    AIE.GameStateManager.PopState();
                    AIE.GameStateManager.PushState("MENU");
                }


                m_imageJustOverlayed = false;
            #endregion

            }                        
            #endregion

            #region MOUSE


            if (m_redUpButton.IsButtonDown())
            {
                m_red += 0.02f;

                if (m_redUpButton.IsButtonClicked())
                {
                    m_blipSound.Play(0.001f * Globals.m_volume, 0.75f, 0.75f);
                }
            }

            else if (m_redDownButton.IsButtonDown())
            {
                m_red -= 0.02f;

                if (m_redDownButton.IsButtonClicked())
                {
                    m_blipSound.Play(0.001f * Globals.m_volume, 0.75f, 0.75f);
                }
            }

            else if (m_greenUpButton.IsButtonDown())
            {
                m_green += 0.02f;

                if (m_greenUpButton.IsButtonClicked())
                {
                    m_blipSound.Play(0.001f * Globals.m_volume, 0.75f, 0.75f);
                }
            }

            else if (m_greenDownButton.IsButtonDown())
            {
                m_green -= 0.02f;

                if (m_greenDownButton.IsButtonClicked())
                {
                    m_blipSound.Play(0.001f * Globals.m_volume, 0.75f, 0.75f);
                }
            }

            else if (m_blueUpButton.IsButtonDown())
            {
                m_blue += 0.02f;

                if (m_blueUpButton.IsButtonClicked())
                {
                    m_blipSound.Play(0.001f * Globals.m_volume, 0.75f, 0.75f);
                }
            }

            else if (m_blueDownButton.IsButtonDown())
            {
                m_blue -= 0.02f;

                if (m_blueDownButton.IsButtonClicked())
                {
                    m_blipSound.Play(0.001f * Globals.m_volume, 0.75f, 0.75f);
                }
            }            

            else if (m_volumeUpButton.IsButtonDown())
            {
                m_options.m_volume += 0.02f;

                if (m_volumeUpButton.IsButtonClicked())
                {
                    m_blipSound.Play(0.001f * Globals.m_volume, 0.75f, 0.75f);
                }
            }

            else if (m_volumeDownButton.IsButtonDown())
            {
                m_options.m_volume -= 0.02f;

                if (m_volumeDownButton.IsButtonClicked())
                {
                    m_blipSound.Play(0.001f * Globals.m_volume, 0.75f, 0.75f);
                }
            }

            //else if (m_fullscreenButton.IsButtonClicked())
            //{
            //    m_fullscreen = !m_fullscreen;
            //    m_blipSound.Play(0.001f, 0.75f, 0.75f);
            //}

            else if (m_backButton.IsButtonClicked() && !(m_showEnemies || m_showGamePadControls || m_showInstructions || m_showKeyboardControls || m_showPowerups || m_showVitaControls) )
            {
                m_blipSound.Play(0.001f * Globals.m_volume, 0.75f, 0.75f);
                m_options.SaveOptions();
                Globals.m_fullscreen = m_options.m_fullscreen;
                AIE.GameStateManager.PopState();
                AIE.GameStateManager.PushState("MENU");
            }


            else if (m_backButton.IsButtonClicked() && (m_showEnemies || m_showGamePadControls || m_showInstructions || m_showKeyboardControls || m_showPowerups || m_showVitaControls))
            {
                m_blipSound.Play(0.001f * Globals.m_volume, 0.75f, 0.75f);

                if (m_showInstructions)
                {
                    m_showInstructions = false;

                    m_buttonSelected = 3;

                    m_indicatorLeft.X = m_instructionsButton.m_texturePosition.X - m_instructionsButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_instructionsButton.m_texturePosition.Y - 16;

                    m_indicatorRight.X = m_instructionsButton.m_texturePosition.X + m_instructionsButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_instructionsButton.m_texturePosition.Y + 16;
                }

                else if (m_showGamePadControls)
                {
                    m_showGamePadControls = false;

                    m_buttonSelected = 13;

                    m_indicatorLeft.X = m_controlsButton.m_texturePosition.X - m_controlsButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_controlsButton.m_texturePosition.Y - 16;

                    m_indicatorRight.X = m_controlsButton.m_texturePosition.X + m_controlsButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_controlsButton.m_texturePosition.Y + 16;
                }

                else if (m_showKeyboardControls)
                {
                    m_showKeyboardControls = false;

                    m_buttonSelected = 13;

                    m_indicatorLeft.X = m_controlsButton.m_texturePosition.X - m_controlsButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_controlsButton.m_texturePosition.Y - 16;

                    m_indicatorRight.X = m_controlsButton.m_texturePosition.X + m_controlsButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_controlsButton.m_texturePosition.Y + 16;
                }

                else if (m_showVitaControls)
                {
                    m_showVitaControls = false;

                    m_buttonSelected = 13;

                    m_indicatorLeft.X = m_controlsButton.m_texturePosition.X - m_controlsButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_controlsButton.m_texturePosition.Y - 16;

                    m_indicatorRight.X = m_controlsButton.m_texturePosition.X + m_controlsButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_controlsButton.m_texturePosition.Y + 16;
                }

                else if (m_showEnemies)
                {
                    m_showEnemies = false;

                    m_buttonSelected = 8;

                    m_indicatorLeft.X = m_enemiesButton.m_texturePosition.X - m_enemiesButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_enemiesButton.m_texturePosition.Y - 16;

                    m_indicatorRight.X = m_enemiesButton.m_texturePosition.X + m_enemiesButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_enemiesButton.m_texturePosition.Y + 16;
                }

                else if (m_showPowerups)
                {
                    m_showPowerups = false;

                    m_buttonSelected = 11;

                    m_indicatorLeft.X = m_powerupsButton.m_texturePosition.X - m_powerupsButton.m_rectangle.Width / 2;
                    m_indicatorLeft.Y = m_powerupsButton.m_texturePosition.Y - 16;

                    m_indicatorRight.X = m_powerupsButton.m_texturePosition.X + m_powerupsButton.m_rectangle.Width / 2;
                    m_indicatorRight.Y = m_powerupsButton.m_texturePosition.Y + 16;
                }
            }

            else if (m_instructionsButton.IsButtonClicked())
            {
                m_blipSound.Play(0.001f, 0.75f, 0.75f);

                m_showInstructions = true;
                m_imageJustOverlayed = true;
                m_buttonSelected = 14;

                m_indicatorLeft.X = m_backButton.m_texturePosition.X - m_backButton.m_rectangle.Width / 2;
                m_indicatorLeft.Y = m_backButton.m_texturePosition.Y - 16;

                m_indicatorRight.X = m_backButton.m_texturePosition.X + m_backButton.m_rectangle.Width / 2;
                m_indicatorRight.Y = m_backButton.m_texturePosition.Y + 16;
            }

            else if (m_enemiesButton.IsButtonClicked())
            {
                m_blipSound.Play(0.001f, 0.75f, 0.75f);

                m_showEnemies = true;
                m_imageJustOverlayed = true;
                m_buttonSelected = 14;

                m_indicatorLeft.X = m_backButton.m_texturePosition.X - m_backButton.m_rectangle.Width / 2;
                m_indicatorLeft.Y = m_backButton.m_texturePosition.Y - 16;

                m_indicatorRight.X = m_backButton.m_texturePosition.X + m_backButton.m_rectangle.Width / 2;
                m_indicatorRight.Y = m_backButton.m_texturePosition.Y + 16;
            }

            else if (m_powerupsButton.IsButtonClicked())
            {
                m_blipSound.Play(0.001f * Globals.m_volume, 0.75f, 0.75f);

                m_showPowerups = true;
                m_imageJustOverlayed = true;
                m_buttonSelected = 14;

                m_indicatorLeft.X = m_backButton.m_texturePosition.X - m_backButton.m_rectangle.Width / 2;
                m_indicatorLeft.Y = m_backButton.m_texturePosition.Y - 16;

                m_indicatorRight.X = m_backButton.m_texturePosition.X + m_backButton.m_rectangle.Width / 2;
                m_indicatorRight.Y = m_backButton.m_texturePosition.Y + 16;
            }

            else if (m_controlsButton.IsButtonClicked())
            {
                m_blipSound.Play(0.001f * Globals.m_volume, 0.75f, 0.75f);

                m_showKeyboardControls = true;
                m_imageJustOverlayed = true;
                m_buttonSelected = 14;

                m_indicatorLeft.X = m_backButton.m_texturePosition.X - m_backButton.m_rectangle.Width / 2;
                m_indicatorLeft.Y = m_backButton.m_texturePosition.Y - 16;

                m_indicatorRight.X = m_backButton.m_texturePosition.X + m_backButton.m_rectangle.Width / 2;
                m_indicatorRight.Y = m_backButton.m_texturePosition.Y + 16;
            }



            #endregion
            
            #region MOVING BACKGROUND
            if (Globals.m_backgroundRectangle.X + 1 >= 960)
            {
                Globals.m_backgroundRectangle = new Rectangle(-960, 0, 960, 544);
            }

            else
            {
                Globals.m_backgroundRectangle = new Rectangle(Globals.m_backgroundRectangle.X + 1, 0, 960, 544);
            }

            if (Globals.m_backgroundRectangle2.X + 1 >= 960)
            {
                Globals.m_backgroundRectangle2 = new Rectangle(-960, 0, 960, 544);
            }

            else
            {
                Globals.m_backgroundRectangle2 = new Rectangle(Globals.m_backgroundRectangle2.X + 1, 0, 960, 544);
            }

            #endregion

            m_options.m_volume = Math.Min(m_options.m_volume, 1.0f);
            m_options.m_volume = Math.Max(m_options.m_volume, 0.0f);

            Globals.m_volume = m_options.m_volume;
                                  
            m_playerColour = new Color(m_red, m_green, m_blue);
                        
            m_options.m_red = m_red;
            m_options.m_green = m_green;
            m_options.m_blue = m_blue;
            //m_options.m_fullscreen = m_fullscreen;


        }

        public override void Draw(GameTime gT, SpriteBatch sb)
        {
            sb.Begin();

            sb.Draw(m_backgroundSprite, Globals.m_backgroundRectangle, Color.White);
            sb.Draw(m_backgroundSprite, Globals.m_backgroundRectangle2, Color.White);

            sb.Draw(m_playerColourButtonTexture, new Vector2(150, 200), Color.White);
            sb.Draw(m_volumeButtonTexture, new Vector2(150, 350), Color.White);
            //sb.Draw(m_fullscreenButtonTexture, new Vector2(150, 415), Color.White);

            sb.Draw(m_titleTexture, new Rectangle(10, -10, 960, 150), Color.White);

            m_redUpButton.Draw(sb, Color.Red);
            m_redDownButton.Draw(sb, Color.Red, (float)Math.PI, 1.0f);
            m_greenUpButton.Draw(sb, Color.Green);
            m_greenDownButton.Draw(sb, Color.Green, (float)Math.PI, 1.0f);
            m_blueUpButton.Draw(sb, Color.Blue);
            m_blueDownButton.Draw(sb, Color.Blue, (float)Math.PI, 1.0f);
            m_volumeUpButton.Draw(sb);
            m_volumeDownButton.Draw(sb, Color.White, (float)Math.PI, 1.0f);
            //m_fullscreenButton.Draw(sb);
            m_instructionsButton.Draw(sb);
            m_enemiesButton.Draw(sb); 
            m_powerupsButton.Draw(sb);
            m_backButton.Draw(sb);
            m_controlsButton.Draw(sb);

            sb.Draw(m_indicator, m_indicatorLeft, new Rectangle(0, 0, 23, 20), Color.White, 0.0f, new Vector2( 12, 10), 0.5f, SpriteEffects.None, 0.0f);
            sb.Draw(m_indicator, m_indicatorRight, new Rectangle(0, 0, 23, 20), Color.White, (float)Math.PI, new Vector2(12, 10), 0.5f, SpriteEffects.None, 0.0f);    

            sb.Draw(m_playerGlowTexture, new Vector2(385, 210), new Rectangle(0, 0, 64, 64), m_playerColour, 0.0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0.0f);    
            sb.Draw(m_playerTexture, new Vector2(385, 210), new Rectangle(0, 0, 64, 64), Color.White, 0.0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0.0f);

            int volumePercent = (int)(m_options.m_volume * 100);

            sb.DrawString(Globals.m_defaultFont, volumePercent.ToString(), new Vector2(390, 345), Color.White);

            if (m_showEnemies)
            {
                sb.Draw(m_enemiesInformation, new Vector2(75, 20), new Rectangle(0, 0, 900, 484), Color.White, 0.0f, Vector2.Zero, 0.9f, SpriteEffects.None, 0.0f);
            }

            else if (m_showKeyboardControls)
            {
                sb.Draw(m_pcControls, new Vector2(75, 20), new Rectangle(0, 0, 900, 484), Color.White, 0.0f, Vector2.Zero, 0.9f, SpriteEffects.None, 0.0f);
            }

            else if (m_showVitaControls)
            {
                sb.Draw(m_vitaControls, new Vector2(75, 20), new Rectangle(0, 0, 900, 484), Color.White, 0.0f, Vector2.Zero, 0.9f, SpriteEffects.None, 0.0f);
            }

            else if (m_showGamePadControls)
            {
                sb.Draw(m_gamePadControls, new Vector2(75, 20), new Rectangle(0, 0, 900, 484), Color.White, 0.0f, Vector2.Zero, 0.9f, SpriteEffects.None, 0.0f);
            }

            else if (m_showPowerups)
            {
                sb.Draw(m_powerupInformation, new Vector2(75, 20), new Rectangle(0, 0, 900, 484), Color.White, 0.0f, Vector2.Zero, 0.9f, SpriteEffects.None, 0.0f);
            }

            else if (m_showInstructions)
            {
                sb.Draw(m_instructionsInformation, new Vector2(75, 20), new Rectangle(0, 0, 900, 484), Color.White, 0.0f, Vector2.Zero, 0.9f, SpriteEffects.None, 0.0f);
            }            
            
            sb.End();
        }

    }

    public class OptionsObject
    {
        public float m_red {get; set;}
        public float m_green {get; set;}
        public float m_blue {get; set;}
        public float m_volume {get; set;}
        public bool m_fullscreen {get; set;}

        public OptionsObject()
        {
            m_red = 0f;
            m_green = 0f;
            m_blue = 0f;
            m_volume = 0.5f;
            m_fullscreen = false;
        }

        public void LoadOptions()
        {
            
#if PSM			
            if (File.Exists("Documents/Options.emu"))
            {
                FileStream filestream = File.OpenRead("Documents/Options.emu");

                BinaryReader bin = new BinaryReader(filestream);

                m_red = bin.ReadSingle();
                m_green = bin.ReadSingle();
                m_blue = bin.ReadSingle();

                m_volume = bin.ReadSingle();

                m_fullscreen = bin.ReadBoolean();

                bin.Close();
            }

            else
            {
                SaveOptions();
                LoadOptions();
            }
#else
            if (File.Exists("Options.emu"))
            {
                FileStream filestream = File.OpenRead("Options.emu");

                BinaryReader bin = new BinaryReader(filestream);

                m_red = bin.ReadSingle();
                m_green = bin.ReadSingle();
                m_blue = bin.ReadSingle();

                m_volume = bin.ReadSingle();

                m_fullscreen = bin.ReadBoolean();

                bin.Close();
            }

            else
            {
                SaveOptions();
                LoadOptions();
            }

#endif
        }

        public void SaveOptions()
        {            
#if PSM
            FileStream filestream = File.OpenWrite("Documents/Options.emu");
#else
			FileStream filestream = File.OpenWrite("Options.emu");
#endif
            BinaryWriter bin = new BinaryWriter(filestream);

            bin.Write(m_red);
            bin.Write(m_green);
            bin.Write(m_blue);
            
            bin.Write(m_volume);

            bin.Write(m_fullscreen);

            bin.Close();
        
        }
    }

}
