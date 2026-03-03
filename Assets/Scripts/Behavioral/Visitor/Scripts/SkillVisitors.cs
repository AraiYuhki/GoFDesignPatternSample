namespace DesignPatterns.Behavioral.Visitor
{
    /// <summary>
    /// スキルのアップグレードコストを計算するVisitor
    /// Visitorパターンにおける ConcreteVisitor に相当し、
    /// 各スキルノードのレベルに基づいてコストを合算する
    /// </summary>
    public sealed class CostCalculator : ISkillTreeVisitor
    {
        /// <summary>攻撃スキルの1レベルあたりの基本コスト</summary>
        private const int AttackCostPerLevel = 100;

        /// <summary>防御スキルの1レベルあたりの基本コスト</summary>
        private const int DefenseCostPerLevel = 80;

        /// <summary>魔法スキルの1レベルあたりの基本コスト</summary>
        private const int MagicCostPerLevel = 120;

        /// <summary>合計コスト</summary>
        private int totalCost;

        /// <summary>合計コストを取得する</summary>
        public int TotalCost
        {
            get { return totalCost; }
        }

        /// <summary>
        /// 計算結果をリセットする
        /// </summary>
        public void Reset()
        {
            totalCost = 0;
        }

        /// <inheritdoc/>
        public void Visit(AttackSkillNode node)
        {
            int cost = node.Level * AttackCostPerLevel;
            totalCost += cost;
            InGameLogger.Log($"  攻撃スキル [{node.SkillName}] Lv.{node.Level} → コスト: {cost}G", LogColor.Orange);
        }

        /// <inheritdoc/>
        public void Visit(DefenseSkillNode node)
        {
            int cost = node.Level * DefenseCostPerLevel;
            totalCost += cost;
            InGameLogger.Log($"  防御スキル [{node.SkillName}] Lv.{node.Level} → コスト: {cost}G", LogColor.Orange);
        }

        /// <inheritdoc/>
        public void Visit(MagicSkillNode node)
        {
            int cost = node.Level * MagicCostPerLevel;
            totalCost += cost;
            InGameLogger.Log($"  魔法スキル [{node.SkillName}] Lv.{node.Level} → コスト: {cost}G", LogColor.Orange);
        }
    }

    /// <summary>
    /// スキルの効果を計算するVisitor
    /// Visitorパターンにおける ConcreteVisitor に相当し、
    /// 各スキルノードの効果値を合算する
    /// </summary>
    public sealed class EffectCalculator : ISkillTreeVisitor
    {
        /// <summary>合計攻撃力ボーナス</summary>
        private int totalAttack;

        /// <summary>合計防御力ボーナス</summary>
        private int totalDefense;

        /// <summary>合計魔力</summary>
        private int totalMagicPower;

        /// <summary>合計攻撃力ボーナスを取得する</summary>
        public int TotalAttack
        {
            get { return totalAttack; }
        }

        /// <summary>合計防御力ボーナスを取得する</summary>
        public int TotalDefense
        {
            get { return totalDefense; }
        }

        /// <summary>合計魔力を取得する</summary>
        public int TotalMagicPower
        {
            get { return totalMagicPower; }
        }

        /// <summary>
        /// 計算結果をリセットする
        /// </summary>
        public void Reset()
        {
            totalAttack = 0;
            totalDefense = 0;
            totalMagicPower = 0;
        }

        /// <inheritdoc/>
        public void Visit(AttackSkillNode node)
        {
            int effect = node.BonusAttack * node.Level;
            totalAttack += effect;
            InGameLogger.Log($"  攻撃スキル [{node.SkillName}] Lv.{node.Level} → 攻撃力 +{effect}", LogColor.Orange);
        }

        /// <inheritdoc/>
        public void Visit(DefenseSkillNode node)
        {
            int effect = node.BonusDefense * node.Level;
            totalDefense += effect;
            InGameLogger.Log($"  防御スキル [{node.SkillName}] Lv.{node.Level} → 防御力 +{effect}", LogColor.Orange);
        }

        /// <inheritdoc/>
        public void Visit(MagicSkillNode node)
        {
            int effect = node.MagicPower * node.Level;
            totalMagicPower += effect;
            InGameLogger.Log($"  魔法スキル [{node.SkillName}] Lv.{node.Level} → 魔力 +{effect} (MP消費: {node.ManaCost})", LogColor.Orange);
        }
    }
}
