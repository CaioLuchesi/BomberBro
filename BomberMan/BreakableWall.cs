using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BomberBro
{
    public class BreakableWall : Wall
    {
        public PowerUp.Kinds? hiddenPowerUp;
        public bool isHidingPowerUp = false;

        public BreakableWall(DrawableRoom room, Texture2D textureImage, Vector2 position, Point frameSize,  
            Point currentFrame, Point sheetSize)
            : base(room, textureImage, position, frameSize, currentFrame, sheetSize) {}

        public BreakableWall(DrawableRoom room, PowerUp.Kinds hiddenPowerUp, Texture2D textureImage, Vector2 position, Point frameSize,
            Point currentFrame, Point sheetSize)
            : base(room, textureImage, position, frameSize, currentFrame, sheetSize) {
            this.hiddenPowerUp = hiddenPowerUp;
            this.isHidingPowerUp = true;
        }

        public override void Destroy()
        {
            if (this.isHidingPowerUp)
            {
                switch (hiddenPowerUp)
                {
                    case PowerUp.Kinds.bombKick:
                        new BombKick(this.room, this.position);
                        break;
                    case PowerUp.Kinds.bombUp:
                        new BombUp(this.room, this.position);
                        break;
                    case PowerUp.Kinds.dangerousBomb:
                        new DangerousBombPowerUp(this.room, this.position);
                        break;
                    case PowerUp.Kinds.fireUp:
                        new FireUp(this.room, this.position);
                        break;
                    case PowerUp.Kinds.fullFire:
                        new FullFire(this.room, this.position);
                        break;
                    case PowerUp.Kinds.lineBomb:
                        new LineBomb(this.room, this.position);
                        break;
                    case PowerUp.Kinds.passThroughBomb:
                        new PassThroughBombPowerUp(this.room, this.position);
                        break;
                    case PowerUp.Kinds.powerGlove:
                        new PowerGlove(this.room, this.position);
                        break;
                    case PowerUp.Kinds.remoteBomb:
                        new RemoteBombPowerUp(this.room, this.position);
                        break;
                    case PowerUp.Kinds.skull:
                        new Skull(this.room, this.position);
                        break;
                    case PowerUp.Kinds.speedUp:
                        new SpeedUp(this.room, this.position);
                        break;
                }
            }
            base.Destroy();
        }
    }
}
