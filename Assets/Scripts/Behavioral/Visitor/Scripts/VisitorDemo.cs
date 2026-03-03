using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DesignPatterns.Behavioral.Visitor
{
    /// <summary>
    /// Visitorパターンのデモシーンを制御するクラス
    ///
    /// 【デモの内容】
    /// - スキルツリーのノード群に対してVisitorを適用
    /// - コスト計算Visitorと効果計算Visitorを切り替えて、
    ///   データ構造を変更せずに異なる処理を実行する様子を確認できる
    /// </summary>
    public sealed class VisitorDemo : PatternDemoBase
    {
        /// <summary>コスト計算ボタン</summary>
        [SerializeField]
        private Button calculateCostButton;

        /// <summary>効果計算ボタン</summary>
        [SerializeField]
        private Button calculateEffectsButton;

        /// <summary>スキルノードのリスト</summary>
        private readonly List<ISkillNode> skillNodes = new List<ISkillNode>();

        /// <summary>コスト計算Visitor</summary>
        private CostCalculator costCalculator;

        /// <summary>効果計算Visitor</summary>
        private EffectCalculator effectCalculator;

        /// <inheritdoc/>
        protected override string PatternName
        {
            get { return "Visitor"; }
        }

        /// <inheritdoc/>
        protected override PatternCategory Category
        {
            get { return PatternCategory.Behavioral; }
        }

        /// <inheritdoc/>
        protected override string Description
        {
            get { return "データ構造と処理を分離し、新しい処理を構造を変更せずに追加する"; }
        }

        /// <inheritdoc/>
        protected override void OnDemoStart()
        {
            costCalculator = new CostCalculator();
            effectCalculator = new EffectCalculator();

            InitializeSkillNodes();

            if (calculateCostButton != null)
            {
                calculateCostButton.onClick.AddListener(OnCalculateCost);
            }
            if (calculateEffectsButton != null)
            {
                calculateEffectsButton.onClick.AddListener(OnCalculateEffects);
            }

            InGameLogger.Log("スキルツリーを作成しました", LogColor.Orange);
            for (int i = 0; i < skillNodes.Count; i++)
            {
                ISkillNode node = skillNodes[i];
                InGameLogger.Log($"  {node.SkillName} (Lv.{node.Level})", LogColor.White);
            }
            InGameLogger.Log("ボタンを押して、異なるVisitorによる処理を確認してください", LogColor.Yellow);
        }

        /// <summary>
        /// スキルノードを初期化する
        /// </summary>
        private void InitializeSkillNodes()
        {
            skillNodes.Add(new AttackSkillNode("パワースラッシュ", 3, 15));
            skillNodes.Add(new AttackSkillNode("連続斬り", 2, 10));
            skillNodes.Add(new DefenseSkillNode("アイアンガード", 4, 12));
            skillNodes.Add(new DefenseSkillNode("リフレクト", 1, 8));
            skillNodes.Add(new MagicSkillNode("ファイアボール", 3, 20, 25));
            skillNodes.Add(new MagicSkillNode("ヒール", 2, 10, 18));
        }

        /// <summary>コスト計算Visitorで全ノードを訪問する</summary>
        private void OnCalculateCost()
        {
            InGameLogger.Log("--- コスト計算 ---", LogColor.Yellow);
            costCalculator.Reset();

            for (int i = 0; i < skillNodes.Count; i++)
            {
                skillNodes[i].Accept(costCalculator);
            }

            InGameLogger.Log($"合計コスト: {costCalculator.TotalCost}G", LogColor.Orange);
        }

        /// <summary>効果計算Visitorで全ノードを訪問する</summary>
        private void OnCalculateEffects()
        {
            InGameLogger.Log("--- 効果計算 ---", LogColor.Yellow);
            effectCalculator.Reset();

            for (int i = 0; i < skillNodes.Count; i++)
            {
                skillNodes[i].Accept(effectCalculator);
            }

            InGameLogger.Log($"合計効果 → 攻撃力: +{effectCalculator.TotalAttack} 防御力: +{effectCalculator.TotalDefense} 魔力: +{effectCalculator.TotalMagicPower}", LogColor.Orange);
        }
    }
}
