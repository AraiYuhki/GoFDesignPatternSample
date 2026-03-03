namespace DesignPatterns.Behavioral.TemplateMethod
{
    /// <summary>
    /// 戦士のターン処理
    /// Template Methodパターンにおける ConcreteClass に相当し、
    /// 剣による物理攻撃を行動フェーズとして実装する
    /// </summary>
    public sealed class WarriorTurn : BattleTurnTemplate
    {
        /// <summary>基本攻撃力</summary>
        private const int BaseAttack = 30;

        /// <summary>攻撃力の振れ幅</summary>
        private const int AttackVariance = 10;

        /// <summary>
        /// WarriorTurnを生成する
        /// </summary>
        public WarriorTurn() : base("戦士")
        {
        }

        /// <summary>
        /// 剣で斬りかかる行動を実行する
        /// ダメージは基本攻撃力にランダムな振れ幅を加えて算出する
        /// </summary>
        protected override void ActionPhase()
        {
            int damage = BaseAttack + UnityEngine.Random.Range(0, AttackVariance + 1);
            InGameLogger.Log($"[{CharacterName}] 剣で斬りかかった！ ダメージ: {damage}", LogColor.Orange);
        }
    }

    /// <summary>
    /// 魔法使いのターン処理
    /// Template Methodパターンにおける ConcreteClass に相当し、
    /// ファイアボールによる魔法攻撃を行動フェーズとして実装する
    /// </summary>
    public sealed class MageTurn : BattleTurnTemplate
    {
        /// <summary>魔法の基本ダメージ</summary>
        private const int BaseMagicDamage = 50;

        /// <summary>ダメージの振れ幅</summary>
        private const int DamageVariance = 20;

        /// <summary>MP消費量</summary>
        private const int ManaCost = 15;

        /// <summary>
        /// MageTurnを生成する
        /// </summary>
        public MageTurn() : base("魔法使い")
        {
        }

        /// <summary>
        /// ファイアボールを唱える行動を実行する
        /// MP消費とダメージを算出してログに表示する
        /// </summary>
        protected override void ActionPhase()
        {
            int damage = BaseMagicDamage + UnityEngine.Random.Range(0, DamageVariance + 1);
            InGameLogger.Log($"[{CharacterName}] ファイアボールを唱えた！ MP消費: {ManaCost} ダメージ: {damage}", LogColor.Orange);
        }
    }

    /// <summary>
    /// 回復役のターン処理
    /// Template Methodパターンにおける ConcreteClass に相当し、
    /// 回復魔法による味方のHP回復を行動フェーズとして実装する
    /// </summary>
    public sealed class HealerTurn : BattleTurnTemplate
    {
        /// <summary>基本回復量</summary>
        private const int BaseHealAmount = 25;

        /// <summary>回復量の振れ幅</summary>
        private const int HealVariance = 15;

        /// <summary>
        /// HealerTurnを生成する
        /// </summary>
        public HealerTurn() : base("回復役")
        {
        }

        /// <summary>
        /// 回復魔法を唱える行動を実行する
        /// 回復量を算出してログに表示する
        /// </summary>
        protected override void ActionPhase()
        {
            int healAmount = BaseHealAmount + UnityEngine.Random.Range(0, HealVariance + 1);
            InGameLogger.Log($"[{CharacterName}] 回復魔法を唱えた！ 回復量: {healAmount}", LogColor.Orange);
        }
    }
}
