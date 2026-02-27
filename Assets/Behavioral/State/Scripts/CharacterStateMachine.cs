namespace DesignPatterns.Behavioral.State {
    /// <summary>
    /// キャラクターの状態を管理するステートマシン
    /// Stateパターンにおける「Context」に相当する
    /// 現在の状態を保持し、入力処理を現在の状態に委譲する
    /// </summary>
    public sealed class CharacterStateMachine {
        /// <summary>現在の状態</summary>
        private ICharacterState currentState;

        /// <summary>
        /// 現在の状態名を返す
        /// </summary>
        public string CurrentStateName {
            get { return currentState.StateName; }
        }

        /// <summary>
        /// ステートマシンを初期状態で生成する
        /// </summary>
        /// <param name="initialState">初期状態</param>
        public CharacterStateMachine(ICharacterState initialState) {
            currentState = initialState;
            currentState.Enter();
        }

        /// <summary>
        /// 入力を処理し、必要に応じて状態遷移を行う
        /// 状態遷移のロジックは現在の状態に委譲する
        /// </summary>
        /// <param name="input">入力コマンド</param>
        public void ProcessInput(string input) {
            InGameLogger.Log($"入力: \"{input}\" (現在: {currentState.StateName})", LogColor.Yellow);

            ICharacterState nextState = currentState.HandleInput(input);

            if (nextState != currentState) {
                InGameLogger.Log($"状態遷移: {currentState.StateName} → {nextState.StateName}", LogColor.Orange);
                currentState = nextState;
                currentState.Enter();
            } else {
                InGameLogger.Log($"状態維持: {currentState.StateName}", LogColor.White);
            }
        }

        /// <summary>
        /// 現在の状態のUpdate処理を実行する
        /// </summary>
        public void UpdateState() {
            currentState.Update();
        }
    }
}
