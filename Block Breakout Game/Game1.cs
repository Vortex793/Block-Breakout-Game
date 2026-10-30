using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        ball Ball;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            screen = Screen.game;
            window = new Rectangle(0, 0, 800, 600);
            _graphics.PreferredBackBufferWidth = window.Width;
            _graphics.PreferredBackBufferHeight = window.Height;
            _graphics.ApplyChanges();
            base.Initialize();
            
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            paddleTexture = Content.Load<Texture2D>("paddle");
            ballTexture = Content.Load<Texture2D>("circle");
            // TODO: use this.Content to load your game content here
            Paddle = new paddle(new Rectangle(350, 530, 120, 20), paddleTexture);
            Ball = new ball(new Rectangle(390, 510, 20, 20), ballTexture, Paddle);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (screen == Screen.menu)
            {
                // Update menu logic


            }
            else if (screen == Screen.game)
            {
                // Update game logic
                Paddle.Update(gameTime);
                Ball.Update(gameTime);
            }
            else if (screen == Screen.gameover)
            {
                // Update game over logic
            }
            else if (screen == Screen.win)
            {
                // Update win logic
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            Paddle.Draw(_spriteBatch);
            Ball.Draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
