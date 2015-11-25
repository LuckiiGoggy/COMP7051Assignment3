using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assgn01
{
    class StateSnapshot
    {
        private KeyboardState m_keys;
        private MouseState m_mouse;
        private GamePadState m_gamepad;


        public KeyboardState keys
        {
            get { return m_keys; }
            private set { m_keys = value; }
        }
        public MouseState mouse
        {
            get { return m_mouse; }
            private set { m_mouse = value; }
        }
        public GamePadState gamepad
        {
            get { return m_gamepad; }
            private set { m_gamepad = value; }
        }

        public StateSnapshot()
        {
            keys = new KeyboardState();
            mouse = Mouse.GetState();
            gamepad = new GamePadState();
        }
        public void UpdateSnapshot(KeyboardState _kb, MouseState _ms, GamePadState _gp)
        {
            keys = _kb;
            mouse = _ms;
            gamepad = _gp;
        }

        public Keys[] GetKeysTapped(Keys[] keys)
        {
            Keys[] oldKeys = m_keys.GetPressedKeys();
            IEnumerable<Keys> result = keys.Except(oldKeys);
            return result.ToArray();
        }


        public Boolean IsKeyTapped(KeyboardState keys, Keys key)
        {
            return (keys.IsKeyDown(key) && !m_keys.IsKeyDown(key));
        }
        

    }
}
