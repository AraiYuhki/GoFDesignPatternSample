using UnityEngine;
using UnityEngine.SceneManagement;

namespace DesignPatterns {
    /// <summary>
    /// シーン遷移を管理するユーティリティクラス
    /// 各デモシーンからメインメニューへの戻りなどに使用する
    /// </summary>
    public static class SceneNavigator {
        /// <summary>メインメニューシーンの名前</summary>
        private const string MainMenuSceneName = "MainMenu";

        /// <summary>
        /// 指定した名前のシーンへ遷移する
        /// </summary>
        /// <param name="sceneName">遷移先のシーン名</param>
        public static void LoadScene(string sceneName) {
            SceneManager.LoadScene(sceneName);
        }

        /// <summary>
        /// メインメニューへ戻る
        /// </summary>
        public static void ReturnToMainMenu() {
            SceneManager.LoadScene(MainMenuSceneName);
        }
    }
}
