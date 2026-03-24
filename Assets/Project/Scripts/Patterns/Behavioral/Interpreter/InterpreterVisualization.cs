using UnityEngine;

namespace GoFPatterns.Patterns.Visualization {
    /// <summary>
    /// Interpreterパターンのビジュアライゼーション
    /// 式ツリーのノードを階層的に配置し、構築と評価の過程を可視化する
    /// </summary>
    [PatternVisualization("interpreter")]
    public class InterpreterVisualization : BasePatternVisualization {
        /// <summary>ノードの半径</summary>
        private const float NodeRadius = 0.7f;
        /// <summary>ルートノードの色</summary>
        private static readonly Color OperatorColor = new Color(0.6f, 0.4f, 0.7f, 1f);
        /// <summary>リーフノードの色</summary>
        private static readonly Color NumberColor = new Color(0.4f, 0.7f, 0.5f, 1f);
        /// <summary>結果表示矩形のサイズ</summary>
        private static readonly Vector2 ResultSize = new Vector2(3f, 1f);
        /// <summary>結果表示の配置位置</summary>
        private static readonly Vector2 ResultPosition = new Vector2(0f, -4f);
        /// <summary>結果表示の色</summary>
        private static readonly Color ResultColor = new Color(0.8f, 0.7f, 0.3f, 1f);

        /// <summary>
        /// バインド時に結果表示領域を配置して初期表示を構築する
        /// </summary>
        /// <param name="demo">バインドされたデモ</param>
        protected override void OnBind(IPatternDemo demo) {
            VisualElement result = AddRect("result", "", ResultPosition, ResultSize, DimColor);
            result.SetVisible(false);
        }

        /// <summary>
        /// ステップに応じて式ツリーの構築と評価のアニメーションを更新する
        /// </summary>
        /// <param name="stepIndex">現在のステップインデックス</param>
        protected override void OnRefresh(int stepIndex) {
            switch (stepIndex) {
                case 0:
                    BuildSimpleTree();
                    break;
                case 1:
                    EvaluateSimpleTree();
                    break;
                case 2:
                    BuildComplexTree();
                    break;
                case 3:
                    EvaluateComplexTree();
                    break;
                case 4:
                    ShowTreeStructure();
                    break;
                case 5:
                    BuildVeryComplexTree();
                    break;
            }
        }

        /// <summary>
        /// 単純な加算式ツリー "3 + 5" を構築する
        /// </summary>
        private void BuildSimpleTree() {
            ClearTreeNodes();

            VisualElement add = AddCircle("s-add", "+", new Vector2(0f, 2f), NodeRadius, OperatorColor);
            VisualElement three = AddCircle("s-three", "3", new Vector2(-2f, 0f), NodeRadius, NumberColor);
            VisualElement five = AddCircle("s-five", "5", new Vector2(2f, 0f), NodeRadius, NumberColor);

            AddArrow("s-add-three", add, three, ArrowColor);
            AddArrow("s-add-five", add, five, ArrowColor);

            add.Pulse(HighlightColor, 0.5f);
            three.Pulse(HighlightColor, 0.5f);
            five.Pulse(HighlightColor, 0.5f);
        }

        /// <summary>
        /// 単純な加算式ツリーを評価してパルスさせる
        /// </summary>
        private void EvaluateSimpleTree() {
            GetElement("s-three")?.Pulse(PulseColor, 0.5f);
            GetElement("s-five")?.Pulse(PulseColor, 0.5f);
            GetArrow("s-add-three")?.Pulse(PulseColor, 0.5f);
            GetArrow("s-add-five")?.Pulse(PulseColor, 0.5f);
            GetElement("s-add")?.Pulse(PulseColor, 0.5f);

            VisualElement result = GetElement("result");
            result.SetVisible(true);
            result.SetLabel("結果: 8");
            result.SetColorImmediate(ResultColor);
            result.Pulse(PulseColor, 0.5f);
        }

        /// <summary>
        /// 複合式ツリー "(2 + 3) * 4" を構築する
        /// </summary>
        private void BuildComplexTree() {
            ClearTreeNodes();

            VisualElement mul = AddCircle("c-mul", "*", new Vector2(0f, 3f), NodeRadius, OperatorColor);
            VisualElement add = AddCircle("c-add", "+", new Vector2(-3f, 1f), NodeRadius, OperatorColor);
            VisualElement four = AddCircle("c-four", "4", new Vector2(3f, 1f), NodeRadius, NumberColor);
            VisualElement two = AddCircle("c-two", "2", new Vector2(-4.5f, -1f), NodeRadius, NumberColor);
            VisualElement three = AddCircle("c-three", "3", new Vector2(-1.5f, -1f), NodeRadius, NumberColor);

            AddArrow("c-mul-add", mul, add, ArrowColor);
            AddArrow("c-mul-four", mul, four, ArrowColor);
            AddArrow("c-add-two", add, two, ArrowColor);
            AddArrow("c-add-three", add, three, ArrowColor);

            mul.Pulse(HighlightColor, 0.5f);
            add.Pulse(HighlightColor, 0.5f);
            four.Pulse(HighlightColor, 0.5f);
            two.Pulse(HighlightColor, 0.5f);
            three.Pulse(HighlightColor, 0.5f);

            VisualElement result = GetElement("result");
            result.SetVisible(false);
        }

        /// <summary>
        /// 複合式ツリーを評価してパルスさせる
        /// </summary>
        private void EvaluateComplexTree() {
            GetElement("c-two")?.Pulse(PulseColor, 0.5f);
            GetElement("c-three")?.Pulse(PulseColor, 0.5f);
            GetArrow("c-add-two")?.Pulse(PulseColor, 0.5f);
            GetArrow("c-add-three")?.Pulse(PulseColor, 0.5f);
            GetElement("c-add")?.Pulse(PulseColor, 0.5f);
            GetElement("c-four")?.Pulse(PulseColor, 0.5f);
            GetArrow("c-mul-add")?.Pulse(PulseColor, 0.5f);
            GetArrow("c-mul-four")?.Pulse(PulseColor, 0.5f);
            GetElement("c-mul")?.Pulse(PulseColor, 0.5f);

            VisualElement result = GetElement("result");
            result.SetVisible(true);
            result.SetLabel("結果: 20");
            result.SetColorImmediate(ResultColor);
            result.Pulse(PulseColor, 0.5f);
        }

        /// <summary>
        /// ツリーの階層構造をハイライトして示す
        /// </summary>
        private void ShowTreeStructure() {
            GetElement("c-mul")?.Pulse(HighlightColor, 0.5f);
            GetElement("c-add")?.Pulse(HighlightColor, 0.5f);

            GetElement("c-two")?.SetColorImmediate(NumberColor);
            GetElement("c-three")?.SetColorImmediate(NumberColor);
            GetElement("c-four")?.SetColorImmediate(NumberColor);
            GetElement("c-mul")?.SetColorImmediate(OperatorColor);
            GetElement("c-add")?.SetColorImmediate(OperatorColor);

            VisualElement result = GetElement("result");
            result.SetVisible(false);
        }

        /// <summary>
        /// より複雑な式ツリー "(10 - 3) * (2 + 1)" を構築して評価する
        /// </summary>
        private void BuildVeryComplexTree() {
            ClearTreeNodes();

            VisualElement mul = AddCircle("v-mul", "*", new Vector2(0f, 3.5f), NodeRadius, OperatorColor);
            VisualElement sub = AddCircle("v-sub", "-", new Vector2(-3f, 1.5f), NodeRadius, OperatorColor);
            VisualElement add = AddCircle("v-add", "+", new Vector2(3f, 1.5f), NodeRadius, OperatorColor);
            VisualElement ten = AddCircle("v-ten", "10", new Vector2(-4.5f, -0.5f), NodeRadius, NumberColor);
            VisualElement three = AddCircle("v-three", "3", new Vector2(-1.5f, -0.5f), NodeRadius, NumberColor);
            VisualElement two = AddCircle("v-two", "2", new Vector2(1.5f, -0.5f), NodeRadius, NumberColor);
            VisualElement one = AddCircle("v-one", "1", new Vector2(4.5f, -0.5f), NodeRadius, NumberColor);

            AddArrow("v-mul-sub", mul, sub, ArrowColor);
            AddArrow("v-mul-add", mul, add, ArrowColor);
            AddArrow("v-sub-ten", sub, ten, ArrowColor);
            AddArrow("v-sub-three", sub, three, ArrowColor);
            AddArrow("v-add-two", add, two, ArrowColor);
            AddArrow("v-add-one", add, one, ArrowColor);

            mul.Pulse(PulseColor, 0.5f);
            sub.Pulse(PulseColor, 0.5f);
            add.Pulse(PulseColor, 0.5f);

            VisualElement result = GetElement("result");
            result.SetVisible(true);
            result.SetLabel("結果: 21");
            result.SetColorImmediate(ResultColor);
            result.Pulse(PulseColor, 0.5f);
        }

        /// <summary>
        /// 全ツリーノードを非表示にしてDim状態にする
        /// </summary>
        private void ClearTreeNodes() {
            string[] prefixes = { "s-", "c-", "v-" };
            string[] simpleIds = { "add", "three", "five" };
            string[] complexIds = { "mul", "add", "four", "two", "three" };
            string[] veryComplexIds = { "mul", "sub", "add", "ten", "three", "two", "one" };

            foreach (string id in simpleIds) {
                GetElement($"s-{id}")?.SetVisible(false);
            }
            foreach (string id in complexIds) {
                GetElement($"c-{id}")?.SetVisible(false);
            }
            foreach (string id in veryComplexIds) {
                GetElement($"v-{id}")?.SetVisible(false);
            }
        }
    }
}
