using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assignment3.MazeObjects
{
    /// <summary>
    /// Walls of the Maze
    /// </summary>
    public class Wall : GameObjects.GameObject3D
    {
        public Wall()
            : base(Game1.ModelLib.Get("Wall"), Game1.TexLib.Get("WallTex"))
        {
        }

        public Wall(Vector3 startPos)
            : base(Game1.ModelLib.Get("Wall"), Game1.TexLib.Get("WallTex"))
        {
            Position = startPos;
        }
    }
}
