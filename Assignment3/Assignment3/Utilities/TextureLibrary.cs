using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assignment3.Utilities
{
    /// <summary>
    /// A Library of Textures
    /// </summary>
    public class TextureLibrary
    {
        public static Dictionary<string, Texture2D> Textures { get; private set; }

        /// <summary>
        /// This will need to read from a text or ini file in the future to take care
        /// of the list, instead of hardcoding all of this in the code.
        /// </summary>
        public void InitTextureLibrary(ContentManager Content)
        {
            Textures = new Dictionary<string, Texture2D>();
            Textures.Add("EyeTex", Content.Load<Texture2D>("eye texture"));
        }

        public Texture2D Get(String key)
        {
            return Textures[key];
        }
    }


}
