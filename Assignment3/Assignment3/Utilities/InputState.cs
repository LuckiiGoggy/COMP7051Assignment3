using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assignment3.Utilities
{
    /// <summary>
    /// Input State holds the current and previous states of the
    /// keyboard, mouse, and gamepad. It provides added functionality
    /// to the states like checking if a key or button has been tapped
    /// or just released.
    /// </summary>
    public class InputState
    {

        #region Current States
        public KeyboardState CurrKeys { get; set; }
        public MouseState CurrMouse { get; set; }
        public GamePadState CurrPad { get; set; } 
        #endregion
        #region Previous States
        public KeyboardState PrevKeys { get; set; }
        public MouseState PrevMouse { get; set; }
        public GamePadState PrevPad { get; set; }
        
        #endregion

        public InputState()
        {
            CurrKeys = new KeyboardState();
            CurrMouse = new MouseState();
            CurrPad = new GamePadState();
            PrevKeys = new KeyboardState();
            PrevMouse = new MouseState();
            PrevPad = new GamePadState();
        }
        public void UpdateState()
        {
            PrevKeys = CurrKeys;
            PrevMouse = CurrMouse;
            PrevPad = CurrPad;
            CurrKeys = Keyboard.GetState();
            CurrMouse = Mouse.GetState();
            CurrPad = GamePad.GetState(0);
        }



        #region Keyboard Methods

        /// <summary>
        /// Returns all key presses that were just tapped
        /// </summary>
        /// <returns>Key presses that were just tapped</returns>
        public Keys[] GetKeysTapped()
        {
            IEnumerable<Keys> result = CurrKeys.GetPressedKeys().Except(PrevKeys.GetPressedKeys());
            return result.ToArray();
        }
        /// <summary>
        /// Returns if a Key was just tapped
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if it was just tapped</returns>
        public Boolean IsKeyTapped(Keys key)
        {
            return (CurrKeys.IsKeyDown(key) && !PrevKeys.IsKeyDown(key));
        }

        /// <summary>
        /// Returns if a key is currently down
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if the key is currently down</returns>
        public Boolean IsKeyDown(Keys key)
        {
            return CurrKeys.IsKeyDown(key);
        }
        /// <summary>
        /// Returns if a key is currently up
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if the key is currently up</returns>
        public Boolean IsKeyUp(Keys key)
        {
            return CurrKeys.IsKeyUp(key);
        }
        #endregion


        #region GamePad Methods

        /// <summary>
        /// Returns if a button was just tapped
        /// </summary>
        /// <param name="button">Button to check</param>
        /// <returns>True if button was just tapped</returns>
        public Boolean IsButtonTapped(Buttons button)
        {
            return (CurrPad.IsButtonDown(button) && !PrevPad.IsButtonDown(button));
        } 

        /// <summary>
        /// Returns if a button was just released
        /// </summary>
        /// <param name="button">Button to check</param>
        /// <returns>True if the button was just released</returns>
        public Boolean IsButtonReleased(Buttons button)
        {
            return (CurrPad.IsButtonUp(button) && !PrevPad.IsButtonUp(button));
        }

        /// <summary>
        /// Returns if a button is currently down
        /// </summary>
        /// <param name="button">Button to check</param>
        /// <returns>True if the button is currently down</returns>
        public Boolean IsButtonDown(Buttons button)
        {
            return CurrPad.IsButtonDown(button);
        }


        /// <summary>
        /// Returns if a button is currently up
        /// </summary>
        /// <param name="button">Button to check</param>
        /// <returns>True if the button is currently up</returns>
        public Boolean IsButtonUp(Buttons button)
        {
            return CurrPad.IsButtonUp(button);
            
        }

        public Vector2 LeftStick { get { return CurrPad.ThumbSticks.Left; } }
        public Vector2 RightStick { get { return CurrPad.ThumbSticks.Right; } }

        public Vector2 LeftStickDelta()
        {
            return CurrPad.ThumbSticks.Left - PrevPad.ThumbSticks.Left;
        }

        public Vector2 RightStickDelta()
        {
            return CurrPad.ThumbSticks.Right - PrevPad.ThumbSticks.Right;
        }
        #endregion
    }
}
