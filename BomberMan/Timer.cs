using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BomberBro
{
    class Timer : Drawable
    {
        public int minutes = 0, seconds = 0;
        public int steps = 0;
        protected String time = "";
        protected Texture2D timerBack;
        private Point currentFrame1, currentFrame2, currentFrame3, currentFrame4;
        public Timer(DrawableRoom room, int timeLimit, Point frameSize, Vector2 origin, Vector2 position)
            : base(room)
        {
            this.minutes = timeLimit;
            this.timerBack = GraphicsLibrary.timerBG;
            this.textureImage = GraphicsLibrary.time;
            this.position = position;
            this.frameSize = frameSize;
            this.origin = origin;
            currentFrame1 = new Point(0, 0);
            currentFrame2 = new Point(0, 0);
            currentFrame3 = new Point(0, 0);
            currentFrame4 = new Point(0, 0);
            this.checkForCollisions = false;
            this.solid = false;
            this.boundToGlobalOrigin = false;
        }

        public override void Update(GameTime gameTime)
        {
            steps--;
            if (steps < 0)
            {
                seconds--;
                steps += 60;
            }
            if (seconds < 0)
            {
                minutes--;
                seconds += 60;
            }
            if (minutes < 0)
            {
                steps = 0;
                seconds = 0;
                minutes = 0;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            room.spriteBatch.Draw(timerBack,
           new Vector2(position.X - 37, position.Y - 11),
           new Rectangle(0,
           0, timerBack.Width, timerBack.Height),
           Color.White, 0, origin,
           1, SpriteEffects.None, 0.001f);

            room.spriteBatch.Draw(textureImage,
            new Vector2((position.X + frameSize.X + frameSize.X - 27), position.Y),
            new Rectangle(minutes / 10 * frameSize.X,
            currentFrame1.Y * frameSize.Y, frameSize.X, frameSize.Y),
            Color.White, 0, origin,
            1, SpriteEffects.None, position.Y / 2000);

            room.spriteBatch.Draw(textureImage,
            new Vector2((position.X + frameSize.X + frameSize.X - 12), position.Y),
            new Rectangle(minutes % 10 * frameSize.X,
            currentFrame2.Y * frameSize.Y, frameSize.X, frameSize.Y),
            Color.White, 0, origin,
            1, SpriteEffects.None, position.Y / 2000);

            room.spriteBatch.Draw(textureImage,
            new Vector2((position.X + frameSize.X + frameSize.X), position.Y + 2),
            new Rectangle(0 * frameSize.X,
            (currentFrame2.Y + 1 * frameSize.Y) + 1, frameSize.X, frameSize.Y - 4),
            Color.White, 0, origin,
            1, SpriteEffects.None, position.Y / 2000);

            room.spriteBatch.Draw(textureImage,
             new Vector2((position.X + frameSize.X + frameSize.X + 10), position.Y),
            new Rectangle(seconds / 10 * frameSize.X,
            currentFrame3.Y * frameSize.Y, frameSize.X, frameSize.Y),
            Color.White, 0, origin,
            1, SpriteEffects.None, position.Y / 2000);

            room.spriteBatch.Draw(textureImage,
             new Vector2((position.X + frameSize.X + frameSize.X + 25), position.Y),
            new Rectangle(seconds % 10 * frameSize.X,
            currentFrame4.Y * frameSize.Y, frameSize.X - 6, frameSize.Y),
            Color.White, 0, origin,
            1, SpriteEffects.None, position.Y / 2000);
            // base.Draw(gameTime);
        }
    }
}
