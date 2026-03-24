using GoFPatterns.Core;
using GoFPatterns.Patterns;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GoFPatterns.UI {
    /// <summary>
    /// デモ実行画面
    /// デモの再生制御UI、ログ表示、ステップ説明、ビジュアライゼーション領域を管理する
    /// </summary>
    public class DemoScreen : BaseScreen {
        /// <summary>パターン名ラベル</summary>
        [SerializeField]
        private TMP_Text patternNameLabel;
        /// <summary>ステップ表示ラベル（例: "Step 3/10"）</summary>
        [SerializeField]
        private TMP_Text stepLabel;
        /// <summary>ステップ説明文ラベル</summary>
        [SerializeField]
        private TMP_Text stepDescriptionLabel;
        /// <summary>ログ表示テキスト</summary>
        [SerializeField]
        private TMP_Text logText;
        /// <summary>ログ表示のScrollRect</summary>
        [SerializeField]
        private ScrollRect logScrollRect;
        /// <summary>再生ボタン</summary>
        [SerializeField]
        private Button playButton;
        /// <summary>一時停止ボタン</summary>
        [SerializeField]
        private Button pauseButton;
        /// <summary>ステップ送りボタン</summary>
        [SerializeField]
        private Button stepButton;
        /// <summary>リセットボタン</summary>
        [SerializeField]
        private Button resetButton;
        /// <summary>戻るボタン</summary>
        [SerializeField]
        private Button backButton;
        /// <summary>自動再生速度スライダー</summary>
        [SerializeField]
        private Slider speedSlider;
        /// <summary>ビジュアライゼーション表示用のRawImage</summary>
        [SerializeField]
        private RawImage visualizationDisplay;

        /// <summary>現在のデモ参照</summary>
        private IPatternDemo currentDemo;

        /// <summary>
        /// 起動時にボタンイベントとログサービスを購読する
        /// </summary>
        private void Start() {
            if (playButton != null) {
                playButton.onClick.AddListener(OnPlayClicked);
            }
            if (pauseButton != null) {
                pauseButton.onClick.AddListener(OnPauseClicked);
            }
            if (stepButton != null) {
                stepButton.onClick.AddListener(OnStepClicked);
            }
            if (resetButton != null) {
                resetButton.onClick.AddListener(OnResetClicked);
            }
            if (backButton != null) {
                backButton.onClick.AddListener(OnBackClicked);
            }
            if (speedSlider != null) {
                speedSlider.onValueChanged.AddListener(OnSpeedChanged);
            }

            // ログ更新イベントを購読する
            DemoManager.Instance.LogService.OnLogAdded += OnLogAdded;
            DemoManager.Instance.LogService.OnLogCleared += OnLogCleared;
        }

        /// <summary>
        /// 画面表示時にデモを開始する
        /// </summary>
        /// <param name="context">パターンID（string）</param>
        protected override void OnShow(object context) {
            string patternId = context as string;
            if (string.IsNullOrEmpty(patternId)) {
                return;
            }

            // ビジュアライゼーションレンダラーにRawImageをバインドする
            if (DemoManager.Instance.VisualizationRenderer != null && visualizationDisplay != null) {
                DemoManager.Instance.VisualizationRenderer.SetTargetImage(visualizationDisplay);
            }

            currentDemo = DemoManager.Instance.StartDemo(patternId);
            if (currentDemo == null) {
                return;
            }

            var definition = DemoManager.Instance.Repository.GetDefinition(patternId);
            SetText(patternNameLabel, definition != null
                ? $"{definition.DisplayName} デモ"
                : currentDemo.DisplayName);

            UpdateStepDisplay();
        }

        /// <summary>
        /// 画面非表示時にデモを停止する
        /// </summary>
        protected override void OnHide() {
            DemoManager.Instance.StopCurrentDemo();
            currentDemo = null;
        }

        /// <summary>
        /// 毎フレームステップ表示とログ表示を更新する
        /// </summary>
        private void Update() {
            if (currentDemo == null) {
                return;
            }
            UpdateStepDisplay();
            RefreshLogDisplay();
        }

        /// <summary>
        /// ステップ表示を更新する
        /// </summary>
        private void UpdateStepDisplay() {
            if (currentDemo == null) {
                return;
            }
            int current = currentDemo.CurrentStepIndex + 1;
            int total = currentDemo.TotalSteps;
            SetText(stepLabel, $"Step {current}/{total}");

            if (currentDemo is BasePatternDemo baseDemo) {
                SetText(stepDescriptionLabel, baseDemo.CurrentStep?.Description ?? "");
            }

            if (stepButton != null) {
                stepButton.interactable = currentDemo.CanStep;
            }
        }

        /// <summary>
        /// ログ表示を更新する
        /// </summary>
        private void RefreshLogDisplay() {
            if (logText == null || currentDemo == null) {
                return;
            }
            var logs = DemoManager.Instance.LogService.Entries;
            var sb = new System.Text.StringBuilder();
            foreach (var entry in logs) {
                sb.AppendLine(entry);
            }
            logText.text = sb.ToString();
        }

        /// <summary>
        /// ログ追加時のコールバック
        /// </summary>
        /// <param name="message">追加されたログ</param>
        private void OnLogAdded(string message) {
            // スクロールを最下部に移動する
            if (logScrollRect != null) {
                Canvas.ForceUpdateCanvases();
                logScrollRect.verticalNormalizedPosition = 0f;
            }
        }

        /// <summary>
        /// ログクリア時のコールバック
        /// </summary>
        private void OnLogCleared() {
            if (logText != null) {
                logText.text = "";
            }
        }

        /// <summary>
        /// 再生ボタン押下時の処理
        /// </summary>
        private void OnPlayClicked() {
            currentDemo?.Play();
        }

        /// <summary>
        /// 一時停止ボタン押下時の処理
        /// </summary>
        private void OnPauseClicked() {
            currentDemo?.Pause();
        }

        /// <summary>
        /// ステップ送りボタン押下時の処理
        /// </summary>
        private void OnStepClicked() {
            if (currentDemo == null) {
                return;
            }
            int countBefore = currentDemo.GetCurrentLogs().Count;
            currentDemo.StepForward();
            var logs = currentDemo.GetCurrentLogs();
            for (int i = countBefore; i < logs.Count; i++) {
                DemoManager.Instance.LogService.Log(logs[i]);
            }
        }

        /// <summary>
        /// リセットボタン押下時の処理
        /// </summary>
        private void OnResetClicked() {
            currentDemo?.ResetDemo();
            DemoManager.Instance.LogService.Clear();
            DemoManager.Instance.LogService.Log($"=== {currentDemo?.DisplayName} リセット ===");
        }

        /// <summary>
        /// 戻るボタン押下時の処理
        /// </summary>
        private void OnBackClicked() {
            ScreenManager.Instance.GoBack();
        }

        /// <summary>
        /// 速度スライダー変更時の処理
        /// </summary>
        /// <param name="value">スライダー値（0〜1）</param>
        private void OnSpeedChanged(float value) {
            // スライダー値（0.1〜3.0）を間隔に変換する（値が大きいほど速い）
            float interval = Mathf.Lerp(3f, 0.3f, value);
            if (currentDemo is BasePatternDemo baseDemo) {
                baseDemo.SetAutoPlayInterval(interval);
            }
        }

        /// <summary>
        /// テキストを安全にセットする
        /// </summary>
        /// <param name="label">対象のテキストコンポーネント</param>
        /// <param name="value">設定する値</param>
        private static void SetText(TMP_Text label, string value) {
            if (label != null) {
                label.text = value ?? "";
            }
        }

        /// <summary>
        /// 破棄時にログサービスの購読を解除する
        /// </summary>
        private void OnDestroy() {
            if (DemoManager.Instance != null) {
                DemoManager.Instance.LogService.OnLogAdded -= OnLogAdded;
                DemoManager.Instance.LogService.OnLogCleared -= OnLogCleared;
            }
        }
    }
}
