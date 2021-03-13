using System;
using Caxapexac.Common.Sharp.Runtime.Patterns.Service;
using UnityEngine;
using UnityEngine.Audio;


namespace Caxapexac.Common.Sharp.Runtime.Services.Audio
{
    /// <summary>
    /// Prefer using SoundFromResourcesManager instead of this one
    /// </summary>
    public class SoundFromInspectorManager : MonoBehaviourService<SoundFromInspectorManager>
    {
        [SerializeField]
        private AudioMixerGroup MixerGroup = null;
        [SerializeField]
        private Sound[] Sounds = null;

        protected override void OnCreateService()
        {
            DontDestroyOnLoad(gameObject);
            foreach (Sound s in Sounds)
            {
                s.Source = gameObject.AddComponent<AudioSource>();
                s.Source.clip = s.Clip;
                s.Source.loop = s.Loop;

                s.Source.outputAudioMixerGroup = MixerGroup;
            }
        }

        protected override void OnDestroyService()
        {
        }

        public void Play(string sound)
        {
            Sound s = Array.Find(Sounds, item => item.Name == sound);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }

            s.Source.volume = s.Volume * (1f + UnityEngine.Random.Range(-s.VolumeVariance / 2f, s.VolumeVariance / 2f));
            s.Source.pitch = s.Pitch * (1f + UnityEngine.Random.Range(-s.PitchVariance / 2f, s.PitchVariance / 2f));

            s.Source.Play();
        }

        public void Stop(string sound)
        {
            Sound s = Array.Find(Sounds, item => item.Name == sound);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }
            s.Source.Stop();
        }
    }
}