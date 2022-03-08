using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BomberBro
{
    public class Shadow : Drawable
    {
        protected Texture2D shadow;
        protected Vector2 shadowOrigin = new Vector2(3, -5);

        public Shadow(DrawableRoom room)
            : base(room)
        {
            shadow = Game.Content.Load<Texture2D>(@"Images/Character/shadow");
        }

        public override void Draw(GameTime gameTime)
        {
            room.spriteBatch.Draw(shadow,
            new Vector2(position.X + room.globalOrigin.X, position.Y + room.globalOrigin.Y),
            new Rectangle(0, 0, 32, 10),
            Color.White, 0, shadowOrigin,
            1.3f, SpriteEffects.None, (position.Y - 1) / 2000);

            base.Draw(gameTime);
        }
    }
}
