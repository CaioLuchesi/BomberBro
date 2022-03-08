using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BomberBro
{
    public class Grabbable : Shadow
    {
        public const float AIR_TIME = 20; // in game steps
        public const float VERTICAL_SPEED_INITIAL_PARAMETER = 2;
        public const float GRABBED_Z = 28;

        public Direction direction = Direction.down;
        public Grabbable other = null;

        public int wallsLeft = 3;
        
        public GrabbingState grabbingState = GrabbingState.nothing;

        protected float horizontalAirSpeed;
        protected float verticalAirSpeed;
        protected float verticalAirAcceleration;
        protected Vector2 targetLandingPosition;
       
        public enum GrabbingState
        {
            nothing = 0,
            grabbed = 1,
            flying = 2,
            landing = 3,
            grabbing = 4,
        }

        public Grabbable(DrawableRoom room) : base(room) { }

        protected virtual void Grab(Grabbable victim)
        {
            this.other = victim;
            this.grabbingState = GrabbingState.grabbing;
            victim.BeGrabbedBy(this);
            // victim.other = this;
            // victim.grabbingState = GrabbingState.grabbed; // pause bomb's timer
        }

        protected virtual void BeGrabbedBy(Grabbable grabber)
        {
            this.ThrowAway();
            this.other = grabber;
            this.grabbingState = GrabbingState.grabbed;
            this.z = GRABBED_Z;
        }

        public virtual void ThrowAway()
        {
            if (this.grabbingState == GrabbingState.grabbing)
            {
                this.grabbingState = GrabbingState.nothing;
                other.AfterBeingThrown(6, direction);
            }
        }

        protected virtual void KeepGrabbing()
        {
            other.position = this.position;
            other.direction = this.direction;
        }

        /// <summary>
        /// Sends an instance of Grabbable flying away
        /// </summary>
        /// <param name="distance">The distance in block units</param>
        /// <param name="direction">The direction</param>
        public virtual void AfterBeingThrown(int distance, Direction direction)
        {
            // other = null;
            grabbingState = GrabbingState.flying;
            alignToDefaultGrid();
            this.targetLandingPosition = this.position;
            this.direction = direction;
            switch (direction)
            {
                case Direction.right:
                    targetLandingPosition.X += distance * DEFAULT_GRID;
                    break;
                case Direction.up:
                    targetLandingPosition.Y -= distance * DEFAULT_GRID;
                    break;
                case Direction.left:
                    targetLandingPosition.X -= distance * DEFAULT_GRID;
                    break;
                case Direction.down:
                    targetLandingPosition.Y += distance * DEFAULT_GRID;
                    break;
            }
            horizontalAirSpeed = distance * DEFAULT_GRID / AIR_TIME;
            verticalAirSpeed = VERTICAL_SPEED_INITIAL_PARAMETER * horizontalAirSpeed;
            verticalAirAcceleration = -2 * verticalAirSpeed / AIR_TIME;
            /* Console.WriteLine("distance: " + distance * DEFAULT_GRID);
            Console.WriteLine("targetLandingPosition: " + targetLandingPosition);
            Console.WriteLine("horizontalAirSpeed: " + horizontalAirSpeed);
            Console.WriteLine("verticalAirSpeed: " + verticalAirSpeed);
            Console.WriteLine("verticalAirAcceleration: " + verticalAirAcceleration); */
        }

        protected virtual void LandingAttempt()
        {
            // check what is below
            z = GRABBED_Z;
            if (this.instanceMeeting<Wall>() != null)
            {
                if (this.instanceMeeting<BreakableWall>() == null)
                {
                    wallsLeft--;
                    if (wallsLeft <= 0)
                    {
                        if (this.GetType() == typeof(Player))
                        {
                            this.gonnaDie = true;
                        }
                    }
                }
                else
                {
                    wallsLeft = 3;
                }
                AfterBeingThrown(1, direction);
            }
            else
            {
                wallsLeft = 3;
                Grabbable below = (Grabbable)this.instanceMeeting<Grabbable>();
                if (below == null)
                {
                    BeforeLanding();
                }
                else
                {
                    if (below.grabbingState == GrabbingState.nothing || below.grabbingState == GrabbingState.landing)
                    {
                        if (below.GetType() == typeof(Player) || below.GetType().IsSubclassOf(typeof(Player)))
                        {
                            ((Player)below).OhNoNotInTheHead();
                        }
                        AfterBeingThrown(1, direction);
                    }
                    else
                    {
                        BeforeLanding();
                    }
                }
            }
        }

        protected virtual void BeforeLanding()
        {
            grabbingState = GrabbingState.landing;
        }

        protected virtual void AfterLanding()
        {
            wallsLeft = 3;
            grabbingState = GrabbingState.nothing;
        }

        private void WrapAroundField()
        {
            Match m = (Match)room;
            if (position.X < DEFAULT_GRID * (m.arenaLeftmostColumn - 0.5)) // 3.5
            {
                position.X += DEFAULT_GRID * m.arenaColumnCount; // 17
                targetLandingPosition.X += DEFAULT_GRID * m.arenaColumnCount;
            }

            if (position.X > DEFAULT_GRID * (m.arenaRightmostColumn + 0.5)) // 20.5
            {
                position.X -= DEFAULT_GRID * m.arenaColumnCount; // 17
                targetLandingPosition.X -= DEFAULT_GRID * m.arenaColumnCount;
            }

            if (position.Y < DEFAULT_GRID * (m.arenaTopmostRow - 0.5)) // 3.5
            {
                position.Y += DEFAULT_GRID * m.arenaRowCount; // 11
                targetLandingPosition.Y += DEFAULT_GRID * m.arenaRowCount;
            }

            if (position.Y > DEFAULT_GRID * (m.arenaBottommostRow + 0.5)) // 14.5
            {
                position.Y -= DEFAULT_GRID * m.arenaRowCount;
                targetLandingPosition.Y -= DEFAULT_GRID * m.arenaRowCount;
            }
        }

        public override void Update(GameTime gameTime)
        {
            switch (grabbingState)
            {
                case GrabbingState.flying:

                    WrapAroundField();

                    // Console.WriteLine("Z position set to " + z);
                    verticalAirSpeed += verticalAirAcceleration;
                    // Console.WriteLine("Vertical Speed set to " + verticalAirSpeed);
                    z += verticalAirSpeed;
                    // Console.WriteLine("Z position set to " + z);
                    

                    switch (this.direction)
                    {
                        case Direction.right:
                            if (position.X + horizontalAirSpeed <= targetLandingPosition.X)
                            {
                                position.X += horizontalAirSpeed;
                            }
                            else
                            {
                                position.X = targetLandingPosition.X;
                                LandingAttempt();
                            }
                            break;
                        case Direction.up:
                            if (position.Y - horizontalAirSpeed >= targetLandingPosition.Y)
                            {
                                position.Y -= horizontalAirSpeed;
                            }
                            else
                            {
                                position.Y = targetLandingPosition.Y;
                                LandingAttempt();
                            }
                            break;
                        case Direction.left:
                            if (position.X - horizontalAirSpeed >= targetLandingPosition.X)
                            {
                                position.X -= horizontalAirSpeed;
                            }
                            else
                            {
                                position.X = targetLandingPosition.X;
                                LandingAttempt();
                            }
                            break;
                        case Direction.down:
                            if (position.Y + horizontalAirSpeed <= targetLandingPosition.Y)
                            {
                                position.Y += horizontalAirSpeed;
                            }
                            else
                            {
                                position.Y = targetLandingPosition.Y;
                                LandingAttempt();
                            }
                            break;
                    }
                    break;
                case GrabbingState.grabbed:
                    break;
                case GrabbingState.landing:
                    if (z + verticalAirSpeed <= 0)
                    {
                        z = 0;
                        AfterLanding();
                    }
                    else
                    {
                        z += verticalAirSpeed;
                    }
                    break;
                case GrabbingState.grabbing:
                    KeepGrabbing();
                    break;
                case GrabbingState.nothing:
                    break;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {            
            base.Draw(gameTime);
        }
    }
}
