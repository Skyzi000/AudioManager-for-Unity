using UnityEngine;

namespace Skyzi000.AudioManager
{
    /// <summary>
    /// AudioManagerのSE関連機能を手軽に使えるようにした便利クラス
    /// </summary>
    public static class SE
    {
        /// <inheritdoc cref="AudioManager.PlaySE(UnityEngine.AudioClip,float,float,float,bool,bool,int)"/>
        public static AudioSource Play(AudioClip audioClip, float volume = 1f, float pitch = 1f, float delay = 0f, bool loop = false, bool allowsDuplicate = true, int priority = AudioManager.SEDefaultPriority) =>
            AudioManager.Instance.PlaySE(audioClip, volume, pitch, delay, loop, allowsDuplicate, priority);

        /// <inheritdoc cref="AudioManager.PlaySE(string,float,float,float,bool,bool,int)"/>
        public static AudioSource Play(string audioClip, float volume = 1f, float pitch = 1f, float delay = 0f, bool loop = false, bool allowsDuplicate = true, int priority = AudioManager.SEDefaultPriority) =>
            AudioManager.Instance.PlaySE(audioClip, volume, pitch, delay, loop, allowsDuplicate, priority);

        /// <inheritdoc cref="AudioManager.StopSE()"/>
        public static void Stop() => AudioManager.Instance.StopSE();

        /// <inheritdoc cref="AudioManager.StopSE(UnityEngine.AudioClip)"/>
        public static void Stop(AudioClip audioClip) => AudioManager.Instance.StopSE(audioClip);

        /// <inheritdoc cref="AudioManager.StopSE(string)"/>
        public static void Stop(string audioClip) => AudioManager.Instance.StopSE(audioClip);

        /// <inheritdoc cref="AudioManager.PauseSE()"/>
        public static void Pause() => AudioManager.Instance.PauseSE();

        /// <inheritdoc cref="AudioManager.PauseSE(UnityEngine.AudioClip)"/>
        public static void Pause(AudioClip audioClip) => AudioManager.Instance.PauseSE(audioClip);

        /// <inheritdoc cref="AudioManager.PauseSE(string)"/>
        public static void Pause(string audioClip) => AudioManager.Instance.PauseSE(audioClip);

        /// <inheritdoc cref="AudioManager.UnPauseSE()"/>
        public static void UnPause() => AudioManager.Instance.UnPauseSE();

        /// <inheritdoc cref="AudioManager.UnPauseSE(UnityEngine.AudioClip)"/>
        public static void UnPause(AudioClip audioClip) => AudioManager.Instance.UnPauseSE(audioClip);

        /// <inheritdoc cref="AudioManager.UnPauseSE(string)"/>
        public static void UnPause(string audioClip) => AudioManager.Instance.UnPauseSE(audioClip);

        /// <inheritdoc cref="AudioManager.FindSESource(UnityEngine.AudioClip)"/>
        public static AudioSource FindSource(AudioClip audioClip) => AudioManager.Instance.FindSESource(audioClip);

        /// <inheritdoc cref="AudioManager.FindSESource(string)"/>
        public static AudioSource FindSource(string audioClip) => AudioManager.Instance.FindSESource(audioClip);

        /// <inheritdoc cref="AudioManager.LoadSE(string)"/>
        public static AudioClip Load(string audioClipName) => AudioManager.LoadSE(audioClipName);
    }
}
