using UnityEngine;

namespace Skyzi000.AudioManager
{
    /// <summary>
    /// AudioManagerのBGM関連機能を手軽に使えるようにした便利クラス
    /// </summary>
    public static class BGM
    {
        public static float Volume
        {
            get => AudioManager.Instance.BGMVolume;
            set => AudioManager.Instance.BGMVolume = value;
        }

        /// <inheritdoc cref="AudioManager.PlayBGM(UnityEngine.AudioClip,float,float,float,bool,bool,int)"/>
        public static AudioSource Play(AudioClip audioClip, float volume = 1f, float pitch = 1f, float delay = 0f, bool loop = true, bool allowsDuplicate = false, int priority = AudioManager.BGMDefaultPriority) =>
            AudioManager.Instance.PlayBGM(audioClip, volume, pitch, delay, loop, allowsDuplicate, priority);

        /// <inheritdoc cref="AudioManager.PlayBGM(string,float,float,float,bool,bool,int)"/>
        public static AudioSource Play(string audioClip, float volume = 1f, float pitch = 1f, float delay = 0f, bool loop = true, bool allowsDuplicate = false, int priority = AudioManager.BGMDefaultPriority) =>
            AudioManager.Instance.PlayBGM(audioClip, volume, pitch, delay, loop, allowsDuplicate, priority);

        /// <inheritdoc cref="AudioManager.StopBGM()"/>
        public static void Stop() => AudioManager.Instance.StopBGM();

        /// <inheritdoc cref="AudioManager.StopBGM(UnityEngine.AudioClip)"/>
        public static void Stop(AudioClip audioClip) => AudioManager.Instance.StopBGM(audioClip);

        /// <inheritdoc cref="AudioManager.StopBGM(string)"/>
        public static void Stop(string audioClip) => AudioManager.Instance.StopBGM(audioClip);

        /// <inheritdoc cref="AudioManager.PauseBGM()"/>
        public static void Pause() => AudioManager.Instance.PauseBGM();

        /// <inheritdoc cref="AudioManager.PauseBGM(UnityEngine.AudioClip)"/>
        public static void Pause(AudioClip audioClip) => AudioManager.Instance.PauseBGM(audioClip);

        /// <inheritdoc cref="AudioManager.PauseBGM(string)"/>
        public static void Pause(string audioClip) => AudioManager.Instance.PauseBGM(audioClip);

        /// <inheritdoc cref="AudioManager.UnPauseBGM()"/>
        public static void UnPause() => AudioManager.Instance.UnPauseBGM();

        /// <inheritdoc cref="AudioManager.UnPauseBGM(UnityEngine.AudioClip)"/>
        public static void UnPause(AudioClip audioClip) => AudioManager.Instance.UnPauseBGM(audioClip);

        /// <inheritdoc cref="AudioManager.UnPauseBGM(string)"/>
        public static void UnPause(string audioClip) => AudioManager.Instance.UnPauseBGM(audioClip);

        /// <inheritdoc cref="AudioManager.FindBGMSource(UnityEngine.AudioClip)"/>
        public static AudioSource FindSource(AudioClip audioClip) => AudioManager.Instance.FindBGMSource(audioClip);

        /// <inheritdoc cref="AudioManager.FindBGMSource(string)"/>
        public static AudioSource FindSource(string audioClip) => AudioManager.Instance.FindBGMSource(audioClip);

        /// <inheritdoc cref="AudioManager.LoadBGM(string)"/>
        public static AudioClip Load(string audioClipName) => AudioManager.LoadBGM(audioClipName);
    }
}
