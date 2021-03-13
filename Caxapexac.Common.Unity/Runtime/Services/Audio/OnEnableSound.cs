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
    /// Setup FX parameters on enable.
    /// </summary>
    public sealed class OnEnableSound : MonoBehaviour
    {
        [SerializeField]
        private AudioClip Sound = null;

        [SerializeField]
        private SoundFxChannel Channel = SoundFxChannel.First;

        /// <summary>
        /// Should new FX force interrupts FX at channel or not.
        /// </summary>
        [SerializeField]
        private bool IsInterrupt = false;

        private void OnEnable()
        {
            Service<SoundFromResourcesManager>.Get().PlayFx(Sound, Channel, IsInterrupt);
        }
    }
}