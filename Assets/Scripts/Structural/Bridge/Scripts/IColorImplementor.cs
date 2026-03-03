using UnityEngine;

namespace DesignPatterns.Structural.Bridge
{
    /// <summary>
    /// 色の描画実装インターフェース（Implementor）
    ///
    /// 【Bridgeパターンにおける役割】
    /// 実装側の階層のインターフェースを定義する
    /// 具体的な色の実装を提供する
    /// </summary>
    public interface IColorImplementor
    {
        /// <summary>色の名前</summary>
        string ColorName { get; }

        /// <summary>
        /// 描画に使用する色を返す
        /// </summary>
        /// <returns>Unityの色</returns>
        Color GetColor();
    }

    /// <summary>赤色の実装（ConcreteImplementor）</summary>
    public sealed class RedColor : IColorImplementor
    {
        /// <inheritdoc/>
        public string ColorName { get { return "赤"; } }

        /// <inheritdoc/>
        public Color GetColor() { return Color.red; }
    }

    /// <summary>青色の実装（ConcreteImplementor）</summary>
    public sealed class BlueColor : IColorImplementor
    {
        /// <inheritdoc/>
        public string ColorName { get { return "青"; } }

        /// <inheritdoc/>
        public Color GetColor() { return Color.blue; }
    }

    /// <summary>緑色の実装（ConcreteImplementor）</summary>
    public sealed class GreenColor : IColorImplementor
    {
        /// <inheritdoc/>
        public string ColorName { get { return "緑"; } }

        /// <inheritdoc/>
        public Color GetColor() { return Color.green; }
    }
}
