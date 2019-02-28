using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceTrouble.Sprites
{
    class Player : Sprite
    {

        public Bullet Bullet;
        private List<Bullet> listBullet = new List<Bullet>();

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

        public Player (Texture2D texture, Vector2 position,float speed,Vector2 direction) : base (texture, position)
        {
            this.speed = speed;
            this.direction = direction;
        }

        public void AddBullet(List<Bullet> sprites)
        {
            var bullet = Bullet.Clone() as Bullet;
            bullet.direction = new Vector2(0,-1);
            bullet.position = new Vector2 (position.X + texture.Width /2,position.Y);
            bullet.position += bullet.direction * 2f;
            bullet.LinearVelocity = 10f;
            bullet.Parent = this;

            sprites.Add(bullet);
        }
    }
}
