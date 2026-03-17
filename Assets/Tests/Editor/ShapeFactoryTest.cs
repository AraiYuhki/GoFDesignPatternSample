using DesignPatterns.Visualization;
using NUnit.Framework;
using UnityEngine;

namespace DesignPatterns.Tests
{
    /// <summary>
    /// ShapeFactoryのユニットテスト
    /// スプライト生成とキャッシュの動作を検証する
    /// </summary>
    [TestFixture]
    public class ShapeFactoryTest
    {
        [Test]
        public void GetCircleSprite_スプライトが生成される()
        {
            var sprite = ShapeFactory.GetCircleSprite();

            Assert.IsNotNull(sprite);
            Assert.IsNotNull(sprite.texture);
            Assert.AreEqual(128, sprite.texture.width);
            Assert.AreEqual(128, sprite.texture.height);
        }

        [Test]
        public void GetCircleSprite_キャッシュされて同じインスタンスが返る()
        {
            var sprite1 = ShapeFactory.GetCircleSprite();
            var sprite2 = ShapeFactory.GetCircleSprite();

            Assert.AreSame(sprite1, sprite2);
        }

        [Test]
        public void GetRectSprite_スプライトが生成される()
        {
            var sprite = ShapeFactory.GetRectSprite();

            Assert.IsNotNull(sprite);
            Assert.IsNotNull(sprite.texture);
        }

        [Test]
        public void GetRectSprite_キャッシュされて同じインスタンスが返る()
        {
            var sprite1 = ShapeFactory.GetRectSprite();
            var sprite2 = ShapeFactory.GetRectSprite();

            Assert.AreSame(sprite1, sprite2);
        }

        [Test]
        public void GetTriangleSprite_スプライトが生成される()
        {
            var sprite = ShapeFactory.GetTriangleSprite();

            Assert.IsNotNull(sprite);
            Assert.IsNotNull(sprite.texture);
        }

        [Test]
        public void GetTriangleSprite_キャッシュされて同じインスタンスが返る()
        {
            var sprite1 = ShapeFactory.GetTriangleSprite();
            var sprite2 = ShapeFactory.GetTriangleSprite();

            Assert.AreSame(sprite1, sprite2);
        }
    }
}
