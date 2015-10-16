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
using TestGameOne;
#endregion

namespace TestGameOne
{
    public class SplashState : AIE.IGameState         
    {
        Texture2D m_backgroundSprite;
        Texture2D m_cursorSprite;
        Vector2 m_mousePos;
        float m_countDown;


        public SplashState() : base()
        {
            m_backgroundSprite = content.Load<Texture2D>("Images/wallpaper1");
            m_cursorSprite = content.Load<Texture2D>("Images/cursor");
            m_mousePos = new Vector2(0, 0);
            m_countDown = 12.0f;
        }

        public override void Update(GameTime gT)
        {
            m_mousePos = Globals.m_mousePosition;
            m_countDown -= (float)(gT.ElapsedGameTime.TotalSeconds);

            if (m_countDown <= 0)
            {
                AIE.GameStateManager.PopState();
                AIE.GameStateManager.PushState("MENU");
            }
        }

        public override void Draw(GameTime gT, SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(m_backgroundSprite, new Rectangle(0, 0, Globals.m_windowWidth, Globals.m_windowHeight), Color.White);
            sb.Draw(m_cursorSprite, m_mousePos, Color.White);            
            sb.DrawString(Globals.m_defaultFont, "THIS IS THE SPLASH STATE\nPREPARE TO BE ASSIMILATED", new Vector2(10, 10), Color.White);
            sb.End();
        }
    }
}
