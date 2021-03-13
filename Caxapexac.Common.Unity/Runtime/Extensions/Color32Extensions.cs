using UnityEngine;


namespace Caxapexac.Common.Sharp.Runtime.Extensions.Unity
{
    public static class Color32Extensions
    {
        public static string ToHex(this Color32 self)
        {
            string hex = self.r.ToString("X2") + self.g.ToString("X2") + self.b.ToString("X2") + self.a.ToString("X2");
            return hex;
        }

        public static string ColorWithNewAlpha(this string hex, float alpha)
        {
            Color col = hex.HexToColor();
            col.a = alpha;
            return ToHex(col);
        }
    }
}