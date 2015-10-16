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

    public class Bullet
    {

        #region BULLET VARIABLES

        Vector2 m_facingDirection;
        Vector2 m_position;
        Vector2 m_origin;
        Vector2 m_dimensions;

        private float m_rotationAngle;
        public float m_speed;
        //private float m_deathCoolDown;
        private float m_textureScale;

        Circle m_collisionRadius;

        #endregion


        /// <summary>
        /// Deafualt Constructor for the bullet class.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="facingDir"></param>
        public Bullet(Vector2 position, float rotation, Vector2 facingDir)
        {
            m_position = position;
            m_rotationAngle = rotation;
            m_facingDirection = facingDir;
            m_textureScale = 0.5f;

            m_dimensions = new Vector2(15.0f, 16.0f) * m_textureScale;
            m_origin = new Vector2(m_dimensions.X / 2.0f, m_dimensions.Y / 2.0f) / m_textureScale;
            m_speed = 350.0f;

            m_collisionRadius = new Circle(m_position, m_dimensions.X / m_textureScale / 2.0f);                     
        }


        /// <summary>
        /// Update function for the bullets
        /// </summary>
        /// <param name="gT"></param>
        public void Update(GameTime gT)
        {

            m_position += m_facingDirection * (float)gT.ElapsedGameTime.TotalSeconds * m_speed;


            //move to play state
            if (m_position.X < 0 || m_position.X > Globals.m_gameWidth || m_position.Y < 0 || m_position.Y > Globals.m_gameHeight)
            {
                //die
            }

            m_collisionRadius.m_center = m_position;
        }

        /// <summary>
        /// Draw Function for the bullet class.
        /// </summary>
        /// <param name="bulletSprite"></param>
        /// <param name="sb"></param>
        public void Draw(Texture2D bulletSprite, SpriteBatch sb)
        {

            sb.Draw(bulletSprite, m_position, new Rectangle(0, 0, 15, 16), Color.White, m_rotationAngle, m_origin, m_textureScale, SpriteEffects.None, 0.0f);

        }

        /// <summary>
        /// Returns the position of the bullet.
        /// </summary>
        /// <returns></returns>
        public Vector2 GetPosition()
        {
            return m_position;
        }

        /// <summary>
        /// Returns the collision radius of each bullet.
        /// </summary>
        /// <returns></returns>
        public Circle GetCircle()
        {
            return m_collisionRadius;
        }
    }
}
