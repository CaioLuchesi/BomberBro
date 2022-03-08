using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BomberBro
{
    public class Revenger : Drawable
    {
        private const float SPEED = 3f;
        public float linearPosition;
        public int id;
        public int inertia = 0;
        public int firepower = 2;
        public int maxBombCount = 1;
        public KeyboardMapping input;
        // protected AdvancedKeyboardState keyboardState = null;
        private List<Bomb> bombs = new List<Bomb>();
        
        public Revenger(DrawableRoom room, float linearPosition, int playerID, KeyboardMapping input)
            : base(room)
        {
            this.linearPosition = linearPosition;
            this.id = playerID;
            this.input = input;
            this.z = Grabbable.DEFAULT_GRID;
            this.textureImage = GraphicsLibrary.normalBomb;
        }

        public void AddBombToList(Bomb bomb)
        {
            this.bombs.Add(bomb);
        }

        public void RemoveBombFromList(Bomb bomb)
        {
            this.bombs.Remove(bomb);
        }

        public override void Update(GameTime gameTime)
        {
            // keyboardState = new AdvancedKeyboardState(Keyboard.GetState(), keyboardState);
            Match m = (Match)room;

            if (room.keyboard.Check(input.right))
            {
                if (inertia == 0)
                {
                    if ((float)m.arenaColumnCount + (float)m.arenaRowCount / 2f <= linearPosition / DEFAULT_GRID && linearPosition / DEFAULT_GRID < ((float)m.arenaColumnCount * 2 + m.arenaRowCount * 1.5))
                        inertia = -1;
                    else
                        inertia = 1;
                }
            }
            else
                if (room.keyboard.Check(input.up))
                {
                    if (inertia == 0)
                    {
                        if ((float)m.arenaColumnCount / 2f <= linearPosition / DEFAULT_GRID && linearPosition / DEFAULT_GRID < ((float)m.arenaColumnCount * 1.5 + (float)m.arenaRowCount))
                            inertia = -1;
                        else
                            inertia = 1;
                    }
                }
                else
                    if (room.keyboard.Check(input.left))
                    {
                        if (inertia == 0)
                        {
                            if ((float)m.arenaColumnCount + (float)m.arenaRowCount / 2f <= linearPosition / DEFAULT_GRID && linearPosition / DEFAULT_GRID < ((float)m.arenaColumnCount * 2 + m.arenaRowCount * 1.5))
                                inertia = 1;
                            else
                                inertia = -1;
                        }
                    }
                    else
                        if (room.keyboard.Check(input.down))
                        {
                            if (inertia == 0)
                            {
                                if ((float)m.arenaColumnCount / 2f <= linearPosition / DEFAULT_GRID && linearPosition / DEFAULT_GRID < ((float)m.arenaColumnCount * 1.5 + (float)m.arenaRowCount))
                                    inertia = 1;
                                else
                                    inertia = -1;
                            }
                        }

            if (room.keyboard.CheckReleased(input.right) || room.keyboard.CheckReleased(input.up) || room.keyboard.CheckReleased(input.left) || room.keyboard.CheckReleased(input.down))
            {
                inertia = 0;
            }

            linearPosition += inertia * SPEED;

            while (linearPosition < 0)
            {
                linearPosition += (2 * m.arenaColumnCount + 2 * m.arenaRowCount) * DEFAULT_GRID;
            }

            while (linearPosition >= (2 * m.arenaColumnCount + 2 * m.arenaRowCount) * DEFAULT_GRID)
            {
                linearPosition -= (2 * m.arenaColumnCount + 2 * m.arenaRowCount) * DEFAULT_GRID;
            }

            if (0 <= linearPosition && linearPosition < m.arenaColumnCount * DEFAULT_GRID)
            {
                position.X = m.arenaLeftmostColumn * DEFAULT_GRID + linearPosition - 16;
                position.Y = (m.arenaTopmostRow - 1) * DEFAULT_GRID;
            }
            else
                if (m.arenaColumnCount * DEFAULT_GRID <= linearPosition && linearPosition < (m.arenaColumnCount + m.arenaRowCount) * DEFAULT_GRID)
                {
                    position.X = (m.arenaRightmostColumn + 1) * DEFAULT_GRID;
                    position.Y = m.arenaTopmostRow * DEFAULT_GRID - m.arenaColumnCount * DEFAULT_GRID + linearPosition - 16;
                }
                else
                    if ((m.arenaColumnCount + m.arenaRowCount) * DEFAULT_GRID <= linearPosition && linearPosition < (2 * m.arenaColumnCount + m.arenaRowCount) * DEFAULT_GRID)
                    {
                        position.X = (m.arenaRightmostColumn + 1) * DEFAULT_GRID + (m.arenaColumnCount + m.arenaRowCount) * DEFAULT_GRID - linearPosition - 16;
                        position.Y = (m.arenaBottommostRow + 1) * DEFAULT_GRID;
                    }
                    else
                        if ((2 * m.arenaColumnCount + m.arenaRowCount) * DEFAULT_GRID <= linearPosition && linearPosition < (2 * m.arenaColumnCount + 2 * m.arenaRowCount) * DEFAULT_GRID)
                        {
                            position.X = (m.arenaLeftmostColumn - 1) * DEFAULT_GRID;
                            position.Y = (m.arenaBottommostRow + 1) * DEFAULT_GRID + (2 * m.arenaColumnCount + m.arenaRowCount) * DEFAULT_GRID - linearPosition - 16;
                        }

            if (room.keyboard.CheckPressed(input.bomb) && bombs.Count < maxBombCount)
            {
                Direction facingDirection;
                Vector2 bombPosition = position;
                if (0 <= linearPosition && linearPosition < m.arenaColumnCount * DEFAULT_GRID) // if (linearPosition < m.arenaColumnCount * DEFAULT_GRID)
                {
                    facingDirection = Direction.down;
                    bombPosition.Y += DEFAULT_GRID;
                }
                else
                    if (m.arenaColumnCount * DEFAULT_GRID <= linearPosition && linearPosition < (m.arenaColumnCount + m.arenaRowCount) * DEFAULT_GRID) // if (linearPosition < (m.arenaColumnCount + m.arenaRowCount) * DEFAULT_GRID)
                    {
                        facingDirection = Direction.left;
                        bombPosition.X -= DEFAULT_GRID;
                    }
                    else
                        if ((m.arenaColumnCount + m.arenaRowCount) * DEFAULT_GRID <= linearPosition && linearPosition < (2 * m.arenaColumnCount + m.arenaRowCount) * DEFAULT_GRID) // if (linearPosition < (2 * m.arenaColumnCount + m.arenaRowCount) * DEFAULT_GRID)
                        {
                            facingDirection = Direction.up;
                            bombPosition.Y -= DEFAULT_GRID;
                        }
                        else
                            // if ((2 * m.arenaColumnCount + m.arenaRowCount) * DEFAULT_GRID <= linearPosition && linearPosition < (2 * m.arenaColumnCount + 2 * m.arenaRowCount) * DEFAULT_GRID)
                            {
                                facingDirection = Direction.right;
                                bombPosition.X += DEFAULT_GRID;
                            }
                NormalBomb revengeBomb = new NormalBomb(room, this, bombPosition);
                revengeBomb.z = Grabbable.GRABBED_Z;
                revengeBomb.AfterBeingThrown(2, facingDirection);
            }
            base.Update(gameTime);
        }
    }
}
