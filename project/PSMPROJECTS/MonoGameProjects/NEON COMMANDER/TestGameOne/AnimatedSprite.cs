﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
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
        public float m_scale { get; set; }
        private int m_currentFrame;
        private int m_totalFrames;
        public bool m_finished { get; set; }
        public int m_framesPlayedPerFrame { get; set; }

        public AnimatedSprite(Texture2D texture, int rows, int columns, float scale)
        {
            m_texture = texture;
            m_rows = rows;
            m_columns = columns;
            m_scale = scale;
            m_currentFrame = 0;
            m_totalFrames = m_rows * m_columns;
            m_finished = false;
            m_position = new Vector2();
            m_framesPlayedPerFrame = 1;
        }

        public AnimatedSprite(Texture2D texture, int rows, int columns, int totalframes, float scale)
        {
            m_texture = texture;
            m_rows = rows;
            m_columns = columns;
            m_scale = scale;
            m_currentFrame = 0;
            m_totalFrames = totalframes;
            m_finished = false;
            m_position = new Vector2();
            m_framesPlayedPerFrame = 1;
        }

        public AnimatedSprite(Texture2D texture, int rows, int columns, int totalframes, int framesPlayedPerFrame, float scale)
        {
            m_texture = texture;
            m_rows = rows;
            m_columns = columns;
            m_scale = scale;
            m_currentFrame = 0;
            m_totalFrames = totalframes;
            m_finished = false;
            m_position = new Vector2();
            m_framesPlayedPerFrame = framesPlayedPerFrame;
        }

        public void Update()
        {
            if (!m_finished)
            {
                m_currentFrame += m_framesPlayedPerFrame;
                if (m_currentFrame >= m_totalFrames)
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
                int width = m_texture.Width / m_columns;
                int height = m_texture.Height / m_rows;
                int row = (int)((float)m_currentFrame / (float)m_columns);
                int column = m_currentFrame % m_columns;

                Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);                

                spriteBatch.Draw(m_texture, m_position, sourceRectangle, Color.White, 0.0f, new Vector2(width / 2, height / 2), m_scale, SpriteEffects.None, 0.0f);
            }
        }
    }
}