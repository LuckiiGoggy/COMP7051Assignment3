using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assignment3.Utilities
{
    public class SoundPlayer
    {
        public float Volume { get; set; }
        Dictionary<string, SoundEffect> SoundEffects;
        Dictionary<string, Song> Musics;
        Dictionary<string, SoundEffectInstance> SoundEffectIns;
        public SoundPlayer()
        {
            SoundEffects = new Dictionary<string, SoundEffect>();
            Musics = new Dictionary<string, Song>();
            SoundEffectIns = new Dictionary<string,SoundEffectInstance>();
            Volume = 1f;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.1f;
        }

        public void InitSoundLibrary(ContentManager Content)
        {
            Musics.Add("BG1", Content.Load<Song>("Music\\nothing_it_can"));
            Musics.Add("BG2", Content.Load<Song>("Music\\Maze_Ambience"));
            SoundEffects.Add("footstep", Content.Load<SoundEffect>("Music\\single_footstep"));
            SoundEffects.Add("collide", Content.Load<SoundEffect>("Music\\punch"));
         
            SoundEffectIns.Add("footstepins", SoundEffects["footstep"].CreateInstance());
           
            SoundEffectIns.Add("collideins",SoundEffects["collide"].CreateInstance());

        }

        public void LoopMusic(string name)
        {
           // MediaPlayer.Volume = (0.05f);
            if (!Musics.ContainsKey(name)) throw new MissingMusicException("MissingMusicEffect: " + name);
            MediaPlayer.Play(Musics[name]);
        }

        public void PauseMusic()
        {
            if(MediaPlayer.State == MediaState.Paused)
                MediaPlayer.Resume();
            else
                MediaPlayer.Pause();            
        }

        public void UpdateVolume()
        {
            if (Game1.fog)
            {
                MediaPlayer.Volume = 0.3f;

            }
            else if (!Game1.fog)
            {
                MediaPlayer.Volume = 0.6f;
            }

        }
        public void ChangeBGVol(float z){
            MediaPlayer.Volume = 0.05f * z;
        }
        public void StopMusic()
        {
            MediaPlayer.Stop();
        }
        public void PlaySoundEffect(string name)
        {
            if (!SoundEffectIns.ContainsKey(name)) throw new MissingSoundEffectException("MissingSoundEffect: " + name);

            if (Game1.fog)
            {
                SoundEffectIns[name].Volume = (float)(0.6);

            }
            else if (!Game1.fog)
            {
                SoundEffectIns[name].Volume = (float)(0.6);
            }
            
            SoundEffectIns[name].Play();

            //SoundEffects[name].Play(0.3f, 0.0f, 1f);   
        }
        
    }
    public class MissingSoundEffectException : Exception
    {
        public MissingSoundEffectException()
        {
        }

        public MissingSoundEffectException(string message)
            : base(message)
        {
        }

        public MissingSoundEffectException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class MissingMusicException : Exception
    {
        public MissingMusicException()
        {
        }

        public MissingMusicException(string message)
            : base(message)
        {
        }

        public MissingMusicException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
