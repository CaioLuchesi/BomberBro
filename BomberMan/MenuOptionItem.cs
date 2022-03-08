using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BomberBro
{
    public class MenuOptionItem : Drawable
    {
        private MenuMode menu;
        public enum FontSize
        {
            normal,
            medium,
            large
        }

        public struct Option
        {
            public string description;
            public Color textColor;
            public Color borderColor;
            public string Description
            {
                set
                {
                    description = value;
                }
            }
            public Option(string description)
            {
                this.description = description;
                this.textColor = Color.White;
                this.borderColor = Color.Black;
            }

            public Option(string description, Color textColor)
            {
                this.description = description;
                this.textColor = textColor;
                this.borderColor = Color.Black;
            }

            public Option(string description, Color textColor, Color borderColor)
            {
                this.description = description;
                this.textColor = textColor;
                this.borderColor = borderColor;
            }

        }

        public int horizontalDistance;
        public string description = "";
        public List<Option> options;
        public int selectedIndex = 0;
        private FontSize size;

        public MenuOptionItem(MenuMode menu, int selectedIndex, Vector2 position, int horizontalDistance, string description, FontSize size, params Option[] options)
            : base(menu)
        {
            this.menu = menu;
            this.selectedIndex = selectedIndex;
            this.position = position;
            this.horizontalDistance = horizontalDistance;
            this.description = description;
            this.size = size;
            this.options = new List<Option>(options);
        }

        public void Left()
        {
            selectedIndex--;
            if (selectedIndex < 0)
            {
                selectedIndex += options.Count;
            }
        }

        public void Right()
        {
            selectedIndex++;
            if (selectedIndex >= options.Count)
            {
                selectedIndex -= options.Count;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (options.Count > 0)
            {
                Option selectedOption = options[selectedIndex];
                if (size == FontSize.normal)
                {
                    room.DrawBorderedString(menu.arialRounded, selectedOption.description, position + new Vector2(horizontalDistance, 10), 16, selectedOption.textColor, selectedOption.borderColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f, 0.9999f);
                }
                else
                    if (size == FontSize.medium)
                    {
                        room.DrawBorderedString(menu.arialRoundedBig, selectedOption.description, position + new Vector2(horizontalDistance, 5), 16, selectedOption.textColor, selectedOption.borderColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f, 0.9999f);
                    }
                    else
                        if (size == FontSize.large)
                        {
                            room.DrawBorderedString(menu.arialRoundedBigger, selectedOption.description, position + new Vector2(horizontalDistance, 0), 16, selectedOption.textColor, selectedOption.borderColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f, 0.9999f);
                        }
            }
            room.DrawBorderedString(menu.arialRoundedBigger, description, position, 16, Color.Orange, Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f, 0.9999f);
            
            // base.Draw(gameTime);
        }
    }
}
