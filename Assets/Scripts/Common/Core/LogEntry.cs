namespace DesignPatterns {
    /// <summary>
    /// ログ表示に使用する1件分のログデータ
    /// </summary>
    public readonly struct LogEntry {
        /// <summary>ログメッセージ</summary>
        public readonly string Message;

        /// <summary>ログの種別を示す色</summary>
        public readonly LogColor Color;

        /// <summary>
        /// LogEntryを生成する
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        /// <param name="color">ログの色</param>
        public LogEntry(string message, LogColor color = LogColor.White) {
            Message = message;
            Color = color;
        }
    }

    /// <summary>
    /// ログ表示で使用する色の種別
    /// </summary>
    public enum LogColor {
        /// <summary>通常メッセージ</summary>
        White,
        /// <summary>生成パターン</summary>
        Blue,
        /// <summary>構造パターン</summary>
        Green,
        /// <summary>振る舞いパターン</summary>
        Orange,
        /// <summary>警告</summary>
        Yellow,
        /// <summary>エラー</summary>
        Red
    }
}
