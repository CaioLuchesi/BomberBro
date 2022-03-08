using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BomberBro
{
    class LineBomb : PowerUp
    {
        public LineBomb(DrawableRoom room, Vector2 position)
            : base(room, position) {
        }

        public override void LoadTextureAndAllThoseStuff()
        {
            textureImage = GraphicsLibrary.lineBomb;
            origin = new Vector2(0, 16);
            collisionMask = new Rectangle(0, 0, 32, 32);
            frameSize = new Point(32, 32);
        }

        public override void Activate(Player other)
        {
            other.lineBomb = true;
            base.Activate(other);
        }
    }
}
