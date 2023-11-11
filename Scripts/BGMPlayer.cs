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

        [field: SerializeField, Tooltip("AudioClip変更時イベント"), PropertyOrder(10)]
        public UnityEvent<AudioClip> OnAudioClipChanged { get; private set; } = new UnityEvent<AudioClip>();

        [FolderPath, SerializeField, Tooltip("AudioClipのあるディレクトリ（AssetListの絞り込みに利用）")]
        private string directoryPath = "";
        
        [SerializeField, AssetList(CustomFilterMethod = nameof(DirectoryPathFilter)), RequiredIn(PrefabKind.InstanceInScene), InlineEditor(InlineEditorModes.LargePreview)]
        private AudioClip audioClip;

        [SerializeField, Tooltip("音量")]
        private float volume = 1f;

        [SerializeField, Tooltip("ピッチ(再生速度)")]
        private float pitch = 1f;

        [SerializeField, Tooltip("遅延秒数"), Min(0f), Unit(Units.Second)]
        private float delay;

        [SerializeField, Tooltip("ループ")]
        private bool loop = true;

        [SerializeField, Tooltip("他のBGMの上に重複して再生するか(後から再生する方のオプションが優先される)")]
        private bool allowsDuplicate;

        [SerializeField, Range(0, 256), Tooltip("優先度(0が最高、256が最低)")]
        private int priority = AudioManager.BGMDefaultPriority;

        public bool CanPlay => Application.isPlaying && AudioClip != null && BGM.FindSource(AudioClip) == null;
        public bool CanPause => Application.isPlaying && AudioClip != null && AudioSource != null && AudioSource.isPlaying && !Mathf.Approximately(AudioSource.pitch, 0f);
        public bool CanUnPause => Application.isPlaying && AudioClip != null && AudioSource != null && AudioSource.isPlaying && Mathf.Approximately(AudioSource.pitch, 0f);
        public bool CanStop => Application.isPlaying && AudioClip != null && AudioSource != null && AudioSource.isPlaying;

        private bool DirectoryPathFilter(Object obj)
            => string.IsNullOrEmpty(directoryPath)
#if UNITY_EDITOR
               || UnityEditor.AssetDatabase.GetAssetPath(obj).StartsWith(directoryPath)
#endif
        ;
        
        [Button(ButtonSizes.Large, Icon = SdfIconType.PlayFill), HideLabel, ButtonGroup, ShowIf(nameof(CanPlay), false)]
        public void Play()
        {
            if (AudioClip != null)
                AudioSource = BGM.Play(AudioClip, volume, pitch, delay, loop, allowsDuplicate, priority);
        }

        [Button(ButtonSizes.Large, Icon = SdfIconType.PauseFill), HideLabel, ButtonGroup, ShowIf(nameof(CanPause), false)]
        public void Pause()
        {
            // isPlayingをtrueに保つためAudioSource.Pause()は使っていない
            if (AudioClip != null && AudioSource != null)
                AudioSource.pitch = 0f;
        }

        [Button(ButtonSizes.Large, Icon = SdfIconType.PlayCircle), HideLabel, ButtonGroup, ShowIf(nameof(CanUnPause), false)]
        public void UnPause()
        {
            if (AudioClip != null && AudioSource != null)
                AudioSource.pitch = pitch;
        }

        [Button(ButtonSizes.Large, Icon = SdfIconType.StopFill), HideLabel, ButtonGroup, ShowIf(nameof(CanStop), false)]
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
