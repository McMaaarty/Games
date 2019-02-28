using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BroFighter.Sprites
{
    class Skeleton : Sprite
    {
        private bool _punchy;
        public bool punchy
        {
            get
            {
                return _punchy;
            }
            set
            {
                _punchy = value;
            }
        }

        public Skeleton(Texture2D texture, Vector2 velocity, Vector2 position, float speed) : base(texture, velocity, position, speed)
        {
            _punchy = false;
        }

        public void Update(Keys right, Keys left, bool human)
        {
            KeyboardState newstate = Keyboard.GetState();

            position += velocity * speed;

            if(human == true)
            {
                if (newstate.IsKeyDown(right))
                {
                    velocity = new Vector2(1, 0);
                }
                else if (newstate.IsKeyDown(left))
                {
                    velocity = new Vector2(-1, 0);
                }
                else
                {
                    velocity = new Vector2(0, 0);
                }
            }
            else
            {
                if (this.isDead == false)
                {
                    velocity = new Vector2(-1, 0);
                }
                else
                {
                    velocity = new Vector2(0, 0);
                }

            }
            
        }
    }
}
