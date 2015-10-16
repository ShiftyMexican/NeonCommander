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
    public class SplashState : AIE.IGameState         
    {
        Vector2 m_mousePos;
        float m_countDown;

        private float m_fade;
        private Texture2D m_emu;

        public SplashState() : base()
        {
            m_mousePos = new Vector2(0, 0);
            m_countDown = 8.0f;

            m_fade = 0.0f;
            m_emu = content.Load<Texture2D>("Images/Dancing-Emu-Productions");
        }

        public override void Update(GameTime gT)
        {
            m_mousePos = Globals.m_mousePosition;
            m_countDown -= (float)(gT.ElapsedGameTime.TotalSeconds);

            if (m_countDown <= 6 && m_countDown >= 4)
            {
                m_fade += (float)(gT.ElapsedGameTime.TotalSeconds * 0.5f);
            }
            
            if (m_countDown <= 3)
            {
                m_fade -= (float)(gT.ElapsedGameTime.TotalSeconds * 0.5f);
            }

            if (m_countDown <= 0)
            {
                AIE.GameStateManager.PopState();
                AIE.GameStateManager.PushState("MENU");
            }

            //TEMPORARY
            //Skip the splash state
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                AIE.GameStateManager.PopState();
                AIE.GameStateManager.PushState("MENU");
            }
            //
        }

        public override void Draw(GameTime gT, SpriteBatch sb)
        {
            sb.Begin();
            //sb.DrawString(Globals.m_defaultFont, "SPLASH STATE", new Vector2(10, 10), Color.White);
            sb.Draw(m_emu, new Rectangle(0, 0, 960, 544), new Color(m_fade, m_fade, m_fade, m_fade));
            sb.End();
        }
    }
}
