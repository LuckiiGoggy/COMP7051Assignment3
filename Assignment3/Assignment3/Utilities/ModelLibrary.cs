using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assignment3.Utilities
{
    /// <summary>
    /// Libaray of Models to be used in the game.
    /// </summary>
    public class ModelLibrary
    {
        public static Dictionary<String, Model> Models = new Dictionary<String, Model>();

        public void InitModelLibrary(ContentManager Content)
        {
            Models.Add("Wall", Content.Load<Model>("MazeWallModel"));
            Models.Add("Floor", Content.Load<Model>("MazeFloorModel"));
        }

        public Model Get(String key)
        {
            return Models[key];
        }
    }
}
