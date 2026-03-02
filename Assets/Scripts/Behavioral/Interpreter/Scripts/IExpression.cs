namespace DesignPatterns.Behavioral.Interpreter {
    /// <summary>
    /// 式を表すインターフェース
    /// Interpreterパターンにおける AbstractExpression に相当し、
    /// コンテキストを受け取って解釈結果を返す
    /// </summary>
    public interface IExpression {
        /// <summary>
        /// 式を解釈して結果を返す
        /// </summary>
        /// <param name="context">ゲームコンテキスト</param>
        /// <returns>解釈結果の文字列</returns>
        string Interpret(GameContext context);
    }
}
