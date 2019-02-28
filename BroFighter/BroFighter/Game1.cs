using BroFighter.Sprites;
using DuckDuckChase.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace BroFighter
{
    enum GameState
    {
        pre_state,
        game_state,
        end_state,
    }

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        Random rdm = new Random();
        GameState state;

        AnimatedSprite skeletonWalk;
        AnimatedSprite skeletonWalkLeft;
        AnimatedSprite skeletonStand;
        AnimatedSprite skeletonDie;
        AnimatedSprite skeletonPunch;

        Texture2D Stand;
        Texture2D endPanel;
        Texture2D deadBody;

        Skeleton skeleton;
        Skeleton skeletons;
        List<Skeleton> listSkeletons;

        private const float delaySpawn = 3;
        private float remainningDelaySpawn = delaySpawn;
        private float remainningDelayDead = delaySpawn / 2;
        private float timer = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Stand = Content.Load<Texture2D>("skeletonStand");
            font = Content.Load<SpriteFont>("font");
            endPanel = Content.Load<Texture2D>("endPanel");
            deadBody = Content.Load<Texture2D>("deadBody");
            Texture2D Walk = Content.Load<Texture2D>("skeletonWalk");
            Texture2D WalkLeft = Content.Load<Texture2D>("skeletonWalkLeft");
            Texture2D Die = Content.Load<Texture2D>("skeletonDead");
            Texture2D Punch = Content.Load<Texture2D>("skeletonAttack");

            skeletonWalk = new AnimatedSprite(Walk,1,10);
            skeletonWalkLeft = new AnimatedSprite(WalkLeft, 1, 10);
            skeletonStand = new AnimatedSprite(Stand, 1, 10);
            skeletonDie = new AnimatedSprite(Die, 1, 10);
            skeletonPunch = new AnimatedSprite(Punch, 1, 10);

            listSkeletons = new List<Skeleton>();
            listSkeletons.Add(skeletons = new Skeleton(Stand, new Vector2(-1, 0), new Vector2(600, 300), 0.3f));
            skeleton = new Skeleton(Stand, new Vector2(1, 0), new Vector2(0, 300), 0.3f);
            state = GameState.pre_state;

        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            switch (state)
            {
                case GameState.pre_state:
                    Pre_Update(gameTime);
                    break;
                case GameState.game_state:
                    Game_Update(gameTime);
                    break;
                case GameState.end_state:
                    End_Update(gameTime);
                    break;
            }
        }

        private void Pre_Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                state = GameState.game_state;
            }
        }

        private void End_Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

        }

        private void Game_Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState newstate = Keyboard.GetState();

            skeleton.Update( Keys.D, Keys.Q, true);

            timer = (float)gameTime.ElapsedGameTime.TotalSeconds;
            remainningDelaySpawn -= timer;
            remainningDelayDead -= timer;

            if (skeleton.velocity.Equals(new Vector2(1, 0)))
            {
                skeletonWalk.Update(gameTime);
            }
            else if (skeleton.velocity.Equals(new Vector2(-1, 0)))
            {
                skeletonWalkLeft.Update(gameTime);
            }
            else if (skeleton.velocity.Equals(new Vector2(0, 0)))
            {
                if (newstate.IsKeyDown(Keys.Space))
                {
                    skeleton.punchy = true;
                    skeletonPunch.Update(gameTime);
                }
                else
                {
                    skeleton.punchy = false;
                    skeletonStand.Update(gameTime);
                }
            }

            if (remainningDelaySpawn <= 0)
            {
                int width = rdm.Next(650,1000);

                listSkeletons.Add(skeletons = new Skeleton(Stand, new Vector2(-1, 0), new Vector2(width, 300), 0.3f));
                remainningDelaySpawn = delaySpawn;
            }

            foreach (var skull in listSkeletons)
            {
                skull.Update(Keys.J, Keys.K, false);

                if (skeleton.punchy == true && skeleton.Rectangle.Intersects(skull.Rectangle))
                {
                    remainningDelayDead = 0.10f;
                    skull.isDead = true;
                }

                if(skull.Rectangle.Intersects(skeleton.Rectangle) && skeleton.punchy == false)
                {
                    skeleton.velocity = skull.velocity;
                }
            }

            for (int i = 0; i < listSkeletons.Count; i++)
            {
                if (listSkeletons[i].isDead == true)
                {
                    if (remainningDelayDead <= 0)
                    {
                        listSkeletons[i].isOut = true;
                    }

                }
            }
            skeletonDie.Update(gameTime);

            skeletonWalkLeft.Update(gameTime);

            if (skeleton.position.X >= 490)
            {
                state = GameState.end_state;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            switch (state)
            {
                case GameState.pre_state:
                    Pre_Draw(gameTime);
                    break;
                case GameState.game_state:
                    Game_Draw(gameTime);
                    break;
                case GameState.end_state:
                    End_Draw(gameTime);
                    break;
            }


            base.Draw(gameTime);
        }

        private void Pre_Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.DrawString(font, "un Joueur", new Vector2(100, 100), Color.White);
            spriteBatch.DrawString(font, "deux Joueur", new Vector2(100, 150), Color.White);
            spriteBatch.End();
        }

        private void End_Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Finish !", new Vector2(100, 100), Color.White);
            spriteBatch.End();
        }

        private void Game_Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            if (skeleton.velocity == new Vector2(1, 0))
            {
                skeletonWalk.Draw(spriteBatch, skeleton.position);
            }
            else if (skeleton.velocity == new Vector2(-1, 0))
            {
                skeletonWalkLeft.Draw(spriteBatch, skeleton.position);
            }
            else if (skeleton.velocity == new Vector2(0, 0))
            {
                if (skeleton.punchy == true)
                {
                    skeletonPunch.Draw(spriteBatch, skeleton.position);
                }
                else
                {
                    skeletonStand.Draw(spriteBatch, skeleton.position);
                }
            }

            spriteBatch.Draw(endPanel, new Vector2(500, 300), Color.White);

           foreach (var skull in listSkeletons)
            {
                if (skull.isDead == true && skull.isOut == false)
                {
                    skeletonDie.Draw(spriteBatch, skull.position);
                }
                else if (skull.isDead == true && skull.isOut == true)
                {
                    spriteBatch.Draw(deadBody, skull.position, Color.White);
                }
                else
                {
                    skeletonWalkLeft.Draw(spriteBatch, skull.position);
                }     
            }

            spriteBatch.End();
        }
    }
}
