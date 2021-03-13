using UnityEngine;


namespace Caxapexac.Common.Sharp.Runtime.Services.Audio
{
    public static class SoundHelper
    {
        public static float GetSoundVolume(float decimalVolume)
        {
            return Mathf.Log10(decimalVolume) * 20;
        }

        public static float GetNormalVolume(float soundVolume)
        {
            return Mathf.Pow(10, soundVolume / 20);
        }
    }
}