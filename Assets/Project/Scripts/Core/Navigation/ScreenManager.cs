using System.Collections.Generic;
using UnityEngine;

namespace GoFPatterns.Core {
    /// <summary>
    /// 1シーン内で複数画面をパネル切り替えで管理するマネージャー
    /// 画面のスタック管理と遷移を提供する
    /// </summary>
    public class ScreenManager : MonoBehaviour {
        /// <summary>管理対象の全画面</summary>
        [SerializeField]
        private BaseScreen[] screens;

        /// <summary>画面IDをキーとする検索用辞書</summary>
        private readonly Dictionary<string, BaseScreen> screenMap = new Dictionary<string, BaseScreen>();
        /// <summary>画面遷移履歴のスタック</summary>
        private readonly Stack<string> history = new Stack<string>();
        /// <summary>現在表示中の画面</summary>
        private BaseScreen currentScreen;

        /// <summary>シングルトンインスタンス</summary>
        private static ScreenManager instance;
        /// <summary>シングルトンインスタンスを取得する</summary>
        public static ScreenManager Instance => instance;

        private void Awake() {
            if (instance != null && instance != this) {
                Destroy(gameObject);
                return;
            }
            instance = this;

            foreach (var screen in screens) {
                if (screen != null) {
                    screenMap[screen.ScreenId] = screen;
                    screen.gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// 指定IDの画面に遷移する
        /// </summary>
        /// <param name="screenId">遷移先の画面ID</param>
        /// <param name="context">画面に渡すコンテキストデータ</param>
        public void NavigateTo(string screenId, object context = null) {
            if (!screenMap.TryGetValue(screenId, out var nextScreen)) {
                return;
            }
            if (currentScreen != null) {
                history.Push(currentScreen.ScreenId);
                currentScreen.Hide();
            }
            currentScreen = nextScreen;
            currentScreen.Show(context);
        }

        /// <summary>
        /// 前の画面に戻る
        /// </summary>
        public void GoBack() {
            if (history.Count == 0) {
                return;
            }
            string previousId = history.Pop();
            if (currentScreen != null) {
                currentScreen.Hide();
            }
            if (screenMap.TryGetValue(previousId, out var previousScreen)) {
                currentScreen = previousScreen;
                currentScreen.Show(null);
            }
        }

        /// <summary>
        /// 遷移履歴があるかどうか
        /// </summary>
        /// <returns>戻れる場合true</returns>
        public bool CanGoBack() {
            return history.Count > 0;
        }

        private void OnDestroy() {
            if (instance == this) {
                instance = null;
            }
        }
    }
}
