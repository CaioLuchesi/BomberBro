using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;

namespace BomberBro
{
    public class AudioLibrary
    {
        private SoundEffect newMeteor;
        private SoundEffect menuBack;
        private SoundEffect menuSelect;
        private SoundEffect menuScroll;
        private SoundEffect powerGet;
        private SoundEffect powerShow;
        public static SoundEffect beep;
        public static SoundEffect explosion;
        public static SoundEffect itemPick;
        private SoundEffect death;
        private Song backMusic;
        private Song startMusic;
        private Song pirateSong;
        private Song forestSong;

        public SoundEffect Death
        {
            get { return death; }
        }

        public SoundEffect NewMeteor
        {
            get { return newMeteor; }
        }

        public SoundEffect MenuBack
        {
            get { return menuBack; }
        }

        public SoundEffect MenuSelect
        {
            get { return menuSelect; }
        }

        public SoundEffect MenuScroll
        {
            get { return menuScroll; }
        }

        public SoundEffect PowerGet
        {
            get { return powerGet; }
        }

        public SoundEffect PowerShow
        {
            get { return powerShow; }
        }

        public Song BackMusic
        {
            get { return backMusic; }
        }

        public Song StartMusic
        {
            get { return startMusic; }
        }
        public Song PirateSong
        {
            get { return pirateSong; }
        }
        public Song ForestSong
        {
            get { return forestSong; }
        }

        public void LoadContent(ContentManager Content)
        {
            try
            {
                pirateSong = Content.Load<Song>("sons/Tale of Sack Sparrow");
                forestSong = Content.Load<Song>("sons/Bomberman World Battle");
                explosion = Content.Load<SoundEffect>("sons/explosion");
                itemPick = Content.Load<SoundEffect>("sons/itempick");
                // newMeteor = Content.Load<SoundEffect>("sons/newmeteor");
                // backMusic = Content.Load<Song>("sons/backMusic");
                // startMusic = Content.Load<Song>("sons/VanHalen_Panama");
                menuBack = Content.Load<SoundEffect>("sons/menu_back");
                menuSelect = Content.Load<SoundEffect>("sons/menu_select3");
                menuScroll = Content.Load<SoundEffect>("sons/menu_scroll");
                powerShow = Content.Load<SoundEffect>("sons/powershow");
                beep = Content.Load<SoundEffect>("sons/beep1");
                death = Content.Load<SoundEffect>("sons/death");
                // powerGet = Content.Load<SoundEffect>("sons/powerget");
            }
            catch (NoAudioHardwareException)
            {
                Console.WriteLine("YOU SUCK");
            }
        }
    }
}
