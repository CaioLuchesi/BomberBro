using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BomberBro
{
    class GraphicsLibrary
    {
        private Texture2D wall;
        private Texture2D breakableWall;
        private Texture2D background;

        public Texture2D[] water = new Texture2D[32];
        // public static Texture2D[] normalBomb = new Texture2D[4];

        public static Texture2D scoreboard;
        public static Texture2D trophy;
        public static Texture2D skullScoreboard;

        public static Texture2D normalBomb;
        public static Texture2D footBomb;
        public static Texture2D passThroughBomb;
        public static Texture2D fireUp;
        public static Texture2D explode;
        public static Texture2D dangerousBomb;
        public static Texture2D remoteBomb;
        
        public static Texture2D winner;
        public static Texture2D drawgame;
        public static Texture2D hurry;
        public static Texture2D ready;
        public static Texture2D fight;
        public static Texture2D timesUp;

        public static Texture2D fireWall;
        public static Texture2D time;
        public static Texture2D separatorTimer;
        public static Texture2D timerBG;
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
        ContentManager Content;
        BattleManager.Arena arena;

        public GraphicsLibrary(ContentManager Content, BattleManager.Arena arena)
        {
            this.Content = Content;
            this.arena = arena;
        }

        public void LoadContent()
        {
            for (int i = 0; i < 32; i++)
            {
                water[i] = Content.Load<Texture2D>(@"Images/Water Tiles/" + i);
            }
            scoreboard = Content.Load<Texture2D>(@"Images/ScoreBoard/ScoreBoard");
            trophy = Content.Load<Texture2D>(@"Images/ScoreBoard/Trophy");
            skullScoreboard = Content.Load<Texture2D>(@"Images/ScoreBoard/skulls");
            timerBG = Content.Load<Texture2D>(@"Images/Timer/clock");
            normalBomb = Content.Load<Texture2D>(@"Images/Bombs/newBomb");
            winner = Content.Load<Texture2D>(@"Images/ScreenMessages/winner");
            drawgame = Content.Load<Texture2D>(@"Images/ScreenMessages/draw");
            timesUp = Content.Load<Texture2D>(@"Images/ScreenMessages/time's up");
            hurry = Content.Load<Texture2D>(@"Images/ScreenMessages/hurry");
            ready = Content.Load<Texture2D>(@"Images/ScreenMessages/ready");
            fight = Content.Load<Texture2D>(@"Images/ScreenMessages/fight");
            remoteBomb = Content.Load<Texture2D>(@"Images/Bombs/remoteBomb");
            dangerousBomb = Content.Load<Texture2D>(@"Images/Bombs/dangerousBomb");
            explode = Content.Load<Texture2D>(@"Images/Fire/FireBall");
            time = Content.Load<Texture2D>(@"Images/Timer/Timer");
            separatorTimer = Content.Load<Texture2D>(@"Images/Timer/separatorTimer");
            passThroughBomb = Content.Load<Texture2D>(@"Images/Bombs/passthroughbomb");
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
            fireWall = Content.Load<Texture2D>(@"Images/Tiles/blocodefogocorrigido");

            if (arena == BattleManager.Arena.pirate)
            {
                wall = Content.Load<Texture2D>(@"Images/Tiles/barril");
                breakableWall = Content.Load<Texture2D>(@"Images/Tiles/redBarrel");
                background = Content.Load<Texture2D>(@"Images/Backgrounds/OnBoat");
            }

            if (arena == BattleManager.Arena.desert)
            {
                wall = Content.Load<Texture2D>(@"Images/Tiles/blocoindestrutivelcorrigido");
                breakableWall = Content.Load<Texture2D>(@"Images/Tiles/desertoblock");
                background = Content.Load<Texture2D>(@"Images/Backgrounds/desertstage2");
            }

            if (arena == BattleManager.Arena.forest)
            {
                wall = Content.Load<Texture2D>(@"Images/Tiles/blocoindestrutivelcorrigido");
                breakableWall = Content.Load<Texture2D>(@"Images/Tiles/blocodestrutivelcorrigido");
                background = Content.Load<Texture2D>(@"Images/Backgrounds/versão2-grassstage");
            }

            if (arena == BattleManager.Arena.excel)
            {
                wall = Content.Load<Texture2D>(@"Images/Tiles/blocodestrutivelexcel");
                breakableWall = Content.Load<Texture2D>(@"Images/Tiles/blocoindestrutivelexcel");
                background = Content.Load<Texture2D>(@"Images/Backgrounds/excel");
            }
        }
    }
}
