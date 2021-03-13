using UnityEngine;


namespace Caxapexac.Common.Sharp.Runtime.Extensions.Unity
{
    public static class GameObjectExtensions
    {
        public static T EnsureComponent<T>(this GameObject self) where T : Component
        {
            return self.GetComponent<T>() ?? self.AddComponent<T>();
        }

        public static bool HasComponent<T>(this GameObject self) where T : Component
        {
            return self.GetComponent<T>() != null;
        }

        public static void DestroyChildren(this GameObject self)
        {
            Transform goTransform = self.transform;
            for (int i = goTransform.childCount - 1; i >= 0; i--)
            {
                Object.DestroyImmediate(goTransform.GetChild(i).gameObject);
            }
        }

        public static T Clone<T>(this GameObject self, Transform parent, string name = "")
        {
            GameObject instance = Object.Instantiate(self, parent, false);
            if (!string.IsNullOrEmpty(name))
            {
                instance.name = name;
            }
            return instance.GetComponent<T>();
        }
    }
}