using System;
using System.Collections.Generic;
using GoFPatterns.Core;
using GoFPatterns.Patterns.Visualization;
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
        /// <summary>ビジュアライゼーションレンダラー</summary>
        [SerializeField]
        private VisualizationRenderer visualizationRenderer;

        /// <summary>パターンIDをキーとするデモ型辞書</summary>
        private readonly Dictionary<string, Type> demoTypeRegistry = new Dictionary<string, Type>();
        /// <summary>パターンIDをキーとするビジュアライゼーション型辞書</summary>
        private readonly Dictionary<string, Type> visualizationTypeRegistry = new Dictionary<string, Type>();
        /// <summary>現在実行中のデモインスタンス</summary>
        private BasePatternDemo currentDemoInstance;
        /// <summary>現在のビジュアライゼーション</summary>
        private BasePatternVisualization currentVisualization;
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
        /// <summary>ビジュアライゼーションレンダラーを取得する</summary>
        public VisualizationRenderer VisualizationRenderer => visualizationRenderer;

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
            DiscoverVisualizationTypes();
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
        /// ロード済みアセンブリからPatternVisualizationAttributeを持つ型を検出して登録する
        /// </summary>
        private void DiscoverVisualizationTypes() {
            var baseType = typeof(BasePatternVisualization);
            var attrType = typeof(PatternVisualizationAttribute);
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                foreach (var type in assembly.GetTypes()) {
                    if (type.IsAbstract || !baseType.IsAssignableFrom(type)) {
                        continue;
                    }
                    var attrs = type.GetCustomAttributes(attrType, false);
                    if (attrs.Length == 0) {
                        continue;
                    }
                    var attr = (PatternVisualizationAttribute)attrs[0];
                    visualizationTypeRegistry[attr.PatternId] = type;
                }
            }
            Debug.Log($"[DemoManager] {visualizationTypeRegistry.Count} visualization type(s) registered.");
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

            BindVisualization(patternId);

            return currentDemoInstance;
        }

        /// <summary>
        /// パターンIDに対応するビジュアライゼーションを検索してバインドする
        /// </summary>
        /// <param name="patternId">パターンID</param>
        private void BindVisualization(string patternId) {
            currentVisualization = null;
            if (visualizationRenderer == null) {
                return;
            }
            if (!visualizationTypeRegistry.TryGetValue(patternId, out var visType)) {
                return;
            }
            var vis = (BasePatternVisualization)System.Activator.CreateInstance(visType);
            vis.SetRenderer(visualizationRenderer);
            currentDemoInstance.BindVisualization(vis);
            currentVisualization = vis;
        }

        /// <summary>
        /// 現在のデモを停止して破棄する
        /// </summary>
        public void StopCurrentDemo() {
            if (currentDemoInstance == null) {
                return;
            }
            currentDemoInstance.Stop();
            if (currentVisualization != null) {
                currentVisualization.Clear();
                currentVisualization = null;
            }
            if (visualizationRenderer != null) {
                visualizationRenderer.ClearAll();
            }
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
