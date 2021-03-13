/*
 * Created by Alexander Sosnovskiy. May 3, 2016
 */
using System;
using UnityEngine;

// ReSharper disable RedundantCast.0
// ReSharper disable InconsistentNaming


namespace Caxapexac.Common.Sharp.Runtime.Cached
{
    /// <summary>
    /// MonoBehaviour with cached properties
    /// </summary>
    public class CachedMonoBehaviour : MonoBehaviour
    {
        [NonSerialized]
        private GameObject _cachedGameObject;
        [NonSerialized]
        private Transform _cachedTransform;
        [NonSerialized]
        private RectTransform _cachedRectTransform;

        [NonSerialized]
        private Collider _cachedCollider;
        [NonSerialized]
        private Collider2D _cachedCollider2D;
        [NonSerialized]
        private Rigidbody _cachedRigidbody;
        [NonSerialized]
        private Rigidbody2D _cachedRigidbody2D;
        [NonSerialized]
        private Joint _cachedJoint;

        [NonSerialized]
        private Camera _cachedCamera;
        [NonSerialized]
        private AudioSource _cachedAudioSource;
        [NonSerialized]
        private Light _cachedLight;
        [NonSerialized]
        private Renderer _cachedRenderer;
        [NonSerialized]
        private ParticleSystem _cachedParticleSystem;

        public new GameObject gameObject
        {
            get
            {
                if ((object)_cachedGameObject == null) _cachedGameObject = base.gameObject;
                return _cachedGameObject;
            }
        }

        public new Transform transform
        {
            get
            {
                if ((object)_cachedTransform == null) _cachedTransform = base.transform;
                return _cachedTransform;
            }
        }

        public RectTransform rectTransform
        {
            get
            {
                if ((object)_cachedRectTransform == null) _cachedRectTransform = transform as RectTransform;
                return _cachedRectTransform;
            }
        }

        public new Collider collider
        {
            get
            {
                if ((object)_cachedCollider == null) _cachedCollider = GetComponent<Collider>();

                return _cachedCollider;
            }
        }

        public new Collider2D collider2D
        {
            get
            {
                if ((object)_cachedCollider2D == null) _cachedCollider2D = GetComponent<Collider2D>();

                return _cachedCollider2D;
            }
        }

        public new Rigidbody rigidbody
        {
            get
            {
                if ((object)_cachedRigidbody == null) _cachedRigidbody = GetComponent<Rigidbody>();

                return _cachedRigidbody;
            }
        }

        public new Rigidbody2D rigidbody2D
        {
            get
            {
                if ((object)_cachedRigidbody2D == null) _cachedRigidbody2D = GetComponent<Rigidbody2D>();
                return _cachedRigidbody2D;
            }
        }

        public Joint joint
        {
            get
            {
                if ((object)_cachedJoint == null) _cachedJoint = GetComponent<Joint>();
                return _cachedJoint;
            }
        }


        public new Camera camera
        {
            get
            {
                if ((object)_cachedCamera == null) _cachedCamera = GetComponent<Camera>();

                return _cachedCamera;
            }
        }

        public new AudioSource audio
        {
            get
            {
                if ((object)_cachedAudioSource == null) _cachedAudioSource = GetComponent<AudioSource>();

                return _cachedAudioSource;
            }
        }

        public new Light light
        {
            get
            {
                if ((object)_cachedLight == null) _cachedLight = GetComponent<Light>();

                return _cachedLight;
            }
        }

        public new Renderer renderer
        {
            get
            {
                if ((object)_cachedRenderer == null) _cachedRenderer = GetComponent<Renderer>();

                return _cachedRenderer;
            }
        }

        public new ParticleSystem particleSystem
        {
            get
            {
                if ((object)_cachedParticleSystem == null) _cachedParticleSystem = GetComponent<ParticleSystem>();

                return _cachedParticleSystem;
            }
        }
    }
}