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
namespace Assignment3
{
    public class Camera
    {

        public Vector3 LookAt { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Up { get; set; }

        public Vector3 Reference { get; set; }


        public Matrix rotationMatrix;
        Vector3 transformedReference;

        public Matrix View { get; set; }
        public Camera()
        {
            LookAt = new Vector3(1, 0, 0);
            Position = new Vector3(0, -1, 0);
            Up = new Vector3(0, 0, 1);
            Reference = new Vector3(0, 1, 0);
        }

        /// <summary>
        /// Creates and returns the View matrix that will be used for rendering.
        /// </summary>
        /// <returns>View matrix created using camera transform.</returns>
        public Matrix GetView()
        {
            return Matrix.CreateLookAt(Position, LookAt, new Vector3(0f,0f,1f));
        }

        /// <summary>
        /// Translate the camera by the specified vector.
        /// </summary>
        /// <param name="translation">The vector to translate the camera by.</param>
        public void Translate(Vector3 translation)
        {
            Position += translation;
            LookAt += translation;
        }

        /// <summary>
        /// Rotates the camera around the y-axis.
        /// </summary>
        /// <param name="yradian"></param>
        public void Rotate(float yradian)
        {
            Up = Vector3.Transform(Up, Matrix.CreateRotationY(yradian));
        }

        public void UpdateCamera(float yaw, float pitch)
        {
            rotationMatrix = Matrix.CreateRotationX(pitch) *  Matrix.CreateRotationZ(yaw);
            
            transformedReference = Vector3.Transform(Reference, rotationMatrix);
            LookAt = Position + transformedReference;

        }
    }
}
