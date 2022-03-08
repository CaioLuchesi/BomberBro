using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace BomberBro
{
    public class BattleManager
    {      
        public List<PlayerData>[] teamData = new List<PlayerData>[8];
        public PlayerData? PlayerDataAtIndex(byte index)
        {
            for (byte i = 0; i < 8; i++)
            {
                foreach (PlayerData data in teamData[i])
                {
                    if (index == 0)
                    {
                        return data;
                    }
                    index--;
                }
            }
            return null;
        }

        public int? winnerTeamID = null;

        public int timeLimit = 3;
        public int numberOfWins = 5;

        private Game game;

        private DrawableRoom _currentRoom = null;
        
        public DrawableRoom currentRoom
        {
            get
            {
                return _currentRoom;
            }
            set
            {
                if (currentRoom != null)
                    game.Components.RemoveAt(game.Components.Count - 1);
                //game.Components.Clear();

                //if(currentRoom != null)
                // currentRoom.Dispose();

                _currentRoom = value;
                game.Components.Add(value);
                value.Visible = true;
                value.Enabled = true;
            }
        }

        public PowerUpManager powerUpManager;

        private int[] wins = new int[8];
        public int[] Wins
        {
            get
            {
                return (int[])wins.Clone();
            }
        }

        private List<int[,]> fragBoard = new List<int[,]>();
        public List<int[,]> FragBoard
        {
            get
            {
                List<int[,]> result = new List<int[,]>();
                for (int i = 0; i < fragBoard.Count; i++)
                {
                    result.Add((int[,])fragBoard[i].Clone());
                }
                return fragBoard.ToList<int[,]>();
            }
        }

        public struct PlayerData
        {
            public enum CPULevel
            {
                easy,
                normal,
                hard,
                none
            }

            public string name;
            public Character character;
            public KeyboardMapping keyboardMapping;
            public CPULevel Level;

            public PlayerData(string name, Character character, KeyboardMapping keyboardMapping,CPULevel Level)
            {
                this.name = name;
                this.character = character;
                this.keyboardMapping = keyboardMapping;
                this.Level = Level;
            }

        }

        public enum SuddenDeathSettings
        {
            off,
            on,
            super
        }

        public SuddenDeathSettings suddenDeathSettings = SuddenDeathSettings.super;

        public enum RevengeSettings
        {
            off,
            on,
            super
        }
        public RevengeSettings revengeSettings = RevengeSettings.off;

        public enum Arena
        {
            pirate,
            forest,
            desert,
            excel
        }
        private Arena arena;

        private int currentMatchIndex
        {
            get
            {
                return fragBoard.Count - 1;
            }
        }

        public BattleManager(Game game, List<PlayerData>[] playerData, PowerUpManager powerUpManager, Arena arena, int timeLimit, int numberOfWins, SuddenDeathSettings suddenDeathSettings, RevengeSettings revengeSettings)
        {
            this.game = game;
            this.teamData = playerData;
            this.arena = arena;
            this.powerUpManager = powerUpManager;
            this.timeLimit = timeLimit;
            this.numberOfWins = numberOfWins;
            this.suddenDeathSettings = suddenDeathSettings;
            this.revengeSettings = revengeSettings;
        }

        public void CreateMatch()
        {
            
            MediaPlayer.Stop();
            fragBoard.Add(new int[8, 8]);
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    fragBoard[currentMatchIndex][x, y] = 0;
                }
            }
            currentRoom = new Match(game, this, arena, powerUpManager);
        }

        public void CreateScoreboard()
        {
            currentRoom = new ScoreboardScreen(game, this, (Match)currentRoom);
                 
        }

        public void DisplayVictoryScreen()
        {
            currentRoom = new VictoryScreen(game, this);
        }

        public void Winner(int teamID)
        {
            wins[teamID]++;
            if (wins[teamID] == numberOfWins)
            {
                winnerTeamID = teamID;
            }
        }

        public void Frag(int fraggerID, int victimID)
        {
            try
            {
                fragBoard[currentMatchIndex][fraggerID, victimID]++;
            }
            catch (IndexOutOfRangeException)
            {
                // do nothing
            }
        }
    }
}
