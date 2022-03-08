using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BomberBro
{
    public class Drawable : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public bool checkForCollisions = true;
        public bool solid = true;
        public bool pausable = true;

        public float rotate = 0; 

        public static Rectangle defaultCollisionMask = new Rectangle(0, 0, 32, 32);

        public bool boundToGlobalOrigin = true;
        
        public const int DEFAULT_GRID = 32;

        public Color blendColor = Color.White;

        public DrawableRoom room;

        public float z = 0;

        public enum Direction
        {
            right = 0,
            up = 90,
            left = 180,
            down = 270
        }

        public Vector2 position = new Vector2(0, 0);
        public bool gonnaDie = false;
        public Vector2 previousPosition = new Vector2(0,0);
        protected Texture2D textureImage;
        protected float scale = 1;
        protected Vector2 origin = new Vector2(0, 0);
        protected Point frameSize = new Point(32, 32);
        protected Point sheetSize = new Point(1,1);
        protected Point currentFrame = new Point(0, 0);
        protected Rectangle collisionMask = new Rectangle(0, 0, 32, 32);
        // public static SpriteBatch spriteBatch;

        public float depth = 0;

        public void StepBack()
        {
            this.position.X = this.previousPosition.X;
            this.position.Y = this.previousPosition.Y;
        }

        public void alignToDefaultGrid()
        {
            this.position = getAlignedPosition();
            /* this.position.X = alignedPosition.X;
            this.position.Y = alignedPosition.Y;
            // setPosition(getAlignedPosition()); */
        }

        public Vector2 getAlignedPosition()
        {
            return getAlignedPosition(this.position);
        }

        public static Vector2 getAlignedPosition(Vector2 position)
        {
            return new Vector2((float)(Math.Round(position.X / DEFAULT_GRID) * DEFAULT_GRID), (float)(Math.Round(position.Y / DEFAULT_GRID) * DEFAULT_GRID));
        }

        /* public Vector2 getLastPosition()
        {
            return this.lastPosition;
        }

        public void setLastPosition(Vector2 lastPos)
        {
            this.lastPosition = lastPos;
        }

        public Vector2 getPosition()
        {
            return this.position;
        }

        public void setPosition(Vector2 Pos)
        {
            this.lastPosition = this.position;
            this.position = Pos;
        }

        public void setX(float pos)
        {
            this.lastPosition.X = this.position.X;
            this.position.X = pos;
        }

        public float getX()
        {
            return (this.position.X);
        }

        public void setY(float pos)
        {
            this.lastPosition.Y = this.position.Y;
            this.position.Y = pos;
        }

        public float getY()
        {
            return (this.position.Y);
        } */

        public Drawable(DrawableRoom room):base(room.Game)
        {
            this.room = room;
            LoadTextureAndAllThoseStuff();
            room.drawable.Add(this);
            // game.Components.Add(this);
        }

        public void StepTowards(Vector2 newPosition, float speed)
        {
            float deltaX = newPosition.X - position.X;
            float deltaY = newPosition.Y - position.Y;
            float delta  = (float)Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));
            if (delta != 0)
            {
                float speedX = (deltaX / delta) * speed;
                float speedY = (deltaY / delta) * speed;
                // float speedY = (deltaY/deltaX) * speedX;
                if (speed >= delta)
                {
                    position.X = newPosition.X;
                    position.Y = newPosition.Y;
                    // setPosition(newPosition);
                }
                else
                {
                    position.X += speedX;
                    position.Y += speedY;
                    // setX(getX() + speedX);
                    // setY(getY() + speedY);
                }
            }

            if (float.IsNaN(position.X) || float.IsNaN(position.Y))
            {
                Console.WriteLine("Bug Alert");
            }
        }

        public Rectangle previousCollisionRect
        {
            get
            {
                return new Rectangle(
                (int)previousPosition.X + collisionMask.X,
                (int)previousPosition.Y + collisionMask.Y,
                collisionMask.Width,
                collisionMask.Height);
            }
        }

        public static Rectangle collisionRectAtPosition(Vector2 position, Rectangle collisionMask)
        {
            return new Rectangle(
            (int)position.X + collisionMask.X,
            (int)position.Y + collisionMask.Y,
            collisionMask.Width,
            collisionMask.Height);
        }

        public Rectangle collisionRectAtPosition(Vector2 position)
        {
            return new Rectangle(
            (int)position.X + collisionMask.X,
            (int)position.Y + collisionMask.Y,
            collisionMask.Width,
            collisionMask.Height);
        }

        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                (int)position.X + collisionMask.X,
                (int)position.Y + collisionMask.Y,
                collisionMask.Width,
                collisionMask.Height);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 drawPosition = position;
            if (boundToGlobalOrigin)
            {
                drawPosition += room.globalOrigin;
            }
            room.spriteBatch.Draw(textureImage,
            new Vector2(drawPosition.X, drawPosition.Y - z), // position,
            new Rectangle(currentFrame.X * frameSize.X,
            currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y),
            blendColor, rotate, origin,
            scale, SpriteEffects.None, depth);
            base.Draw(gameTime);
        }


        public virtual void Destroy() // Do NOT call this during a foreach
        {
            room.drawable.Remove(this);
        }

        public override void Update(GameTime gameTime)
        {
            // this.lastPosition = this.position;
            // UpdateDrawableBackUp();
            CallCollisionEvents();
            AfterCollisionEvents();

            depth = position.Y / 2000;

            if (!gonnaDie)
            {
                base.Update(gameTime);
            }
        }

        public virtual void AfterCollisionEvents() { }

        public Drawable instanceMeetingPlacedAt<T>(Vector2 position)
        {
            // Drawable.UpdateDrawableBackUp();
            try
            {
                foreach (Drawable other in room.drawableBackUp)
                {
                    Type type = other.GetType();
                    if (this != other && (type == typeof(T) || type.IsSubclassOf(typeof(T))))
                    {
                        if (collisionRectAtPosition(position).Intersects(other.collisionRect))
                        {
                            return other;
                        }
                    }
                }
                return null;
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Exception at Drawable's instanceMeetingPlacedAt<T>(Vector2 position)");
                return null;
            }
        }

        public Drawable instanceMeeting<T>()
        {
            return instanceMeetingPlacedAt<T>(this.position);
            // return instanceMeetingPlacedAt<T>(getPosition());
        }

        public Drawable instanceMeetingPlacedAtPreviousPosition<T>()
        {
            return instanceMeetingPlacedAt<T>(previousPosition);
            // return instanceMeetingPlacedAt<T>(getLastPosition());
        }

        public bool instanceMeetingPlacedAt(Vector2 position, Drawable drawable)
        {
            return collisionRectAtPosition(position).Intersects(drawable.collisionRect);
        }

        public bool instanceMeeting(Drawable drawable)
        {
            return instanceMeetingPlacedAt(position, drawable);
            // return instanceMeetingPlacedAt(getPosition(), drawable);
        }

        public bool instanceMeetingPlacedAtPreviousPosition(Drawable drawable)
        {
            return instanceMeetingPlacedAt(previousPosition, drawable);
            // return instanceMeetingPlacedAt(getLastPosition(), drawable);
        }

        public virtual void CallCollisionEvents()
        {
            if (checkForCollisions)
            {
                try
                {
                    foreach (Drawable other in room.drawableBackUp)
                    {
                        if (this != other && other.solid)
                        {
                            if (collisionRect.Intersects(other.collisionRect))
                            {
                                CollisionEvent(other);
                                // other.CollisionEvent(this);
                            }
                        }
                    }
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Exception at Drawable's CallCollisionEvents()");
                }
            }
        }

        public virtual void CollisionEvent(Drawable other)
        {

        }

        public virtual void LoadTextureAndAllThoseStuff()
        {
            /* origin = new Vector2(0, 16);
            collisionMask = new Rectangle(0, 16, 32, 32);
            frameSize = new Point(32, 40); */
        }
   }
}