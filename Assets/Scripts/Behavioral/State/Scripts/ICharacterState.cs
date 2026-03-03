namespace DesignPatterns.Behavioral.State
{
    /// <summary>
    /// キャラクターの状態を定義するインターフェース
    /// Stateパターンにおける「State」に相当する
    /// 各状態で異なる振る舞いを提供し、状態遷移のロジックを各状態に委譲する
    /// </summary>
    public interface ICharacterState
    {
        /// <summary>状態の名称</summary>
        string StateName { get; }

        /// <summary>
        /// 入力に応じて次の状態を返す
        /// 状態遷移のロジックは各状態が判断する
        /// </summary>
        /// <param name="input">入力コマンド ("move", "attack", "damage", "idle")</param>
        /// <returns>遷移先の状態インスタンス。遷移しない場合は自身を返す</returns>
        ICharacterState HandleInput(string input);

        /// <summary>
        /// この状態に入ったときに呼ばれる
        /// 状態に応じた初期化処理を行う
        /// </summary>
        void Enter();

        /// <summary>
        /// この状態での毎フレーム更新処理
        /// 状態に応じた継続的な振る舞いを実行する
        /// </summary>
        void Update();
    }
}
