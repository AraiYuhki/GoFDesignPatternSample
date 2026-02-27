using UnityEngine;
using UnityEngine.UI;

namespace DesignPatterns.Structural.Bridge {
    /// <summary>
    /// Bridgeパターンのデモシーンを制御するクラス
    ///
    /// 【デモの内容】
    /// - 形状（円/四角）と色（赤/青/緑）を独立して切り替え
    /// - 形状の数×色の数の組み合わせをサブクラスなしで実現
    /// - 抽象と実装の分離を体感
    /// </summary>
    public sealed class BridgeDemo : PatternDemoBase {
        /// <summary>赤い円の描画ボタン</summary>
        [SerializeField]
        private Button redCircleButton;

        /// <summary>青い四角の描画ボタン</summary>
        [SerializeField]
        private Button blueRectButton;

        /// <summary>緑い円の描画ボタン</summary>
        [SerializeField]
        private Button greenCircleButton;

        /// <summary>全組み合わせ表示ボタン</summary>
        [SerializeField]
        private Button showAllButton;

        /// <inheritdoc/>
        protected override string PatternName {
            get { return "Bridge"; }
        }

        /// <inheritdoc/>
        protected override PatternCategory Category {
            get { return PatternCategory.Structural; }
        }

        /// <inheritdoc/>
        protected override string Description {
            get { return "抽象化と実装を分離し、それぞれ独立に変更できるようにする"; }
        }

        /// <inheritdoc/>
        protected override void OnDemoStart() {
            if (redCircleButton != null) {
                redCircleButton.onClick.AddListener(OnRedCircle);
            }
            if (blueRectButton != null) {
                blueRectButton.onClick.AddListener(OnBlueRect);
            }
            if (greenCircleButton != null) {
                greenCircleButton.onClick.AddListener(OnGreenCircle);
            }
            if (showAllButton != null) {
                showAllButton.onClick.AddListener(OnShowAll);
            }

            InGameLogger.Log("形状 × 色の組み合わせを確認してください", LogColor.Yellow);
        }

        /// <summary>赤い円を描画する</summary>
        private void OnRedCircle() {
            InGameLogger.Log("--- 赤い円 ---", LogColor.Yellow);
            var shape = new Circle(new RedColor(), 5f);
            shape.Draw();
        }

        /// <summary>青い四角を描画する</summary>
        private void OnBlueRect() {
            InGameLogger.Log("--- 青い四角 ---", LogColor.Yellow);
            var shape = new Rectangle(new BlueColor(), 10f, 6f);
            shape.Draw();
        }

        /// <summary>緑の円を描画する</summary>
        private void OnGreenCircle() {
            InGameLogger.Log("--- 緑の円 ---", LogColor.Yellow);
            var shape = new Circle(new GreenColor(), 3f);
            shape.Draw();
        }

        /// <summary>全組み合わせを表示する</summary>
        private void OnShowAll() {
            InGameLogger.Log("=== 全組み合わせ ===", LogColor.Yellow);

            IColorImplementor[] colors = { new RedColor(), new BlueColor(), new GreenColor() };
            for (int i = 0; i < colors.Length; i++) {
                new Circle(colors[i], 4f).Draw();
                new Rectangle(colors[i], 8f, 5f).Draw();
            }

            InGameLogger.Log($"→ 2形状 × 3色 = 6通りをサブクラスなしで実現", LogColor.Green);
        }
    }
}
