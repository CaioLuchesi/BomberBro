using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BomberBro
{
    public class Wall : Drawable
    {
        // Texture2D textureImage;
        // protected Vector2 position;
        // protected Point frameSize;
        // int collisionOffset;
        // protected Point currentFrame;
        // Point sheetSize;
        // protected float speed;

        public Wall(DrawableRoom room, Texture2D textureImage, Vector2 position, Point frameSize,
        Point currentFrame, Point sheetSize):base(room)
        {
            this.textureImage = textureImage;
            this.position.X = position.X; // setX(position.X);
            this.position.Y = position.Y; // setY(position.Y);
            this.frameSize = frameSize;
            // setCollisionOffset(collisionOffset);
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            // this.speed = speed;
            this.collisionMask = new Rectangle(0, 0, 32, 32);
            this.origin = new Vector2(0, 16);
            this.checkForCollisions = false;
        }

        /* public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                (int)position.X + (int)((collisionOffset+5)*1.3f),
                (int)position.Y - (int)((collisionOffset+3)*1.3f),
                frameSize.X - (int)((collisionOffset+6)*1.3f),
                frameSize.Y - (int)((collisionOffset * 12 + 3)*1.3f));
            }
        } */

        /* public void setCollisionOffset(int collision)
        {
            this.collisionOffset = collision;
        }

        public int getCollisionOffset()
        {
            return this.collisionOffset;
        } */


        /* public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureImage,
            position,
            new Rectangle(currentFrame.X * frameSize.X,
            currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y),
            Color.White, 0, Vector2.Zero,
            1f, SpriteEffects.None, position.Y / 2000);


        } */
        
    }
}
