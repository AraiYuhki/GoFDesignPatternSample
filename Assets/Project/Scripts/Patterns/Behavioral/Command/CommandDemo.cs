using System.Collections.Generic;

namespace GoFPatterns.Patterns {
    // ---- Command interface ----

    /// <summary>
    /// Commandパターンのコマンドインターフェース
    /// 実行と取り消しの対称操作を定義する
    /// </summary>
    public interface IGameCommand {
        /// <summary>コマンドの説明を取得する</summary>
        string Description { get; }
        /// <summary>コマンドを実行する</summary>
        void Execute();
        /// <summary>コマンドを取り消す</summary>
        void Undo();
    }

    // ---- Receiver ----

    /// <summary>
    /// Commandパターンのレシーバー
    /// 実際の操作を担当するプレイヤーキャラクター
    /// </summary>
    public class CommandPlayer {
        /// <summary>初期X座標</summary>
        private const int InitialX = 0;
        /// <summary>初期Y座標</summary>
        private const int InitialY = 0;
        /// <summary>プレイヤーのX座標</summary>
        private int posX;
        /// <summary>プレイヤーのY座標</summary>
        private int posY;
        /// <summary>現在のHP</summary>
        private int hp;

        /// <summary>現在の位置文字列を取得する</summary>
        public string Position => $"({posX}, {posY})";
        /// <summary>現在のHPを取得する</summary>
        public int Hp => hp;

        /// <summary>
        /// CommandPlayerを生成する
        /// </summary>
        /// <param name="initialHp">初期HP</param>
        public CommandPlayer(int initialHp) {
            posX = InitialX;
            posY = InitialY;
            hp = initialHp;
        }

        /// <summary>
        /// 指定量だけ移動する
        /// </summary>
        /// <param name="dx">X方向の移動量</param>
        /// <param name="dy">Y方向の移動量</param>
        public void Move(int dx, int dy) {
            posX += dx;
            posY += dy;
        }

        /// <summary>
        /// HPを回復する
        /// </summary>
        /// <param name="amount">回復量</param>
        public void Heal(int amount) {
            hp += amount;
        }
    }

    // ---- ConcreteCommands ----

    /// <summary>プレイヤーを移動するコマンド</summary>
    public class MoveCommand : IGameCommand {
        /// <summary>移動対象のプレイヤー</summary>
        private readonly CommandPlayer player;
        /// <summary>X方向の移動量</summary>
        private readonly int dx;
        /// <summary>Y方向の移動量</summary>
        private readonly int dy;
        /// <summary>方向の名前</summary>
        private readonly string directionName;

        /// <summary>コマンドの説明</summary>
        public string Description => $"Move {directionName}";

        /// <summary>
        /// MoveCommandを生成する
        /// </summary>
        /// <param name="player">移動対象のプレイヤー</param>
        /// <param name="dx">X方向の移動量</param>
        /// <param name="dy">Y方向の移動量</param>
        /// <param name="directionName">方向の名前</param>
        public MoveCommand(CommandPlayer player, int dx, int dy, string directionName) {
            this.player = player;
            this.dx = dx;
            this.dy = dy;
            this.directionName = directionName;
        }

        /// <summary>
        /// 移動を実行する
        /// </summary>
        public void Execute() {
            player.Move(dx, dy);
        }

        /// <summary>
        /// 移動を取り消す（逆方向に移動）
        /// </summary>
        public void Undo() {
            player.Move(-dx, -dy);
        }
    }

    /// <summary>プレイヤーのHPを回復するコマンド</summary>
    public class HealCommand : IGameCommand {
        /// <summary>回復対象のプレイヤー</summary>
        private readonly CommandPlayer player;
        /// <summary>回復量</summary>
        private readonly int amount;

        /// <summary>コマンドの説明</summary>
        public string Description => $"Heal {amount}";

        /// <summary>
        /// HealCommandを生成する
        /// </summary>
        /// <param name="player">回復対象のプレイヤー</param>
        /// <param name="amount">回復量</param>
        public HealCommand(CommandPlayer player, int amount) {
            this.player = player;
            this.amount = amount;
        }

        /// <summary>
        /// 回復を実行する
        /// </summary>
        public void Execute() {
            player.Heal(amount);
        }

        /// <summary>
        /// 回復を取り消す（同量のHP減少）
        /// </summary>
        public void Undo() {
            player.Heal(-amount);
        }
    }

    // ---- Invoker ----

    /// <summary>
    /// Commandパターンのインボーカー
    /// コマンドの実行履歴を管理してUndo/Redoを提供する
    /// </summary>
    public class ActionInvoker {
        /// <summary>実行済みコマンドのスタック（Undo用）</summary>
        private readonly Stack<IGameCommand> undoStack = new Stack<IGameCommand>();
        /// <summary>取り消し済みコマンドのスタック（Redo用）</summary>
        private readonly Stack<IGameCommand> redoStack = new Stack<IGameCommand>();

        /// <summary>Undoが可能かどうかを取得する</summary>
        public bool CanUndo => undoStack.Count > 0;
        /// <summary>Redoが可能かどうかを取得する</summary>
        public bool CanRedo => redoStack.Count > 0;

        /// <summary>
        /// コマンドを実行して履歴に追加する
        /// </summary>
        /// <param name="command">実行するコマンド</param>
        public void Execute(IGameCommand command) {
            command.Execute();
            undoStack.Push(command);
            redoStack.Clear();
        }

        /// <summary>
        /// 最後に実行したコマンドを取り消す
        /// </summary>
        /// <returns>取り消したコマンドの説明（Undoできなければnull）</returns>
        public string Undo() {
            if (!CanUndo) {
                return null;
            }
            var command = undoStack.Pop();
            command.Undo();
            redoStack.Push(command);
            return command.Description;
        }

        /// <summary>
        /// 取り消したコマンドを再実行する
        /// </summary>
        /// <returns>再実行したコマンドの説明（Redoできなければnull）</returns>
        public string Redo() {
            if (!CanRedo) {
                return null;
            }
            var command = redoStack.Pop();
            command.Execute();
            undoStack.Push(command);
            return command.Description;
        }
    }

    // ---- Demo ----

    /// <summary>
    /// Commandパターンのデモ
    /// ActionInvokerを例にコマンドの実行・Undo・Redoの仕組みを示す
    /// </summary>
    [PatternDemo("command")]
    public class CommandDemo : BasePatternDemo {
        /// <summary>デモのパターンID</summary>
        public override string PatternId => "command";

        /// <summary>デモの表示名</summary>
        public override string DisplayName => "Command";

        /// <summary>初期HP</summary>
        private const int InitialHp = 80;
        /// <summary>コマンドを実行するプレイヤー</summary>
        private CommandPlayer player;
        /// <summary>Undo/Redoを管理するインボーカー</summary>
        private ActionInvoker invoker;

        /// <summary>
        /// リセット時にプレイヤーとインボーカーを再生成する
        /// </summary>
        protected override void OnReset() {
            player = null;
            invoker = null;
        }

        /// <summary>
        /// Commandパターンのシナリオを構築する
        /// </summary>
        /// <param name="scenario">ステップを追加するシナリオ</param>
        protected override void BuildScenario(DemoScenario scenario) {
            player = new CommandPlayer(InitialHp);
            invoker = new ActionInvoker();

            scenario.AddStep(new DemoStep(
                "北へ移動するコマンドを実行する",
                () => {
                    var cmd = new MoveCommand(player, 0, 1, "North");
                    invoker.Execute(cmd);
                    Log("Invoker", $"Execute({cmd.Description})", $"位置: {player.Position}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "東へ移動するコマンドを実行する",
                () => {
                    var cmd = new MoveCommand(player, 1, 0, "East");
                    invoker.Execute(cmd);
                    Log("Invoker", $"Execute({cmd.Description})", $"位置: {player.Position}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "HPを回復するコマンドを実行する",
                () => {
                    var cmd = new HealCommand(player, 20);
                    invoker.Execute(cmd);
                    Log("Invoker", $"Execute({cmd.Description})", $"HP: {player.Hp}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "Undo — 回復コマンドを取り消す",
                () => {
                    string undone = invoker.Undo();
                    Log("Invoker", "Undo()", $"'{undone}' を取り消し → HP: {player.Hp}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "Undo — 東移動コマンドを取り消す",
                () => {
                    string undone = invoker.Undo();
                    Log("Invoker", "Undo()", $"'{undone}' を取り消し → 位置: {player.Position}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "Redo — 東移動コマンドを再実行する",
                () => {
                    string redone = invoker.Redo();
                    Log("Invoker", "Redo()", $"'{redone}' を再実行 → 位置: {player.Position}");
                }
            ));
        }
    }
}
