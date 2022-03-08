using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BomberBro
{
    class MarqueeMessage : ScreenMessage
    {
        protected float speed;

        public MarqueeMessage(DrawableRoom room, Texture2D messageTexture, int timer, Color blinkColor)
            : base(room, messageTexture, timer, blinkColor)
        {
            this.origin = new Vector2(messageTexture.Width, messageTexture.Height / 2);
            this.position = new Vector2(800 + messageTexture.Width, 300);
            this.speed = (800f + (float)messageTexture.Width) / (float)timer;
        }

        public override void Update(GameTime gameTime)
        {
            this.position.X -= speed;
            base.Update(gameTime);
        }
    }
}
