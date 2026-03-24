using UnityEngine;

namespace GoFPatterns.Patterns.Visualization {
    /// <summary>
    /// Builderパターンのビジュアライゼーション
    /// Director・Builder・Productの関係をステップごとに可視化する
    /// </summary>
    [PatternVisualization("builder")]
    public class BuilderVisualization : BasePatternVisualization {
        /// <summary>Director矩形の位置</summary>
        private static readonly Vector2 DirectorPosition = new Vector2(0f, 3.5f);
        /// <summary>Director矩形のサイズ</summary>
        private static readonly Vector2 DirectorSize = new Vector2(2.8f, 1.3f);
        /// <summary>WarriorBuilder矩形の位置</summary>
        private static readonly Vector2 WarriorBuilderPosition = new Vector2(-3.5f, 0.5f);
        /// <summary>MageBuilder矩形の位置</summary>
        private static readonly Vector2 MageBuilderPosition = new Vector2(3.5f, 0.5f);
        /// <summary>Builder矩形のサイズ</summary>
        private static readonly Vector2 BuilderSize = new Vector2(2.8f, 1.3f);
        /// <summary>WarriorProduct矩形の位置</summary>
        private static readonly Vector2 WarriorProductPosition = new Vector2(-3.5f, -2.5f);
        /// <summary>MageProduct矩形の位置</summary>
        private static readonly Vector2 MageProductPosition = new Vector2(3.5f, -2.5f);
        /// <summary>Product矩形のサイズ</summary>
        private static readonly Vector2 ProductSize = new Vector2(2.5f, 1.2f);
        /// <summary>パルスアニメーションの秒数</summary>
        private const float PulseDuration = 0.5f;

        /// <summary>Directorの色</summary>
        private static readonly Color DirectorColor = new Color(0.7f, 0.5f, 0.2f, 1f);
        /// <summary>WarriorBuilderの色</summary>
        private static readonly Color WarriorColor = new Color(0.8f, 0.3f, 0.3f, 1f);
        /// <summary>MageBuilderの色</summary>
        private static readonly Color MageColor = new Color(0.3f, 0.3f, 0.8f, 1f);
        /// <summary>WarriorProductの色</summary>
        private static readonly Color WarriorProductColor = new Color(0.9f, 0.4f, 0.4f, 1f);
        /// <summary>MageProductの色</summary>
        private static readonly Color MageProductColor = new Color(0.4f, 0.4f, 0.9f, 1f);

        /// <summary>
        /// バインド時に全要素を非表示で配置する
        /// </summary>
        /// <param name="demo">バインドされたデモ</param>
        protected override void OnBind(IPatternDemo demo) {
            VisualElement director = AddRect("director", "Director", DirectorPosition, DirectorSize, DirectorColor);
            director.SetVisible(false);

            VisualElement warriorBuilder = AddRect("warriorBuilder", "Warrior\nBuilder", WarriorBuilderPosition, BuilderSize, WarriorColor);
            warriorBuilder.SetVisible(false);

            VisualElement mageBuilder = AddRect("mageBuilder", "Mage\nBuilder", MageBuilderPosition, BuilderSize, MageColor);
            mageBuilder.SetVisible(false);

            VisualElement warriorProduct = AddRect("warriorProduct", "Warrior", WarriorProductPosition, ProductSize, WarriorProductColor);
            warriorProduct.SetVisible(false);

            VisualElement mageProduct = AddRect("mageProduct", "Mage", MageProductPosition, ProductSize, MageProductColor);
            mageProduct.SetVisible(false);

            VisualArrow arrowDirWarrior = AddArrow("arrowDirWarrior", director, warriorBuilder, ArrowColor);
            arrowDirWarrior.gameObject.SetActive(false);

            VisualArrow arrowDirMage = AddArrow("arrowDirMage", director, mageBuilder, ArrowColor);
            arrowDirMage.gameObject.SetActive(false);

            VisualArrow arrowWarriorProduct = AddArrow("arrowWarriorProduct", warriorBuilder, warriorProduct, ArrowColor);
            arrowWarriorProduct.gameObject.SetActive(false);

            VisualArrow arrowMageProduct = AddArrow("arrowMageProduct", mageBuilder, mageProduct, ArrowColor);
            arrowMageProduct.gameObject.SetActive(false);
        }

        /// <summary>
        /// ステップに応じて要素の表示とアニメーションを更新する
        /// </summary>
        /// <param name="stepIndex">現在のステップインデックス</param>
        protected override void OnRefresh(int stepIndex) {
            VisualElement director = GetElement("director");
            VisualElement warriorBuilder = GetElement("warriorBuilder");
            VisualElement mageBuilder = GetElement("mageBuilder");
            VisualElement warriorProduct = GetElement("warriorProduct");
            VisualElement mageProduct = GetElement("mageProduct");
            VisualArrow arrowDirWarrior = GetArrow("arrowDirWarrior");
            VisualArrow arrowDirMage = GetArrow("arrowDirMage");
            VisualArrow arrowWarriorProduct = GetArrow("arrowWarriorProduct");
            VisualArrow arrowMageProduct = GetArrow("arrowMageProduct");

            switch (stepIndex) {
                case 0:
                    director.SetVisible(true);
                    director.Pulse(PulseColor, PulseDuration);
                    break;
                case 1:
                    warriorBuilder.SetVisible(true);
                    warriorBuilder.Pulse(PulseColor, PulseDuration);
                    arrowDirWarrior.gameObject.SetActive(true);
                    arrowDirWarrior.Pulse(PulseColor, PulseDuration);
                    break;
                case 2:
                    director.Pulse(HighlightColor, PulseDuration);
                    arrowDirWarrior.Pulse(HighlightColor, PulseDuration);
                    warriorBuilder.Pulse(HighlightColor, PulseDuration);
                    warriorBuilder.SetLabel("Warrior\nBuilder\n(building)");
                    break;
                case 3:
                    warriorBuilder.SetLabel("Warrior\nBuilder");
                    warriorProduct.SetVisible(true);
                    warriorProduct.SetLabel("Warrior\nHP=150 ATK=30");
                    warriorProduct.Pulse(PulseColor, PulseDuration);
                    arrowWarriorProduct.gameObject.SetActive(true);
                    arrowWarriorProduct.Pulse(PulseColor, PulseDuration);
                    break;
                case 4:
                    warriorBuilder.SetColorImmediate(DimColor);
                    arrowDirWarrior.gameObject.SetActive(false);
                    mageBuilder.SetVisible(true);
                    mageBuilder.Pulse(PulseColor, PulseDuration);
                    arrowDirMage.gameObject.SetActive(true);
                    arrowDirMage.Pulse(PulseColor, PulseDuration);
                    break;
                case 5:
                    director.Pulse(HighlightColor, PulseDuration);
                    arrowDirMage.Pulse(HighlightColor, PulseDuration);
                    mageBuilder.Pulse(HighlightColor, PulseDuration);
                    mageBuilder.SetLabel("Mage\nBuilder\n(building)");
                    break;
                case 6:
                    mageBuilder.SetLabel("Mage\nBuilder");
                    mageProduct.SetVisible(true);
                    mageProduct.SetLabel("Mage\nHP=80 ATK=60");
                    mageProduct.Pulse(PulseColor, PulseDuration);
                    arrowMageProduct.gameObject.SetActive(true);
                    arrowMageProduct.Pulse(PulseColor, PulseDuration);
                    break;
            }
        }
    }
}
