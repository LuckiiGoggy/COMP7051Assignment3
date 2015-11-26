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
using Microsoft.Xna.Framework.Storage;
using System.IO;

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
        public static SoundPlayer soundPlayer;
        public static List<Sprite> sprites;

        public static TextObject scoreLabel;

        PhysicsEngine phys;

        SpriteController controller;
        Sprite paddle;
        public static Sprite ball;

        Assgn01.Console console;
        public static SpriteFont font;

        public GeroUtilities.InputState input = new GeroUtilities.InputState();

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


        public bool GameSaveRequested = false;
        IAsyncResult result;

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

            phys = new PhysicsEngine(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
            sprites = new List<Sprite>();
            soundPlayer = new SoundPlayer();
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
            soundPlayer.InitSoundLibrary(Content);
            scoreLabel = new TextObject("Score: " + score, font, Color.Black, false);

            t_ball = this.Content.Load<Texture2D>("Ball");
            t_block = this.Content.Load<Texture2D>("Block");
            t_paddle = this.Content.Load<Texture2D>("Paddle");
            WinScreen = this.Content.Load<Texture2D>("WinScreen");
            LoseScreen = this.Content.Load<Texture2D>("LoseScreen");
            StartScreen = this.Content.Load<Texture2D>("StartScreen");

            paddle = new Sprite(t_paddle, new Vector2(0, graphics.GraphicsDevice.Viewport.Height - t_paddle.Height));
            paddle.PhysType = PhysicsTransform.PhysicsType.Solid;
            
            controller = new SpriteController(paddle);

            console = new Assgn01.Console(graphics.GraphicsDevice, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);

            ball = new Sprite(t_ball,
                                    new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - t_ball.Width, graphics.GraphicsDevice.Viewport.Height / 2 - t_ball.Height)
                                    , PhysicsTransform.PhysicsType.Elastic, true);
            ball.Velocity = new Vector2(0, 0.3f);

            gameStarted = false;
            gameOver = true;
            soundPlayer.LoopMusic("BG1");
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
            //Update the input
            input.UpdateState();

            //Update the positions of the objects
            if (!gameStarted || gameOver)
            {
                soundPlayer.PauseMusic();
            }
            if (gameStarted && !gameOver)
            {
                soundPlayer.ResumeMusic();
            }

            if(!gameOver)
            {
                //
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
                    soundPlayer.PauseMusic();
                }

                if (console.IsActivated())
                {
                    console.TypeInto(input.GetKeysTapped());
                    if (input.IsKeyTapped(Keys.Escape)) console.Deactivate();
                    if (input.IsKeyTapped(Keys.Back)) console.BackSpace();
                }
                if (input.IsKeyDown(Keys.C) && !console.IsActivated())
                {
                    console.Activate();
                }
            }
            else 
            {
                if (input.IsKeyDown(Keys.X) || input.IsButtonDown(Buttons.X)){
                    ResetGame();
                    //soundPlayer.PauseMusic();
                }
                
            }


            // Check for button for saving
            if (input.IsButtonTapped(Buttons.Start))
            {
                if ((GameSaveRequested == false))
                {
                    GameSaveRequested = true;
                    result = StorageDevice.BeginShowSelector(
                            PlayerIndex.One, null, null);
                }
            }

            if ((GameSaveRequested) && (result.IsCompleted))
            {
                StorageDevice device = StorageDevice.EndShowSelector(result);
                if (device != null && device.IsConnected)
                {
                    DoSaveGame(device);
                    //DoLoadGame(device);
                }
                // Reset the request flag
                GameSaveRequested = false;
            }


            snapshot.UpdateSnapshot(Keyboard.GetState(), Mouse.GetState(), GamePad.GetState(0));

            
            base.Update(gameTime);
        }

        public static List<Sprite> toRemove = new List<Sprite>();

        public static void Remove(Sprite sprite)
        {
            toRemove.Add(sprite);
        }


        public struct SaveGameData
        {
            public List<int> Scores;
        }

        private static void DoSaveGame(StorageDevice device)
        {
            // Create the data to save.
            SaveGameData data = new SaveGameData();
            data.Scores = new List<int>();
            data.Scores.Add(10);
            data.Scores.Add(13);

            // Open a storage container.
            IAsyncResult result = device.BeginOpenContainer("Storage", null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            string filename = "savegame.sav";

            // Check to see whether the save exists.
            if (container.FileExists(filename))
                // Delete it so that we can create one fresh.
                container.DeleteFile(filename);

            // Create the file.
            Stream stream = container.CreateFile(filename);


#if WINDEMO
            // Convert the object to XML data and put it in the stream.
            XmlSerializer serializer = new XmlSerializer(typeof(SaveGameData));
            serializer.Serialize(stream, data);
#else
            using (StreamWriter sw = new StreamWriter(stream))
            {
                foreach (int score in data.Scores)
                {
                    System.Console.WriteLine("Avatar {0}", score);
                    sw.WriteLine("Avatar {0}", score);
                }
                sw.Close();
            }
#endif

            // Close the file.
            stream.Close();

            // Dispose the container, to commit changes.
            container.Dispose();
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
                for (int x = 0; x < graphics.GraphicsDevice.Viewport.Width / t_block.Width - 2; x++)
                {
                    sprites.Add(new Sprite(t_block, new Vector2((t_block.Width + 1) * x + t_block.Width, t_block.Height + (t_block.Height + 1) * i), PhysicsTransform.PhysicsType.Fragile));
                    maxScore++;
                }
            }

            paddle.Position = new Vector2(graphics.GraphicsDevice.Viewport.Width/2 - t_paddle.Width/2, graphics.GraphicsDevice.Viewport.Height - (t_paddle.Height * 2));

            ball.Position = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - t_ball.Width, graphics.GraphicsDevice.Viewport.Height / 2 - t_ball.Height);
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
                spriteBatch.Draw(StartScreen, new Rectangle(0, 0,
            graphics.GraphicsDevice.Viewport.Width,
            graphics.GraphicsDevice.Viewport.Height), Color.White);
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
                soundPlayer.PauseMusic();
                spriteBatch.Draw(WinScreen, new Rectangle(0,0,graphics.GraphicsDevice.Viewport.Width,graphics.GraphicsDevice.Viewport.Height), Color.White);
            }
            else if (!gameWon)
            {
                soundPlayer.PauseMusic();
                spriteBatch.Draw(LoseScreen, new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), Color.White);
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