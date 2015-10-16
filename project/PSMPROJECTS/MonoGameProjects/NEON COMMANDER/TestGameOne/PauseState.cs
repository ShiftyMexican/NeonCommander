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
    public class PauseState : AIE.IGameState
    {
        private Texture2D m_backgroundTexture;
        private Texture2D m_title;
        private Texture2D m_resumeButtonTexture;
        private Texture2D m_exitButtonTexture;
        private Texture2D m_indicatorTexture;

        private Button m_resumeButton;
        private Button m_exitButton;

        private Vector2 m_indicatorLeftPosition;
        private Vector2 m_indicatorRightPosition;

        private SoundEffect m_blipSound;

        private int m_buttonSelected;

        public PauseState()
            : base()
        {
            m_backgroundTexture = content.Load<Texture2D>("Images/LevelBackground");
            m_title = content.Load<Texture2D>("Images/Paused");
            m_resumeButtonTexture = content.Load<Texture2D>("Images/ResumeText");
            m_exitButtonTexture = content.Load<Texture2D>("Images/MainMenuText");
            m_indicatorTexture = content.Load<Texture2D>("Images/LeftSelector");

            m_resumeButton = new Button(480, 350, 150, 50, m_resumeButtonTexture);
            m_exitButton = new Button(480, 450, 220, 50, m_exitButtonTexture);

            m_indicatorLeftPosition = new Vector2();
            m_indicatorRightPosition = new Vector2();

            m_blipSound = content.Load<SoundEffect>("Sounds/Blip");

            m_buttonSelected = 0;
        }

        public override void Update(GameTime gT)
        {
            m_resumeButton.Update(gT);
            m_exitButton.Update(gT);

            #region GAMEPAD/KEYBOARD RELATED
            
            
            if (InputManager.InputManager.IsKeyJustPressed(Keys.Down) ||
                InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.LeftThumbstickDown) && GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < 0 || InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.DPadDown))
            {
                if (m_buttonSelected == 0)
                {
                    m_buttonSelected += 1;
                    m_blipSound.Play(0.1f * Globals.m_volume, 0.4f, 0.4f);
                }
            }

            if (InputManager.InputManager.IsKeyJustPressed(Keys.Up) ||
                InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.LeftThumbstickUp) && GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0 || InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.DPadUp))
            {
                if (m_buttonSelected == 1)
                {
                    m_buttonSelected -= 1;
                    m_blipSound.Play(0.1f * Globals.m_volume, 0.4f, 0.4f);
                } 
            }
            
            if (m_buttonSelected == 0)
            {
                m_indicatorLeftPosition = new Vector2(m_resumeButton.m_texturePosition.X - m_resumeButton.m_rectangle.Width / 2, m_resumeButton.m_texturePosition.Y - 8.0f);
                m_indicatorRightPosition = new Vector2(m_resumeButton.m_texturePosition.X + m_resumeButton.m_rectangle.Width / 2, m_resumeButton.m_texturePosition.Y - 8.0f);
                
                //Resume button clicked
                if (InputManager.InputManager.IsKeyJustPressed(Keys.Enter) || InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.A))
                {
                    m_blipSound.Play(0.1f * Globals.m_volume, 0.75f, 0.75f);
                    AIE.GameStateManager.PopState();
                    AIE.GameStateManager.SetFreezeUpdate(false);
                }
            }

            else if (m_buttonSelected == 1)
            {
                m_indicatorLeftPosition = new Vector2(m_exitButton.m_texturePosition.X - m_exitButton.m_rectangle.Width / 2, m_exitButton.m_texturePosition.Y - 8.0f);
                m_indicatorRightPosition = new Vector2(m_exitButton.m_texturePosition.X + m_exitButton.m_rectangle.Width / 2, m_exitButton.m_texturePosition.Y - 8.0f);

                //Exit button clicked
                if (InputManager.InputManager.IsKeyJustPressed(Keys.Enter) || InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.A))
                {
                    m_blipSound.Play(0.1f * Globals.m_volume, 0.4f, 0.4f);
                    AIE.GameStateManager.PopState();
                    AIE.GameStateManager.PopState();
                    AIE.GameStateManager.PushState("MENU");
                }
            }

            if (m_exitButton.IsButtonClicked())
            {
                m_blipSound.Play(0.1f * Globals.m_volume, 0.4f, 0.4f);
                AIE.GameStateManager.PopState();
                AIE.GameStateManager.PopState();
                AIE.GameStateManager.PushState("MENU");
            }

            if (m_resumeButton.IsButtonClicked())
            {
                m_blipSound.Play(0.1f * Globals.m_volume, 0.75f, 0.75f);
                AIE.GameStateManager.PopState();
                AIE.GameStateManager.SetFreezeUpdate(false);
            }
            #endregion

        }

        public override void Draw(GameTime gT, SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(m_title, new Vector2(0, 0), Color.White);
            sb.Draw(m_indicatorTexture, m_indicatorLeftPosition, new Rectangle(0, 0, 23, 58), Color.White, 0.0f, new Vector2(12, 29), 1.0f, SpriteEffects.None, 0.0f);
            sb.Draw(m_indicatorTexture, m_indicatorRightPosition, new Rectangle(0, 0, 23, 58), Color.White, (float)Math.PI, new Vector2(12, 29), 1.0f, SpriteEffects.None, 0.0f);

            m_resumeButton.Draw(sb);
            m_exitButton.Draw(sb);

            sb.End();
        }

    }
}
