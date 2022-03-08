using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BomberBro
{
    class BattleGroundLibrary
    {
        private Texture2D wall;
        private Texture2D breakableWall;
        private Texture2D background;
        public static Texture2D normalBomb;
        public static Texture2D footBomb;
        public static Texture2D passThroughBomb;
        public static Texture2D fireUp;
        public static Texture2D explode;
        public static Texture2D dangerousBomb;
        public static Texture2D remoteBomb;

        public static Texture2D fireWall;
        public static Texture2D time;
        // Power-ups Begin
        public static Texture2D speedUp;
        public static Texture2D bombUp;
        public static Texture2D bombKick;
        public static Texture2D dangerousBombPowerUp;
        public static Texture2D fullFire;
        public static Texture2D lineBomb;
        public static Texture2D passThroughBombPowerUp;
        public static Texture2D powerGlove;
        public static Texture2D remoteBombPowerUp;
        public static Texture2D skull;


        private const byte DEFAULT = 0;
        private const byte PIRATE  = 1;
        private const byte DESERT  = 2;

       

        public Texture2D Wall
        {
            get{return wall;}
        }
        public Texture2D BreakableWall
        {
            get { return breakableWall; }
        }
        public Texture2D Background
        {
            get { return background; }
        }
        public void LoadContent(ContentManager Content,byte tipo)
        {
            remoteBomb = Content.Load<Texture2D>(@"Images/remoteBomb");
            dangerousBomb = Content.Load<Texture2D>(@"Images/dangerousBomb");
            fireWall = Content.Load<Texture2D>(@"Images/blocodefogocorrigido");
            explode = Content.Load<Texture2D>(@"Images/Explosion");
            time = Content.Load<Texture2D>(@"Images/Timer");
            footBomb = Content.Load<Texture2D>(@"Images/ball_soccer");
            normalBomb = Content.Load<Texture2D>(@"Images/newbomb");
            passThroughBomb = Content.Load<Texture2D>(@"Images/passthroughbomb");
            fireUp = Content.Load<Texture2D>(@"Images/Power-ups/fireUp");
            speedUp = Content.Load<Texture2D>(@"Images/Power-ups/speedUp");
            bombUp = Content.Load<Texture2D>(@"Images/Power-ups/bombUp");
            dangerousBombPowerUp = Content.Load<Texture2D>(@"Images/Power-ups/dangerousBomb");
            bombKick = Content.Load<Texture2D>(@"Images/Power-ups/bombKick");
            fullFire = Content.Load<Texture2D>(@"Images/Power-ups/fullFire");
            lineBomb = Content.Load<Texture2D>(@"Images/Power-ups/lineBomb");
            passThroughBombPowerUp = Content.Load<Texture2D>(@"Images/Power-ups/passThroughBomb");
            powerGlove = Content.Load<Texture2D>(@"Images/Power-ups/powerGlove");
            remoteBombPowerUp = Content.Load<Texture2D>(@"Images/Power-ups/remoteBomb");
            skull = Content.Load<Texture2D>(@"Images/Power-ups/skull");
            if (tipo == DEFAULT)
            {
                wall = Content.Load<Texture2D>(@"Images/blocoindestrutivelcorrigido");
                breakableWall = Content.Load<Texture2D>(@"Images/blocodestrutivelcorrigido");
                background = Content.Load<Texture2D>(@"Images/versão2-grassstage");
            }
            if (tipo == DESERT)
            {
                wall = Content.Load<Texture2D>(@"Images/blocoindestrutivelcorrigido");
                breakableWall = Content.Load<Texture2D>(@"Images/desertoblock");
                background = Content.Load<Texture2D>(@"Images/desertstage2");
            }
            if (tipo == PIRATE)
            {
                wall = Content.Load<Texture2D>(@"Images/blocoindestrutivelpirata");
                breakableWall = Content.Load<Texture2D>(@"Images/pirateblock");
                background = Content.Load<Texture2D>(@"Images/BASE pirata");
            }
        }
    }
}
