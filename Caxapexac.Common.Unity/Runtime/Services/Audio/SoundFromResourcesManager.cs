// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using Caxapexac.Common.Sharp.Runtime.Patterns.Service;
using UnityEngine;


namespace Caxapexac.Common.Sharp.Runtime.Services.Audio
{
    /// <summary>
    /// SoundManager channel for playing FX.
    /// </summary>
    public enum SoundFxChannel
    {
        First = 0,
        Second = 1,
        Third = 2,
        Fourth = 3,
        Fifth = 4,
        Max = Fifth
    }


    /// <summary>
    /// Sound manager.
    /// </summary>
    public sealed class SoundFromResourcesManager : MonoBehaviourService<SoundFromResourcesManager>
    {
        /// <summary>
        /// FX-es volume.
        /// </summary>
        public float SoundVolume
        {
            get => _sounds[0].volume;
            set
            {
                foreach (var item in _sounds)
                {
                    item.volume = value;
                }
            }
        }

        /// <summary>
        /// Music volume.
        /// </summary>
        public float MusicVolume
        {
            get => _music.volume;
            set => _music.volume = value;
        }

        /// <summary>
        /// Name of last played music.
        /// </summary>
        public string LastPlayedMusic { get; private set; }

        private bool _isLastPlayedMusicLooped;

        private AudioSource _music;

        private AudioSource[] _sounds;

        protected override void OnCreateService()
        {
            DontDestroyOnLoad(gameObject);

            _music = gameObject.AddComponent<AudioSource>();

            _sounds = new AudioSource[(int)SoundFxChannel.Max + 1];
            var go = gameObject;
            for (var i = 0; i < _sounds.Length; i++)
            {
                _sounds[i] = go.AddComponent<AudioSource>();
            }

            _music.loop = false;
            _music.playOnAwake = false;
            foreach (var item in _sounds)
            {
                item.loop = false;
                item.playOnAwake = false;
            }
        }

        protected override void OnDestroyService()
        {
        }

        /// <summary>
        /// Play music.
        /// </summary>
        /// <param name="music">Music name.</param>
        /// <param name="isLooped">Is looped.</param>
        public void PlayMusic(string music, bool isLooped = false)
        {
            if (LastPlayedMusic == music && _music.isPlaying)
            {
                return;
            }

            StopMusic();

            LastPlayedMusic = music;
            _isLastPlayedMusicLooped = isLooped;

            if (MusicVolume > 0f && !string.IsNullOrEmpty(LastPlayedMusic))
            {
                _music.clip = Resources.Load<AudioClip>(LastPlayedMusic);
                _music.loop = isLooped;
                _music.Play();
            }
        }

        /// <summary>
        /// Play FX.
        /// </summary>
        /// <param name="clip">AudioClip object.</param>
        /// <param name="channel">Channel for playing.</param>
        /// <param name="forceInterrupt">Force interrupt previous FX at chanel.</param>
        public void PlayFx(AudioClip clip, SoundFxChannel channel = SoundFxChannel.First, bool forceInterrupt = false)
        {
            var fx = _sounds[(int)channel];
            if (!forceInterrupt && fx.isPlaying)
            {
                return;
            }

            StopFx(channel);

            fx.clip = clip;

            if (SoundVolume > 0f && clip != null)
            {
                fx.Play();
            }
        }

        /// <summary>
        /// Stop playing FX at channel.
        /// </summary>
        /// <param name="channel">Channel.</param>
        public void StopFx(SoundFxChannel channel)
        {
            var fx = _sounds[(int)channel];
            if (fx.isPlaying)
            {
                fx.Stop();
            }
            fx.clip = null;
        }

        /// <summary>
        /// Stop playing music.
        /// </summary>
        /// <returns>The music.</returns>
        public void StopMusic()
        {
            _music.Stop();
        }

        /// <summary>
        /// Validates music after music volume set to zero / restore volume.
        /// </summary>
        /// <returns>The music.</returns>
        public void ValidateMusic()
        {
            if (MusicVolume > 0f && !string.IsNullOrEmpty(LastPlayedMusic))
            {
                if (!_music.isPlaying)
                {
                    PlayMusic(LastPlayedMusic, _isLastPlayedMusicLooped);
                }
            }
            else
            {
                StopMusic();
            }
        }
    }
}