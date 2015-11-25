using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assgn02
{
    public class Sprite : PhysicsTransform
    {
        Texture2D m_texture;
        public float Width{get; set;}
        public float Height { get; set; }

        public Texture2D Texture
        {
            get { return m_texture; }
            set { m_texture = value; }
        }


        public Sprite(Texture2D texture)
        {
            m_texture = texture;
            Width = texture.Width;
            Height = texture.Height;
        }

        public Sprite(Texture2D texture, Vector2 iniPos)
        {
            m_texture = texture;
            Position = iniPos;
            Width = texture.Width;
            Height = texture.Height;
        }

        public Sprite(Texture2D texture, Vector2 iniPos, PhysicsType type)
        {
            m_texture = texture;
            Position = iniPos;
            PhysType = type;
            Width = texture.Width;
            Height = texture.Height;
        }

        public Sprite(Texture2D texture, Vector2 iniPos, PhysicsType type, bool _isNonStop)
        {
            m_texture = texture;
            Position = iniPos;
            PhysType = type;
            IsNonStop = _isNonStop;
            Width = texture.Width;
            Height = texture.Height;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            

            spriteBatch.Draw(m_texture, Position, Color.White);
                      
        }

        public override void Update(int timeDelta)
        {
            base.Update(timeDelta);

            
        }

        


    }
}
