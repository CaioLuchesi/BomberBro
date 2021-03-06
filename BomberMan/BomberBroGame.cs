using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using DPSF;
//using DPSF.ParticleSystems;

namespace BomberBro
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// // Represents different states of the game
   
    public class BomberBroGame : Microsoft.Xna.Framework.Game
    {
        /* Matrix msWorldMatrix = Matrix.Identity;
        Matrix msViewMatrix = Matrix.Identity;
        Matrix msProjectionMatrix = Matrix.Identity; */

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteSortMode frontToBack = new SpriteSortMode();
        private SpriteSortMode sortMode = new SpriteSortMode();
               
 



        // Audio Stuff
        private AudioLibrary audio;




        private Vector2 pos1 = new Vector2(0, 0);
        private int State = 1;
       
        protected KeyboardState oldKeyboardState;
        protected GamePadState oldGamePadState;

        public AnimatedSpriteParticleSystem mcAnimatedSpriteParticleSystem = null;
        MouseState mcCurrentMouseState;         // Holds the Mouse's Current State
        MouseState mcPreviousMouseState;        // Holds the Mouse's Previous State
        Match match;
        public BomberBroGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Used for input handling
            oldKeyboardState = Keyboard.GetState();
            oldGamePadState = GamePad.GetState(PlayerIndex.One);
            frontToBack = SpriteSortMode.FrontToBack;
            // sortMode = SpriteSortMode.Immediate;
        }

        public void AssignCharacterTextures()
        {
            Character.bishop = new Character(Content.Load<Texture2D>(@"Images/Character/BombermanBishop"), Content.Load<Texture2D>(@"Images/Mugshots/bishop"), 0);
            Character.blackBomber = new Character(Content.Load<Texture2D>(@"Images/Character/BombermanBlack"), Content.Load<Texture2D>(@"Images/Mugshots/blackbomber"), 0);
            Character.blueBomber = new Character(Content.Load<Texture2D>(@"Images/Character/BombermanBlue"), Content.Load<Texture2D>(@"Images/Mugshots/bluebomber"), 0);
            Character.fairy = new Character(Content.Load<Texture2D>(@"Images/Character/BombermanFairy"), Content.Load<Texture2D>(@"Images/Mugshots/fairy"), 1);
            Character.greenBomber = new Character(Content.Load<Texture2D>(@"Images/Character/BombermanGreen"), Content.Load<Texture2D>(@"Images/Mugshots/greenbomber"), 0);
            Character.hero = new Character(Content.Load<Texture2D>(@"Images/Character/BombermanHero"), Content.Load<Texture2D>(@"Images/Mugshots/hero"), 2);
            Character.merchant = new Character(Content.Load<Texture2D>(@"Images/Character/BombermanMerchant"), Content.Load<Texture2D>(@"Images/Mugshots/merchant"), 0);
            Character.monk = new Character(Content.Load<Texture2D>(@"Images/Character/BombermanMonk"), Content.Load<Texture2D>(@"Images/Mugshots/monk"), 3);
            Character.ninja = new Character(Content.Load<Texture2D>(@"Images/Character/BombermanNinja"), Content.Load<Texture2D>(@"Images/Mugshots/ninja"), 3);
            Character.redBomber = new Character(Content.Load<Texture2D>(@"Images/Character/BombermanRed"), Content.Load<Texture2D>(@"Images/Mugshots/redbomber"), 0);
            Character.whiteBomber = new Character(Content.Load<Texture2D>(@"Images/Character/BombermanWhite"), Content.Load<Texture2D>(@"Images/Mugshots/whitebomber"), 0);
            Character.witch = new Character(Content.Load<Texture2D>(@"Images/Character/BombermanWitch"), Content.Load<Texture2D>(@"Images/Mugshots/witch"), 2);
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
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Para escolher tamanho da janela ou modo fullscreen:
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), spriteBatch);
            Services.AddService(typeof(BomberBroGame), this);
            // Load Audio Elements
            audio = new AudioLibrary();
            audio.LoadContent(Content);
            Services.AddService(typeof(AudioLibrary), audio);

     

         
            
            AssignCharacterTextures();
            sortMode = frontToBack;

            ShowMenu();

            /* List<BattleManager.PlayerData>[] playerData = new List<BattleManager.PlayerData>[8];
            playerData[0] = new List<BattleManager.PlayerData>();
            playerData[1] = new List<BattleManager.PlayerData>();
            playerData[2] = new List<BattleManager.PlayerData>();
            playerData[3] = new List<BattleManager.PlayerData>();
            playerData[4] = new List<BattleManager.PlayerData>();
            playerData[5] = new List<BattleManager.PlayerData>();
            playerData[6] = new List<BattleManager.PlayerData>();
            playerData[7] = new List<BattleManager.PlayerData>();

            playerData[0].Add(new BattleManager.PlayerData("TalkingLunchBag555", Character.ninja, new KeyboardMapping(Keys.W, Keys.S, Keys.A, Keys.D, Keys.LeftShift, Keys.LeftControl, Keys.LeftAlt, Keys.Space)));
            playerData[1].Add(new BattleManager.PlayerData("YOU-ALL-SUCK", Character.merchant, new KeyboardMapping(Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.NumPad1, Keys.NumPad3, Keys.NumPad2, Keys.NumPad6)));
            playerData[2].Add(new BattleManager.PlayerData("ReallyBadPlayer", Character.bishop, new KeyboardMapping(Keys.I, Keys.K, Keys.J, Keys.L, Keys.RightShift, Keys.RightControl, Keys.Enter, Keys.Back)));
            playerData[3].Add(new BattleManager.PlayerData("HarryPotterFanGirl", Character.witch, new KeyboardMapping(Keys.NumPad8, Keys.NumPad5, Keys.NumPad4, Keys.NumPad6, Keys.NumPad7, Keys.NumPad9, Keys.Add, Keys.Multiply)));
            playerData[4].Add(new BattleManager.PlayerData("WatchMeWin", Character.hero, new KeyboardMapping(Keys.W, Keys.S, Keys.A, Keys.D, Keys.LeftShift, Keys.LeftControl, Keys.LeftAlt, Keys.Space)));
            playerData[5].Add(new BattleManager.PlayerData("xXx_SePhIrOtDaRkS0NiC1997_xXx", Character.monk, new KeyboardMapping(Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.NumPad1, Keys.NumPad3, Keys.NumPad2, Keys.NumPad6)));
            playerData[6].Add(new BattleManager.PlayerData("ObamaBinLaden", Character.whiteBomber, new KeyboardMapping(Keys.I, Keys.K, Keys.J, Keys.L, Keys.RightShift, Keys.RightControl, Keys.Enter, Keys.Back)));
            playerData[7].Add(new BattleManager.PlayerData("A_Freaking_Bus", Character.blueBomber, new KeyboardMapping(Keys.NumPad8, Keys.NumPad5, Keys.NumPad4, Keys.NumPad6, Keys.NumPad7, Keys.NumPad9, Keys.Add, Keys.Multiply)));

            BattleManager battleManager = new BattleManager(this, playerData, new PowerUpManager(12, 12, 12, 3, 3, 3, 2, 2, 9, 2, 2), GraphicsLibrary.PIRATE, 2, 5, BattleManager.SuddenDeathSettings.off, BattleManager.RevengeSettings.on);
            battleManager.CreateMatch(); */

            try
            {
                mcAnimatedSpriteParticleSystem = new AnimatedSpriteParticleSystem(this);
                mcAnimatedSpriteParticleSystem.AutoInitialize(this.GraphicsDevice, this.Content);
                mcAnimatedSpriteParticleSystem.MousePosition = new Vector3(200, 200, 200);
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("WTH?");
                // DoNothing();
            }
        }

        public void ShowMenu()
        {
            Components.Clear();
            MenuMode menu = new MenuMode(this);
            Components.Add(menu);
            menu.Visible = true;
            menu.Enabled = true;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            //texture.Dispose();
            mcAnimatedSpriteParticleSystem.Destroy();
            spriteBatch.Dispose();
            Content.Unload();
            this.Components.Clear();
        }

   
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            /* match.Visible = true;
            match.Enabled = true; */ 
            // Handle Game Inputs
            //ScenesInput();

            try
            {
                // Save the Mouse State and get its new State
                    // mcAnimatedSpriteParticleSystem.EmitAt(gameTime, new Vector3(900, 700, 0));
                mcAnimatedSpriteParticleSystem.MousePosition = new Vector3(900, 700, 0);

                mcAnimatedSpriteParticleSystem.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            }
            catch (NullReferenceException)
            {
                // DoNothing();
            }
           
            base.Update(gameTime);
        }



        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Begin..
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend,sortMode,SaveStateMode.None);

            // Draw all Game Components..
            base.Draw(gameTime);

            // End.
            spriteBatch.End();

            try
            {
                mcAnimatedSpriteParticleSystem.Draw();
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("FAIL 404");
                // DoNothing();
            }
        }

        public void setSortMode(SpriteSortMode s)
        {
            this.sortMode = s;
        }
        public SpriteSortMode getSortMode()
        {
            return this.sortMode;
        }
        public BomberBroGame getBomberBro()
        {
            return this;
        }
    }
}