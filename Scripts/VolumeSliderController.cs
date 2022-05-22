using System;
using UnityEngine;
using UnityEngine.UI;

namespace Skyzi000.AudioManager
{
    /// <summary>
    /// Sliderの名前をもとに、自動的にボリュームの種類を判別し、Sliderと同期して音量を変更するクラス。Slider自体にAddComponentして使う。
    /// </summary>
    [RequireComponent(typeof(Slider))]
    public class VolumeSliderController : MonoBehaviour
    {
        /// <summary>
        /// 何のボリュームか。(Voiceは未実装)
        /// </summary>
        public enum VolumeType
        {
            BGM,
            SE,
            Voice,
            Master
        }

        private VolumeType _volumeType;
        private Slider _volumeSlider;
        private bool _isInitialized;

        private void Start()
        {
            Init();
            _volumeSlider.onValueChanged.AddListener(ChangeVolume);
        }

        private void Init()
        {
            _volumeSlider = GetComponent<Slider>();
            // 自分が何のスライダーかの識別
            if (_volumeSlider.transform.name.Contains("BGM"))
            {
                _volumeType = VolumeType.BGM;
                _volumeSlider.value = BGM.Volume;
            }
            else if (_volumeSlider.transform.name.Contains("SE"))
            {
                _volumeType = VolumeType.SE;
                _volumeSlider.value = SE.Volume;
            }
            else if (_volumeSlider.transform.name.IndexOf("Voice", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                _volumeType = VolumeType.Voice;
                Debug.LogWarning("Voice volume control is not yet implemented.\n" +
                                 "Voiceボリュームの管理機能は未実装です。");
            }
            else if (_volumeSlider.transform.name.IndexOf("Master", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                _volumeType = VolumeType.Master;
                _volumeSlider.value = AudioManager.Instance.MasterVolume;
            }
            else
                Debug.LogError("Include one of [BGM],[SE],[Voice],[Master] (Voice and Master are not case sensitive) in the Slider name.\n" +
                               "Sliderの名前に[BGM],[SE],[Voice],[Master](Voice, Masterの大文字小文字の区別は無し)のいずれかを含めてください。");

            _isInitialized = true;
        }

        private void ChangeVolume(float volume)
        {
            if (!_isInitialized)
                return;
            if (AudioManager.Instance.Mixer == null)
                throw new InvalidOperationException($"{nameof(AudioManager.Instance.Mixer)} is null");
            switch (_volumeType)
            {
                case VolumeType.BGM:
                    BGM.Volume = volume;
                    break;
                case VolumeType.SE:
                    SE.Volume = volume;
                    break;
                case VolumeType.Voice:
                    // TODO: ここにVoice音量の設定処理を書く
                    break;
                case VolumeType.Master:
                    AudioManager.Instance.MasterVolume = volume;
                    break;
                default:
                    Debug.LogError("Include one of [BGM],[SE],[Voice],[Master] (Voice and Master are not case sensitive) in the Slider name.\n" +
                                   "Sliderの名前に[BGM],[SE],[Voice],[Master](Voice, Masterの大文字小文字の区別は無し)のいずれかを含めてください。");
                    break;
            }
        }
    }
}
