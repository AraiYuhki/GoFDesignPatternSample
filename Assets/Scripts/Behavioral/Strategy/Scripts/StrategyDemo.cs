using UnityEngine;
using UnityEngine.UI;

namespace DesignPatterns.Behavioral.Strategy
{
    /// <summary>
    /// Strategyパターンのデモシーンを制御するクラス
    ///
    /// 【デモの内容】
    /// - 3つの戦略（攻撃型・防御型・逃走型）を実行時に切り替え
    /// - 戦略を選択してから実行ボタンで行動を確認
    /// - アルゴリズムのカプセル化と動的な切り替えを体験する
    /// </summary>
    public sealed class StrategyDemo : PatternDemoBase
    {
        /// <summary>攻撃型戦略に切り替えるボタン</summary>
        [SerializeField]
        private Button aggressiveButton;

        /// <summary>防御型戦略に切り替えるボタン</summary>
        [SerializeField]
        private Button defensiveButton;

        /// <summary>逃走型戦略に切り替えるボタン</summary>
        [SerializeField]
        private Button fleeButton;

        /// <summary>現在の戦略を実行するボタン</summary>
        [SerializeField]
        private Button executeButton;

        /// <summary>現在選択中の戦略</summary>
        private IEnemyStrategy currentStrategy;

        /// <inheritdoc/>
        protected override string PatternName
        {
            get { return "Strategy"; }
        }

        /// <inheritdoc/>
        protected override PatternCategory Category
        {
            get { return PatternCategory.Behavioral; }
        }

        /// <inheritdoc/>
        protected override string Description
        {
            get { return "アルゴリズムをカプセル化し、実行時に切り替え可能にする"; }
        }

        /// <inheritdoc/>
        protected override void OnDemoStart()
        {
            currentStrategy = new AggressiveStrategy();
            InGameLogger.Log($"初期戦略: {currentStrategy.StrategyName}", LogColor.Yellow);

            if (aggressiveButton != null)
            {
                aggressiveButton.onClick.AddListener(OnSelectAggressive);
            }
            if (defensiveButton != null)
            {
                defensiveButton.onClick.AddListener(OnSelectDefensive);
            }
            if (fleeButton != null)
            {
                fleeButton.onClick.AddListener(OnSelectFlee);
            }
            if (executeButton != null)
            {
                executeButton.onClick.AddListener(OnExecuteStrategy);
            }

            InGameLogger.Log("戦略を切り替えて実行ボタンを押してください", LogColor.Yellow);
        }

        /// <summary>攻撃型戦略に切り替える</summary>
        private void OnSelectAggressive()
        {
            currentStrategy = new AggressiveStrategy();
            InGameLogger.Log($"戦略を変更: {currentStrategy.StrategyName}", CategoryColor);
        }

        /// <summary>防御型戦略に切り替える</summary>
        private void OnSelectDefensive()
        {
            currentStrategy = new DefensiveStrategy();
            InGameLogger.Log($"戦略を変更: {currentStrategy.StrategyName}", CategoryColor);
        }

        /// <summary>逃走型戦略に切り替える</summary>
        private void OnSelectFlee()
        {
            currentStrategy = new FleeStrategy();
            InGameLogger.Log($"戦略を変更: {currentStrategy.StrategyName}", CategoryColor);
        }

        /// <summary>現在の戦略を実行する</summary>
        private void OnExecuteStrategy()
        {
            if (currentStrategy == null)
            {
                InGameLogger.Log("戦略が設定されていません", LogColor.Red);
                return;
            }

            string result = currentStrategy.Execute();
            InGameLogger.Log($"[{currentStrategy.StrategyName}] {result}", CategoryColor);
        }
    }
}
