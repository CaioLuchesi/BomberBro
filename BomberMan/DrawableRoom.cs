using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BomberBro
{
    public class DrawableRoom : DrawableGameComponent
    {
        public List<Drawable> drawable = new List<Drawable>();
        public List<Drawable> drawableBackUp = new List<Drawable>();
        public AdvancedKeyboardState keyboard = null;
        public Game game;

        public Vector2 globalOrigin = Vector2.Zero;

        public bool paused = false;

        public SpriteBatch spriteBatch;

        public DrawableRoom(Game game) : base(game)
        {
            this.game = game;
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
        }

        // Use this before foreach loops
        public void UpdateDrawableBackUp()
        {
            drawableBackUp = drawable.ToList();
        }

        public override void Update(GameTime gameTime)
        {
            keyboard = new AdvancedKeyboardState(Keyboard.GetState(), keyboard);

            List<Drawable> seekAndDestroy = new List<Drawable>();

            UpdateDrawableBackUp();

            try
            {
                foreach (Drawable d in drawableBackUp)
                {
                    if (!(paused && d.pausable))
                    {
                        d.previousPosition.X = d.position.X;
                        d.previousPosition.Y = d.position.Y;
                        d.Update(gameTime);
                        if (d.gonnaDie)
                        {
                            seekAndDestroy.Add(d);
                            // d.Destroy();
                        }
                    }
                }
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Exception at BattleGround's Update()");
                // DoNothing();
            }

            foreach (Drawable d in seekAndDestroy)
            {
                d.Destroy();
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            UpdateDrawableBackUp();
            foreach (Drawable d in drawableBackUp)
            {
                if (d.Visible)
                {
                    d.Draw(gameTime);
                }
            }
            base.Draw(gameTime);
        }

        public int numberOfInstances<T>()
        {
            int result = 0;
            foreach (Drawable other in drawableBackUp)
            {
                Type type = other.GetType();
                if (type == typeof(T) || type.IsSubclassOf(typeof(T)))
                {
                    result++;
                }
            }
            return result;
        }

        public Drawable instanceMeetingCollisionMaskPlacedAt<T>(Vector2 position, Rectangle collisionMask)
        {
            Rectangle collisionRectangle = Drawable.collisionRectAtPosition(position, collisionMask);
            // Drawable.UpdateDrawableBackUp();
            try
            {
                foreach (Drawable other in drawableBackUp)
                {
                    Type type = other.GetType();
                    if (type == typeof(T) || type.IsSubclassOf(typeof(T)))
                    {
                        if (collisionRectangle.Intersects(other.collisionRect))
                        {
                            return other;
                        }
                    }
                }
                return null;
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Exception at Drawable's instanceMeetingRectangle<T>(Rectangle collisionRectangle)");
                return null;
            }
        }

        public void DrawBorderedString(SpriteFont spriteFont, string text, Vector2 position, uint borderPrecision, Color fillColor, Color borderColor, float rotation, Vector2 origin, float scale, SpriteEffects spriteEffects, float layerDepth, float borderLayerDepth)
        {
            if (borderPrecision > 0)
            {
                double angleDistance = 2 * Math.PI / borderPrecision;
                for (int i = 0; i < borderPrecision; i++)
                {
                    double angle = i * angleDistance;
                    Vector2 distortion = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                    spriteBatch.DrawString(spriteFont, text, position + distortion, borderColor, rotation, origin, scale, spriteEffects, borderLayerDepth);
                }
                spriteBatch.DrawString(spriteFont, text, position, fillColor, rotation, origin, scale, spriteEffects, layerDepth);
            }
            else
            {
                Console.WriteLine("BorderMustBeLargerThanZeroException");
            }
        }
    }
}
