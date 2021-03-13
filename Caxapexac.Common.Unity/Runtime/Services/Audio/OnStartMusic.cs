// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System.Collections;
using Caxapexac.Common.Sharp.Runtime.Patterns.Service;
using UnityEngine;


namespace Caxapexac.Common.Sharp.Runtime.Services.Audio
{
    /// <summary>
    /// Setup music parameters on start.
    /// </summary>
    public sealed class OnStartMusic : MonoBehaviour
    {
        [SerializeField]
        private string Music = "";

        [SerializeField]
        private bool IsLooped = true;

        private IEnumerator Start()
        {
            yield return null;
            var sm = Service<SoundFromResourcesManager>.Get();
            if (sm.MusicVolume <= 0f)
            {
                sm.StopMusic();
            }
            sm.PlayMusic(Music, IsLooped);
        }
    }
}