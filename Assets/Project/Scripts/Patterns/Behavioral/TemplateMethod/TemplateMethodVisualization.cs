using UnityEngine;

namespace GoFPatterns.Patterns.Visualization {
    /// <summary>
    /// Template Methodパターンのビジュアライゼーション
    /// 4つのステップを縦に配置し、CSVとJSONの2列で各ステップの実行を可視化する
    /// </summary>
    [PatternVisualization("template-method")]
    public class TemplateMethodVisualization : BasePatternVisualization {
        /// <summary>テンプレートステップの名前配列</summary>
        private static readonly string[] StepNames = { "OpenFile", "ExtractData", "ParseData", "CloseFile" };
        /// <summary>テンプレートステップのX座標</summary>
        private const float StepX = 0f;
        /// <summary>テンプレートステップの開始Y座標</summary>
        private const float StepStartY = 3.5f;
        /// <summary>ステップ間の垂直間隔</summary>
        private const float StepSpacing = 2.2f;
        /// <summary>CSV列のX座標</summary>
        private const float CsvX = -4f;
        /// <summary>JSON列のX座標</summary>
        private const float JsonX = 4f;
        /// <summary>テンプレートステップ矩形のサイズ</summary>
        private static readonly Vector2 StepSize = new Vector2(2.8f, 1f);
        /// <summary>実装矩形のサイズ</summary>
        private static readonly Vector2 ImplSize = new Vector2(2.8f, 1f);
        /// <summary>テンプレートステップの色</summary>
        private static readonly Color StepColor = new Color(0.5f, 0.5f, 0.6f, 1f);
        /// <summary>CSVの色</summary>
        private static readonly Color CsvColor = new Color(0.4f, 0.7f, 0.5f, 1f);
        /// <summary>JSONの色</summary>
        private static readonly Color JsonColor = new Color(0.4f, 0.5f, 0.8f, 1f);
        /// <summary>CSVラベルの配置位置</summary>
        private static readonly Vector2 CsvLabelPosition = new Vector2(-4f, 4.5f);
        /// <summary>JSONラベルの配置位置</summary>
        private static readonly Vector2 JsonLabelPosition = new Vector2(4f, 4.5f);
        /// <summary>列ラベルのサイズ</summary>
        private static readonly Vector2 LabelSize = new Vector2(2.5f, 0.8f);

        /// <summary>
        /// バインド時にテンプレートステップと2つの列を配置して初期表示を構築する
        /// </summary>
        /// <param name="demo">バインドされたデモ</param>
        protected override void OnBind(IPatternDemo demo) {
            AddRect("csv-label", "CsvMiner", CsvLabelPosition, LabelSize, DimColor);
            AddRect("json-label", "JsonMiner", JsonLabelPosition, LabelSize, DimColor);

            for (int i = 0; i < StepNames.Length; i++) {
                float y = StepStartY - i * StepSpacing;
                AddRect($"step{i}", StepNames[i], new Vector2(StepX, y), StepSize, StepColor);
                AddRect($"csv{i}", "", new Vector2(CsvX, y), ImplSize, DimColor);
                AddRect($"json{i}", "", new Vector2(JsonX, y), ImplSize, DimColor);
            }

            SetCsvLabels();
            SetJsonLabels();
        }

        /// <summary>
        /// ステップに応じてCSV・JSONの各ステップのハイライトを更新する
        /// </summary>
        /// <param name="stepIndex">現在のステップインデックス</param>
        protected override void OnRefresh(int stepIndex) {
            switch (stepIndex) {
                case 0:
                    GetElement("csv-label")?.SetColorImmediate(CsvColor);
                    GetElement("csv-label")?.Pulse(HighlightColor, 0.5f);
                    break;
                case 1:
                    HighlightCsvSteps();
                    break;
                case 2:
                    DimCsvSteps();
                    GetElement("json-label")?.SetColorImmediate(JsonColor);
                    GetElement("json-label")?.Pulse(HighlightColor, 0.5f);
                    break;
                case 3:
                    HighlightJsonSteps();
                    break;
                case 4:
                    HighlightComparison();
                    break;
            }
        }

        /// <summary>
        /// CSV列のラベルを設定する
        /// </summary>
        private void SetCsvLabels() {
            GetElement("csv0")?.SetLabel("CSVを開く");
            GetElement("csv1")?.SetLabel("カンマ区切り");
            GetElement("csv2")?.SetLabel("型変換");
            GetElement("csv3")?.SetLabel("CSVを閉じる");
        }

        /// <summary>
        /// JSON列のラベルを設定する
        /// </summary>
        private void SetJsonLabels() {
            GetElement("json0")?.SetLabel("JSONを開く");
            GetElement("json1")?.SetLabel("ツリー走査");
            GetElement("json2")?.SetLabel("再帰解析");
            GetElement("json3")?.SetLabel("JSONを閉じる");
        }

        /// <summary>
        /// CSV列の全ステップをハイライトする
        /// </summary>
        private void HighlightCsvSteps() {
            for (int i = 0; i < StepNames.Length; i++) {
                GetElement($"step{i}")?.Pulse(PulseColor, 0.5f);
                GetElement($"csv{i}")?.SetColorImmediate(CsvColor);
                GetElement($"csv{i}")?.Pulse(PulseColor, 0.5f);
            }
        }

        /// <summary>
        /// JSON列の全ステップをハイライトする
        /// </summary>
        private void HighlightJsonSteps() {
            for (int i = 0; i < StepNames.Length; i++) {
                GetElement($"step{i}")?.Pulse(PulseColor, 0.5f);
                GetElement($"json{i}")?.SetColorImmediate(JsonColor);
                GetElement($"json{i}")?.Pulse(PulseColor, 0.5f);
            }
        }

        /// <summary>
        /// CSV列の全ステップをDim状態にする
        /// </summary>
        private void DimCsvSteps() {
            for (int i = 0; i < StepNames.Length; i++) {
                GetElement($"csv{i}")?.SetColorImmediate(DimColor);
            }
        }

        /// <summary>
        /// 比較ステップで共通構造をハイライトする
        /// </summary>
        private void HighlightComparison() {
            for (int i = 0; i < StepNames.Length; i++) {
                GetElement($"step{i}")?.SetColorImmediate(HighlightColor);
                GetElement($"step{i}")?.Pulse(PulseColor, 0.5f);
                GetElement($"csv{i}")?.SetColorImmediate(CsvColor);
                GetElement($"json{i}")?.SetColorImmediate(JsonColor);
            }
        }
    }
}
