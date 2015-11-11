using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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
        View view;
        Effect effect;
        Vector3 avatarPosition = new Vector3(-3, 2, 0);
        float avatarYaw = -1.6f;
        float avatarPitch;
        float currentavatarPitch;
        float rotationSpeed = 2f / 60f;
        float forwardSpeed = 5f / 60f;

        public static Renderer3D Renderer3D = new Renderer3D();

        public static ModelLibrary ModelLib = new ModelLibrary();
        public static TextureLibrary TexLib = new TextureLibrary();
        public static GameWindow Wind;

        private EffectParameter cameraPositionParameter;
        private EffectParameter specularPowerParameter;
        private EffectParameter specularIntensityParameter;

        private float specularPower, specularIntensity;


        public CollisionChecker colChecker;
        Vector3 v;
        bool XrayMode = true;
        public float zoomFactor = 0.5f;
        GamePadState gState;
        KeyboardState kState;

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

            ModelLib.InitModelLibrary(Content);
            TexLib.InitTextureLibrary(Content);

     
            //view = new View(this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);
            view = new MazeObjects.Maze();
            //view.Add(new GameObjects.GameObject3D(ModelLib.Get("Ball"), TexLib.Get("EyeTex")));
            // TODO: use this.Content to load your game content here


            effect = Content.Load<Effect>("PerPixelLighting");


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
            effect.Parameters["ambientLightColor"].SetValue(
                new Vector4(255, 255, 255, 0));
            effect.Parameters["diffuseLightColor"].SetValue(
                Color.CornflowerBlue.ToVector4());
            effect.Parameters["specularLightColor"].SetValue(
                Color.White.ToVector4());

            // Set the light position to a fixed location.
            // This will place the light source behind, to the right, and above the
            // initial camera position.
            effect.Parameters["lightPosition"].SetValue(
                new Vector3(0f, 1f, 0f));


            effect.Parameters["FullStrengthDistance"].SetValue(0.001f);
            effect.Parameters["MaxDistance"].SetValue(10);

            colChecker = new CollisionChecker(view.GameObject3DList);
            colChecker.CreateBoxes();
        }
        
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public void UpdatePosition()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState currentState = GamePad.GetState(PlayerIndex.One);

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                // Rotate left.
                avatarYaw += rotationSpeed;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                // Rotate right.
                avatarYaw -= rotationSpeed;
            }
            if (keyboardState.IsKeyDown(Keys.W) || currentState.ThumbSticks.Left.Y > 0f)
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
                    }

                }
            }
            if (keyboardState.IsKeyDown(Keys.S) || currentState.ThumbSticks.Left.Y < 0f)
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
                    }

                }
              
            }
            if (keyboardState.IsKeyDown(Keys.A) || currentState.ThumbSticks.Left.X < 0f)
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
                    }

                }

                
            }
            if (keyboardState.IsKeyDown(Keys.D) || currentState.ThumbSticks.Left.X > 0f)
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
                    }

                }
            }

            if (currentState.ThumbSticks.Right.X != 0)
            {
                avatarYaw -= currentState.ThumbSticks.Right.X * rotationSpeed;
            }

            if (currentState.ThumbSticks.Right.Y != 0)
            {
                avatarPitch += currentState.ThumbSticks.Right.Y * rotationSpeed;
                if (Math.Abs(avatarPitch) > 0.3)
                    avatarPitch = currentavatarPitch;
                
                currentavatarPitch = avatarPitch;
            }

            if (keyboardState.IsKeyDown(Keys.X) || (currentState.Buttons.Y == ButtonState.Pressed) &&(gState.Buttons.Y != ButtonState.Pressed ))
            {
                XrayMode = !XrayMode;
            }
           
            if (keyboardState.IsKeyDown(Keys.Z) || (currentState.Buttons.B == ButtonState.Pressed))
            {

                if (view.ZoomFactor > 0.1)
                    view.ZoomFactor -= 0.1f;
                    
            } else if ((keyboardState.IsKeyDown(Keys.Z) && keyboardState.IsKeyDown(Keys.LeftShift)) || keyboardState.IsKeyDown(Keys.C) || (currentState.Buttons.A == ButtonState.Pressed))
                {
                    if (view.ZoomFactor < 2)
                    {
                        view.ZoomFactor += 0.1f;
                       
                    }
                }
            if (keyboardState.IsKeyDown(Keys.Home) || (currentState.Buttons.Start == ButtonState.Pressed) && (gState.Buttons.Start != ButtonState.Pressed))
            {
                avatarPosition = new Vector3(-3, 2, 0);
                view.ZoomFactor = MathHelper.PiOver4;
                avatarYaw = -1.6f;
                avatarPitch = 0;
            }

            if (keyboardState.IsKeyDown(Keys.L) && !kState.IsKeyDown(Keys.L))
            {
                night = !night;
            }

            gState = currentState;
            kState = keyboardState;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            UpdatePosition();

            //Console.WriteLine(avatarPosition.ToString());
            view.Camera.Position = avatarPosition;

            effect.Parameters["lightPosition"].SetValue(avatarPosition);
            view.Camera.UpdateCamera(avatarYaw, avatarPitch);
         
            // TODO: Add your update logic here

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
            GraphicsDevice.Clear(Color.Black);

            if (night)
                Renderer3D.Night();
            else
                Renderer3D.Day();
            SetSharedEffectParameters();
            //Renderer3D.Render(view);
            Renderer3D.Render(view, effect);
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
