﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace Skyzi000.AudioManager
{
    /// <summary>
    /// <see cref="AudioMixer"/>とDOTweenの力を借りて強くなった自作<see cref="AudioManager"/>からDOTweenを取り除いた。
    /// </summary>
    public class AudioManager : SingletonMonoBehaviour<AudioManager>
    {
        [field: SerializeField]
        protected override bool DontDestroyOnLoadEnabled { get; set; } = true;

        [field: SerializeField, Required]
        public AudioMixer Mixer { get; private set; }

        [field: SerializeField, Required]
        public AudioMixerGroup BGMGroup { get; private set; }

        [field: SerializeField, Required]
        public AudioMixerGroup SEGroup { get; private set; }

        /// <summary>
        /// BGMを全て一時停止しているか
        /// </summary>
        public bool IsPauseBGM { get; private set; } = false;

        [field: SerializeField, Header("UnityEvents")]
        public UnityEvent OnPauseBGM { get; private set; } = new UnityEvent();

        [field: SerializeField]
        public UnityEvent OnUnPauseBGM { get; private set; } = new UnityEvent();

        /// <summary>
        /// SEを全て一時停止しているか
        /// </summary>
        public bool IsPauseSE { get; private set; } = false;

        [field: SerializeField]
        public UnityEvent OnPauseSE { get; private set; } = new UnityEvent();

        [field: SerializeField]
        public UnityEvent OnUnPauseSE { get; private set; } = new UnityEvent();

        [SerializeField, Header("Maximum number that can be played at one time")]
        private int bgmSourceNum = 2;

        [SerializeField]
        private int seSourceNum = 8;

        [ShowInInspector, ReadOnly, Header("Default priority of AudioSource")]
        public const int BGMDefaultPriority = 64;

        [ShowInInspector, ReadOnly]
        public const int SEDefaultPriority = 128;

        [ShowInInspector, ReadOnly, Header("The path to read under the Resources folder")]
        private static string _bgmDirectory = "BGM";

        [ShowInInspector, ReadOnly]
        private static string _seDirectory = "SE";

        private GameObject _parentBGMSource;
        private GameObject _parentSESource;
        private readonly List<AudioSource> _bgmSources = new List<AudioSource>();
        private readonly List<AudioSource> _seSources = new List<AudioSource>();


        protected override void Init()
        {
            _parentBGMSource = new GameObject("BGMSources");
            _parentBGMSource.transform.SetParent(transform);
            _parentSESource = new GameObject("SESources");
            _parentSESource.transform.SetParent(transform);
            for (var i = 0; i < bgmSourceNum; i++)
            {
                var bgmSource = _parentBGMSource.AddComponent<AudioSource>();
                bgmSource.outputAudioMixerGroup = BGMGroup;
                _bgmSources.Add(bgmSource);
            }

            for (var i = 0; i < seSourceNum; i++)
            {
                var seSource = _parentSESource.AddComponent<AudioSource>();
                seSource.outputAudioMixerGroup = SEGroup;
                _seSources.Add(seSource);
            }
        }


        #region BGM

        /// <summary>
        /// BGMを再生する
        /// </summary>
        /// <param name="audioClip">再生するBGM</param>
        /// <param name="volume">音量</param>
        /// <param name="pitch">ピッチ(再生速度)</param>
        /// <param name="delay">遅延秒数</param>
        /// <param name="loop">ループ</param>
        /// <param name="allowsDuplicate">他のBGMの上に重複して再生するか(後から再生する方のオプションが優先される)</param>
        /// <param name="priority">優先度(0が最高、256が最低)</param>
        /// <returns>再生に利用したAudioSource</returns>
        public AudioSource PlayBGM(AudioClip audioClip,
            float volume = 1f,
            float pitch = 1f,
            float delay = 0f,
            bool loop = true,
            bool allowsDuplicate = false,
            int priority = BGMDefaultPriority) =>
            Play(_bgmSources, audioClip, volume, pitch, delay, loop, allowsDuplicate, priority, IsPauseBGM);

        /// <inheritdoc cref="PlayBGM(UnityEngine.AudioClip,float,float,float,bool,bool,int)"/>
        public AudioSource PlayBGM(string audioClip,
            float volume = 1f,
            float pitch = 1f,
            float delay = 0f,
            bool loop = true,
            bool allowsDuplicate = false,
            int priority = BGMDefaultPriority) =>
            PlayBGM(LoadBGM(audioClip), volume, pitch, delay, loop, allowsDuplicate, priority);

        /// <summary>
        /// 全てのBGMを停止する
        /// </summary>
        public void StopBGM() => StopAll(_bgmSources);

        /// <summary>
        /// 指定したBGMを停止する
        /// </summary>
        public void StopBGM(AudioClip audioClip)
        {
            if (audioClip != null)
                FindBGMSource(audioClip)?.Stop();
        }

        /// <inheritdoc cref="StopBGM(UnityEngine.AudioClip)"/>
        public void StopBGM(string audioClip) => StopBGM(LoadBGM(audioClip));

        /// <summary>
        /// 全てのBGMを一時停止する
        /// </summary>
        public void PauseBGM()
        {
            IsPauseBGM = true;
            OnPauseBGM.Invoke();
            PauseAll(_bgmSources);
        }

        /// <summary>
        /// 全てのBGMの一時停止を解除する
        /// </summary>
        public void UnPauseBGM()
        {
            IsPauseBGM = false;
            OnUnPauseBGM.Invoke();
            UnPauseAll(_bgmSources);
        }

        /// <summary>
        /// 指定したBGMを一時停止する
        /// </summary>
        public void PauseBGM(AudioClip audioClip)
        {
            if (audioClip != null)
                FindBGMSource(audioClip)?.Pause();
        }

        /// <summary>
        /// 指定したBGMの一時停止を解除する
        /// </summary>
        public void UnPauseBGM(AudioClip audioClip)
        {
            if (audioClip != null)
                FindBGMSource(audioClip)?.UnPause();
        }

        /// <inheritdoc cref="PauseBGM(UnityEngine.AudioClip)"/>
        public void PauseBGM(string audioClip) => PauseBGM(LoadBGM(audioClip));

        /// <inheritdoc cref="UnPauseBGM(UnityEngine.AudioClip)"/>
        public void UnPauseBGM(string audioClip) => UnPauseBGM(LoadBGM(audioClip));

        /// <summary>
        /// 指定したBGMを再生中の<see cref="AudioSource"/>を探す
        /// </summary>
        public AudioSource FindBGMSource(AudioClip audioClip) =>
            audioClip == null ? null : _bgmSources.Find(s => s.clip == audioClip && s.isPlaying);

        /// <inheritdoc cref="FindBGMSource(UnityEngine.AudioClip)"/>
        public AudioSource FindBGMSource(string audioClip) => FindBGMSource(LoadBGM(audioClip));

        /// <summary>
        /// ResourcesフォルダにあるBGMを読み込む(存在しなければnull)
        /// </summary>
        public static AudioClip LoadBGM(string audioClipName) => LoadAudioClip(_bgmDirectory, audioClipName);

        #endregion


        #region SE

        /// <summary>
        /// SEを再生する
        /// </summary>
        /// <param name="audioClip">再生するSE</param>
        /// <param name="volume">音量</param>
        /// <param name="pitch">ピッチ(再生速度)</param>
        /// <param name="delay">遅延秒数</param>
        /// <param name="loop">ループ</param>
        /// <param name="allowsDuplicate">他のSEの上に重複して再生するか(後から再生する方のオプションが優先される)</param>
        /// <param name="priority">優先度(0が最高、256が最低)</param>
        /// <returns>再生に利用したAudioSource</returns>
        public AudioSource PlaySE(AudioClip audioClip,
            float volume = 1f,
            float pitch = 1f,
            float delay = 0f,
            bool loop = false,
            bool allowsDuplicate = true,
            int priority = SEDefaultPriority) =>
            Play(_seSources, audioClip, volume, pitch, delay, loop, allowsDuplicate, priority, IsPauseSE);

        /// <inheritdoc cref="PlaySE(UnityEngine.AudioClip,float,float,float,bool,bool,int)"/>
        public AudioSource PlaySE(string audioClip,
            float volume = 1f,
            float pitch = 1f,
            float delay = 0f,
            bool loop = false,
            bool allowsDuplicate = true,
            int priority = SEDefaultPriority) =>
            PlaySE(LoadSE(audioClip), volume, pitch, delay, loop, allowsDuplicate, priority);

        /// <summary>
        /// 全てのSEを停止する
        /// </summary>
        public void StopSE() => StopAll(_seSources);

        /// <summary>
        /// 指定したSEを停止する
        /// </summary>
        public void StopSE(AudioClip audioClip)
        {
            if (audioClip != null)
                FindSESource(audioClip)?.Stop();
        }

        /// <inheritdoc cref="StopSE(UnityEngine.AudioClip)"/>
        public void StopSE(string audioClip) => StopSE(LoadSE(audioClip));

        /// <summary>
        /// 全てのSEを一時停止する
        /// </summary>
        public void PauseSE()
        {
            IsPauseSE = true;
            OnPauseSE.Invoke();
            PauseAll(_seSources);
        }

        /// <summary>
        /// 全てのSEの一時停止を解除する
        /// </summary>
        public void UnPauseSE()
        {
            IsPauseSE = false;
            OnUnPauseSE.Invoke();
            UnPauseAll(_seSources);
        }

        /// <summary>
        /// 指定したSEを一時停止する
        /// </summary>
        public void PauseSE(AudioClip audioClip)
        {
            if (audioClip != null)
                FindSESource(audioClip)?.Pause();
        }

        /// <summary>
        /// 指定したSEの一時停止を解除する
        /// </summary>
        public void UnPauseSE(AudioClip audioClip)
        {
            if (audioClip != null)
                FindSESource(audioClip)?.UnPause();
        }

        /// <inheritdoc cref="PauseSE(UnityEngine.AudioClip)"/>
        public void PauseSE(string audioClip) => PauseSE(LoadSE(audioClip));

        /// <inheritdoc cref="UnPauseSE(UnityEngine.AudioClip)"/>
        public void UnPauseSE(string audioClip) => UnPauseSE(LoadSE(audioClip));

        /// <summary>
        /// 指定したSEを再生中の<see cref="AudioSource"/>を探す
        /// </summary>
        public AudioSource FindSESource(AudioClip audioClip) =>
            audioClip == null ? null : _seSources.Find(s => s.clip == audioClip && s.isPlaying);

        /// <inheritdoc cref="FindSESource(UnityEngine.AudioClip)"/>
        public AudioSource FindSESource(string audioClip) => FindSESource(LoadSE(audioClip));

        /// <summary>
        /// ResourcesフォルダにあるSEを読み込む(存在しなければnull)
        /// </summary>
        public static AudioClip LoadSE(string audioClipName) => LoadAudioClip(_seDirectory, audioClipName);

        #endregion


        /// <summary>
        /// 管理下にある全てのオーディオを停止する
        /// </summary>
        public void StopAll()
        {
            StopBGM();
            StopSE();
        }

        /// <summary>
        /// 管理下にある全てのオーディオを一時停止する
        /// </summary>
        public void PauseAll()
        {
            PauseBGM();
            PauseSE();
        }

        /// <summary>
        /// 管理下にある全てのオーディオの一時停止を解除する
        /// </summary>
        public void UnPauseAll()
        {
            UnPauseBGM();
            UnPauseSE();
        }

        private static AudioClip LoadAudioClip(string directory, string path) => Resources.Load<AudioClip>(Path.Combine(directory, path));

        private static void StopAll(List<AudioSource> sources) => sources.ForEach(s => s.Stop());

        // ReSharper disable once SuggestBaseTypeForParameter
        private static void StopAll(List<AudioSource> sources, AudioSource ignoreSource) => sources.ForEach(s =>
        {
            if (s != ignoreSource)
                s.Stop();
        });

        private static void PauseAll(List<AudioSource> sources) => sources.ForEach(Pause);

        private static void UnPauseAll(List<AudioSource> sources) => sources.ForEach(UnPause);

        // AudioSource.Pause()で一時停止してしまうと、終了検知にisPlayingを使ってるところで誤検知してしまうため、その回避策としてpitchで制御している
        private static void Pause(AudioSource source) => source.pitch = 0f;

        // 一時停止時のpitchは覚えてないため、復元できないが、現状pitchを使うことはないため問題はない(？)
        private static void UnPause(AudioSource source) => source.pitch = 1f;

        private static IEnumerator DelayCoroutine(float delay, Action callback)
        {
            yield return new WaitForSeconds(delay);
            callback?.Invoke();
        }

        private static AudioSource Play(List<AudioSource> audioSources,
            AudioClip audioClip,
            float volume,
            float pitch,
            float delay,
            bool loop,
            bool allowsDuplicate,
            int priority,
            bool isPause)
        {
            if (audioSources == null)
                throw new ArgumentNullException(nameof(audioSources));
            if (audioSources.Count == 0)
                throw new ArgumentException("Value cannot be an empty collection.", nameof(audioSources));
            if (audioClip == null)
                throw new ArgumentNullException(nameof(audioClip));

            delay = Mathf.Max(0f, delay);
            AudioSource playSource;
            if (allowsDuplicate)
            {
                // 重複再生して良いなら
                playSource = audioSources.FirstOrDefault(s => !s.isPlaying);

                // 全部再生中だったとき
                if (playSource == null)
                {
                    playSource = audioSources.OrderByDescending(s => s.priority)
                        .ThenByDescending(s => s.time)
                        .FirstOrDefault();
                    if (playSource != null)
                    {
                        playSource.Stop();
                    }
                    else
                    {
                        playSource = audioSources[0];
                    }
                }
            }
            else
            {
                // 重複再生がだめなら
                playSource = audioSources.Find(s => s.isPlaying && s.clip == audioClip);

                // すでに同じものが再生中であればそのSourceを返す
                if (playSource != null)
                {
                    playSource.volume = volume;
                    playSource.pitch = pitch;
                    playSource.loop = loop;
                    playSource.priority = priority;
                    return playSource;
                }

                if (playSource == null)
                    playSource = audioSources[0];

                if (Mathf.Approximately(delay, 0f))
                    StopAll(audioSources);
                else
                    Instance.StartCoroutine(DelayCoroutine(delay, () => StopAll(audioSources, playSource)));
            }

            playSource.clip = audioClip;
            playSource.volume = volume;
            playSource.pitch = pitch;
            playSource.loop = loop;
            playSource.priority = priority;

            if (Mathf.Approximately(delay, 0f))
                playSource.Play();
            else
                playSource.PlayDelayed(delay);

            if (isPause)
                Pause(playSource);
            return playSource;
        }
    }
}