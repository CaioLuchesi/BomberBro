using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using DPSF;
//using DPSF.ParticleSystems;

namespace BomberBro
{
    class Fireball : Fire
    {
        int firepower;
        protected bool gonnaDieLater = false;
        // protected Player owner;
        const int firespeed = 20;
        protected Vector2 targetPosition;

        public Direction direction;

        public Fireball(DrawableRoom room, Player owner, Revenger revenger, int firepower, Direction direction, Vector2 position)
            : base(room, owner, revenger)
        {
            this.position = new Vector2((float)(Math.Round(position.X / DEFAULT_GRID) * DEFAULT_GRID), (float)(Math.Round(position.Y / DEFAULT_GRID) * DEFAULT_GRID));
            this.firepower = firepower;
            this.direction = direction;
            this.collisionMask = new Rectangle(2, 2, 28, 28);
            targetPosition = position;
            switch (direction)
            {
                case Direction.right:
                    targetPosition.X = position.X + firepower * DEFAULT_GRID;
                    break;
                case Direction.up:
                    targetPosition.Y = position.Y - firepower * DEFAULT_GRID;
                    origin = new Vector2(0, 0);
                    this.rotate = 300; 
                    break;
                case Direction.left:
                    targetPosition.X = position.X - firepower * DEFAULT_GRID;
                    origin = new Vector2(0, 16);
                    this.rotate = 600; 
                    break;
                case Direction.down:
                    targetPosition.Y = position.Y + firepower * DEFAULT_GRID;
                    origin = new Vector2(0, 32);
                    this.rotate = 900;
                    break;
            }
            /* try
            {
                mcAnimatedSpriteParticleSystem = new AnimatedSpriteParticleSystem(this.Game);
                mcAnimatedSpriteParticleSystem.AutoInitialize(this.Game.GraphicsDevice, this.Game.Content);
                mcAnimatedSpriteParticleSystem.MousePosition = new Vector3(200, 200, 200);
            }
            catch (InvalidOperationException)
            {
                // DoNothing();
            } */
        }

        protected override void UnloadContent()
        {
            // mcAnimatedSpriteParticleSystem.Destroy();
            base.UnloadContent();
        }

        public override void LoadTextureAndAllThoseStuff()
        {
            origin = new Vector2(0,16);
            collisionMask = new Rectangle(0, 0, 32, 32);
            frameSize = new Point(32, 32);
        }

        public override void CollisionEvent(Drawable other)
        {
            Type type = other.GetType();
            if (type == typeof(Wall))
            {
                // this.position = Drawable.getAlignedPosition(this.position);
                gonnaDie = true;
            }
            else
            {
                if (type == typeof(BreakableWall))
                {
                    other.gonnaDie = true;
                    this.targetPosition = other.getAlignedPosition();
                }
            }
            base.CollisionEvent(other);
        }

        public override void Update(GameTime gameTime)
        {
            /* if (gonnaDieLater)
            {
                gonnaDie = true;
            } */

            ((BomberBroGame)(this.room.Game)).mcAnimatedSpriteParticleSystem.EmitAt(gameTime, new Vector3(position.X + room.globalOrigin.X + DEFAULT_GRID / 2, position.Y + room.globalOrigin.Y + DEFAULT_GRID / 2 - 10, 0));

            switch (direction)
            {
                case Direction.right:
                    if (this.position.X + firespeed >= targetPosition.X)
                    {
                        this.position.X = targetPosition.X;
                        gonnaDie = true;
                    }
                    else
                    {
                        if (instanceMeetingPlacedAt<Wall>(new Vector2(position.X + firespeed, position.Y)) != null && instanceMeetingPlacedAt<BreakableWall>(new Vector2(position.X + firespeed, position.Y)) == null)
                        {
                            this.position.X = (float)Math.Ceiling(this.position.X / Drawable.DEFAULT_GRID) * Drawable.DEFAULT_GRID;
                            gonnaDie = true;
                        }
                        else
                        {
                            this.position.X = this.position.X + firespeed;
                        }
                    }
                    break;
                case Direction.up:
                    if (this.position.Y - firespeed <= targetPosition.Y)
                    {
                        this.position.Y = targetPosition.Y;
                        gonnaDie = true;
                    }
                    else
                    {
                        if (instanceMeetingPlacedAt<Wall>(new Vector2(position.X, position.Y - firespeed)) != null && instanceMeetingPlacedAt<BreakableWall>(new Vector2(position.X, position.Y - firespeed)) == null)
                        {
                            this.position.Y = (float)Math.Floor(this.position.Y / Drawable.DEFAULT_GRID) * Drawable.DEFAULT_GRID;
                            gonnaDie = true;
                        }
                        else
                        {
                            this.position.Y = this.position.Y - firespeed;
                        }
                    }
                    break;
                case Direction.left:
                    if (this.position.X - firespeed <= targetPosition.X)
                    {
                        this.position.X = targetPosition.X;
                        gonnaDie = true;
                    }
                    else
                    {
                        if (instanceMeetingPlacedAt<Wall>(new Vector2(position.X - firespeed, position.Y)) != null && instanceMeetingPlacedAt<BreakableWall>(new Vector2(position.X - firespeed, position.Y)) == null)
                        {
                            this.position.X = (float)Math.Floor(this.position.X / Drawable.DEFAULT_GRID) * Drawable.DEFAULT_GRID;
                            gonnaDie = true;
                        }
                        else
                        {
                            this.position.X = this.position.X - firespeed;
                        }
                    }
                    break;
                case Direction.down:
                    if (this.position.Y + firespeed >= targetPosition.Y)
                    {
                        this.position.Y = targetPosition.Y;
                        gonnaDie = true;
                    }
                    else
                    {
                        if (instanceMeetingPlacedAt<Wall>(new Vector2(position.X, position.Y + firespeed)) != null && instanceMeetingPlacedAt<BreakableWall>(new Vector2(position.X, position.Y + firespeed)) == null)
                        {
                            this.position.Y = (float)Math.Ceiling(this.position.Y / Drawable.DEFAULT_GRID) * Drawable.DEFAULT_GRID;
                            gonnaDie = true;
                        }
                        else
                        {
                            this.position.Y = this.position.Y + firespeed;
                        }
                    }
                    break;
            }

            if (gonnaDieLater)
            {
                gonnaDie = true;
            }
            
            ((BomberBroGame)(this.room.Game)).mcAnimatedSpriteParticleSystem.EmitAt(gameTime, new Vector3(position.X + room.globalOrigin.X + DEFAULT_GRID / 2, position.Y + room.globalOrigin.Y + DEFAULT_GRID / 2 - 10, 0));
            new FireTrail(room, owner, revenger, position);

            //origin = new Vector2(0, 16);
            //this.rotate = 0;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        { }
    }
}
