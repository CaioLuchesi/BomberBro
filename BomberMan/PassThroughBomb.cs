﻿using System;
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
    class PassThroughBomb : Bomb
    {
        public int timer = 180;
        public PassThroughBomb(DrawableRoom room, Player owner, int firepower) : base(room, owner, firepower) { }
        public PassThroughBomb(DrawableRoom room, Player owner, int firepower, Vector2 position) : base(room, owner, firepower, position) { }

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
            textureImage = GraphicsLibrary.passThroughBomb;
            base.LoadTextureAndAllThoseStuff();
        }
        public override void Explode() 
        {
            if (this.goRight)
                new PassThroughFireball(this.room, this.owner, this.revenger, firepower, Direction.right, this.position);

            if (this.goUp)
                new PassThroughFireball(this.room, this.owner, this.revenger, firepower, Direction.up, this.position);
            
            if (this.goLeft)
                new PassThroughFireball(this.room, this.owner, this.revenger, firepower, Direction.left, this.position);

            if (this.goDown)
                new PassThroughFireball(this.room, this.owner, this.revenger, firepower, Direction.down, this.position);

            base.Explode();
        }
    }
}