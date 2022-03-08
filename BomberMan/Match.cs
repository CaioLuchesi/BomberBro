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
using Microsoft.Xna.Framework.Media;

namespace BomberBro
{
    public class Match : DrawableRoom
    {
        // public const int DEFAULT_GRID = 32;

        /* MouseState mcCurrentMouseState;         // Holds the Mouse's Current State
        MouseState mcPreviousMouseState;        // Holds the Mouse's Previous State
        Matrix msWorldMatrix = Matrix.Identity;
        Matrix msViewMatrix = Matrix.Identity;
        Matrix msProjectionMatrix = Matrix.Identity;
        AnimatedSpriteParticleSystem mcAnimatedSpriteParticleSystem = null; */

        protected ScreenMessage screenMessage = null;

        public BattleManager root;

        private Point[] spawnPoints = {new Point(4, 4), new Point(20, 4), new Point(4, 14), new Point(20, 14), new Point(8, 6), new Point(16, 6), new Point(8, 12), new Point(16, 12)};

        private Texture2D[] scoreNumbers = new Texture2D[10];
        private int[] wins;

        protected Queue<Vector2> fallingWallPositions = new Queue<Vector2>();
        protected bool suddenDeathRightNow = false;
        protected int suddenDeathCountdown;
        protected int suddenDeathStepsPerWall;

        public int arenaTopmostRow = 4;
        public int arenaRightmostColumn = 20;
        public int arenaBottommostRow = 14;
        public int arenaLeftmostColumn = 4;

        public Vector2?[] mugshotCoordinates = new Vector2?[8];
        public Vector2?[] scoreCoordinates = new Vector2?[8];

        public int arenaColumnCount
        { get { return 1 + arenaRightmostColumn - arenaLeftmostColumn; } }

        public int arenaRowCount
        { get { return 1 + arenaBottommostRow - arenaTopmostRow; } }

        int waterTileSubimageIndex = 0;
        protected int drawGameCountdown = 120;
        protected bool matchOver = false;
        // protected Game game;
        // protected SpriteBatch spriteBatch;
        private GraphicsLibrary graphicsLibrary;
        private AudioLibrary audio;
        /* AnimatedSpriteParticleSystem mcAnimatedSpriteParticleSystem = null;
        MouseState mcCurrentMouseState;
        MouseState mcPreviousMouseState; */

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

        public List<Player>[] teams = new List<Player>[8];
        public List<Revenger>[] revengers = new List<Revenger>[8];
        public int remainingTeamsCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < 8; i++)
                {
                    if (teams[i] != null)
                    {
                        foreach (Player player in teams[i])
                        {
                            if (! player.dead)
                            {
                                count++;
                                break;
                            }
                        }
                    }
                }
                return count;
            }
        }

        public int firstRemainingTeamIndex
        {
            get
            {
                for (int i = 0; i < 8; i++)
                {
                    if (teams[i] != null)
                    {
                        foreach (Player player in teams[i])
                        {
                            if (! player.dead)
                            {
                                return i;
                            }
                        }
                    }
                }
                return -1;
            }
        }
        protected Song soundtrack;
        protected SpriteFont timeFont, timeSmallFont;
        protected int min = 3, seg = 0, milliseconds = 60;
        protected String time = "";
        protected int lin, col;
        private Timer timer;
        private List<Wall> wallList = new List<Wall>(); // 152 blocos
        private List<BreakableWall> breakablewallList = new List<BreakableWall>(); //152 blocos
        protected int[,] spriteMatrix = new int[13, 19]
                                       {{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                                        {1,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,1},
                                        {1,0,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,0,1},
                                        {1,2,2,2,2,0,0,2,2,2,2,2,0,0,2,2,2,2,1},
                                        {1,2,1,2,1,0,1,2,1,2,1,2,1,0,1,2,1,2,1},
                                        {1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1},
                                        {1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1},
                                        {1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1},
                                        {1,2,1,2,1,0,1,2,1,2,1,2,1,0,1,2,1,2,1},
                                        {1,2,2,2,2,0,0,2,2,2,2,2,0,0,2,2,2,2,1},
                                        {1,0,1,2,1,2,1,2,1,2,1,2,1,2,1,2,1,0,1},
                                        {1,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,1},
                                        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}};




        private BattleManager.Arena arena;

        public Match(Game game,BattleManager root, BattleManager.Arena arena, PowerUpManager powerUpManager): base(game)
        {           
            // arena = 0;
            // spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            // Drawable.spriteBatch = spriteBatch;
            // this.game = game;
            
            this.root = root;
            this.arena = arena;
            this.wins = root.Wins;
            this.audio = (AudioLibrary)Game.Services.GetService(typeof(AudioLibrary));
            
            switch (arena)
            {
                case BattleManager.Arena.pirate:
                    soundtrack = audio.PirateSong;
                    globalOrigin = new Vector2(0, 48);
                    break;
                case BattleManager.Arena.excel:
                    soundtrack = audio.PirateSong;
                    globalOrigin = new Vector2(1, 35);
                    break;
                case BattleManager.Arena.desert:
                    soundtrack = audio.PirateSong;
                    break;
                case BattleManager.Arena.forest:
                    soundtrack = audio.ForestSong;
                    break;
                default:
                    globalOrigin = new Vector2(0, 0);
                    break;
            }

            graphicsLibrary = new GraphicsLibrary(game.Content, arena);
            graphicsLibrary.LoadContent();
 
            timer = new Timer(this,root.timeLimit,new Point(18, 18), new Vector2(3, 25), new Vector2(Game.Window.ClientBounds.Width / 2 - 35, 48));

            CreatePlayersAndDefineMugshotCoordinates();
            
            /* teams[0] = new List<Player>();
            teams[0].Add(new Player(
            this, Character.bishop,
            new Vector2(4 * Drawable.DEFAULT_GRID, 4 * Drawable.DEFAULT_GRID), new Point(32, 40),
            new Point(0, 0),
            new Point(8, 8), 1.5f, new KeyboardMapping(Keys.W, Keys.S, Keys.A, Keys.D, Keys.LeftShift, Keys.LeftControl, Keys.LeftAlt, Keys.Space)));

            teams[1] = new List<Player>();
            teams[1].Add(new Player(
            this, Character.whiteBomber,
            new Vector2(800f - 160f, 600f - 147f), new Point(32, 40),
            new Point(0, 0),
            new Point(8, 8), 1.5f, new KeyboardMapping(Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.NumPad1, Keys.NumPad3, Keys.NumPad2, Keys.NumPad6)));

            teams[2] = new List<Player>();
            teams[2].Add(new Player(
            this, Character.merchant,
            new Vector2(4 * Drawable.DEFAULT_GRID, 14 * Drawable.DEFAULT_GRID), new Point(32, 40),
            new Point(0, 0),
            new Point(8, 8), 1.5f, new KeyboardMapping(Keys.I, Keys.K, Keys.J, Keys.L, Keys.RightShift, Keys.RightControl, Keys.Enter, Keys.Back)));

            teams[3] = new List<Player>();
            teams[3].Add(new Player(
            this, Character.hero,
            new Vector2(800f - 160f, 4 * Drawable.DEFAULT_GRID), new Point(32, 40),
            new Point(0, 0),
            new Point(8, 8), 1.5f, new KeyboardMapping(Keys.NumPad8, Keys.NumPad5, Keys.NumPad4, Keys.NumPad6, Keys.NumPad7, Keys.NumPad9, Keys.Add, Keys.Back))); */

            for (lin = 0; lin <= 18; lin++)
            {
                for (col = 0; col <= 12; col++)
                {
                    if (spriteMatrix[col, lin] == 1)
                    {
                        new Wall(this, graphicsLibrary.Wall, new Vector2((lin * 32) + 3 * Drawable.DEFAULT_GRID, (col * 32) + 3 * Drawable.DEFAULT_GRID), new Point(32, 47), new Point(0, 0), new Point(8, 8));
                    }
                }
            }
            List<Point> possibleBreakableWallPositions = new List<Point>();
            for (int y = 0; y < 13; y++)
            {
                for (int x = 0; x < 19; x++)
                {
                    if (spriteMatrix[y, x] == 2)
                    {
                        possibleBreakableWallPositions.Add(new Point(x, y));
                    }
                }
            }

            int numberOfBreakableWalls = 90; // max 121
            Console.WriteLine(powerUpManager.items.Count);
            if (powerUpManager.items.Count > numberOfBreakableWalls)
            {
                numberOfBreakableWalls = powerUpManager.items.Count;
            }

            if (numberOfBreakableWalls <= possibleBreakableWallPositions.Count)
            {
                PowerUp.Kinds selectedPowerUp;
                for (int i = 0; i < numberOfBreakableWalls; i++)
                {
                    int index = random.Next(possibleBreakableWallPositions.Count);
                    Point point = possibleBreakableWallPositions[index];
                    possibleBreakableWallPositions.Remove(point);
                    if (i < powerUpManager.items.Count)
                    {
                        selectedPowerUp = powerUpManager.items[i];
                        new BreakableWall(this, selectedPowerUp, graphicsLibrary.BreakableWall, new Vector2((point.X + 3) * Drawable.DEFAULT_GRID, (point.Y + 3) * Drawable.DEFAULT_GRID), new Point(32, 47), new Point(0, 0), new Point(8, 8));
                    }
                    else
                    {
                        new BreakableWall(this, graphicsLibrary.BreakableWall, new Vector2((point.X + 3) * Drawable.DEFAULT_GRID, (point.Y + 3) * Drawable.DEFAULT_GRID), new Point(32, 47), new Point(0, 0), new Point(8, 8));
                    }
                }
            }
            else
            {
                Console.WriteLine("FAIL");
            }

            timeFont = game.Content.Load<SpriteFont>(@"Font/Time");
            timeSmallFont = game.Content.Load<SpriteFont>(@"Font/TimeSmall");

            screenMessage = new MarqueeMessage(this, GraphicsLibrary.fight, 120, Color.Red);

            MediaPlayer.Play(soundtrack);
            MediaPlayer.Volume = 0.2f;
            MediaPlayer.IsRepeating = true;
            // bugExterminator.Start();
            /* try
            {
                mcAnimatedSpriteParticleSystem = new AnimatedSpriteParticleSystem(this.Game);
                mcAnimatedSpriteParticleSystem.AutoInitialize(this.Game.GraphicsDevice, this.Game.Content);
                mcAnimatedSpriteParticleSystem.MousePosition = new Vector3(200, 200, 0);
            }
            catch (InvalidOperationException)
            {
                // DoNothing();
            } */
            // TODO: Construct any child components here
            //StartSuddenDeath(60 * 39);
        }

        private void CreatePlayersAndDefineMugshotCoordinates()
        {
            Vector2 coordinate = new Vector2(0, 20);
            byte id = 0;

            for (int i = 0; i <= 9; i++)
            {
                scoreNumbers[i] = game.Content.Load<Texture2D>(@"Images/Score/" + i);
            }

            for (byte i = 0; i < 8; i++)
            {
                if (root.teamData[i] != null && root.teamData[i].Count > 0)
                {
                    teams[i] = new List<Player>();
                    foreach (BattleManager.PlayerData data in root.teamData[i])
                    {
                        if (240 < coordinate.X && coordinate.X < 496)
                        {
                            coordinate.X = 496;
                        }
                        mugshotCoordinates[id] = new Vector2(coordinate.X, coordinate.Y);
                        coordinate.X += 48;
                        if (data.Level != BattleManager.PlayerData.CPULevel.none)
                        {
                            Artificial_Intelligence newPlayer = new Artificial_Intelligence(this, data.character,
                            new Vector2(spawnPoints[id].X, spawnPoints[id].Y) * Drawable.DEFAULT_GRID, new Point(32, 40),
                            new Point(0, 0),
                            new Point(8, 8), 1.5f, data.keyboardMapping);
                            teams[i].Add(newPlayer);

                            newPlayer.id = id;
                        }
                        else
                        {
                            Player newPlayer = new Player(this, data.character,
                            new Vector2(spawnPoints[id].X, spawnPoints[id].Y) * Drawable.DEFAULT_GRID, new Point(32, 40),
                            new Point(0, 0),
                            new Point(8, 8), 1.5f, data.keyboardMapping);
                            teams[i].Add(newPlayer);

                            newPlayer.id = id;
                        }

                        
                        id++;
                    }
                    scoreCoordinates[i] = new Vector2(coordinate.X, coordinate.Y);
                    coordinate.X += 32;
                }
            }
            Console.WriteLine("LOL");
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
            /* try
            {
                mcAnimatedSpriteParticleSystem = new AnimatedSpriteParticleSystem(null);
                mcAnimatedSpriteParticleSystem.AutoInitialize(this.GraphicsDevice, game.Content);
                mcAnimatedSpriteParticleSystem.MousePosition = new Vector3(200, 200, 200);
            }
            catch (InvalidOperationException)
            {
                // DoNothing();
            } */
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

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                MenuMode menu = (MenuMode)game.Services.GetService(typeof(MenuMode));
                menu.Enabled = true;
                menu.Visible = true;
                this.Enabled = false;
                this.Visible = false;
                MediaPlayer.Play(menu.backgroundMusic);
                MediaPlayer.Volume = 0.3f;
                MediaPlayer.IsRepeating = true;
            }

            if (timer.minutes == 0)
            {
                if (root.suddenDeathSettings != BattleManager.SuddenDeathSettings.off)
                {
                    if (timer.seconds == 43 && timer.steps == 59)
                    {
                        screenMessage = new MarqueeMessage(this, GraphicsLibrary.hurry, 60 * 2, Color.Red);
                    }
                    else
                    {
                        if (timer.seconds == 40 && timer.steps == 59)
                        {
                            StartSuddenDeath(60 * 39);
                        }
                    }
                }
            }

            if (timer.minutes == 0 && timer.seconds == 0 && timer.steps == 59)
            {
                screenMessage = new ScreenMessage(this, GraphicsLibrary.timesUp, 300, Color.White,root);
                matchOver = true;
                this.paused = true;
            }

            base.Update(gameTime);
            if (suddenDeathRightNow)
            {
                UpdateSuddenDeath();
            }
            
            AreYouDead();
            /* try
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
                } */
        }

        protected void UpdateSuddenDeath()
        {
            if (!paused)
            {
                suddenDeathCountdown--;
                if (suddenDeathCountdown == 0)
                {
                    if (fallingWallPositions.Count > 0)
                    {
                        new FallingWall(this, graphicsLibrary.Wall, fallingWallPositions.Dequeue(), new Point(32, 47), new Point(0, 0), new Point(8, 8));
                    }
                    suddenDeathCountdown = suddenDeathStepsPerWall;
                }
            }
        }

        protected void StartSuddenDeath(int maxTotalSteps)
        {
            suddenDeathRightNow = true;
            int numberOfWalls = (int)(arenaColumnCount * arenaRowCount - ((arenaColumnCount - 1) / 2) * ((arenaRowCount - 1) / 2));
            suddenDeathStepsPerWall = (int)Math.Floor((double)maxTotalSteps / (double)numberOfWalls);
            suddenDeathCountdown = suddenDeathStepsPerWall;
            Console.WriteLine("Number of walls: " + numberOfWalls);
            Console.WriteLine(suddenDeathStepsPerWall + " steps/wall");
            Drawable.Direction spiralDirection = Drawable.Direction.right;

            int leftLimit = arenaLeftmostColumn;
            int topLimit = arenaTopmostRow;
            int rightLimit = arenaRightmostColumn;
            int bottomLimit = arenaBottommostRow;

            int x = leftLimit;
            int y = topLimit;

            while (topLimit <= bottomLimit && leftLimit <= rightLimit)
            {
                Vector2 wallPosition = new Vector2(x, y) * Drawable.DEFAULT_GRID;
                if (root.suddenDeathSettings == BattleManager.SuddenDeathSettings.super)
                {
                    if (x % 2 != 1 || y % 2 != 1)
                    {
                        fallingWallPositions.Enqueue(wallPosition);
                    }
                }
                else
                {
                    if ((x % 2 != 1 || y % 2 != 1) && !(8 <= x && x <= 16 && 6 <= y && y <= 12))
                    {
                        fallingWallPositions.Enqueue(wallPosition);
                    }
                }

                switch (spiralDirection)
                {
                    case Drawable.Direction.right:
                        if (x < rightLimit)
                        {
                            x++;
                        }
                        else
                        {
                            spiralDirection = Drawable.Direction.down;
                            y++;
                            topLimit++;
                        }
                        break;
                    case Drawable.Direction.down:
                        if (y < bottomLimit)
                        {
                            y++;
                        }
                        else
                        {
                            spiralDirection = Drawable.Direction.left;
                            x--;
                            rightLimit--;
                        }
                        break;
                    case Drawable.Direction.left:
                        if (x > leftLimit)
                        {
                            x--;
                        }
                        else
                        {
                            spiralDirection = Drawable.Direction.up;
                            y--;
                            bottomLimit--;
                        }
                        break;
                    case Drawable.Direction.up:
                        if (y > topLimit)
                        {
                            y--;
                        }
                        else
                        {
                            spiralDirection = Drawable.Direction.right;
                            x++;
                            leftLimit++;
                        }
                        break;
                }
            }
        }

        protected void AreYouDead()
        {
            if (! matchOver)
            {
                if (drawGameCountdown > 0)
                {
                    switch (remainingTeamsCount)
                    {
                        case 0:
                            if (screenMessage != null)
                            {
                                screenMessage.Destroy();
                            }
                            screenMessage = new ScreenMessage(this, GraphicsLibrary.drawgame, 300, Color.White, root);
                            matchOver = true;
                            this.paused = true;
                            break;
                        case 1:
                            drawGameCountdown--;
                            break;
                    }
                }
                else
                {
                    if (screenMessage != null)
                    {
                        screenMessage.Destroy();
                    }
                    screenMessage = new ScreenMessage(this, GraphicsLibrary.winner, 300, Color.White, root);
                    string winners = "";
                    root.Winner((int)firstRemainingTeamIndex);
                    for (int i = 0; i < 8; i++)
                    {
                        Console.WriteLine("Team " + i + "(" + (i + 1) + "): " + root.Wins[i]);
                    }
                    bool comma = false;
                    foreach (Player player in teams[(int)firstRemainingTeamIndex])
                    {
                        if (!player.dead)
                        {
                            player.Visible = false;
                            new PlayerWinAnimation(this, player.character.spritesheet, new Point(32, 40), new Vector2(3, 25), player.position, player.character.winAnimationSequence);
                        }

                        BattleManager.PlayerData playerData = (BattleManager.PlayerData)(((BattleManager)root).PlayerDataAtIndex(player.id));
                        if (!comma)
                        {
                            winners = winners + playerData.name;
                        }
                        else
                        {
                            winners = winners + ", " + playerData.name;
                        }
                        comma = true;
                    }
                    screenMessage.text = winners;

                    matchOver = true;
                    this.paused = true;
                }
            }
        }
        
        public override void Draw(GameTime gameTime)
        {
           // Vector2 nowVector = new Vector2(Game.Window.ClientBounds.Width/2 - 25, 10);

            
            //Draw the Time
           // spriteBatch.DrawString(timeSmallFont, "Time", new Vector2(Game.Window.ClientBounds.Width / 2 - 10, 1), Color.White);
           // spriteBatch.DrawString(timeFont, time, nowVector, Color.Black);

            //Draw the Background

            if (arena == BattleManager.Arena.pirate)
            {
                for (int i = 0; i < 25; i++)
                {
                    spriteBatch.Draw(graphicsLibrary.water[waterTileSubimageIndex], new Vector2(i * Drawable.DEFAULT_GRID, 0), new Rectangle(0, 0, 32, 32), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    spriteBatch.Draw(graphicsLibrary.water[waterTileSubimageIndex], new Vector2(i * Drawable.DEFAULT_GRID, 32), new Rectangle(0, 0, 32, 32), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    spriteBatch.Draw(graphicsLibrary.water[waterTileSubimageIndex], new Vector2(i * Drawable.DEFAULT_GRID, 64), new Rectangle(0, 0, 32, 32), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    spriteBatch.Draw(graphicsLibrary.water[waterTileSubimageIndex], new Vector2(i * Drawable.DEFAULT_GRID, 96), new Rectangle(0, 0, 32, 32), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }

                waterTileSubimageIndex++;
            
                if (waterTileSubimageIndex >= 32)
                {
                    waterTileSubimageIndex -= 32;
                }
            }

            spriteBatch.Draw(graphicsLibrary.Background, new Vector2(0, 0), new Rectangle(0, 0, 800, 600), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.0001f);

            DrawCharacterMugshots();

            base.Draw(gameTime);
        }

        private void DrawCharacterMugshots()
        {
            for (byte i = 0; i < 8; i++)
            {
                if (mugshotCoordinates[i] != null)
                {
                    BattleManager.PlayerData data = (BattleManager.PlayerData)root.PlayerDataAtIndex(i);
                    spriteBatch.Draw(data.character.mugshot, (Vector2)mugshotCoordinates[i], null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                }
            }

            for (byte i = 0; i < 8; i++)
            {
                if (scoreCoordinates[i] != null)
                {
                     spriteBatch.Draw(scoreNumbers[wins[i]], (Vector2)scoreCoordinates[i], null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                }
            }
        }
    }
}