using UnityEngine;

namespace Skyzi000.AudioManager
{
    public static class AudioExtensions
    {
        /// <summary>
        /// 0 ~ 1の値をdB( デシベル )に変換
        /// </summary>
        public static float ConvertVolume2Db(float volume) =>
            volume < 0.01f ? -80f : 20f * Mathf.Log10(Mathf.Clamp01(volume));

        /// <summary>
        /// dBを0~1の値に変換
        /// </summary>
        public static float ConvertDb2Volume(float decibel) =>
            Mathf.Clamp01(Mathf.Pow(10f, decibel / 20f));
    }
}
