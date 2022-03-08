using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BomberBro
{
    public class Skull : PowerUp
    {
        public enum Curses
        {
            bomberrhea,
            bomblessness,
            caffeine,
            invisibility,
            mirrorMovement,
            teleport,
            tooSlow
        }

        public Curses curse;

        public Skull(DrawableRoom room, Vector2 position)
            : base(room, position)
        {
            switch (new Random().Next(7))
            {
                case 0:
                    this.curse = Curses.bomberrhea;
                    break;
                case 1:
                    this.curse = Curses.bomblessness;
                    break;
                case 2:
                    this.curse = Curses.caffeine;
                    break;
                case 3:
                    this.curse = Curses.invisibility;
                    break;
                case 4:
                    this.curse = Curses.mirrorMovement;
                    break;
                case 5:
                    this.curse = Curses.tooSlow;
                    break;
                case 6:
                    this.curse = Curses.teleport;
                    break;
            }
        }

        public Skull(DrawableRoom room, Vector2 position, Curses curse)
            : base(room, position)
        {
            this.curse = curse;
        }

        public override void LoadTextureAndAllThoseStuff()
        {
            textureImage = GraphicsLibrary.skull;
            origin = new Vector2(0, 16);
            collisionMask = new Rectangle(0, 0, 32, 32);
            frameSize = new Point(32, 32);
        }

        public static void CurseThisPlayer(Player other, Curses curse)
        {
            switch (curse)
            {
                case Curses.bomberrhea:
                    other.cursedState = Curses.bomberrhea;
                    other.skullBackUpData = other.maxBombCount;
                    other.maxBombCount = 1000;
                    break;
                case Curses.bomblessness:
                    other.cursedState = Curses.bomblessness;
                    other.skullBackUpData = other.maxBombCount;
                    other.maxBombCount = 0;
                    break;
                case Curses.caffeine:
                    other.cursedState = Curses.caffeine;
                    other.skullBackUpData = other.speed;
                    other.speed = 5.5f;
                    break;
                case Curses.invisibility:
                    other.cursedState = Curses.invisibility;
                    break;
                case Curses.mirrorMovement:
                    other.cursedState = Curses.mirrorMovement;
                    Keys up = other.input.getUp();
                    other.input.setUp(other.input.getDown());
                    other.input.setDown(up);

                    Keys left = other.input.getLeft();
                    other.input.setLeft(other.input.getRight());
                    other.input.setRight(left);
                    break;
                case Curses.teleport:
                    Player[] possibleVictims = new Player[7];
                    // List<Player> possibleVictims = new List<Player>();
                    int victimCount = 0;
                    foreach (Drawable possibleVictim in other.room.drawableBackUp)
                    {
                        if (possibleVictim != other)
                        {
                            if (possibleVictim.GetType() == typeof(Player))
                            {
                                // possibleVictims.Add((Player)drawable);
                                possibleVictims[victimCount] = (Player)possibleVictim;
                                victimCount++;
                                Console.WriteLine("Possible Victim Found");
                            }
                        }
                    }

                    Console.WriteLine(victimCount + " n00bs found");

                    if (victimCount > 0)
                    {
                        Player victim = possibleVictims[new Random().Next(victimCount)];

                        // Swap other and victim's "position", "z", "grabbingState" and "other" variables
                        other.gonnaSwapPositionsWith = victim;
                    }
                    break;
                case Curses.tooSlow:
                    other.cursedState = Curses.tooSlow;
                    other.skullBackUpData = other.speed;
                    other.speed = 0.5f;
                    break;
            }
        }

        public override void Activate(Player other)
        {
            Skull.CurseThisPlayer(other, this.curse);
            base.Activate(other);
        }
    }
}
