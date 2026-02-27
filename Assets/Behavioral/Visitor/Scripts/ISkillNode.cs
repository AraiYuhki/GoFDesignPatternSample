namespace DesignPatterns.Behavioral.Visitor {
    /// <summary>
    /// スキルツリーのノードを表すインターフェース
    /// Visitorパターンにおける Element に相当し、
    /// Visitorの受け入れメソッドを定義する
    /// </summary>
    public interface ISkillNode {
        /// <summary>スキル名を取得する</summary>
        string SkillName { get; }

        /// <summary>スキルレベルを取得する</summary>
        int Level { get; }

        /// <summary>
        /// Visitorを受け入れて処理を委譲する
        /// </summary>
        /// <param name="visitor">訪問するVisitor</param>
        void Accept(ISkillTreeVisitor visitor);
    }
}
