using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BomberBro
{
    class PowerUpDestroyAnimation : Drawable
    {
        int dx = 2, dy = 4;
        private float cont = 15;
        private const float speedSprite = 15;

        public PowerUpDestroyAnimation(DrawableRoom room, Texture2D textureImage, Point frameSize, float scale, Vector2 origin, Vector2 position)
            : base(room)
        {
            this.textureImage = textureImage;

            this.position.X = position.X;
            this.position.Y = position.Y;
            // setPosition(position);

            this.scale = 1.3f;
            this.frameSize = frameSize;
            this.origin = origin;
        }

        public override void Update(GameTime gameTime)
        {

            if (cont >= speedSprite)
            {
                if (dx == 7 && dy == 5)
                    this.Destroy();

                currentFrame = new Point(dx, dy);

                if (dx == 5)
                {
                    dy = 5;
                    dx = 5;
                }
                dx++;
                cont = 0;
            }
            cont++;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
