using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckDuckChase.Sprites
{
    class Doggo : Sprite
    {

        private bool _jump;
        public bool jump
        {
            get
            {
                return _jump;
            }
            set
            {
                _jump = value;
            }
        }

        public Doggo(Texture2D texture, Vector2 velocity, Vector2 position, float speed) : base(texture, velocity, position, speed)
        {
            _jump = false;
        }

        public void Update()
        {
            position += velocity * speed;

            if (this.position.X >= Game1.screenWidth)
            {
                this.isOut = true;
            }

            if (this.jump == true)
            {
                velocity = new Vector2(0, -1);
                speed = 2f;
            }
        }
    }
}
