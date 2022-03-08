using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;

namespace BomberBro
{
    class DangerousBomb : Bomb
    {
        public int timer = 180;
        public DangerousBomb(DrawableRoom room, Player owner, int firepower) : base(room, owner, firepower) { }
        public DangerousBomb(DrawableRoom room, Player owner, int firepower, Vector2 position) : base(room, owner, firepower, position) { }

        public override void Update(GameTime gameTime)
        {
            if (this.grabbingState == GrabbingState.nothing)
            {
                timer -= 1;
                if (timer <= 0)
                {
                    Explode();
                }
            }
            base.Update(gameTime);
        }

        public override void LoadTextureAndAllThoseStuff()
        {
            textureImage = GraphicsLibrary.dangerousBomb;
            base.LoadTextureAndAllThoseStuff();
        }

        public override void Explode()
        {
            if (firepower > 4)
            {
                firepower = 4;
            }
            this.alignToDefaultGrid();
            Vector2 startingPosition = new Vector2(position.X - firepower * DEFAULT_GRID, position.Y - firepower * DEFAULT_GRID);
            Vector2 finalPosition = new Vector2(position.X + firepower * DEFAULT_GRID, position.Y + firepower * DEFAULT_GRID);
            Match m = (Match)room;
            if (startingPosition.X < m.arenaLeftmostColumn * DEFAULT_GRID)
            {
                startingPosition.X = 4 * DEFAULT_GRID;
            }

            if (startingPosition.Y < m.arenaTopmostRow * DEFAULT_GRID)
            {
                startingPosition.Y = 4 * DEFAULT_GRID;
            }

            if (finalPosition.X > m.arenaRightmostColumn * DEFAULT_GRID)
            {
                finalPosition.X = 20 * DEFAULT_GRID;
            }

            if (finalPosition.Y > m.arenaBottommostRow * DEFAULT_GRID)
            {
                finalPosition.Y = 14 * DEFAULT_GRID;
            }

            Vector2 v = new Vector2();
            for (v.X = startingPosition.X; v.X <= finalPosition.X; v.X += DEFAULT_GRID)
            {
                for (v.Y = startingPosition.Y; v.Y <= finalPosition.Y; v.Y += DEFAULT_GRID)
                {
                    if (instanceMeetingPlacedAt<Wall>(v) == null || instanceMeetingPlacedAt<BreakableWall>(v) != null)
                    {
                        new FireSquare(this.room, this.owner, this.revenger, v);
                    }
                }
            }
            base.Explode();
        }
    }
}