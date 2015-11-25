using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assgn01
{

    class Billboard
    {
        public VertexPositionNormalTexture[] billboard;
        BasicEffect m_effect;



        float speed = 0.25f;

        public Billboard()
        {
            billboard = new VertexPositionNormalTexture[6];
            //TopLeft
            billboard[0].Position = new Vector3(-5f, -5f, 0f) - Vector3.UnitZ;
            billboard[0].TextureCoordinate = new Vector2(0f, 0f);
            billboard[0].Normal = -Vector3.UnitZ;
            //TopRight
            billboard[1].Position = new Vector3(5f, -5f, 0f) - Vector3.UnitZ;
            billboard[1].TextureCoordinate = new Vector2(1f, 0f);
            billboard[1].Normal = -Vector3.UnitZ;
            //BottomLeft
            billboard[2].Position = new Vector3(-5f, 5f, 0f) - Vector3.UnitZ;
            billboard[2].TextureCoordinate = new Vector2(0f, 1f);
            billboard[2].Normal = -Vector3.UnitZ;
            //TopRight
            billboard[3].Position = new Vector3(5f, -5f, 0f) - Vector3.UnitZ;
            billboard[3].TextureCoordinate = new Vector2(1f, 0f);
            billboard[3].Normal = -Vector3.UnitZ;
            //BottomRight
            billboard[4].Position = new Vector3(5f, 5f, 0f) - Vector3.UnitZ;
            billboard[4].TextureCoordinate = new Vector2(1f, 1f);
            billboard[4].Normal = -Vector3.UnitZ;
            //BottomLeft
            billboard[5].Position = new Vector3(-5f, 5f, 0f) - Vector3.UnitZ;
            billboard[5].TextureCoordinate = new Vector2(0f, 1f);
            billboard[5].Normal = -Vector3.UnitZ;
        }

        public void AddEffect(BasicEffect effect)
        {
            m_effect = effect;
        }

        public void MoveUp()
        {
            m_effect.World *= Matrix.CreateTranslation(0f, speed, 0f);
        }
        public void MoveDown()
        {
            m_effect.World *= Matrix.CreateTranslation(0f, -speed, 0f);
        }
        public void MoveLeft()
        {
            m_effect.World *= Matrix.CreateTranslation(-speed, 0f, 0f);
        }
        public void MoveRight()
        {
            m_effect.World *= Matrix.CreateTranslation(speed, 0f, 0f);
        }

        public void Move(Vector3 delta)
        {
            m_effect.World *= Matrix.CreateTranslation(delta.X * speed * 0.01f, 
                delta.Y * -speed * 0.01f, 0f);
        }

        public void RotateUp()
        {
            m_effect.World *= Matrix.CreateRotationX(MathHelper.Pi / 4 * speed);
        }

        public void RotateDown()
        {
            m_effect.World *= Matrix.CreateRotationX(-MathHelper.Pi / 4 * speed);
        }

        public void RotateLeft()
        {
            m_effect.World *= Matrix.CreateRotationY(MathHelper.Pi / 4 * speed);
        }

        public void RotateRight()
        {
            m_effect.World *= Matrix.CreateRotationY(-MathHelper.Pi/4 * speed);
        }

        public void Rotate(Vector2 delta)
        {
            m_effect.World *= Matrix.CreateRotationY((delta.X * 0.25f * MathHelper.Pi) / 4 * speed);
            m_effect.World *= Matrix.CreateRotationX((delta.Y * 0.25f * MathHelper.Pi) / 4 * speed);
        }

        public void ApplyEffect(GraphicsDevice graphics, RenderTarget2D renderTarget)
        {

            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            graphics.RasterizerState = rs;
            m_effect.TextureEnabled = true;
            m_effect.Texture = renderTarget;

            foreach (EffectPass pass in m_effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList,
                billboard, 0, 2);
            }
        }

    
    
    
    
    }


}
