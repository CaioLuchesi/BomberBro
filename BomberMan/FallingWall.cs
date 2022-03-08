using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BomberBro
{
    class FallingWall : Shadow
    {
        public int fallingSpeed = 35;
        public bool gonnaBeDying = false;

        public FallingWall(DrawableRoom room, Texture2D textureImage, Vector2 position, Point frameSize,
        Point currentFrame, Point sheetSize):base(room)
        {
            this.textureImage = textureImage;
            this.position.X = position.X; // setX(position.X);
            this.position.Y = position.Y; // setY(position.Y);
            this.frameSize = frameSize;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.collisionMask = new Rectangle(0, 0, 32, 32);
            this.origin = new Vector2(0, 16);
            this.checkForCollisions = true;
            this.z = 600 + room.globalOrigin.Y;
        }

        public override void Update(GameTime gameTime)
        {
            if (gonnaBeDying)
            {
                gonnaDie = true;
            }
            else
            {
                z -= fallingSpeed;
                if (z < 0)
                {
                    z = 0;
                    TurnIntoAWall();
                }
            }
 	        base.Update(gameTime);
        }

        public void TurnIntoAWall()
        {
            Drawable victim = instanceMeeting<Bomb>();
            if (victim != null)
            {
                Bomb victimAsBomb = (Bomb)victim;
                if (victimAsBomb.grabbingState == Grabbable.GrabbingState.nothing || victimAsBomb.grabbingState == Grabbable.GrabbingState.landing)
                {
                    victimAsBomb.Explode();
                }
            }

            victim = instanceMeeting<Wall>();
            if (victim != null)
            {
                try
                {
                    ((BreakableWall)victim).hiddenPowerUp = null;
                }
                catch (InvalidCastException)
                {
                    // LOL
                }
                victim.gonnaDie = true;
            }

            victim = instanceMeeting<PowerUp>();
            if (victim != null)
            {
                victim.gonnaDie = true;
            }

            victim = instanceMeeting<Player>();
            if (victim != null)
            {
                Player victimAsPlayer = (Player)victim;
                if (victimAsPlayer.grabbingState == Grabbable.GrabbingState.nothing || victimAsPlayer.grabbingState == Grabbable.GrabbingState.landing || victimAsPlayer.grabbingState == Grabbable.GrabbingState.grabbing)
                {
                    victimAsPlayer.gonnaDie = true; ;
                }
            }

            new Wall(this.room, this.textureImage, this.position, new Point(32, 47), new Point(0, 0), new Point(8, 8));
            gonnaBeDying = true;
        }
    }
}
