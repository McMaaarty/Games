using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckDuckChase.Sprites
{
    class Sprite
    {

        private Texture2D _texture;
        private Vector2 _position;
        private Vector2 _velocity;
        private float _speed;
        private bool _isDead;
        private bool _isOut;

        public Texture2D texture
        {
            get { return _texture; }
            set { _texture = value; }
        }
        public Vector2 position
        {
            get { return _position; }
            set { _position = value; }
        }
        public Vector2 velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }
        public float speed
        {
            get { return _speed; }
            set { _speed = value; }
        }
        public bool isDead
        {
            get { return _isDead; }
            set { _isDead = value; }
        }
        public bool isOut
        {
            get { return _isOut; }
            set { _isOut = value; }
        }
        
        public Rectangle Rectangle { get { return new Rectangle((int)position.X, (int)position.Y, _texture.Width, _texture.Height); } }

        public Sprite(Texture2D texture,Vector2 velocity, Vector2 position, float speed)
        {
            this.texture = texture;
            this.velocity = velocity;
            this.position = position;
            this.speed = speed;
            _isDead = false;
            _isOut = false;
        }

    }
}
