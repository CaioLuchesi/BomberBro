using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BomberBro
{
    public class Bomb:Grabbable
    {
        public bool goRight = true;
        public bool goUp = true;
        public bool goLeft = true;
        public bool goDown = true;

        public bool gonnaExplode = false;

        protected Player owner = null;
        protected Revenger revenger = null;
        protected int firepower = 2;
        const int movingSpeed = 6;
        public Vector2 nextPosition
        {
            get
            {
                Vector2 result = new Vector2((float)Math.Round(position.X / DEFAULT_GRID) * DEFAULT_GRID, (float)Math.Round(position.Y / DEFAULT_GRID) * DEFAULT_GRID);
                switch (direction)
                {
                    case Direction.right:
                        result.X += DEFAULT_GRID;
                        break;
                    case Direction.up:
                        result.Y -= DEFAULT_GRID;
                        break;
                    case Direction.left:
                        result.X -= DEFAULT_GRID;
                        break;
                    case Direction.down:
                        result.Y += DEFAULT_GRID;
                        break;
                }
                return result;
            }
        }
        public bool moving = false;
        public Player kicker = null;
        // public Direction direction = Direction.down;

        public enum Kinds
        {
            NormalBomb = 0,
            PassThroughBomb = 1,
            DangerousBomb = 2,
            RemoteBomb = 3
        }

        public override void LoadTextureAndAllThoseStuff()
        {
            origin = new Vector2(0, 16);
            collisionMask = defaultCollisionMask;
            frameSize = new Point(32, 40);
        }

        public Bomb(DrawableRoom room, Revenger revenger, Vector2 position)
            : base(room)
        {
            this.revenger = revenger;
            if (revenger != null)
            {
                this.position = Drawable.getAlignedPosition(position);
                this.revenger.AddBombToList(this);
            }
        }

        public Bomb(DrawableRoom room, Player owner, int firepower)
            : base(room)
        {
            this.owner = owner;
            this.firepower = firepower;
            if (owner != null)
            {
                this.position = owner.getAlignedPosition();
                this.owner.AddBombToList(this);
            }
        }

        public Bomb(DrawableRoom room, Player owner, int firepower, Vector2 position)
            : base(room)
        {
            this.owner = owner;
            this.firepower = firepower;
            this.position = Drawable.getAlignedPosition(position);
            if (owner != null)
            {
                this.owner.AddBombToList(this);
            }
        }

        public virtual void Explode()
        {
            // this.owner.bomb.Remove(this);
            if (owner != null)
            {
                this.owner.RemoveBombFromList(this);
            }
            if (revenger != null)
            {
                this.revenger.RemoveBombFromList(this);
            }
            gonnaDie = true;
            AudioLibrary.explosion.Play(0.8f, 0f, 0f);
        }

        public override void Update(GameTime gameTime)
        {
            if (moving)
            {
                if (instanceMeetingPlacedAt<Wall>(nextPosition) == null && instanceMeetingPlacedAt<Player>(nextPosition) == null && instanceMeetingPlacedAt<Bomb>(nextPosition) == null)
                {
                    if (room.keyboard.CheckPressed(kicker.input.stop))
                    {
                        moving = false;
                        alignToDefaultGrid();
                    }
                    else
                    {
                        switch (direction)
                        {
                            case Direction.right:
                                position.X += movingSpeed;
                                break;
                            case Direction.up:
                                position.Y -= movingSpeed;
                                break;
                            case Direction.left:
                                position.X -= movingSpeed;
                                break;
                            case Direction.down:
                                position.Y += movingSpeed;
                                break;
                        }
                    }
                }
                else
                {
                    position = new Vector2((float)Math.Round(position.X / DEFAULT_GRID) * DEFAULT_GRID, (float)Math.Round(position.Y / DEFAULT_GRID) * DEFAULT_GRID);
                    moving = false;
                }
            }
            base.Update(gameTime);
        }

        public override void AfterCollisionEvents()
        {
            if (this.gonnaExplode)
            {
                Explode();
            }
            base.AfterCollisionEvents();
        }

        public override void CollisionEvent(Drawable other)
        {
            if (this.grabbingState == GrabbingState.nothing)
            {
                Type type = other.GetType();
                if (type == typeof(Fire) || type.IsSubclassOf(typeof(Fire)))
                {
                    if (type == typeof(Fireball) || type.IsSubclassOf(typeof(Fireball)))
                    {
                        switch (((Fireball)other).direction)
                        {
                            case Direction.right:
                                this.goLeft = false;
                                break;
                            case Direction.up:
                                this.goDown = false;
                                break;
                            case Direction.left:
                                this.goRight = false;
                                break;
                            case Direction.down:
                                this.goUp = false;
                                break;
                        }
                        if (type == typeof(Fireball))
                        {
                            other.Destroy();
                        }
                    }
                    gonnaExplode = true;
                    // Explode();
                }
                base.CollisionEvent(other);
            }
        }
    }
}
