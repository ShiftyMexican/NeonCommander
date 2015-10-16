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
using TextureAtlas;
#endregion

namespace TestGameOne
{
    public class Paddle
    {
        private Vector2 m_position;
        private Vector2 m_origin;
        private Vector2 m_dimensions;
        private Vector2 m_speed;
        private Keys m_left;
        private Keys m_right;
        private Rectangle m_collisionRect;
        float m_spriteScale;
        Texture2D t;

        public Paddle(Keys left, Keys right, Vector2 position)
        {
            m_left = left;
            m_right = right;
            m_position = position;
            m_collisionRect = new Rectangle((int)m_position.X, (int)m_position.Y, 256, 32);
            m_spriteScale = 1.0f;
            m_dimensions = new Vector2(m_collisionRect.Width, m_collisionRect.Height) * m_spriteScale;
            m_origin = (m_dimensions / 2.0f) / m_spriteScale;
            m_speed = new Vector2(450, 0);
            t = new Texture2D(AIE.GameStateManager.Game.GraphicsDevice, 1, 1);
            t.SetData(new[] { Color.White });
        }

        public void Update(GameTime gT)
        {
            if (Keyboard.GetState().IsKeyDown(m_left))
            {
                m_position -= m_speed * (float)gT.ElapsedGameTime.TotalSeconds;
            }

            if (Keyboard.GetState().IsKeyDown(m_right))
            {
                m_position += m_speed * (float)gT.ElapsedGameTime.TotalSeconds;
            }

            m_collisionRect.X = (int)(m_position.X - m_origin.X);
            m_collisionRect.Y = (int)(m_position.Y - m_origin.Y);
        }

        public Vector2 GetPosition()
        {
            return m_position;
        }

        public Rectangle GetRect()
        {
            return m_collisionRect;
        }

        public void Draw(Texture2D playerSprite, SpriteBatch sb)
        {
            sb.Draw(playerSprite, m_position, new Rectangle(0, 0, (int)m_dimensions.X, (int)m_dimensions.Y), Color.White, 0.0f, m_origin, m_spriteScale, SpriteEffects.None, 1.0f);
            int bw = 2; // Border width

            sb.Draw(t, new Rectangle(m_collisionRect.Left, m_collisionRect.Top, bw, m_collisionRect.Height), Color.Black); // Left
            sb.Draw(t, new Rectangle(m_collisionRect.Right, m_collisionRect.Top, bw, m_collisionRect.Height), Color.Black); // Right
            sb.Draw(t, new Rectangle(m_collisionRect.Left, m_collisionRect.Top, m_collisionRect.Width, bw), Color.Black); // Top
            sb.Draw(t, new Rectangle(m_collisionRect.Left, m_collisionRect.Bottom, m_collisionRect.Width, bw), Color.Black);
            
        }
    }

    public class Ball
    {
        private Vector2 m_position;
        private Vector2 m_dimensions;
        private Vector2 m_origin;
        private Vector2 m_speed;
        private Rectangle m_collisionRect;
        private float m_spriteScale;

        Texture2D t;

        public Ball()
        {
            m_position = new Vector2(Globals.m_windowWidth/2, Globals.m_windowHeight/2);
            m_collisionRect = new Rectangle((int)m_position.X, (int)m_position.Y, 32, 32);
            m_spriteScale = 1.0f;
            m_dimensions = new Vector2(m_collisionRect.Width, m_collisionRect.Height) * m_spriteScale;
            m_origin = (m_dimensions / 2.0f) / m_spriteScale;
            m_speed = new Vector2(150, 150);
            t = new Texture2D(AIE.GameStateManager.Game.GraphicsDevice, 1, 1);
            t.SetData(new[] { Color.White });
        }

        public void Update(GameTime gT)
        {
            m_position += m_speed * (float)gT.ElapsedGameTime.TotalSeconds;

            m_collisionRect.X = (int)(m_position.X - m_origin.X);
            m_collisionRect.Y = (int)(m_position.Y - m_origin.Y);

            if (m_position.X >= Globals.m_windowWidth || m_position.X <= 0)
            {
                ReverseXDirection();                
            }

            if (m_position.Y >= (Globals.m_windowHeight - m_dimensions.Y/2) || m_position.Y <= m_dimensions.Y/2)
            {
                ResetPosition();
            }
        }

        public Vector2 GetPosition()
        {
            return m_position;
        }

        public void ResetPosition()
        {
            m_position = new Vector2(Globals.m_windowWidth / 2, Globals.m_windowHeight / 2);
        }

        public void ReverseXDirection()
        {
            m_speed.X *= -1;
        }

        public void ReverseYDirection()
        {
            m_speed.Y *= -1;
        }
       
        public Rectangle GetRect()
        {
            return new Rectangle(m_collisionRect.X, m_collisionRect.Y, m_collisionRect.Width, m_collisionRect.Height);
        }       

        public void Draw(Texture2D playerSprite, SpriteBatch sb)
        {
            sb.Draw(playerSprite, m_position, new Rectangle(0, 0, 32, 32), Color.Red, 0.0f, m_origin, m_spriteScale, SpriteEffects.None, 1.0f);
            int bw = 2; // Border width

            sb.Draw(t, new Rectangle(m_collisionRect.Left, m_collisionRect.Top, bw, m_collisionRect.Height), Color.Black); // Left
            sb.Draw(t, new Rectangle(m_collisionRect.Right, m_collisionRect.Top, bw, m_collisionRect.Height), Color.Black); // Right
            sb.Draw(t, new Rectangle(m_collisionRect.Left, m_collisionRect.Top, m_collisionRect.Width, bw), Color.Black); // Top
            sb.Draw(t, new Rectangle(m_collisionRect.Left, m_collisionRect.Bottom, m_collisionRect.Width, bw), Color.Black);
        }
    }

    public class Pong : AIE.IGameState
    {
        private Texture2D m_backgroundSprite;
        private Texture2D m_paddleSprite;
        private Texture2D m_ballSprite;
        private Paddle m_playerOne;
        private Paddle m_playerTwo;
        private Ball m_enemy;

        public Pong()       : base()
        {
            m_backgroundSprite = content.Load<Texture2D>("Images/wallpaper2");
            m_paddleSprite = content.Load<Texture2D>("Images/pong-paddle");
            m_ballSprite = content.Load<Texture2D>("Images/ball");
            m_playerOne = new Paddle(Keys.A, Keys.D, new Vector2(360, 680));
            m_playerTwo = new Paddle(Keys.J, Keys.K, new Vector2(360, 20));
            m_enemy = new Ball();
        }

        public override void Update(GameTime gT)
        {
            m_playerOne.Update(gT);
            m_playerTwo.Update(gT);
            m_enemy.Update(gT);

            if (m_playerOne.GetRect().Intersects(m_enemy.GetRect()) || m_playerTwo.GetRect().Intersects(m_enemy.GetRect()))
            {
                m_enemy.ReverseYDirection();
            } 
        }

        public override void Draw(GameTime gT, SpriteBatch sb)
        {
            sb.Begin();
            m_playerOne.Draw(m_paddleSprite, sb);
            m_playerTwo.Draw(m_paddleSprite, sb);
            m_enemy.Draw(m_ballSprite, sb);
            sb.End();
        }
    }
}
