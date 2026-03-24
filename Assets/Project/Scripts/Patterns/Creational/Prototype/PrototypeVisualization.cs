using UnityEngine;

namespace GoFPatterns.Patterns.Visualization {
    /// <summary>
    /// Prototypeパターンのビジュアライゼーション
    /// オリジナル・レジストリ・クローン・バリアントの関係をステップごとに可視化する
    /// </summary>
    [PatternVisualization("prototype")]
    public class PrototypeVisualization : BasePatternVisualization {
        /// <summary>Original円の位置</summary>
        private static readonly Vector2 OriginalPosition = new Vector2(-3.5f, 0f);
        /// <summary>Registry矩形の位置</summary>
        private static readonly Vector2 RegistryPosition = new Vector2(0f, 3.5f);
        /// <summary>Registry矩形のサイズ</summary>
        private static readonly Vector2 RegistrySize = new Vector2(2.8f, 1.3f);
        /// <summary>Clone円の位置</summary>
        private static readonly Vector2 ClonePosition = new Vector2(3f, 1f);
        /// <summary>Variant円の位置</summary>
        private static readonly Vector2 VariantPosition = new Vector2(3f, -2.5f);
        /// <summary>円の半径</summary>
        private const float CircleRadius = 0.9f;
        /// <summary>パルスアニメーションの秒数</summary>
        private const float PulseDuration = 0.5f;

        /// <summary>Originalの色（緑）</summary>
        private static readonly Color OriginalColor = new Color(0.2f, 0.7f, 0.3f, 1f);
        /// <summary>Registryの色</summary>
        private static readonly Color RegistryColor = new Color(0.4f, 0.4f, 0.6f, 1f);
        /// <summary>Cloneの色（緑、初期状態）</summary>
        private static readonly Color CloneColor = new Color(0.2f, 0.7f, 0.3f, 1f);
        /// <summary>Clone変更後の色（赤）</summary>
        private static readonly Color CloneModifiedColor = new Color(0.8f, 0.3f, 0.3f, 1f);
        /// <summary>Variantの色（金）</summary>
        private static readonly Color VariantColor = new Color(0.9f, 0.75f, 0.2f, 1f);

        /// <summary>
        /// バインド時に全要素を非表示で配置する
        /// </summary>
        /// <param name="demo">バインドされたデモ</param>
        protected override void OnBind(IPatternDemo demo) {
            VisualElement original = AddCircle("original", "Original\n(Green)", OriginalPosition, CircleRadius, OriginalColor);
            original.SetVisible(false);

            VisualElement registry = AddRect("registry", "Registry", RegistryPosition, RegistrySize, RegistryColor);
            registry.SetVisible(false);

            VisualElement clone = AddCircle("clone", "Clone", ClonePosition, CircleRadius, CloneColor);
            clone.SetVisible(false);

            VisualElement variant = AddCircle("variant", "Variant", VariantPosition, CircleRadius, VariantColor);
            variant.SetVisible(false);

            VisualArrow arrowOriginalToRegistry = AddArrow("arrowOrigToReg", original, registry, ArrowColor);
            arrowOriginalToRegistry.gameObject.SetActive(false);

            VisualArrow arrowRegistryToClone = AddArrow("arrowRegToClone", registry, clone, ArrowColor);
            arrowRegistryToClone.gameObject.SetActive(false);

            VisualArrow arrowRegistryToVariant = AddArrow("arrowRegToVariant", registry, variant, ArrowColor);
            arrowRegistryToVariant.gameObject.SetActive(false);
        }

        /// <summary>
        /// ステップに応じて要素の表示とアニメーションを更新する
        /// </summary>
        /// <param name="stepIndex">現在のステップインデックス</param>
        protected override void OnRefresh(int stepIndex) {
            VisualElement original = GetElement("original");
            VisualElement registry = GetElement("registry");
            VisualElement clone = GetElement("clone");
            VisualElement variant = GetElement("variant");
            VisualArrow arrowOrigToReg = GetArrow("arrowOrigToReg");
            VisualArrow arrowRegToClone = GetArrow("arrowRegToClone");
            VisualArrow arrowRegToVariant = GetArrow("arrowRegToVariant");

            switch (stepIndex) {
                case 0:
                    original.SetVisible(true);
                    original.Pulse(PulseColor, PulseDuration);
                    registry.SetVisible(true);
                    registry.Pulse(PulseColor, PulseDuration);
                    arrowOrigToReg.gameObject.SetActive(true);
                    arrowOrigToReg.Pulse(PulseColor, PulseDuration);
                    registry.SetLabel("Registry\n(slime)");
                    break;
                case 1:
                    registry.Pulse(HighlightColor, PulseDuration);
                    arrowRegToClone.gameObject.SetActive(true);
                    arrowRegToClone.Pulse(PulseColor, PulseDuration);
                    clone.SetVisible(true);
                    clone.Pulse(PulseColor, PulseDuration);
                    clone.SetLabel("Clone\n(Green)");
                    break;
                case 2:
                    original.Pulse(HighlightColor, PulseDuration);
                    clone.Pulse(HighlightColor, PulseDuration);
                    original.SetLabel("Original\n(separate)");
                    clone.SetLabel("Clone\n(separate)");
                    break;
                case 3:
                    clone.SetColorImmediate(CloneModifiedColor);
                    clone.SetLabel("Clone\n(Red)\nHP=80");
                    clone.Pulse(HighlightColor, PulseDuration);
                    break;
                case 4:
                    original.SetLabel("Original\n(Green)\nHP=50");
                    original.Pulse(PulseColor, PulseDuration);
                    break;
                case 5:
                    registry.Pulse(HighlightColor, PulseDuration);
                    arrowRegToVariant.gameObject.SetActive(true);
                    arrowRegToVariant.Pulse(PulseColor, PulseDuration);
                    variant.SetVisible(true);
                    variant.Pulse(PulseColor, PulseDuration);
                    variant.SetLabel("Variant\n(Gold)\nHP=120");
                    break;
            }
        }
    }
}
