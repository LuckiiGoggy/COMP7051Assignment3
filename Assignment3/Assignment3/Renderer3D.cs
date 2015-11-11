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
    
    public class Renderer3D
    {

       

        public void Render(View view){
            
         
            Game1.graphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            Game1.graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Game1.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            Game1.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

            foreach (GameObject3D gameObject in view.GameObject3DList)
            {
                Matrix World = gameObject.GetWorldMatrix();

                foreach (ModelMesh mesh in gameObject.Model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.TextureEnabled = true;
                        effect.Texture = gameObject.Texture;
                        effect.Projection = view.GetProjectionMatrix();
                        effect.World = World;
                        effect.View = view.Camera.GetView();
                        effect.LightingEnabled = true;
                        effect.DirectionalLight0.DiffuseColor = new Vector3(1, 1, 1);
                        effect.AmbientLightColor = new Vector3(1, 1, 1);
                        effect.DirectionalLight0.Direction = new Vector3(0, -1, 0);
                        effect.DirectionalLight0.SpecularColor = new Vector3(0, 1, 0);
                        Game1.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
                    }

                    mesh.Draw();
                }
            }
        }

        public void Render(View view, Effect effect)
        {
            Game1.graphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            Game1.graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Game1.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            Game1.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;


            effect.Parameters["view"].SetValue(view.Camera.GetView());
            effect.Parameters["projection"].SetValue(view.GetProjectionMatrix());
            effect.CurrentTechnique = effect.Techniques[2];



            foreach (GameObject3D gameObject in view.GameObject3DList)
            {
                Matrix World = gameObject.GetWorldMatrix();
                effect.Parameters["world"].SetValue(World);
                foreach (ModelMesh mesh in gameObject.Model.Meshes)
                {
                    int passCount = effect.CurrentTechnique.Passes.Count;
                    for (int i = 0; i < passCount; i++)
                    {
                        // EffectPass.Apply will update the device to
                        // begin using the state information defined in the current pass
                        effect.CurrentTechnique.Passes[i].Apply();

                        foreach (ModelMeshPart meshPart in mesh.MeshParts)
                        {
                            meshPart.Effect = effect;
                            effect.Parameters["ambientLightColor"].SetValue(
                                new Vector4(0, 0, 0, 255));
                            effect.Parameters["diffuseLightColor"].SetValue(
                                Color.CornflowerBlue.ToVector4());
                            effect.Parameters["specularLightColor"].SetValue(
                                Color.White.ToVector4());
                            effect.Parameters["ModelTexture"].SetValue(gameObject.Texture);
                        }
                        mesh.Draw();
                    }
                }
            }
        }
    }
}
