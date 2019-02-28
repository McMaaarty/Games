using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckDuckChase.Sprites
{
    class Duck : Sprite
    {

        Random rdm = new Random();

        private bool _flightUp;
        public bool flightUp
        {
            get
            {
                return _flightUp;
            }
            set
            {
                _flightUp = value;
            }
        }

        private bool _flightDown;
        public bool flightDown
        {
            get
            {
                return _flightDown;
            }
            set
            {
                _flightDown = value;
            }
        }

        private bool _flightLeft;
        public bool flightLeft
        {
            get
            {
                return _flightLeft;
            }
            set
            {
                _flightLeft = value;
            }
        }

        private bool _flightRight;
        public bool flightRight
        {
            get
            {
                return _flightRight;
            }
            set
            {
                _flightRight = value;
            }
        }

        private bool _flightDiagLeft;
        public bool flightDiagLeft
        {
            get
            {
                return _flightDiagLeft;
            }
            set
            {
                _flightDiagLeft = value;
            }
        }

        private bool _flightDiagRight;
        public bool flightDiagRight
        {
            get
            {
                return _flightDiagRight;
            }
            set
            {
                _flightDiagRight = value;
            }
        }

        private bool _hasFlee;
        public bool hasFlee
        {
            get
            {
                return _hasFlee;
            }
            set
            {
                _hasFlee = value;
            }
        }

        private const float delay = 0.5f;
        private float remainningDelay = delay;
        private float timer;

        public Duck (Texture2D texture, Vector2 velocity, Vector2 position, float speed) : base( texture,  velocity,  position,  speed)
        {
            _flightDiagLeft = false;
            _flightDiagRight = false;
            _flightDown = false;
            _flightLeft = false;
            _flightRight = false;
            _flightUp = true;
            hasFlee = false;

        }

        public void Update(GameTime gameTime)
        {
            position += velocity * speed;

            if (this.position.Y <= 0 - this.texture.Height)
            {
                this.hasFlee = true;
            }

            if (this.position.Y >= 300 && this.isDead == true)
            {
                this.isOut = true;
            }

            timer = (float)gameTime.ElapsedGameTime.TotalSeconds;

            remainningDelay -= timer;

            if (remainningDelay <= 0)
            {
                int flee = rdm.Next(0, 100);

                if (flee <= 25)
                {
                    flightUp = false;
                    flightDiagLeft = false;
                    flightLeft = false;
                    flightRight = true;
                    flightDiagRight = false;
                }
                else if (flee > 25 && flee <= 50)
                {
                    flightUp = false;
                    flightDiagLeft = false;
                    flightLeft = true;
                    flightRight = false;
                    flightDiagRight = false;
                }
                else if (flee > 50 && flee <= 75)
                {
                    flightUp = false;
                    flightDiagLeft = false;
                    flightLeft = false;
                    flightRight = false;
                    flightDiagRight = true;
                }
                else if (flee > 75)
                {
                    flightUp = false;
                    flightDiagLeft = true;
                    flightLeft = false;
                    flightRight = false;
                    flightDiagRight = false;
                }
                remainningDelay = delay;
            }

            if (flightUp == true)
                velocity = new Vector2(0, -1);
            if (flightDown == true)
                velocity = new Vector2(0, 1);
            if (flightLeft == true)
                velocity = new Vector2(-1, 0);
            if (flightRight == true)
                velocity = new Vector2(1, 0);
            if (flightDiagLeft == true)
                velocity = new Vector2(-1, -1);
            if (flightDiagRight == true)
                velocity = new Vector2(1, -1);
        }
    }
}
