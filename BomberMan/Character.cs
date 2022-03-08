using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BomberBro
{
    public class Character
    {
        public static Character whiteBomber;
        public static Character blackBomber;
        public static Character redBomber;
        public static Character blueBomber;
        public static Character greenBomber;
        public static Character witch;
        public static Character bishop;
        public static Character merchant;
        public static Character hero;
        public static Character ninja;
        public static Character fairy;
        public static Character monk;

        public Texture2D mugshot;
        public Texture2D spritesheet;
        public List<Point> winAnimationSequence;

        public Character(Texture2D spritesheet, Texture2D mugshot, int winAnimationSequenceIndex)
        {
            this.spritesheet = spritesheet;
            this.mugshot = mugshot;
            this.winAnimationSequence = winAnimationSequences[winAnimationSequenceIndex];
        }

        private static readonly Point[] _sequence3 = { new Point(3, 7), new Point(4, 7), new Point(5, 7), new Point(6, 7), new Point(7, 7), new Point(7, 6) };
        private static readonly Point[] _sequence2 = { new Point(3, 7), new Point(4, 7), new Point(5, 7), new Point(6, 7), new Point(7, 7), new Point(6, 7), new Point(5, 7), new Point(4, 7) };
        private static readonly Point[] _sequence1 = { new Point(3, 7), new Point(4, 7), new Point(5, 7), new Point(6, 7) };
        private static readonly Point[] _sequence0 = { new Point(3, 7), new Point(4, 7), new Point(5, 7), new Point(4, 7) };

        public static List<Point>[] winAnimationSequences = 
        {
            new List<Point>(_sequence0), // white, black, red, green, blue, bishop, merchant
            new List<Point>(_sequence1), // fairy
            new List<Point>(_sequence2), // hero, witch
            new List<Point>(_sequence3)  // ninja, monk
        };
    }
}
