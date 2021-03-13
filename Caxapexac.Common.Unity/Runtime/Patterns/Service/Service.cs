// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System;
using UnityEngine;

//using UnityEngine;


namespace Caxapexac.Common.Sharp.Runtime.Patterns.Service
{
    /// <summary>
    /// Service locator pattern implementation.
    /// </summary>
    public class Service<T> where T : class
    {
        private static T _instance;

        /// <summary>
        /// Is instance of service created.
        /// </summary>
        public static bool IsRegistered
        {
            get => (object)_instance != null;
        }

        /// <summary>
        /// Get service instance of generic type with lazy initialization.
        /// Warning: Touching MonoBehaviourService<T>-services at any Awake() method will lead to undefined behaviour!
        /// </summary>
        public static T Get()
        {
            // Unwrap IsRegistered for performance.
            if ((object)_instance != null)
            {
                return _instance;
            }
            var type = typeof(T);

            // Special case for unity scriptable objects.
            if (type.IsSubclassOf(typeof(ScriptableObject)))
            {
                var list = Resources.FindObjectsOfTypeAll(type);
                if (list == null || list.Length == 0)
                {
                    throw new UnityException(
                        $"Service<{type.Name}>.Get() can be used only with exists / loaded asset of this type");
                }
                _instance = list[0] as T;
                return _instance;
            }

            // Not allow any unity components except wrapped to special class.
#if UNITY_EDITOR
            if (type.IsSubclassOf(typeof(Component)) && !type.IsSubclassOf(typeof(MonoBehaviourService<T>)))
            {
                throw new UnityException($"\"{type.Name}\" - invalid type, should be inherited from MonoBehaviourService");
            }
#endif

            // Special case for MonoBehaviourService<T> components.
            if (type.IsSubclassOf(typeof(MonoBehaviourService<T>)))
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    throw new UnityException($"Service<{type.Name}>.Get() can be used only at PLAY mode");
                }
#endif
                new GameObject(
#if UNITY_EDITOR
                    "_SERVICE_" + type.Name
#endif
                ).AddComponent(type);
            }
            else
            {
                Register(Activator.CreateInstance(type) as T);
            }
            return _instance;
        }

        /// <summary>
        /// Register instance as service.
        /// </summary>
        /// <param name="instance">Service instance.</param>
        public static void Register(T instance)
        {
            if (IsRegistered)
            {
                throw new UnityException($"Cant register \"{typeof(T).Name}\" as service - type already registered");
            }
            if (instance == null)
            {
                throw new UnityException("Cant register null instance as service");
            }
            _instance = instance;
        }

        /// <summary>
        /// Unregister service instance.
        /// </summary>
        /// <param name="instance">Instance for unregister.</param>
        /// <param name="force">Force unregister instance even on invalid input instance.</param>
        public static void Unregister(T instance, bool force = false)
        {
            if (instance == _instance || force)
            {
                _instance = null;
            }
        }
    }
}