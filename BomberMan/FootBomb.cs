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
    class FootBomb : Bomb
    {
        public FootBomb(DrawableRoom room, Player owner, int firepower) : base(room, owner, firepower) { }
        public FootBomb(DrawableRoom room, Player owner, int firepower, Vector2 position) : base(room, owner, firepower, position) { }

        public override void LoadTextureAndAllThoseStuff()
        {
            textureImage = GraphicsLibrary.footBomb;
            base.LoadTextureAndAllThoseStuff();
        }
        public override void Explode() 
        {
            new Fireball(this.room, this.owner, this.revenger, firepower, Direction.right, this.position);
            new Fireball(this.room, this.owner, this.revenger, firepower, Direction.up, this.position);
            new Fireball(this.room, this.owner, this.revenger, firepower, Direction.left, this.position);
            new Fireball(this.room, this.owner, this.revenger, firepower, Direction.down, this.position);
            base.Explode();
        }
    }
}