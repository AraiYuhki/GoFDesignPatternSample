using System.Collections.Generic;
using GoFPatterns.Patterns;
using UnityEngine;

namespace GoFPatterns.Core {
    /// <summary>
    /// デモの生成・開始・停止を統括するマネージャー
    /// PatternRepositoryとIPatternDemoを仲介し、デモのライフサイクルを管理する
    /// </summary>
    public class DemoManager : MonoBehaviour {
        /// <summary>パターン定義リポジトリ</summary>
        [SerializeField]
        private PatternRepository patternRepository;

        /// <summary>登録されたデモのPrefab一覧</summary>
        [SerializeField]
        private BasePatternDemo[] demoPrefabs;

        /// <summary>パターンIDをキーとするデモPrefab辞書</summary>
        private readonly Dictionary<string, BasePatternDemo> demoPrefabMap = new Dictionary<string, BasePatternDemo>();
        /// <summary>現在実行中のデモインスタンス</summary>
        private BasePatternDemo currentDemoInstance;
        /// <summary>ログサービス</summary>
        private readonly LogService logService = new LogService();

        /// <summary>シングルトンインスタンス</summary>
        private static DemoManager instance;
        /// <summary>シングルトンインスタンスを取得する</summary>
        public static DemoManager Instance => instance;
        /// <summary>パターンリポジトリを取得する</summary>
        public PatternRepository Repository => patternRepository;
        /// <summary>ログサービスを取得する</summary>
        public LogService LogService => logService;
        /// <summary>現在実行中のデモを取得する</summary>
        public IPatternDemo CurrentDemo => currentDemoInstance;

        private void Awake() {
            if (instance != null && instance != this) {
                Destroy(gameObject);
                return;
            }
            instance = this;

            foreach (var prefab in demoPrefabs) {
                if (prefab != null) {
                    demoPrefabMap[prefab.PatternId] = prefab;
                }
            }
        }

        /// <summary>
        /// 指定パターンIDのデモを開始する
        /// </summary>
        /// <param name="patternId">開始するパターンのID</param>
        /// <returns>開始されたデモ（見つからない場合はnull）</returns>
        public IPatternDemo StartDemo(string patternId) {
            StopCurrentDemo();

            if (!demoPrefabMap.TryGetValue(patternId, out var prefab)) {
                return null;
            }

            currentDemoInstance = Instantiate(prefab, transform);
            currentDemoInstance.Initialize();
            logService.Clear();
            logService.Log($"=== {currentDemoInstance.DisplayName} デモ開始 ===");

            return currentDemoInstance;
        }

        /// <summary>
        /// 現在のデモを停止して破棄する
        /// </summary>
        public void StopCurrentDemo() {
            if (currentDemoInstance == null) {
                return;
            }
            currentDemoInstance.Stop();
            Destroy(currentDemoInstance.gameObject);
            currentDemoInstance = null;
        }

        private void OnDestroy() {
            StopCurrentDemo();
            if (instance == this) {
                instance = null;
            }
        }
    }
}
