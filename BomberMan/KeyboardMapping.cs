using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace BomberBro
{
    public class KeyboardMapping
    {
        public Keys up, down, left, right, bomb, linebomb, detonate, stop;

        public KeyboardMapping() {}

        public KeyboardMapping(Keys up, Keys down, Keys left, Keys right, Keys bomb, Keys linebomb, Keys detonate, Keys stop)
        {
            this.up = up;
            this.down = down;
            this.left = left;
            this.right = right;
            this.bomb = bomb;
            this.linebomb = linebomb;
            this.detonate = detonate;
            this.stop = stop;
        }

        public Keys getUp()
        {
            return up;
        }

        public void setUp(Keys up)
        {
            this.up = up;
        }

        public Keys getDown()
        {
            return down;
        }

        public void setDown(Keys down)
        {
            this.down = down;
        }

        public Keys getLeft()
        {
            return left;
        }

        public void setLeft(Keys left)
        {
            this.left = left;
        }

        public Keys getRight()
        {
            return right;
        }

        public void setRight(Keys right)
        {
            this.right = right;
        }

        public Keys getBomb()
        {
            return bomb;
        }

        public void setBomb(Keys bomb)
        {
            this.bomb = bomb;
        }

        public Keys getLinebomb()
        {
            return linebomb;
        }

        public void setLinebomb(Keys linebomb)
        {
            this.linebomb = linebomb;
        }

        public Keys getDetonate()
        {
            return detonate;
        }

        public void setDetonate(Keys detonate)
        {
            this.detonate = detonate;
        }

        public Keys getStop()
        {
            return stop;
        }

        public void setStop(Keys stop)
        {
            this.stop = stop;
        }



    }
}
