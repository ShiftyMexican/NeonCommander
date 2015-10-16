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
    public class Leaderboards : AIE.IGameState
    {
        private Texture2D m_titleTexture;
        private Texture2D m_backgroundSprite;
        private Texture2D m_backButtonTexture;
        private Texture2D m_selectorTexture;
        private Vector2 m_indicatorLeftPosition;
        private Vector2 m_indicatorRightPosition;

        private HighScoresObject m_highScores;
        private Button m_backButton;

        public Leaderboards()
            : base()
        {
            m_titleTexture = content.Load<Texture2D>("Images/LeaderboardsTitle");
            m_backgroundSprite = content.Load<Texture2D>("Images/MenuBackground");
            m_backButtonTexture = content.Load<Texture2D>("Images/BackText");
            m_selectorTexture = content.Load<Texture2D>("Images/LeftSelector");

            m_highScores = new HighScoresObject();
            m_highScores.LoadHighScores();
            m_indicatorLeftPosition = new Vector2(418, 498);
            m_indicatorRightPosition = new Vector2(518, 498);
            m_backButton = new Button(468, 498, 100, 40, m_backButtonTexture);
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.A) || InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.B) || InputManager.InputManager.IsKeyJustPressed(Keys.Enter) || m_backButton.IsButtonClicked())
            {
                AIE.GameStateManager.PopState();
                AIE.GameStateManager.PushState("MENU");
            }

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

            m_backButton.Update(gameTime);
        }


        public override void Draw(GameTime gT, SpriteBatch sb)
        {
            sb.Begin();

            sb.Draw(m_backgroundSprite, Globals.m_backgroundRectangle, Color.White);
            sb.Draw(m_backgroundSprite, Globals.m_backgroundRectangle2, Color.White);
            sb.Draw(m_titleTexture, new Rectangle(0, -10, 955, 150), Color.White);

            for (int i = 0; i < m_highScores.m_highScoresList.Count; i++)
            {
                sb.DrawString(Globals.m_defaultFont, m_highScores.m_highScoresList[i].m_name, new Vector2(350, (i * 35) + 125), Color.White, 0f, Vector2.Zero, 0.4f, SpriteEffects.None, 0.0f);
                sb.DrawString(Globals.m_defaultFont, m_highScores.m_highScoresList[i].m_score.ToString(), new Vector2(550, (i * 35) + 125), Color.White, 0f, Vector2.Zero, 0.4f, SpriteEffects.None, 0.0f);
            }


            sb.Draw(m_selectorTexture, m_indicatorLeftPosition, new Rectangle(0, 0, 23, 58), Color.White, 0.0f, new Vector2(12, 29), 0.80f, SpriteEffects.None, 0.0f);
            sb.Draw(m_selectorTexture, m_indicatorRightPosition, new Rectangle(0, 0, 23, 58), Color.White, (float)Math.PI, new Vector2(12, 29), 0.80f, SpriteEffects.None, 0.0f);

            m_backButton.Draw(sb);
            sb.End();
        }

    }
}
