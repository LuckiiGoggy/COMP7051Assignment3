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

        public int[,] m_MazeMap;

        /// <summary>
        /// Constructor of Maze View.
        /// </summary>
        public Maze()
            : base(Game1.Wind.ClientBounds.Width, Game1.Wind.ClientBounds.Height)
        {



            int [,] mazeSpec = {{1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                               {2, 0, 1, 1, 0, 1, 0, 0, 0, 1, 0, 0, 1, 0, 1},
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
                    if (mazeSpec[row, col] == 2)
                    {
                        Add(new Frog(new Vector3(2 * col, 2 * row, 0), new Vector2(row, col)));
                        Add(new Floor(new Vector3(2 * col, 2 * row, -1)));
                    }
                    if(mazeSpec[row,col] == 1)
                        Add(new Wall(new Vector3(2 * col, 2 * row, 0)));
                    if (mazeSpec[row, col] == 0)
                        Add(new Floor(new Vector3(2 * col, 2 * row, -1)));
                }
            }

            m_MazeMap = mazeSpec;
        }

        public List<Vector2> GetAdjacentCells(Vector2 centerPos)
        {
            List<Vector2> result = new List<Vector2>();
            int[] rows = {(int)centerPos.X - 1, (int)centerPos.X + 1, (int)centerPos.X - 1, (int)centerPos.X + 1};
            int[] cols = {(int)centerPos.Y - 1, (int)centerPos.Y + 1, (int)centerPos.Y + 1, (int)centerPos.Y - 1};


            for (int i = 0; i < rows.Length; i++)
            {
                if (rows[i] >= 0 && cols[i] >= 0) result.Add(new Vector2(rows[i], cols[i]));
            }

            return result;
        }

        public List<Vector2> GetAdjacentAvailableCells(Vector2 centerPos)
        {
            List<Vector2> result = new List<Vector2>();
            int[] rows = { (int)centerPos.X - 1, (int)centerPos.X + 1, (int)centerPos.X, (int)centerPos.X };
            int[] cols = { (int)centerPos.Y, (int)centerPos.Y, (int)centerPos.Y + 1, (int)centerPos.Y - 1 };


            for (int i = 0; i < rows.Length; i++)
            {
                if (rows[i] >= 0 && cols[i] >= 0) 
                    if(m_MazeMap[rows[i], cols[i]] == 0)
                        result.Add(new Vector2(rows[i], cols[i]));
            }

            return result;
        }
    }
}
