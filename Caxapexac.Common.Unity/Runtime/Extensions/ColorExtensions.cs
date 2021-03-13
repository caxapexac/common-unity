using System.Globalization;
using UnityEngine;


namespace Caxapexac.Common.Sharp.Runtime.Extensions.Unity
{
    public static class ColorExtensions
    {
        public static string ToHex(this Color self)
        {
            return $"#{(int)(self.r * 255):X2}{(int)(self.g * 255):X2}{(int)(self.b * 255):X2}";
        }

        public static Color HexToColor(this string hex)
        {
            hex = hex.Replace("0x", ""); //in case the string is formatted 0xFFFFFF
            hex = hex.Replace("#", ""); //in case the string is formatted #FFFFFF
            byte a = 255; //assume fully visible unless specified in hex
            byte r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);

            //Only use alpha if the string has enough characters

            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);
            }
            return new Color32(r, g, b, a);
        }

        public static float[] ToFloats(this Color self)
        {
            return new[] {self.r, self.g, self.b, self.a};
        }
        
        public static Color ToColor(this float[] self)
        {
            return new Color(self[0], self[1], self[2], self[3]);
        }

        public static Color WithAlphaSetTo(this Color self, float a)
        {
            return new Color(self.r, self.g, self.b, a);
        }

        public static Color BrightnessOffset(this Color color, float offset)
        {
            return new Color(color.r + offset, color.g + offset, color.b + offset, color.a);
        }
        
        public static Color RandomColor(this Color color)
        {
            return new Color(Random.Range(.1f, .9f), Random.Range(.1f, .9f), Random.Range(.1f, .9f));
        }

        public static Color RandomBright(this Color color)
        {
            return new Color(Random.Range(.4f, 1), Random.Range(.4f, 1), Random.Range(.4f, 1));
        }

        public static Color RandomDim(this Color color)
        {
            return new Color(Random.Range(.4f, .6f), Random.Range(.4f, .8f), Random.Range(.4f, .8f));
        }
    }
}