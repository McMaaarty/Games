using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceTrouble.Sprites
{
    class Invaders : Sprite
    {

        public Bullet Bullet;
        public int go;

        private bool _goLeft;
        public bool goLeft
        {
            get
            {
                return _goLeft;
            }
            set
            {
                _goLeft = value;
            }
        }

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

        private bool _lastOfTheLine;
        public bool lastOfTheLine
        {
            get
            {
                return _lastOfTheLine;
            }
            set
            {
                this._lastOfTheLine = value;
            }
        }

        private float _speed;
        public float speed
        {
            get
            {
                return _speed;
            }
            set
            {
                this._speed = value;
            }
        }

        private float _shootFreq;
        private float timer;
        private const float delay = 5;
        private float remainningDelay = delay;

        public float shootSpeed
        {
            get
            {
                return _shootFreq;
            }
            set
            {
                this._shootFreq = value;
            }
        }

        public Invaders(Texture2D texture, Vector2 position, float speed, Vector2 direction, float shootSpeed) : base(texture, position)

        {
            this.speed = speed;
            this.direction = direction;
            this.shootSpeed = shootSpeed;
            this.lastOfTheLine = false;
            goLeft = true;
            
        }

        public void Update(GameTime gameTime)
        {
            position += direction * speed;

            if (goLeft)
            {
                direction = new Vector2(1, 0);
            }else
            {
                direction = new Vector2(-1, 0);
            }

            timer = (float)gameTime.ElapsedGameTime.TotalSeconds;

            remainningDelay -= timer;

            if (remainningDelay <= 0)
            {
                speed = speed + 0.15f;
                position = new Vector2(this.position.X, this.position.Y + texture.Height);
                remainningDelay = delay;
            }
        }

        public void AddBullet(List<Bullet> sprites)
        {
            var bullet = Bullet.Clone() as Bullet;
            bullet.direction = new Vector2(0,1);
            bullet.position = this.position;
            bullet.position += bullet.direction * 2f;
            bullet.LinearVelocity = shootSpeed;
            bullet.Parent = this;

            sprites.Add(bullet);
        }

    }
}
