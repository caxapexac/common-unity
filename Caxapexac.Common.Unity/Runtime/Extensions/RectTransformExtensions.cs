using UnityEngine;


namespace Caxapexac.Common.Sharp.Runtime.Extensions.Unity
{
    public static class RectTransformExtensions
    {
        public static void SetPositionX(this RectTransform self, float x)
        {
            self.anchoredPosition = self.anchoredPosition.WithX(x);
        }

        public static void SetPositionY(this RectTransform self, float y)
        {
            self.anchoredPosition = self.anchoredPosition.WithY(y);
        }

        public static void OffsetPositionX(this RectTransform self, float x)
        {
            self.anchoredPosition = self.anchoredPosition.OffsetX(x);
        }

        public static void OffsetPositionY(this RectTransform self, float y)
        {
            self.anchoredPosition = self.anchoredPosition.OffsetY(y);
        }
    }
}