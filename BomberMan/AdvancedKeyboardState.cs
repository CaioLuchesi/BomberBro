using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BomberBro
{
    public class AdvancedKeyboardState
    {
        private KeyboardState keyboardState;
        private KeyboardState previousKeyboardState;

        public Keys[] GetPressedKeys()
        {
            List<Keys> currentList = new List<Keys>(keyboardState.GetPressedKeys());
            List<Keys> previous = new List<Keys>(previousKeyboardState.GetPressedKeys());
            List<Keys> intersection = currentList.Intersect<Keys>(previous).ToList<Keys>();



            foreach (Keys keys in intersection)
            {
                //if(!keys.Equals(Keys.Back))
                currentList.Remove(keys);
            }

            Keys[] current = currentList.ToArray();
            /*for (int i = 0; i < intersection.Length; i++)
            {
                for (int ii = 0; ii < current.Length; ii++)
                {
                    if (current[ii] == intersection[i])
                    {
                        if ((ii + 1) < current.Length)
                        {
                            if (current[ii] != current[ii + 1])
                                current[ii] = current[ii + 1];
                            else
                                current[ii] = Keys.None;
                        }
                    }
                }
            }*/
            return current;
        }


        public AdvancedKeyboardState(KeyboardState keyboardState)
        {
            this.keyboardState = keyboardState;
        }

        public AdvancedKeyboardState(KeyboardState keyboardState, AdvancedKeyboardState previousAdvancedKeyboardState)
        {
            this.keyboardState = keyboardState;
            if (previousAdvancedKeyboardState != null)
            {
                this.previousKeyboardState = previousAdvancedKeyboardState.keyboardState;
            }
        }

        public AdvancedKeyboardState(KeyboardState keyboardState, KeyboardState previousKeyboardState)
        {
            this.keyboardState = keyboardState;
            this.previousKeyboardState = previousKeyboardState;
        }

        public bool Check(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }

        public bool CheckReleased(Keys key)
        {
            if (previousKeyboardState != null)
            {
                return previousKeyboardState.IsKeyDown(key) && keyboardState.IsKeyUp(key);
            }
            else
            {
                return false;
            }
        }

        public bool CheckPressed(Keys key)
        {
            if (previousKeyboardState != null)
            {
                return previousKeyboardState.IsKeyUp(key) && keyboardState.IsKeyDown(key);
            }
            else
            {
                return keyboardState.IsKeyDown(key);
            }
        }
    }
}
