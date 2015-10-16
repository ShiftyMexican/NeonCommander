#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
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
        public static Vector2 m_mousePosition { get; set; }
        #endregion

        public static Random m_rng { get; set; }
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

        public Game1()
            : base()
        {
            #region GRAPHICS AND CONTENT
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 544;
            graphics.PreferredBackBufferWidth = 960;
            graphics.ApplyChanges();
            this.Window.Title = "Test Game";
            Globals.m_windowHeight = graphics.PreferredBackBufferHeight;
            Globals.m_windowWidth = graphics.PreferredBackBufferWidth;
            Content.RootDirectory = "Content";
            #endregion

            Globals.m_rng = new Random();

            Globals.m_mousePosition = new Vector2(0, 0);
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

            #region CONSTANT FONTS AND SPRITES
            Globals.m_defaultFont = Content.Load<SpriteFont>("Fonts/SpriteFont1");
            #endregion

            Particles.ParticleManager.Initialise(spriteBatch, GraphicsDevice);

            // TODO: use this.Content to load your game content here
            AIE.GameStateManager.SetState("CONCEPT", new Concept());
            AIE.GameStateManager.PushState("CONCEPT");
            
            
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

            Globals.m_mousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            AIE.GameStateManager.UpdateGameStates(gameTime);
            

            // TODO: Add your update logic here

            Particles.ParticleManager.Update(gameTime);
            base.Update(gameTime);
            InputManager.InputManager.UpdateInputs();
        }
                        

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            

            // TODO: Add your drawing code here
            AIE.GameStateManager.DrawGameStates(gameTime, spriteBatch);

            Particles.ParticleManager.Begin();
            Particles.ParticleManager.Draw();
            Particles.ParticleManager.End();

            base.Draw(gameTime);
        }
    }
}
