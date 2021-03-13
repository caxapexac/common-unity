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
    /// Setup FX parameters on start.
    /// </summary>
    public sealed class OnStartSound : MonoBehaviour
    {
        [SerializeField]
        private AudioClip Sound = null;

        [SerializeField]
        private SoundFxChannel Channel = SoundFxChannel.First;

        /// <summary>
        /// Should new FX force interrupts FX at channel or not.
        /// </summary>
        public bool IsInterrupt;

        private IEnumerator Start()
        {
            yield return null;
            Service<SoundFromResourcesManager>.Get().PlayFx(Sound, Channel, IsInterrupt);
        }
    }
}