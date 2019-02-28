using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using DuckDuckChase.Sprites;
using System;

namespace DuckDuckChase
{

    enum GameState
    {
        preUpdate,
        update,
        postUpdate,
    } 

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont text;

        public static int screenWidth;
        public static int screenHeight;

        private Texture2D duckTexture;
        private Texture2D duckTextureDead;
        private Texture2D bgTexture;
        private Texture2D bullet;
        private Texture2D playerTexture;
        private Texture2D DogGotThemBoth;
        private Texture2D DogGotOne;
        private Texture2D DogGotThemBoth2;
        private Rectangle hitBox;
        private Rectangle hitBoxDuck;

        private Sprite player;
        private Duck duck;
        private Doggo dog;

        private AnimatedSprite animDuckFlightRight2;
        private AnimatedSprite animDuckFlightLeft2;
        private AnimatedSprite animDuckFlightUp2;
        private AnimatedSprite animDuckFlightDiagRight2;
        private AnimatedSprite animDuckFlightDiagLeft2;

        private AnimatedSprite animDogWalk; 
        private AnimatedSprite animDogFound;
        private AnimatedSprite animDogLol;

        private List<Duck> listSprite;

        private Random rdm = new Random();
        private MouseState oldState;
        private GameState state;

        private const float delaySpawn = 2;
        private float remainningDelaySpawn = delaySpawn;
        private float timer = 0;

        private const int Ammo = 3;
        private int remainningBullet = Ammo;
        private int duckCount = 0;
        private int duckCountOut = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = true;
            screenWidth = graphics.PreferredBackBufferWidth;
            screenHeight = graphics.PreferredBackBufferHeight;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            text = Content.Load<SpriteFont>("txt");
            listSprite = new List<Duck>();

            bgTexture = Content.Load<Texture2D>("stage");
            bullet = Content.Load<Texture2D>("bullet");

            Texture2D DogWalk = Content.Load<Texture2D>("WalkingDog");
            Texture2D DogFound = Content.Load<Texture2D>("FoundDog");
            DogGotThemBoth = Content.Load<Texture2D>("GotThemBoth1");
            DogGotThemBoth2 = Content.Load<Texture2D>("GotThemBoth2");
            Texture2D DogLol = Content.Load<Texture2D>("LaughtDog");
            DogGotOne = Content.Load<Texture2D>("GotThemOne");

            Texture2D duckTextureUp = Content.Load<Texture2D>("DuckFlightUp");
            Texture2D duckTextureLeft = Content.Load<Texture2D>("DuckFlightLeft");
            Texture2D duckTextureRight = Content.Load<Texture2D>("DuckFlightRight");
            Texture2D duckTextureDiagLeft = Content.Load<Texture2D>("DuckFlightDiagLeft");
            Texture2D duckTextureDiagRight = Content.Load<Texture2D>("DuckFlightDiagRight");

            duckTextureDead = Content.Load<Texture2D>("falling");
            playerTexture = Content.Load<Texture2D>("target");
            duckTexture = duckTextureRight;

            animDuckFlightRight2 = new AnimatedSprite(duckTexture, 1, 3);
            animDuckFlightUp2 = new AnimatedSprite(duckTextureUp, 1, 3);
            animDuckFlightLeft2 = new AnimatedSprite(duckTextureLeft, 1, 3);
            animDuckFlightDiagRight2 = new AnimatedSprite(duckTextureDiagRight, 1, 3);
            animDuckFlightDiagLeft2 = new AnimatedSprite(duckTextureDiagLeft, 1, 3);

            animDogWalk = new AnimatedSprite(DogWalk, 1, 5);
            animDogFound = new AnimatedSprite(DogFound, 1, 3);
            animDogLol = new AnimatedSprite(DogLol, 1, 2);

            player = new Sprite(playerTexture, new Vector2(0, 0), new Vector2(screenHeight, screenWidth / 2), 6f);
            dog = new Doggo(DogWalk, new Vector2(1, 0), new Vector2(0, 325), 1.5f);

            state = GameState.preUpdate;
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (state)
            {
                case GameState.preUpdate:
                    Pre_Update(gameTime);
                    break;
                case GameState.update:
                    The_Update(gameTime);
                    break;
                case GameState.postUpdate:
                    Post_Update(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        private void The_Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            timer = (float)gameTime.ElapsedGameTime.TotalSeconds;
            remainningDelaySpawn -= timer;

            //Handling player
            MouseState newState = Mouse.GetState();
            Vector2 mousePosition = new Vector2(newState.X, newState.Y);
            player.position = new Vector2(mousePosition.X - 25, mousePosition.Y - 25);
            hitBox = new Rectangle((int)player.position.X + 18, (int)player.position.Y + 18, 12, 12);

            if (newState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released && remainningBullet > 0)
            {
                remainningBullet--;
            }

            //Ducks move and hitbox
            foreach (var duck in listSprite)
            {
                hitBoxDuck = new Rectangle((int)duck.position.X, (int)duck.position.Y, 50, 50);
                duck.Update(gameTime);

                //hitBox and die
                if (newState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released && hitBoxDuck.Intersects(hitBox) && remainningBullet > 0)
                {
                    duck.isDead = true;
                    duckCount++;

                    if (duckCount == 2)
                    {
                        remainningDelaySpawn = delaySpawn * 1.50f;
                    }
                }

                if (duck.flightDiagLeft == true)
                    animDuckFlightDiagLeft2.Update(gameTime);
                if (duck.flightDiagRight == true)
                    animDuckFlightDiagRight2.Update(gameTime);
                if (duck.flightDown == true)
                    duck.texture = duckTextureDead;
                if (duck.flightUp == true)
                    animDuckFlightUp2.Update(gameTime);
                if (duck.flightLeft == true)
                    animDuckFlightLeft2.Update(gameTime);
                if (duck.flightRight == true)
                    animDuckFlightRight2.Update(gameTime);

            }

            for (int i = 0; i < listSprite.Count; i++)
            {
                if (listSprite[i].isDead == true)
                {
                    listSprite[i].texture = duckTextureDead;
                    listSprite[i].velocity = new Vector2(0, 1);
                }
            }

            for (int i = 0; i < listSprite.Count; i++)
            {
                if (listSprite[i].hasFlee == true)
                {
                    duckCountOut++;
                }
            }

            if (duckCount == 2 || duckCount == 1 && duckCountOut > 200 || duckCountOut > 200 )
            {
                if (remainningDelaySpawn <= 0)
                {
                    dog.isOut = false;
                    duckCountOut = 0;
                    remainningBullet = 3;
                    remainningDelaySpawn = delaySpawn;
                    state = GameState.postUpdate;
                }
            }

            if (remainningDelaySpawn <= 0)
            {
                if (listSprite.Count <= 1)
                {
                    int width = rdm.Next(200, screenHeight - duckTexture.Height * 2);
                    duck = new Duck(duckTexture, new Vector2(0, -1), new Vector2(width, 300), 2.5f);
                    listSprite.Add(duck);
                }
                remainningDelaySpawn = delaySpawn;
            }

            oldState = newState;
        }

        private void Pre_Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            timer = (float)gameTime.ElapsedGameTime.TotalSeconds;

            remainningDelaySpawn -= timer;

            if (remainningDelaySpawn <= -1)
            {
                dog.jump = true;
                remainningDelaySpawn = delaySpawn;
            }

            animDogFound.Update(gameTime);

            if (remainningDelaySpawn <= 1.50 && dog.jump == true)
            {
                dog.isOut = true;
                remainningDelaySpawn = delaySpawn;
                state = GameState.update;
            }

            animDogWalk.Update(gameTime);
            dog.Update();
        }

        private void Post_Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            timer = (float)gameTime.ElapsedGameTime.TotalSeconds;
            remainningDelaySpawn -= timer;

            animDogLol.Update(gameTime);

            if (remainningDelaySpawn <= 0 && dog.isOut == false && duckCount == 2)
            {
                dog.position = new Vector2(300, 275);
                dog.texture = DogGotThemBoth;
                dog.isOut = true;
                dog.texture = DogGotThemBoth2;
                remainningDelaySpawn = delaySpawn;
            }
            else if (remainningDelaySpawn <= 0 && dog.isOut == false && duckCount == 1)
            {
                dog.position = new Vector2(300, 275);
                dog.isOut = true;
                dog.texture = DogGotOne;
                remainningDelaySpawn = delaySpawn;
            }
            else if (remainningDelaySpawn <= 0 && dog.isOut == false && duckCount == 0)
            {
                dog.position = new Vector2(300, 275);
                dog.isOut = true;
                dog.texture = DogGotOne;
                remainningDelaySpawn = delaySpawn;
            }

            if (remainningDelaySpawn <= 0 && dog.isOut == true)
            {
                for (int i = 0; i < listSprite.Count; i++)
                {
                    listSprite.RemoveAt(i);
                    i--;
                }
                duckCount = 0;
                state = GameState.update;
                remainningDelaySpawn = delaySpawn;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            switch (state)
            {
                case GameState.preUpdate:
                    Pre_Draw(gameTime);
                    break;
                case GameState.update:
                    The_Draw(gameTime);
                    break;
                case GameState.postUpdate:
                    Post_Draw(gameTime);
                    break;
            }

            base.Draw(gameTime);
        }

        private void Pre_Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            spriteBatch.Draw(bgTexture, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);

            if (dog.isOut == false)
            {
                if (dog.jump == false)
                {
                    animDogWalk.Draw(spriteBatch, dog.position);
                }
                else
                {
                    animDogFound.Draw(spriteBatch, dog.position);
                }
            }

            spriteBatch.End();
        }

        private void The_Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            spriteBatch.Draw(bgTexture, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);

            foreach (var duck in listSprite)
            {
                if (duck.isOut == false)
                {
                    if (duck.isDead == false)
                    {
                        if (duck.flightDiagLeft == true)
                            animDuckFlightDiagLeft2.Draw(spriteBatch, duck.position);
                        if (duck.flightDiagRight == true)
                            animDuckFlightDiagRight2.Draw(spriteBatch, duck.position);
                        if (duck.flightUp == true)
                            animDuckFlightUp2.Draw(spriteBatch, duck.position);
                        if (duck.flightLeft == true)
                            animDuckFlightLeft2.Draw(spriteBatch, duck.position);
                        if (duck.flightRight == true)
                            animDuckFlightRight2.Draw(spriteBatch, duck.position);

                    }
                    else
                    {
                        spriteBatch.Draw(duck.texture, duck.position, Color.White);
                    }
                }
            }

            for (int i = 0; i < remainningBullet; i++)
                spriteBatch.Draw(bullet, new Vector2(100 + 15 * i, screenHeight - 30), Color.White);

            spriteBatch.Draw(player.texture, player.position, Color.White);

            spriteBatch.End();
        }

        private void Post_Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            spriteBatch.Draw(bgTexture, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);

            if (dog.isOut == true)
            {
                if (duckCount == 1 || duckCount == 2)
                {
                    spriteBatch.Draw(dog.texture, dog.position, Color.White);
                }
                else if (duckCount == 0)
                {
                    animDogLol.Draw(spriteBatch, dog.position);
                }

            }
            spriteBatch.End();
        }
    }
}
