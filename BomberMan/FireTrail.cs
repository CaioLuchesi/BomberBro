using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BomberBro
{
    public class FireTrail : Fire
    {
        private int timer = 15;

        public FireTrail(DrawableRoom room, Player owner, Revenger revenger, Vector2 position)
            : base(room, owner, revenger)
        {
            this.position = position;
            this.collisionMask = new Rectangle(2, 2, 28, 28);
            this.Visible = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (timer <= 0)
            {
                this.Destroy();
            }
            else
            {
                timer--;
            }
            base.Update(gameTime);
        }
    }
}
