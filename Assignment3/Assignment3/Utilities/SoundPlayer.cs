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
        Dictionary<string, SoundEffect> SoundEffects;
        Dictionary<string, Song> Musics;

        public SoundPlayer()
        {
            SoundEffects = new Dictionary<string, SoundEffect>();
            Musics = new Dictionary<string, Song>();
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.1f;
        }

        public void InitSoundLibrary(ContentManager Content)
        {
            Musics.Add("BG1", Content.Load<Song>("Music\\nothing_it_can"));
            Musics.Add("BG2", Content.Load<Song>("Music\\Maze_Ambience"));
            SoundEffects.Add("footstep", Content.Load<SoundEffect>("Music\\single_footstep"));
        }

        public void LoopMusic(string name)
        {
            if (!Musics.ContainsKey(name)) throw new MissingMusicException("MissingMusicEffect: " + name);
            MediaPlayer.Play(Musics[name]);
        }

        public void DecreaseMusicVolume()
        {
            if (MediaPlayer.Volume > 0.0f) MediaPlayer.Volume -= 0.1f;
        }

        public void StopMusic()
        {
            MediaPlayer.Stop();
        }
        public void PlaySoundEffect(string name)
        {
            if (!SoundEffects.ContainsKey(name)) throw new MissingSoundEffectException("MissingSoundEffect: " + name);
            SoundEffectInstance inst = SoundEffects[name].CreateInstance();
            inst.Volume = 1.0f;
            SoundEffects[name].Play(0.5f, 0.0f, 0.0f);
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
