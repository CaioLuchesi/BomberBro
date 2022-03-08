using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BomberBro
{
    public class MenuMode : DrawableRoom
    {
        private byte[] characterAnimationHorizontalSequence =
        {
            0,
            32,
            0,
            64
        };
        int NumberOfPlayers
        {
            get
            {
                int result = 0;
                for (int i = 0; i < 8; i++)
                {
                    if (playerJoinOptionItems[i].selectedIndex != 4)
                    {
                        result++;
                    }
                }
                return result;
            }
        }
        byte animationFrame = 0;
        byte animationTimer = TIMER;
        const int TIMER = 10;
        int selectorPositionX,selectorPositionY;
        // int timerUp = TIMER;
        bool visible = true;
        Keys last_Key = 0;
        private string playerJoinCaption       = "SHIFT: Edit Name        CTRL: Keyboard Mapping";
        private string playerJoinCaptionBackUp = "SHIFT: Edit Name        CTRL: Keyboard Mapping";
        private string CPULevelCaption         = "CTRL: CPU LEVEL";
        public Song backgroundMusic;
        int backgroundOrigin = 0;
        float timer = 0;
        bool isFalse = true;
        bool editingName = false;
        MenuScreen _currentMenuScreen = MenuScreen.pressStart;
        private Texture2D characterSelection, selector, characterSelectTitle, mugshotBorder;

        private Texture2D[,] powerUps = new Texture2D[3, 4];
        private Point selectedPowerUp = new Point(0, 0);
        private int[,] powerUpAmmount = 
        {
            {12, 12, 12, 2},
            {2, 1, 1, 1},
            {2, 1, 1, 0}
        };
        private int TotalPowerUps
        {
            get
            {
                int result = 0;
                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        result += powerUpAmmount[x, y];
                    }
                }
                return result;
            }
        }
        const int MAX_POWERUPS = 90;

        private Vector2[,] characterSelectionPositions = 
        {
            {new Vector2(200, 100), new Vector2(200, 200), new Vector2(200, 300)},
            {new Vector2(300, 100), new Vector2(300, 200), new Vector2(300, 300)},
            {new Vector2(400, 100), new Vector2(400, 200), new Vector2(400, 300)},
            {new Vector2(500, 100), new Vector2(500, 200), new Vector2(500, 300)}
        };
        private Point[,] characterSelectionFramePositions = 
        {
            {new Point(0, 0), new Point(0, 160), new Point(0, 320)},
            {new Point(0, 200), new Point(0, 40), new Point(0, 360)},
            {new Point(0, 80), new Point(0, 120), new Point(0, 400)},
            {new Point(0, 240), new Point(0, 280), new Point(0, 440)}
        };
        private Character[,] characterPositions =
        {
            {Character.whiteBomber, Character.hero, Character.blackBomber},
            {Character.ninja, Character.monk, Character.redBomber},
            {Character.bishop, Character.fairy, Character.blueBomber},
            {Character.merchant, Character.witch, Character.greenBomber}
        };

        private Point[] playerCharacter = 
        {
            new Point(0, 0), new Point(1, 0), new Point(2, 0), new Point(3, 0), new Point(0, 1), new Point(1, 1), new Point(2, 1), new Point(3, 1)
        };
        private int playerIDCharacterSelect = 0;

        MenuScreen CurrentMenuScreen
        {
            get
            {
                return _currentMenuScreen;
            }
            set
            {
                _currentMenuScreen = value;
                selectedOptionItem = 0;
                foreach (MenuOptionItem menuOptionItem in menuOptionsItems)
                {
                    menuOptionItem.Visible = false;
                }
                foreach (MenuOptionItem keyboardMappingOptionItem in keyboardMappingOptionItems)
                {
                    keyboardMappingOptionItem.Visible = false;
                }
                foreach (MenuOptionItem editNameOptionItem in editNameOptionItems)
                {
                    editNameOptionItem.Visible = false;
                }
                foreach (MenuOptionItem CPULevelOptionItem in CPULevelOptionItems)
                {
                    CPULevelOptionItem.Visible = false;
                }
                foreach (MenuOptionItem playerJoinOptionItem in playerJoinOptionItems)
                {
                    playerJoinOptionItem.Visible = false;
                }
                foreach (MenuOptionItem battleOptionsOptionItem in battleOptionsOptionItems)
                {
                    battleOptionsOptionItem.Visible = false;
                }
                foreach (MenuOptionItem previewOptionItem in previewOptionItems)
                {
                    previewOptionItem.Visible = false;
                }
                switch (value)
                {
                    case MenuScreen.menu:
                        foreach (MenuOptionItem menuOptionItem in menuOptionsItems)
                        {
                            menuOptionItem.Visible = true;
                        }
                        break;
                    case MenuScreen.keyboardMapping:
                        foreach (MenuOptionItem keyboardMappingOptionItem in keyboardMappingOptionItems)
                        {
                            keyboardMappingOptionItem.Visible = true;
                        }
                        break;
                    case MenuScreen.CPULevel:
                        foreach (MenuOptionItem CPULevelOptionItem in CPULevelOptionItems)
                        {
                            CPULevelOptionItem.Visible = true;
                        }
                        break;
                    case MenuScreen.playerJoin:
                        foreach (MenuOptionItem playerJoinOptionItem in playerJoinOptionItems)
                        {
                            playerJoinOptionItem.Visible = true;
                        }
                        break;
                    case MenuScreen.battleOptions:
                        foreach (MenuOptionItem battleOptionsOptionItem in battleOptionsOptionItems)
                        {
                            battleOptionsOptionItem.Visible = true;
                        }
                        break;
                    case MenuScreen.characterSelect:
                        playerIDCharacterSelect = -1;
                        do
                        {
                            playerIDCharacterSelect++;
                        }
                        while (playerIDCharacterSelect < 8 && playerJoinOptionItems[playerIDCharacterSelect].selectedIndex == 4);
                        selectorPositionX = playerCharacter[playerIDCharacterSelect].X;
                        selectorPositionY = playerCharacter[playerIDCharacterSelect].Y;
                        break;
                    case MenuScreen.preview:
                        foreach (MenuOptionItem previewOptionItem in previewOptionItems)
                        {
                            previewOptionItem.Visible = true;
                        }
                        break;
                }
            }
        }

        private void DrawSelectedItemIndicator(Vector2 initialPosition, int verticalDistance)
        {
            Vector2 delta = new Vector2(0, verticalDistance);
            spriteBatch.Draw(selectedItemIndicator, initialPosition + delta * selectedOptionItem, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        }

        Texture2D background, logo, whiteBomber, enterInput, border, stageSelect, selectedItemIndicator, whitePixel;
        List<Level> levelSelect = new List<Level>();
        float MAX_SCALE = 0.7f;
        float MIN_SCALE = 0.2f;
        int selectedLevel = 0;
        const double ROTATION_SPEED = Math.PI / 64;
        public SpriteFont arialRounded;
        public SpriteFont arialRoundedBig;
        public SpriteFont arialRoundedBigger;

        private int selectedOptionItem = 0;

        private List<MenuOptionItem> CurrentMenuScreenOptionItems
        {
            get
            {
                switch (CurrentMenuScreen)
                {
                    case MenuScreen.menu:
                        return menuOptionsItems;
                    case MenuScreen.keyboardMapping:
                        return keyboardMappingOptionItems;
                    case MenuScreen.playerJoin:
                        return playerJoinOptionItems;
                    case MenuScreen.CPULevel:
                        return CPULevelOptionItems;
                    case MenuScreen.battleOptions:
                        return battleOptionsOptionItems;
                    case MenuScreen.preview:
                        return previewOptionItems;
                    default:
                        return null;
                }
            }
        }

        private List<MenuOptionItem> playerJoinOptionItems = new List<MenuOptionItem>();
        private List<MenuOptionItem> CPULevelOptionItems = new List<MenuOptionItem>();
        private List<MenuOptionItem> battleOptionsOptionItems = new List<MenuOptionItem>();
        private List<MenuOptionItem> menuOptionsItems = new List<MenuOptionItem>();
        private List<MenuOptionItem> keyboardMappingOptionItems = new List<MenuOptionItem>();
        private List<MenuOptionItem> editNameOptionItems = new List<MenuOptionItem>();
        private List<MenuOptionItem> previewOptionItems = new List<MenuOptionItem>();
        private KeyboardMapping[] keyboardMapping = new KeyboardMapping[8];
        private string[] name = 
        {
            "PLAYER 1", "PLAYER 2", "PLAYER 3", "PLAYER 4", "PLAYER 5", "PLAYER 6", "PLAYER 7", "PLAYER 8"
        };
        private BattleManager.PlayerData.CPULevel[] CPULevel = new BattleManager.PlayerData.CPULevel[8];
        private List<Character> character = new List<Character>();


        private int selectedPlayer;

        const int X_PROPORTION = 290;
        const int Y_PROPORTION = 75;

        protected enum MenuScreen
        {
            pressStart,
            menu,
            networkPlay,
            options,
            playerJoin,
            CPULevel,
            keyboardMapping,
            battleOptions,
            characterSelect,
            levelSelect,
            preview,
            powerUps
        }

        protected double angleDistance
        {
            get
            {
                return (Math.PI * 2) / levelSelect.Count;
            }
        }

        protected double dynamicAngleOrigin = 0;

        protected double angleOrigin
        {
            get
            {
                return angleDistance * selectedLevel;
            }
        }

        public struct Level
        {
            public Texture2D thumbnail;
            public double angle;
            public string name;
            
            public Level(Texture2D thumbnail, string name)
            {
                this.thumbnail = thumbnail;
                this.name = name;
                this.angle = 0;
            }

            public Level(Texture2D thumbnail, string name, double angle)
            {
                this.thumbnail = thumbnail;
                this.name = name;
                this.angle = angle;
            }
        }

        public void FillRectangle(Rectangle rectangle, Color fillColor, float layerDepth)
        {
            spriteBatch.Draw(whitePixel, rectangle, null, fillColor, 0f, Vector2.Zero, SpriteEffects.None, layerDepth);
        }

        public void InitializeLists()
        {
            playerJoinOptionItems.Add(new MenuOptionItem(this, 0, new Vector2(100,  90), 500, "PLAYER 1", MenuOptionItem.FontSize.large, new MenuOptionItem.Option("HUMAN", Color.Yellow), new MenuOptionItem.Option("EASY", Color.Cyan), new MenuOptionItem.Option("NORMAL", Color.Lime), new MenuOptionItem.Option("HARD", Color.Red), new MenuOptionItem.Option("OFF", Color.Black)));
            playerJoinOptionItems.Add(new MenuOptionItem(this, 2, new Vector2(100, 150), 500, "PLAYER 2", MenuOptionItem.FontSize.large, new MenuOptionItem.Option("HUMAN", Color.Yellow), new MenuOptionItem.Option("EASY", Color.Cyan), new MenuOptionItem.Option("NORMAL", Color.Lime), new MenuOptionItem.Option("HARD", Color.Red), new MenuOptionItem.Option("OFF", Color.Black)));
            playerJoinOptionItems.Add(new MenuOptionItem(this, 2, new Vector2(100, 210), 500, "PLAYER 3", MenuOptionItem.FontSize.large, new MenuOptionItem.Option("HUMAN", Color.Yellow), new MenuOptionItem.Option("EASY", Color.Cyan), new MenuOptionItem.Option("NORMAL", Color.Lime), new MenuOptionItem.Option("HARD", Color.Red), new MenuOptionItem.Option("OFF", Color.Black)));
            playerJoinOptionItems.Add(new MenuOptionItem(this, 2, new Vector2(100, 270), 500, "PLAYER 4", MenuOptionItem.FontSize.large, new MenuOptionItem.Option("HUMAN", Color.Yellow), new MenuOptionItem.Option("EASY", Color.Cyan), new MenuOptionItem.Option("NORMAL", Color.Lime), new MenuOptionItem.Option("HARD", Color.Red), new MenuOptionItem.Option("OFF", Color.Black)));
            playerJoinOptionItems.Add(new MenuOptionItem(this, 2, new Vector2(100, 330), 500, "PLAYER 5", MenuOptionItem.FontSize.large, new MenuOptionItem.Option("HUMAN", Color.Yellow), new MenuOptionItem.Option("EASY", Color.Cyan), new MenuOptionItem.Option("NORMAL", Color.Lime), new MenuOptionItem.Option("HARD", Color.Red), new MenuOptionItem.Option("OFF", Color.Black)));
            playerJoinOptionItems.Add(new MenuOptionItem(this, 2, new Vector2(100, 390), 500, "PLAYER 6", MenuOptionItem.FontSize.large, new MenuOptionItem.Option("HUMAN", Color.Yellow), new MenuOptionItem.Option("EASY", Color.Cyan), new MenuOptionItem.Option("NORMAL", Color.Lime), new MenuOptionItem.Option("HARD", Color.Red), new MenuOptionItem.Option("OFF", Color.Black)));
            playerJoinOptionItems.Add(new MenuOptionItem(this, 2, new Vector2(100, 450), 500, "PLAYER 7", MenuOptionItem.FontSize.large, new MenuOptionItem.Option("HUMAN", Color.Yellow), new MenuOptionItem.Option("EASY", Color.Cyan), new MenuOptionItem.Option("NORMAL", Color.Lime), new MenuOptionItem.Option("HARD", Color.Red), new MenuOptionItem.Option("OFF", Color.Black)));
            playerJoinOptionItems.Add(new MenuOptionItem(this, 2, new Vector2(100, 510), 500, "PLAYER 8", MenuOptionItem.FontSize.large, new MenuOptionItem.Option("HUMAN", Color.Yellow), new MenuOptionItem.Option("EASY", Color.Cyan), new MenuOptionItem.Option("NORMAL", Color.Lime), new MenuOptionItem.Option("HARD", Color.Red), new MenuOptionItem.Option("OFF", Color.Black)));

            battleOptionsOptionItems.Add(new MenuOptionItem(this, 4, new Vector2(100,  90), 500, "NUMBER OF WINS", MenuOptionItem.FontSize.large, new MenuOptionItem.Option("1", Color.Lime), new MenuOptionItem.Option("2", Color.Lime), new MenuOptionItem.Option("3", Color.Lime), new MenuOptionItem.Option("4", Color.Lime), new MenuOptionItem.Option("5", Color.Lime), new MenuOptionItem.Option("6", Color.Lime), new MenuOptionItem.Option("7", Color.Lime), new MenuOptionItem.Option("8", Color.Lime), new MenuOptionItem.Option("9", Color.Lime)));
            battleOptionsOptionItems.Add(new MenuOptionItem(this, 1, new Vector2(100, 150), 500, "TIME", MenuOptionItem.FontSize.large, new MenuOptionItem.Option("2:00", Color.Lime), new MenuOptionItem.Option("3:00", Color.Lime), new MenuOptionItem.Option("4:00", Color.Lime), new MenuOptionItem.Option("5:00", Color.Lime)));
            battleOptionsOptionItems.Add(new MenuOptionItem(this, 1, new Vector2(100, 210), 500, "SUDDEN DEATH", MenuOptionItem.FontSize.large, new MenuOptionItem.Option("OFF", Color.Red), new MenuOptionItem.Option("PARTIAL", Color.Lime), new MenuOptionItem.Option("FULL", Color.Yellow)));
            battleOptionsOptionItems.Add(new MenuOptionItem(this, 0, new Vector2(100, 270), 500, "REVENGE", MenuOptionItem.FontSize.large, new MenuOptionItem.Option("OFF", Color.Red), new MenuOptionItem.Option("ON", Color.Lime), new MenuOptionItem.Option("SUPER", Color.Yellow)));
            battleOptionsOptionItems.Add(new MenuOptionItem(this, 0, new Vector2(100, 330), 500, "TAG TEAMS", MenuOptionItem.FontSize.large, new MenuOptionItem.Option("OFF", Color.Red), new MenuOptionItem.Option("ON", Color.Lime)));
            // battleOptionsOptionItems.Add(new MenuOptionItem(this, 0, new Vector2(100, 390), 500, "SCAVENGER", new MenuOptionItem.Option("OFF", Color.Red), new MenuOptionItem.Option("ON", Color.Lime)));


            menuOptionsItems.Add(new MenuOptionItem(this, 0, new Vector2(300, 190), 500, "LOCAL PLAY", MenuOptionItem.FontSize.large, new MenuOptionItem.Option("", Color.Blue)));
            menuOptionsItems.Add(new MenuOptionItem(this, 0, new Vector2(300, 250), 500, "LAN PLAY", MenuOptionItem.FontSize.large, new MenuOptionItem.Option("", Color.Blue)));
            menuOptionsItems.Add(new MenuOptionItem(this, 0, new Vector2(300, 310), 500, "OPTIONS", MenuOptionItem.FontSize.large, new MenuOptionItem.Option("", Color.Blue)));
            menuOptionsItems.Add(new MenuOptionItem(this, 0, new Vector2(300, 370), 500, "QUIT", MenuOptionItem.FontSize.large, new MenuOptionItem.Option("", Color.Blue)));

            previewOptionItems.Add(new MenuOptionItem(this, 0, new Vector2(200, 190), 500, "CUSTOMIZE POWER-UPS", MenuOptionItem.FontSize.large));
            previewOptionItems.Add(new MenuOptionItem(this, 0, new Vector2(200, 250), 500, "START MATCH", MenuOptionItem.FontSize.large));



            keyboardMappingOptionItems.Add(new MenuOptionItem(this, 0, new Vector2(100, 90), 400, "UP", MenuOptionItem.FontSize.large, new MenuOptionItem.Option("", Color.Blue)));
            keyboardMappingOptionItems.Add(new MenuOptionItem(this, 0, new Vector2(100, 150), 400, "DOWN", MenuOptionItem.FontSize.large, new MenuOptionItem.Option("", Color.Blue)));
            keyboardMappingOptionItems.Add(new MenuOptionItem(this, 0, new Vector2(100, 210), 400, "LEFT", MenuOptionItem.FontSize.large, new MenuOptionItem.Option("", Color.Blue)));
            keyboardMappingOptionItems.Add(new MenuOptionItem(this, 0, new Vector2(100, 270), 400, "RIGHT", MenuOptionItem.FontSize.large, new MenuOptionItem.Option("", Color.Blue)));
            keyboardMappingOptionItems.Add(new MenuOptionItem(this, 0, new Vector2(100, 330), 400, "BOMB", MenuOptionItem.FontSize.large, new MenuOptionItem.Option("", Color.Blue)));
            keyboardMappingOptionItems.Add(new MenuOptionItem(this, 0, new Vector2(100, 390), 400, "LINE BOMB", MenuOptionItem.FontSize.large, new MenuOptionItem.Option("", Color.Blue)));
            keyboardMappingOptionItems.Add(new MenuOptionItem(this, 0, new Vector2(100, 450), 400, "DETONATE BOMB",MenuOptionItem.FontSize.large, new MenuOptionItem.Option("", Color.Blue)));
            keyboardMappingOptionItems.Add(new MenuOptionItem(this, 0, new Vector2(100, 510), 400, "STOP KICKED BOMB",MenuOptionItem.FontSize.large, new MenuOptionItem.Option("", Color.Blue)));

            
            editNameOptionItems.Add(new MenuOptionItem(this, 0, new Vector2(100,  90), 180, "NAME", MenuOptionItem.FontSize.medium, new MenuOptionItem.Option("", Color.Blue)));

            CPULevelOptionItems.Add(new MenuOptionItem(this, 1, new Vector2(100, 90), 500, "CPU LEVEL", MenuOptionItem.FontSize.large, new MenuOptionItem.Option("EASY", Color.Blue), new MenuOptionItem.Option("NORMAL", Color.Lime), new MenuOptionItem.Option("HARD", Color.Red)));
           
            foreach (MenuOptionItem menuOptionItem in menuOptionsItems)
            {
                menuOptionItem.Visible = false;
            }

            foreach (MenuOptionItem keyboardMappingOptionItem in keyboardMappingOptionItems)
            {
                keyboardMappingOptionItem.Visible = false;
            }

            foreach (MenuOptionItem editNameOptionItem in editNameOptionItems)
            {
                editNameOptionItem.Visible = false;
            }

            foreach (MenuOptionItem CPULevelOptionItem in CPULevelOptionItems)
            {
                CPULevelOptionItem.Visible = false;
            }

            foreach (MenuOptionItem playerJoinOptionItem in playerJoinOptionItems)
            {
                playerJoinOptionItem.Visible = false;
            }
            
            foreach (MenuOptionItem battleOptionsOptionItem in battleOptionsOptionItems)
            {
                battleOptionsOptionItem.Visible = false;
            }

            foreach (MenuOptionItem previewOptionItem in previewOptionItems)
            {
                previewOptionItem.Visible = false;
            }
        }

        public MenuMode(Game game):base(game)
        {
            backgroundMusic = game.Content.Load<Song>(@"sons/menu");
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.Volume = 0.3f;
            MediaPlayer.IsRepeating = true;
            background = game.Content.Load<Texture2D>(@"Images/Menu/Menu Background");
            logo = game.Content.Load<Texture2D>(@"Images/Menu/logo");
            whiteBomber = game.Content.Load<Texture2D>(@"Images/Menu/bomberman copy");
            enterInput = game.Content.Load<Texture2D>(@"Images/Menu/pressEnter");
            border = game.Content.Load<Texture2D>(@"Images/Level Select/Border");
            stageSelect = game.Content.Load<Texture2D>(@"Images/Menu/stageSelect");
            arialRounded = game.Content.Load<SpriteFont>(@"Font/ArialRoundedMTBold");
            arialRoundedBig = game.Content.Load<SpriteFont>(@"Font/ArialRoundedMTBoldAndBig");
            arialRoundedBigger = game.Content.Load<SpriteFont>(@"Font/ArialRoundedMTBoldAndBigger");
            selectedItemIndicator = game.Content.Load<Texture2D>(@"Images/Bombs/newbomb");
            whitePixel = game.Content.Load<Texture2D>(@"Images/Menu/WhitePixel");
            levelSelect.Add(new Level(game.Content.Load<Texture2D>(@"Images/Level Select/OnBoat"), "Pirate Fleet"));
            levelSelect.Add(new Level(game.Content.Load<Texture2D>(@"Images/Level Select/Forest small"), "Grasslands"));
            levelSelect.Add(new Level(game.Content.Load<Texture2D>(@"Images/Level Select/Badlands"), "Badlands"));
            levelSelect.Add(new Level(game.Content.Load<Texture2D>(@"Images/Level Select/Bathhouse"), "Bathhouse"));
            levelSelect.Add(new Level(game.Content.Load<Texture2D>(@"Images/Level Select/Ruins"), "Ruins"));
            levelSelect.Add(new Level(game.Content.Load<Texture2D>(@"Images/Level Select/Throne"), "Throne"));
            /* levelSelect.Add(new Level(game.Content.Load<Texture2D>(@"Images/Level Select/House"), "House"));
            levelSelect.Add(new Level(game.Content.Load<Texture2D>(@"Images/Level Select/Sky"), "Sky")); */
            levelSelect.Add(new Level(game.Content.Load<Texture2D>(@"Images/Level Select/Bridge"), "Bridge"));
            levelSelect.Add(new Level(game.Content.Load<Texture2D>(@"Images/Level Select/Ocean"), "Ocean"));

            characterSelection = game.Content.Load<Texture2D>(@"Images/CharacterSelect/Character Select");
            selector = game.Content.Load<Texture2D>(@"Images/CharacterSelect/big selector");
            characterSelectTitle = game.Content.Load<Texture2D>(@"Images/CharacterSelect/character_Select");
            mugshotBorder = game.Content.Load<Texture2D>(@"Images/CharacterSelect/mugshot border");

            powerUps[0, 0] = game.Content.Load<Texture2D>(@"Images/Power-ups Menu/fireUp");
            powerUps[0, 1] = game.Content.Load<Texture2D>(@"Images/Power-ups Menu/bombUp");
            powerUps[0, 2] = game.Content.Load<Texture2D>(@"Images/Power-ups Menu/speedUp");
            powerUps[0, 3] = game.Content.Load<Texture2D>(@"Images/Power-ups Menu/bombKick");

            powerUps[1, 0] = game.Content.Load<Texture2D>(@"Images/Power-ups Menu/fullFire");
            powerUps[1, 1] = game.Content.Load<Texture2D>(@"Images/Power-ups Menu/passThroughBomb");
            powerUps[1, 2] = game.Content.Load<Texture2D>(@"Images/Power-ups Menu/dangerousBomb");
            powerUps[1, 3] = game.Content.Load<Texture2D>(@"Images/Power-ups Menu/remoteBomb");

            powerUps[2, 0] = game.Content.Load<Texture2D>(@"Images/Power-ups Menu/powerGlove");
            powerUps[2, 1] = game.Content.Load<Texture2D>(@"Images/Power-ups Menu/lineBomb");
            powerUps[2, 2] = game.Content.Load<Texture2D>(@"Images/Power-ups Menu/skulls");
            // DONE

            for (int i = 0; i < levelSelect.Count; i++)
            {
                double angle = angleDistance * i - (0.5 * Math.PI);
                levelSelect[i] = new Level(levelSelect[i].thumbnail, levelSelect[i].name, angle);
                Console.WriteLine(angle);
            }
            keyboardMapping[0] = new KeyboardMapping(Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.NumPad1, Keys.NumPad3, Keys.NumPad2, Keys.NumPad6);
            keyboardMapping[1] = new KeyboardMapping(Keys.W, Keys.S, Keys.A, Keys.D, Keys.LeftShift, Keys.LeftControl, Keys.CapsLock, Keys.Tab);
            keyboardMapping[2] = new KeyboardMapping(Keys.I, Keys.K, Keys.J, Keys.L, Keys.RightShift, Keys.RightControl, Keys.Enter, Keys.Back);
            keyboardMapping[3] = new KeyboardMapping(Keys.NumPad8, Keys.NumPad5, Keys.NumPad4, Keys.NumPad6, Keys.NumPad7, Keys.NumPad9, Keys.Add, Keys.Multiply);
            keyboardMapping[4] = new KeyboardMapping(Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.NumPad1, Keys.NumPad3, Keys.NumPad2, Keys.NumPad6);
            keyboardMapping[5] = new KeyboardMapping(Keys.W, Keys.S, Keys.A, Keys.D, Keys.LeftShift, Keys.LeftControl, Keys.CapsLock, Keys.Tab);
            keyboardMapping[6] = new KeyboardMapping(Keys.I, Keys.K, Keys.J, Keys.L, Keys.RightShift, Keys.RightControl, Keys.Enter, Keys.Back);
            keyboardMapping[7] = new KeyboardMapping(Keys.NumPad8, Keys.NumPad5, Keys.NumPad4, Keys.NumPad6, Keys.NumPad7, Keys.NumPad9, Keys.Add, Keys.Multiply);

            
            for (int i = 0; i < 8; i++)
            {
                CPULevel[i] = BattleManager.PlayerData.CPULevel.none;
            }
            InitializeLists();
            game.Services.AddService(typeof(MenuMode), this);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (CurrentMenuScreen != MenuScreen.keyboardMapping && editingName != true && CurrentMenuScreen != MenuScreen.characterSelect && CurrentMenuScreen != MenuScreen.powerUps)
            {
                if (CurrentMenuScreenOptionItems != null)
                {
                    if (keyboard.CheckPressed(Keys.Down))
                    {
                        selectedOptionItem++;
                        if (selectedOptionItem >= CurrentMenuScreenOptionItems.Count)
                        {
                            selectedOptionItem -= CurrentMenuScreenOptionItems.Count;
                        }
                    }
                    if (keyboard.CheckPressed(Keys.Up))
                    {
                        selectedOptionItem--;
                        if (selectedOptionItem < 0)
                        {
                            selectedOptionItem += CurrentMenuScreenOptionItems.Count;
                        }
                    }

                    if (keyboard.CheckPressed(Keys.Right))
                    {
                        CurrentMenuScreenOptionItems[selectedOptionItem].Right();
                    }
                    if (keyboard.CheckPressed(Keys.Left))
                    {
                        CurrentMenuScreenOptionItems[selectedOptionItem].Left();
                    }
                }
            }
            
            switch (CurrentMenuScreen)
            {
                case MenuScreen.pressStart:
                    if (keyboard.CheckPressed(Keys.Enter))
                    {
                        CurrentMenuScreen = MenuScreen.menu;
                    }
                    timer++;
                    if (timer >= 45)
                    {
                        timer -= 45;
                        isFalse = !isFalse;
                    }
                    break;
                case MenuScreen.menu:
                    if (keyboard.CheckPressed(Keys.Enter))
                    {
                        if (selectedOptionItem == 0)
                        {
                            CurrentMenuScreen = MenuScreen.playerJoin;
                        }
                        else
                            if (selectedOptionItem == 1)
                            {
                                CurrentMenuScreen = MenuScreen.networkPlay;
                            }
                            else
                                if (selectedOptionItem == 2)
                                {
                                    CurrentMenuScreen = MenuScreen.options;
                                }
                                else
                                    if (selectedOptionItem == 3)
                                        game.Exit();
                    }
                 
                    if (keyboard.CheckPressed(Keys.Back))
                    {
                        CurrentMenuScreen = MenuScreen.pressStart;
                    }
                    if (keyboard.CheckPressed(Keys.Escape))
                    {
                        CurrentMenuScreen = MenuScreen.pressStart;
                    }
                    break;
                case MenuScreen.playerJoin:
                    
                    if (playerJoinOptionItems[selectedOptionItem].options[playerJoinOptionItems[selectedOptionItem].selectedIndex].description.Equals("CPU"))
                    {
                        playerJoinCaption = CPULevelCaption;
                        if (keyboard.CheckPressed(Keys.RightControl) || keyboard.CheckPressed(Keys.LeftControl))
                        {
                            selectedPlayer = selectedOptionItem;
                            CurrentMenuScreen = MenuScreen.CPULevel;
                            //
                        }
                    }
                    else
                        if (playerJoinOptionItems[selectedOptionItem].options[playerJoinOptionItems[selectedOptionItem].selectedIndex].description.Equals("HUMAN"))
                        {
                            playerJoinCaption = playerJoinCaptionBackUp;
                            if (keyboard.CheckPressed(Keys.RightControl) || keyboard.CheckPressed(Keys.LeftControl))
                            {
                                selectedPlayer = selectedOptionItem;
                                foreach (MenuOptionItem keyboardMappingOptionItem in keyboardMappingOptionItems)
                                {
                                    keyboardMappingOptionItem.options[0] = new MenuOptionItem.Option("", Color.Blue);
                                }
                                CurrentMenuScreen = MenuScreen.keyboardMapping;
                                //
                            }
                            
                            
                        }
                        else
                        {
                            playerJoinCaption = "";
                        }

                    if (editingName == false && (keyboard.CheckPressed(Keys.RightShift) || keyboard.CheckPressed(Keys.LeftShift)))
                    {
                        selectedPlayer = selectedOptionItem;
                        editingName = true;
                        playerJoinOptionItems[selectedOptionItem].description = "";
                        //
                    }

                    if (editingName)
                    {
                        UpdateEditName();
                        
                        if (keyboard.CheckPressed(Keys.Enter))
                        {
                            editingName = false;
                        }
                        if (keyboard.CheckPressed(Keys.Escape))
                        {
                            editingName = false;
                        }
                    }
                    else
                    {
                        if (keyboard.CheckPressed(Keys.Back))
                        {
                            CurrentMenuScreen = MenuScreen.menu;
                        }

                        if (keyboard.CheckPressed(Keys.Enter) && NumberOfPlayers >= 2)
                        {
                            CurrentMenuScreen = MenuScreen.battleOptions;
                        }

                        if (keyboard.CheckPressed(Keys.Escape))
                        {
                            CurrentMenuScreen = MenuScreen.menu;
                        }
                    }
                    break;
                case MenuScreen.keyboardMapping:
                    if (keyboard.CheckPressed(Keys.Enter))
                    {
                        if (selectedOptionItem == keyboardMappingOptionItems.Count - 1)
                            CurrentMenuScreen = MenuScreen.playerJoin;
                        else
                        {
                            selectedOptionItem++;
                            if (selectedOptionItem >= CurrentMenuScreenOptionItems.Count)
                            {
                                selectedOptionItem -= CurrentMenuScreenOptionItems.Count;
                            }
                        }  
                    }
                    if (keyboard.CheckPressed(Keys.Back))
                    {
                        selectedOptionItem--;
                        if (selectedOptionItem < 0)
                        {
                            selectedOptionItem += CurrentMenuScreenOptionItems.Count;
                        }
                    }
                    UpdateKeyboard();
                    break;
                case MenuScreen.CPULevel:
                    if (keyboard.CheckPressed(Keys.Enter))
                    {
                        CPULevel[selectedPlayer] = (BattleManager.PlayerData.CPULevel)CPULevelOptionItems[0].selectedIndex;
                        CurrentMenuScreen = MenuScreen.playerJoin;
                    }
                    
                    // timerUp -= 1;

                    break;
                case MenuScreen.battleOptions:
                    if (keyboard.CheckPressed(Keys.Enter))
                    {
                        CurrentMenuScreen = MenuScreen.levelSelect;
                    }
                    if (keyboard.CheckPressed(Keys.Back))
                    {
                        CurrentMenuScreen = MenuScreen.playerJoin;
                    }

                    if (keyboard.CheckPressed(Keys.Escape))
                    {
                        CurrentMenuScreen = MenuScreen.playerJoin;
                    }
                    break;
                case MenuScreen.levelSelect:
                    if (dynamicAngleOrigin == angleOrigin)
                    {
                        if (keyboard.Check(Keys.Left))
                        {
                            selectedLevel--;
                        }
                        else
                        {
                            if (keyboard.Check(Keys.Right))
                            {
                                selectedLevel++;
                            }
                        }
                        while (selectedLevel >= levelSelect.Count)
                        {
                            selectedLevel -= levelSelect.Count;
                        }
                        while (selectedLevel < 0)
                        {
                            selectedLevel += levelSelect.Count;
                        }
                    }

                    if (dynamicAngleOrigin != angleOrigin)
                    {
                        if (dynamicAngleOrigin - angleOrigin > Math.PI)
                        {
                            dynamicAngleOrigin -= 2 * Math.PI;
                            if (dynamicAngleOrigin + ROTATION_SPEED >= angleOrigin)
                            {
                                dynamicAngleOrigin = angleOrigin;
                            }
                            else
                            {
                                dynamicAngleOrigin += ROTATION_SPEED;
                            }
                        }
                        else
                        {
                            if (angleOrigin - dynamicAngleOrigin > Math.PI)
                            {
                                dynamicAngleOrigin += 2 * Math.PI;
                                if (dynamicAngleOrigin - ROTATION_SPEED <= angleOrigin)
                                {
                                    dynamicAngleOrigin = angleOrigin;
                                }
                                else
                                {
                                    dynamicAngleOrigin -= ROTATION_SPEED;
                                }
                            }
                            else
                            {
                                if (dynamicAngleOrigin - angleOrigin > 0)
                                {
                                    if (dynamicAngleOrigin - ROTATION_SPEED <= angleOrigin)
                                    {
                                        dynamicAngleOrigin = angleOrigin;
                                    }
                                    else
                                    {
                                        dynamicAngleOrigin -= ROTATION_SPEED;
                                    }
                                }
                                else
                                {
                                    // angleOrigin - dynamicAngleOrigin > 0
                                    if (dynamicAngleOrigin + ROTATION_SPEED >= angleOrigin)
                                    {
                                        dynamicAngleOrigin = angleOrigin;
                                    }
                                    else
                                    {
                                        dynamicAngleOrigin += ROTATION_SPEED;
                                    }
                                }
                            }
                        }
                        Console.WriteLine(dynamicAngleOrigin);
                    }
                    while (dynamicAngleOrigin < 0)
                    {
                        dynamicAngleOrigin += Math.PI * 2;
                    }
                    while (dynamicAngleOrigin >= Math.PI * 2)
                    {
                        dynamicAngleOrigin -= Math.PI * 2;
                    }

                    if (keyboard.CheckPressed(Keys.Back))
                    {
                        CurrentMenuScreen = MenuScreen.battleOptions;
                    }
                    if (keyboard.CheckPressed(Keys.Escape))
                    {
                        CurrentMenuScreen = MenuScreen.battleOptions;
                    }
                    if (keyboard.CheckPressed(Keys.Enter))
                    {
                        CurrentMenuScreen = MenuScreen.characterSelect;
                    }
                    break;
                case MenuScreen.characterSelect:
                    if (keyboard.CheckPressed(Keys.Down))
                    {
                        if (selectorPositionY == 2)
                        {
                            selectorPositionY = 0;
                        }
                        else
                        {
                            selectorPositionY += 1;
                        }
                        animationFrame = 0;
                    }
                    if (keyboard.CheckPressed(Keys.Up))
                    {
                        if (selectorPositionY == 0)
                        {
                            selectorPositionY = 2;
                        }
                        else
                        {
                            selectorPositionY -= 1;
                        }
                        animationFrame = 0;
                    }

                    if (keyboard.CheckPressed(Keys.Right))
                    {
                        if (selectorPositionX == 3)
                        {
                            selectorPositionX = 0;
                        }
                        else
                        {
                            selectorPositionX += 1;
                        }
                        animationFrame = 0;
                    }
                    if (keyboard.CheckPressed(Keys.Left))
                    {
                        if (selectorPositionX == 0)
                        {
                            selectorPositionX = 3;
                        }
                        else
                        {
                            selectorPositionX -= 1;
                        }
                        animationFrame = 0;
                    }

                    if (keyboard.CheckPressed(Keys.Back))
                    {
                        CurrentMenuScreen = MenuScreen.levelSelect;
                    }
                    if (keyboard.CheckPressed(Keys.Escape))
                    {
                        CurrentMenuScreen = MenuScreen.levelSelect;
                    }
                    if (keyboard.CheckPressed(Keys.Enter))
                    {
                        playerCharacter[playerIDCharacterSelect] = new Point(selectorPositionX, selectorPositionY);
                        do
                        {
                            playerIDCharacterSelect++;
                        }
                        while (playerIDCharacterSelect < 8 && playerJoinOptionItems[playerIDCharacterSelect].selectedIndex == 4);
                        if (playerIDCharacterSelect >= 8)
                        {
                            CurrentMenuScreen = MenuScreen.preview;
                        }
                        else
                        {
                            selectorPositionX = playerCharacter[playerIDCharacterSelect].X;
                            selectorPositionY = playerCharacter[playerIDCharacterSelect].Y;
                        }
                    }
                    break;
                case MenuScreen.preview:
                    if (keyboard.CheckPressed(Keys.Enter))
                    {
                        if (selectedOptionItem == 0)
                        {
                            CurrentMenuScreen = MenuScreen.powerUps;
                        }
                        else
                        {
                            if (selectedOptionItem == 1)
                            {
                                CreateBattleManager();
                            }
                        }
                    }
                    if (keyboard.CheckPressed(Keys.Back) || keyboard.CheckPressed(Keys.Escape))
                    {
                        CurrentMenuScreen = MenuScreen.characterSelect;
                    }
                    break;
                case MenuScreen.powerUps:
                    if (keyboard.CheckPressed(Keys.Down))
                    {
                        if (selectedPowerUp.Y == 3)
                        {
                            selectedPowerUp.Y = 0;
                        }
                        else
                        {
                            selectedPowerUp.Y += 1;
                        }
                    }
                    if (keyboard.CheckPressed(Keys.Up))
                    {
                        if (selectedPowerUp.Y == 0)
                        {
                            selectedPowerUp.Y = 3;
                        }
                        else
                        {
                            selectedPowerUp.Y -= 1;
                        }
                    }
                    if (keyboard.CheckPressed(Keys.Right))
                    {
                        if (selectedPowerUp.X == 2)
                        {
                            selectedPowerUp.X = 0;
                        }
                        else
                        {
                            selectedPowerUp.X += 1;
                        }
                    }
                    if (keyboard.CheckPressed(Keys.Left))
                    {
                        if (selectedPowerUp.X == 0)
                        {
                            selectedPowerUp.X = 2;
                        }
                        else
                        {
                            selectedPowerUp.X -= 1;
                        }
                    }
                    if (keyboard.CheckPressed(Keys.Back))
                    {
                        if (selectedPowerUp.X == 3 && selectedPowerUp.Y == 4) // DONE
                        {
                            CurrentMenuScreen = MenuScreen.preview;
                        }
                        else
                        {
                            if (powerUpAmmount[selectedPowerUp.X, selectedPowerUp.Y] > 0)
                            {
                                powerUpAmmount[selectedPowerUp.X, selectedPowerUp.Y] -= 1;
                            }
                        }
                    }
                    else
                    {
                        if (keyboard.CheckPressed(Keys.Escape))
                        {
                            CurrentMenuScreen = MenuScreen.preview;
                        }
                        else
                        {
                            if (keyboard.CheckPressed(Keys.Enter))
                            {
                                if (selectedPowerUp.X == 2 && selectedPowerUp.Y == 3) // DONE
                                {
                                    CurrentMenuScreen = MenuScreen.preview;
                                }
                                else
                                {
                                    if (TotalPowerUps < MAX_POWERUPS)
                                    {
                                        powerUpAmmount[selectedPowerUp.X, selectedPowerUp.Y] += 1;
                                    }
                                }
                            }
                        }
                    }
                    break;
            }
        }

        private void UpdateEditName()
        {
            Keys[] pressed_Key = keyboard.GetPressedKeys();

            for (int i = 0; i < pressed_Key.Length; i++)
            {
                if (pressed_Key[i] == Keys.Back && playerJoinOptionItems[selectedOptionItem].description != "")
                {
                    playerJoinOptionItems[selectedOptionItem].description = playerJoinOptionItems[selectedOptionItem].description.Substring(0, playerJoinOptionItems[selectedOptionItem].description.Length - 1);
                    name[selectedPlayer] = playerJoinOptionItems[selectedOptionItem].description.ToString();
                }
                else
                    if (pressed_Key[i] == Keys.Space)
                    {
                        playerJoinOptionItems[selectedOptionItem].description = playerJoinOptionItems[selectedOptionItem].description + " ";
                        name[selectedPlayer] = playerJoinOptionItems[selectedOptionItem].description.ToString();
                    }
                    else
                    {
                        if (playerJoinOptionItems[selectedOptionItem].description.Length != 23)
                        {
                            if (pressed_Key[i] != Keys.Enter && pressed_Key[i] != Keys.Back && pressed_Key[i] != Keys.RightShift && pressed_Key[i] != Keys.LeftShift && pressed_Key[i] != Keys.RightControl && pressed_Key[i] != Keys.LeftControl && pressed_Key[i] != Keys.CapsLock && pressed_Key[i] != Keys.Tab && pressed_Key[i] != Keys.LeftAlt && pressed_Key[i] != Keys.RightAlt && pressed_Key[i] != Keys.LeftWindows && pressed_Key[i] != Keys.RightWindows && pressed_Key[i] != Keys.Escape && pressed_Key[i] != Keys.Home && pressed_Key[i] != Keys.None)
                            {
                                if (pressed_Key[i] == Keys.D0)
                                    playerJoinOptionItems[selectedOptionItem].description = playerJoinOptionItems[selectedOptionItem].description + "0";
                                else
                                    if (pressed_Key[i] == Keys.D1)
                                        playerJoinOptionItems[selectedOptionItem].description = playerJoinOptionItems[selectedOptionItem].description + "1";
                                    else
                                        if (pressed_Key[i] == Keys.D2)
                                            playerJoinOptionItems[selectedOptionItem].description = playerJoinOptionItems[selectedOptionItem].description + "2";
                                        else
                                            if (pressed_Key[i] == Keys.D3)
                                                playerJoinOptionItems[selectedOptionItem].description = playerJoinOptionItems[selectedOptionItem].description + "3";
                                            else
                                                if (pressed_Key[i] == Keys.D4)
                                                    playerJoinOptionItems[selectedOptionItem].description = playerJoinOptionItems[selectedOptionItem].description + "4";
                                                else
                                                    if (pressed_Key[i] == Keys.D5)
                                                        playerJoinOptionItems[selectedOptionItem].description = playerJoinOptionItems[selectedOptionItem].description + "5";
                                                    else
                                                        if (pressed_Key[i] == Keys.D6)
                                                            playerJoinOptionItems[selectedOptionItem].description = playerJoinOptionItems[selectedOptionItem].description + "6";
                                                        else
                                                            if (pressed_Key[i] == Keys.D7)
                                                                playerJoinOptionItems[selectedOptionItem].description = playerJoinOptionItems[selectedOptionItem].description + "7";
                                                            else
                                                                if (pressed_Key[i] == Keys.D8)
                                                                    playerJoinOptionItems[selectedOptionItem].description = playerJoinOptionItems[selectedOptionItem].description + "8";
                                                                else
                                                                    if (pressed_Key[i] == Keys.D9)
                                                                        playerJoinOptionItems[selectedOptionItem].description = playerJoinOptionItems[selectedOptionItem].description + "9";
                                                                    else
                                if (last_Key != Keys.RightShift && last_Key != Keys.LeftShift)
                                {
                                    playerJoinOptionItems[selectedOptionItem].description = playerJoinOptionItems[selectedOptionItem].description + pressed_Key[i].ToString().ToLowerInvariant();
                                   // name[selectedPlayer] = playerJoinOptionItems[selectedOptionItem].description.ToString();
                                }
                                else
                                {
                                    playerJoinOptionItems[selectedOptionItem].description = playerJoinOptionItems[selectedOptionItem].description + pressed_Key[i].ToString().ToUpper();
                                   // name[selectedPlayer] = playerJoinOptionItems[selectedOptionItem].description.ToString();
                                }


                                name[selectedPlayer] = playerJoinOptionItems[selectedOptionItem].description.ToString();
                              
                                
                            }

                        }
                    }

                last_Key = pressed_Key[i];
            }
        }

        private void UpdateKeyboard()
        {
            Keys[] pressed_Key = keyboard.GetPressedKeys();


            for (int i = 0; i < pressed_Key.Length; i++)
            {
                if (pressed_Key[i] != last_Key && pressed_Key[i] != Keys.Enter && pressed_Key[i] != Keys.None)
                {

                    keyboardMappingOptionItems[selectedOptionItem].options[0] = new MenuOptionItem.Option(pressed_Key[i].ToString(), Color.Blue);

                    if (selectedOptionItem == 0)
                        keyboardMapping[selectedPlayer].up = pressed_Key[i];
                    else
                        if (selectedOptionItem == 1)
                            keyboardMapping[selectedPlayer].down = pressed_Key[i];
                        else
                            if (selectedOptionItem == 2)
                                keyboardMapping[selectedPlayer].left = pressed_Key[i];
                            else
                                if (selectedOptionItem == 3)
                                    keyboardMapping[selectedPlayer].right = pressed_Key[i];
                                else
                                    if (selectedOptionItem == 4)
                                        keyboardMapping[selectedPlayer].bomb = pressed_Key[i];
                                    else
                                        if (selectedOptionItem == 5)
                                            keyboardMapping[selectedPlayer].linebomb = pressed_Key[i];
                                        else
                                            if (selectedOptionItem == 6)
                                                keyboardMapping[selectedPlayer].detonate = pressed_Key[i];
                                            else
                                                if (selectedOptionItem == 7)
                                                    keyboardMapping[selectedPlayer].stop = pressed_Key[i];

                }
                last_Key = pressed_Key[i];
            }
        }

        private void CreateBattleManager()
        {
            List<BattleManager.PlayerData>[] playerData = new List<BattleManager.PlayerData>[8];
            /* playerData[0] = new List<BattleManager.PlayerData>();
            playerData[1] = new List<BattleManager.PlayerData>();
            playerData[2] = new List<BattleManager.PlayerData>();
            playerData[3] = new List<BattleManager.PlayerData>();
            playerData[4] = new List<BattleManager.PlayerData>();
            playerData[5] = new List<BattleManager.PlayerData>();
            playerData[6] = new List<BattleManager.PlayerData>(); */
            // playerData[7] = new List<BattleManager.PlayerData>();

            /*playerData[0].Add(new BattleManager.PlayerData("TalkingLunchBag555", Character.ninja, new KeyboardMapping(Keys.W, Keys.S, Keys.A, Keys.D, Keys.LeftShift, Keys.LeftControl, Keys.LeftAlt, Keys.Space)));
            playerData[1].Add(new BattleManager.PlayerData("YOU-ALL-SUCK", Character.merchant, new KeyboardMapping(Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.NumPad1, Keys.NumPad3, Keys.NumPad2, Keys.NumPad6)));
            playerData[2].Add(new BattleManager.PlayerData("ReallyBadPlayer", Character.bishop, new KeyboardMapping(Keys.I, Keys.K, Keys.J, Keys.L, Keys.RightShift, Keys.RightControl, Keys.Enter, Keys.Back)));
            playerData[3].Add(new BattleManager.PlayerData("HarryPotterFanGirl", Character.witch, new KeyboardMapping(Keys.NumPad8, Keys.NumPad5, Keys.NumPad4, Keys.NumPad6, Keys.NumPad7, Keys.NumPad9, Keys.Add, Keys.Multiply)));
            playerData[4].Add(new BattleManager.PlayerData("WatchMeWin", Character.hero, new KeyboardMapping(Keys.W, Keys.S, Keys.A, Keys.D, Keys.LeftShift, Keys.LeftControl, Keys.LeftAlt, Keys.Space)));
            playerData[5].Add(new BattleManager.PlayerData("xXx_SePhIrOtDaRkS0NiC1997_xXx", Character.monk, new KeyboardMapping(Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.NumPad1, Keys.NumPad3, Keys.NumPad2, Keys.NumPad6)));
            playerData[6].Add(new BattleManager.PlayerData("ObamaBinLaden", Character.whiteBomber, new KeyboardMapping(Keys.I, Keys.K, Keys.J, Keys.L, Keys.RightShift, Keys.RightControl, Keys.Enter, Keys.Back)));
            playerData[7].Add(new BattleManager.PlayerData("A_Freaking_Bus", Character.blueBomber, new KeyboardMapping(Keys.NumPad8, Keys.NumPad5, Keys.NumPad4, Keys.NumPad6, Keys.NumPad7, Keys.NumPad9, Keys.Add, Keys.Multiply)));*/

            /*playerData[0].Add(new BattleManager.PlayerData(name[0], Character.ninja, keyboardMapping[0]));
            playerData[1].Add(new BattleManager.PlayerData(name[1], Character.merchant, keyboardMapping[1]));
            playerData[2].Add(new BattleManager.PlayerData(name[2], Character.witch, keyboardMapping[2]));
            playerData[3].Add(new BattleManager.PlayerData(name[3], Character.hero, keyboardMapping[3]));*/


            int teamIndex = 0;
            int level = 0;
            for (int i = 0; i < 8; i++)
            {
                if (playerJoinOptionItems[i].selectedIndex != 4)
                {
                    playerData[teamIndex] = new List<BattleManager.PlayerData>();
                    switch (playerJoinOptionItems[i].selectedIndex)
                    {
                        case 0:
                            playerData[teamIndex].Add(new BattleManager.PlayerData(name[i], characterPositions[playerCharacter[i].X, playerCharacter[i].Y], keyboardMapping[i], BattleManager.PlayerData.CPULevel.none));
                            break;
                        default:
                            playerData[teamIndex].Add(new BattleManager.PlayerData(name[i], characterPositions[playerCharacter[i].X, playerCharacter[i].Y], new KeyboardMapping(), CPULevel[level++]));
                            break;
                    }
                    teamIndex++;
                }
            }

            BattleManager battleManager;

            this.Enabled = false;
            this.Visible = false;
            battleManager = new BattleManager(game, playerData, new PowerUpManager(powerUpAmmount[0, 0], powerUpAmmount[0, 1], powerUpAmmount[0, 2], powerUpAmmount[1, 1], powerUpAmmount[0, 3], powerUpAmmount[2, 2], powerUpAmmount[2, 1], powerUpAmmount[1, 0], powerUpAmmount[2, 0], powerUpAmmount[1, 2], powerUpAmmount[1, 3]), (BattleManager.Arena)selectedLevel, this.battleOptionsOptionItems[1].selectedIndex + 2, this.battleOptionsOptionItems[0].selectedIndex + 1, (BattleManager.SuddenDeathSettings)this.battleOptionsOptionItems[2].selectedIndex, (BattleManager.RevengeSettings)this.battleOptionsOptionItems[3].selectedIndex);
            battleManager.CreateMatch();
        }

        private void DrawFancyBackground()
        {
            if (backgroundOrigin >= 33)
            {
                backgroundOrigin = 0;
            }
            spriteBatch.Draw(background, new Vector2(-backgroundOrigin), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            backgroundOrigin++;
        }

        public override void Draw(GameTime gameTime)
        {
            switch (CurrentMenuScreen)
            {
                case MenuScreen.pressStart:
                    DrawFancyBackground();

                    spriteBatch.Draw(logo, new Vector2(400, 60), null, Color.White, 0, new Vector2(logo.Width / 2, 0), 0.6f, SpriteEffects.None, 0.5f);
                    spriteBatch.Draw(whiteBomber, new Vector2(400, 170), null, Color.White, 0, new Vector2(whiteBomber.Width / 2, 0), 1f, SpriteEffects.None, 0.6f);
                    if (!isFalse)
                    {
                        spriteBatch.Draw(enterInput, new Vector2(400, 500), null, Color.White, 0, new Vector2(enterInput.Width / 2, 0), 1f, SpriteEffects.None, 1f);
                    }
                    break;
                case MenuScreen.menu:
                    FillRectangle(new Rectangle(50, 75, 700, 475), new Color(0, 0, 0, 75), 0.0001f);
                    DrawFancyBackground();
                    DrawSelectedItemIndicator(new Vector2(270, 185), 60);
                    break;
                case MenuScreen.playerJoin:
                    FillRectangle(new Rectangle(50, 75, 700, 475), new Color(0, 0, 0, 75), 0.0001f);
                    DrawFancyBackground();
                    DrawSelectedItemIndicator(new Vector2(60, 85), 60);
                    if (editingName)
                    {
                        DrawBorderedString(arialRoundedBigger, "ENTER: DONE      BACK SPACE: ERASE", new Vector2(400, 580), 16, Color.White, Color.Red, 0f, arialRoundedBigger.MeasureString(playerJoinCaption) / 2, 1f, SpriteEffects.None, 1f, 0.99999f);
                    }
                    else
                    {
                        DrawBorderedString(arialRoundedBigger, playerJoinCaption, new Vector2(400, 580), 16, Color.White, Color.Red, 0f, arialRoundedBigger.MeasureString(playerJoinCaption) / 2, 1f, SpriteEffects.None, 1f, 0.99999f);
                    }
                    break;
                case MenuScreen.keyboardMapping:
                    FillRectangle(new Rectangle(50, 75, 700, 475), new Color(0, 0, 0, 75), 0.0001f);
                    DrawFancyBackground();
                    DrawSelectedItemIndicator(new Vector2(60, 85), 60);
                    break;
                case MenuScreen.CPULevel:
                    FillRectangle(new Rectangle(50, 75, 700, 475), new Color(0, 0, 0, 75), 0.0001f);
                    DrawFancyBackground();
                    DrawSelectedItemIndicator(new Vector2(60, 85), 60);
                    break;
                case MenuScreen.battleOptions:
                    FillRectangle(new Rectangle(50, 75, 700, 475), new Color(0, 0, 0, 75), 0.0001f);
                    DrawFancyBackground();
                    DrawSelectedItemIndicator(new Vector2(60, 85), 60);
                    break;
                case MenuScreen.levelSelect:
                    DrawFancyBackground();

                    spriteBatch.Draw(stageSelect, new Vector2(400, 70), null, Color.White, 0, new Vector2(stageSelect.Width / 2, stageSelect.Height / 2), 1f, SpriteEffects.None, 1f);
                    for (int i = 0; i < levelSelect.Count; i++)
                    {
                        double angle = levelSelect[i].angle - dynamicAngleOrigin;
                        double cos = Math.Cos(angle);
                        double sin = Math.Sin(angle);
                        float x = (float)(400 + cos * X_PROPORTION);
                        float y = (float)(300 - sin * Y_PROPORTION);
                        float scale = (float)((-sin / 2 + 0.5f) * (MAX_SCALE - MIN_SCALE) + MIN_SCALE);
                        spriteBatch.Draw(levelSelect[i].thumbnail, new Vector2(x, y), null, Color.White, 0, new Vector2(levelSelect[i].thumbnail.Width / 2, levelSelect[i].thumbnail.Height / 2), scale, SpriteEffects.None, (float)-(sin / 4 - 0.75f));
                    }
                    if (angleOrigin == dynamicAngleOrigin)
                    {
                        spriteBatch.Draw(border, new Vector2(400, 300 + Y_PROPORTION), null, Color.White, 0, new Vector2(border.Width / 2, border.Height / 2), MAX_SCALE, SpriteEffects.None, 1f);
                    }
                    DrawBorderedString(arialRoundedBig, levelSelect[selectedLevel].name, new Vector2(400, 500), 16, Color.White, Color.Red, 0f, arialRoundedBig.MeasureString(levelSelect[selectedLevel].name) / 2, 1f, SpriteEffects.None, 1f, 0.9999f);
                    break;
                case MenuScreen.characterSelect:
                    spriteBatch.Draw(characterSelectTitle, new Vector2(400, 70), null, Color.White, 0, new Vector2(characterSelectTitle.Width / 2, characterSelectTitle.Height / 2), 1f, SpriteEffects.None, 1f);
                    FillRectangle(new Rectangle(200, 150, 400, 300), new Color(0, 0, 0, 75), 0.0001f);
                    for (int y = 0; y < 3; y++)
                    {
                        for (int x = 0; x < 4; x++)
                        {
                            Point p = characterSelectionFramePositions[x, y];
                            if (x == selectorPositionX && y == selectorPositionY)
                            {
                                spriteBatch.Draw(characterSelection, new Vector2(220 + (x * 100), 165 + (y * 100)), new Rectangle(p.X + characterAnimationHorizontalSequence[animationFrame], p.Y, 32, 40), Color.White, 0, Vector2.Zero, 1.7f, SpriteEffects.None, 0.9f);
                            }
                            else
                            {
                                spriteBatch.Draw(characterSelection, new Vector2(220 + (x * 100), 165 + (y * 100)), new Rectangle(p.X, p.Y, 32, 40), Color.White, 0, Vector2.Zero, 1.7f, SpriteEffects.None, 0.9f);
                            }
                        }
                    }
                    spriteBatch.Draw(selector, new Vector2(selectorPositionX, selectorPositionY) * 100 + new Vector2(220, 165), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.99f);
                    string caption = playerJoinOptionItems[playerIDCharacterSelect].description; //(playerIDCharacterSelect + 1) + "P";
                    DrawBorderedString(arialRounded, caption, new Vector2(selectorPositionX, selectorPositionY) * 100 + new Vector2(247, 157), 8, Color.White, Color.Black, 0f, arialRounded.MeasureString(caption) / 2, 1f, SpriteEffects.None, 1f, 0.9999f);
                    // white, yellow, lime, red, cyan, blue, orange, pink
                    // if (!(selectorPositionX < 0) && !(selectorPositionX > 3) && !(selectorPositionY < 0) && !(selectorPositionY > 2))
                    /* if (timerUp == TIMER)
                    {
                        if (visible == true)
                        {
                            visible = false;
                        }
                        else
                        {
                            visible = true;
                        }
                        timerUp = 0;
                    }
                    else
                    {
                        if (visible)
                        {
                            spriteBatch.Draw(selector, characterSelectionPositions[selectorPositionY, selectorPositionX], new Rectangle(0, 0, 32, 40), Color.White, 0, Vector2.Zero, 1.7f, SpriteEffects.None, 0.9f);
                        }
                    } */
                    // timerUp += 1;
                    DrawFancyBackground();
                    if (animationTimer <= 0)
                    {
                        animationFrame++;
                        if (animationFrame > 3)
                        {
                            animationFrame = 0;
                        }
                        animationTimer = 10;
                    }
                    else
                    {
                        animationTimer--;
                    }
                    for (int i = 0; i < 8; i++)
                    {
                        if (playerJoinOptionItems[i].selectedIndex != 4)
                        {
                            spriteBatch.Draw(mugshotBorder, new Vector2(96 * i + 32, 530), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                            if (i < playerIDCharacterSelect)
                            {
                                spriteBatch.Draw(characterPositions[playerCharacter[i].X, playerCharacter[i].Y].mugshot, new Vector2(96 * i + 40, 533), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                            }
                            else
                            {
                                if (i == playerIDCharacterSelect)
                                {
                                    spriteBatch.Draw(characterPositions[selectorPositionX, selectorPositionY].mugshot, new Vector2(96 * i + 40, 533), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                                }
                            }
                        }
                    }
                    // spriteBatch.Draw(characterSelect, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 2.5f, SpriteEffects.None, 0f);
                    break;
                case MenuScreen.preview:
                    FillRectangle(new Rectangle(50, 75, 700, 475), new Color(0, 0, 0, 75), 0.0001f);
                    DrawFancyBackground();
                    DrawSelectedItemIndicator(new Vector2(170, 185), 60);
                    break;
                case MenuScreen.powerUps:
                    DrawFancyBackground();
                    FillRectangle(new Rectangle(50, 75, 700, 475), new Color(0, 0, 0, 75), 0.0001f);
                    for (int y = 0; y < 4; y++)
                    {
                        for (int x = 0; x < 3; x++)
                        {
                            if (x == 2 && y == 3)
                            {
                                DrawBorderedString(arialRoundedBigger, "DONE", new Vector2(x * 200 + 106, y * 100 + 104), 8, Color.White, Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f, 0.999f);
                            }
                            else
                            {
                                spriteBatch.Draw(powerUps[x, y], new Vector2(x * 200 + 100, y * 100 + 100), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                                DrawBorderedString(arialRoundedBig, "x", new Vector2(x * 200 + 165, y * 100 + 140), 8, Color.White, Color.Black, 0f, arialRoundedBig.MeasureString("x"), 1f, SpriteEffects.None, 1f, 0.999f);
                                DrawBorderedString(arialRoundedBigger, powerUpAmmount[x, y].ToString(), new Vector2(x * 200 + 170, y * 100 + 100), 8, Color.White, Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f, 0.999f);
                            }
                            if (selectedPowerUp.X == x && selectedPowerUp.Y == y)
                            {
                                spriteBatch.Draw(selectedItemIndicator, new Vector2(x * 200 + 70, y * 100 + 100), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                            }
                        }
                    }
                    DrawBorderedString(arialRoundedBigger, "SLOTS: " + (MAX_POWERUPS - TotalPowerUps), new Vector2(306, 504), 8, Color.Yellow, Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f, 0.999f);
                    break;
            }
            base.Draw(gameTime);
        }
    }
}
