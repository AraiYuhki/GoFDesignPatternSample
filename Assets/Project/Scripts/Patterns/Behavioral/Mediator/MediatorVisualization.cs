using UnityEngine;

namespace GoFPatterns.Patterns.Visualization {
    /// <summary>
    /// Mediatorパターンのビジュアライゼーション
    /// 中央にMediator（ChatRoom）を配置し、周囲のユーザー間通信を矢印で可視化する
    /// </summary>
    [PatternVisualization("mediator")]
    public class MediatorVisualization : BasePatternVisualization {
        /// <summary>Mediatorの配置位置</summary>
        private static readonly Vector2 MediatorPosition = new Vector2(0f, 0f);
        /// <summary>Aliceの配置位置</summary>
        private static readonly Vector2 AlicePosition = new Vector2(-4f, 3f);
        /// <summary>Bobの配置位置</summary>
        private static readonly Vector2 BobPosition = new Vector2(4f, 3f);
        /// <summary>Charlieの配置位置</summary>
        private static readonly Vector2 CharliePosition = new Vector2(0f, -3.5f);
        /// <summary>Mediator矩形のサイズ</summary>
        private static readonly Vector2 MediatorSize = new Vector2(3f, 1.5f);
        /// <summary>ユーザーの半径</summary>
        private const float UserRadius = 0.8f;
        /// <summary>Mediatorの色</summary>
        private static readonly Color MediatorColor = new Color(0.6f, 0.5f, 0.7f, 1f);
        /// <summary>Aliceの色</summary>
        private static readonly Color AliceColor = new Color(0.8f, 0.4f, 0.5f, 1f);
        /// <summary>Bobの色</summary>
        private static readonly Color BobColor = new Color(0.4f, 0.6f, 0.8f, 1f);
        /// <summary>Charlieの色</summary>
        private static readonly Color CharlieColor = new Color(0.4f, 0.7f, 0.5f, 1f);

        /// <summary>
        /// バインド時にMediatorとユーザー要素を配置して初期表示を構築する
        /// </summary>
        /// <param name="demo">バインドされたデモ</param>
        protected override void OnBind(IPatternDemo demo) {
            AddRect("mediator", "ChatRoom", MediatorPosition, MediatorSize, MediatorColor);

            VisualElement alice = AddCircle("alice", "Alice", AlicePosition, UserRadius, DimColor);
            VisualElement bob = AddCircle("bob", "Bob", BobPosition, UserRadius, DimColor);
            VisualElement charlie = AddCircle("charlie", "Charlie", CharliePosition, UserRadius, DimColor);

            alice.SetVisible(false);
            bob.SetVisible(false);
            charlie.SetVisible(false);
        }

        /// <summary>
        /// ステップに応じてメッセージ送受信のアニメーションを更新する
        /// </summary>
        /// <param name="stepIndex">現在のステップインデックス</param>
        protected override void OnRefresh(int stepIndex) {
            VisualElement mediator = GetElement("mediator");
            VisualElement alice = GetElement("alice");
            VisualElement bob = GetElement("bob");
            VisualElement charlie = GetElement("charlie");

            switch (stepIndex) {
                case 0:
                    mediator.Pulse(HighlightColor, 0.5f);
                    break;
                case 1:
                    alice.SetVisible(true);
                    bob.SetVisible(true);
                    charlie.SetVisible(true);
                    alice.SetColorImmediate(AliceColor);
                    bob.SetColorImmediate(BobColor);
                    charlie.SetColorImmediate(CharlieColor);
                    alice.Pulse(HighlightColor, 0.5f);
                    bob.Pulse(HighlightColor, 0.5f);
                    charlie.Pulse(HighlightColor, 0.5f);
                    AddArrow("alice-mediator", alice, mediator, ArrowColor);
                    AddArrow("bob-mediator", bob, mediator, ArrowColor);
                    AddArrow("charlie-mediator", charlie, mediator, ArrowColor);
                    AddArrow("mediator-alice", mediator, alice, ArrowColor);
                    AddArrow("mediator-bob", mediator, bob, ArrowColor);
                    AddArrow("mediator-charlie", mediator, charlie, ArrowColor);
                    break;
                case 2:
                    alice.Pulse(PulseColor, 0.5f);
                    GetArrow("alice-mediator")?.Pulse(PulseColor, 0.5f);
                    mediator.Pulse(PulseColor, 0.5f);
                    GetArrow("mediator-bob")?.Pulse(PulseColor, 0.5f);
                    GetArrow("mediator-charlie")?.Pulse(PulseColor, 0.5f);
                    bob.Pulse(PulseColor, 0.5f);
                    charlie.Pulse(PulseColor, 0.5f);
                    break;
                case 3:
                    bob.Pulse(PulseColor, 0.5f);
                    GetArrow("bob-mediator")?.Pulse(PulseColor, 0.5f);
                    mediator.Pulse(PulseColor, 0.5f);
                    GetArrow("mediator-alice")?.Pulse(PulseColor, 0.5f);
                    GetArrow("mediator-charlie")?.Pulse(PulseColor, 0.5f);
                    alice.Pulse(PulseColor, 0.5f);
                    charlie.Pulse(PulseColor, 0.5f);
                    break;
                case 4:
                    mediator.Pulse(HighlightColor, 0.5f);
                    alice.Pulse(HighlightColor, 0.5f);
                    bob.Pulse(HighlightColor, 0.5f);
                    charlie.Pulse(HighlightColor, 0.5f);
                    break;
            }
        }
    }
}
