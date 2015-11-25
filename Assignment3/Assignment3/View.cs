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

using Assignment3.GameObjects;

namespace Assignment3
{
    public class View
    {
        public List<GameObject3D> GameObject3DList { get; private set; }

        public Camera Camera { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public float ZoomFactor { get; set; }
        public View(float width, float height)
        {
            GameObject3DList = new List<GameObject3D>();
            Width = width;
            Height = height;
            ZoomFactor = MathHelper.PiOver4;
            Camera = new Camera();
        }

        public void Add(GameObject3D gameObject3D)
        {
            GameObject3DList.Add(gameObject3D);
        }

        public void Destroy(GameObject3D gameObject3D)
        {
            GameObject3DList.Remove(gameObject3D);
        }

        public Matrix GetProjectionMatrix()
        {
            return Matrix.CreatePerspectiveFieldOfView(ZoomFactor, Width / Height, 0.1f, 99999f);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (GameObject3DList.Count > 0)
                foreach (GameObject3D obj in GameObject3DList)
                    obj.Update(gameTime);
        }
    }
}
