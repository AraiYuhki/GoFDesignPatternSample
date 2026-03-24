namespace GoFPatterns.Patterns {
    // ---- Legacy interface ----

    /// <summary>
    /// 旧式のログ出力クラス
    /// WriteLogメソッドで文字列を出力する旧APIを提供する
    /// </summary>
    public class OldLogger {
        /// <summary>最後に出力されたメッセージ</summary>
        private string lastMessage;

        /// <summary>最後に出力されたメッセージを取得する</summary>
        public string LastMessage => lastMessage;

        /// <summary>
        /// ログメッセージを出力する（旧API）
        /// </summary>
        /// <param name="message">出力するメッセージ</param>
        public void WriteLog(string message) {
            lastMessage = message;
        }
    }

    // ---- Modern interface ----

    /// <summary>
    /// 新式のロガーインターフェース
    /// ログレベルとメッセージを受け取るモダンなAPIを定義する
    /// </summary>
    public interface IModernLogger {
        /// <summary>
        /// ログレベル付きでメッセージを出力する
        /// </summary>
        /// <param name="level">ログレベル（例: Info, Warning, Error）</param>
        /// <param name="message">出力するメッセージ</param>
        void Log(string level, string message);
    }

    // ---- Adapter ----

    /// <summary>
    /// OldLoggerをIModernLoggerとして使えるようにするアダプター
    /// レベル付きのモダンAPIを旧式のWriteLogに変換する
    /// </summary>
    public class LoggerAdapter : IModernLogger {
        /// <summary>ラップ対象の旧式ロガー</summary>
        private readonly OldLogger oldLogger;

        /// <summary>ラップ対象のOldLoggerを取得する</summary>
        public OldLogger WrappedLogger => oldLogger;

        /// <summary>
        /// LoggerAdapterを生成する
        /// </summary>
        /// <param name="oldLogger">ラップする旧式ロガー</param>
        public LoggerAdapter(OldLogger oldLogger) {
            this.oldLogger = oldLogger;
        }

        /// <summary>
        /// モダンAPIをOldLoggerのWriteLogに変換して出力する
        /// </summary>
        /// <param name="level">ログレベル</param>
        /// <param name="message">出力するメッセージ</param>
        public void Log(string level, string message) {
            oldLogger.WriteLog($"[{level}] {message}");
        }
    }

    // ---- Demo ----

    /// <summary>
    /// Adapterパターンのデモ
    /// OldLoggerの旧APIをIModernLoggerのモダンAPIに適合させる流れを示す
    /// </summary>
    [PatternDemo("adapter")]
    public class AdapterDemo : BasePatternDemo {
        /// <summary>デモのパターンID</summary>
        public override string PatternId => "adapter";

        /// <summary>デモの表示名</summary>
        public override string DisplayName => "Adapter";

        /// <summary>旧式ロガー</summary>
        private OldLogger oldLogger;

        /// <summary>アダプターで包んだモダンロガー</summary>
        private LoggerAdapter adapter;

        /// <summary>
        /// リセット時にドメインオブジェクトをクリアする
        /// </summary>
        protected override void OnReset() {
            oldLogger = null;
            adapter = null;
        }

        /// <summary>
        /// Adapterパターンのシナリオを構築する
        /// </summary>
        /// <param name="scenario">ステップを追加するシナリオ</param>
        protected override void BuildScenario(DemoScenario scenario) {
            oldLogger = new OldLogger();

            scenario.AddStep(new DemoStep(
                "OldLoggerの旧APIを確認する",
                () => {
                    oldLogger.WriteLog("テストメッセージ");
                    Log("OldLogger", "WriteLog(\"テストメッセージ\")", $"出力: {oldLogger.LastMessage}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "IModernLoggerインターフェースの要件を確認する",
                () => {
                    Log("IModernLogger", "Log(level, message)", "レベル付きの新APIが必要");
                }
            ));

            scenario.AddStep(new DemoStep(
                "LoggerAdapterを作成してOldLoggerをラップする",
                () => {
                    adapter = new LoggerAdapter(oldLogger);
                    Log("Client", "new LoggerAdapter(oldLogger)", "アダプターを生成");
                }
            ));

            scenario.AddStep(new DemoStep(
                "アダプター経由でLog()を呼び出す",
                () => {
                    adapter.Log("Info", "システム起動");
                    Log("LoggerAdapter", "Log(\"Info\", \"システム起動\")", $"OldLogger受信: {oldLogger.LastMessage}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "OldLoggerが正しくメッセージを受信したことを検証する",
                () => {
                    string expected = "[Info] システム起動";
                    bool match = oldLogger.LastMessage == expected;
                    Log("検証", "OldLogger.LastMessage", match ? "一致 — 変換成功" : "不一致");
                }
            ));

            scenario.AddStep(new DemoStep(
                "アダプターを透過的にIModernLoggerとして使用する",
                () => {
                    IModernLogger modernLogger = adapter;
                    modernLogger.Log("Error", "接続タイムアウト");
                    Log("Client", "IModernLogger.Log(\"Error\", ...)", $"透過的に利用: {oldLogger.LastMessage}");
                }
            ));
        }
    }
}
