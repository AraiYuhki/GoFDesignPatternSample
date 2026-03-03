using UnityEngine;

namespace DesignPatterns.Creational.Singleton
{
    /// <summary>
    /// Singletonパターンを適用したGameManager
    /// アプリケーション全体で唯一のインスタンスを保証し、
    /// グローバルなアクセスポイントを提供する
    ///
    /// 【Singletonパターンの意図】
    /// クラスのインスタンスが1つしか存在しないことを保証し、
    /// そのインスタンスへのグローバルなアクセス手段を提供する
    /// </summary>
    public sealed class GameManager : MonoBehaviour
    {
        /// <summary>唯一のインスタンスへの参照</summary>
        private static GameManager instance;

        /// <summary>インスタンスの生成回数を追跡するカウンター</summary>
        private static int creationAttemptCount;

        /// <summary>現在のスコア</summary>
        private int score;

        /// <summary>
        /// Singletonインスタンスを取得する
        /// インスタンスが存在しない場合はnullを返す
        /// </summary>
        public static GameManager Instance => instance;

        /// <summary>
        /// 現在のスコアを取得する
        /// </summary>
        public int Score => score;

        /// <summary>
        /// インスタンスの生成試行回数を取得する
        /// </summary>
        public static int CreationAttemptCount => creationAttemptCount;

        private void Awake()
        {
            creationAttemptCount++;

            if (instance != null && instance != this)
            {
                InGameLogger.Log($"[Singleton] 2つ目のGameManagerが検出されました（試行 #{creationAttemptCount}）→ 破棄します", LogColor.Red);
                Destroy(gameObject);
                return;
            }

            instance = this;
            InGameLogger.Log($"[Singleton] GameManagerインスタンスを生成しました（試行 #{creationAttemptCount}）", LogColor.Blue);
        }

        private void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }

        /// <summary>
        /// スコアを加算する
        /// </summary>
        /// <param name="amount">加算する値</param>
        public void AddScore(int amount)
        {
            score += amount;
            InGameLogger.Log($"[Singleton] スコア +{amount} → 合計: {score}", LogColor.Blue);
        }

        /// <summary>
        /// スコアをリセットする
        /// </summary>
        public void ResetScore()
        {
            score = 0;
            InGameLogger.Log("[Singleton] スコアをリセットしました", LogColor.Blue);
        }
    }
}
