namespace DesignPatterns.Behavioral.Interpreter {
    /// <summary>
    /// コマンド文字列を解析してIExpressionに変換するパーサー
    /// "MOVE UP 3", "STATUS", "REPEAT 3 MOVE RIGHT 1" などの文字列を解釈する
    /// </summary>
    public static class CommandParser {
        /// <summary>MOVEコマンドのキーワード</summary>
        private const string MoveKeyword = "MOVE";

        /// <summary>STATUSコマンドのキーワード</summary>
        private const string StatusKeyword = "STATUS";

        /// <summary>REPEATコマンドのキーワード</summary>
        private const string RepeatKeyword = "REPEAT";

        /// <summary>
        /// コマンド文字列を解析してIExpressionを返す
        /// </summary>
        /// <param name="commandText">コマンド文字列</param>
        /// <returns>解析されたIExpression、解析失敗時はnull</returns>
        public static IExpression Parse(string commandText) {
            if (string.IsNullOrEmpty(commandText)) {
                return null;
            }

            string[] tokens = commandText.Trim().Split(' ');
            if (tokens.Length == 0) {
                return null;
            }

            return ParseTokens(tokens, 0);
        }

        /// <summary>
        /// トークン配列を指定位置から解析する
        /// </summary>
        /// <param name="tokens">トークン配列</param>
        /// <param name="startIndex">解析開始位置</param>
        /// <returns>解析されたIExpression、解析失敗時はnull</returns>
        private static IExpression ParseTokens(string[] tokens, int startIndex) {
            if (startIndex >= tokens.Length) {
                return null;
            }

            string keyword = tokens[startIndex];

            if (keyword == MoveKeyword) {
                return ParseMove(tokens, startIndex);
            }

            if (keyword == StatusKeyword) {
                return new StatusExpression();
            }

            if (keyword == RepeatKeyword) {
                return ParseRepeat(tokens, startIndex);
            }

            return null;
        }

        /// <summary>
        /// MOVEコマンドを解析する
        /// 形式: MOVE {方向} {距離}
        /// </summary>
        /// <param name="tokens">トークン配列</param>
        /// <param name="startIndex">MOVEキーワードの位置</param>
        /// <returns>解析されたMoveExpression、解析失敗時はnull</returns>
        private static MoveExpression ParseMove(string[] tokens, int startIndex) {
            int directionIndex = startIndex + 1;
            int distanceIndex = startIndex + 2;

            if (distanceIndex >= tokens.Length) {
                return null;
            }

            string direction = tokens[directionIndex];
            int distance;
            if (!int.TryParse(tokens[distanceIndex], out distance)) {
                return null;
            }

            return new MoveExpression(direction, distance);
        }

        /// <summary>
        /// REPEATコマンドを解析する
        /// 形式: REPEAT {回数} {内包コマンド}
        /// </summary>
        /// <param name="tokens">トークン配列</param>
        /// <param name="startIndex">REPEATキーワードの位置</param>
        /// <returns>解析されたRepeatExpression、解析失敗時はnull</returns>
        private static RepeatExpression ParseRepeat(string[] tokens, int startIndex) {
            int countIndex = startIndex + 1;
            int innerStartIndex = startIndex + 2;

            if (countIndex >= tokens.Length) {
                return null;
            }

            int count;
            if (!int.TryParse(tokens[countIndex], out count)) {
                return null;
            }

            IExpression innerExpression = ParseTokens(tokens, innerStartIndex);
            if (innerExpression == null) {
                return null;
            }

            return new RepeatExpression(count, innerExpression);
        }
    }
}
