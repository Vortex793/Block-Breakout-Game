using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Block_Breakout_Game
{
    internal class ball
    {
        private Texture2D _ballTexture;
        private Rectangle _ballRect;
        private Vector2 _ballSpeed;

        paddle Paddle;   
        public ball(Rectangle ballRect, Texture2D ballTexture, paddle paddle)

        {
            _ballRect = ballRect;
            _ballTexture = ballTexture;
            Paddle = paddle;
            _ballSpeed = new Vector2(200f, 200f); // Initial speed
        }


        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Move the ball
            _ballRect.X += (int)(_ballSpeed.X * deltaTime);
            _ballRect.Y += (int)(_ballSpeed.Y * deltaTime);

            // Bounce off left and right walls
            if (_ballRect.Left <= 0 || _ballRect.Right >= 800)
            {
                _ballSpeed.X *= -1;
            }

            // Bounce off the top wall
            if (_ballRect.Top <= 0)
            {
                _ballSpeed.Y *= -1;
            }

            // Bounce off the paddle
            if (_ballRect.Intersects(Paddle.PaddleRect) && _ballSpeed.Y > 0)
            {
                _ballSpeed.Y *= -1;

                // Move ball above paddle to prevent repeated collisions
                _ballRect.Y = Paddle.PaddleRect.Top - _ballRect.Height;
            }

            // Bottom of screen (optional: handle losing a life here)
            if (_ballRect.Bottom >= 600)
            {
                _ballSpeed.Y *= -1;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_ballTexture, _ballRect, Color.White);
        }
    }
}
