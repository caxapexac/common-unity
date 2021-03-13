// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using Caxapexac.Common.Sharp.Runtime.Patterns.Service;
using UnityEngine;


namespace Caxapexac.Common.Sharp.Runtime.Services.Analytics
{
    /// <summary>
    /// Send analytic event on enable.
    /// </summary>
    public sealed class SendAnalyticEventOnEnable : MonoBehaviour
    {
        [SerializeField]
        private string Category = "Category";

        [SerializeField]
        private string Event = "Event";

        private void OnEnable()
        {
            if (!string.IsNullOrEmpty(Category) && !string.IsNullOrEmpty(Event))
            {
                Service<GoogleAnalyticsManager>.Get().TrackEvent(Category, Event);
            }
        }
    }
}