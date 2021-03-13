// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Caxapexac.Common.Sharp.Runtime.Patterns.Service;
using Leopotam.Localization;
using UnityEngine;
using UnityEngine.Networking;


namespace Caxapexac.Common.Sharp.Runtime.Services.Analytics
{
    /// <summary>
    /// Simple GoogleAnalytics manager. Supports tracking of events, screens.
    /// </summary>
    public sealed class GoogleAnalyticsManager : MonoBehaviourService<GoogleAnalyticsManager>
    {
        [SerializeField]
        private string TrackerId = "";

        /// <summary>
        /// Is TrackerID filled ans manager ready to send data.
        /// </summary>
        public bool WasInit
        {
            get => !string.IsNullOrEmpty(TrackerId);
        }

        /// <summary>
        /// Get device identifier, replacement for SystemInfo.deviceUniqueIdentifier.
        /// </summary>
        public string DeviceHash
        {
            get
            {
                if (!string.IsNullOrEmpty(_deviceHash))
                {
                    return _deviceHash;
                }
                _deviceHash = PlayerPrefs.GetString(DeviceHashKey, null);
                if (!string.IsNullOrEmpty(_deviceHash))
                {
                    return _deviceHash;
                }

                // Dont care about floating point regional format for double.
                var userData =
                    $"{SystemInfo.graphicsDeviceVendor}/{SystemInfo.graphicsDeviceVersion}/{SystemInfo.deviceModel}/{SystemInfo.deviceName}/{SystemInfo.operatingSystem}/{SystemInfo.processorCount}/{SystemInfo.systemMemorySize}/{Application.systemLanguage}/{(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds}";
                var data = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(userData));
                var sb = new StringBuilder();
                for (var i = 0; i < data.Length; i++)
                {
                    sb.Append(data[i].ToString("x2"));
                }
                _deviceHash = sb.ToString();
                PlayerPrefs.SetString(DeviceHashKey, _deviceHash);
#if UNITY_EDITOR
                Debug.Log("[GA] New device hash generated: " + _deviceHash);
#endif
                return _deviceHash;
            }
        }

        private const string AnalyticsUrl = "http://www.google-analytics.com/collect?v=1&tid={0}&cid={1}&sr={2}x{3}&an={4}&av={5}&z=";

        private const string DeviceHashKey = "_deviceHash";

        private readonly Queue<string> _requests = new Queue<string>(64);

        private string _requestUrl;

        private string _deviceHash;

        protected override void OnCreateService()
        {
            DontDestroyOnLoad(gameObject);
        }

        protected override void OnDestroyService()
        {
        }

        private IEnumerator Start()
        {
            _requestUrl = null;

            // Wait for additional init.
            yield return null;

#if UNITY_EDITOR
            if (string.IsNullOrEmpty(TrackerId))
            {
                Debug.LogWarning("GA.TrackerID not defined");
            }
#endif
            if (!string.IsNullOrEmpty(TrackerId))
            {
                _requestUrl = string.Format(AnalyticsUrl, TrackerId, DeviceHash, Screen.width,
                    Screen.height, Application.identifier, Application.version);
            }

            string url = null;
            string data;

            while (true)
            {
                if (_requests.Count > 0)
                {
                    data = _requests.Dequeue();

                    // If tracking id defined and url inited.
                    if (!string.IsNullOrEmpty(_requestUrl))
                    {
                        url = $"{_requestUrl}{UnityEngine.Random.Range(1, 99999)}&{data}&ul={Service<CsvLocalization>.Get().Language}";
                    }
                }

                if (url != null)
                {
#if UNITY_EDITOR
                    Debug.Log("[GA REQUEST] " + url);
#endif

                    using (var req = UnityWebRequest.Get(url))
                    {
                        req.SetRequestHeader("user-agent", "");
                        yield return req.SendWebRequest();
                    }
                    url = null;
                }
                else
                {
                    yield return null;
                }
            }
        }

        private void EnqueueRequest(string url)
        {
            _requests.Enqueue(url);
        }

        /// <summary>
        /// Track current screen.
        /// </summary>
        public void TrackScreen()
        {
            TrackScreen(Service<ScreenManager>.Get().Current);
        }

        /// <summary>
        /// Track screen with custom name.
        /// </summary>
        /// <param name="screenName">Custom screen name.</param>
        public void TrackScreen(string screenName)
        {
            // Old version of screen tracking: EnqueueRequest (string.Format ("t=screenview&cd={0}", UnityWebRequest.EscapeURL (screenName)));
            // ReSharper disable once StringLiteralTypo
            EnqueueRequest($"t=pageview&dp={UnityWebRequest.EscapeURL(screenName)}");
        }

        /// <summary>
        /// Track event.
        /// </summary>
        /// <param name="category">Category name.</param>
        /// <param name="action">Action name.</param>
        public void TrackEvent(string category, string action)
        {
            EnqueueRequest($"t=event&ec={UnityWebRequest.EscapeURL(category)}&ea={UnityWebRequest.EscapeURL(action)}");
        }

        /// <summary>
        /// Track event.
        /// </summary>
        /// <param name="category">Category name.</param>
        /// <param name="action">Action name.</param>
        /// <param name="label">Label name.</param>
        /// <param name="value">Value.</param>
        public void TrackEvent(string category, string action, string label, string value)
        {
            EnqueueRequest($"t=event&ec={UnityWebRequest.EscapeURL(category)}&ea={UnityWebRequest.EscapeURL(action)}&el={UnityWebRequest.EscapeURL(label)}&ev={UnityWebRequest.EscapeURL(value)}");
        }

        /// <summary>
        /// Track transaction for e-commerce, in-app purchases.
        /// </summary>
        /// <param name="transactionId">Transaction ID, will be truncated up to 100 symbols.</param>
        /// <param name="productName">Product name.</param>
        /// <param name="sku">Product code.</param>
        /// <param name="price">Product price.</param>
        /// <param name="currency">ISO currency code, 3 letters. USD by default</param>
        public void TrackTransaction(string transactionId, string productName, string sku, decimal price, string currency = "USD")
        {
            transactionId = (transactionId.Length <= 100) ? transactionId : transactionId.Substring(0, 100);
            EnqueueRequest($"t=transaction&ti={UnityWebRequest.EscapeURL(transactionId)}&tr={price}&cu={UnityWebRequest.EscapeURL(currency)}&ts=0&tt=0");
            EnqueueRequest(
                $"t=item&ti={UnityWebRequest.EscapeURL(transactionId)}&in={UnityWebRequest.EscapeURL(productName)}&ic={UnityWebRequest.EscapeURL(sku)}&ip={price}&iq=1&cu={UnityWebRequest.EscapeURL(currency)}");
        }

        /// <summary>
        /// Track exception event.
        /// </summary>
        /// <param name="description">Description of exception.</param>
        /// <param name="isFatal">Is exception fatal.</param>
        public void TrackException(string description, bool isFatal)
        {
            EnqueueRequest($"t=exception&exd={UnityWebRequest.EscapeURL(description)}&exf={(isFatal ? 1 : 0)}");
        }
    }
}