using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BomberBro.Core;


namespace BomberBro
{
    /// <summary>
    /// This is a game component thats represents the Instrucions Scene
    /// </summary>
    public class OptionsScene : BomberBro.Core.GameScene
    {
        public OptionsScene(Game game, Texture2D textureBack, Texture2D textureFront)
            : base(game)
        {
            Components.Add(new BomberBro.Core.ImageComponent(game, textureBack, 
                ImageComponent.DrawMode.Stretch));
            Components.Add(new BomberBro.Core.ImageComponent(game, textureFront, 
                ImageComponent.DrawMode.Center));
        } 
    }
}