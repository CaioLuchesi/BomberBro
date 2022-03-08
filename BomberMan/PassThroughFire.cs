using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace BomberBro
{
    class PassThroughFire : Fire
    {
        public PassThroughFire(Game game, Player owner, int firepower, Direction direction, Vector2 position):base(game, owner, firepower,direction, position)
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
