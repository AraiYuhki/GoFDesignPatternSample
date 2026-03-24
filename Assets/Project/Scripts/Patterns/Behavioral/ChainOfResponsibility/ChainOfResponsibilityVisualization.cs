using UnityEngine;

namespace GoFPatterns.Patterns.Visualization {
    /// <summary>
    /// Chain of Responsibilityパターンのビジュアライゼーション
    /// 3つのハンドラーをチェーン状に配置し、チケット処理の委譲を矢印で可視化する
    /// </summary>
    [PatternVisualization("chain-of-responsibility")]
    public class ChainOfResponsibilityVisualization : BasePatternVisualization {
        /// <summary>BasicSupportの配置位置</summary>
        private static readonly Vector2 BasicPosition = new Vector2(-4f, 0f);
        /// <summary>SeniorSupportの配置位置</summary>
        private static readonly Vector2 SeniorPosition = new Vector2(0f, 0f);
        /// <summary>ManagerSupportの配置位置</summary>
        private static readonly Vector2 ManagerPosition = new Vector2(4f, 0f);
        /// <summary>チケットの配置位置</summary>
        private static readonly Vector2 TicketPosition = new Vector2(-4f, 3.5f);
        /// <summary>ハンドラー矩形のサイズ</summary>
        private static readonly Vector2 HandlerSize = new Vector2(2.8f, 1.4f);
        /// <summary>チケットの半径</summary>
        private const float TicketRadius = 0.7f;
        /// <summary>BasicSupportの色</summary>
        private static readonly Color BasicColor = new Color(0.4f, 0.7f, 0.5f, 1f);
        /// <summary>SeniorSupportの色</summary>
        private static readonly Color SeniorColor = new Color(0.4f, 0.5f, 0.8f, 1f);
        /// <summary>ManagerSupportの色</summary>
        private static readonly Color ManagerColor = new Color(0.7f, 0.4f, 0.5f, 1f);
        /// <summary>チケットの色</summary>
        private static readonly Color TicketColor = new Color(0.8f, 0.7f, 0.3f, 1f);

        /// <summary>
        /// バインド時に3つのハンドラーとチケット要素を配置して初期表示を構築する
        /// </summary>
        /// <param name="demo">バインドされたデモ</param>
        protected override void OnBind(IPatternDemo demo) {
            VisualElement basic = AddRect("basic", "BasicSupport", BasicPosition, HandlerSize, DimColor);
            VisualElement senior = AddRect("senior", "SeniorSupport", SeniorPosition, HandlerSize, DimColor);
            VisualElement manager = AddRect("manager", "ManagerSupport", ManagerPosition, HandlerSize, DimColor);

            VisualElement ticket = AddCircle("ticket", "Ticket", TicketPosition, TicketRadius, TicketColor);
            ticket.SetVisible(false);
        }

        /// <summary>
        /// ステップに応じてチェーン構築とチケット処理のアニメーションを更新する
        /// </summary>
        /// <param name="stepIndex">現在のステップインデックス</param>
        protected override void OnRefresh(int stepIndex) {
            VisualElement basic = GetElement("basic");
            VisualElement senior = GetElement("senior");
            VisualElement manager = GetElement("manager");
            VisualElement ticket = GetElement("ticket");

            switch (stepIndex) {
                case 0:
                    basic.SetColorImmediate(BasicColor);
                    senior.SetColorImmediate(SeniorColor);
                    manager.SetColorImmediate(ManagerColor);
                    basic.Pulse(HighlightColor, 0.5f);
                    senior.Pulse(HighlightColor, 0.5f);
                    manager.Pulse(HighlightColor, 0.5f);
                    break;
                case 1:
                    AddArrow("basic-senior", basic, senior, ArrowColor);
                    AddArrow("senior-manager", senior, manager, ArrowColor);
                    GetArrow("basic-senior")?.Pulse(PulseColor, 0.5f);
                    GetArrow("senior-manager")?.Pulse(PulseColor, 0.5f);
                    break;
                case 2:
                    ticket.SetVisible(true);
                    ticket.SetLabel("Low\nパスワード\nリセット");
                    DimAllHandlers();
                    basic.SetColorImmediate(BasicColor);
                    basic.Pulse(PulseColor, 0.5f);
                    ticket.Pulse(PulseColor, 0.5f);
                    break;
                case 3:
                    ticket.SetLabel("Medium\nアカウント\n復旧");
                    DimAllHandlers();
                    basic.SetColorImmediate(BasicColor);
                    GetArrow("basic-senior")?.Pulse(PulseColor, 0.5f);
                    senior.SetColorImmediate(SeniorColor);
                    senior.Pulse(PulseColor, 0.5f);
                    ticket.Pulse(PulseColor, 0.5f);
                    break;
                case 4:
                    ticket.SetLabel("High\nデータ消失");
                    DimAllHandlers();
                    basic.SetColorImmediate(BasicColor);
                    senior.SetColorImmediate(SeniorColor);
                    GetArrow("basic-senior")?.Pulse(PulseColor, 0.5f);
                    GetArrow("senior-manager")?.Pulse(PulseColor, 0.5f);
                    manager.SetColorImmediate(ManagerColor);
                    manager.Pulse(PulseColor, 0.5f);
                    ticket.Pulse(PulseColor, 0.5f);
                    break;
                case 5:
                    ticket.SetLabel("Critical\n全システム\n障害");
                    DimAllHandlers();
                    GetArrow("basic-senior")?.Pulse(DimColor, 0.5f);
                    GetArrow("senior-manager")?.Pulse(DimColor, 0.5f);
                    ticket.SetColorImmediate(new Color(0.8f, 0.3f, 0.3f, 1f));
                    ticket.Pulse(HighlightColor, 0.5f);
                    break;
            }
        }

        /// <summary>
        /// 全ハンドラーをDim状態にする
        /// </summary>
        private void DimAllHandlers() {
            GetElement("basic")?.SetColorImmediate(DimColor);
            GetElement("senior")?.SetColorImmediate(DimColor);
            GetElement("manager")?.SetColorImmediate(DimColor);
        }
    }
}
