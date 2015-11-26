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
using Assignment3.MazeObjects;

namespace Assignment3
{
    /// <summary>
    /// Handles the Collision between objects.
    /// </summary>
    public class CollisionChecker
    {

        List<GameObject3D> GameObjectList;
        List<BoundingBox> BoundingBoxes = new List<BoundingBox>();
        public CollisionChecker(List<GameObject3D> gameObject)
        {
            GameObjectList = gameObject;
        }

        /// <summary>
        /// Creates the bounding boxes of the objects
        /// </summary>
        public void CreateBoxes()
        {
            foreach (GameObject3D obj in GameObjectList)
            {
                if(obj is Wall || obj is Floor)
                    BoundingBoxes.Add(CreateBoundingBox(obj.Model, obj.GetWorldMatrix()));
            }
        }

        /// <summary>
        /// Creates a bounding box for an model
        /// </summary>
        /// <param name="model">Model that the bounding box will be created for</param>
        /// <param name="worldTransform">Position of the Model</param>
        /// <returns>A Bounding box for the model</returns>
        public BoundingBox CreateBoundingBox(Model model, Matrix worldTransform)
        {

            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
                    int vertexBufferSize = meshPart.NumVertices * vertexStride;

                    float[] vertexData = new float[vertexBufferSize / sizeof(float)];
                    meshPart.VertexBuffer.GetData<float>(vertexData);

                    for (int i = 0; i < vertexBufferSize / sizeof(float); i += vertexStride / sizeof(float))
                    {
                        Vector3 transformedPosition = Vector3.Transform(new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]), worldTransform);

                        min = Vector3.Min(min, transformedPosition);
                        max = Vector3.Max(max, transformedPosition);
                    }
                }
            }
            return new BoundingBox(min, max);
        }

        /// <summary>
        /// Check if a collision occured
        /// </summary>
        /// <param name="cameraPosition"> Position of the Camera</param>
        /// <returns>True if a collision occured</returns>
        public bool CheckCollision(Vector3 cameraPosition)
        {
            foreach (BoundingBox box in BoundingBoxes)
            {
                if(box.Intersects(new BoundingSphere(cameraPosition, 0.2f))){
               /* if(box.Intersects(CreateBoundingBox(Game1.ModelLib.Get("Wall"), Matrix.CreateScale(0.7f,0.7f,0.7f) * Matrix.CreateRotationX(0) * Matrix.CreateRotationY(0) *
                Matrix.CreateRotationZ(0) * Matrix.CreateTranslation(cameraPosition)))){
                * */
                   Console.WriteLine("COLLIDE");
                   return true;
               }        
            }
            Console.WriteLine("NOT");
            return false;
        }

        public void CheckDistance(Vector3 cameraPosition)
        {
            foreach (GameObject3D obj in GameObjectList)
            {
                if (obj is Frog)
                {
                    float a = (obj.Position - cameraPosition).Length();
                    if(a > 10){
                        if(Game1.soundPlayer.Volume > 0.1)
                            Game1.soundPlayer.ChangeBGVol(2 / (a + 5));
                    }


                    
                    Console.WriteLine((obj.Position - cameraPosition).Length().ToString());
                }
            }
        }


    }
}
