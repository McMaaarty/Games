using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceTrouble.Sprites;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SpaceTrouble
{
    enum GameState
    {
        Start,
        Gameplay,
        Newplay,
        Score,
        EndOfGame,
    }

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Player player;
        private List<Bullet> listBulletInvader;
        private List<Bullet> listBulletPlayer;
        private List<Invaders> listInvadersLigne1;
        private List<Invaders> listInvadersLigne2;
        private List<World> listEntities;
        private List<int> listScore;

        private SpriteFont text;
        private int score = 0;
        private int lvl = 0;
        private const float SPEEDINVADER = 1f;
        private float speedInvader = SPEEDINVADER;
        private const float SHOOTSPEED = 1f;
        private float shootSpeed = SHOOTSPEED;
        private const int SHOOTFREQ = 500;
        private int shootFreq = SHOOTFREQ;
        private const float delay = 3;
        private float remainningDelay = delay;
        private float timer = 0;

        private Random rdm = new Random();

        public static int screenWidht = 700;
        public static int screenHeight = 1000;
        private GameState state = GameState.Start;

        private KeyboardState previousState;
        private bool tour;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = screenWidht;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.ApplyChanges();

            previousState = Keyboard.GetState();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            text = Content.Load<SpriteFont>("txt");

            Texture2D invaders = Content.Load<Texture2D>("Invaders");
            Texture2D fighter = Content.Load<Texture2D>("fighter");
            Texture2D terre = Content.Load<Texture2D>("bullet");

            player = new Player(fighter, new Vector2(50, screenHeight - fighter.Height), 1f, new Vector2(0, 0))
            {
                Bullet = new Bullet(Content.Load<Texture2D>("bullet"), new Vector2(0, 0)),
            };

            listBulletInvader = new List<Bullet>();
            listBulletPlayer = new List<Bullet>();

            listEntities = new List<World>();

            for (int k=1; k < 5; k++)
            {
                for (int i = 0; i < 15; i++)
                {
                    for (int j = 0; j < 15; j++)
                    {
                        listEntities.Add(new World(terre, new Vector2(screenWidht - screenWidht / 4 * k + terre.Width * j + 50, screenHeight - player.texture.Height * 3 + terre.Height * i), true));
                    }
                }
            }
            listInvadersLigne1 = new List<Invaders>();
            listInvadersLigne2 = new List<Invaders>();
            listScore = new List<int>();

                for (int i = 0; i < 7; i++)
                {
                listInvadersLigne1.Add(
                        new Invaders(invaders, new Vector2(70 * i + 50, 0), speedInvader, new Vector2(0, 0), shootSpeed)
                        {
                            Bullet = new Bullet(Content.Load<Texture2D>("bullet"), new Vector2(0, 0)),
                        }
                    );
                }
                for (int i = 0; i < 7; i++)
                {
                listInvadersLigne2.Add(
                        new Invaders(invaders, new Vector2(70 * i + 50, 50), speedInvader, new Vector2(0, 0), shootSpeed)
                        {
                            Bullet = new Bullet(Content.Load<Texture2D>("bullet"), new Vector2(0, 0)),
                        }
                    );
                }
        }

        protected override void UnloadContent()
        {
            
        }

        protected override void Update(GameTime gameTime)
        {

            switch (state)
            {
                case GameState.Start:
                    StartUpdate(gameTime);
                    break;
                case GameState.Gameplay:
                    GameUpdate(gameTime);
                    break;
                case GameState.Newplay:
                    NewGameUpdate(gameTime);
                    break;
                case GameState.Score:
                    ScoreUpdate(gameTime);
                    break;
                case GameState.EndOfGame:
                    EndUpdate(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        private void StartUpdate(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                state = GameState.Newplay;

            if (Keyboard.GetState().IsKeyDown(Keys.Z))
                state = GameState.Score;

        }

        private void GameUpdate(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState newState = Keyboard.GetState();

            if (previousState.IsKeyUp(Keys.D) && newState.IsKeyDown(Keys.D) && player.position.X < screenWidht - player.texture.Width)
            {
                player.position = new Vector2(player.position.X + 5, player.position.Y);
            }
            if (previousState.IsKeyUp(Keys.Q) && newState.IsKeyDown(Keys.Q) && player.position.X > 0)
            {
                player.position = new Vector2(player.position.X - 5, player.position.Y);
            }
            if (previousState.IsKeyUp(Keys.Space) && newState.IsKeyDown(Keys.Space))
            {
                player.AddBullet(listBulletPlayer);
            }

            foreach (var invader in listInvadersLigne1)
            {
                invader.Update(gameTime);

                invader.go = rdm.Next(0, shootFreq);

                if (invader.go == 50)
                {
                    invader.AddBullet(listBulletInvader);
                }

                if (invader.position.Y >= screenHeight - invader.texture.Height)
                    state = GameState.EndOfGame;
            }
            foreach (var invader in listInvadersLigne2)
            {
                invader.Update(gameTime);

                invader.go = rdm.Next(0, shootFreq);

                if (invader.go == 50)
                {
                    invader.AddBullet(listBulletInvader);
                }

                if (invader.position.Y >= screenHeight - invader.texture.Height)
                    state = GameState.EndOfGame;
            }
            if (listInvadersLigne1.Count > 0)
            {
                if (listInvadersLigne1[0].position.X < 0)
                {
                    foreach (var invader in listInvadersLigne1)
                    {
                        invader.goLeft = true;
                    }
                }
                int max = listInvadersLigne1.Count;
                if (listInvadersLigne1[max - 1].position.X > Game1.screenWidht - listInvadersLigne1[0].texture.Width)
                {
                    foreach (var invader in listInvadersLigne1)
                    {
                        invader.goLeft = false;
                    }
                }
            }

            if (listInvadersLigne2.Count > 0)
            {
                if (listInvadersLigne2[0].position.X < 0)
                {
                    foreach (var invader in listInvadersLigne2)
                    {
                        invader.goLeft = true;
                    }
                }
                int max2 = listInvadersLigne2.Count;
                if (listInvadersLigne2[max2 - 1].position.X > Game1.screenWidht - listInvadersLigne2[0].texture.Width)
                {
                    foreach (var invader in listInvadersLigne2)
                    {
                        invader.goLeft = false;
                    }
                }
            }
            

            foreach (var shoot in listBulletPlayer)
            {
                shoot.Update(gameTime);

                foreach (var invader in listInvadersLigne1)
                {
                    if (invader.Rectangle.Contains(shoot.Rectangle))
                    {
                        score += 300;
                        invader.IsRemoved = true;
                        shoot.IsRemoved = true;
                    }            
                }
                foreach (var invader in listInvadersLigne2)
                {
                    if (invader.Rectangle.Contains(shoot.Rectangle))
                    {
                        score += 300;
                        invader.IsRemoved = true;
                        shoot.IsRemoved = true;
                    }
                }

                foreach (var terre in listEntities)
                {
                    if (shoot.Rectangle.Intersects(terre.Rectangle))
                    {
                        terre.IsRemoved = true;
                        shoot.IsRemoved = true;
                    }
                }
            }

            foreach (var bullet in listBulletInvader)
            {
                bullet.Update(gameTime);

                if (player.Rectangle.Contains(bullet.Rectangle))
                    state = GameState.EndOfGame;

                foreach (var terre in listEntities)
                {
                    if (bullet.Rectangle.Intersects(terre.Rectangle))
                    {
                        terre.IsRemoved = true;
                        bullet.IsRemoved = true;
                    }
                }
            }

            for (int i = 0; i < listBulletPlayer.Count; i++)
            {
                if (listBulletPlayer[i].IsRemoved)
                {
                    listBulletPlayer.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 1; i < listBulletPlayer.Count; i++)
            {
                    listBulletPlayer.RemoveAt(i);
                    i--;
            }

            for (int i = 0; i < listBulletInvader.Count; i++)
            {
                if (listBulletInvader[i].IsRemoved)
                {
                    listBulletInvader.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < listEntities.Count; i++)
            {
                if (listEntities[i].IsRemoved)
                {
                    listEntities.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < listInvadersLigne1.Count; i++)
            {
                if (listInvadersLigne1[i].IsRemoved)
                {
                    listInvadersLigne1.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < listInvadersLigne2.Count; i++)
            {
                if (listInvadersLigne2[i].IsRemoved)
                {
                    listInvadersLigne2.RemoveAt(i);
                    i--;
                }
            }

            if (listInvadersLigne1.Count == 0 && listInvadersLigne2.Count == 0)
            {
                state = GameState.Newplay;
            }

        }

        private void NewGameUpdate(GameTime gameTime)
        {

            timer = (float)gameTime.ElapsedGameTime.TotalSeconds;

            remainningDelay -= timer;

            if (remainningDelay <= 0)
            {
                UnloadContent();
                lvl += 1;

                if (lvl > 0 && lvl <= 5)
                {
                    shootSpeed += 2f;
                }
                else if (lvl < 5 && lvl <= 10)
                {
                    shootFreq -= 50;
                }
                else if (lvl < 10 && lvl <= 15)
                {
                    speedInvader += 1f;
                }
                else if (lvl < 15 && lvl <= 17)
                {
                    shootSpeed += 2f;
                    shootFreq -= 50;
                    speedInvader += 1f;
                }else if (lvl == 18)
                {
                    state = GameState.EndOfGame;
                }
                LoadContent();
                remainningDelay = delay;
                state = GameState.Gameplay;
            }
        }

        private void ScoreUpdate (GameTime gameTime)
        {
            KeyboardState newState = Keyboard.GetState();
            if (tour == false)
            {
                string line;
                int number;
                System.IO.StreamReader file = new System.IO.StreamReader(@"C:\Users\coflo\source\repos\SpaceTrouble\SpaceTrouble\score.txt");
                while ((line = file.ReadLine()) != null)
                {
                    try
                    {
                        number = Convert.ToInt32(line.Trim());
                        listScore.Add(number);
                    }
                    catch
                    {

                    }


                }
                file.Close();

                listScore.Sort();
                listScore.Reverse();
                tour = true;
            }
            
            if (previousState.IsKeyUp(Keys.Enter) && newState.IsKeyDown(Keys.Enter))
            {
                state = GameState.Start;
            }
        }

        private void EndUpdate(GameTime gameTime)
        {
            KeyboardState newState = Keyboard.GetState();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                UnloadContent();
                string text = "" + score;
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\coflo\source\repos\SpaceTrouble\SpaceTrouble\score.txt", true))
                {
                    file.WriteLine("\n" + text);
                }
                score = 0;
                lvl = 0;
                speedInvader = SPEEDINVADER;
                shootSpeed = SHOOTSPEED;
                shootFreq = SHOOTFREQ;
                LoadContent();
                state = GameState.Gameplay;
            }
               
        }

        protected override void Draw(GameTime gameTime)
        {

            switch (state)
            {
                case GameState.Start:
                    StartDraw(gameTime);
                    break;
                case GameState.Gameplay:
                    GameDraw(gameTime);
                    break;
                case GameState.Newplay:
                    NewGameDraw(gameTime);
                    break;
                case GameState.Score:
                    ScoreDraw(gameTime);
                    break;
                case GameState.EndOfGame:
                    EndDraw(gameTime);
                    break;
            }

            base.Draw(gameTime);

        }

        private void StartDraw (GameTime gametime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            string txt = "Jouer";
            spriteBatch.DrawString(text, txt, new Vector2(screenWidht / 2 - txt.Length / 2, screenHeight / 2 - 20), Color.Black);
            txt = "Score";
            spriteBatch.DrawString(text, txt, new Vector2(screenWidht / 2 - txt.Length / 2, screenHeight / 2 + 20), Color.Black);
            txt = "Enter pour valider";
            spriteBatch.DrawString(text, txt, new Vector2(screenWidht / 2 - txt.Length / 2, screenHeight - 50), Color.Black);

            spriteBatch.End();

        }

        private void GameDraw(GameTime gametime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(player.texture, player.position, Color.White);

            foreach (var invader in listInvadersLigne1)
                spriteBatch.Draw(invader.texture, invader.position, Color.White);

            foreach (var invader in listInvadersLigne2)
                spriteBatch.Draw(invader.texture, invader.position, Color.White);

            foreach (var bullet in listBulletInvader)
                spriteBatch.Draw(bullet.texture, bullet.position, Color.White);

            foreach (var bullet in listBulletPlayer)
                spriteBatch.Draw(bullet.texture, bullet.position, Color.White);

            string txt = score.ToString();
            spriteBatch.DrawString(text, txt, new Vector2(10, 10), Color.Black);

            foreach (var terre in listEntities)
                spriteBatch.Draw(terre.texture, terre.position, Color.White);

            spriteBatch.End();

        }

        private void NewGameDraw(GameTime gametime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            string txt = "Level " + lvl.ToString();
            spriteBatch.DrawString(text, txt, new Vector2(screenWidht / 2 - txt.Length / 2, screenHeight / 2), Color.Black);

            spriteBatch.End();
        }

        private void ScoreDraw(GameTime gametime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            for (int i = 0; i < listScore.Count; i++)
            {
                if (i < 10)
                spriteBatch.DrawString(text, " " + listScore[i], new Vector2(screenWidht / 2, screenHeight / 2 + 20 * i), Color.Black);
            }

            spriteBatch.End();

        }

        private void EndDraw(GameTime gametime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            string txt = "GAME OVER \nscore :" + score.ToString();
            if (lvl == 18)
            {
                txt = txt + "\nYOU WIN !";
            }
            spriteBatch.DrawString(text, txt, new Vector2(screenWidht / 2 - txt.Length / 2, screenHeight / 2), Color.Black);


            spriteBatch.End();
        }
    }
}
