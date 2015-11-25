using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assgn02
{
    /// <summary>
    /// Sprite Controller controls a 2D Sprite by connecting
    /// input controls from XNA to the velocity values of the
    /// Sprite. 
    /// 
    /// For example if the player inputs a left D-pad button
    /// the sprite controller would add a negative velocity on
    /// the x axis.
    /// </summary>
    class SpriteController
    {
        /// <summary>
        /// The controlled sprite by the controller.
        /// 
        /// Could change this to a list of sprites if
        /// the object would control multiple sprites 
        /// as a group. 
        /// Or even create a group sprite object that
        /// the controller controls instead of just one
        /// sprite.
        /// </summary>
        Sprite m_sprite;

        float m_movementSpd = 0.5f;

        float MovementSpeed
        {
            get { return m_movementSpd; }
            set { m_movementSpd = value; }
        }

        public SpriteController(Sprite sprite)
        {
            m_sprite = sprite;
        }

        /// <summary>
        /// Updates the Controller with the current states of
        /// the inputs.
        /// Moves the sprite depending on the 
        /// </summary>
        /// <param name="gameTime">Get the game time for synchronizing</param>
        public void Update(GameTime gameTime)
        {
            KeyboardState keys = Keyboard.GetState();
            GamePadState pad = GamePad.GetState(0);

            if (keys.IsKeyDown(Keys.Left)) m_sprite.Move(new Vector2(-m_movementSpd, 0));
            if (keys.IsKeyDown(Keys.Right)) m_sprite.Move(new Vector2(m_movementSpd, 0));
            //if (keys.IsKeyDown(Keys.Up)) m_sprite.Move(new Vector2(0, -m_movementSpd));
            //if (keys.IsKeyDown(Keys.Down)) m_sprite.Move(new Vector2(0, m_movementSpd));

            if (pad.ThumbSticks.Left.X < 0) m_sprite.Move(new Vector2(-m_movementSpd, 0));
            if (pad.ThumbSticks.Left.X > 0) m_sprite.Move(new Vector2(m_movementSpd, 0));

            if (pad.ThumbSticks.Right.X < 0) m_sprite.Move(new Vector2(-m_movementSpd, 0));
            if (pad.ThumbSticks.Right.X > 0) m_sprite.Move(new Vector2(m_movementSpd, 0));

            if (pad.DPad.Left == ButtonState.Pressed) m_sprite.Move(new Vector2(-m_movementSpd, 0));
            if (pad.DPad.Right == ButtonState.Pressed) m_sprite.Move(new Vector2(m_movementSpd, 0));
        }
    }
}
