using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assignment3.Utilities
{
    public class ModelLibrary
    {
        public static Dictionary<String, Model> Models = new Dictionary<String, Model>();

        public void InitModelLibrary(ContentManager Content)
        {
            Models.Add("Ball", Content.Load<Model>("balls"));
        }

        public Model Get(String key)
        {
            return Models[key];
        }
    }
}
