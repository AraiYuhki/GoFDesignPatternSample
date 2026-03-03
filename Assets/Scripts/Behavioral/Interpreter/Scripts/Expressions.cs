using System.Text;

namespace DesignPatterns.Behavioral.Interpreter
{
    /// <summary>
    /// 移動コマンドを表す式
    /// Interpreterパターンにおける TerminalExpression に相当し、
    /// 指定された方向と距離でキャラクターの位置を更新する
    /// </summary>
    public sealed class MoveExpression : IExpression
    {
        /// <summary>移動方向</summary>
        private readonly string direction;

        /// <summary>移動距離</summary>
        private readonly int distance;

        /// <summary>
        /// MoveExpressionを生成する
        /// </summary>
        /// <param name="direction">移動方向 (UP/DOWN/LEFT/RIGHT)</param>
        /// <param name="distance">移動距離</param>
        public MoveExpression(string direction, int distance)
        {
            this.direction = direction;
            this.distance = distance;
        }

        /// <summary>
        /// 移動コマンドを解釈し、コンテキストの位置を更新する
        /// </summary>
        /// <param name="context">ゲームコンテキスト</param>
        /// <returns>移動結果の文字列</returns>
        public string Interpret(GameContext context)
        {
            switch (direction)
            {
                case "UP":
                    context.Y += distance;
                    break;
                case "DOWN":
                    context.Y -= distance;
                    break;
                case "LEFT":
                    context.X -= distance;
                    break;
                case "RIGHT":
                    context.X += distance;
                    break;
                default:
                    return $"不明な方向: {direction}";
            }

            return $"{context.CharacterName} が {DirectionToJapanese(direction)} に {distance} 移動 → 現在位置: ({context.X}, {context.Y})";
        }

        /// <summary>
        /// 方向を日本語に変換する
        /// </summary>
        /// <param name="dir">英語の方向文字列</param>
        /// <returns>日本語の方向文字列</returns>
        private static string DirectionToJapanese(string dir)
        {
            switch (dir)
            {
                case "UP":
                    return "上";
                case "DOWN":
                    return "下";
                case "LEFT":
                    return "左";
                case "RIGHT":
                    return "右";
                default:
                    return dir;
            }
        }
    }

    /// <summary>
    /// ステータス表示コマンドを表す式
    /// Interpreterパターンにおける TerminalExpression に相当し、
    /// キャラクターの現在位置を返す
    /// </summary>
    public sealed class StatusExpression : IExpression
    {
        /// <summary>
        /// ステータスコマンドを解釈し、現在の位置情報を返す
        /// </summary>
        /// <param name="context">ゲームコンテキスト</param>
        /// <returns>現在位置の文字列</returns>
        public string Interpret(GameContext context)
        {
            return $"[STATUS] {context.CharacterName} の現在位置: ({context.X}, {context.Y})";
        }
    }

    /// <summary>
    /// 繰り返しコマンドを表す式
    /// Interpreterパターンにおける NonterminalExpression に相当し、
    /// 内包する式を指定回数繰り返し実行する
    /// </summary>
    public sealed class RepeatExpression : IExpression
    {
        /// <summary>繰り返し回数</summary>
        private readonly int count;

        /// <summary>繰り返す式</summary>
        private readonly IExpression expression;

        /// <summary>
        /// RepeatExpressionを生成する
        /// </summary>
        /// <param name="count">繰り返し回数</param>
        /// <param name="expression">繰り返す式</param>
        public RepeatExpression(int count, IExpression expression)
        {
            this.count = count;
            this.expression = expression;
        }

        /// <summary>
        /// 繰り返しコマンドを解釈し、内包する式をN回実行する
        /// </summary>
        /// <param name="context">ゲームコンテキスト</param>
        /// <returns>全実行結果を連結した文字列</returns>
        public string Interpret(GameContext context)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"[REPEAT x{count}]");

            for (int i = 0; i < count; i++)
            {
                string result = expression.Interpret(context);
                builder.Append("\n  ").Append(result);
            }

            return builder.ToString();
        }
    }
}
