namespace GoFPatterns.Patterns {
    // ---- Implementation interface ----

    /// <summary>
    /// Bridgeパターンの実装側インターフェース
    /// 図形の描画方法を抽象化する
    /// </summary>
    public interface IRenderer {
        /// <summary>レンダラーの名前を取得する</summary>
        string Name { get; }

        /// <summary>
        /// 指定された図形を描画する
        /// </summary>
        /// <param name="shapeName">描画する図形の名前</param>
        /// <returns>描画結果の説明文</returns>
        string RenderShape(string shapeName);
    }

    // ---- Concrete Implementations ----

    /// <summary>ベクター方式で図形を描画するレンダラー</summary>
    public class VectorRenderer : IRenderer {
        /// <summary>レンダラーの名前</summary>
        public string Name => "VectorRenderer";

        /// <summary>
        /// ベクター方式で図形を描画する
        /// </summary>
        /// <param name="shapeName">描画する図形の名前</param>
        /// <returns>描画結果の説明文</returns>
        public string RenderShape(string shapeName) {
            return $"{shapeName} をベクター描画（線分と制御点で構成）";
        }
    }

    /// <summary>ラスター方式で図形を描画するレンダラー</summary>
    public class RasterRenderer : IRenderer {
        /// <summary>レンダラーの名前</summary>
        public string Name => "RasterRenderer";

        /// <summary>
        /// ラスター方式で図形を描画する
        /// </summary>
        /// <param name="shapeName">描画する図形の名前</param>
        /// <returns>描画結果の説明文</returns>
        public string RenderShape(string shapeName) {
            return $"{shapeName} をラスター描画（ピクセルで構成）";
        }
    }

    // ---- Abstraction ----

    /// <summary>
    /// Bridgeパターンの抽象側基底クラス
    /// IRendererへの参照を保持し、描画処理を委譲する
    /// </summary>
    public abstract class Shape {
        /// <summary>描画を委譲するレンダラー</summary>
        private IRenderer renderer;

        /// <summary>図形の名前を取得する</summary>
        public abstract string ShapeName { get; }

        /// <summary>現在のレンダラーを取得する</summary>
        public IRenderer CurrentRenderer => renderer;

        /// <summary>
        /// Shapeを生成する
        /// </summary>
        /// <param name="renderer">描画を委譲するレンダラー</param>
        protected Shape(IRenderer renderer) {
            this.renderer = renderer;
        }

        /// <summary>
        /// レンダラーを切り替える
        /// </summary>
        /// <param name="newRenderer">新しいレンダラー</param>
        public void SetRenderer(IRenderer newRenderer) {
            renderer = newRenderer;
        }

        /// <summary>
        /// 図形を描画する
        /// </summary>
        /// <returns>描画結果の説明文</returns>
        public string Draw() {
            return renderer.RenderShape(ShapeName);
        }
    }

    // ---- Refined Abstractions ----

    /// <summary>円を表す図形クラス</summary>
    public class CircleShape : Shape {
        /// <summary>図形の名前</summary>
        public override string ShapeName => "Circle";

        /// <summary>
        /// CircleShapeを生成する
        /// </summary>
        /// <param name="renderer">描画を委譲するレンダラー</param>
        public CircleShape(IRenderer renderer) : base(renderer) { }
    }

    /// <summary>四角形を表す図形クラス</summary>
    public class SquareShape : Shape {
        /// <summary>図形の名前</summary>
        public override string ShapeName => "Square";

        /// <summary>
        /// SquareShapeを生成する
        /// </summary>
        /// <param name="renderer">描画を委譲するレンダラー</param>
        public SquareShape(IRenderer renderer) : base(renderer) { }
    }

    // ---- Demo ----

    /// <summary>
    /// Bridgeパターンのデモ
    /// 図形（抽象）と描画方式（実装）を独立に組み合わせられることを示す
    /// </summary>
    [PatternDemo("bridge")]
    public class BridgeDemo : BasePatternDemo {
        /// <summary>デモのパターンID</summary>
        public override string PatternId => "bridge";

        /// <summary>デモの表示名</summary>
        public override string DisplayName => "Bridge";

        /// <summary>ベクターレンダラー</summary>
        private VectorRenderer vectorRenderer;

        /// <summary>ラスターレンダラー</summary>
        private RasterRenderer rasterRenderer;

        /// <summary>円の図形</summary>
        private CircleShape circle;

        /// <summary>四角形の図形</summary>
        private SquareShape square;

        /// <summary>
        /// リセット時にドメインオブジェクトをクリアする
        /// </summary>
        protected override void OnReset() {
            vectorRenderer = null;
            rasterRenderer = null;
            circle = null;
            square = null;
        }

        /// <summary>
        /// Bridgeパターンのシナリオを構築する
        /// </summary>
        /// <param name="scenario">ステップを追加するシナリオ</param>
        protected override void BuildScenario(DemoScenario scenario) {
            vectorRenderer = new VectorRenderer();
            rasterRenderer = new RasterRenderer();

            scenario.AddStep(new DemoStep(
                "VectorRendererを作成する",
                () => {
                    Log("Client", "new VectorRenderer()", "ベクター描画エンジンを生成");
                }
            ));

            scenario.AddStep(new DemoStep(
                "CircleをVectorRendererで作成する",
                () => {
                    circle = new CircleShape(vectorRenderer);
                    Log("Client", "new CircleShape(vectorRenderer)", $"レンダラー: {circle.CurrentRenderer.Name}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "CircleをVectorRendererで描画する",
                () => {
                    string result = circle.Draw();
                    Log("Circle", "Draw()", result);
                }
            ));

            scenario.AddStep(new DemoStep(
                "CircleのレンダラーをRasterRendererに切り替える",
                () => {
                    circle.SetRenderer(rasterRenderer);
                    Log("Client", "circle.SetRenderer(rasterRenderer)", $"レンダラー: {circle.CurrentRenderer.Name}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "同じCircleをRasterRendererで描画する",
                () => {
                    string result = circle.Draw();
                    Log("Circle", "Draw()", result);
                }
            ));

            scenario.AddStep(new DemoStep(
                "SquareをRasterRendererで作成する",
                () => {
                    square = new SquareShape(rasterRenderer);
                    Log("Client", "new SquareShape(rasterRenderer)", $"レンダラー: {square.CurrentRenderer.Name}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "SquareをRasterRendererで描画する",
                () => {
                    string result = square.Draw();
                    Log("Square", "Draw()", result);
                }
            ));
        }
    }
}
