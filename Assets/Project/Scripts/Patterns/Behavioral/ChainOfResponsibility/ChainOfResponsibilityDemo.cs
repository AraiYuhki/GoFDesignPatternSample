using System.Collections.Generic;

namespace GoFPatterns.Patterns {
    // ---- SupportTicket ----

    /// <summary>
    /// サポートチケットの重大度レベル
    /// </summary>
    public enum TicketSeverity {
        /// <summary>低い重大度</summary>
        Low = 1,
        /// <summary>中程度の重大度</summary>
        Medium = 2,
        /// <summary>高い重大度</summary>
        High = 3,
        /// <summary>致命的な重大度</summary>
        Critical = 4
    }

    /// <summary>
    /// サポートチケットを表すクラス
    /// 重大度と説明を保持する
    /// </summary>
    public class SupportTicket {
        /// <summary>チケットの重大度</summary>
        private readonly TicketSeverity severity;
        /// <summary>チケットの説明</summary>
        private readonly string description;

        /// <summary>チケットの重大度を取得する</summary>
        public TicketSeverity Severity => severity;
        /// <summary>チケットの説明を取得する</summary>
        public string Description => description;

        /// <summary>
        /// SupportTicketを生成する
        /// </summary>
        /// <param name="severity">チケットの重大度</param>
        /// <param name="description">チケットの説明</param>
        public SupportTicket(TicketSeverity severity, string description) {
            this.severity = severity;
            this.description = description;
        }
    }

    // ---- Handler interface ----

    /// <summary>
    /// Chain of Responsibilityパターンのハンドラーインターフェース
    /// 次のハンドラーの設定とチケット処理を定義する
    /// </summary>
    public interface ISupportHandler {
        /// <summary>ハンドラーの名前を取得する</summary>
        string Name { get; }
        /// <summary>
        /// 次のハンドラーを設定する
        /// </summary>
        /// <param name="handler">次のハンドラー</param>
        /// <returns>設定されたハンドラー（チェーン構築用）</returns>
        ISupportHandler SetNext(ISupportHandler handler);
        /// <summary>
        /// チケットを処理する
        /// </summary>
        /// <param name="ticket">処理対象のチケット</param>
        /// <returns>処理結果の説明文（処理できなければnull）</returns>
        string Handle(SupportTicket ticket);
    }

    // ---- Base handler ----

    /// <summary>
    /// ハンドラーの共通基底クラス
    /// 次のハンドラーへの転送ロジックを提供する
    /// </summary>
    public abstract class BaseSupportHandler : ISupportHandler {
        /// <summary>次のハンドラー</summary>
        private ISupportHandler nextHandler;

        /// <summary>ハンドラーの名前を取得する</summary>
        public abstract string Name { get; }

        /// <summary>
        /// 次のハンドラーを設定する
        /// </summary>
        /// <param name="handler">次のハンドラー</param>
        /// <returns>設定されたハンドラー（チェーン構築用）</returns>
        public ISupportHandler SetNext(ISupportHandler handler) {
            nextHandler = handler;
            return handler;
        }

        /// <summary>
        /// チケットを処理する（処理できなければ次へ転送する）
        /// </summary>
        /// <param name="ticket">処理対象のチケット</param>
        /// <returns>処理結果の説明文</returns>
        public virtual string Handle(SupportTicket ticket) {
            if (nextHandler != null) {
                return nextHandler.Handle(ticket);
            }
            return null;
        }
    }

    // ---- ConcreteHandlers ----

    /// <summary>低重大度のチケットを処理する基本サポート</summary>
    public class BasicSupport : BaseSupportHandler {
        /// <summary>処理可能な最大重大度</summary>
        private static readonly TicketSeverity MaxSeverity = TicketSeverity.Low;

        /// <summary>ハンドラーの名前</summary>
        public override string Name => "BasicSupport";

        /// <summary>
        /// 低重大度のチケットを処理する
        /// </summary>
        /// <param name="ticket">処理対象のチケット</param>
        /// <returns>処理結果の説明文</returns>
        public override string Handle(SupportTicket ticket) {
            if (ticket.Severity <= MaxSeverity) {
                return $"{Name} が '{ticket.Description}' を処理しました (重大度: {ticket.Severity})";
            }
            return base.Handle(ticket);
        }
    }

    /// <summary>中重大度のチケットを処理するシニアサポート</summary>
    public class SeniorSupport : BaseSupportHandler {
        /// <summary>処理可能な最大重大度</summary>
        private static readonly TicketSeverity MaxSeverity = TicketSeverity.Medium;

        /// <summary>ハンドラーの名前</summary>
        public override string Name => "SeniorSupport";

        /// <summary>
        /// 中重大度のチケットを処理する
        /// </summary>
        /// <param name="ticket">処理対象のチケット</param>
        /// <returns>処理結果の説明文</returns>
        public override string Handle(SupportTicket ticket) {
            if (ticket.Severity <= MaxSeverity) {
                return $"{Name} が '{ticket.Description}' を処理しました (重大度: {ticket.Severity})";
            }
            return base.Handle(ticket);
        }
    }

    /// <summary>高重大度のチケットを処理するマネージャーサポート</summary>
    public class ManagerSupport : BaseSupportHandler {
        /// <summary>処理可能な最大重大度</summary>
        private static readonly TicketSeverity MaxSeverity = TicketSeverity.High;

        /// <summary>ハンドラーの名前</summary>
        public override string Name => "ManagerSupport";

        /// <summary>
        /// 高重大度のチケットを処理する
        /// </summary>
        /// <param name="ticket">処理対象のチケット</param>
        /// <returns>処理結果の説明文</returns>
        public override string Handle(SupportTicket ticket) {
            if (ticket.Severity <= MaxSeverity) {
                return $"{Name} が '{ticket.Description}' を処理しました (重大度: {ticket.Severity})";
            }
            return base.Handle(ticket);
        }
    }

    // ---- Demo ----

    /// <summary>
    /// Chain of Responsibilityパターンのデモ
    /// サポートチケットの重大度に応じてハンドラーチェーンを辿る仕組みを示す
    /// </summary>
    [PatternDemo("chain-of-responsibility")]
    public class ChainOfResponsibilityDemo : BasePatternDemo {
        /// <summary>デモのパターンID</summary>
        public override string PatternId => "chain-of-responsibility";

        /// <summary>デモの表示名</summary>
        public override string DisplayName => "Chain of Responsibility";

        /// <summary>チェーンの先頭ハンドラー</summary>
        private BasicSupport basicSupport;
        /// <summary>シニアサポートハンドラー</summary>
        private SeniorSupport seniorSupport;
        /// <summary>マネージャーサポートハンドラー</summary>
        private ManagerSupport managerSupport;

        /// <summary>
        /// リセット時にハンドラーを再生成する
        /// </summary>
        protected override void OnReset() {
            basicSupport = null;
            seniorSupport = null;
            managerSupport = null;
        }

        /// <summary>
        /// Chain of Responsibilityパターンのシナリオを構築する
        /// </summary>
        /// <param name="scenario">ステップを追加するシナリオ</param>
        protected override void BuildScenario(DemoScenario scenario) {
            basicSupport = new BasicSupport();
            seniorSupport = new SeniorSupport();
            managerSupport = new ManagerSupport();

            scenario.AddStep(new DemoStep(
                "3つのサポートハンドラーを生成する",
                () => {
                    Log("Client", "ハンドラー生成", "BasicSupport, SeniorSupport, ManagerSupport を生成");
                }
            ));

            scenario.AddStep(new DemoStep(
                "ハンドラーをチェーンで接続する: Basic → Senior → Manager",
                () => {
                    basicSupport.SetNext(seniorSupport);
                    seniorSupport.SetNext(managerSupport);
                    Log("Client", "チェーン構築", "BasicSupport → SeniorSupport → ManagerSupport");
                }
            ));

            scenario.AddStep(new DemoStep(
                "低重大度チケットを送信する — BasicSupportが処理する",
                () => {
                    var ticket = new SupportTicket(TicketSeverity.Low, "パスワードリセット");
                    string result = basicSupport.Handle(ticket);
                    Log("BasicSupport", $"Handle({ticket.Description})", result);
                }
            ));

            scenario.AddStep(new DemoStep(
                "中重大度チケットを送信する — BasicをスキップしてSeniorSupportが処理する",
                () => {
                    var ticket = new SupportTicket(TicketSeverity.Medium, "アカウント復旧");
                    string result = basicSupport.Handle(ticket);
                    Log("SeniorSupport", $"Handle({ticket.Description})", result);
                }
            ));

            scenario.AddStep(new DemoStep(
                "高重大度チケットを送信する — Basic・SeniorをスキップしてManagerSupportが処理する",
                () => {
                    var ticket = new SupportTicket(TicketSeverity.High, "データ消失");
                    string result = basicSupport.Handle(ticket);
                    Log("ManagerSupport", $"Handle({ticket.Description})", result);
                }
            ));

            scenario.AddStep(new DemoStep(
                "致命的重大度チケットを送信する — どのハンドラーも処理できない",
                () => {
                    var ticket = new SupportTicket(TicketSeverity.Critical, "全システム障害");
                    string result = basicSupport.Handle(ticket);
                    string message = result ?? "どのハンドラーも処理できませんでした";
                    Log("Chain", $"Handle({ticket.Description})", message);
                }
            ));
        }
    }
}
