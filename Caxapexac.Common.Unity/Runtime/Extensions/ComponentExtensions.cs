using UnityEngine;


namespace Caxapexac.Common.Sharp.Runtime.Extensions.Unity
{
    public static class ComponentExtensions
    {
        public static T AddComponent<T>(this Component self) where T : Component
        {
            return self.gameObject.AddComponent<T>();
        }

        public static T EnsureComponent<T>(this Component self) where T : Component
        {
            return self.GetComponent<T>() ?? self.AddComponent<T>();
        }

        public static bool HasComponent<T>(this Component self) where T : Component
        {
            return self.GetComponent<T>() != null;
        }
    }
}