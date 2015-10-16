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
#endregion

namespace InputManager
{
    public static class InputManager
    {

        #region MOUSE AND KEYBOARD STATES
        static private KeyboardState m_keyboardOldState;
        static private MouseState m_mouseOldState;
        #endregion

        #region GAMEPAD STATES
        static private GamePadState m_gamePadOneOldState;
        static private GamePadState m_gamePadTwoOldState;
        static private GamePadState m_gamePadThreeOldState;
        static private GamePadState m_gamePadFourOldState;
        #endregion

        static bool m_initialised;

        /// <summary>
        /// Input Manager constructor
        /// Initialises all inputs
        /// </summary>
        static InputManager()
        {
             #region MOUSE AND KEYBOARD STATES
             m_keyboardOldState = new KeyboardState();
             m_mouseOldState = new MouseState();
             #endregion

             #region GAMEPAD STATES
             m_gamePadOneOldState   = new GamePadState();
             m_gamePadTwoOldState   = new GamePadState();
             m_gamePadThreeOldState = new GamePadState();
             m_gamePadFourOldState  = new GamePadState();
             #endregion

             m_initialised = true;
        }

        /// <summary>
        /// Calling this function inherently calls the InputManager's constructor
        /// Do this during the loading/initialisation stage to stop potential hangs
        /// </summary>
        static public void InitaliseInputManager()
        {
            m_initialised = true;
        }

        /// <summary>
        /// Updates the old state inputs, should be called at the end of the main update function
        /// </summary>
        static public void UpdateInputs()
        {
            #region UPDATING MOUSE, KEYBOARD AND GAMEPAD STATES
            if (m_initialised)
            {
                m_keyboardOldState = Keyboard.GetState();
                m_mouseOldState = Mouse.GetState();

                if (GamePad.GetState(PlayerIndex.One).IsConnected)
                    m_gamePadOneOldState = GamePad.GetState(PlayerIndex.One);

                if (GamePad.GetState(PlayerIndex.Two).IsConnected)
                    m_gamePadTwoOldState = GamePad.GetState(PlayerIndex.Two);

                if (GamePad.GetState(PlayerIndex.Three).IsConnected)
                    m_gamePadThreeOldState = GamePad.GetState(PlayerIndex.Three);

                if (GamePad.GetState(PlayerIndex.Four).IsConnected)
                    m_gamePadFourOldState = GamePad.GetState(PlayerIndex.Four);
            }
            #endregion
        }

        #region KEYBOARD INPUT CHECKS
        /// <summary>
        /// Checks if the key on the keyboard was just pressed down this frame.
        /// Will return true if the key was pushed down on this frame.
        /// </summary>
        /// <param name="key"> Use Keys.A for the A key, etc...</param>
        /// <returns></returns>
        public static bool IsKeyJustPressed(Keys key)
        {
            if (Keyboard.GetState().IsKeyDown(key) && m_keyboardOldState.IsKeyUp(key))
                return true;

            else return false;
        }

        /// <summary>
        /// Checks if the key on the keyboard was just released this frame.
        /// Will return true if the key was released on this frame.
        /// </summary>
        /// <param name="key"> Use Keys.A for the A key, etc...</param>
        /// <returns></returns>
        public static bool IsKeyJustReleased(Keys key)
        {
            if (Keyboard.GetState().IsKeyUp(key) && m_keyboardOldState.IsKeyDown(key))
                return true;

            else return false;
        }
        #endregion

        #region MOUSE INPUT CHECKS
        /// <summary>
        /// Returns whether the left mouse button has just been clicked or not.
        /// If the mouse was clicked this frame then this will return true.
        /// </summary>
        /// <returns></returns>
        public static bool IsMouseLeftButtonJustClicked()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && m_mouseOldState.LeftButton == ButtonState.Released)
                return true;

            else return false;
        }

        /// <summary>
        /// Returns whether the left mouse button has just been released or not.
        /// If the mouse was not clicked last frame then this will return true.
        /// </summary>
        /// <returns></returns>
        public static bool IsMouseLeftButtonJustReleased()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Released && m_mouseOldState.LeftButton == ButtonState.Pressed)
                return true;

            else return false;
        }

        public static bool IsMouseRightButtonJustClicked()
        {
            if (Mouse.GetState().RightButton == ButtonState.Pressed && m_mouseOldState.RightButton == ButtonState.Released)
                return true;

            else return false;
        }

        public static bool IsMouseRightButtonJustReleased()
        {
            if (Mouse.GetState().RightButton == ButtonState.Released && m_mouseOldState.RightButton == ButtonState.Pressed)
                return true;

            else return false;
        }
        #endregion

        #region GAMEPAD INPUT CHECKS
        /// <summary>
        /// Checks if the button on the gamePad was just pressed down this frame.
        /// Requires a specific player. Will return true if button was pushed down on this frame.
        /// </summary>
        /// <param name="player"> Use PlayerIndex.One for player one, etc...</param>
        /// <param name="button"> Use Buttons.A for the A button, etc...</param>
        /// <returns></returns>
        public static bool IsGamePadButtonJustPressed(PlayerIndex player, Buttons button)
        {
            if (player == PlayerIndex.One)
            {
                if (GamePad.GetState(player).IsButtonDown(button) && m_gamePadOneOldState.IsButtonUp(button))
                    return true;
            }

            else if (player == PlayerIndex.Two)
            {
                if (GamePad.GetState(player).IsButtonDown(button) && m_gamePadTwoOldState.IsButtonUp(button))
                    return true;
            }

            else if (player == PlayerIndex.Three)
            {
                if (GamePad.GetState(player).IsButtonDown(button) && m_gamePadThreeOldState.IsButtonUp(button))
                    return true;
            }

            else if (player == PlayerIndex.Four)
            {
                if (GamePad.GetState(player).IsButtonDown(button) && m_gamePadFourOldState.IsButtonUp(button))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the button on the gamePad was just released this frame.
        /// Requires a specific player. Will return true if button was released on this frame.
        /// </summary>
        /// <param name="player"> Use PlayerIndex.One for player one, etc...</param>
        /// <param name="button"> Use Buttons.A for the A button, etc...</param>
        /// <returns></returns>
        public static bool IsGamePadButtonJustReleased(PlayerIndex player, Buttons button)
        {
            if (player == PlayerIndex.One)
            {
                if (GamePad.GetState(player).IsButtonUp(button) && m_gamePadOneOldState.IsButtonDown(button))
                    return true;
            }

            else if (player == PlayerIndex.Two)
            {
                if (GamePad.GetState(player).IsButtonUp(button) && m_gamePadTwoOldState.IsButtonDown(button))
                    return true;
            }

            else if (player == PlayerIndex.Three)
            {
                if (GamePad.GetState(player).IsButtonUp(button) && m_gamePadThreeOldState.IsButtonDown(button))
                    return true;
            }

            else if (player == PlayerIndex.Four)
            {
                if (GamePad.GetState(player).IsButtonUp(button) && m_gamePadFourOldState.IsButtonDown(button))
                    return true;
            }

            return false;
        }
        #endregion

    }
}
