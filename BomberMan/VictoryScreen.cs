using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace BomberBro
{
    class VictoryScreen: DrawableRoom
    {
        int index = 0;
        private float timer = 20;
        private const float speedSprite = 20;
        private List<Point> animationSequence;
        Song victoryFanfare;

        public BattleManager root = null;
        private Texture2D background, whitePixel, trophy, victory;
        int backgroundOrigin = 0;
        private SpriteFont arialRoundedBigger;
        private SpriteFont arialRounded;

        public VictoryScreen(Game game, BattleManager root)
            : base(game)
        {
            this.root = root;
            this.background = game.Content.Load<Texture2D>(@"Images/Menu/Menu Background");
            this.whitePixel = game.Content.Load<Texture2D>(@"Images/Menu/WhitePixel");
            this.trophy = game.Content.Load<Texture2D>(@"Images/Scoreboard/Trophy");
            this.arialRoundedBigger = game.Content.Load<SpriteFont>(@"Font/ArialRoundedMTBoldAndBigger");
            this.victory = game.Content.Load<Texture2D>(@"Images/Scoreboard/victory");
            this.victoryFanfare = game.Content.Load<Song>(@"sons/Bomberman World Battle");
            MediaPlayer.Play(victoryFanfare);
            MediaPlayer.Volume = 0.3f;
            MediaPlayer.IsRepeating = true;

            foreach (BattleManager.PlayerData player in root.teamData[(int)root.winnerTeamID])
            {
                new PlayerWinAnimation(this, player.character.spritesheet, new Point(32, 40), new Vector2(3, 25), new Vector2(384, 300), player.character.winAnimationSequence);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (keyboard != null)
            {
                if (keyboard.CheckPressed(Keys.Escape))
                {
                    if (root != null)
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
                }
            }
            base.Update(gameTime);
        }

        private void DrawFancyBackground()
        {
            if (backgroundOrigin >= 33)
            {
                backgroundOrigin = 0;
            }
            spriteBatch.Draw(background, new Vector2(-backgroundOrigin), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            backgroundOrigin++;
        }

        public override void Draw(GameTime gameTime)
        {           
            spriteBatch.Draw(victory, new Vector2(400, 100), null, Color.White, 0, new Vector2(victory.Width, victory.Height) / 2, 1f, SpriteEffects.None, 0f);
            
            foreach (BattleManager.PlayerData player in root.teamData[(int)root.winnerTeamID])
            {
                DrawBorderedString(arialRoundedBigger, player.name, new Vector2(400, 400), 8, Color.White, Color.Black, 0f, arialRoundedBigger.MeasureString(player.name) / 2, 1f, SpriteEffects.None, 1f, 0.999f);
            }
            DrawFancyBackground();
            base.Draw(gameTime);
        }
    }
}
