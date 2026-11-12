using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

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

        List<SoundEffect> glassSounds;
        Random random = new Random();

        Song mainMusic, menuSong, deathMusic;

        Video spaceBG;
        VideoPlayer videoPlayer;
        Texture2D bgTexture;
        TimeSpan lastVideoPosition = TimeSpan.Zero;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
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

            glassSounds = new List<SoundEffect>
            {
                Content.Load<SoundEffect>("glass1"),
                Content.Load<SoundEffect>("glass2"),
                Content.Load<SoundEffect>("glass3")
            };

            gameFont = Content.Load<SpriteFont>("gameFont");
            smallerGameFont = Content.Load<SpriteFont>("File");
            blockTexture = Content.Load<Texture2D>("rectangle");

            //Tunes
            mainMusic = Content.Load<Song>("main song");
            menuSong = Content.Load<Song>("menu song");
            deathMusic = Content.Load<Song>("MissingDemo");

            Paddle = new paddle(new Rectangle(350, 530, 120, 20), paddleTexture);
            Ball = new ball(new Rectangle(390, 510, 20, 20), ballTexture, Paddle, glassSounds, random);

            spaceBG = Content.Load<Video>("spaceVid");
            videoPlayer = new VideoPlayer();
            //videoPlayer.IsLooped = true;
            videoPlayer.Volume = 0f;
            videoPlayer.Play(spaceBG);

            MediaPlayer.Volume = 0.1f;

            CreateBlocks();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState keyboardState = Keyboard.GetState();

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

                    MediaPlayer.Stop();
                    TimeSpan startTime = TimeSpan.FromSeconds(9.50);
                    MediaPlayer.Play(mainMusic, startTime);
                    MediaPlayer.IsRepeating = true;
                }
            }
            else if (screen == Screen.game)
            {
                if (videoPlayer.State == MediaState.Playing)
                {
                    if (videoPlayer.PlayPosition - lastVideoPosition >= TimeSpan.FromMilliseconds(33) ||
                        videoPlayer.PlayPosition < lastVideoPosition)
                    {
                        try
                        {
                            bgTexture = videoPlayer.GetTexture();
                            lastVideoPosition = videoPlayer.PlayPosition;
                        }
                        catch (InvalidOperationException)
                        {
                            // Frame not ready yet
                        }
                    }
                }

                Paddle.Update(gameTime);
                Ball.Update(gameTime, blocks);

                if (blocks.Count == 0)
                {
                    screen = Screen.win;
                    MediaPlayer.Stop();
                }

                if (Ball.IsLost)
                {
                    screen = Screen.gameover;
                    MediaPlayer.Stop();
                }
            }
            else if (screen == Screen.gameover)
            {


                if (MediaPlayer.State != MediaState.Playing)
                {
                    MediaPlayer.Stop();
                    MediaPlayer.Play(deathMusic);
                }

                if (keyboardState.IsKeyDown(Keys.Enter) && oldState.IsKeyUp(Keys.Enter))
                {
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

            Color[] colors =
            {
                Color.Green,
                Color.Blue,
                Color.Green,
                Color.Blue,
                Color.Green,
                Color.Blue,
                Color.Green
            };

            for (int row = 0; row < rows; row++)
            {
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
            Ball = new ball(new Rectangle(390, 510, 20, 20), ballTexture, Paddle, glassSounds, random);
            blocks.Clear(); 
            CreateBlocks();

            lastVideoPosition = TimeSpan.Zero;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            if (screen == Screen.menu)
            {
                _spriteBatch.DrawString(gameFont, "Block Breakout Game", new Vector2(30, 100), Color.White);
                _spriteBatch.DrawString(gameFont, "Block Breakout Game", new Vector2(40, 110), Color.Purple);
                _spriteBatch.DrawString(smallerGameFont, "PRESS ENTER TO START", new Vector2(190, 500), Color.White);
            }
            else if (screen == Screen.game)
            {
                if (bgTexture != null && !bgTexture.IsDisposed)
                {
                    _spriteBatch.Draw(
                        bgTexture,
                        new Rectangle(0, 0, window.Width, window.Height),
                        Color.White
                    );
                }

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