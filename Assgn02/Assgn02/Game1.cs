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
        Texture2D HighScoreScreen;

        Boolean gameOver;
        Boolean gameWon;
        Boolean gameStarted;
        Boolean inHighScore;

        StateSnapshot snapshot;

        int score;
        int maxScore;

        Texture2D t_ball;
        Texture2D t_block;
        Texture2D t_paddle;


        public static String displayText = "";

        public bool GameSaveRequested = false;
        public bool GameLoadRequested = true;
        IAsyncResult result;

        public static List<int> HighScores = new List<int>();

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
            HighScoreScreen = this.Content.Load<Texture2D>("HighScoreScreen");

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

            if (input.IsButtonTapped(Buttons.Back) || input.IsKeyTapped(Keys.Q)) this.Exit();

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
                        UpdateHighScore();
                    }
                    sprite.Update(gameTime.ElapsedGameTime.Milliseconds);
                }




                RemoveToRemove();

                scoreLabel.SetText("Score: " + score);

                if(score == maxScore)
                {
                    gameOver = true;
                    gameWon = true;
                    UpdateHighScore();
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
            else if(!gameStarted)
            {
                if (input.IsKeyTapped(Keys.X) || input.IsButtonTapped(Buttons.X)) ResetGame();
                if (input.IsButtonTapped(Buttons.Y) || input.IsKeyTapped(Keys.Y))
                {
                    gameStarted = true;
                    inHighScore = true;
                }
            }
            else
            {
                if (input.IsKeyTapped(Keys.X) || input.IsButtonTapped(Buttons.X)) gameStarted = false;
            }


            // Check for button for saving
            if (input.IsButtonTapped(Buttons.Start))
            {
                if (!GameSaveRequested && !GameLoadRequested)
                {
                    GameSaveRequested = true;
                }
            }

            if (GameSaveRequested || GameLoadRequested)
                result = StorageDevice.BeginShowSelector(PlayerIndex.One, null, null);



            if ((GameSaveRequested || GameLoadRequested) && (result.IsCompleted))
            {
                StorageDevice device = StorageDevice.EndShowSelector(result);
                if (device != null && device.IsConnected)
                {
                    if (GameLoadRequested)
                    {
                        DoLoadGame(device);

                        GameLoadRequested = false;
                    }
                    if (GameSaveRequested)
                    {
                        DoSaveGame(device);

                        // Reset the request flag
                        GameSaveRequested = false;
                    }
                }
            }

            base.Update(gameTime);
        }

        public static List<Sprite> toRemove = new List<Sprite>();

        public static void Remove(Sprite sprite)
        {
            toRemove.Add(sprite);
        }

        private static void DoSaveGame(StorageDevice device)
        {

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
                foreach (int score in HighScores)
                {
                    System.Console.WriteLine("{0}", score);
                    sw.WriteLine("{0}", score);
                }
                sw.Close();
            }
#endif

            // Close the file.
            stream.Close();

            // Dispose the container, to commit changes.
            container.Dispose();
        }

        public static int HighScoreLimit = 10;
        private void UpdateHighScore()
        {
            HighScores.Add(score);
            List<int> NewHighScores = new List<int>();
            HighScores = HighScores.OrderByDescending(v => v).ToList();

            for (int i = 0; i < HighScoreLimit && i < HighScores.Count; i++)
                NewHighScores.Add(HighScores[i]);

            HighScores = NewHighScores;

            GameSaveRequested = true;
            
        }


        private static void DoLoadGame(StorageDevice device)
        {
            // Open a storage container.
            IAsyncResult result = device.BeginOpenContainer("Storage", null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            string filename = "savegame.sav";

            // Check to see whether the save exists.
            if (!container.FileExists(filename))
            {
                // If not, dispose of the container and return.
                container.Dispose();
                return;
            }

            // Open the file.
            Stream stream = container.OpenFile(filename, FileMode.Open);
            HighScores = new List<int>();

#if WINDEMO
            // Read the data from the file.
            XmlSerializer serializer = new XmlSerializer(typeof(SaveGameData));
            SaveGameData data = (SaveGameData)serializer.Deserialize(stream);
#else
            using (StreamReader sr = new StreamReader(stream))
            {
                System.Console.WriteLine("Loading...");
                String input;
                displayText = "";
                while ((input = sr.ReadLine()) != null)
                {
                    System.Console.WriteLine(input);
                    HighScores.Add(Int32.Parse(input));
                }
                sr.Close();
            }
#endif

            // Close the file.
            stream.Close();

            // Dispose the container.
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
            inHighScore = false;
        }

        public void CheckHighScores()
        {

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
            else if (inHighScore)
            {
                spriteBatch.Draw(HighScoreScreen, new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height), Color.White);
                List<TextObject> scoreLabels = new List<TextObject>();

                for (int i=0; i<HighScores.Count; i++)
                {
                    int sW = graphics.GraphicsDevice.Viewport.Width;
                    int sH = graphics.GraphicsDevice.Viewport.Height;
                    if (i == 9)
                        scoreLabels.Add(new TextObject("[" + (i + 1) + "]:   " + HighScores[i].ToString(), font, Color.Black, false));
                    else
                        scoreLabels.Add(new TextObject("[" + (i + 1) + "]:    " + HighScores[i].ToString(), font, Color.Black, false));
                    scoreLabels[i].Position = new Vector2(sW/3, sH/5 + sH/15 * i);
                    scoreLabels[i].Draw(spriteBatch);
                }

            
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