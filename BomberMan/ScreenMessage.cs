using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BomberBro
{
    public class ScreenMessage : Drawable
    {
        public static List<ScreenMessage> messages = new List<ScreenMessage>();

        public int timer;
        public Color blinkColor;
        public BattleManager battleManager = null;
        public SpriteFont arialRounded;
        public string text = "";

        public ScreenMessage(DrawableRoom room, Texture2D messageTexture, int timer, Color blinkColor, BattleManager battleManager)
            : base(room)
        {
            this.textureImage = messageTexture;
            this.timer = timer;
            this.origin = new Vector2(messageTexture.Width / 2, messageTexture.Height / 2);
            this.position = new Vector2(400, 300);
            this.frameSize = new Point(messageTexture.Width, messageTexture.Height);
            this.boundToGlobalOrigin = false;
            this.pausable = false;
            this.blinkColor = blinkColor;
            this.arialRounded = room.game.Content.Load<SpriteFont>(@"Font/ArialRoundedMTBold");
            this.battleManager = battleManager;
        }

        public ScreenMessage(DrawableRoom room, Texture2D messageTexture, int timer, Color blinkColor)
            : base(room)
        {
            this.textureImage = messageTexture;
            this.timer = timer;
            this.origin = new Vector2(messageTexture.Width / 2, messageTexture.Height / 2);
            this.position = new Vector2(400, 300);
            this.frameSize = new Point(messageTexture.Width, messageTexture.Height);
            this.boundToGlobalOrigin = false;
            this.pausable = false;
            this.blinkColor = blinkColor;
            this.arialRounded = room.game.Content.Load<SpriteFont>(@"Font/ArialRoundedMTBold");
        }

        public override void Update(GameTime gameTime)
        {
            if (timer % 5 == 0)
            {
                Color auxColor = blendColor;
                blendColor = blinkColor;
                blinkColor = auxColor;
            }
            timer -= 1;
            if (timer <= 0)
            {
                this.gonnaDie = true;
                if (battleManager != null)
                {
                    battleManager.CreateScoreboard();
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            depth = 1f;
            base.Draw(gameTime);
            room.DrawBorderedString(arialRounded, text, new Vector2(400, 350), 16, Color.White, Color.Black, 0f, arialRounded.MeasureString(text) / 2, 1f, SpriteEffects.None, 1f, 0.99999f);
        }
    }
}
