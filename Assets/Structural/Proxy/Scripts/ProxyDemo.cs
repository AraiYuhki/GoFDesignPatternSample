using UnityEngine;
using UnityEngine.UI;

namespace DesignPatterns.Structural.Proxy {
    /// <summary>
    /// Proxyパターンのデモシーンを制御するクラス
    ///
    /// 【デモの内容】
    /// - プロキシ経由で画像をロードし、遅延ロードの効果を確認
    /// - 初回表示時のみロードが発生し、2回目以降はキャッシュから表示
    /// - プロキシの状態確認で未ロード/ロード済みの違いを体験
    /// </summary>
    public sealed class ProxyDemo : PatternDemoBase {
        /// <summary>画像1表示ボタン</summary>
        [SerializeField]
        private Button displayImage1Button;

        /// <summary>画像2表示ボタン</summary>
        [SerializeField]
        private Button displayImage2Button;

        /// <summary>画像3表示ボタン</summary>
        [SerializeField]
        private Button displayImage3Button;

        /// <summary>全画像の状態確認ボタン</summary>
        [SerializeField]
        private Button checkStatusButton;

        /// <summary>プロキシ経由の画像群</summary>
        private IImage[] images;

        /// <inheritdoc/>
        protected override string PatternName {
            get { return "Proxy"; }
        }

        /// <inheritdoc/>
        protected override PatternCategory Category {
            get { return PatternCategory.Structural; }
        }

        /// <inheritdoc/>
        protected override string Description {
            get { return "オブジェクトへのアクセスを制御する代理を提供する。ここでは遅延ロードを実現する"; }
        }

        /// <inheritdoc/>
        protected override void OnDemoStart() {
            InGameLogger.Log("--- プロキシを作成（画像はまだロードされない） ---", LogColor.Yellow);
            images = new IImage[] {
                new ImageProxy("hero_portrait.png", 2048),
                new ImageProxy("world_map.png", 8192),
                new ImageProxy("title_screen.png", 4096)
            };

            if (displayImage1Button != null) {
                displayImage1Button.onClick.AddListener(() => DisplayImage(0));
            }
            if (displayImage2Button != null) {
                displayImage2Button.onClick.AddListener(() => DisplayImage(1));
            }
            if (displayImage3Button != null) {
                displayImage3Button.onClick.AddListener(() => DisplayImage(2));
            }
            if (checkStatusButton != null) {
                checkStatusButton.onClick.AddListener(OnCheckStatus);
            }

            InGameLogger.Log("画像を表示すると初めてロードされます", LogColor.Yellow);
        }

        /// <summary>
        /// 指定インデックスの画像を表示する
        /// </summary>
        /// <param name="index">画像のインデックス</param>
        private void DisplayImage(int index) {
            InGameLogger.Log($"--- 画像 {index + 1} を表示 ---", LogColor.Yellow);
            images[index].Display();
        }

        /// <summary>
        /// 全画像の状態を表示する
        /// </summary>
        private void OnCheckStatus() {
            InGameLogger.Log("=== 全画像の状態 ===", LogColor.Yellow);
            for (int i = 0; i < images.Length; i++) {
                InGameLogger.Log($"  [{i + 1}] {images[i].GetInfo()}", LogColor.Green);
            }
        }
    }
}
