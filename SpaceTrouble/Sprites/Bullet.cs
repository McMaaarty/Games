using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceTrouble.Sprites
{
    class Bullet : Sprite
    {
        private Vector2 _direction;
        public Vector2 direction
        {
            get
            {
                return _direction;
            }
            set
            {
                _direction = value;
            }
        }

        public float LinearVelocity = 4f;

        public Bullet(Texture2D texture, Vector2 position)
          : base(texture ,position)
        {

        }

        public void Update(GameTime gameTime)
        {
            if (this.position.Y < 0 || this.position.Y > Game1.screenHeight)
                this.IsRemoved = true;

            position += direction * LinearVelocity;
        }
    }
}
