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
    class RemoteBomb : Bomb
    {
        public int timer = -1;
        public RemoteBomb(DrawableRoom room, Player owner, int firepower) : base(room, owner, firepower) { }
        public RemoteBomb(DrawableRoom room, Player owner, int firepower, Vector2 position) : base(room, owner, firepower, position) { }

        public bool canBeGrabbed // OTHERWISE IT BLOWS UP IN YOUR HAND
        {
            get
            {
                return (timer < 0);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (timer >= 0)
            {
                timer -= 1;
            }
            if (timer == 0)
            {
                Explode();
            }
            base.Update(gameTime);
        }

        public override void LoadTextureAndAllThoseStuff()
        {
            base.LoadTextureAndAllThoseStuff();
            textureImage = GraphicsLibrary.remoteBomb;
            frameSize = new Point(32, 32);
            origin = new Vector2(0, 0);
        }

        public override void Explode()
        {
            if (this.goRight)
                new Fireball(this.room, this.owner, this.revenger, firepower, Direction.right, this.position);

            if (this.goUp)
                new Fireball(this.room, this.owner, this.revenger, firepower, Direction.up, this.position);

            if (this.goLeft)
                new Fireball(this.room, this.owner, this.revenger, firepower, Direction.left, this.position);

            if (this.goDown)
                new Fireball(this.room, this.owner, this.revenger, firepower, Direction.down, this.position);

            base.Explode();
        }
    }
}