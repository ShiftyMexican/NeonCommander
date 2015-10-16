#region Using Statements
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
using AIE;
#endregion


namespace TestGameOne
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>        
    /// 

    public static class Globals
    {    
        #region CONSTANT FONTS AND SPRITES
        public static SpriteFont m_defaultFont { get; set; }
        #endregion

        #region WINDOW RELATED
        public static int m_windowWidth { get; set; }
        public static int m_windowHeight { get; set; }
        public static int m_gameWidth { get; set; }
        public static int m_gameHeight { get; set; }
        public static Vector2 m_mousePosition { get; set; }
        public static Camera2D m_gameCamera { get; set; }
        public static bool m_fullscreen { get; set; }

        public static Rectangle m_backgroundRectangle { get; set; }
        public static Rectangle m_backgroundRectangle2 { get; set; }
        #endregion

        public static Random m_rng { get; set; }
        public static float m_volume { get; set; }
    }

    public static class MathFunctions
    {
        public static float AngleBetween360Degrees( Vector2 lhs, Vector2 rhs)
        {
	        float XLength =  (rhs.X - lhs.X);
	        float YLength =  (rhs.Y - lhs.Y);
	        
	        float absXLength = Math.Abs(XLength);
	        float absYLength = Math.Abs(YLength);

            float fAngle = (float)((Math.PI / 2) - Math.Atan(absYLength / absXLength));

	        //Quadrant 2
	        if( XLength >= 0 && YLength > 0 )
	        {
                fAngle = (float)((Math.PI) - fAngle);
	        }
	        
	        //Quadrant 3
	        else if( XLength < 0 && YLength >= 0 )
	        {
	        	fAngle = (float)(fAngle + (Math.PI));
	        }

	        //Quadrant 4
	        else if( XLength < 0 && YLength < 0 )
	        {
	        	fAngle = (float)((Math.PI * 2) - fAngle);
	        }

            return fAngle;
        }

        public static float AngleBetween180Degrees(Vector2 lhs, Vector2 rhs)
        {
            float XLength = (rhs.X - lhs.X);
            float YLength = (rhs.Y - lhs.Y);

            float absXLength = Math.Abs(XLength);
            float absYLength = Math.Abs(YLength);

            float fAngle = (float)((Math.PI / 2) - Math.Atan(absYLength / absXLength));

            //Quadrant 2
            if (YLength > 0)
            {
                fAngle = (float)((Math.PI) - fAngle);
            }

            return fAngle;
        }   



    }
       
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public SoundEffect m_menuSong;
        public SoundEffect m_gameSong;

        public SoundEffectInstance m_menuSongInstance;
        public SoundEffectInstance m_gameSongInstance;

        private OptionsObject m_options;

        public Game1()
            : base()
        {
            #region GRAPHICS AND CONTENT
            graphics = new GraphicsDeviceManager(this);

            this.graphics.PreferredBackBufferHeight = 544;
            this.graphics.PreferredBackBufferWidth = 960;
            graphics.ApplyChanges();

            this.Window.Title = "Test Game";
            Globals.m_windowHeight = graphics.PreferredBackBufferHeight;
            Globals.m_windowWidth = graphics.PreferredBackBufferWidth;
            Content.RootDirectory = "Content";
            #endregion

            Globals.m_rng = new Random();
            this.IsMouseVisible = false;

            m_options = new OptionsObject();

            Globals.m_mousePosition = new Vector2(0, 0);
            
            Globals.m_gameWidth = 1260;
            Globals.m_gameHeight = 1260;

            Globals.m_backgroundRectangle = new Rectangle(0, 0, 960, 544);
            Globals.m_backgroundRectangle2 = new Rectangle(-960, 0, 960, 544);

            m_options.LoadOptions();

            //Globals.m_fullscreen = m_options.m_fullscreen;
            Globals.m_volume = m_options.m_volume;
            
        }


        
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            

            // TODO: Add your initialization logic here
            AIE.GameStateManager.Initialise(this);
            InputManager.InputManager.InitaliseInputManager();

            //if (Globals.m_fullscreen != m_options.m_fullscreen)
            //{
            //    m_options.m_fullscreen = Globals.m_fullscreen;
            //    graphics.IsFullScreen = m_options.m_fullscreen;
            //}
           
           
            base.Initialize();
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //graphics.IsFullScreen = m_options.m_fullscreen;
            
            #region CONSTANT FONTS AND SPRITES
            Globals.m_defaultFont = Content.Load<SpriteFont>("Fonts/SpriteFont1");
            #endregion
                        
            m_menuSong = Content.Load<SoundEffect>("Sounds/LastStand");
            m_gameSong = Content.Load<SoundEffect>("Sounds/RailJetShortLoop");

            m_menuSongInstance = m_menuSong.CreateInstance();
            m_gameSongInstance = m_gameSong.CreateInstance();

            m_menuSongInstance.IsLooped = true;
            m_gameSongInstance.IsLooped = true;
            
            Globals.m_gameCamera = new Camera2D(GraphicsDevice.Viewport);

            // TODO: use this.Content to load your game content here
            AIE.GameStateManager.SetState("PLAYSTATE", null);
            AIE.GameStateManager.SetState("SPLASH", new SplashState());
            AIE.GameStateManager.SetState("MENU", new MenuState(this));
            AIE.GameStateManager.SetState("HELP", new HelpState());
            AIE.GameStateManager.SetState("LEADERBOARD", null);
            AIE.GameStateManager.SetState("PAUSE", null);
            AIE.GameStateManager.SetState("POSTGAME", new PostGameState(0));
            AIE.GameStateManager.PushState("SPLASH");            
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {      
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Globals.m_volume != m_options.m_volume)
            {
                m_options.m_volume = Globals.m_volume;
            }

            Globals.m_mousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            AIE.GameStateManager.UpdateGameStates(gameTime);
            

            // TODO: Add your update logic here

            base.Update(gameTime);
            InputManager.InputManager.UpdateInputs();

            if (AIE.GameStateManager.IsStateOnTop("PLAYSTATE"))
            {
                m_menuSongInstance.Stop();
                m_gameSongInstance.Volume = 0.05f * m_options.m_volume;
                m_gameSongInstance.Play();
            }
                
            else if (!AIE.GameStateManager.IsStateOnTop("SPLASH"))
            {
                m_gameSongInstance.Stop();
                m_menuSongInstance.Volume = 0.5f * m_options.m_volume;
                m_menuSongInstance.Play();
            }
        }
                        

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            

            // TODO: Add your drawing code here
            AIE.GameStateManager.DrawGameStates(gameTime, spriteBatch);

            base.Draw(gameTime);
        }      
    }
}
