using System.Collections.Generic;
using System.Text;

namespace GoFPatterns.Patterns {
    // ---- Visitor interface ----

    /// <summary>
    /// Visitorパターンのビジターインターフェース
    /// 各図形に対する操作を定義する
    /// </summary>
    public interface IShapeVisitor {
        /// <summary>ビジターの名前を取得する</summary>
        string Name { get; }
        /// <summary>
        /// Circleを訪問する
        /// </summary>
        /// <param name="circle">訪問対象のCircle</param>
        /// <returns>訪問結果の説明文</returns>
        string Visit(VisitorCircle circle);
        /// <summary>
        /// Rectangleを訪問する
        /// </summary>
        /// <param name="rectangle">訪問対象のRectangle</param>
        /// <returns>訪問結果の説明文</returns>
        string Visit(VisitorRectangle rectangle);
    }

    // ---- Element interface ----

    /// <summary>
    /// Visitorパターンの要素インターフェース
    /// ビジターを受け入れるAcceptメソッドを定義する
    /// </summary>
    public interface IVisitableShape {
        /// <summary>図形の名前を取得する</summary>
        string ShapeName { get; }
        /// <summary>
        /// ビジターを受け入れて処理を委譲する
        /// </summary>
        /// <param name="visitor">受け入れるビジター</param>
        /// <returns>訪問結果の説明文</returns>
        string Accept(IShapeVisitor visitor);
    }

    // ---- ConcreteElements ----

    /// <summary>
    /// Visitorパターンの具象要素
    /// 半径を持つ円
    /// </summary>
    public class VisitorCircle : IVisitableShape {
        /// <summary>円の半径</summary>
        private readonly double radius;

        /// <summary>円の半径を取得する</summary>
        public double Radius => radius;
        /// <summary>図形の名前を取得する</summary>
        public string ShapeName => $"Circle(r={radius})";

        /// <summary>
        /// VisitorCircleを生成する
        /// </summary>
        /// <param name="radius">半径</param>
        public VisitorCircle(double radius) {
            this.radius = radius;
        }

        /// <summary>
        /// ビジターを受け入れて処理を委譲する
        /// </summary>
        /// <param name="visitor">受け入れるビジター</param>
        /// <returns>訪問結果の説明文</returns>
        public string Accept(IShapeVisitor visitor) {
            return visitor.Visit(this);
        }
    }

    /// <summary>
    /// Visitorパターンの具象要素
    /// 幅と高さを持つ長方形
    /// </summary>
    public class VisitorRectangle : IVisitableShape {
        /// <summary>長方形の幅</summary>
        private readonly double width;
        /// <summary>長方形の高さ</summary>
        private readonly double height;

        /// <summary>長方形の幅を取得する</summary>
        public double Width => width;
        /// <summary>長方形の高さを取得する</summary>
        public double Height => height;
        /// <summary>図形の名前を取得する</summary>
        public string ShapeName => $"Rectangle(w={width}, h={height})";

        /// <summary>
        /// VisitorRectangleを生成する
        /// </summary>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        public VisitorRectangle(double width, double height) {
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// ビジターを受け入れて処理を委譲する
        /// </summary>
        /// <param name="visitor">受け入れるビジター</param>
        /// <returns>訪問結果の説明文</returns>
        public string Accept(IShapeVisitor visitor) {
            return visitor.Visit(this);
        }
    }

    // ---- ConcreteVisitors ----

    /// <summary>
    /// Visitorパターンの具象ビジター
    /// 各図形の面積を計算する
    /// </summary>
    public class AreaCalculator : IShapeVisitor {
        /// <summary>円周率の近似値</summary>
        private const double Pi = 3.14159265;

        /// <summary>ビジターの名前</summary>
        public string Name => "AreaCalculator";

        /// <summary>
        /// Circleの面積を計算する
        /// </summary>
        /// <param name="circle">対象のCircle</param>
        /// <returns>計算結果の説明文</returns>
        public string Visit(VisitorCircle circle) {
            double area = Pi * circle.Radius * circle.Radius;
            return $"{circle.ShapeName} の面積: {area:F2}";
        }

        /// <summary>
        /// Rectangleの面積を計算する
        /// </summary>
        /// <param name="rectangle">対象のRectangle</param>
        /// <returns>計算結果の説明文</returns>
        public string Visit(VisitorRectangle rectangle) {
            double area = rectangle.Width * rectangle.Height;
            return $"{rectangle.ShapeName} の面積: {area:F2}";
        }
    }

    /// <summary>
    /// Visitorパターンの具象ビジター
    /// 各図形の描画データをエクスポートする
    /// </summary>
    public class DrawingExporter : IShapeVisitor {
        /// <summary>ビジターの名前</summary>
        public string Name => "DrawingExporter";

        /// <summary>
        /// Circleの描画データをエクスポートする
        /// </summary>
        /// <param name="circle">対象のCircle</param>
        /// <returns>エクスポート結果の説明文</returns>
        public string Visit(VisitorCircle circle) {
            return $"{circle.ShapeName} → SVG: <circle r=\"{circle.Radius}\"/>";
        }

        /// <summary>
        /// Rectangleの描画データをエクスポートする
        /// </summary>
        /// <param name="rectangle">対象のRectangle</param>
        /// <returns>エクスポート結果の説明文</returns>
        public string Visit(VisitorRectangle rectangle) {
            return $"{rectangle.ShapeName} → SVG: <rect w=\"{rectangle.Width}\" h=\"{rectangle.Height}\"/>";
        }
    }

    // ---- Demo ----

    /// <summary>
    /// Visitorパターンのデモ
    /// 図形コレクションに対して異なるビジターを適用し、開放閉鎖原則を示す
    /// </summary>
    [PatternDemo("visitor")]
    public class VisitorDemo : BasePatternDemo {
        /// <summary>デモのパターンID</summary>
        public override string PatternId => "visitor";

        /// <summary>デモの表示名</summary>
        public override string DisplayName => "Visitor";

        /// <summary>図形のコレクション</summary>
        private readonly List<IVisitableShape> shapes = new List<IVisitableShape>();
        /// <summary>面積計算ビジター</summary>
        private AreaCalculator areaCalculator;
        /// <summary>描画エクスポートビジター</summary>
        private DrawingExporter drawingExporter;

        /// <summary>
        /// リセット時に図形とビジターを再生成する
        /// </summary>
        protected override void OnReset() {
            shapes.Clear();
            areaCalculator = null;
            drawingExporter = null;
        }

        /// <summary>
        /// Visitorパターンのシナリオを構築する
        /// </summary>
        /// <param name="scenario">ステップを追加するシナリオ</param>
        protected override void BuildScenario(DemoScenario scenario) {
            shapes.Clear();

            scenario.AddStep(new DemoStep(
                "図形コレクションを作成する",
                () => {
                    shapes.Add(new VisitorCircle(5));
                    shapes.Add(new VisitorRectangle(4, 6));
                    shapes.Add(new VisitorCircle(3));
                    var nameBuilder = new StringBuilder();
                    for (int i = 0; i < shapes.Count; i++) {
                        if (i > 0) {
                            nameBuilder.Append(", ");
                        }
                        nameBuilder.Append(shapes[i].ShapeName);
                    }
                    Log("Client", "図形生成", $"[{nameBuilder}]");
                }
            ));

            scenario.AddStep(new DemoStep(
                "AreaCalculatorビジターを生成する",
                () => {
                    areaCalculator = new AreaCalculator();
                    Log("Client", "AreaCalculator生成", "面積計算用ビジターを生成");
                }
            ));

            scenario.AddStep(new DemoStep(
                "AreaCalculatorで全図形を訪問して面積を計算する",
                () => {
                    for (int i = 0; i < shapes.Count; i++) {
                        string result = shapes[i].Accept(areaCalculator);
                        Log("AreaCalculator", $"Visit({shapes[i].ShapeName})", result);
                    }
                }
            ));

            scenario.AddStep(new DemoStep(
                "DrawingExporterビジターを生成する",
                () => {
                    drawingExporter = new DrawingExporter();
                    Log("Client", "DrawingExporter生成", "SVGエクスポート用ビジターを生成");
                }
            ));

            scenario.AddStep(new DemoStep(
                "DrawingExporterで全図形を訪問してエクスポートする",
                () => {
                    for (int i = 0; i < shapes.Count; i++) {
                        string result = shapes[i].Accept(drawingExporter);
                        Log("DrawingExporter", $"Visit({shapes[i].ShapeName})", result);
                    }
                }
            ));

            scenario.AddStep(new DemoStep(
                "開放閉鎖原則 — 図形クラスを変更せずに新しい操作を追加できることを確認する",
                () => {
                    Log("Visitor", "設計上の利点",
                        "新しいビジターを追加するだけで新しい操作を実現できる");
                    Log("Visitor", "開放閉鎖原則",
                        "図形クラス(Circle, Rectangle)は変更不要 — ビジターの追加で拡張可能");
                }
            ));
        }
    }
}
