using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#region XNA Namespaces
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
#endregion

namespace Assignment3.GameObjects
{
    public class GameObject3D
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }
        public Model Model { get; private set; }
        public Texture2D Texture { get; private set; }

        public GameObject3D()
        {

            Scale = new Vector3(1, 1, 1);
        }

        public GameObject3D(Model mdl, Texture2D texture)
        {

            Scale = Vector3.One;
            Model = mdl;
            Texture = texture;
        }

        public void Translate(Vector3 translation)
        {
            Position += translation;
        }

        public void Rotate(Vector3 rotation)
        {
            Rotation += rotation;
        }

        public Matrix GetWorldMatrix()
        {
            return Matrix.CreateScale(Scale) * Matrix.CreateRotationX(Rotation.X) * Matrix.CreateRotationY(Rotation.Y) *
                Matrix.CreateRotationZ(Rotation.Z) * Matrix.CreateTranslation(Position);
        }

    }
}
