using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BomberBro
{
    class BombKick : PowerUp
    {
        public BombKick(DrawableRoom room, Vector2 position)
            : base(room, position) {
        }

        public override void LoadTextureAndAllThoseStuff()
        {
            textureImage = GraphicsLibrary.bombKick;
            origin = new Vector2(0, 16);
            collisionMask = new Rectangle(0, 0, 32, 32);
            frameSize = new Point(32, 32);
        }

        public override void Activate(Player other)
        {
            other.bombKick = true;
            base.Activate(other);
        }
    }
}
