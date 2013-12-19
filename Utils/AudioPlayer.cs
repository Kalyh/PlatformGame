using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MP3player
{
    class AudioPlayer
    {
        private string Pcommand;
        private bool isOpen;
        public string media;

        [DllImport("winmm.dll")]
        private static extern long mciSendString(string strCommand,
        StringBuilder strReturn, int iReturnLength, int bla);

        /// <SUMMARY>
        /// Not much to construct here
        /// </SUMMARY>
        public AudioPlayer(string m)
        {
            media = m;
        }

        /// <SUMMARY>
        /// Stops currently playing audio file
        /// </SUMMARY>
        public void Close()
        {
            Pcommand = "close " + media;
            mciSendString(Pcommand, null, 0, 0);
            isOpen = false;
        }

        /// <SUMMARY>
        /// Opens audio file to play
        /// </SUMMARY>
        /// <param name="sFileName" />This is the audio file's path and filename</param />
        public void Open(string sFileName)
        {
            Pcommand = "open \"" + sFileName + "\" type waveaudio alias " + media;
            mciSendString(Pcommand, null, 0, 0);
            isOpen = true;
        }

        /// <SUMMARY>
        /// Plays selected audio file
        /// </SUMMARY>
        /// <param name="loop" />If True,audio file will repeat</param />
        public void Play()
        {
            if (isOpen)
            {
                Pcommand = "play " + media;
                mciSendString(Pcommand, null, 0, 0);
            }
        }

        /// <SUMMARY>
        /// Pauses currently playing audio file
        /// </SUMMARY>
        public void Pause()
        {
            Pcommand = "pause " + media;
            mciSendString(Pcommand, null, 0, 0);
        }

        public void kill()
        {
            Pcommand = "close " + media;
            mciSendString(Pcommand, null, 0, 0); 
        }

        /// <SUMMARY>
        /// Get/Set Left Volume Factor
        /// </SUMMARY>
        public int LeftVolume
        {
            get
            {
                return 0; //Guess could be used to return Volume level: I don't need it
            }
            set
            {
                mciSendString(string.Concat
        ("setaudio MediaFile left volume to ", value), null, 0, 0);
            }
        }

        /// <SUMMARY>
        /// Get/Set Right Volume Factor
        /// </SUMMARY>
        public int RightVolume
        {
            get
            {
                return 0; //Guess could be used to return Volume level: I don't need it
            }
            set
            {
                mciSendString(string.Concat
        ("setaudio MediaFile right volume to ", value), null, 0, 0);
            }
        }

        /// <SUMMARY>
        /// Get/Set Main Volume Factor
        /// </SUMMARY>
        public int MasterVolume
        {
            get
            {
                return 0; //Guess could be used to return Volume level: I don't need it
            }
            set
            {
                mciSendString(string.Concat
        ("setaudio MediaFile volume to ", value), null, 0, 0);
            }
        }

        /// <SUMMARY>
        /// Get/Set Bass Volume Factor
        /// </SUMMARY>
        public int Bass
        {
            get
            {
                return 0;
            }
            set
            {
                mciSendString(string.Concat
        ("setaudio MediaFile bass to ", value), null, 0, 0);
            }
        }

        /// <SUMMARY>
        /// Get/Set Treble Volume Factor
        /// </SUMMARY>
        public int Treble
        {
            get
            {
                return 0;
            }
            set
            {
                mciSendString(string.Concat
        ("setaudio MediaFile treble to ", value), null, 0, 0);
            }
        }
    }
}