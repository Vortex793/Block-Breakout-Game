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

		private Vector2 _paddleSpeed;
		public paddle(Rectangle paddleRect, Texture2D paddleTexture)

		{
			_paddleRect = paddleRect;
			_paddleTexture = paddleTexture;
		}


			public Rectangle PaddleRect
		{
			get { return _paddleRect; }
			set { _paddleRect = value; }
		}

		public void Update(GameTime gameTime)
		{
            // Update paddle position based on user input
            KeyboardState keyboardState = Keyboard.GetState();
            _paddleSpeed = Vector2.Zero;

            if (keyboardState.IsKeyDown(Keys.A))
            {
                _paddleSpeed.X = -1;
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                _paddleSpeed.X = 1;
            }

            float speed = 300f;
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _paddleRect.X += (int)(_paddleSpeed.X * speed * deltaTime);

            _paddleRect.X = MathHelper.Clamp(_paddleRect.X, 0, 800 - _paddleRect.Width);
        }

		public void Draw(SpriteBatch spriteBatch)
		{
			// Draw the paddle

            spriteBatch.Draw(_paddleTexture, _paddleRect, Color.White);
        }
	}
}