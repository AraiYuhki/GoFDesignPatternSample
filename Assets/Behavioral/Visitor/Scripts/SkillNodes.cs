namespace DesignPatterns.Behavioral.Visitor {
    /// <summary>
    /// 攻撃スキルノード
    /// Visitorパターンにおける ConcreteElement に相当し、
    /// 攻撃力ボーナスを持つスキルを表す
    /// </summary>
    public sealed class AttackSkillNode : ISkillNode {
        /// <summary>スキル名</summary>
        private readonly string skillName;

        /// <summary>スキルレベル</summary>
        private readonly int level;

        /// <summary>攻撃力ボーナス</summary>
        private readonly int bonusAttack;

        /// <summary>
        /// AttackSkillNodeを生成する
        /// </summary>
        /// <param name="skillName">スキル名</param>
        /// <param name="level">スキルレベル</param>
        /// <param name="bonusAttack">攻撃力ボーナス</param>
        public AttackSkillNode(string skillName, int level, int bonusAttack) {
            this.skillName = skillName;
            this.level = level;
            this.bonusAttack = bonusAttack;
        }

        /// <inheritdoc/>
        public string SkillName {
            get { return skillName; }
        }

        /// <inheritdoc/>
        public int Level {
            get { return level; }
        }

        /// <summary>攻撃力ボーナスを取得する</summary>
        public int BonusAttack {
            get { return bonusAttack; }
        }

        /// <inheritdoc/>
        public void Accept(ISkillTreeVisitor visitor) {
            visitor.Visit(this);
        }
    }

    /// <summary>
    /// 防御スキルノード
    /// Visitorパターンにおける ConcreteElement に相当し、
    /// 防御力ボーナスを持つスキルを表す
    /// </summary>
    public sealed class DefenseSkillNode : ISkillNode {
        /// <summary>スキル名</summary>
        private readonly string skillName;

        /// <summary>スキルレベル</summary>
        private readonly int level;

        /// <summary>防御力ボーナス</summary>
        private readonly int bonusDefense;

        /// <summary>
        /// DefenseSkillNodeを生成する
        /// </summary>
        /// <param name="skillName">スキル名</param>
        /// <param name="level">スキルレベル</param>
        /// <param name="bonusDefense">防御力ボーナス</param>
        public DefenseSkillNode(string skillName, int level, int bonusDefense) {
            this.skillName = skillName;
            this.level = level;
            this.bonusDefense = bonusDefense;
        }

        /// <inheritdoc/>
        public string SkillName {
            get { return skillName; }
        }

        /// <inheritdoc/>
        public int Level {
            get { return level; }
        }

        /// <summary>防御力ボーナスを取得する</summary>
        public int BonusDefense {
            get { return bonusDefense; }
        }

        /// <inheritdoc/>
        public void Accept(ISkillTreeVisitor visitor) {
            visitor.Visit(this);
        }
    }

    /// <summary>
    /// 魔法スキルノード
    /// Visitorパターンにおける ConcreteElement に相当し、
    /// マナコストと魔力を持つスキルを表す
    /// </summary>
    public sealed class MagicSkillNode : ISkillNode {
        /// <summary>スキル名</summary>
        private readonly string skillName;

        /// <summary>スキルレベル</summary>
        private readonly int level;

        /// <summary>マナコスト</summary>
        private readonly int manaCost;

        /// <summary>魔力</summary>
        private readonly int magicPower;

        /// <summary>
        /// MagicSkillNodeを生成する
        /// </summary>
        /// <param name="skillName">スキル名</param>
        /// <param name="level">スキルレベル</param>
        /// <param name="manaCost">マナコスト</param>
        /// <param name="magicPower">魔力</param>
        public MagicSkillNode(string skillName, int level, int manaCost, int magicPower) {
            this.skillName = skillName;
            this.level = level;
            this.manaCost = manaCost;
            this.magicPower = magicPower;
        }

        /// <inheritdoc/>
        public string SkillName {
            get { return skillName; }
        }

        /// <inheritdoc/>
        public int Level {
            get { return level; }
        }

        /// <summary>マナコストを取得する</summary>
        public int ManaCost {
            get { return manaCost; }
        }

        /// <summary>魔力を取得する</summary>
        public int MagicPower {
            get { return magicPower; }
        }

        /// <inheritdoc/>
        public void Accept(ISkillTreeVisitor visitor) {
            visitor.Visit(this);
        }
    }
}
