using UnityEngine;


namespace Caxapexac.Common.Sharp.Runtime.Patterns.Service
{
    /// <summary>
    /// MonoBehaviour base class for service locator pattern.
    /// Warning: Touching services at any Awake() method will lead to undefined behaviour!
    /// </summary>
    public abstract class MonoBehaviourService<T> : MonoBehaviour where T : class
    {
        private void Awake()
        {
            if (Service<T>.IsRegistered)
            {
                DestroyImmediate(this);
                return;
            }
#if UNITY_EDITOR
            var type = GetType();

            // check for allowed scenes if possible.
            var attrs = type.GetCustomAttributes(typeof(MonoBehaviourServiceFilterAttribute), true);
            if (attrs.Length > 0)
            {
                var sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                var i = attrs.Length - 1;
                for (; i >= 0; i--)
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(
                        sceneName, ((MonoBehaviourServiceFilterAttribute)attrs[i]).Name))
                    {
                        break;
                    }
                }
                if (i == -1)
                {
                    throw new UnityException(
                        $"\"{type.Name}\" service cant be used at scene \"{sceneName}\"");
                }
            }
#endif
            Service<T>.Register(this as T);
            OnCreateService();
        }

        private void OnDestroy()
        {
            if (Service<T>.IsRegistered)
            {
                OnDestroyService();
                Service<T>.Unregister(this as T);
            }
        }

        /// <summary>
        /// Replacement of Awake method, will be raised only once for service.
        /// Dont use Awake method in inherited classes!
        /// </summary>
        protected abstract void OnCreateService();

        /// <summary>
        /// Replacement of OnDestroy method, will be raised only once for service.
        /// Dont use OnDestroy method in inherited classes!
        /// </summary>
        protected abstract void OnDestroyService();
    }
}