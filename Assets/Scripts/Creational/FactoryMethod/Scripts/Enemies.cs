namespace DesignPatterns.Creational.FactoryMethod
{
    /// <summary>
    /// スライム（ConcreteProduct）
    /// 弱いが数が多い敵
    /// </summary>
    public sealed class Slime : IEnemy
    {
        /// <inheritdoc/>
        public string Name { get { return "スライム"; } }

        /// <inheritdoc/>
        public int Hp { get { return 30; } }

        /// <inheritdoc/>
        public int Attack { get { return 5; } }

        /// <inheritdoc/>
        public string PerformAttack()
        {
            return $"{Name}の体当たり！（{Attack}ダメージ）";
        }
    }

    /// <summary>
    /// ゴブリン（ConcreteProduct）
    /// バランスの取れた中堅の敵
    /// </summary>
    public sealed class Goblin : IEnemy
    {
        /// <inheritdoc/>
        public string Name { get { return "ゴブリン"; } }

        /// <inheritdoc/>
        public int Hp { get { return 80; } }

        /// <inheritdoc/>
        public int Attack { get { return 15; } }

        /// <inheritdoc/>
        public string PerformAttack()
        {
            return $"{Name}の棍棒攻撃！（{Attack}ダメージ）";
        }
    }

    /// <summary>
    /// ドラゴン（ConcreteProduct）
    /// 強力なボス級の敵
    /// </summary>
    public sealed class Dragon : IEnemy
    {
        /// <inheritdoc/>
        public string Name { get { return "ドラゴン"; } }

        /// <inheritdoc/>
        public int Hp { get { return 500; } }

        /// <inheritdoc/>
        public int Attack { get { return 50; } }

        /// <inheritdoc/>
        public string PerformAttack()
        {
            return $"{Name}のブレス攻撃！（{Attack}ダメージ）";
        }
    }
}
