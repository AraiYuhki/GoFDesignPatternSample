using UnityEngine;

namespace GoFPatterns.Patterns.Visualization {
    /// <summary>
    /// Abstract Factoryパターンのビジュアライゼーション
    /// Dark/Lightの2つのファクトリ列でButton/Dialogの生成過程を可視化する
    /// </summary>
    [PatternVisualization("abstract-factory")]
    public class AbstractFactoryVisualization : BasePatternVisualization {
        /// <summary>DarkFactory矩形の位置</summary>
        private static readonly Vector2 DarkFactoryPosition = new Vector2(-3.5f, 3f);
        /// <summary>LightFactory矩形の位置</summary>
        private static readonly Vector2 LightFactoryPosition = new Vector2(3.5f, 3f);
        /// <summary>Factory矩形のサイズ</summary>
        private static readonly Vector2 FactorySize = new Vector2(2.8f, 1.3f);
        /// <summary>DarkButton矩形の位置</summary>
        private static readonly Vector2 DarkButtonPosition = new Vector2(-3.5f, 0.5f);
        /// <summary>DarkDialog矩形の位置</summary>
        private static readonly Vector2 DarkDialogPosition = new Vector2(-3.5f, -2.5f);
        /// <summary>LightButton矩形の位置</summary>
        private static readonly Vector2 LightButtonPosition = new Vector2(3.5f, 0.5f);
        /// <summary>LightDialog矩形の位置</summary>
        private static readonly Vector2 LightDialogPosition = new Vector2(3.5f, -2.5f);
        /// <summary>Product矩形のサイズ</summary>
        private static readonly Vector2 ProductSize = new Vector2(2.5f, 1.2f);
        /// <summary>パルスアニメーションの秒数</summary>
        private const float PulseDuration = 0.5f;

        /// <summary>DarkFactoryの色</summary>
        private static readonly Color DarkFactoryColor = new Color(0.3f, 0.2f, 0.5f, 1f);
        /// <summary>DarkProductの色</summary>
        private static readonly Color DarkProductColor = new Color(0.4f, 0.3f, 0.6f, 1f);
        /// <summary>LightFactoryの色</summary>
        private static readonly Color LightFactoryColor = new Color(0.3f, 0.6f, 0.8f, 1f);
        /// <summary>LightProductの色</summary>
        private static readonly Color LightProductColor = new Color(0.4f, 0.7f, 0.9f, 1f);

        /// <summary>
        /// バインド時に全要素を非表示で配置する
        /// </summary>
        /// <param name="demo">バインドされたデモ</param>
        protected override void OnBind(IPatternDemo demo) {
            VisualElement darkFactory = AddRect("darkFactory", "Dark\nFactory", DarkFactoryPosition, FactorySize, DarkFactoryColor);
            darkFactory.SetVisible(false);

            VisualElement darkButton = AddRect("darkButton", "Dark\nButton", DarkButtonPosition, ProductSize, DarkProductColor);
            darkButton.SetVisible(false);

            VisualElement darkDialog = AddRect("darkDialog", "Dark\nDialog", DarkDialogPosition, ProductSize, DarkProductColor);
            darkDialog.SetVisible(false);

            VisualElement lightFactory = AddRect("lightFactory", "Light\nFactory", LightFactoryPosition, FactorySize, LightFactoryColor);
            lightFactory.SetVisible(false);

            VisualElement lightButton = AddRect("lightButton", "Light\nButton", LightButtonPosition, ProductSize, LightProductColor);
            lightButton.SetVisible(false);

            VisualElement lightDialog = AddRect("lightDialog", "Light\nDialog", LightDialogPosition, ProductSize, LightProductColor);
            lightDialog.SetVisible(false);

            VisualArrow arrowDarkButton = AddArrow("arrowDarkButton", darkFactory, darkButton, ArrowColor);
            arrowDarkButton.gameObject.SetActive(false);

            VisualArrow arrowDarkDialog = AddArrow("arrowDarkDialog", darkFactory, darkDialog, ArrowColor);
            arrowDarkDialog.gameObject.SetActive(false);

            VisualArrow arrowLightButton = AddArrow("arrowLightButton", lightFactory, lightButton, ArrowColor);
            arrowLightButton.gameObject.SetActive(false);

            VisualArrow arrowLightDialog = AddArrow("arrowLightDialog", lightFactory, lightDialog, ArrowColor);
            arrowLightDialog.gameObject.SetActive(false);
        }

        /// <summary>
        /// ステップに応じて要素の表示とアニメーションを更新する
        /// </summary>
        /// <param name="stepIndex">現在のステップインデックス</param>
        protected override void OnRefresh(int stepIndex) {
            VisualElement darkFactory = GetElement("darkFactory");
            VisualElement darkButton = GetElement("darkButton");
            VisualElement darkDialog = GetElement("darkDialog");
            VisualElement lightFactory = GetElement("lightFactory");
            VisualElement lightButton = GetElement("lightButton");
            VisualElement lightDialog = GetElement("lightDialog");
            VisualArrow arrowDarkButton = GetArrow("arrowDarkButton");
            VisualArrow arrowDarkDialog = GetArrow("arrowDarkDialog");
            VisualArrow arrowLightButton = GetArrow("arrowLightButton");
            VisualArrow arrowLightDialog = GetArrow("arrowLightDialog");

            switch (stepIndex) {
                case 0:
                    darkFactory.SetVisible(true);
                    darkFactory.Pulse(PulseColor, PulseDuration);
                    break;
                case 1:
                    darkFactory.Pulse(HighlightColor, PulseDuration);
                    darkButton.SetVisible(true);
                    darkButton.Pulse(PulseColor, PulseDuration);
                    arrowDarkButton.gameObject.SetActive(true);
                    arrowDarkButton.Pulse(PulseColor, PulseDuration);
                    break;
                case 2:
                    darkFactory.Pulse(HighlightColor, PulseDuration);
                    darkDialog.SetVisible(true);
                    darkDialog.Pulse(PulseColor, PulseDuration);
                    arrowDarkDialog.gameObject.SetActive(true);
                    arrowDarkDialog.Pulse(PulseColor, PulseDuration);
                    break;
                case 3:
                    darkFactory.SetColorImmediate(DimColor);
                    darkButton.SetColorImmediate(DimColor);
                    darkDialog.SetColorImmediate(DimColor);
                    lightFactory.SetVisible(true);
                    lightFactory.Pulse(PulseColor, PulseDuration);
                    break;
                case 4:
                    lightFactory.Pulse(HighlightColor, PulseDuration);
                    lightButton.SetVisible(true);
                    lightButton.Pulse(PulseColor, PulseDuration);
                    arrowLightButton.gameObject.SetActive(true);
                    arrowLightButton.Pulse(PulseColor, PulseDuration);
                    break;
                case 5:
                    lightFactory.Pulse(HighlightColor, PulseDuration);
                    lightDialog.SetVisible(true);
                    lightDialog.Pulse(PulseColor, PulseDuration);
                    arrowLightDialog.gameObject.SetActive(true);
                    arrowLightDialog.Pulse(PulseColor, PulseDuration);
                    break;
                case 6:
                    darkFactory.SetColorImmediate(DarkFactoryColor);
                    darkButton.SetColorImmediate(DarkProductColor);
                    darkDialog.SetColorImmediate(DarkProductColor);
                    darkFactory.Pulse(HighlightColor, PulseDuration);
                    darkButton.Pulse(HighlightColor, PulseDuration);
                    darkDialog.Pulse(HighlightColor, PulseDuration);
                    lightFactory.Pulse(HighlightColor, PulseDuration);
                    lightButton.Pulse(HighlightColor, PulseDuration);
                    lightDialog.Pulse(HighlightColor, PulseDuration);
                    break;
            }
        }
    }
}
