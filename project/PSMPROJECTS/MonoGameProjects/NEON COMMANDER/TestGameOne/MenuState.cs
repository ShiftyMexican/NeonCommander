#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
using TestGameOne;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TestGameOne
{
    public class MenuState : AIE.IGameState
    {
        private Texture2D m_backgroundSprite;

        private Texture2D m_cursorSprite;
        private Texture2D m_playTexture;
        private Texture2D m_helpAndOptionTexture;
        private Texture2D m_title;
        private Texture2D m_leaderboardsButtonTexture;
        private Texture2D m_exitButtonTexture;
        private Texture2D m_indicatorTexture;

        private SoundEffect m_blipSound;

        private Vector2 m_indicatorLeftPosition;
        private Vector2 m_indicatorRightPosition;

        private Button m_playButton;
        private Button m_helpButton;
        private Button m_leaderboardButton;
        private Button m_exitButton;


        Game m_game;

        private int m_buttonSelected; //0 is play, 1 is help and options

        public MenuState(Game game)
            : base()
        {

            m_game = game;
            m_backgroundSprite = content.Load<Texture2D>("Images/MenuBackground");
            m_cursorSprite = content.Load<Texture2D>("Images/arrows");
            m_title = content.Load<Texture2D>("Images/NeonCommanderTitle");
            
            m_playTexture = AIE.GameStateManager.Game.Content.Load<Texture2D>("Images/PlayButton");
            m_helpAndOptionTexture = AIE.GameStateManager.Game.Content.Load<Texture2D>("Images/HelpAndOptions");
            m_leaderboardsButtonTexture = content.Load<Texture2D>("Images/Leaderboards");
            m_exitButtonTexture = content.Load<Texture2D>("Images/ExitButton");
            m_indicatorTexture = content.Load<Texture2D>("Images/LeftSelector");

            m_blipSound = content.Load<SoundEffect>("Sounds/Blip");
            
            m_playButton = new Button(480, 190, 100, 50, m_playTexture);
            m_helpButton = new Button(480, 290, 300, 50, m_helpAndOptionTexture);
            m_leaderboardButton = new Button(480, 400, 220, 50, m_leaderboardsButtonTexture);
            m_exitButton = new Button(480, 500, 100, 50, m_exitButtonTexture);

            m_indicatorLeftPosition = new Vector2();
            m_indicatorRightPosition = new Vector2();

            m_buttonSelected = 0;
            
        }

        public override void Update(GameTime gT)
        {
            //Button management
            m_playButton.Update(gT);
            m_helpButton.Update(gT);
            m_leaderboardButton.Update(gT);
            m_exitButton.Update(gT);

            #region GAMEPAD/KEYBOARD RELATED
            
            if (InputManager.InputManager.IsKeyJustPressed(Keys.Down) ||
                InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.LeftThumbstickDown) && GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < 0 || InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.DPadDown))
            {
                m_buttonSelected += 1;
                m_blipSound.Play(0.01f * Globals.m_volume, 0.4f, 0.4f);
            }

            if (InputManager.InputManager.IsKeyJustPressed(Keys.Up) ||
                InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.LeftThumbstickUp) && GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0 || InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.DPadUp))
            {
                m_buttonSelected -= 1;
                m_blipSound.Play(0.01f * Globals.m_volume, 0.4f, 0.4f);
            }
            
            if (m_buttonSelected > 3)
                m_buttonSelected = 0;

            if (m_buttonSelected < 0)
                m_buttonSelected = 3;

            if (m_buttonSelected == 0)
            {
                m_playButton.ButtonSelected();
                m_helpButton.ButtonDeselected();
                m_leaderboardButton.ButtonDeselected();
                m_exitButton.ButtonDeselected();

                m_indicatorLeftPosition = new Vector2(m_playButton.m_texturePosition.X - m_playButton.m_rectangle.Width / 2, m_playButton.m_texturePosition.Y);
                m_indicatorRightPosition = new Vector2(m_playButton.m_texturePosition.X + m_playButton.m_rectangle.Width / 2, m_playButton.m_texturePosition.Y);

                if (m_playButton.IsButtonClicked() || (m_playButton.KeyboardInputGiven() && InputManager.InputManager.IsKeyJustPressed(Keys.Enter)))
                {
                    m_blipSound.Play(0.01f * Globals.m_volume, 0.75f, 0.75f);
                    AIE.GameStateManager.PopState();
                    AIE.GameStateManager.SetState("PLAYSTATE", new PlayState());
                    AIE.GameStateManager.PushState("PLAYSTATE");
                }
            }
            else if (m_buttonSelected == 1)
            {
                m_playButton.ButtonDeselected();
                m_helpButton.ButtonSelected();
                m_leaderboardButton.ButtonDeselected();
                m_exitButton.ButtonDeselected();

                m_indicatorLeftPosition = new Vector2(m_helpButton.m_texturePosition.X - m_helpButton.m_rectangle.Width / 2, m_helpButton.m_texturePosition.Y);
                m_indicatorRightPosition = new Vector2(m_helpButton.m_texturePosition.X + m_helpButton.m_rectangle.Width / 2, m_helpButton.m_texturePosition.Y);

                if (m_helpButton.IsButtonClicked() || (m_helpButton.KeyboardInputGiven() && InputManager.InputManager.IsKeyJustPressed(Keys.Enter)))
                {
                    m_blipSound.Play(0.01f * Globals.m_volume, 0.75f, 0.75f);
                    AIE.GameStateManager.PopState();
                    AIE.GameStateManager.PushState("HELP");
                }

         
            }
            else if (m_buttonSelected == 2)
            {
                m_playButton.ButtonDeselected();
                m_helpButton.ButtonDeselected();
                m_leaderboardButton.ButtonSelected();
                m_exitButton.ButtonDeselected();

                m_indicatorLeftPosition = new Vector2(m_leaderboardButton.m_texturePosition.X - m_leaderboardButton.m_rectangle.Width / 2, m_leaderboardButton.m_texturePosition.Y);
                m_indicatorRightPosition = new Vector2(m_leaderboardButton.m_texturePosition.X + m_leaderboardButton.m_rectangle.Width / 2, m_leaderboardButton.m_texturePosition.Y);

                if (m_leaderboardButton.IsButtonClicked() || (m_leaderboardButton.KeyboardInputGiven() && InputManager.InputManager.IsKeyJustPressed(Keys.Enter)))
                {
                    m_blipSound.Play(0.01f * Globals.m_volume, 0.75f, 0.75f);
                    AIE.GameStateManager.PopState();
                    AIE.GameStateManager.SetState("LEADERBOARD", new Leaderboards());
                    AIE.GameStateManager.PushState("LEADERBOARD");
                }
            
            }
            else if (m_buttonSelected == 3)
            {
                m_playButton.ButtonDeselected();
                m_helpButton.ButtonDeselected();
                m_leaderboardButton.ButtonDeselected();
                m_exitButton.ButtonSelected();

                m_indicatorLeftPosition = new Vector2(m_exitButton.m_texturePosition.X - m_exitButton.m_rectangle.Width / 2, m_exitButton.m_texturePosition.Y);
                m_indicatorRightPosition = new Vector2(m_exitButton.m_texturePosition.X + m_exitButton.m_rectangle.Width / 2, m_exitButton.m_texturePosition.Y);

                if (m_exitButton.IsButtonClicked() || (m_exitButton.KeyboardInputGiven() && InputManager.InputManager.IsKeyJustPressed(Keys.Enter)))
                {
                    m_blipSound.Play(0.01f * Globals.m_volume, 0.75f, 0.75f);
                    m_game.Exit();
                }
            }
            #endregion

            #region MOUSE RELATED

            if (m_playButton.IsButtonClicked())
            {
                m_blipSound.Play(0.01f * Globals.m_volume, 0.75f, 0.75f);
                AIE.GameStateManager.PopState();
                AIE.GameStateManager.PushState("PLAYSTATE");
            }

            else if (m_helpButton.IsButtonClicked())
            {
                m_blipSound.Play(0.01f * Globals.m_volume, 0.75f, 0.75f);
                AIE.GameStateManager.PopState();
                AIE.GameStateManager.PushState("HELP");
            }

            else if (m_leaderboardButton.IsButtonClicked())
            {
                m_blipSound.Play(0.01f * Globals.m_volume, 0.75f, 0.75f);
                AIE.GameStateManager.PopState();
                AIE.GameStateManager.PushState("LEADERBOARD");
            }

            else if (m_exitButton.IsButtonClicked())
            {
                m_blipSound.Play(0.01f * Globals.m_volume, 0.75f, 0.75f);
                m_game.Exit();
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

        }

        public override void Draw(GameTime gT, SpriteBatch sb)
        {
            sb.Begin();

            sb.Draw(m_backgroundSprite, Globals.m_backgroundRectangle, Color.White);
            sb.Draw(m_backgroundSprite, Globals.m_backgroundRectangle2, Color.White);

            sb.Draw(m_title, new Rectangle(5, -10, 955, 150), Color.White);

            m_playButton.Draw(sb);
            m_helpButton.Draw(sb);
            m_leaderboardButton.Draw(sb);
            m_exitButton.Draw(sb);

            sb.Draw(m_indicatorTexture, m_indicatorLeftPosition, new Rectangle(0, 0, 23, 58), Color.White, 0.0f, new Vector2(12, 29), 1.0f, SpriteEffects.None, 0.0f);
            sb.Draw(m_indicatorTexture, m_indicatorRightPosition, new Rectangle(0, 0, 23, 58), Color.White, (float)Math.PI, new Vector2(12, 29), 1.0f, SpriteEffects.None, 0.0f);
            
            sb.Draw(m_cursorSprite, Globals.m_mousePosition, Color.Violet);
            sb.End();
        }
    }
}
