using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using BomberBro.Core;

namespace BomberBro
{
    /// <summary>
    /// This is a game component that implements the Action Scene.
    /// </summary>
    public class ActionScene : BomberBro.Core.GameScene
    {
        // Basics
        protected Texture2D actionTexture;
        private AudioLibrary audio;
        protected SpriteBatch spriteBatch = null;
       
       
        // Game Elements
        protected SpriteFont font;
        protected ImageComponent background;
        protected Match battleGround;

        // Gui Stuff
        protected Vector2 pausePosition;
        protected Vector2 gameoverPosition;
        protected Rectangle pauseRect = new Rectangle(1, 120, 200, 44);
        protected Rectangle gameoverRect = new Rectangle(1, 170, 350, 48);

        // GameState elements
        protected bool paused;
        protected bool gameOver;
        protected TimeSpan elapsedTime = TimeSpan.Zero;
        protected bool twoPlayers;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="game">The main game object</param>
        /// <param name="theTexture">Texture with the sprite elements</param>
        /// <param name="backgroundTexture">Texture for the background</param>
        /// <param name="font">Font used in the score</param>
        public ActionScene(Game game,byte tipo)
            : base(game)
        {

            // Get the current sprite batch
            spriteBatch = (SpriteBatch)
                Game.Services.GetService(typeof(SpriteBatch));

            // Get the audio library
            audio = (AudioLibrary)
                Game.Services.GetService(typeof(AudioLibrary));
            PowerUpManager defaultPowerUpManager = new PowerUpManager(9, 9, 9, 5, 3, 2, 2, 1, 2, 1, 1);

            // battleGround = new Match(game, tipo, defaultPowerUpManager);
            // Components.Add(battleGround);

            battleGround.Visible = false;
            battleGround.Enabled = false;
            
            font = Game.Content.Load<SpriteFont>("Font/Pause_GameOver");
        }
        

        /// <summary>
        /// Show the action scene
        /// </summary>
        public override void Show()
        {
            // MediaPlayer.Play(audio.BackMusic);


            battleGround.Visible = true;
            battleGround.Enabled = true;
            
            paused = false;
            pausePosition.X = (Game.Window.ClientBounds.Width - pauseRect.Width) / 2 + 50;
            pausePosition.Y = (Game.Window.ClientBounds.Height - pauseRect.Height) / 2;

            gameOver = false;
            gameoverPosition.X = (Game.Window.ClientBounds.Width - gameoverRect.Width) / 2;
            gameoverPosition.Y = (Game.Window.ClientBounds.Height - gameoverRect.Height) / 2;

            base.Show();
        }

        /// <summary>
        /// Hide the scene
        /// </summary>
        public override void Hide()
        {
           // Stop the background music
           MediaPlayer.Stop();

           base.Hide();
        }

        /// <summary>
        /// Indicate the 2-players game mode
        /// </summary>
        public bool TwoPlayers
        {
            get { return twoPlayers; }
            set { twoPlayers = value; }
        }

        /// <summary>
        /// True, if the game is in GameOver state
        /// </summary>
        public bool GameOver
        {
            get { return gameOver; }
        }

        /// <summary>
        /// Paused mode
        /// </summary>
        public bool Paused
        {
            get { return paused; }
            set
            {
                paused = value;
                if (paused)
                {
                    MediaPlayer.Pause();
                    battleGround.Enabled = false;
                    
                }
                else
                {
                    MediaPlayer.Resume();
                    battleGround.Enabled = true;
                }
            }
        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        public void battleGroundDipose()
        {
            // TODO: Unload any non ContentManager content here
            battleGround.Dispose();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            
            base.Update(gameTime);
        }

        /// <summary>
        /// Allows the game component to draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
           // Game.GraphicsDevice.Clear(Color.White);

            // Draw all game components
            base.Draw(gameTime);


            if (paused)
            {
                // Draw the "pause" text

                spriteBatch.DrawString(font, "PAUSE", new Vector2(pausePosition.X, pausePosition.Y), Color.Black);
                
            }
            if (gameOver)
            {
                // Draw the "gameover" text
                spriteBatch.DrawString(font, "GAME OVER", new Vector2(pausePosition.X, pausePosition.Y),Color.Black);
            }
        }
        
    }
}