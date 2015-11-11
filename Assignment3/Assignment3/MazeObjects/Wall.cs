using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assignment3.MazeObjects
{
    public class Wall : GameObjects.GameObject3D
    {
        public Wall()
            : base(Game1.ModelLib.Get("Ball"), Game1.TexLib.Get("EyeTex"))
        {
        }

        public Wall(Vector3 startPos)
            : base(Game1.ModelLib.Get("Ball"), Game1.TexLib.Get("EyeTex"))
        {
            Position = startPos;
        }
    }
}
