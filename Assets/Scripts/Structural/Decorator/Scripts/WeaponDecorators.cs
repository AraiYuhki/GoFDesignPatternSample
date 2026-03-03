namespace DesignPatterns.Structural.Decorator
{
    /// <summary>
    /// 武器デコレーターの基底クラス（Decorator）
    /// ラップする武器への参照を保持し、機能を委譲・拡張する
    /// </summary>
    public abstract class WeaponDecorator : IWeapon
    {
        /// <summary>ラップされた武器</summary>
        protected readonly IWeapon wrappedWeapon;

        /// <inheritdoc/>
        public abstract string Name { get; }

        /// <inheritdoc/>
        public abstract int AttackPower { get; }

        /// <summary>
        /// デコレーターを生成する
        /// </summary>
        /// <param name="weapon">ラップする武器</param>
        protected WeaponDecorator(IWeapon weapon)
        {
            wrappedWeapon = weapon;
        }

        /// <inheritdoc/>
        public abstract string GetDescription();
    }

    /// <summary>
    /// 炎エンチャント（ConcreteDecorator）
    /// 武器に炎属性のダメージを追加する
    /// </summary>
    public sealed class FireEnchantment : WeaponDecorator
    {
        /// <summary>炎エンチャントの追加攻撃力</summary>
        private const int BonusAttack = 10;

        /// <inheritdoc/>
        public override string Name { get { return $"炎の{wrappedWeapon.Name}"; } }

        /// <inheritdoc/>
        public override int AttackPower { get { return wrappedWeapon.AttackPower + BonusAttack; } }

        /// <summary>
        /// 炎エンチャントを生成する
        /// </summary>
        /// <param name="weapon">エンチャント対象の武器</param>
        public FireEnchantment(IWeapon weapon) : base(weapon)
        {
        }

        /// <inheritdoc/>
        public override string GetDescription()
        {
            return $"{Name} (攻撃力: {AttackPower}) [🔥+{BonusAttack}]";
        }
    }

    /// <summary>
    /// 氷エンチャント（ConcreteDecorator）
    /// 武器に氷属性の効果を追加する
    /// </summary>
    public sealed class IceEnchantment : WeaponDecorator
    {
        /// <summary>氷エンチャントの追加攻撃力</summary>
        private const int BonusAttack = 8;

        /// <inheritdoc/>
        public override string Name { get { return $"氷の{wrappedWeapon.Name}"; } }

        /// <inheritdoc/>
        public override int AttackPower { get { return wrappedWeapon.AttackPower + BonusAttack; } }

        /// <summary>
        /// 氷エンチャントを生成する
        /// </summary>
        /// <param name="weapon">エンチャント対象の武器</param>
        public IceEnchantment(IWeapon weapon) : base(weapon)
        {
        }

        /// <inheritdoc/>
        public override string GetDescription()
        {
            return $"{Name} (攻撃力: {AttackPower}) [❄️+{BonusAttack}]";
        }
    }

    /// <summary>
    /// 毒エンチャント（ConcreteDecorator）
    /// 武器に毒属性の効果を追加する
    /// </summary>
    public sealed class PoisonEnchantment : WeaponDecorator
    {
        /// <summary>毒エンチャントの追加攻撃力</summary>
        private const int BonusAttack = 5;

        /// <inheritdoc/>
        public override string Name { get { return $"毒の{wrappedWeapon.Name}"; } }

        /// <inheritdoc/>
        public override int AttackPower { get { return wrappedWeapon.AttackPower + BonusAttack; } }

        /// <summary>
        /// 毒エンチャントを生成する
        /// </summary>
        /// <param name="weapon">エンチャント対象の武器</param>
        public PoisonEnchantment(IWeapon weapon) : base(weapon)
        {
        }

        /// <inheritdoc/>
        public override string GetDescription()
        {
            return $"{Name} (攻撃力: {AttackPower}) [☠️+{BonusAttack}]";
        }
    }
}
