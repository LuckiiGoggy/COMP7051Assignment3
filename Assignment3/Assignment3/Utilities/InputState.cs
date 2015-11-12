using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assignment3.Utilities
{
    class InputState
    {
        public KeyboardState CurrKeys {get; set;}
        public MouseState CurrMouse {get; set;}
        public GamePadState CurrPad { get; set; }
        public KeyboardState PrevKeys { get; set; }
        public MouseState PrevMouse { get; set; }
        public GamePadState PrevPad { get; set; }


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

        public Keys[] GetKeysTapped()
        {
            IEnumerable<Keys> result = CurrKeys.GetPressedKeys().Except(PrevKeys.GetPressedKeys());
            return result.ToArray();
        }


        public Boolean IsKeyTapped(Keys key)
        {
            return (CurrKeys.IsKeyDown(key) && !PrevKeys.IsKeyDown(key));
        }

        public Boolean IsButtonTapped(Buttons key)
        {
            return (CurrPad.IsButtonDown(key) && !PrevPad.IsButtonDown(key));
        }
    }
}
