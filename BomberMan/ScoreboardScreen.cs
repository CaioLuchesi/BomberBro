using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace BomberBro
{
    public class ScoreboardScreen : DrawableRoom
    {
        public BattleManager root = null;
        private Match match;
        private Texture2D background, whitePixel, trophy, frags;
        int backgroundOrigin = 0;
        private SpriteFont arialRoundedBig;
        private SpriteFont arialRounded;

        public ScoreboardScreen(Game game, BattleManager root, Match match)
            : base(game)
        {
            this.root = root;
            this.match = match;
            this.background = game.Content.Load<Texture2D>(@"Images/Menu/Menu Background");
            this.whitePixel = game.Content.Load<Texture2D>(@"Images/Menu/WhitePixel");
            this.trophy = game.Content.Load<Texture2D>(@"Images/Scoreboard/Trophy");
            this.frags = game.Content.Load<Texture2D>(@"Images/Scoreboard/skulls");
            this.arialRoundedBig = game.Content.Load<SpriteFont>(@"Font/ArialRoundedMTBoldAndBig");
            this.arialRounded = game.Content.Load<SpriteFont>(@"Font/ArialRoundedMTBold");
        }

        public override void Update(GameTime gameTime)
        {
            if (keyboard != null)
            {
                if (keyboard.CheckPressed(Keys.Enter))
                {
                    if (root != null)
                    {
                        if (root.winnerTeamID == null)
                        {
                            root.CreateMatch();
                        }
                        else
                        {
                            root.DisplayVictoryScreen();
                        }
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
            DrawFancyBackground();

            Vector2 mainPosition = new Vector2(86, 30);
            Vector2 mugshotDistance = new Vector2(44, 54);
            Vector2 trophyDistance = new Vector2(110, 54);
            Vector2 fragsDistance = new Vector2(210, 54);
            int inc = 0;
            for (int i = 0; i < 8; i++)
            {
                if (match.teams[i] != null)
                {
                    foreach (Player player in match.teams[i])
                    {

                        if (inc > 3)
                        {
                            spriteBatch.Draw(GraphicsLibrary.scoreboard, new Vector2(mainPosition.X + GraphicsLibrary.scoreboard.Width + 20, mainPosition.Y), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.0001f);
                            spriteBatch.Draw(player.character.mugshot, new Vector2(mainPosition.X + GraphicsLibrary.scoreboard.Width + 20, mainPosition.Y) + mugshotDistance, null, Color.White, 0, new Vector2(player.character.mugshot.Width, player.character.mugshot.Height) / 2, 1.23f, SpriteEffects.None, 0.0002f);
                            spriteBatch.Draw(trophy, new Vector2(mainPosition.X + GraphicsLibrary.scoreboard.Width + 20, mainPosition.Y) + trophyDistance, null, Color.White, 0, new Vector2(trophy.Width, trophy.Height) / 2, 1f, SpriteEffects.None, 0.0002f);
                            DrawBorderedString(arialRounded, "WINS", new Vector2(mainPosition.X + GraphicsLibrary.scoreboard.Width + 20, mainPosition.Y + trophy.Height - 13) + trophyDistance, 32, Color.White, Color.Black, 0f, arialRounded.MeasureString("WINS") / 2, 1f, SpriteEffects.None, 0.0002f, 0.00019f);
                            DrawBorderedString(arialRoundedBig, root.Wins[i].ToString(), new Vector2(mainPosition.X + GraphicsLibrary.scoreboard.Width + 13 + trophy.Width, mainPosition.Y + 5) + trophyDistance, 32, Color.White, Color.Red, 0f, arialRoundedBig.MeasureString(root.Wins[i].ToString()) / 2, 1f, SpriteEffects.None, 0.0002f, 0.00019f);
                            spriteBatch.Draw(frags, new Vector2(mainPosition.X + GraphicsLibrary.scoreboard.Width + 20, mainPosition.Y) + fragsDistance, null, Color.White, 0, new Vector2(frags.Width, frags.Height) / 2, 1f, SpriteEffects.None, 0.0002f);
                            DrawBorderedString(arialRounded, "FRAGS", new Vector2(mainPosition.X + GraphicsLibrary.scoreboard.Width + 20, mainPosition.Y + trophy.Height - 13) + fragsDistance, 32, Color.White, Color.Black, 0f, arialRounded.MeasureString("FRAGS") / 2, 1f, SpriteEffects.None, 0.0002f, 0.00019f);
                            mainPosition += new Vector2(0, 140);
                        }
                        else
                        {
                            spriteBatch.Draw(GraphicsLibrary.scoreboard, mainPosition, null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.0001f);
                            spriteBatch.Draw(player.character.mugshot, mainPosition + mugshotDistance, null, Color.White, 0, new Vector2(player.character.mugshot.Width, player.character.mugshot.Height) / 2, 1.23f, SpriteEffects.None, 0.0002f);
                            spriteBatch.Draw(trophy, mainPosition + trophyDistance, null, Color.White, 0, new Vector2(trophy.Width, trophy.Height) / 2, 1f, SpriteEffects.None, 0.0002f);
                            DrawBorderedString(arialRounded, "WINS", new Vector2(mainPosition.X, mainPosition.Y + trophy.Height - 13) + trophyDistance, 32, Color.White, Color.Black, 0f, arialRounded.MeasureString("WINS") / 2, 1f, SpriteEffects.None, 0.0002f, 0.00019f);
                            DrawBorderedString(arialRoundedBig, root.Wins[i].ToString(), new Vector2(mainPosition.X + trophy.Width - 7, mainPosition.Y + 5) + trophyDistance, 32, Color.White, Color.Red, 0f, arialRoundedBig.MeasureString(root.Wins[i].ToString()) / 2, 1f, SpriteEffects.None, 0.0002f, 0.00019f);
                            spriteBatch.Draw(frags, mainPosition + fragsDistance, null, Color.White, 0, new Vector2(frags.Width, frags.Height) / 2, 1f, SpriteEffects.None, 0.0002f);
                            DrawBorderedString(arialRounded, "FRAGS", new Vector2(mainPosition.X, mainPosition.Y + trophy.Height - 13) + fragsDistance, 32, Color.White, Color.Black, 0f, arialRounded.MeasureString("FRAGS") / 2, 1f, SpriteEffects.None, 0.0002f, 0.00019f);
                            mainPosition += new Vector2(0, 140);
                        }
                        if (inc == 3)
                            mainPosition = new Vector2(86, 30);
                        inc++;
                    }
                }
            }
            base.Draw(gameTime);
        }
    }
}
