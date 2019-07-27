using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
namespace Flappy_bird
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        const int size = 60;
        
        //Vector2 rotation = Vector2.Zero;
        int windowWidth = 800;
        int windowHeight = 600;

        Vector2 playerPosition;
        float rotation = 0;

        float gravity = 0;

        bool bspace;

        Texture2D rabbit;
        Texture2D spike;
        Texture2D backArrow, restartArrow;
        Texture2D dot;

        List<Rectangle> obstacle = new List<Rectangle>();
        int speed = 5;
        int obstacleSize = 50;

        SpriteFont bigFont;
        SpriteFont normalFont;
        int score = 0;

        bool pause = true;
        int scene = 1;

        int optionSelect = 0;

        Rectangle rectBack, rectRestart, rectMouse, rectPlayer;
        Random rand = new Random();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.graphics.PreferredBackBufferWidth = windowWidth;
            this.graphics.PreferredBackBufferHeight = windowHeight;
            this.graphics.ApplyChanges();
            this.IsMouseVisible = true;
            playerPosition = new Vector2(80, windowHeight / 2);
            rectBack = new Rectangle(20, (windowHeight / 8) * 7, 50, 50);
            rectRestart = new Rectangle(windowWidth - 70, (windowHeight / 8) * 7, 50, 50);
            rectMouse = new Rectangle(0, 0, 1, 1);
            rectPlayer = new Rectangle((int)playerPosition.X, (int)playerPosition.Y, size, size);
            int random = rand.Next(windowHeight - 300);
            int random1 = rand.Next(200, 300);
            obstacle.Add(new Rectangle(windowWidth, 0, obstacleSize, random));
            obstacle.Add(new Rectangle(windowWidth, random + random1, obstacleSize, windowHeight - (random + random1)));
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            rabbit = Content.Load<Texture2D>("Rabbit");
            spike = Content.Load<Texture2D>("spike");
            bigFont = Content.Load<SpriteFont>("bigFont");
            normalFont = Content.Load<SpriteFont>("normalfont");
            backArrow = Content.Load<Texture2D>("back");
            restartArrow = Content.Load<Texture2D>("restart");
            dot = new Texture2D(GraphicsDevice, 1, 1);
            dot.SetData<Color>(new Color[] { Color.White });
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
            rectMouse.X = mouse.X;
            rectMouse.Y = mouse.Y;

            switch (scene)
            {
                #region mainmenu
                case 1:
                    if ((mouse.Y > windowHeight / 6) && (mouse.Y < (windowHeight / 6) * 2))
                    {
                        optionSelect = 1;
                        if (mouse.LeftButton == ButtonState.Pressed)
                        { 
                            scene = 2;
                            playerPosition = new Vector2(80, windowHeight / 2);
                            rectPlayer = new Rectangle((int)playerPosition.X, (int)playerPosition.Y, size, size);
                            score = 0;
                            pause = true;
                            rotation = 0;
                            gravity = 0;
                            obstacle.Clear();
                            int random = rand.Next(windowHeight - 300);
                            int random1 = rand.Next(200, 300);
                            obstacle.Add(new Rectangle(windowWidth, 0, obstacleSize, random));
                            obstacle.Add(new Rectangle(windowWidth, random + random1, obstacleSize, windowHeight - (random + random1)));
                        }
                    }
                    else if ((mouse.Y > (windowHeight / 6) * 2) && (mouse.Y < (windowHeight / 6) * 3))
                    {
                        optionSelect = 2;
                        if (mouse.LeftButton == ButtonState.Pressed)
                            scene = 10;
                    }
                    else if ((mouse.Y > (windowHeight / 6) * 3) && (mouse.Y < (windowHeight / 6) * 4))
                    {
                        optionSelect = 3;
                        if (mouse.LeftButton == ButtonState.Pressed)
                            scene = 20;
                    }
                    else if ((mouse.Y > (windowHeight / 6) * 4) && (mouse.Y < (windowHeight / 6) * 5))
                    {
                        optionSelect = 4;
                        if (mouse.LeftButton == ButtonState.Pressed)
                            Exit();
                    }
                    else
                    {
                        optionSelect = 0;
                    }
                    break;
                #endregion

                #region game
                case 2:
                    optionSelect = 0;
                    if (scene == 2)
                    {
                        if (pause == false)
                        {
                            if (keyboard.IsKeyDown(Keys.Space) && !bspace)
                            {

                                gravity -= 30;
                                if (gravity < -15)
                                    gravity = -15;
                                bspace = true;
                            }

                            bspace = keyboard.IsKeyDown(Keys.Space);

                            rotation = MathHelper.ToRadians(gravity * 2);

                            playerPosition.Y += gravity;

                            if (playerPosition.Y < 0)
                            {
                                playerPosition.Y = 1;
                                gravity += 1;
                            }

                            if (gravity < 20)
                                gravity += 0.8f;

                            if ((playerPosition.Y) > windowHeight)
                            {
                                scene = 3;
                            }
                            rectPlayer = new Rectangle((int)playerPosition.X, (int)playerPosition.Y, size, size);

                            bool b = true;
                            int generate = 0;
                            for (int i = obstacle.Count - 1; i >= 0; i--)
                            {
                                Rectangle r = obstacle[i];                               
                                if (rectPlayer.Intersects(r))
                                {
                                    scene = 3;
                                }

                                r.X -= speed;
                                obstacle.RemoveAt(i);
                                if (r.X > -obstacleSize)
                                    obstacle.Add(r);
                                if (r.X == playerPosition.X && b)
                                { 
                                    score++;
                                    b = false;
                                }
                                if (generate < r.X)
                                    generate = r.X;                                           
                            }

                            if (generate <= windowWidth / 2)
                            {
                                int random = rand.Next(windowHeight - 300);
                                int random1 = rand.Next(200, 300);
                                obstacle.Add(new Rectangle(windowWidth, 0, obstacleSize, random));
                                obstacle.Add(new Rectangle(windowWidth, random + random1, obstacleSize, windowHeight - (random + random1)));
                            }

                        }
                        else
                        {
                            if (keyboard.IsKeyDown(Keys.Space))
                                pause = false;
                        }

                    }
                    break;
                #endregion

                #region gameover
                case 3:
                    optionSelect = 0;
                    if (rectBack.Intersects(rectMouse))
                    {
                        optionSelect = 1;
                        if (mouse.LeftButton == ButtonState.Pressed)
                            scene = 1;
                    }
                    else if (rectRestart.Intersects(rectMouse))
                    {
                        optionSelect = 2;
                        if (mouse.LeftButton == ButtonState.Pressed)
                        {
                            scene = 2;
                            playerPosition = new Vector2(80, windowHeight / 2);
                            rectPlayer = new Rectangle((int)playerPosition.X, (int)playerPosition.Y, size, size);
                            score = 0;
                            pause = true;
                            rotation = 0;
                            gravity = 0;
                            obstacle.Clear();
                            int random = rand.Next(windowHeight - 300);
                            int random1 = rand.Next(200, 300);
                            obstacle.Add(new Rectangle(windowWidth, 0, obstacleSize, random));
                            obstacle.Add(new Rectangle(windowWidth, random + random1, obstacleSize, windowHeight - (random + random1)));
                        }
                    }
                    break;
                #endregion

                #region scoreboard
                case 10:
                    if (rectBack.Intersects(rectMouse))
                    {
                        optionSelect = 1;
                        if (mouse.LeftButton == ButtonState.Pressed)
                            scene = 1;
                    }
                    break;
                #endregion

                #region help
                case 20:
                    if (rectBack.Intersects(rectMouse))
                    {
                        optionSelect = 1;
                        if (mouse.LeftButton == ButtonState.Pressed)
                            scene = 1;
                    }
                    break;
               #endregion

            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);



            spriteBatch.Begin();
            switch (scene)
            {
                #region mainmenu
                case 1:
                    spriteBatch.DrawString(bigFont, "New Game", new Vector2(windowWidth / 30, windowHeight / 6), Color.Black);
                    spriteBatch.DrawString(bigFont, "Scoreboard", new Vector2(windowWidth / 30, (windowHeight / 6) * 2), Color.Black);
                    spriteBatch.DrawString(bigFont, "Help and about", new Vector2(windowWidth / 30, (windowHeight / 6) * 3), Color.Black);
                    spriteBatch.DrawString(bigFont, "Exit", new Vector2(windowWidth / 30, (windowHeight / 6) * 4), Color.Black);
                    switch (optionSelect)
                    {
                        case 1:
                            spriteBatch.DrawString(bigFont, "New Game", new Vector2(windowWidth / 30, windowHeight / 6), Color.Green);
                            break;
                        case 2:
                            spriteBatch.DrawString(bigFont, "Scoreboard", new Vector2(windowWidth / 30, (windowHeight / 6) * 2), Color.Green);
                            break;
                        case 3:
                            spriteBatch.DrawString(bigFont, "Help and about", new Vector2(windowWidth / 30, (windowHeight / 6) * 3), Color.Green);
                            break;
                        case 4:
                            spriteBatch.DrawString(bigFont, "Exit", new Vector2(windowWidth / 30, (windowHeight / 6) * 4), Color.Green);
                            break;
                    }
                    
                    break;
                #endregion

                #region game
                case 2:
                    spriteBatch.Draw(rabbit, new Rectangle(rectPlayer.X + size, rectPlayer.Y + size, size, size), null, Color.White, rotation, new Vector2(rabbit.Width, rabbit.Height), SpriteEffects.None, 0);
                    if(pause == true)
                        spriteBatch.DrawString(bigFont, "PRESS SPACE", new Vector2(windowWidth / 4, windowHeight / 3), Color.Black);
                   
                    foreach (Rectangle recSpike in obstacle)
                    {
                        spriteBatch.Draw(spike, recSpike, Color.White);
                    }
                    spriteBatch.DrawString(normalFont, "Score: " + score, new Vector2(windowWidth / 2, 10), Color.Black);

                    break;
                #endregion

                #region gameover
                case 3:
                    spriteBatch.DrawString(bigFont, "GAME OVER", new Vector2((windowWidth / 2) - 200, windowHeight / 4), Color.Black);
                    spriteBatch.DrawString(bigFont, "Score: " + score, new Vector2((windowWidth / 2) - 200, (windowHeight / 4) * 2), Color.Blue);
                    spriteBatch.Draw(backArrow, rectBack, Color.White);
                    spriteBatch.Draw(restartArrow, rectRestart, Color.White);
                    if(optionSelect == 1)
                        spriteBatch.Draw(backArrow, rectBack, Color.Green);
                    else if (optionSelect == 2)
                        spriteBatch.Draw(restartArrow, rectRestart, Color.Green);
                    break;
                #endregion

                #region scoreboard
                case 10:
                    spriteBatch.Draw(backArrow, rectBack, Color.White);
                    if (optionSelect == 1)
                        spriteBatch.Draw(backArrow, rectBack, Color.Green);
                    break;
                #endregion

                #region help
                case 20:
                    spriteBatch.Draw(backArrow, rectBack, Color.White);
                    if (optionSelect == 1)
                        spriteBatch.Draw(backArrow, rectBack, Color.Green);
                    break;
                #endregion

            }
            spriteBatch.End();



            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
