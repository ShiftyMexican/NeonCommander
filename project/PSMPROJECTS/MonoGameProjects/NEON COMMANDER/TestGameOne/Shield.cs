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
    class Shield
    {

        Player m_player;
        PowerUp m_powerup;

        private float m_scale;
        private float m_rotation;
        private float m_shieldTime;
        private float m_lerpTimer;
        private float m_shieldLerp;
        private bool m_peakHit;
        private Texture2D m_shieldTexture;
        private Vector2 m_dimensions;
        private Vector2 m_origin;
        private Vector2 m_position;
        private Vector2 m_timerPosition;

        Circle m_collisionRadius;

        //Circle m_collisionRadius;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Shield()
            : base()
        {
        }

        /// <summary>
        /// Overloaded Constructor
        /// </summary>
        /// <param name="position"></param>
        /// <param name="shieldTexture"></param>
        /// <param name="powerup"></param>
        /// <param name="player"></param>
        public Shield(Vector2 position, PowerUp powerup, Player player)
        {
            m_player = player;
            m_shieldTexture = AIE.GameStateManager.Game.Content.Load<Texture2D>("Images/ShieldTexture3");
            m_position = position;
            m_powerup = powerup;
            m_scale = 1.0f;
            m_dimensions = new Vector2(110, 110) / m_scale;
            m_origin = (m_dimensions / 2.0f);
            m_collisionRadius = new Circle(m_position, m_dimensions.X / 2);
            m_rotation = 0.0f;
            m_shieldTime = 15.0f;
            m_timerPosition = new Vector2(0, 0);
            m_lerpTimer = 0.0f;

            m_peakHit = false;
        }

        public void Update(GameTime gT)
        {
            m_shieldTime -= (float)gT.ElapsedGameTime.TotalSeconds;

          
            m_shieldLerp += 0.0016f;

            if (m_peakHit == false)
                m_lerpTimer += 0.005f;

            
            if (m_lerpTimer >= 1f)
                m_peakHit = true;

            if(m_peakHit == true)
                m_lerpTimer -= 0.01f;


            m_position = m_player.GetPosition();
            m_timerPosition = new Vector2(m_player.GetPosition().X, m_player.GetPosition().Y + 200);
            m_rotation += 0.1f;

            m_collisionRadius.m_center = m_position;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(m_shieldTexture, m_position, new Rectangle(0, 0, 110, 110), Color.Lerp(Color.White, Color.Transparent, m_shieldLerp), m_rotation, m_origin, m_scale, SpriteEffects.None, 0.0f);
            sb.DrawString(Globals.m_defaultFont, "SHIELD", m_timerPosition, Color.Lerp(Color.Transparent, Color.White, m_lerpTimer), 0.0f, Globals.m_defaultFont.MeasureString("SHIELD") / 2, 1.0f, SpriteEffects.None, 0.0f);
        }

        public Circle GetCollisionCircle()
        {
            return m_collisionRadius;
        }
    }
}
