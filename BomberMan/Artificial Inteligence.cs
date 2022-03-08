using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Collections;

namespace BomberBro
{
    public class Artificial_Intelligence : Player
    {


        public Artificial_Intelligence(DrawableRoom room, Character character, Vector2 position, Point frameSize,
        Point currentFrame, Point sheetSize, float speed, KeyboardMapping input)
            : base(room, character, position, frameSize, currentFrame, sheetSize, speed, input)
        {
            this.character = character;
            this.textureImage = character.spritesheet;
            this.position = position; // setPosition(position);
            this.frameSize = frameSize;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.speed = speed;
            this.input = input;
            this.scale = 1.3f;
            this.origin = new Vector2(3, 25);
            this.collisionMask = new Rectangle(7, 7, 18, 18);
        }

    }
}
