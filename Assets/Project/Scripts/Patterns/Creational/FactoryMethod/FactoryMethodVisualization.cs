using UnityEngine;

namespace GoFPatterns.Patterns.Visualization {
    /// <summary>
    /// Factory Methodパターンのビジュアライゼーション
    /// 左側にCreator矩形、右側にProduct円を配置しファクトリメソッドの動作を可視化する
    /// </summary>
    [PatternVisualization("factory-method")]
    public class FactoryMethodVisualization : BasePatternVisualization {
        /// <summary>ForestCreator矩形の位置</summary>
        private static readonly Vector2 ForestCreatorPosition = new Vector2(-4f, 1.5f);
        /// <summary>DungeonCreator矩形の位置</summary>
        private static readonly Vector2 DungeonCreatorPosition = new Vector2(-4f, -2f);
        /// <summary>Creator矩形のサイズ</summary>
        private static readonly Vector2 CreatorSize = new Vector2(2.8f, 1.5f);
        /// <summary>Goblin円の位置</summary>
        private static readonly Vector2 GoblinPosition = new Vector2(3f, 1.5f);
        /// <summary>Orc円の位置</summary>
        private static readonly Vector2 OrcPosition = new Vector2(3f, -2f);
        /// <summary>Product円の半径</summary>
        private const float ProductRadius = 0.9f;
        /// <summary>パルスアニメーションの秒数</summary>
        private const float PulseDuration = 0.5f;

        /// <summary>ForestCreatorの色</summary>
        private static readonly Color ForestColor = new Color(0.2f, 0.6f, 0.3f, 1f);
        /// <summary>DungeonCreatorの色</summary>
        private static readonly Color DungeonColor = new Color(0.5f, 0.3f, 0.2f, 1f);
        /// <summary>Goblinの色</summary>
        private static readonly Color GoblinColor = new Color(0.3f, 0.8f, 0.4f, 1f);
        /// <summary>Orcの色</summary>
        private static readonly Color OrcColor = new Color(0.8f, 0.3f, 0.3f, 1f);

        /// <summary>
        /// バインド時に全要素を非表示で配置する
        /// </summary>
        /// <param name="demo">バインドされたデモ</param>
        protected override void OnBind(IPatternDemo demo) {
            VisualElement forestCreator = AddRect("forestCreator", "Forest\nCreator", ForestCreatorPosition, CreatorSize, ForestColor);
            forestCreator.SetVisible(false);

            VisualElement dungeonCreator = AddRect("dungeonCreator", "Dungeon\nCreator", DungeonCreatorPosition, CreatorSize, DungeonColor);
            dungeonCreator.SetVisible(false);

            VisualElement goblin = AddCircle("goblin", "Goblin", GoblinPosition, ProductRadius, GoblinColor);
            goblin.SetVisible(false);

            VisualElement orc = AddCircle("orc", "Orc", OrcPosition, ProductRadius, OrcColor);
            orc.SetVisible(false);

            VisualArrow arrowForest = AddArrow("arrowForest", forestCreator, goblin, ArrowColor);
            arrowForest.gameObject.SetActive(false);

            VisualArrow arrowDungeon = AddArrow("arrowDungeon", dungeonCreator, orc, ArrowColor);
            arrowDungeon.gameObject.SetActive(false);
        }

        /// <summary>
        /// ステップに応じて要素の表示とアニメーションを更新する
        /// </summary>
        /// <param name="stepIndex">現在のステップインデックス</param>
        protected override void OnRefresh(int stepIndex) {
            VisualElement forestCreator = GetElement("forestCreator");
            VisualElement dungeonCreator = GetElement("dungeonCreator");
            VisualElement goblin = GetElement("goblin");
            VisualElement orc = GetElement("orc");
            VisualArrow arrowForest = GetArrow("arrowForest");
            VisualArrow arrowDungeon = GetArrow("arrowDungeon");

            switch (stepIndex) {
                case 0:
                    forestCreator.SetVisible(true);
                    forestCreator.Pulse(PulseColor, PulseDuration);
                    break;
                case 1:
                    forestCreator.Pulse(HighlightColor, PulseDuration);
                    goblin.SetVisible(true);
                    goblin.Pulse(PulseColor, PulseDuration);
                    arrowForest.gameObject.SetActive(true);
                    arrowForest.Pulse(PulseColor, PulseDuration);
                    break;
                case 2:
                    goblin.Pulse(HighlightColor, PulseDuration);
                    goblin.SetLabel("Goblin\nAttack!");
                    break;
                case 3:
                    forestCreator.SetColorImmediate(DimColor);
                    goblin.SetColorImmediate(DimColor);
                    goblin.SetLabel("Goblin");
                    arrowForest.gameObject.SetActive(false);
                    dungeonCreator.SetVisible(true);
                    dungeonCreator.Pulse(PulseColor, PulseDuration);
                    break;
                case 4:
                    dungeonCreator.Pulse(HighlightColor, PulseDuration);
                    orc.SetVisible(true);
                    orc.Pulse(PulseColor, PulseDuration);
                    arrowDungeon.gameObject.SetActive(true);
                    arrowDungeon.Pulse(PulseColor, PulseDuration);
                    break;
                case 5:
                    orc.Pulse(HighlightColor, PulseDuration);
                    orc.SetLabel("Orc\nAttack!");
                    break;
            }
        }
    }
}
