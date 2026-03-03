namespace DesignPatterns.Behavioral.Visitor
{
    /// <summary>
    /// スキルツリーの各ノードを訪問するVisitorインターフェース
    /// Visitorパターンにおける Visitor に相当し、
    /// ノードの型ごとに異なる処理を定義する
    /// </summary>
    public interface ISkillTreeVisitor
    {
        /// <summary>
        /// 攻撃スキルノードを訪問する
        /// </summary>
        /// <param name="node">訪問対象の攻撃スキルノード</param>
        void Visit(AttackSkillNode node);

        /// <summary>
        /// 防御スキルノードを訪問する
        /// </summary>
        /// <param name="node">訪問対象の防御スキルノード</param>
        void Visit(DefenseSkillNode node);

        /// <summary>
        /// 魔法スキルノードを訪問する
        /// </summary>
        /// <param name="node">訪問対象の魔法スキルノード</param>
        void Visit(MagicSkillNode node);
    }
}
