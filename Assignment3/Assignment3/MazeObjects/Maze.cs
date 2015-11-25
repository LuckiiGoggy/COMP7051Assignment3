using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assignment3.GameObjects;
namespace Assignment3.MazeObjects
{
    /// <summary>
    /// The Maze View handles the rendering and creation of the maze.
    /// </summary>
    class Maze : View
    {

        /// <summary>
        /// Constructor of Maze View.
        /// </summary>
        public Maze()
            : base(Game1.Wind.ClientBounds.Width, Game1.Wind.ClientBounds.Height)
        {



            int[,] mazeSpec = {{1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                               {0, 0, 1, 1, 0, 1, 0, 0, 0, 1, 0, 0, 1, 0, 1},
                               {1, 0, 1, 0, 0, 1, 0, 1, 1, 1, 1, 0, 0, 0, 1},
                               {1, 0, 1, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1, 1, 1},
                               {1, 0, 0, 0, 1, 0, 1, 1, 0, 1, 1, 0, 0, 0, 1},
                               {1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 1, 0, 1},
                               {1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1},
                               {1, 0, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0, 1, 0, 1},
                               {1, 0, 1, 1, 0, 1, 0, 1, 1, 1, 1, 0, 0, 0, 1},
                               {1, 0, 1, 0, 1, 1, 1, 0, 0, 1, 0, 0, 1, 0, 1},
                               {1, 0, 0, 0, 0, 0, 1, 1, 0, 1, 1, 0, 1, 0, 1},
                               {1, 0, 1, 1, 0, 0, 0, 1, 0, 1, 0, 0, 1, 0, 1},
                               {1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1},
                               {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1}};

            for (int row = 0; row < mazeSpec.GetLength(0); row++)
            {
                for (int col = 0; col < mazeSpec.GetLength(1); col++)
                {
                    if(mazeSpec[row,col] == 1)
                        Add(new Wall(new Vector3(2 * col, 2 * row, 0)));
                    if (mazeSpec[row, col] == 0)
                        Add(new Floor(new Vector3(2 * col, 2 * row, -1)));
                }
            }
        }
    }
}
