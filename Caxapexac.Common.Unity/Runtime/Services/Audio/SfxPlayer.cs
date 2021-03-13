using Caxapexac.Common.Sharp.Runtime.Patterns.Singleton;
using UnityEngine;


namespace Caxapexac.Common.Sharp.Runtime.Services.Audio
{
    public class SfxPlayer : Singleton<SfxPlayer>
    {
        public AudioSource AudioPrefab2D;
        public AudioSource AudioPrefab3D;

        public void Play2D(AudioClip audioClip)
        {
            AudioSource audioSource = Instantiate(AudioPrefab2D, transform);
            audioSource.clip = audioClip;
            audioSource.Play();
        }

        public void Play3D(AudioClip audioClip, Vector3 position)
        {
            AudioSource audioSource = Instantiate(AudioPrefab3D, position, Quaternion.identity, transform);
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }
}