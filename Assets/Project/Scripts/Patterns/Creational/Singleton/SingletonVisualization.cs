using UnityEngine;

namespace GoFPatterns.Patterns.Visualization {
    /// <summary>
    /// Singletonパターンのビジュアライゼーション
    /// 中央のInstanceを2つのクライアントが共有する様子を2Dで可視化する
    /// </summary>
    [PatternVisualization("singleton")]
    public class SingletonVisualization : BasePatternVisualization {
        /// <summary>Instance矩形の位置</summary>
        private static readonly Vector2 InstancePosition = new Vector2(0f, -0.5f);
        /// <summary>Instance矩形のサイズ</summary>
        private static readonly Vector2 InstanceSize = new Vector2(3f, 1.5f);
        /// <summary>Client A円の位置</summary>
        private static readonly Vector2 ClientAPosition = new Vector2(-3.5f, 3f);
        /// <summary>Client B円の位置</summary>
        private static readonly Vector2 ClientBPosition = new Vector2(3.5f, 3f);
        /// <summary>クライアント円の半径</summary>
        private const float ClientRadius = 0.8f;
        /// <summary>パルスアニメーションの秒数</summary>
        private const float PulseDuration = 0.5f;

        /// <summary>Instanceの色</summary>
        private static readonly Color InstanceColor = new Color(0.2f, 0.5f, 0.8f, 1f);
        /// <summary>Client Aの色</summary>
        private static readonly Color ClientAColor = new Color(0.3f, 0.7f, 0.4f, 1f);
        /// <summary>Client Bの色</summary>
        private static readonly Color ClientBColor = new Color(0.7f, 0.3f, 0.5f, 1f);

        /// <summary>
        /// バインド時に全要素を非表示で配置する
        /// </summary>
        /// <param name="demo">バインドされたデモ</param>
        protected override void OnBind(IPatternDemo demo) {
            VisualElement instance = AddRect("instance", "Instance", InstancePosition, InstanceSize, InstanceColor);
            instance.SetVisible(false);

            VisualElement clientA = AddCircle("clientA", "Client A", ClientAPosition, ClientRadius, ClientAColor);
            clientA.SetVisible(false);

            VisualElement clientB = AddCircle("clientB", "Client B", ClientBPosition, ClientRadius, ClientBColor);
            clientB.SetVisible(false);

            VisualArrow arrowA = AddArrow("arrowA", clientA, instance, ArrowColor);
            arrowA.gameObject.SetActive(false);

            VisualArrow arrowB = AddArrow("arrowB", clientB, instance, ArrowColor);
            arrowB.gameObject.SetActive(false);
        }

        /// <summary>
        /// ステップに応じて要素の表示とアニメーションを更新する
        /// </summary>
        /// <param name="stepIndex">現在のステップインデックス</param>
        protected override void OnRefresh(int stepIndex) {
            VisualElement instance = GetElement("instance");
            VisualElement clientA = GetElement("clientA");
            VisualElement clientB = GetElement("clientB");
            VisualArrow arrowA = GetArrow("arrowA");
            VisualArrow arrowB = GetArrow("arrowB");

            switch (stepIndex) {
                case 0:
                    instance.SetVisible(true);
                    instance.SetLabel("Instance\n(new)");
                    instance.Pulse(PulseColor, PulseDuration);
                    clientA.SetVisible(true);
                    clientA.Pulse(HighlightColor, PulseDuration);
                    arrowA.gameObject.SetActive(true);
                    arrowA.Pulse(PulseColor, PulseDuration);
                    break;
                case 1:
                    instance.SetLabel("Instance");
                    clientB.SetVisible(true);
                    clientB.Pulse(HighlightColor, PulseDuration);
                    arrowB.gameObject.SetActive(true);
                    arrowB.Pulse(PulseColor, PulseDuration);
                    break;
                case 2:
                    instance.Pulse(HighlightColor, PulseDuration);
                    instance.SetLabel("Instance\n(same)");
                    arrowA.Pulse(HighlightColor, PulseDuration);
                    arrowB.Pulse(HighlightColor, PulseDuration);
                    break;
                case 3:
                    instance.SetLabel("Instance\nScore=500");
                    clientA.Pulse(HighlightColor, PulseDuration);
                    arrowA.Pulse(PulseColor, PulseDuration);
                    instance.Pulse(PulseColor, PulseDuration);
                    break;
                case 4:
                    instance.SetLabel("Instance\nLevel=2");
                    clientB.Pulse(HighlightColor, PulseDuration);
                    arrowB.Pulse(PulseColor, PulseDuration);
                    instance.Pulse(PulseColor, PulseDuration);
                    break;
            }
        }
    }
}
