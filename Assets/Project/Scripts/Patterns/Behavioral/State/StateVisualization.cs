using UnityEngine;

namespace GoFPatterns.Patterns.Visualization {
    /// <summary>
    /// Stateパターンのビジュアライゼーション
    /// 4つの状態をダイヤモンド型に配置し、状態遷移を矢印のパルスで可視化する
    /// </summary>
    [PatternVisualization("state")]
    public class StateVisualization : BasePatternVisualization {
        /// <summary>Idle状態の配置位置（上）</summary>
        private static readonly Vector2 IdlePosition = new Vector2(0f, 3.5f);
        /// <summary>Walking状態の配置位置（右）</summary>
        private static readonly Vector2 WalkingPosition = new Vector2(4f, 0f);
        /// <summary>Attacking状態の配置位置（下）</summary>
        private static readonly Vector2 AttackingPosition = new Vector2(0f, -3.5f);
        /// <summary>Dead状態の配置位置（左）</summary>
        private static readonly Vector2 DeadPosition = new Vector2(-4f, 0f);
        /// <summary>状態の半径</summary>
        private const float StateRadius = 0.9f;
        /// <summary>Idle状態の色</summary>
        private static readonly Color IdleColor = new Color(0.4f, 0.7f, 0.9f, 1f);
        /// <summary>Walking状態の色</summary>
        private static readonly Color WalkingColor = new Color(0.4f, 0.8f, 0.4f, 1f);
        /// <summary>Attacking状態の色</summary>
        private static readonly Color AttackingColor = new Color(0.9f, 0.4f, 0.3f, 1f);
        /// <summary>Dead状態の色</summary>
        private static readonly Color DeadColor = new Color(0.5f, 0.3f, 0.5f, 1f);
        /// <summary>現在アクティブな状態の識別子</summary>
        private string currentStateId;

        /// <summary>
        /// バインド時に4つの状態と遷移矢印を配置して初期表示を構築する
        /// </summary>
        /// <param name="demo">バインドされたデモ</param>
        protected override void OnBind(IPatternDemo demo) {
            VisualElement idle = AddCircle("idle", "Idle", IdlePosition, StateRadius, IdleColor);
            VisualElement walking = AddCircle("walking", "Walking", WalkingPosition, StateRadius, WalkingColor);
            VisualElement attacking = AddCircle("attacking", "Attacking", AttackingPosition, StateRadius, AttackingColor);
            VisualElement dead = AddCircle("dead", "Dead", DeadPosition, StateRadius, DeadColor);

            AddArrow("idle-walking", idle, walking, ArrowColor);
            AddArrow("idle-attacking", idle, attacking, ArrowColor);
            AddArrow("walking-idle", walking, idle, ArrowColor);
            AddArrow("walking-attacking", walking, attacking, ArrowColor);
            AddArrow("attacking-idle", attacking, idle, ArrowColor);
            AddArrow("attacking-dead", attacking, dead, ArrowColor);

            currentStateId = "idle";
            HighlightState("idle");
        }

        /// <summary>
        /// ステップに応じて状態遷移のアニメーションを更新する
        /// </summary>
        /// <param name="stepIndex">現在のステップインデックス</param>
        protected override void OnRefresh(int stepIndex) {
            switch (stepIndex) {
                case 0:
                    TransitionTo("idle", null);
                    break;
                case 1:
                    TransitionTo("walking", "idle-walking");
                    break;
                case 2:
                    TransitionTo("attacking", "walking-attacking");
                    break;
                case 3:
                    TransitionTo("idle", "attacking-idle");
                    break;
                case 4:
                    TransitionTo("dead", "attacking-dead");
                    break;
                case 5:
                    GetElement("dead")?.Pulse(DimColor, 0.5f);
                    break;
            }
        }

        /// <summary>
        /// 指定の状態へ遷移して矢印をパルスさせる
        /// </summary>
        /// <param name="targetStateId">遷移先の状態識別子</param>
        /// <param name="arrowId">パルスさせる遷移矢印の識別子（nullの場合はパルスなし）</param>
        private void TransitionTo(string targetStateId, string arrowId) {
            DimAllStates();
            HighlightState(targetStateId);
            currentStateId = targetStateId;

            if (arrowId != null) {
                GetArrow(arrowId)?.Pulse(PulseColor, 0.5f);
            }
        }

        /// <summary>
        /// 指定の状態をハイライトする
        /// </summary>
        /// <param name="stateId">ハイライトする状態の識別子</param>
        private void HighlightState(string stateId) {
            VisualElement element = GetElement(stateId);
            if (element != null) {
                element.Pulse(HighlightColor, 0.5f);
                Color stateColor = GetStateColor(stateId);
                element.SetColorImmediate(stateColor);
            }
        }

        /// <summary>
        /// 全状態要素をDim状態にする
        /// </summary>
        private void DimAllStates() {
            GetElement("idle")?.SetColorImmediate(DimColor);
            GetElement("walking")?.SetColorImmediate(DimColor);
            GetElement("attacking")?.SetColorImmediate(DimColor);
            GetElement("dead")?.SetColorImmediate(DimColor);
        }

        /// <summary>
        /// 状態識別子に対応する色を取得する
        /// </summary>
        /// <param name="stateId">状態の識別子</param>
        /// <returns>状態固有の色</returns>
        private static Color GetStateColor(string stateId) {
            switch (stateId) {
                case "idle":
                    return IdleColor;
                case "walking":
                    return WalkingColor;
                case "attacking":
                    return AttackingColor;
                case "dead":
                    return DeadColor;
                default:
                    return DimColor;
            }
        }
    }
}
