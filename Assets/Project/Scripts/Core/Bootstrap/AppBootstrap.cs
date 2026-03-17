using UnityEngine;

namespace GoFPatterns.Core {
    /// <summary>
    /// アプリケーション起動時の初期化処理
    /// 最初にパターン一覧画面を表示する
    /// </summary>
    public class AppBootstrap : MonoBehaviour {
        /// <summary>起動時に表示する画面のID</summary>
        [SerializeField]
        private string initialScreenId = "list";

        private void Start() {
            if (ScreenManager.Instance != null) {
                ScreenManager.Instance.NavigateTo(initialScreenId);
            }
        }
    }
}
