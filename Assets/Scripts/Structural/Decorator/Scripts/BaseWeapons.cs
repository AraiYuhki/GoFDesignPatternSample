namespace DesignPatterns.Structural.Decorator {
    /// <summary>
    /// 基本の剣（ConcreteComponent）
    /// </summary>
    public sealed class Sword : IWeapon {
        /// <inheritdoc/>
        public string Name { get { return "鉄の剣"; } }

        /// <inheritdoc/>
        public int AttackPower { get { return 20; } }

        /// <inheritdoc/>
        public string GetDescription() {
            return $"{Name} (攻撃力: {AttackPower})";
        }
    }

    /// <summary>
    /// 基本の弓（ConcreteComponent）
    /// </summary>
    public sealed class Bow : IWeapon {
        /// <inheritdoc/>
        public string Name { get { return "木の弓"; } }

        /// <inheritdoc/>
        public int AttackPower { get { return 15; } }

        /// <inheritdoc/>
        public string GetDescription() {
            return $"{Name} (攻撃力: {AttackPower})";
        }
    }
}
