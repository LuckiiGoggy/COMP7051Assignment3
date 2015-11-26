using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assgn01
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
            SoundEffectIns = new Dictionary<string, SoundEffectInstance>();
            Volume = 1f;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.5f;
        }

        public void InitSoundLibrary(ContentManager Content)
        {
            Musics.Add("BG1", Content.Load<Song>("nothing_it_can"));

            SoundEffects.Add("collideins", Content.Load<SoundEffect>("punch"));

            SoundEffectIns.Add("collideins", SoundEffects["collideins"].CreateInstance());
        }

        public void LoopMusic(string name)
        {
            MediaPlayer.Volume = (0.5f);
            if (!Musics.ContainsKey(name)) throw new MissingMusicException("MissingMusicEffect: " + name);
            if (MediaPlayer.State == MediaState.Playing)
            {

            }
            else
            {
                MediaPlayer.Play(Musics[name]);
            }
           
        }

        public void PauseMusic()
        {
                MediaPlayer.Pause();
        }

        public void ResumeMusic()
        {
            MediaPlayer.Resume();
        }

        public void PlaySoundEffect(string name)
        {
            if (!SoundEffectIns.ContainsKey(name)) throw new MissingSoundEffectException("MissingSoundEffect: " + name);
            

           // SoundEffectIns[name].Play();

            SoundEffects[name].Play(0.1f, 0.0f, 0f);   
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
