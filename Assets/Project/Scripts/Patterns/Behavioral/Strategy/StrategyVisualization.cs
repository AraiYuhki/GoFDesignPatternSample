using UnityEngine;

namespace GoFPatterns.Patterns.Visualization {
    /// <summary>
    /// Strategyパターンのビジュアライゼーション
    /// キャラクターを中心に3つの戦略を扇形に配置し、戦略切り替えを矢印で可視化する
    /// </summary>
    [PatternVisualization("strategy")]
    public class StrategyVisualization : BasePatternVisualization {
        /// <summary>キャラクターの配置位置</summary>
        private static readonly Vector2 CharacterPosition = new Vector2(-3f, 0f);
        /// <summary>Aggressive戦略の配置位置</summary>
        private static readonly Vector2 AggressivePosition = new Vector2(3f, 3f);
        /// <summary>Defensive戦略の配置位置</summary>
        private static readonly Vector2 DefensivePosition = new Vector2(3f, -3f);
        /// <summary>Balanced戦略の配置位置</summary>
        private static readonly Vector2 BalancedPosition = new Vector2(4.5f, 0f);
        /// <summary>キャラクターの半径</summary>
        private const float CharacterRadius = 1.2f;
        /// <summary>戦略矩形のサイズ</summary>
        private static readonly Vector2 StrategySize = new Vector2(2.5f, 1.2f);
        /// <summary>キャラクターの色</summary>
        private static readonly Color CharacterColor = new Color(0.3f, 0.5f, 0.8f, 1f);
        /// <summary>Aggressive戦略の色</summary>
        private static readonly Color AggressiveColor = new Color(0.8f, 0.3f, 0.3f, 1f);
        /// <summary>Defensive戦略の色</summary>
        private static readonly Color DefensiveColor = new Color(0.3f, 0.4f, 0.8f, 1f);
        /// <summary>Balanced戦略の色</summary>
        private static readonly Color BalancedColor = new Color(0.3f, 0.7f, 0.4f, 1f);
        /// <summary>現在アクティブな戦略矢印のID</summary>
        private string activeArrowId;

        /// <summary>
        /// バインド時にキャラクターと3つの戦略要素を配置して初期表示を構築する
        /// </summary>
        /// <param name="demo">バインドされたデモ</param>
        protected override void OnBind(IPatternDemo demo) {
            AddCircle("character", "勇者", CharacterPosition, CharacterRadius, CharacterColor);
            AddRect("aggressive", "Aggressive", AggressivePosition, StrategySize, AggressiveColor);
            AddRect("defensive", "Defensive", DefensivePosition, StrategySize, DefensiveColor);
            AddRect("balanced", "Balanced", BalancedPosition, StrategySize, BalancedColor);

            activeArrowId = null;
        }

        /// <summary>
        /// ステップに応じて戦略切り替えのアニメーションを更新する
        /// </summary>
        /// <param name="stepIndex">現在のステップインデックス</param>
        protected override void OnRefresh(int stepIndex) {
            VisualElement character = GetElement("character");

            switch (stepIndex) {
                case 0:
                    ActivateStrategy("aggressive", AggressiveColor);
                    break;
                case 1:
                    GetElement("aggressive")?.Pulse(PulseColor, 0.5f);
                    character.Pulse(PulseColor, 0.5f);
                    GetArrow(activeArrowId)?.Pulse(PulseColor, 0.5f);
                    break;
                case 2:
                    ActivateStrategy("defensive", DefensiveColor);
                    break;
                case 3:
                    GetElement("defensive")?.Pulse(PulseColor, 0.5f);
                    character.Pulse(PulseColor, 0.5f);
                    GetArrow(activeArrowId)?.Pulse(PulseColor, 0.5f);
                    break;
                case 4:
                    ActivateStrategy("balanced", BalancedColor);
                    break;
                case 5:
                    GetElement("balanced")?.Pulse(PulseColor, 0.5f);
                    character.Pulse(PulseColor, 0.5f);
                    GetArrow(activeArrowId)?.Pulse(PulseColor, 0.5f);
                    break;
            }
        }

        /// <summary>
        /// 指定の戦略をアクティブにして矢印を切り替える
        /// </summary>
        /// <param name="strategyId">アクティブにする戦略の識別子</param>
        /// <param name="color">戦略固有の色</param>
        private void ActivateStrategy(string strategyId, Color color) {
            DimAllStrategies();

            VisualElement character = GetElement("character");
            VisualElement strategy = GetElement(strategyId);
            strategy.SetColorImmediate(color);
            strategy.Pulse(HighlightColor, 0.5f);

            if (activeArrowId != null) {
                GetArrow(activeArrowId)?.SetColor(DimColor);
            }

            string arrowId = $"char-{strategyId}";
            AddArrow(arrowId, character, strategy, HighlightColor);
            GetArrow(arrowId)?.SetColor(HighlightColor);
            activeArrowId = arrowId;
        }

        /// <summary>
        /// 全戦略要素をDim状態にする
        /// </summary>
        private void DimAllStrategies() {
            GetElement("aggressive")?.SetColorImmediate(DimColor);
            GetElement("defensive")?.SetColorImmediate(DimColor);
            GetElement("balanced")?.SetColorImmediate(DimColor);
        }
    }
}
