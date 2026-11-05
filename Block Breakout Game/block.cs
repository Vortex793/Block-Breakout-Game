using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Block_Breakout_Game
{
    public class block
    {
        private Texture2D _Texture;
        private Rectangle _Rect;
        private bool _isVisible;
        private Color _Color;










        public block(Texture2D texture, Rectangle rectangle)
        {
            _Texture = texture;
            _Rect = rectangle;
            _isVisible = true;
        }

        public Rectangle Rect
        {
            get { return _Rect; }
        }

        public bool IsVisible
        {
            get { return _isVisible; }
            
        }

        public void Destroy()
        {
            _isVisible = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_isVisible)
            {
                spriteBatch.Draw(_Texture, _Rect, Color.White);
            }
        }
    }
}
