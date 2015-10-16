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

namespace TestGameOne
{
    class Button
    {
        public Rectangle m_rectangle;
        private Texture2D m_buttonTexture;
        private Vector2 m_origin;
        public Vector2 m_texturePosition;
        private bool m_isMouseOver;
        private bool m_buttonClicked;
        bool m_buttonSelected;
        private bool m_keyboardInput;
        

        public Button(int x, int y, int width, int height, Texture2D buttonTexture)
        {                       
            m_isMouseOver = false;
            m_buttonClicked = false;
            m_buttonSelected = false;
            m_keyboardInput = false;
            
            m_origin = new Vector2(width / 2, height / 2);
            m_texturePosition = new Vector2(x, y);

            m_rectangle = new Rectangle((int)(x - m_origin.X), (int)(y - m_origin.Y), width, height);
            m_buttonTexture = buttonTexture;

        }

        public void Update(GameTime gT)
        {
            if (Globals.m_mousePosition.X > m_rectangle.X && Globals.m_mousePosition.X < m_rectangle.X + m_rectangle.Width &&
                Globals.m_mousePosition.Y > m_rectangle.Y && Globals.m_mousePosition.Y < m_rectangle.Y + m_rectangle.Height)
            {
                m_isMouseOver = true;
            }

            else
            {
                m_isMouseOver = false;
            }

            if ((m_isMouseOver == true && InputManager.InputManager.IsMouseLeftButtonJustClicked()) || (InputManager.InputManager.IsGamePadButtonJustPressed(PlayerIndex.One, Buttons.A) && m_buttonSelected == true))
            {                
                m_buttonClicked = true;
            }

            else
            {
                m_buttonClicked = false;
            }

            if (Keyboard.GetState().GetPressedKeys().Length > 0)
            {
                m_keyboardInput = true;
            }
            else
            {
                m_keyboardInput = false;
            }
            
        }

        #region DRAW FUNCTIONS
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(m_buttonTexture, m_texturePosition, new Rectangle(0, 0, m_rectangle.Width, m_rectangle.Height), Color.White, 0.0f, m_origin, 1.0f, SpriteEffects.None, 0.0f);
        }

        public void Draw(SpriteBatch sb, Color color)
        {
            sb.Draw(m_buttonTexture, m_texturePosition, new Rectangle(0, 0, m_rectangle.Width, m_rectangle.Height), color, 0.0f, m_origin, 1.0f, SpriteEffects.None, 0.0f);
        }

        public void Draw(SpriteBatch sb, Color color, float rot, float scale )
        {
            sb.Draw(m_buttonTexture, m_texturePosition, new Rectangle(0, 0, m_rectangle.Width, m_rectangle.Height), color, rot, m_origin, scale, SpriteEffects.None, 0.0f);
        }
        #endregion

        public bool IsButtonClicked()
        {
            return m_buttonClicked;
        }

        public bool IsMouseOver()
        {
            return m_isMouseOver;
        }

        public bool IsButtonDown()
        {
            if (m_isMouseOver && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                return true;
            }

            else
                return false;
        }

        public void ButtonSelected()
        {
            m_buttonSelected = true;
        }

        public void ButtonDeselected()
        {
            m_buttonSelected = false;
        }

        public bool KeyboardInputGiven()
        {
            return m_keyboardInput;
        }

    }
}
