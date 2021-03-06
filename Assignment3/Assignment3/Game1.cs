using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
using Assignment3.Utilities;
using Assignment3.GameObjects;
namespace Assignment3
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static View view;
        Effect effect;
        Vector3 avatarPosition = new Vector3(-3, 2, 0);
        float avatarYaw = -1.6f;
        float avatarPitch;
        float currentavatarPitch;
        float rotationSpeed = 2f / 60f;
        float forwardSpeed = 3f / 60f;

        public static InputState input = new InputState();


        public static Renderer3D Renderer3D = new Renderer3D();

        //public static ModelLibrary ModelLib = new ModelLibrary();
        //public static TextureLibrary TexLib = new TextureLibrary();
        public static Library<Texture2D> TexLib = new Library<Texture2D>();
        public static Library<Model> ModelLib = new Library<Model>();

        public static GameWindow Wind;

        public static Random rand = new Random();
        public static bool fog = true;

        private EffectParameter cameraPositionParameter;
        private EffectParameter specularPowerParameter;
        private EffectParameter specularIntensityParameter;

        private float specularPower, specularIntensity;


        public CollisionChecker colChecker;
        Vector3 v;
        bool XrayMode = true;
        public float zoomFactor = 0.5f;
        int stepCounter;
        public static SoundPlayer soundPlayer = new SoundPlayer();


        bool night = false;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            Wind = this.Window;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // set the initial specular values
            specularPower = 20;
            specularIntensity = 1;

            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ModelLib.LoadContent(Content, "Models");
            TexLib.LoadContent(Content, "Models\\Textures");

            soundPlayer.InitSoundLibrary(Content);

            
     
            //view = new View(this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);
            view = new MazeObjects.Maze();
            //view.Add(new GameObjects.GameObject3D(ModelLib.Get("Ball"), TexLib.Get("EyeTex")));
            // TODO: use this.Content to load your game content here


            effect = Content.Load<Effect>("Shaders\\PerPixelLighting");


            // cache the effect parameters
            cameraPositionParameter = effect.Parameters["cameraPosition"];
            specularPowerParameter = effect.Parameters["specularPower"];
            specularIntensityParameter = effect.Parameters["specularIntensity"];
            cameraPositionParameter = effect.Parameters["cameraPosition"];

            //
            // set up some basic effect parameters that do not change during the
            // course of execution
            //

            // set the light colors
            effect.Parameters["ambientLightColor"].SetValue(new Vector4(255, 255, 255, 0));
            effect.Parameters["diffuseLightColor"].SetValue(Color.Red.ToVector4());
            effect.Parameters["specularLightColor"].SetValue(Color.White.ToVector4());

            // Set the light position to a fixed location.
            // This will place the light source behind, to the right, and above the
            // initial camera position.
            effect.Parameters["lightPosition"].SetValue(
                new Vector3(0f, 1f, 0f));
            effect.Parameters["lightRotation"].SetValue(view.Camera.rotationMatrix);


            effect.Parameters["FullStrengthDistance"].SetValue(0.5f);
            effect.Parameters["MaxDistance"].SetValue(1);

            colChecker = new CollisionChecker(view.GameObject3DList);
            colChecker.CreateBoxes();

            stepCounter = 0;

            soundPlayer.LoopMusic("BG1");
        }
        
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public void UpdatePosition(GameTime time)
        {
            soundPlayer.UpdateVolume();

            colChecker.CheckDistance(avatarPosition);
            #region Camera Rotation
            if (input.IsKeyDown(Keys.Left)) avatarYaw += rotationSpeed;// Rotate left.
            if (input.IsKeyDown(Keys.Right)) avatarYaw -= rotationSpeed;// Rotate right.
            if (input.RightStick.X != 0) avatarYaw -= input.RightStick.X * rotationSpeed;
            if (input.RightStick.Y != 0)
            {
                avatarPitch += input.RightStick.Y * rotationSpeed;
                if (Math.Abs(avatarPitch) > 0.3) avatarPitch = currentavatarPitch;

                currentavatarPitch = avatarPitch;
            } 
            #endregion

            if (input.IsKeyDown(Keys.W) || input.LeftStick.Y > 0.5f)
            {
               
                Matrix forwardMovement = Matrix.CreateRotationZ(avatarYaw);
                v = new Vector3(0, forwardSpeed, 0);
                v = Vector3.Transform(v, forwardMovement);

                avatarPosition.Y += v.Y;
                avatarPosition.X += v.X;
                if (XrayMode)
                {
                    if (colChecker.CheckCollision(avatarPosition))
                    {
                        avatarPosition.Y -= v.Y;
                        avatarPosition.X -= v.X;
                        soundPlayer.PlaySoundEffect("collideins");
      
                    }

                }
            }
            if (input.IsKeyDown(Keys.S) || input.LeftStick.Y < 0f)
            {
                
                Matrix forwardMovement = Matrix.CreateRotationZ(avatarYaw);
                v = new Vector3(0, -forwardSpeed, 0);
                v = Vector3.Transform(v, forwardMovement);

              
                avatarPosition.Y += v.Y;
                avatarPosition.X += v.X;
                if (XrayMode)
                {
                    if (colChecker.CheckCollision(avatarPosition))
                    {
                        avatarPosition.Y -= v.Y;
                        avatarPosition.X -= v.X;
                        soundPlayer.PlaySoundEffect("collideins");
                    }
                }
              
            }
            if (input.IsKeyDown(Keys.A) || input.LeftStick.X < 0f)
            {
               
                Matrix forwardMovement = Matrix.CreateRotationZ(avatarYaw);
                v = new Vector3(-forwardSpeed, 0, 0);
                v = Vector3.Transform(v, forwardMovement);

                avatarPosition.Y += v.Y;
                avatarPosition.X += v.X;

                if (XrayMode)
                {
                    if (colChecker.CheckCollision(avatarPosition))
                    {
                        avatarPosition.Y -= v.Y;
                        avatarPosition.X -= v.X;
                        soundPlayer.PlaySoundEffect("collideins");
                    }

                } 
            }
            if (input.IsKeyDown(Keys.D) || input.LeftStick.X > 0f)
            {
              
                
                Matrix forwardMovement = Matrix.CreateRotationZ(avatarYaw);
                v = new Vector3(forwardSpeed, 0, 0);
                v = Vector3.Transform(v, forwardMovement);
                avatarPosition.Y += v.Y;
                avatarPosition.X += v.X;

                if (XrayMode)
                {
                    if (colChecker.CheckCollision(avatarPosition))
                    {
                        avatarPosition.Y -= v.Y;
                        avatarPosition.X -= v.X;
                        soundPlayer.PlaySoundEffect("collideins");
                    }

                }
            }

            if (input.IsKeyDown(Keys.W) || input.IsKeyDown(Keys.S) || input.IsKeyDown(Keys.D) || input.IsKeyDown(Keys.A)
              || input.LeftStick.Y > 0.5f || input.LeftStick.Y < 0f || input.LeftStick.X < 0f || input.LeftStick.X > 0f)
            {
                stepCounter += (int)(Math.Sqrt(Math.Pow(v.Y, 2) + Math.Pow(v.X, 2))* 100);
               
            }

            if (input.IsKeyTapped(Keys.X) || input.IsButtonTapped(Buttons.Y)) XrayMode = !XrayMode;
            
           
            if (input.IsKeyDown(Keys.Z) || input.IsButtonDown(Buttons.B))
            {

                if (view.ZoomFactor > 0.1)
                    view.ZoomFactor -= 0.1f;
                    
            } else if ((input.IsKeyDown(Keys.Z) && input.IsKeyDown(Keys.LeftShift)) || input.IsKeyDown(Keys.C) || input.IsButtonDown(Buttons.A))
                {
                    if (view.ZoomFactor < 2)
                    {
                        view.ZoomFactor += 0.1f;
                       
                    }
                }
            if (input.IsKeyDown(Keys.Home) || input.IsButtonTapped(Buttons.Start))
            {
                avatarPosition = new Vector3(-3, 2, 0);
                view.ZoomFactor = MathHelper.PiOver4;
                avatarYaw = -1.6f;
                avatarPitch = 0;
            }

            if (input.IsKeyTapped(Keys.L) || input.IsButtonTapped(Buttons.LeftShoulder))
            {
                night = !night;
                if (night)
                {
                    soundPlayer.StopMusic();
                    soundPlayer.LoopMusic("BG2");
                }
                else
                {
                    soundPlayer.StopMusic();
                    soundPlayer.LoopMusic("BG1");
                }
            }

            if (input.IsKeyTapped(Keys.G) || input.IsButtonTapped(Buttons.RightShoulder))
            {
                fog = !fog;    
                
                effect.Parameters["FogEnabled"].SetValue(!effect.Parameters["FogEnabled"].GetValueBoolean());
            }

            if (input.IsKeyTapped(Keys.F) || input.IsButtonTapped(Buttons.RightTrigger)) effect.Parameters["FlashLightOn"].SetValue(!effect.Parameters["FlashLightOn"].GetValueBoolean());

            if (input.IsKeyTapped(Keys.M) || input.IsButtonTapped(Buttons.LeftTrigger))
            {
                soundPlayer.PauseMusic();
            }
            if (stepCounter % 13 <= 0 && stepCounter > 10)
                soundPlayer.PlaySoundEffect("footstepins");

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            input.UpdateState();
            // Allows the game to exit
            if (input.IsButtonDown(Buttons.Back) || input.IsKeyDown(Keys.Escape))
                this.Exit();

            UpdatePosition(gameTime);

            //Console.WriteLine(avatarPosition.ToString());
            view.Camera.Position = avatarPosition;

            effect.Parameters["lightPosition"].SetValue(avatarPosition);
            effect.Parameters["lightRotation"].SetValue(view.Camera.rotationMatrix);
            
            view.Camera.UpdateCamera(avatarYaw, avatarPitch);
         






            base.Update(gameTime);
        }


        private void SetSharedEffectParameters()
        {
            specularPowerParameter.SetValue(specularPower);
            specularIntensityParameter.SetValue(specularIntensity);
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            
            view.Update(gameTime);
            GraphicsDevice.Clear(Color.Gray);

            if (night){
                Renderer3D.Night();
               
                
            }
            else
            {
                Renderer3D.Day();
                
               
            }
                
            SetSharedEffectParameters();
            //Renderer3D.Render(view);
            Renderer3D.Render(view, effect);
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
