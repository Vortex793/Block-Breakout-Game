using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
	
namespace Block_Breakout_Game
{
    public class paddle
	{
        private Rectangle _paddleRect;
        private Texture2D _paddleTexture;

        public paddle(Rectangle paddleRect, Texture2D paddleTexture)
		{
			_paddleRect = paddleRect;
			_paddleTexture = paddleTexture;
			
			public Rectangle PaddleRect
			{
				get { return _paddleRect; }
				set { _paddleRect = value; }
			}

		}



		public void Initialize()
		{
			// Initialize paddle position and size
			paddleRect = new Rectangle(400, 550, 100, 20); // Example position and size
		}

		public void LoadContent(GraphicsDevice graphicsDevice, Texture2D texture)
		{
			// Load the paddle texture and set its initial position
			paddleTexture = texture;
			paddleRect = new Rectangle(400 - paddleTexture.Width / 2, 550, paddleTexture.Width, paddleTexture.Height);
		}
		public void Update(GameTime gameTime)
		{
			// Update paddle position based on user input
			KeyboardState keyboardState = Keyboard.GetState();
			if (keyboardState.IsKeyDown(Keys.Left))
			{
				paddleRect.X -= 5; // Move left
			}
			if (keyboardState.IsKeyDown(Keys.Right))
			{
				paddleRect.X += 5; // Move right
			}
			// Keep the paddle within the screen bounds
			paddleRect.X = MathHelper.Clamp(paddleRect.X, 0, 800 - paddleRect.Width);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			// Draw the paddle
			spriteBatch.Draw(paddleTexture, paddleRect, Color.White);
		}
	}
