using System.Collections.Generic;
using System.Text;

namespace GoFPatterns.Patterns {
    // ---- Abstract class (Template) ----

    /// <summary>
    /// Template Methodパターンの抽象クラス
    /// データマイニングのアルゴリズム骨格を定義し、各ステップをサブクラスに委譲する
    /// </summary>
    public abstract class DataMiner {
        /// <summary>実行されたステップの履歴</summary>
        private readonly List<string> stepLog = new List<string>();

        /// <summary>マイナーの名前を取得する</summary>
        public abstract string MinerName { get; }
        /// <summary>実行されたステップの履歴を取得する</summary>
        public IReadOnlyList<string> StepLog => stepLog;

        /// <summary>
        /// データマイニングのテンプレートメソッド
        /// 固定のアルゴリズム骨格に沿ってサブクラスの各ステップを順番に実行する
        /// </summary>
        /// <returns>マイニング結果のサマリー</returns>
        public string Mine() {
            stepLog.Clear();
            string openResult = OpenFile();
            stepLog.Add(openResult);
            string extractResult = ExtractData();
            stepLog.Add(extractResult);
            string parseResult = ParseData();
            stepLog.Add(parseResult);
            string closeResult = CloseFile();
            stepLog.Add(closeResult);

            var summary = new StringBuilder();
            summary.Append($"{MinerName}: ");
            for (int i = 0; i < stepLog.Count; i++) {
                if (i > 0) {
                    summary.Append(" → ");
                }
                summary.Append(stepLog[i]);
            }
            return summary.ToString();
        }

        /// <summary>
        /// ファイルを開く（サブクラスで実装）
        /// </summary>
        /// <returns>ステップの実行結果</returns>
        protected abstract string OpenFile();

        /// <summary>
        /// データを抽出する（サブクラスで実装）
        /// </summary>
        /// <returns>ステップの実行結果</returns>
        protected abstract string ExtractData();

        /// <summary>
        /// データを解析する（サブクラスで実装）
        /// </summary>
        /// <returns>ステップの実行結果</returns>
        protected abstract string ParseData();

        /// <summary>
        /// ファイルを閉じる（サブクラスで実装）
        /// </summary>
        /// <returns>ステップの実行結果</returns>
        protected abstract string CloseFile();
    }

    // ---- ConcreteClasses ----

    /// <summary>
    /// CSVファイルからデータをマイニングする具象クラス
    /// </summary>
    public class CsvMiner : DataMiner {
        /// <summary>マイナーの名前</summary>
        public override string MinerName => "CsvMiner";

        /// <summary>
        /// CSVファイルを開く
        /// </summary>
        /// <returns>ステップの実行結果</returns>
        protected override string OpenFile() {
            return "CSVファイルを開く";
        }

        /// <summary>
        /// CSVの行データを抽出する
        /// </summary>
        /// <returns>ステップの実行結果</returns>
        protected override string ExtractData() {
            return "カンマ区切りで行を分割";
        }

        /// <summary>
        /// CSVデータを解析する
        /// </summary>
        /// <returns>ステップの実行結果</returns>
        protected override string ParseData() {
            return "各フィールドを型変換";
        }

        /// <summary>
        /// CSVファイルを閉じる
        /// </summary>
        /// <returns>ステップの実行結果</returns>
        protected override string CloseFile() {
            return "CSVストリームを閉じる";
        }
    }

    /// <summary>
    /// JSONファイルからデータをマイニングする具象クラス
    /// </summary>
    public class JsonMiner : DataMiner {
        /// <summary>マイナーの名前</summary>
        public override string MinerName => "JsonMiner";

        /// <summary>
        /// JSONファイルを開く
        /// </summary>
        /// <returns>ステップの実行結果</returns>
        protected override string OpenFile() {
            return "JSONファイルを開く";
        }

        /// <summary>
        /// JSONのキーバリューペアを抽出する
        /// </summary>
        /// <returns>ステップの実行結果</returns>
        protected override string ExtractData() {
            return "JSONツリーをトラバース";
        }

        /// <summary>
        /// JSONデータを解析する
        /// </summary>
        /// <returns>ステップの実行結果</returns>
        protected override string ParseData() {
            return "ネストされたオブジェクトを再帰的に解析";
        }

        /// <summary>
        /// JSONファイルを閉じる
        /// </summary>
        /// <returns>ステップの実行結果</returns>
        protected override string CloseFile() {
            return "JSONリーダーを閉じる";
        }
    }

    // ---- Demo ----

    /// <summary>
    /// Template Methodパターンのデモ
    /// DataMinerを例にアルゴリズムの骨格を固定しつつ各ステップを差し替える仕組みを示す
    /// </summary>
    [PatternDemo("template-method")]
    public class TemplateMethodDemo : BasePatternDemo {
        /// <summary>デモのパターンID</summary>
        public override string PatternId => "template-method";

        /// <summary>デモの表示名</summary>
        public override string DisplayName => "Template Method";

        /// <summary>CSVマイナー</summary>
        private CsvMiner csvMiner;
        /// <summary>JSONマイナー</summary>
        private JsonMiner jsonMiner;

        /// <summary>
        /// リセット時にマイナーを再生成する
        /// </summary>
        protected override void OnReset() {
            csvMiner = null;
            jsonMiner = null;
        }

        /// <summary>
        /// Template Methodパターンのシナリオを構築する
        /// </summary>
        /// <param name="scenario">ステップを追加するシナリオ</param>
        protected override void BuildScenario(DemoScenario scenario) {
            csvMiner = new CsvMiner();
            jsonMiner = new JsonMiner();

            scenario.AddStep(new DemoStep(
                "CsvMinerを生成する",
                () => {
                    Log("Client", "CsvMiner生成", "CSVファイル用のDataMinerを生成");
                }
            ));

            scenario.AddStep(new DemoStep(
                "CsvMiner.Mine()を実行する — テンプレートメソッドが4ステップを順に呼ぶ",
                () => {
                    csvMiner.Mine();
                    IReadOnlyList<string> steps = csvMiner.StepLog;
                    for (int i = 0; i < steps.Count; i++) {
                        Log("CsvMiner", $"Step{i + 1}", steps[i]);
                    }
                }
            ));

            scenario.AddStep(new DemoStep(
                "JsonMinerを生成する",
                () => {
                    Log("Client", "JsonMiner生成", "JSONファイル用のDataMinerを生成");
                }
            ));

            scenario.AddStep(new DemoStep(
                "JsonMiner.Mine()を実行する — 同じテンプレートメソッドが異なるステップを呼ぶ",
                () => {
                    jsonMiner.Mine();
                    IReadOnlyList<string> steps = jsonMiner.StepLog;
                    for (int i = 0; i < steps.Count; i++) {
                        Log("JsonMiner", $"Step{i + 1}", steps[i]);
                    }
                }
            ));

            scenario.AddStep(new DemoStep(
                "両者を比較する — アルゴリズム構造は同じで詳細が異なることを確認する",
                () => {
                    Log("Template Method", "比較",
                        "OpenFile → ExtractData → ParseData → CloseFile の順序は共通");
                    Log("CsvMiner", "詳細", "CSV固有の処理: カンマ区切り分割、型変換");
                    Log("JsonMiner", "詳細", "JSON固有の処理: ツリートラバース、再帰解析");
                }
            ));
        }
    }
}
