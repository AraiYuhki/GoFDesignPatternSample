using UnityEngine;

namespace GoFPatterns.Patterns.Visualization {
    /// <summary>
    /// Bridgeパターンのビジュアライゼーション
    /// 抽象（Shape）と実装（Renderer）を独立に組み合わせる構造をステップ実行で可視化する
    /// </summary>
    [PatternVisualization("bridge")]
    public class BridgeVisualization : BasePatternVisualization {
        /// <summary>Circle抽象の表示位置</summary>
        private static readonly Vector2 CirclePosition = new Vector2(-3.5f, 2.0f);

        /// <summary>Square抽象の表示位置</summary>
        private static readonly Vector2 SquarePosition = new Vector2(-3.5f, -2.0f);

        /// <summary>VectorRenderer実装の表示位置</summary>
        private static readonly Vector2 VectorPosition = new Vector2(3.5f, 2.0f);

        /// <summary>RasterRenderer実装の表示位置</summary>
        private static readonly Vector2 RasterPosition = new Vector2(3.5f, -2.0f);

        /// <summary>抽象側ラベルの表示位置</summary>
        private static readonly Vector2 AbstractionLabelPosition = new Vector2(-3.5f, 4.2f);

        /// <summary>実装側ラベルの表示位置</summary>
        private static readonly Vector2 ImplementationLabelPosition = new Vector2(3.5f, 4.2f);

        /// <summary>要素の矩形サイズ</summary>
        private static readonly Vector2 RectSize = new Vector2(2.8f, 1.4f);

        /// <summary>ラベルの矩形サイズ</summary>
        private static readonly Vector2 LabelSize = new Vector2(3.0f, 0.8f);

        /// <summary>
        /// バインド時に初期レイアウトを構築する
        /// </summary>
        /// <param name="demo">バインドされたデモ</param>
        protected override void OnBind(IPatternDemo demo) {
            AddRect("absLabel", "Abstraction", AbstractionLabelPosition, LabelSize, new Color(0.2f, 0.3f, 0.5f, 1f));
            AddRect("impLabel", "Implementation", ImplementationLabelPosition, LabelSize, new Color(0.2f, 0.3f, 0.5f, 1f));

            VisualElement circle = AddRect("circle", "Circle", CirclePosition, RectSize, DimColor);
            VisualElement square = AddRect("square", "Square", SquarePosition, RectSize, DimColor);
            VisualElement vector = AddRect("vector", "VectorRenderer", VectorPosition, RectSize, DimColor);
            VisualElement raster = AddRect("raster", "RasterRenderer", RasterPosition, RectSize, DimColor);

            circle.SetVisible(false);
            square.SetVisible(false);

            AddArrow("circleToVector", circle, vector, ArrowColor);
            AddArrow("circleToRaster", circle, raster, ArrowColor);
            AddArrow("squareToRaster", square, raster, ArrowColor);

            GetArrow("circleToVector").SetColor(DimColor);
            GetArrow("circleToRaster").SetColor(DimColor);
            GetArrow("squareToRaster").SetColor(DimColor);
        }

        /// <summary>
        /// ステップごとの表示更新を行う
        /// </summary>
        /// <param name="stepIndex">現在のステップインデックス</param>
        protected override void OnRefresh(int stepIndex) {
            switch (stepIndex) {
                case 0:
                    RefreshStep0();
                    break;
                case 1:
                    RefreshStep1();
                    break;
                case 2:
                    RefreshStep2();
                    break;
                case 3:
                    RefreshStep3();
                    break;
                case 4:
                    RefreshStep4();
                    break;
                case 5:
                    RefreshStep5();
                    break;
                case 6:
                    RefreshStep6();
                    break;
            }
        }

        /// <summary>
        /// Step0: VectorRendererを作成して表示する
        /// </summary>
        private void RefreshStep0() {
            VisualElement vector = GetElement("vector");
            vector.SetColorImmediate(HighlightColor);
            vector.Pulse(HighlightColor, 0.6f);
        }

        /// <summary>
        /// Step1: CircleをVectorRendererで作成する
        /// </summary>
        private void RefreshStep1() {
            VisualElement circle = GetElement("circle");
            circle.SetVisible(true);
            circle.SetColorImmediate(new Color(0.4f, 0.6f, 0.9f, 1f));
            circle.Pulse(HighlightColor, 0.6f);

            VisualArrow arrow = GetArrow("circleToVector");
            arrow.SetColor(ArrowColor);
            arrow.Pulse(PulseColor, 0.6f);
        }

        /// <summary>
        /// Step2: CircleをVectorRendererで描画する
        /// </summary>
        private void RefreshStep2() {
            GetElement("circle").Pulse(PulseColor, 0.5f);
            GetArrow("circleToVector").Pulse(PulseColor, 0.6f);
            GetElement("vector").Pulse(PulseColor, 0.5f);
            GetElement("vector").SetLabel("VectorRenderer\nCircle描画中");
        }

        /// <summary>
        /// Step3: CircleのレンダラーをRasterRendererに切り替える
        /// </summary>
        private void RefreshStep3() {
            GetArrow("circleToVector").SetColor(DimColor);

            VisualArrow arrow = GetArrow("circleToRaster");
            arrow.SetColor(HighlightColor);
            arrow.Pulse(HighlightColor, 0.6f);

            VisualElement raster = GetElement("raster");
            raster.SetColorImmediate(HighlightColor);
            raster.Pulse(HighlightColor, 0.6f);

            GetElement("circle").Pulse(HighlightColor, 0.5f);

            GetElement("vector").SetLabel("VectorRenderer");
        }

        /// <summary>
        /// Step4: 同じCircleをRasterRendererで描画する
        /// </summary>
        private void RefreshStep4() {
            GetElement("circle").Pulse(PulseColor, 0.5f);
            GetArrow("circleToRaster").Pulse(PulseColor, 0.6f);
            GetElement("raster").Pulse(PulseColor, 0.5f);
            GetElement("raster").SetLabel("RasterRenderer\nCircle描画中");
        }

        /// <summary>
        /// Step5: SquareをRasterRendererで作成する
        /// </summary>
        private void RefreshStep5() {
            VisualElement square = GetElement("square");
            square.SetVisible(true);
            square.SetColorImmediate(new Color(0.9f, 0.5f, 0.3f, 1f));
            square.Pulse(HighlightColor, 0.6f);

            VisualArrow arrow = GetArrow("squareToRaster");
            arrow.SetColor(ArrowColor);
            arrow.Pulse(PulseColor, 0.6f);

            GetElement("raster").SetLabel("RasterRenderer");
        }

        /// <summary>
        /// Step6: SquareをRasterRendererで描画する
        /// </summary>
        private void RefreshStep6() {
            GetElement("square").Pulse(PulseColor, 0.5f);
            GetArrow("squareToRaster").Pulse(PulseColor, 0.6f);
            GetElement("raster").Pulse(PulseColor, 0.5f);
            GetElement("raster").SetLabel("RasterRenderer\nSquare描画中");
        }
    }
}
