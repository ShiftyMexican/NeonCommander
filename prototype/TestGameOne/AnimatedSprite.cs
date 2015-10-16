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

namespace TextureAtlas
{
    public class AnimatedSprite
    {
        public Texture2D m_texture { get; set; }
        public Vector2 m_position { get; set; }
        public int m_rows { get; set; }
        public int m_columns { get; set; }
        private int m_currentFrame;
        private int m_totalFrames;
        private bool m_finished;

        public AnimatedSprite(Texture2D texture, int rows, int columns)
        {
            m_texture = texture;
            m_rows = rows;
            m_columns = columns;
            m_currentFrame = 0;
            m_totalFrames = m_rows * m_columns;
            m_finished = false;
            m_position = new Vector2();
        }

        public AnimatedSprite(Texture2D texture, int rows, int columns, int totalframes)
        {
            m_texture = texture;
            m_rows = rows;
            m_columns = columns;
            m_currentFrame = 0;
            m_totalFrames = totalframes;
            m_finished = false;
            m_position = new Vector2();
        }

        public void Update()
        {
            if (!m_finished)
            {
                m_currentFrame++;
                if (m_currentFrame == m_totalFrames)
                {
                    m_currentFrame = 0;
                    m_finished = true;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!m_finished)
            {
                Vector2 newPos = m_position;
                newPos.Y -= 32;
                newPos.X -= 32;
                
                int width = m_texture.Width / m_columns;
                int height = m_texture.Height / m_rows;
                int row = (int)((float)m_currentFrame / (float)m_columns);
                int column = m_currentFrame % m_columns;

                Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
                Rectangle destinationRectangle = new Rectangle((int)newPos.X, (int)newPos.Y, width, height);

                spriteBatch.Draw(m_texture, destinationRectangle, sourceRectangle, Color.White);
            }
        }

        public void ResetAnimation()
        {
            m_finished = false;
        }
    }
}