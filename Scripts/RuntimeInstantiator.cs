using UnityEngine;

namespace Skyzi000.AudioManager
{
    public static class RuntimeInstantiator
    {
        /// <summary>
        /// ゲーム開始時、自動的にオブジェクトを生成する
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InstantiateObjectOnLoad()
        {
            var prefab = Resources.Load<GameObject>(nameof(AudioManager));
            Object.Instantiate(prefab);
        }
    }
}
