using UnityEngine;

namespace GoFPatterns.Patterns.Visualization {
    /// <summary>
    /// Visitorパターンのビジュアライゼーション
    /// 左側に図形要素、右側にビジター要素を配置し、訪問時の矢印パルスで可視化する
    /// </summary>
    [PatternVisualization("visitor")]
    public class VisitorVisualization : BasePatternVisualization {
        /// <summary>Circle(r=5)の配置位置</summary>
        private static readonly Vector2 Circle1Position = new Vector2(-4f, 3f);
        /// <summary>Rectangle(4x6)の配置位置</summary>
        private static readonly Vector2 RectPosition = new Vector2(-4f, 0f);
        /// <summary>Circle(r=3)の配置位置</summary>
        private static readonly Vector2 Circle2Position = new Vector2(-4f, -3f);
        /// <summary>AreaCalculatorの配置位置</summary>
        private static readonly Vector2 AreaCalcPosition = new Vector2(4f, 2f);
        /// <summary>DrawingExporterの配置位置</summary>
        private static readonly Vector2 DrawExpPosition = new Vector2(4f, -2f);
        /// <summary>図形の半径</summary>
        private const float ShapeRadius = 0.8f;
        /// <summary>ビジター矩形のサイズ</summary>
        private static readonly Vector2 VisitorSize = new Vector2(3.2f, 1.2f);
        /// <summary>図形の色</summary>
        private static readonly Color ShapeColor = new Color(0.4f, 0.6f, 0.8f, 1f);
        /// <summary>AreaCalculatorの色</summary>
        private static readonly Color AreaCalcColor = new Color(0.4f, 0.7f, 0.5f, 1f);
        /// <summary>DrawingExporterの色</summary>
        private static readonly Color DrawExpColor = new Color(0.7f, 0.5f, 0.4f, 1f);
        /// <summary>図形要素のID配列</summary>
        private static readonly string[] ShapeIds = { "circle1", "rect1", "circle2" };

        /// <summary>
        /// バインド時に図形要素とビジター要素を配置して初期表示を構築する
        /// </summary>
        /// <param name="demo">バインドされたデモ</param>
        protected override void OnBind(IPatternDemo demo) {
            AddCircle("circle1", "Circle\n(r=5)", Circle1Position, ShapeRadius, ShapeColor);
            AddRect("rect1", "Rectangle\n(4x6)", RectPosition, new Vector2(2.2f, 1.2f), ShapeColor);
            AddCircle("circle2", "Circle\n(r=3)", Circle2Position, ShapeRadius, ShapeColor);

            VisualElement areaCalc = AddRect("area-calc", "AreaCalculator", AreaCalcPosition, VisitorSize, DimColor);
            VisualElement drawExp = AddRect("draw-exp", "DrawingExporter", DrawExpPosition, VisitorSize, DimColor);

            areaCalc.SetVisible(false);
            drawExp.SetVisible(false);
        }

        /// <summary>
        /// ステップに応じてビジターの訪問アニメーションを更新する
        /// </summary>
        /// <param name="stepIndex">現在のステップインデックス</param>
        protected override void OnRefresh(int stepIndex) {
            VisualElement areaCalc = GetElement("area-calc");
            VisualElement drawExp = GetElement("draw-exp");

            switch (stepIndex) {
                case 0:
                    for (int i = 0; i < ShapeIds.Length; i++) {
                        GetElement(ShapeIds[i])?.Pulse(HighlightColor, 0.5f);
                    }
                    break;
                case 1:
                    areaCalc.SetVisible(true);
                    areaCalc.SetColorImmediate(AreaCalcColor);
                    areaCalc.Pulse(HighlightColor, 0.5f);
                    break;
                case 2:
                    VisitAllShapes(areaCalc, "area");
                    break;
                case 3:
                    DimVisitArrows("area");
                    drawExp.SetVisible(true);
                    drawExp.SetColorImmediate(DrawExpColor);
                    drawExp.Pulse(HighlightColor, 0.5f);
                    break;
                case 4:
                    VisitAllShapes(drawExp, "draw");
                    break;
                case 5:
                    areaCalc.Pulse(HighlightColor, 0.5f);
                    drawExp.Pulse(HighlightColor, 0.5f);
                    for (int i = 0; i < ShapeIds.Length; i++) {
                        GetElement(ShapeIds[i])?.Pulse(HighlightColor, 0.5f);
                    }
                    break;
            }
        }

        /// <summary>
        /// ビジターが全図形を訪問するアニメーションを実行する
        /// </summary>
        /// <param name="visitor">訪問するビジターの要素</param>
        /// <param name="arrowPrefix">矢印IDのプレフィックス</param>
        private void VisitAllShapes(VisualElement visitor, string arrowPrefix) {
            for (int i = 0; i < ShapeIds.Length; i++) {
                VisualElement shape = GetElement(ShapeIds[i]);
                string arrowId = $"{arrowPrefix}-{ShapeIds[i]}";
                AddArrow(arrowId, visitor, shape, ArrowColor);
                GetArrow(arrowId)?.Pulse(PulseColor, 0.5f);
                shape.Pulse(PulseColor, 0.5f);
            }
            visitor.Pulse(PulseColor, 0.5f);
        }

        /// <summary>
        /// 指定プレフィックスの訪問矢印をDim状態にする
        /// </summary>
        /// <param name="arrowPrefix">矢印IDのプレフィックス</param>
        private void DimVisitArrows(string arrowPrefix) {
            for (int i = 0; i < ShapeIds.Length; i++) {
                string arrowId = $"{arrowPrefix}-{ShapeIds[i]}";
                GetArrow(arrowId)?.SetColor(DimColor);
            }
        }
    }
}
