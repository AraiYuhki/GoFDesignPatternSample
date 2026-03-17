using System.Collections.Generic;
using GoFPatterns.Core;
using UnityEngine;

namespace GoFPatterns.Patterns {
    /// <summary>
    /// 全パターンデモの共通基底クラス
    /// DemoScenarioを使ったステップ進行、ログ管理、ビジュアライゼーション連携を提供する
    /// サブクラスはBuildScenarioをオーバーライドしてステップ列を定義する
    /// </summary>
    public abstract class BasePatternDemo : MonoBehaviour, IPatternDemo {
        /// <summary>シナリオ（ステップ列）</summary>
        private readonly DemoScenario scenario = new DemoScenario();
        /// <summary>蓄積されたログ</summary>
        private readonly List<string> logs = new List<string>();
        /// <summary>自動再生中かどうか</summary>
        private bool isPlaying;
        /// <summary>自動再生の経過時間</summary>
        private float autoPlayTimer;
        /// <summary>自動再生の間隔（秒）</summary>
        private float autoPlayInterval = 1.5f;
        /// <summary>バインドされたビジュアライゼーション</summary>
        private IPatternVisualization visualization;

        /// <summary>パターンIDを返す（サブクラスで実装）</summary>
        public abstract string PatternId { get; }
        /// <summary>表示名を返す（サブクラスで実装）</summary>
        public abstract string DisplayName { get; }
        /// <summary>次ステップへ進行可能か</summary>
        public bool CanStep => scenario.HasNext;
        /// <summary>現在のステップインデックス</summary>
        public int CurrentStepIndex => scenario.CurrentIndex;
        /// <summary>全ステップ数</summary>
        public int TotalSteps => scenario.TotalSteps;

        /// <summary>
        /// デモを初期化しシナリオを構築する
        /// </summary>
        public void Initialize() {
            logs.Clear();
            scenario.Clear();
            BuildScenario(scenario);
            OnInitialize();
        }

        /// <summary>
        /// 自動再生を開始する
        /// </summary>
        public void Play() {
            isPlaying = true;
            autoPlayTimer = 0f;
        }

        /// <summary>
        /// 自動再生を一時停止する
        /// </summary>
        public void Pause() {
            isPlaying = false;
        }

        /// <summary>
        /// 一時停止から再開する
        /// </summary>
        public void Resume() {
            isPlaying = true;
        }

        /// <summary>
        /// 自動再生を停止する
        /// </summary>
        public void Stop() {
            isPlaying = false;
        }

        /// <summary>
        /// デモを初期状態にリセットする
        /// </summary>
        public void ResetDemo() {
            isPlaying = false;
            logs.Clear();
            scenario.Reset();
            OnReset();
            visualization?.Clear();
            Initialize();
            visualization?.Refresh();
        }

        /// <summary>
        /// 1ステップ進める
        /// </summary>
        public void StepForward() {
            var step = scenario.ExecuteNext();
            if (step == null) {
                isPlaying = false;
                return;
            }
            string logEntry = FormatLogEntry(step);
            logs.Add(logEntry);
            visualization?.Refresh();
        }

        /// <summary>
        /// 現在のログ一覧を取得する
        /// </summary>
        /// <returns>ログの読み取り専用リスト</returns>
        public IReadOnlyList<string> GetCurrentLogs() {
            return logs;
        }

        /// <summary>
        /// 自動再生の間隔を設定する
        /// </summary>
        /// <param name="interval">間隔（秒）</param>
        public void SetAutoPlayInterval(float interval) {
            autoPlayInterval = Mathf.Max(0.1f, interval);
        }

        /// <summary>
        /// ビジュアライゼーションをバインドする
        /// </summary>
        /// <param name="vis">バインドするビジュアライゼーション</param>
        public void BindVisualization(IPatternVisualization vis) {
            visualization = vis;
            visualization?.Bind(this);
        }

        /// <summary>
        /// ログを追加する（サブクラスのステップ処理内から使用）
        /// </summary>
        /// <param name="actor">アクター名</param>
        /// <param name="action">アクション名</param>
        /// <param name="result">結果の説明</param>
        protected void Log(string actor, string action, string result = "") {
            int stepNum = scenario.CurrentIndex + 1;
            string entry = string.IsNullOrEmpty(result)
                ? $"[Step {stepNum:D2}] {actor} -> {action}"
                : $"[Step {stepNum:D2}] {actor} -> {action} -> {result}";
            logs.Add(entry);
        }

        /// <summary>
        /// シナリオのステップ列を構築する（サブクラスで実装）
        /// </summary>
        /// <param name="scenario">ステップを追加するシナリオ</param>
        protected abstract void BuildScenario(DemoScenario scenario);

        /// <summary>
        /// 初期化時の追加処理（サブクラスでオーバーライド可能）
        /// </summary>
        protected virtual void OnInitialize() { }

        /// <summary>
        /// リセット時の追加処理（サブクラスでオーバーライド可能）
        /// </summary>
        protected virtual void OnReset() { }

        /// <summary>
        /// 自動再生の更新処理
        /// </summary>
        private void Update() {
            if (!isPlaying || !CanStep) {
                if (isPlaying && !CanStep) {
                    isPlaying = false;
                }
                return;
            }
            autoPlayTimer += Time.deltaTime;
            if (autoPlayTimer >= autoPlayInterval) {
                autoPlayTimer = 0f;
                StepForward();
            }
        }

        /// <summary>
        /// ステップのログエントリをフォーマットする
        /// </summary>
        /// <param name="step">対象のステップ</param>
        /// <returns>フォーマットされたログ文字列</returns>
        private string FormatLogEntry(DemoStep step) {
            int stepNum = scenario.CurrentIndex + 1;
            if (!string.IsNullOrEmpty(step.Actor) && !string.IsNullOrEmpty(step.ActionName)) {
                return $"[Step {stepNum:D2}] {step.Actor} -> {step.ActionName}";
            }
            return $"[Step {stepNum:D2}] {step.Description}";
        }
    }
}
