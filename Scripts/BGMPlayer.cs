using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Skyzi000.AudioManager
{
    /// <summary>
    /// シーン上に置くだけで、AudioManagerを利用してBGMを流してくれるやつ
    /// </summary>
    public class BGMPlayer : MonoBehaviour
    {
        /// <summary>
        /// 再生するBGM
        /// </summary>
        public AudioClip AudioClip
        {
            get => audioClip;
            set
            {
                audioClip = value;
                OnAudioClipChanged.Invoke(audioClip);
            }
        }

        /// <summary>
        /// 再生に利用している、<see cref="AudioManager"/>管理下の<see cref="AudioSource"/>
        /// </summary>
        /// <remarks>一度も再生していなければnull</remarks>
        public AudioSource AudioSource { get; private set; }

        [field: SerializeField, Header("AudioClip変更時イベント"), PropertyOrder(10)]
        public UnityEvent<AudioClip> OnAudioClipChanged { get; private set; } = new UnityEvent<AudioClip>();


        [SerializeField, Header("BGM"), AssetSelector, Required]
        private AudioClip audioClip;

        [SerializeField, Header("音量")]
        private float volume = 1f;

        [SerializeField, Header("ピッチ(再生速度)")]
        private float pitch = 1f;

        [SerializeField, Header("遅延秒数"), Min(0f)]
        private float delay;

        [SerializeField, Header("ループ")]
        private bool loop = true;

        [SerializeField, Header("他のBGMの上に重複して再生するか(後から再生する方のオプションが優先される)")]
        private bool allowsDuplicate;

        [SerializeField, Range(0, 256), Header("優先度(0が最高、256が最低)")]
        private int priority = AudioManager.BGMDefaultPriority;


        public void Play()
        {
            if (AudioClip != null)
                AudioSource = BGM.Play(AudioClip, volume, pitch, delay, loop, allowsDuplicate, priority);
        }

        public void Pause()
        {
            // isPlayingをtrueに保つためAudioSource.Pause()は使っていない
            if (AudioClip != null && AudioSource != null)
                AudioSource.pitch = 0f;
        }

        public void UnPause()
        {
            if (AudioClip != null && AudioSource != null)
                AudioSource.pitch = pitch;
        }

        public void Stop()
        {
            if (AudioClip != null)
                BGM.Stop(AudioClip);
        }

        private void OnEnable()
        {
            if (AudioSource == null || AudioSource.clip != AudioClip)
            {
                Play();
            }
            else
            {
                UnPause();
            }
        }

        private void OnDisable() => Pause();

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!UnityEditor.EditorApplication.isPlaying)
                return;
            if (AudioSource == null || AudioSource.clip != AudioClip)
                Play();
            if (AudioSource == null)
                return;
            AudioSource.volume = volume;
            AudioSource.pitch = pitch;
            AudioSource.loop = loop;
            AudioSource.priority = priority;
        }
#endif
    }
}
