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

namespace Assignment3.GameObjects
{
    public class Transform3D
    {
        #region Members
        /*** Transformations ***/
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }
        #endregion

        /// <summary>
        /// Default constructor for the transform object, where all transform data are set to zero.
        /// </summary>
        public Transform3D()
        {
            Position = Vector3.Zero;
            Rotation = Vector3.Zero;
            Scale = new Vector3(1, 1, 1);
        }
    }
}
