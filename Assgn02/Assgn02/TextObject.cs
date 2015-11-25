using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assgn02;

namespace Assgn01
{
    public class TextObject
    {
        private SpriteFont font;
        private Color color;
        private Vector2 position;
        private String actualText;
        private Boolean hasShadow;

        public SpriteFont Font
        {
            get { return font; }
            set { font = value; }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        
        
        public TextObject(String _text, SpriteFont _font, Color _color, Boolean _hasShadow)
        {
            color = _color;
            font = _font;
            actualText = _text;
            hasShadow = _hasShadow;
            position = new Vector2(0, 0);
        }

        public TextObject(String _text, SpriteFont _font, Color _color, Boolean _hasShadow, Vector2 _position)
        {
            color = _color;
            font = _font;
            actualText = _text;
            hasShadow = _hasShadow;
            position = _position;
        }
        public void MoveUp()
        {
            position -= new Vector2(0, 1);
        }
        public void MoveDown()
        {
            position += new Vector2(0, 1);
        }
        public void MoveLeft()
        {
            position -= new Vector2(1, 0);
        }
        public void MoveRight()
        {
            position += new Vector2(1, 0);
        }

        public void Move(Vector2 delta)
        {
            position += delta;
        }



        public void Draw(SpriteBatch spritebatch)
        {
            if (hasShadow)
                for (int i = 0; i < 15; i++ )
                    spritebatch.DrawString(font, actualText, position - new Vector2(i,i), Color.Black);
            spritebatch.DrawString(font, actualText, position, color);
        }

        public void SetText(string _text)
        {
            actualText = _text;
        }
    }
}
