using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BomberBro
{
    public class Fire : Drawable
    {
        protected Player owner;
        protected Revenger revenger;

        public Fire(DrawableRoom room, Player owner, Revenger revenger)
            : base(room)
        {
            this.owner = owner;
            this.revenger = revenger;
        }
    }
}
