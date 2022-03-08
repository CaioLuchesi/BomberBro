using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BomberBro
{
    class FireSquare : Fire
    {
        public int timer = 30;

        public FireSquare(DrawableRoom room, Player owner, Revenger revenger, Vector2 position)
            : base(room, owner, revenger)
        {
            this.position = new Vector2((float)(Math.Round(position.X / DEFAULT_GRID) * DEFAULT_GRID), (float)(Math.Round(position.Y / DEFAULT_GRID) * DEFAULT_GRID));
        }

        public override void Update(GameTime gameTime)
        {
            timer -= 1;
            if (timer <= 0)
            {
                this.gonnaDie = true;
            }
            base.Update(gameTime);
        }

        public override void LoadTextureAndAllThoseStuff()
        {
            textureImage = GraphicsLibrary.fireWall;
            origin = new Vector2(0, 16);
            collisionMask = new Rectangle(0, 0, 32, 32);
            frameSize = new Point(32, 48);
        }

        public override void CollisionEvent(Drawable other)
        {
            if (other.GetType() == typeof(BreakableWall))
            {
                other.gonnaDie = true;
            }
            base.CollisionEvent(other);
        }
    }
}
