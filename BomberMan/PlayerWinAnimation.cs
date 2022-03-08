using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BomberBro
{
    class PlayerWinAnimation : Drawable
    {
        int index = 0;
        private float timer = 20;
        private const float speedSprite = 20;
        private List<Point> animationSequence;

        public PlayerWinAnimation(DrawableRoom room, Texture2D textureImage, Point frameSize,Vector2 origin, Vector2 position, List<Point> animationSequence):base (room)
        {
            this.textureImage = textureImage;
            this.animationSequence = animationSequence;
            this.position.X = position.X;
            this.position.Y = position.Y;
            // setPosition(position);
            this.solid = false;
            this.checkForCollisions = false;
            this.pausable = false;

            this.scale = 1.3f;
            this.frameSize = frameSize;
            this.origin = origin;
        }

        public override void Update(GameTime gameTime)
        {
            if (timer >= speedSprite)
            {
                currentFrame = animationSequence[index];
                index++;
                if (index >= animationSequence.Count)
                {
                    index = 0;
                }

                timer = 0;
            }
            timer++;

            base.Update(gameTime);
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
            currentFrame.Y * frameSize.Y + 15, frameSize.X, frameSize.Y),
            blendColor, 0, origin,
            scale, SpriteEffects.None, depth);
           
        }
    }
}
