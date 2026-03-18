using System;
using System.Collections.Generic;
using GoFPatterns.Core;
using UnityEngine;

namespace GoFPatterns.Patterns {
    /// <summary>
    /// デモの生成・開始・停止を統括するマネージャー
    /// PatternRepositoryとIPatternDemoを仲介し、デモのライフサイクルを管理する
    /// PatternDemoAttributeが付与されたクラスをリフレクションで自動検出する
    /// </summary>
    public class DemoManager : MonoBehaviour {
        /// <summary>パターン定義リポジトリ</summary>
        [SerializeField]
        private PatternRepository patternRepository;

        /// <summary>パターンIDをキーとするデモ型辞書</summary>
        private readonly Dictionary<string, Type> demoTypeRegistry = new Dictionary<string, Type>();
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

        /// <summary>
        /// 起動時にシングルトンを設定し、PatternDemoAttributeが付与されたデモ型を自動検出する
        /// </summary>
        private void Awake() {
            if (instance != null && instance != this) {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DiscoverDemoTypes();
        }

        /// <summary>
        /// ロード済みアセンブリからPatternDemoAttributeを持つ型を検出して登録する
        /// </summary>
        private void DiscoverDemoTypes() {
            var baseType = typeof(BasePatternDemo);
            var attrType = typeof(PatternDemoAttribute);
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                foreach (var type in assembly.GetTypes()) {
                    if (type.IsAbstract || !baseType.IsAssignableFrom(type)) {
                        continue;
                    }
                    var attrs = type.GetCustomAttributes(attrType, false);
                    if (attrs.Length == 0) {
                        continue;
                    }
                    var attr = (PatternDemoAttribute)attrs[0];
                    demoTypeRegistry[attr.PatternId] = type;
                }
            }
            Debug.Log($"[DemoManager] {demoTypeRegistry.Count} demo type(s) registered.");
        }

        /// <summary>
        /// 指定パターンIDのデモを開始する
        /// </summary>
        /// <param name="patternId">開始するパターンのID</param>
        /// <returns>開始されたデモ（見つからない場合はnull）</returns>
        public IPatternDemo StartDemo(string patternId) {
            StopCurrentDemo();

            if (!demoTypeRegistry.TryGetValue(patternId, out var demoType)) {
                Debug.LogWarning($"[DemoManager] No demo registered for patternId: {patternId}");
                return null;
            }

            var demoGo = new GameObject($"[Demo] {patternId}");
            demoGo.transform.SetParent(transform);
            currentDemoInstance = (BasePatternDemo)demoGo.AddComponent(demoType);
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

        /// <summary>
        /// 破棄時に実行中のデモを停止しシングルトン参照をクリアする
        /// </summary>
        private void OnDestroy() {
            StopCurrentDemo();
            if (instance == this) {
                instance = null;
            }
        }
    }
}
