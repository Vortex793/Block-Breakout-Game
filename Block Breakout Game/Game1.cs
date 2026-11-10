using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

using System.Reflection.Emit;
using System.Runtime.InteropServices.JavaScript;


namespace Block_Breakout_Game
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        enum Screen
        {
            menu,
            game,
            gameover,
            win
        }

        Rectangle window;
        Screen screen;

        paddle Paddle;

        Texture2D paddleTexture;

        Texture2D ballTexture;
        
        Texture2D blockTexture;

        private List<block> blocks;

        ball Ball;

        KeyboardState oldState;

        SpriteFont gameFont, smallerGameFont;

        SoundEffect glassHit;

        Song mainMusic, menuSong;

        Video spaceBG;
        VideoPlayer videoPlayer;
        Texture2D bgTexture;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            screen = Screen.menu;
            window = new Rectangle(0, 0, 800, 600);
            _graphics.PreferredBackBufferWidth = window.Width;
            _graphics.PreferredBackBufferHeight = window.Height;
            _graphics.ApplyChanges();
         

            blocks = new List<block>();
            base.Initialize();
            
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            paddleTexture = Content.Load<Texture2D>("paddle");
            ballTexture = Content.Load<Texture2D>("circle");
            glassHit = Content.Load<SoundEffect>("glass1");
            // TODO: use this.Content to load your game content here
            gameFont = Content.Load<SpriteFont>("gameFont");
            smallerGameFont = Content.Load<SpriteFont>("File");
            blockTexture = Content.Load<Texture2D>("rectangle");
            mainMusic = Content.Load<Song>("main song");
            menuSong = Content.Load<Song>("menu song");
            Paddle = new paddle(new Rectangle(350, 530, 120, 20), paddleTexture);
            Ball = new ball(new Rectangle(390, 510, 20, 20), ballTexture, Paddle, glassHit);
            spaceBG = Content.Load<Video>("spaceVid");
            videoPlayer = new VideoPlayer();
            //videoPlayer.IsLooped = true;
            MediaPlayer.Volume = 0.1f;
            

            CreateBlocks();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState keyboardState = Keyboard.GetState();

            
            // TODO: Add your update logic here
            if (screen == Screen.menu)
            {

                if (MediaPlayer.State != MediaState.Playing)
                {
                    MediaPlayer.Stop();
                    MediaPlayer.Play(menuSong);
                }
                    
                if (keyboardState.IsKeyDown(Keys.Enter) && oldState.IsKeyUp(Keys.Enter))
                {
                    ResetGame();
                    screen = Screen.game;
                    videoPlayer.Play(spaceBG);
                    MediaPlayer.Stop();
                    TimeSpan startTime = TimeSpan.FromSeconds(9.50);
                    MediaPlayer.Play(mainMusic, startTime);
                    MediaPlayer.IsRepeating = true;
                }

            }
            else if (screen == Screen.game)
            {


                Paddle.Update(gameTime);
                Ball.Update(gameTime, blocks);
                if (blocks.Count == 0)
                {
                    screen = Screen.win;
                }
                if (Ball.IsLost)
                {
                    screen = Screen.gameover;
                    MediaPlayer.Stop();
                }
                for (int i = 0; i < 20; i++)
                {


                   

                    //blocks.Add(new block(
                    //    blockTexture,
                    //    new Rectangle(x, y, size, size),
                    //    speed));
                }
            }
            else if (screen == Screen.gameover)
            {
                
                if (keyboardState.IsKeyDown(Keys.Enter) && oldState.IsKeyUp(Keys.Enter))
                {
                    //ResetGame();
                    MediaPlayer.Stop();
                    screen = Screen.menu;
                }
            }
            else if (screen == Screen.win)
            {
                if (keyboardState.IsKeyDown(Keys.Enter) && oldState.IsKeyUp(Keys.Enter))
                {
                    screen = Screen.menu;
                }
            }
            base.Update(gameTime);
            oldState = keyboardState;
        }
        private void CreateBlocks()
        {
            
            int blockWidth = 70;
            int blockHeight = 20;
            int spacing = 8;
            int rows = 7;
            int columns = 10;

            int startX = 14;
            int startY = 50;
            for (int row = 0; row < rows; row++)
            {
                Color[] colors =
                {

                    Color.Green,
                    Color.Blue,
                                        Color.Green,
                    Color.Blue,
                                        Color.Green,
                    Color.Blue,
                                        Color.Green,
                    Color.Blue,
                                        Color.Green,
                    Color.Blue,
                                        Color.Green,
                    Color.Blue,
                                        Color.Green,
                    Color.Blue,
                                        Color.Green,
                    Color.Blue,
                };
                Color blockColor = colors[row];
                for (int column = 0; column < columns; column++)
                {
                    int x = startX + column * (blockWidth + spacing);
                    int y = startY + row * (blockHeight + spacing);

                    Rectangle blockRect = new Rectangle(x, y, blockWidth, blockHeight);
                    blocks.Add(new block(blockTexture, blockRect, blockColor));
                }
            }
        }
        private void ResetGame()
        {
            Paddle = new paddle(new Rectangle(350, 530, 120, 20), paddleTexture);
            Ball = new ball(new Rectangle(390, 510, 20, 20), ballTexture, Paddle, glassHit);
            blocks.Clear();
            CreateBlocks();
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            if (screen == Screen.menu)
            {
                _spriteBatch.DrawString(gameFont, "Block Breakout Game", new Vector2(30, 100), Color.White);
                _spriteBatch.DrawString(gameFont, "Block Breakout Game", new Vector2(40, 110), Color.Purple);
                _spriteBatch.DrawString(smallerGameFont, "PRESS ENTER TO START", new Vector2(190, 500), Color.White);
            }
            else if (screen == Screen.game)
            {
                // Draw the video background
                //if (videoPlayer.State == MediaState.Playing)
                //{
                //    try
                //    {
                //        bgTexture = videoPlayer.GetTexture();

                //        if (bgTexture != null)
                //        {
                //            _spriteBatch.Draw(
                //                bgTexture,
                //                GraphicsDevice.Viewport.Bounds,
                //                Color.White
                //            );
                //        }
                //    }
                //    catch (InvalidOperationException)
                //    {
                //        // No video frame is available yet.
                //    }
                //}

                if (videoPlayer.State == MediaState.Playing)
                {
                    try
                    {
                        bgTexture = videoPlayer.GetTexture();

                        if (bgTexture != null)
                        {
                            _spriteBatch.Draw(
                                bgTexture,
                                new Rectangle(0, 0, window.Width, window.Height),
                                Color.White
                            );
                        }
                    }
                    catch (InvalidOperationException)
                    {
                        // Video frame not available yet
                    }
                }

                // Draw game objects over the background
                Paddle.Draw(_spriteBatch);
                Ball.Draw(_spriteBatch);

                foreach (block Block in blocks)
                {
                    Block.Draw(_spriteBatch);
                }

            }
            else if (screen == Screen.gameover)
            {
                string gameOverText = "Game Over";
                Vector2 textSize = gameFont.MeasureString(gameOverText);
                Vector2 textPosition = new Vector2((window.Width - textSize.X) / 2, (window.Height - textSize.Y) / 2);
                _spriteBatch.DrawString(gameFont, gameOverText, textPosition, Color.White);

                _spriteBatch.DrawString(smallerGameFont, "PRESS ENTER TO TRY AGAIN", new Vector2(190, 500), Color.White);
            }
            else if (screen == Screen.win)
            {
                _spriteBatch.DrawString(gameFont, "You Win!", new Vector2(260, 250), Color.White);
                _spriteBatch.DrawString(smallerGameFont, "PRESS ENTER TO PLAY AGAIN", new Vector2(160, 500), Color.White);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
