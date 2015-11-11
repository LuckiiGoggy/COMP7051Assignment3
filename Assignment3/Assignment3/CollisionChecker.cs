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

namespace Assignment3
{
    public class CollisionChecker
    {

        List<GameObject3D> GameObjectList;
        List<BoundingBox> BoundingBoxes = new List<BoundingBox>();
        public CollisionChecker(List<GameObject3D> gameObject)
        {
            GameObjectList = gameObject;
        }

        public void CreateBoxes()
        {
            foreach (GameObject3D obj in GameObjectList)
            {
                BoundingBoxes.Add(CreateBoundingBox(obj.Model, obj.GetWorldMatrix()));
            }
        }

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



    }
}
