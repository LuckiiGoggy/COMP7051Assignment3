using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assignment3.GameObjects;

namespace Assignment3.MazeObjects
{
    /// <summary>
    /// Floor Object of the Maze
    /// </summary>
    class Floor : GameObject3D
    {
         public Floor()
            : base(Game1.ModelLib.Get("Floor"), Game1.TexLib.Get("FloorTex"))
        {
        }

        public Floor(Vector3 startPos)
            : base(Game1.ModelLib.Get("Floor"), Game1.TexLib.Get("FloorTex"))
        {
            Scale = new Vector3(1f, 1f, 0.1f);
            Position = startPos;
        }
    }
}
