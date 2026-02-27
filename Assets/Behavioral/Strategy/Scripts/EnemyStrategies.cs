namespace DesignPatterns.Behavioral.Strategy {
    /// <summary>
    /// 攻撃的な戦略
    /// 敵に接近して全力で攻撃する行動パターンを実装する
    /// </summary>
    public sealed class AggressiveStrategy : IEnemyStrategy {
        /// <inheritdoc/>
        public string StrategyName {
            get { return "Aggressive (攻撃型)"; }
        }

        /// <inheritdoc/>
        public string Execute() {
            return "敵に突進して全力攻撃! ダメージ+50%, 防御-30%";
        }
    }

    /// <summary>
    /// 防御的な戦略
    /// 距離を保ちながら防御を固める行動パターンを実装する
    /// </summary>
    public sealed class DefensiveStrategy : IEnemyStrategy {
        /// <inheritdoc/>
        public string StrategyName {
            get { return "Defensive (防御型)"; }
        }

        /// <inheritdoc/>
        public string Execute() {
            return "シールドを構えて防御態勢! 防御+50%, 攻撃-20%";
        }
    }

    /// <summary>
    /// 逃走戦略
    /// 敵から離れて安全を確保する行動パターンを実装する
    /// </summary>
    public sealed class FleeStrategy : IEnemyStrategy {
        /// <inheritdoc/>
        public string StrategyName {
            get { return "Flee (逃走型)"; }
        }

        /// <inheritdoc/>
        public string Execute() {
            return "全速力で撤退! 移動速度+100%, 攻撃不可";
        }
    }
}
