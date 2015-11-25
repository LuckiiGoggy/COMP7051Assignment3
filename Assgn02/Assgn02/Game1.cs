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
using Assgn01;

namespace Assgn02
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        static Color bgColor = Color.CornflowerBlue;

        public static List<Sprite> sprites;

        public static TextObject scoreLabel;

        PhysicsEngine phys;

        SpriteController controller;
        Sprite paddle;
        public static Sprite ball;

        Assgn01.Console console;
        public static SpriteFont font;

        Texture2D WinScreen;
        Texture2D LoseScreen;
        Texture2D StartScreen;

        Boolean gameOver;
        Boolean gameWon;
        Boolean gameStarted;

        StateSnapshot snapshot;

        int score;
        int maxScore;

        Texture2D t_ball;
        Texture2D t_block;
        Texture2D t_paddle;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();

            phys = new PhysicsEngine(this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);
            sprites = new List<Sprite>();

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
            font = Content.Load<SpriteFont>("Courier New");
            snapshot = new StateSnapshot();

            scoreLabel = new TextObject("Score: " + score, font, Color.Black, false);

            t_ball = this.Content.Load<Texture2D>("Ball");
            t_block = this.Content.Load<Texture2D>("Block");
            t_paddle = this.Content.Load<Texture2D>("Paddle");
            WinScreen = this.Content.Load<Texture2D>("WinScreen");
            LoseScreen = this.Content.Load<Texture2D>("LoseScreen");
            StartScreen = this.Content.Load<Texture2D>("StartScreen");

            paddle = new Sprite(t_paddle, new Vector2(0, this.Window.ClientBounds.Height - t_paddle.Height));
            paddle.PhysType = PhysicsTransform.PhysicsType.Solid;
            
            controller = new SpriteController(paddle);

            console = new Assgn01.Console(graphics.GraphicsDevice, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);

            ball = new Sprite(t_ball,
                                    new Vector2(this.Window.ClientBounds.Width / 2 - t_ball.Width, this.Window.ClientBounds.Height / 2 - t_ball.Height)
                                    , PhysicsTransform.PhysicsType.Elastic, true);
            ball.Velocity = new Vector2(0, 0.3f);

            gameStarted = false;
            gameOver = true;

            //ResetGame();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            //Update the positions of the objects

            if(!gameOver)
            {
                 controller.Update(gameTime);

                 phys.CheckWithEachOther(ball, paddle, gameTime.ElapsedGameTime.Milliseconds);
                 phys.CheckWithEachOther(ball, sprites, gameTime.ElapsedGameTime.Milliseconds);

                foreach(Sprite sprite in sprites)
                {
                    phys.CheckWithScreenBounds(sprite); 
                    if (ball.CollisionV == new Vector2(0, -1))
                    {
                        gameOver = true;
                        gameWon = false;
                    }
                    sprite.Update(gameTime.ElapsedGameTime.Milliseconds);
                }




                RemoveToRemove();

                scoreLabel.SetText("Score: " + score);

                if(score == maxScore)
                {
                    gameOver = true;
                    gameWon = true;
                }

                if (console.IsActivated())
                {
                    console.TypeInto(snapshot.GetKeysTapped(Keyboard.GetState().GetPressedKeys()));
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape)) console.Deactivate();
                    if (snapshot.IsKeyTapped(Keyboard.GetState(), Keys.Back)) console.BackSpace();
                }
                if (Keyboard.GetState().IsKeyDown(Keys.C) && !console.IsActivated())
                {
                    console.Activate();
                }
            }
            else 
            {
                if (Keyboard.GetState().IsKeyDown(Keys.X)) ResetGame();
                if (GamePad.GetState(0).Buttons.X == ButtonState.Pressed) ResetGame();
            }


            snapshot.UpdateSnapshot(Keyboard.GetState(), Mouse.GetState(), GamePad.GetState(0));
            base.Update(gameTime);
        }

        public static List<Sprite> toRemove = new List<Sprite>();

        public static void Remove(Sprite sprite)
        {
            toRemove.Add(sprite);
        }



        public void RemoveToRemove()
        {
            foreach(Sprite sprite in toRemove)
            {
                sprites.Remove(sprite);
                score++;
            }
            toRemove.Clear();
        }

        public void ResetGame()
        {
            score = 0;
            maxScore = 0;

            int rows = 10;

            sprites.Clear();

            for (int i = 0; i < rows; i++)
            {
                for (int x = 0; x < this.Window.ClientBounds.Width / t_block.Width - 2; x++)
                {
                    sprites.Add(new Sprite(t_block, new Vector2((t_block.Width + 1) * x + t_block.Width, t_block.Height + (t_block.Height + 1) * i), PhysicsTransform.PhysicsType.Fragile));
                    maxScore++;
                }
            }

            paddle.Position = new Vector2(this.Window.ClientBounds.Width/2 - t_paddle.Width/2, this.Window.ClientBounds.Height - (t_paddle.Height * 2));

            ball.Position = new Vector2(this.Window.ClientBounds.Width / 2 - t_ball.Width, this.Window.ClientBounds.Height / 2 - t_ball.Height);
            ball.Velocity = new Vector2(0, 0.3f);

            sprites.Add(ball);
            sprites.Add(paddle);

            gameWon = false;
            gameOver = false;
            gameStarted = true;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(bgColor);

            spriteBatch.Begin();

            //Draw all objects
            if (!gameStarted)
            {
                spriteBatch.Draw(StartScreen, Vector2.Zero, Color.White);
            }
            else if (!gameOver)
            {
                foreach (Sprite sprite in sprites)
                {
                    sprite.Draw(spriteBatch);
                }

                scoreLabel.Draw(spriteBatch);


                if (console.IsActivated())
                    console.Draw(spriteBatch);

            }
            else if (gameWon)
            {
                spriteBatch.Draw(WinScreen, Vector2.Zero, Color.White);
            }
            else if (!gameWon)
            {
                spriteBatch.Draw(LoseScreen, Vector2.Zero, Color.White);
            }

            spriteBatch.End();


            base.Draw(gameTime);
        }

        public static void SetBGColor(int r, int g, int b)
        {
            bgColor = new Color(r, g, b);
        }

        public static void SetBallSpeed(float spd)
        {
            if (ball.Velocity == Vector2.Zero)
                ball.Velocity = new Vector2(0, spd);
            else
                ball.Velocity = Vector2.Normalize(ball.Velocity) * spd;
        }
    }
}