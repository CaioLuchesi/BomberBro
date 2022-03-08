using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;

namespace BomberBro
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class BattleGround : Microsoft.Xna.Framework.DrawableGameComponent
    {
       // public const int DEFAULT_GRID = 32;

        /*MouseState mcCurrentMouseState;         // Holds the Mouse's Current State
       MouseState mcPreviousMouseState;        // Holds the Mouse's Previous State
       Matrix msWorldMatrix = Matrix.Identity;
       Matrix msViewMatrix = Matrix.Identity;
       Matrix msProjectionMatrix = Matrix.Identity;
       AnimatedSpriteParticleSystem mcAnimatedSpriteParticleSystem = null;*/ 

       protected Game game;
       protected SpriteBatch spriteBatch;
       private BattleGroundLibrary battlegroundLibrary;

        AnimatedSpriteParticleSystem mcAnimatedSpriteParticleSystem = null;
       MouseState mcCurrentMouseState;
       MouseState mcPreviousMouseState; 

       protected Random random = new Random();

       /* protected Thread bugExterminator = new Thread(new ThreadStart(CheckForBugs));

       protected static void CheckForBugs()
       {
           for (; ; )
           {
               foreach (Drawable other in Drawable.drawableBackUp)
               {
                   if (other.GetType() == typeof(Player))
                   {
                       if (float.IsNaN(other.position.X) || float.IsNaN(other.position.Y) || float.IsNaN(other.lastPosition.X) || float.IsNaN(other.lastPosition.Y))
                       {
                           Console.WriteLine("WARNING: BUG DETECTED");
                           other.position = other.lastPosition;
                       }
                   }
               }
           }
       } */

       private Player player1 = null;
       private Player player2 = null;
       protected SpriteFont timeFont, timeSmallFont;
       protected int min = 3, seg = 0, milliseconds = 60;
       protected String time = "";
       protected int lin, col;
       private Timer timer;
       private List<Wall> wallList = new List<Wall>();// 152 blocos
       private List<BreakableWall> breakablewallList = new List<BreakableWall>(); //152 blocos
       protected int[,] spriteMatrix = new int[13, 19]
                                      {{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                                       {1,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,1},
                                       {1,0,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,0,1},
                                       {1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1},
                                       {1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1},
                                       {1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1},
                                       {1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1},
                                       {1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1},
                                       {1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1},
                                       {1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1},
                                       {1,0,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,0,1},
                                       {1,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,1},
                                       {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}};
       

        

        

        public BattleGround(Game game,byte tipo, PowerUpManager powerUpManager)
            : base(game)
        {
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            Drawable.spriteBatch = spriteBatch;
            this.game = game;

            battlegroundLibrary = new BattleGroundLibrary();
            battlegroundLibrary.LoadContent(Game.Content, tipo);

            /* PowerUp powerup = new FullFire(this.Game, new Vector2(5 * DEFAULT_GRID, 4 * DEFAULT_GRID));
            PowerUp powerup2 = new LineBomb(this.Game, new Vector2(4 * DEFAULT_GRID, 5 * DEFAULT_GRID));
            PowerUp powerup3 = new BombUp(this.Game, new Vector2(4 * DEFAULT_GRID, 6 * DEFAULT_GRID));
            PowerUp powerup4 = new BombUp(this.Game, new Vector2(4 * DEFAULT_GRID, 7 * DEFAULT_GRID));
            PowerUp powerup5 = new BombUp(this.Game, new Vector2(4 * DEFAULT_GRID, 8 * DEFAULT_GRID));
            PowerUp powerup6 = new PowerGlove(this.Game, new Vector2(4 * DEFAULT_GRID, 9 * DEFAULT_GRID));
            PowerUp powerup7 = new PassThroughBombPowerUp(this.Game, new Vector2(4 * DEFAULT_GRID, 10 * DEFAULT_GRID));
            PowerUp powerup8 = new BombKick(this.Game, new Vector2(4 * DEFAULT_GRID, 11 * DEFAULT_GRID));

            PowerUp powerup9 = new LineBomb(this.Game, new Vector2(13 * DEFAULT_GRID, 14 * DEFAULT_GRID));
            PowerUp powerup10 = new BombUp(this.Game, new Vector2(14 * DEFAULT_GRID, 14 * DEFAULT_GRID));
            PowerUp powerup11 = new BombUp(this.Game, new Vector2(15 * DEFAULT_GRID, 14 * DEFAULT_GRID));
            PowerUp powerup12 = new BombUp(this.Game, new Vector2(16 * DEFAULT_GRID, 14 * DEFAULT_GRID));
            PowerUp powerup13 = new BombUp(this.Game, new Vector2(17 * DEFAULT_GRID, 14 * DEFAULT_GRID));
            PowerUp powerup14 = new PowerGlove(this.Game, new Vector2(18 * DEFAULT_GRID, 14 * DEFAULT_GRID));
            PowerUp powerup15 = new BombKick(this.Game, new Vector2(19 * DEFAULT_GRID, 14 * DEFAULT_GRID)); */
 
            timer = new Timer(game, new Point(18, 27), new Vector2(3, 25), new Vector2(Game.Window.ClientBounds.Width / 2 - 35, 40));


            player1 = new Player(
            this.Game, Game.Content.Load<Texture2D>(@"Images/BombermanBishop"),
            new Vector2(4 * Drawable.DEFAULT_GRID, 4 * Drawable.DEFAULT_GRID), new Point(32, 40),
            5, new Point(0, 0),
            new Point(8, 8), 1.5f, new KeyboardInput(Keys.W, Keys.S, Keys.A, Keys.D, Keys.LeftShift, Keys.LeftControl, Keys.LeftAlt, Keys.Space));
            
            player2 = new Player(
            this.Game, Game.Content.Load<Texture2D>(@"Images/BombermanWhite"),
            new Vector2(800f - 160f, 600f - 147f), new Point(32, 40),
            5, new Point(0, 0),
            new Point(8, 8), 1.5f, new KeyboardInput(Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.NumPad1, Keys.NumPad3, Keys.NumPad2, Keys.NumPad6));

            new Player(
            this.Game, Game.Content.Load<Texture2D>(@"Images/BombermanNinja"),
            new Vector2(4 * Drawable.DEFAULT_GRID, 14 * Drawable.DEFAULT_GRID), new Point(32, 40),
            5, new Point(0, 0),
            new Point(8, 8), 1.5f, new KeyboardInput(Keys.I, Keys.K, Keys.J, Keys.L, Keys.RightShift, Keys.RightControl, Keys.Enter, Keys.Back));
            // FootBomb footBomb = new FootBomb(this.Game, null, 999, new Vector2(5 * DEFAULT_GRID, 14 * DEFAULT_GRID));

            for (lin = 0; lin <= 18; lin++)
                for (col = 0; col <= 12; col++)
                {
                    if (spriteMatrix[col, lin] == 1)
                        wallList.Add(new Wall(this.Game, battlegroundLibrary.Wall,
                        new Vector2((lin * 32) + 3 * Drawable.DEFAULT_GRID /*((Game.Window.ClientBounds.Width - (19 * 32)) / 2) */, (col * 32) + 3 * Drawable.DEFAULT_GRID /*((Game.Window.ClientBounds.Height - (13 * 32)) / 2)*/), new Point(32, 47),
                        2, new Point(0, 0), // x = 96; y = 92 128 124
                        new Point(8, 8), 0));
                }
            List<Point> possibleBreakableWallPositions = new List<Point>();
            for (int y = 0; y < 13; y++)
            {
                for (int x = 0; x < 19; x++)
                {
                    if (spriteMatrix[y, x] == 2) // Why, C#? Why?
                    {
                        possibleBreakableWallPositions.Add(new Point(x, y));
                    }
 
                    /* if (spriteMatrix[y, x] == 2)
                    {
                        if (random.Next(3) < 2)
                        {
                            new BreakableWall(this.Game, battlegroundLibrary.BreakableWall, new Vector2((x + 3) * DEFAULT_GRID, (y + 3) * DEFAULT_GRID), new Point(32, 47), 2, new Point(0, 0), new Point(8, 8), 0);
                        }
                    } */
                }
            }

            int numberOfBreakableWalls = 110;
            Console.WriteLine(powerUpManager.items.Count);
            if (powerUpManager.items.Count > numberOfBreakableWalls)
            {
                numberOfBreakableWalls = powerUpManager.items.Count;
            }

            if (numberOfBreakableWalls <= possibleBreakableWallPositions.Count)
            {
                PowerUp.Kinds selectedPowerUp;
                for (int i = 0; i < numberOfBreakableWalls; i++) // max = 135
                {
                    int index = random.Next(possibleBreakableWallPositions.Count);
                    Point point = possibleBreakableWallPositions[index];
                    possibleBreakableWallPositions.Remove(point);
                    if (i < powerUpManager.items.Count)
                    {
                        selectedPowerUp = powerUpManager.items[i];
                        new BreakableWall(selectedPowerUp, this.Game, battlegroundLibrary.BreakableWall, new Vector2((point.X + 3) * Drawable.DEFAULT_GRID, (point.Y + 3) * Drawable.DEFAULT_GRID), new Point(32, 47), 2, new Point(0, 0), new Point(8, 8), 0);
                    }
                    else
                    {
                        new BreakableWall(this.Game, battlegroundLibrary.BreakableWall, new Vector2((point.X + 3) * Drawable.DEFAULT_GRID, (point.Y + 3) * Drawable.DEFAULT_GRID), new Point(32, 47), 2, new Point(0, 0), new Point(8, 8), 0);
                    }
                }
            }
            else
            {
                Console.WriteLine("FAIL");
            }

            timeFont = Game.Content.Load<SpriteFont>(@"Font/Time");
            timeSmallFont = Game.Content.Load<SpriteFont>(@"Font/TimeSmall");
            // bugExterminator.Start();
             try
            {
                mcAnimatedSpriteParticleSystem = new AnimatedSpriteParticleSystem(this.Game);
                mcAnimatedSpriteParticleSystem.AutoInitialize(this.Game.GraphicsDevice, this.Game.Content);
                mcAnimatedSpriteParticleSystem.MousePosition = new Vector3(200, 200, 0);
            }
            catch (InvalidOperationException)
            {
                // DoNothing();
            } 
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            base.Initialize();
        }
        protected override void LoadContent()
        {
            // TODO: Add your initialization code here
           
            base.LoadContent();
        }

       /// <summary>
       /// UnloadContent will be called once per game and is the place to unload
       /// all content.
       /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            // mcAnimatedSpriteParticleSystem.Destroy();
            spriteBatch.Dispose();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
             try
            {
                // Save the Mouse State and get its new State
                mcPreviousMouseState = mcCurrentMouseState;
                mcCurrentMouseState = Mouse.GetState();
                if (mcCurrentMouseState.X != mcPreviousMouseState.X ||
                        mcCurrentMouseState.Y != mcPreviousMouseState.Y)
                {
                    mcAnimatedSpriteParticleSystem.MousePosition = new Vector3(mcCurrentMouseState.X, mcCurrentMouseState.Y, 100);
                }

                mcAnimatedSpriteParticleSystem.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            catch (NullReferenceException)
            {
                // DoNothing();
            } 

            List<Drawable> seekAndDestroy = new List<Drawable>();
            
            Drawable.UpdateDrawableBackUp();

            try
            {
                foreach (Drawable d in Drawable.drawableBackUp)
                {
                    d.lastPosition.X = d.position.X;
                    d.lastPosition.Y = d.position.Y;
                    d.Update(gameTime);
                    if (d.gonnaDie)
                    {
                        seekAndDestroy.Add(d);
                        // d.Destroy();
                    }
                }
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Exception at BattleGround's Update()");
                // DoNothing();
            }

            foreach (Drawable d in seekAndDestroy)
            {
                d.Destroy();
            }

                // player1.Update(gameTime, Game.Window.ClientBounds);
                // player2.Update(gameTime, Game.Window.ClientBounds);

                // Update all sprites
               
                /*
                foreach (Wall w in wallList)
                {
                    w.Update(gameTime, Game.Window.ClientBounds);

                    // Check for collisions and exit game if there is one
                    if (w.collisionRect.Intersects(player1.collisionRect))
                    {

                        player1.setPosition(player1.getLastPosition());



                    }
                    if (w.collisionRect.Intersects(player2.collisionRect))
                    {
                        player2.setPosition(player2.getLastPosition());

                    }

                }

                foreach (BreakableWall b in breakablewallList)
                {
                    b.Update(gameTime, Game.Window.ClientBounds);

                    // Check for collisions and exit game if there is one
                    if (b.collisionRect.Intersects(player1.collisionRect))
                    {

                        player1.setPosition(player1.getLastPosition());



                    }
                    if (b.collisionRect.Intersects(player2.collisionRect))
                    {
                        player2.setPosition(player2.getLastPosition());

                    }

                }
                */
                
                base.Update(gameTime);
            
        }

        public override void Draw(GameTime gameTime)
        {
           // Vector2 nowVector = new Vector2(Game.Window.ClientBounds.Width/2 - 25, 10);

            
            //Draw the Time
           // spriteBatch.DrawString(timeSmallFont, "Time", new Vector2(Game.Window.ClientBounds.Width / 2 - 10, 1), Color.White);
           // spriteBatch.DrawString(timeFont, time, nowVector, Color.Black);

            //Draw the Background
            spriteBatch.Draw(battlegroundLibrary.Background, new Vector2(0, 0), Color.White);

            Drawable.UpdateDrawableBackUp();
            foreach (Drawable d in Drawable.drawableBackUp)
            {
                d.Draw(gameTime);
            }

             try
            {
                mcAnimatedSpriteParticleSystem.Draw();
            }
            catch (NullReferenceException)
            {
                // DoNothing();
            } 
            
            /*
            // Draw the player
            player1.Draw(gameTime, spriteBatch);
            player2.Draw(gameTime, spriteBatch);


            // Draw all Walls
            foreach (Wall w in wallList)
            w.Draw(gameTime, spriteBatch);

            //Draw all BreakableWalls
            foreach (BreakableWall b in breakablewallList)
            b.Draw(gameTime, spriteBatch); */

            base.Draw(gameTime);

            /* try
            {
                mcAnimatedSpriteParticleSystem.Draw();
            }
            catch (NullReferenceException)
            {
                // DoNothing();
            } */
       }
    }
}