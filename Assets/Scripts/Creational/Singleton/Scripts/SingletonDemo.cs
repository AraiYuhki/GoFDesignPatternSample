using UnityEngine;
using UnityEngine.UI;

namespace DesignPatterns.Creational.Singleton {
    /// <summary>
    /// Singletonパターンのデモシーンを制御するクラス
    ///
    /// 【デモの内容】
    /// - 複数のGameManagerインスタンス生成を試み、1つしか存在できないことを確認
    /// - 複数のコンポーネントから同一インスタンスへアクセスできることを確認
    /// - スコアの加算・リセット操作でSingletonの動作を体験
    /// </summary>
    public sealed class SingletonDemo : PatternDemoBase {
        /// <summary>GameManagerプレハブ（生成テスト用）</summary>
        [SerializeField]
        private GameObject gameManagerPrefab;

        /// <summary>スコア加算ボタン</summary>
        [SerializeField]
        private Button addScoreButton;

        /// <summary>スコアリセットボタン</summary>
        [SerializeField]
        private Button resetScoreButton;

        /// <summary>重複生成テストボタン</summary>
        [SerializeField]
        private Button duplicateTestButton;

        /// <summary>インスタンス確認ボタン</summary>
        [SerializeField]
        private Button checkInstanceButton;

        /// <summary>スコア加算時の加算値</summary>
        private const int ScoreIncrement = 10;

        /// <inheritdoc/>
        protected override string PatternName {
            get { return "Singleton"; }
        }

        /// <inheritdoc/>
        protected override PatternCategory Category {
            get { return PatternCategory.Creational; }
        }

        /// <inheritdoc/>
        protected override string Description {
            get { return "クラスのインスタンスが1つだけ存在することを保証し、グローバルなアクセスポイントを提供する"; }
        }

        /// <inheritdoc/>
        protected override void OnDemoStart() {
            if (addScoreButton != null) {
                addScoreButton.onClick.AddListener(OnAddScore);
            }
            if (resetScoreButton != null) {
                resetScoreButton.onClick.AddListener(OnResetScore);
            }
            if (duplicateTestButton != null) {
                duplicateTestButton.onClick.AddListener(OnDuplicateTest);
            }
            if (checkInstanceButton != null) {
                checkInstanceButton.onClick.AddListener(OnCheckInstance);
            }

            InGameLogger.Log("ボタンを押してSingletonの動作を確認してください", LogColor.Yellow);
        }

        /// <summary>
        /// スコアを加算する
        /// </summary>
        private void OnAddScore() {
            if (GameManager.Instance != null) {
                GameManager.Instance.AddScore(ScoreIncrement);
            } else {
                InGameLogger.Log("[エラー] GameManagerのインスタンスが存在しません", LogColor.Red);
            }
        }

        /// <summary>
        /// スコアをリセットする
        /// </summary>
        private void OnResetScore() {
            if (GameManager.Instance != null) {
                GameManager.Instance.ResetScore();
            } else {
                InGameLogger.Log("[エラー] GameManagerのインスタンスが存在しません", LogColor.Red);
            }
        }

        /// <summary>
        /// 重複インスタンスの生成を試みる
        /// Singletonにより2つ目は破棄されることを確認する
        /// </summary>
        private void OnDuplicateTest() {
            InGameLogger.Log("--- 重複生成テスト ---", LogColor.Yellow);
            InGameLogger.Log("新しいGameManagerの生成を試みます...", LogColor.White);

            if (gameManagerPrefab != null) {
                Instantiate(gameManagerPrefab);
            } else {
                var duplicate = new GameObject("GameManager_Duplicate");
                duplicate.AddComponent<GameManager>();
            }
        }

        /// <summary>
        /// 現在のSingletonインスタンスの状態を確認する
        /// </summary>
        private void OnCheckInstance() {
            InGameLogger.Log("--- インスタンス確認 ---", LogColor.Yellow);

            if (GameManager.Instance != null) {
                InGameLogger.Log(
                    $"インスタンス: 存在する (ID: {GameManager.Instance.GetInstanceID()})",
                    LogColor.Blue
                );
                InGameLogger.Log(
                    $"現在のスコア: {GameManager.Instance.Score}",
                    LogColor.Blue
                );
                InGameLogger.Log(
                    $"生成試行回数: {GameManager.CreationAttemptCount}",
                    LogColor.Blue
                );
            } else {
                InGameLogger.Log("インスタンス: 存在しない", LogColor.Red);
            }
        }
    }
}
