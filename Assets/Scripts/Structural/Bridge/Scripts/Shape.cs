namespace DesignPatterns.Structural.Bridge
{
    /// <summary>
    /// 形状の抽象クラス（Abstraction）
    ///
    /// 【Bridgeパターンにおける役割】
    /// 抽象側の階層を定義する
    /// 実装（色）への参照を保持し、形状と色を独立して拡張可能にする
    ///
    /// 【Bridgeパターンの意図】
    /// 抽象化と実装を分離し、それぞれ独立に変更できるようにする
    /// 形状（円・四角）と色（赤・青・緑）の組み合わせを、継承の爆発なしに実現する
    /// </summary>
    public abstract class Shape
    {
        /// <summary>この形状に適用する色の実装</summary>
        protected readonly IColorImplementor colorImplementor;

        /// <summary>形状の名前</summary>
        public abstract string ShapeName { get; }

        /// <summary>
        /// 形状を生成する
        /// </summary>
        /// <param name="colorImplementor">適用する色の実装</param>
        protected Shape(IColorImplementor colorImplementor)
        {
            this.colorImplementor = colorImplementor;
        }

        /// <summary>
        /// 形状を描画する（ログで表現）
        /// </summary>
        public abstract void Draw();
    }

    /// <summary>
    /// 円形状（RefinedAbstraction）
    /// </summary>
    public sealed class Circle : Shape
    {
        /// <summary>円の半径</summary>
        private readonly float radius;

        /// <inheritdoc/>
        public override string ShapeName { get { return "円"; } }

        /// <summary>
        /// 円を生成する
        /// </summary>
        /// <param name="colorImplementor">適用する色</param>
        /// <param name="radius">半径</param>
        public Circle(IColorImplementor colorImplementor, float radius) : base(colorImplementor)
        {
            this.radius = radius;
        }

        /// <inheritdoc/>
        public override void Draw()
        {
            InGameLogger.Log(
                $"● {colorImplementor.ColorName}い{ShapeName}を描画（半径: {radius}）",
                LogColor.Green
            );
        }
    }

    /// <summary>
    /// 四角形状（RefinedAbstraction）
    /// </summary>
    public sealed class Rectangle : Shape
    {
        /// <summary>幅</summary>
        private readonly float width;

        /// <summary>高さ</summary>
        private readonly float height;

        /// <inheritdoc/>
        public override string ShapeName { get { return "四角"; } }

        /// <summary>
        /// 四角形を生成する
        /// </summary>
        /// <param name="colorImplementor">適用する色</param>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        public Rectangle(IColorImplementor colorImplementor, float width, float height) : base(colorImplementor)
        {
            this.width = width;
            this.height = height;
        }

        /// <inheritdoc/>
        public override void Draw()
        {
            InGameLogger.Log(
                $"■ {colorImplementor.ColorName}い{ShapeName}を描画（{width} x {height}）",
                LogColor.Green
            );
        }
    }
}
