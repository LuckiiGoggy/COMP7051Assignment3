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
        Vector3 avatarPosition = new Vector3(0, 0, -50);
        float avatarYaw;
        float rotationSpeed = 1f / 60f;
        float forwardSpeed = 50f / 60f;
        public static Renderer3D Renderer3D = new Renderer3D();

        public static ModelLibrary ModelLib = new ModelLibrary();
        public static TextureLibrary TexLib = new TextureLibrary();
        public static GameWindow Wind;
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

            if (keyboardState.IsKeyDown(Keys.Left) || (currentState.DPad.Left == ButtonState.Pressed))
            {
                // Rotate left.
                avatarYaw += rotationSpeed;
            }
            if (keyboardState.IsKeyDown(Keys.Right) || (currentState.DPad.Right == ButtonState.Pressed))
            {
                // Rotate right.
                avatarYaw -= rotationSpeed;
            }
            if (keyboardState.IsKeyDown(Keys.Up) || (currentState.DPad.Up == ButtonState.Pressed))
            {
                Matrix forwardMovement = Matrix.CreateRotationY(avatarYaw);
                Vector3 v = new Vector3(0, 0, forwardSpeed);
                v = Vector3.Transform(v, forwardMovement);
                avatarPosition.Z += v.Z;
                avatarPosition.X += v.X;
            }
            if (keyboardState.IsKeyDown(Keys.Down) || (currentState.DPad.Down == ButtonState.Pressed))
            {
                Matrix forwardMovement = Matrix.CreateRotationY(avatarYaw);
                Vector3 v = new Vector3(0, 0, -forwardSpeed);
                v = Vector3.Transform(v, forwardMovement);
                avatarPosition.Z += v.Z;
                avatarPosition.X += v.X;
            }
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
            view.Camera.Position = avatarPosition;
            view.Camera.UpdateCamera(avatarYaw);
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Renderer3D.Render(view);
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
