namespace GoFPatterns.Patterns {
    // ---- Expression interface ----

    /// <summary>
    /// Interpreterパターンの抽象式インターフェース
    /// 式ツリーの各ノードが実装する共通契約
    /// </summary>
    public interface IExpression {
        /// <summary>
        /// 式を評価して結果を返す
        /// </summary>
        /// <returns>評価結果の整数値</returns>
        int Interpret();
        /// <summary>
        /// 式の文字列表現を返す
        /// </summary>
        /// <returns>式の文字列表現</returns>
        string ToExpressionString();
    }

    // ---- TerminalExpression ----

    /// <summary>
    /// Interpreterパターンの終端式
    /// 数値リテラルを表す葉ノード
    /// </summary>
    public class NumberExpression : IExpression {
        /// <summary>保持する数値</summary>
        private readonly int number;

        /// <summary>
        /// NumberExpressionを生成する
        /// </summary>
        /// <param name="number">数値</param>
        public NumberExpression(int number) {
            this.number = number;
        }

        /// <summary>
        /// 数値をそのまま返す
        /// </summary>
        /// <returns>保持する数値</returns>
        public int Interpret() {
            return number;
        }

        /// <summary>
        /// 数値の文字列表現を返す
        /// </summary>
        /// <returns>数値の文字列</returns>
        public string ToExpressionString() {
            return number.ToString();
        }
    }

    // ---- NonTerminalExpressions ----

    /// <summary>
    /// Interpreterパターンの非終端式
    /// 2つの式の加算を表す
    /// </summary>
    public class AddExpression : IExpression {
        /// <summary>左辺の式</summary>
        private readonly IExpression left;
        /// <summary>右辺の式</summary>
        private readonly IExpression right;

        /// <summary>
        /// AddExpressionを生成する
        /// </summary>
        /// <param name="left">左辺の式</param>
        /// <param name="right">右辺の式</param>
        public AddExpression(IExpression left, IExpression right) {
            this.left = left;
            this.right = right;
        }

        /// <summary>
        /// 左辺と右辺を加算した結果を返す
        /// </summary>
        /// <returns>加算結果</returns>
        public int Interpret() {
            return left.Interpret() + right.Interpret();
        }

        /// <summary>
        /// 加算式の文字列表現を返す
        /// </summary>
        /// <returns>加算式の文字列</returns>
        public string ToExpressionString() {
            return $"({left.ToExpressionString()} + {right.ToExpressionString()})";
        }
    }

    /// <summary>
    /// Interpreterパターンの非終端式
    /// 2つの式の減算を表す
    /// </summary>
    public class SubtractExpression : IExpression {
        /// <summary>左辺の式</summary>
        private readonly IExpression left;
        /// <summary>右辺の式</summary>
        private readonly IExpression right;

        /// <summary>
        /// SubtractExpressionを生成する
        /// </summary>
        /// <param name="left">左辺の式</param>
        /// <param name="right">右辺の式</param>
        public SubtractExpression(IExpression left, IExpression right) {
            this.left = left;
            this.right = right;
        }

        /// <summary>
        /// 左辺から右辺を減算した結果を返す
        /// </summary>
        /// <returns>減算結果</returns>
        public int Interpret() {
            return left.Interpret() - right.Interpret();
        }

        /// <summary>
        /// 減算式の文字列表現を返す
        /// </summary>
        /// <returns>減算式の文字列</returns>
        public string ToExpressionString() {
            return $"({left.ToExpressionString()} - {right.ToExpressionString()})";
        }
    }

    /// <summary>
    /// Interpreterパターンの非終端式
    /// 2つの式の乗算を表す
    /// </summary>
    public class MultiplyExpression : IExpression {
        /// <summary>左辺の式</summary>
        private readonly IExpression left;
        /// <summary>右辺の式</summary>
        private readonly IExpression right;

        /// <summary>
        /// MultiplyExpressionを生成する
        /// </summary>
        /// <param name="left">左辺の式</param>
        /// <param name="right">右辺の式</param>
        public MultiplyExpression(IExpression left, IExpression right) {
            this.left = left;
            this.right = right;
        }

        /// <summary>
        /// 左辺と右辺を乗算した結果を返す
        /// </summary>
        /// <returns>乗算結果</returns>
        public int Interpret() {
            return left.Interpret() * right.Interpret();
        }

        /// <summary>
        /// 乗算式の文字列表現を返す
        /// </summary>
        /// <returns>乗算式の文字列</returns>
        public string ToExpressionString() {
            return $"({left.ToExpressionString()} * {right.ToExpressionString()})";
        }
    }

    // ---- Demo ----

    /// <summary>
    /// Interpreterパターンのデモ
    /// 式ツリーを構築して算術式を評価する仕組みを示す
    /// </summary>
    [PatternDemo("interpreter")]
    public class InterpreterDemo : BasePatternDemo {
        /// <summary>デモのパターンID</summary>
        public override string PatternId => "interpreter";

        /// <summary>デモの表示名</summary>
        public override string DisplayName => "Interpreter";

        /// <summary>
        /// リセット時の追加処理
        /// </summary>
        protected override void OnReset() {
        }

        /// <summary>
        /// Interpreterパターンのシナリオを構築する
        /// </summary>
        /// <param name="scenario">ステップを追加するシナリオ</param>
        protected override void BuildScenario(DemoScenario scenario) {
            scenario.AddStep(new DemoStep(
                "\"3 + 5\" の式ツリーを構築する",
                () => {
                    IExpression three = new NumberExpression(3);
                    IExpression five = new NumberExpression(5);
                    IExpression addExpr = new AddExpression(three, five);
                    Log("Client", "式ツリー構築", $"式: {addExpr.ToExpressionString()}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "\"3 + 5\" を評価する → 8",
                () => {
                    IExpression three = new NumberExpression(3);
                    IExpression five = new NumberExpression(5);
                    IExpression addExpr = new AddExpression(three, five);
                    int result = addExpr.Interpret();
                    Log("Interpreter", $"Interpret({addExpr.ToExpressionString()})", $"結果: {result}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "\"(2 + 3) * 4\" の式ツリーを構築する",
                () => {
                    IExpression two = new NumberExpression(2);
                    IExpression three = new NumberExpression(3);
                    IExpression four = new NumberExpression(4);
                    IExpression addExpr = new AddExpression(two, three);
                    IExpression mulExpr = new MultiplyExpression(addExpr, four);
                    Log("Client", "式ツリー構築", $"式: {mulExpr.ToExpressionString()}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "\"(2 + 3) * 4\" を評価する → 20",
                () => {
                    IExpression two = new NumberExpression(2);
                    IExpression three = new NumberExpression(3);
                    IExpression four = new NumberExpression(4);
                    IExpression addExpr = new AddExpression(two, three);
                    IExpression mulExpr = new MultiplyExpression(addExpr, four);
                    int result = mulExpr.Interpret();
                    Log("Interpreter", $"Interpret({mulExpr.ToExpressionString()})", $"結果: {result}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "式ツリーの構造を示す — ノードの階層を確認する",
                () => {
                    Log("Interpreter", "ツリー構造", "MultiplyExpression");
                    Log("Interpreter", "├─ 左辺", "AddExpression(2, 3)");
                    Log("Interpreter", "└─ 右辺", "NumberExpression(4)");
                }
            ));

            scenario.AddStep(new DemoStep(
                "複雑な式 \"(10 - 3) * (2 + 1)\" を構築して評価する",
                () => {
                    IExpression ten = new NumberExpression(10);
                    IExpression three = new NumberExpression(3);
                    IExpression two = new NumberExpression(2);
                    IExpression one = new NumberExpression(1);
                    IExpression subExpr = new SubtractExpression(ten, three);
                    IExpression addExpr = new AddExpression(two, one);
                    IExpression mulExpr = new MultiplyExpression(subExpr, addExpr);
                    int result = mulExpr.Interpret();
                    Log("Interpreter", $"Interpret({mulExpr.ToExpressionString()})", $"結果: {result}");
                }
            ));
        }
    }
}
