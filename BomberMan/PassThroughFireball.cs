using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace BomberBro
{
    class PassThroughFireball : Fireball
    {
        public PassThroughFireball(DrawableRoom room, Player owner, Revenger revenger, int firepower, Direction direction, Vector2 position)
            : base(room, owner, revenger, firepower, direction, position)
        { }

        public override void CollisionEvent(Drawable other)
        {
            Type type = other.GetType();
            
            if (type == typeof(Wall))
            {
                gonnaDie = true;
            }

            if (type == typeof(BreakableWall))
            {
                other.gonnaDie = true;
                // gonnaDie stays false - PassThroughFire doesn't stop at any instance of BreakableWall
            }

            // base.CollisionEvent(other); // No need to use base.CollisionEvent(other);
        }
    }
}
