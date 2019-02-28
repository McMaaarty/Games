using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceTrouble.Sprites
{
    class Sprite
    {
        public Sprite Parent;
        public bool IsRemoved = false;

        private Texture2D _texture;
        public Texture2D texture
        {
            get
            {
                return this._texture;
            }
            set
            {
                this._texture = value;
            }
        }

        private Vector2 _position;
        public Vector2 position
        {
            get
            {
                return this._position;
            }
            set
            {
                this._position = value;
            }
        }

        public Rectangle Rectangle { get { return new Rectangle((int)position.X, (int)position.Y, _texture.Width, _texture.Height); } }

        public Sprite (Texture2D texture, Vector2 position) 
        {
            this.position = position;
            this.texture = texture;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

    }
}
