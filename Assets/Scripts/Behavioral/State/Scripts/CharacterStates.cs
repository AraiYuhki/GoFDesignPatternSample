namespace DesignPatterns.Behavioral.State
{
    /// <summary>
    /// 待機状態
    /// キャラクターが何もしていない基本状態を表す
    /// </summary>
    public sealed class IdleState : ICharacterState
    {
        /// <inheritdoc/>
        public string StateName
        {
            get { return "Idle (待機)"; }
        }

        /// <inheritdoc/>
        public void Enter()
        {
            InGameLogger.Log("  → 待機状態に入りました。周囲を警戒中...", LogColor.Orange);
        }

        /// <inheritdoc/>
        public void Update()
        {
            InGameLogger.Log("  [Idle] 静かに立っている...", LogColor.White);
        }

        /// <inheritdoc/>
        public ICharacterState HandleInput(string input)
        {
            switch (input)
            {
                case "move":
                    return new MoveState();
                case "attack":
                    return new AttackState();
                case "damage":
                    return new DamageState();
                default:
                    return this;
            }
        }
    }

    /// <summary>
    /// 移動状態
    /// キャラクターが移動中の状態を表す
    /// </summary>
    public sealed class MoveState : ICharacterState
    {
        /// <inheritdoc/>
        public string StateName
        {
            get { return "Move (移動)"; }
        }

        /// <inheritdoc/>
        public void Enter()
        {
            InGameLogger.Log("  → 移動状態に入りました。走り出す!", LogColor.Orange);
        }

        /// <inheritdoc/>
        public void Update()
        {
            InGameLogger.Log("  [Move] 前方に走っている...", LogColor.White);
        }

        /// <inheritdoc/>
        public ICharacterState HandleInput(string input)
        {
            switch (input)
            {
                case "idle":
                    return new IdleState();
                case "attack":
                    return new AttackState();
                case "damage":
                    return new DamageState();
                default:
                    return this;
            }
        }
    }

    /// <summary>
    /// 攻撃状態
    /// キャラクターが攻撃中の状態を表す
    /// </summary>
    public sealed class AttackState : ICharacterState
    {
        /// <inheritdoc/>
        public string StateName
        {
            get { return "Attack (攻撃)"; }
        }

        /// <inheritdoc/>
        public void Enter()
        {
            InGameLogger.Log("  → 攻撃状態に入りました。剣を振りかぶる!", LogColor.Orange);
        }

        /// <inheritdoc/>
        public void Update()
        {
            InGameLogger.Log("  [Attack] 連続攻撃中!", LogColor.White);
        }

        /// <inheritdoc/>
        public ICharacterState HandleInput(string input)
        {
            switch (input)
            {
                case "idle":
                    return new IdleState();
                case "move":
                    return new MoveState();
                case "damage":
                    return new DamageState();
                default:
                    return this;
            }
        }
    }

    /// <summary>
    /// ダメージ状態
    /// キャラクターが被ダメージ中の状態を表す
    /// </summary>
    public sealed class DamageState : ICharacterState
    {
        /// <inheritdoc/>
        public string StateName
        {
            get { return "Damage (被弾)"; }
        }

        /// <inheritdoc/>
        public void Enter()
        {
            InGameLogger.Log("  → ダメージ状態に入りました。のけぞる!", LogColor.Red);
        }

        /// <inheritdoc/>
        public void Update()
        {
            InGameLogger.Log("  [Damage] ダメージから回復中...", LogColor.White);
        }

        /// <inheritdoc/>
        public ICharacterState HandleInput(string input)
        {
            switch (input)
            {
                case "idle":
                    return new IdleState();
                case "move":
                    return new MoveState();
                case "attack":
                    return new AttackState();
                default:
                    return this;
            }
        }
    }
}
