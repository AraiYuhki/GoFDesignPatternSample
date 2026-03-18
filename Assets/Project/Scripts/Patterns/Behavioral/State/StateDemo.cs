namespace GoFPatterns.Patterns {
    // ---- State interface ----

    /// <summary>
    /// Stateパターンのステートインターフェース
    /// キャラクターの各状態が実装する共通契約
    /// </summary>
    public interface ICharacterState {
        /// <summary>状態の名前を取得する</summary>
        string StateName { get; }
        /// <summary>
        /// 状態に入ったときに呼ばれる
        /// </summary>
        /// <param name="character">状態を持つキャラクター</param>
        void Enter(StateCharacter character);
        /// <summary>
        /// 入力を処理して次の状態へ遷移させる
        /// </summary>
        /// <param name="character">状態を持つキャラクター</param>
        /// <param name="input">入力コマンド</param>
        /// <returns>処理の説明文</returns>
        string HandleInput(StateCharacter character, string input);
    }

    // ---- ConcreteStates ----

    /// <summary>待機状態 — moveで移動、attackで攻撃に遷移する</summary>
    public class IdleState : ICharacterState {
        /// <summary>状態の名前</summary>
        public string StateName => "Idle";
        /// <summary>
        /// Idle状態に入ったときの処理
        /// </summary>
        /// <param name="character">対象キャラクター</param>
        public void Enter(StateCharacter character) { }
        /// <summary>
        /// Idle状態での入力処理
        /// </summary>
        /// <param name="character">対象キャラクター</param>
        /// <param name="input">入力コマンド</param>
        /// <returns>処理の説明文</returns>
        public string HandleInput(StateCharacter character, string input) {
            switch (input) {
                case "move":
                    character.ChangeState(new WalkingState());
                    return "Idle → Walking に遷移";
                case "attack":
                    character.ChangeState(new AttackingState());
                    return "Idle → Attacking に遷移";
                default:
                    return $"Idle: '{input}' を無視";
            }
        }
    }

    /// <summary>移動状態 — stopでIdle、attackで攻撃に遷移する</summary>
    public class WalkingState : ICharacterState {
        /// <summary>状態の名前</summary>
        public string StateName => "Walking";
        /// <summary>
        /// Walking状態に入ったときの処理
        /// </summary>
        /// <param name="character">対象キャラクター</param>
        public void Enter(StateCharacter character) { }
        /// <summary>
        /// Walking状態での入力処理
        /// </summary>
        /// <param name="character">対象キャラクター</param>
        /// <param name="input">入力コマンド</param>
        /// <returns>処理の説明文</returns>
        public string HandleInput(StateCharacter character, string input) {
            switch (input) {
                case "stop":
                    character.ChangeState(new IdleState());
                    return "Walking → Idle に遷移";
                case "attack":
                    character.ChangeState(new AttackingState());
                    return "Walking → Attacking に遷移";
                default:
                    return $"Walking: 移動しながら '{input}'";
            }
        }
    }

    /// <summary>攻撃状態 — attackEndでIdle、hitで被ダメージ判定に遷移する</summary>
    public class AttackingState : ICharacterState {
        /// <summary>状態の名前</summary>
        public string StateName => "Attacking";
        /// <summary>
        /// Attacking状態に入ったときの処理
        /// </summary>
        /// <param name="character">対象キャラクター</param>
        public void Enter(StateCharacter character) { }
        /// <summary>
        /// Attacking状態での入力処理
        /// </summary>
        /// <param name="character">対象キャラクター</param>
        /// <param name="input">入力コマンド</param>
        /// <returns>処理の説明文</returns>
        public string HandleInput(StateCharacter character, string input) {
            switch (input) {
                case "attackEnd":
                    character.ChangeState(new IdleState());
                    return "Attacking → Idle に遷移 (攻撃終了)";
                case "die":
                    character.ChangeState(new DeadState());
                    return "Attacking → Dead に遷移 (致命傷)";
                default:
                    return $"Attacking: 攻撃中に '{input}' を受信 (無視)";
            }
        }
    }

    /// <summary>死亡状態 — すべての入力を拒否する終端状態</summary>
    public class DeadState : ICharacterState {
        /// <summary>状態の名前</summary>
        public string StateName => "Dead";
        /// <summary>
        /// Dead状態に入ったときの処理
        /// </summary>
        /// <param name="character">対象キャラクター</param>
        public void Enter(StateCharacter character) { }
        /// <summary>
        /// Dead状態での入力処理（すべて拒否）
        /// </summary>
        /// <param name="character">対象キャラクター</param>
        /// <param name="input">入力コマンド</param>
        /// <returns>拒否の説明文</returns>
        public string HandleInput(StateCharacter character, string input) =>
            $"Dead: '{input}' を受信したが死亡中のため無効";
    }

    // ---- Context ----

    /// <summary>
    /// Stateパターンのコンテキスト
    /// 現在の状態オブジェクトに処理を委譲するキャラクター
    /// </summary>
    public class StateCharacter {
        /// <summary>キャラクターの名前</summary>
        private readonly string name;
        /// <summary>現在の状態</summary>
        private ICharacterState currentState;

        /// <summary>キャラクターの名前を取得する</summary>
        public string Name => name;
        /// <summary>現在の状態名を取得する</summary>
        public string CurrentStateName => currentState?.StateName ?? "なし";

        /// <summary>
        /// StateCharacterを生成する
        /// </summary>
        /// <param name="name">キャラクター名</param>
        public StateCharacter(string name) {
            this.name = name;
            currentState = new IdleState();
        }

        /// <summary>
        /// 状態を切り替える
        /// </summary>
        /// <param name="newState">次の状態</param>
        public void ChangeState(ICharacterState newState) {
            currentState = newState;
            currentState.Enter(this);
        }

        /// <summary>
        /// 入力を現在の状態に委譲して処理する
        /// </summary>
        /// <param name="input">入力コマンド</param>
        /// <returns>処理結果の説明文</returns>
        public string HandleInput(string input) {
            return currentState.HandleInput(this, input);
        }
    }

    // ---- Demo ----

    /// <summary>
    /// Stateパターンのデモ
    /// StateCharacterを例に状態ごとに振る舞いが変わる仕組みを示す
    /// </summary>
    [PatternDemo("state")]
    public class StateDemo : BasePatternDemo {
        /// <summary>デモのパターンID</summary>
        public override string PatternId => "state";

        /// <summary>デモの表示名</summary>
        public override string DisplayName => "State";

        /// <summary>状態機械として動くキャラクター</summary>
        private StateCharacter character;

        /// <summary>
        /// リセット時にキャラクターを再生成する
        /// </summary>
        protected override void OnReset() {
            character = null;
        }

        /// <summary>
        /// Stateパターンのシナリオを構築する
        /// </summary>
        /// <param name="scenario">ステップを追加するシナリオ</param>
        protected override void BuildScenario(DemoScenario scenario) {
            character = new StateCharacter("勇者");

            scenario.AddStep(new DemoStep(
                "キャラクターはIdle状態で開始する",
                () => {
                    Log(character.Name, "初期状態", $"現在: {character.CurrentStateName}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "moveコマンドを送る — IdleからWalkingへ遷移する",
                () => {
                    string result = character.HandleInput("move");
                    Log(character.Name, "HandleInput(move)", $"{result} → 現在: {character.CurrentStateName}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "attackコマンドを送る — WalkingからAttackingへ遷移する",
                () => {
                    string result = character.HandleInput("attack");
                    Log(character.Name, "HandleInput(attack)", $"{result} → 現在: {character.CurrentStateName}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "attackEndコマンドを送る — AttackingからIdleへ戻る",
                () => {
                    string result = character.HandleInput("attackEnd");
                    Log(character.Name, "HandleInput(attackEnd)", $"{result} → 現在: {character.CurrentStateName}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "attackしてからdieコマンドを送る — Deadへ遷移する",
                () => {
                    character.HandleInput("attack");
                    string result = character.HandleInput("die");
                    Log(character.Name, "HandleInput(die)", $"{result} → 現在: {character.CurrentStateName}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "Dead状態でmoveを送る — すべての入力が拒否される",
                () => {
                    string result = character.HandleInput("move");
                    Log(character.Name, "HandleInput(move)", result);
                }
            ));
        }
    }
}
