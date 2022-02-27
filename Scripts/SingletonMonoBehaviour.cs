using System;
using UnityEngine;

#nullable enable
namespace Skyzi000.AudioManager
{
    /// <summary>
    /// <see cref="MonoBehaviour"/>を継承し、初期化メソッド<see cref="Init"/>を備え、
    /// <see cref="UnityEngine.Object.DontDestroyOnLoad"/>も自己設定可能なシングルトンクラス
    /// </summary>
    /// <remarks>明示的に<see cref="DontDestroyOnLoadEnabled"/>を<see langword="true"/>にしない限りDontDestroyにはならない</remarks>
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        /// <summary>
        /// <see cref="UnityEngine.Object.DontDestroyOnLoad"/>を設定するかどうかのフラグ。<br/>
        /// このフラグを有効にする場合は、継承先クラスで次のようにオーバーライドする。
        /// <code>
        /// [field: SerializeField]
        /// protected override bool DontDestroyOnLoadEnabled { get; set; } = true;
        /// </code>
        /// </summary>
        /// <remarks>
        /// Inspectorで設定できるようにしたい場合、
        /// 継承先で<code>[field: SerializeField]</code>を付けられるように<see langword="virtual"/>にした。
        /// </remarks>
        [field: NonSerialized]
        protected virtual bool DontDestroyOnLoadEnabled { get; set; } = false;

        private static T? _instance;
        private bool _isInitialized;

        /// <summary>
        /// 唯一のインスタンス
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = (T) FindObjectOfType(typeof(T)); // ここに時間がかかる場合Tagで検索することも検討する
                if (_instance == null)
                {
                    Debug.LogError($"{typeof(T)} is missing.");
                    Debug.LogWarning($"Forcibly generates a {typeof(T)}.");
                    var go = new GameObject(nameof(T), typeof(T));
                    _instance = go.GetComponent(typeof(T)) as T ?? throw new NullReferenceException();
                }
                else
                {
                    _instance.InitIfNeeded();
                }

                return _instance;
            }
        }

        /// <summary>
        /// 初期化(<see cref="Awake"/>か初アクセス時に行われる)
        /// </summary>
        /// <remarks></remarks>
        protected abstract void Init();

        /// <summary>
        /// 初期化処理は<see cref="Init"/>で行う
        /// </summary>
        protected void Awake()
        {
            if (this != Instance) // ここまで呼び出されなかった場合、ここで初期化される
            {
                Destroy(this);
                Debug.LogWarning($"{typeof(T)} は既に他のGameObjectにアタッチされているため、コンポーネントを破棄しました。" +
                                 $"アタッチされているGameObjectは {Instance.gameObject.name} です。");
                return;
            }

            if (DontDestroyOnLoadEnabled) DontDestroyOnLoad(gameObject);
        }

        private void InitIfNeeded()
        {
            if (_isInitialized)
                return;
            Init();
            _isInitialized = true;
        }
    }
}
