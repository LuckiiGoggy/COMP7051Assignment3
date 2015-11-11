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
        float lightIntensity = 1;
        public void Day()
        {
            lightIntensity = 20;
        }

        public void Night()
        {
            lightIntensity = 0.3f;
        }

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
                        effect.AmbientLightColor = new Vector3(lightIntensity, lightIntensity, lightIntensity);
                        effect.DirectionalLight0.Direction = new Vector3(0, 0, 0);
                        effect.DirectionalLight0.SpecularColor = new Vector3(0, 0, 0);
                        Game1.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
                    }

                    mesh.Draw();
                }
            }
        }

    }
}
