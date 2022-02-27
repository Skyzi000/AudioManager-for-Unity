using Sirenix.OdinInspector;
using UnityEngine;

namespace Skyzi000.AudioManager
{
    /// <summary>
    /// シーン上に置くだけで、AudioManager経由でBGMを流してくれるやつ
    /// </summary>
    public class BGMPlayer : MonoBehaviour
    {
        [SerializeField, Header("流すBGM"), AssetSelector]
        private AudioClip audioClip;

        [SerializeField, Header("音量(通常は1)")]
        private float volume = 1f;

        [SerializeField, Header("ピッチ(再生速度)")]
        private float pitch = 1f;

        [SerializeField, Header("遅延秒数")]
        private float delay;

        [SerializeField, Header("ループ")]
        private bool loop = true;

        [SerializeField, Header("他のBGMの上に重複して再生するか(後から再生する方のオプションが優先される)")]
        private bool allowsDuplicate;

        [SerializeField, Range(0, 256), Header("優先度(0が最高、256が最低)")]
        private int priority = AudioManager.BGMDefaultPriority;

        private void Start()
        {
            if (audioClip != null)
                _ = BGM.Play(audioClip, volume, pitch, delay, loop, allowsDuplicate, priority);
        }
    }
}
