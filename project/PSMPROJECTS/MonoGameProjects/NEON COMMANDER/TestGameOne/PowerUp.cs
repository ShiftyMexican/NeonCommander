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
    public class PowerUp
    {
        #region POWERUP_VARIABLES
        private Vector2 m_dimensions;
        private Vector2 m_origin;
        private Vector2 m_position;
        Circle m_collisionRadius;
        Texture2D m_texture;
        private float m_spriteScale;
        private float m_countdown;
        private float m_rotationAngle;
        public int m_powerupID { get; set; }
        bool m_activePowerup;

        PlayState m_playState;
        Player m_player;
        #endregion

        public PowerUp(Vector2 position, Texture2D powerupTexture, int powerupID, PlayState playState, Player pPlayer)
        {
            m_playState = playState;
            m_player = pPlayer;

            m_texture = AIE.GameStateManager.Game.Content.Load<Texture2D>("Images/ShieldTexture3");

            m_position = position;
            m_spriteScale = 0.45f;
            m_dimensions = new Vector2(110, 110) * m_spriteScale;
            m_origin = (m_dimensions / 2.0f) / m_spriteScale;
            m_collisionRadius = new Circle(m_position, m_dimensions.X / 2);
            m_rotationAngle = 0.0f;

            m_powerupID = powerupID;
            m_activePowerup = true;
            m_countdown = 10.0f;
        }

        /// <summary>
        /// Update function for the power ups.
        /// </summary>
        /// <param name="gT"></param>
        public void Update(GameTime gT)
        {
            m_countdown -= (float)gT.ElapsedGameTime.TotalSeconds;
            
            if(m_countdown <= 0.0f && m_activePowerup == true)
            {
                m_activePowerup = false;
            }

            m_rotationAngle += 0.1f;
        }

        /// <summary>
        /// Draw Function for the Power Ups
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(m_texture, m_position, new Rectangle(0, 0, 110, 110), Color.Red, m_rotationAngle, m_origin, m_spriteScale, SpriteEffects.None, 0.0f);
            sb.DrawString(Globals.m_defaultFont, "?", new Vector2(m_position.X + 28, m_position.Y + 18), Color.White, 0.0f, m_origin, 0.6f, SpriteEffects.None, 0.0f);
        }

        public void SetPowerup(Vector2 position, Texture2D powerupTexture, int powerupID)
        {
            m_position = position;
            m_texture = powerupTexture;
            m_powerupID = powerupID;
            m_activePowerup = true;
        }

        public void UsePowerup()
        {
            m_activePowerup = false;
        }

        public Circle GetCollisionCircle()
        {
            return m_collisionRadius;
        }

        public void PowerUpOne()
        {
            m_playState.m_mineEnemyList.Clear();
            m_playState.m_missileEnemyList.Clear();
            m_playState.m_multiShooterList.Clear();
            m_playState.m_seekerEnemyList.Clear();
            m_playState.m_bulletList.Clear();
            m_playState.m_mineList.Clear();

            m_playState.m_powerupUsed = true;
        }
    }
}
