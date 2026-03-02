namespace DesignPatterns.Behavioral.Command {
    /// <summary>
    /// コマンドを定義するインターフェース
    /// Commandパターンにおける「Command」に相当する
    /// 要求をオブジェクトとしてカプセル化し、実行と取り消しの操作を提供する
    /// </summary>
    public interface ICommand {
        /// <summary>コマンドの説明テキスト</summary>
        string Description { get; }

        /// <summary>
        /// コマンドを実行する
        /// </summary>
        void Execute();

        /// <summary>
        /// コマンドの実行を取り消す
        /// </summary>
        void Undo();
    }
}
