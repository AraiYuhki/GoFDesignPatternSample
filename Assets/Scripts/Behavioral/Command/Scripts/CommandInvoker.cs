using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Behavioral.Command
{
    /// <summary>
    /// コマンドの実行・取り消し・再実行を管理するInvokerクラス
    /// Commandパターンにおける「Invoker」に相当する
    /// コマンド履歴を保持し、Undo/Redo機能を提供する
    /// </summary>
    public sealed class CommandInvoker
    {
        /// <summary>実行済みコマンドの履歴</summary>
        private readonly List<ICommand> history = new List<ICommand>();

        /// <summary>Undoされたコマンドのスタック（Redo用）</summary>
        private readonly List<ICommand> redoStack = new List<ICommand>();

        /// <summary>ログ生成用のStringBuilder</summary>
        private readonly StringBuilder logBuilder = new StringBuilder();

        /// <summary>
        /// コマンドを実行し、履歴に追加する
        /// 新しいコマンドを実行するとRedoスタックはクリアされる
        /// </summary>
        /// <param name="command">実行するコマンド</param>
        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            history.Add(command);
            redoStack.Clear();
        }

        /// <summary>
        /// 最後に実行したコマンドを取り消す
        /// 取り消したコマンドはRedoスタックに移動する
        /// </summary>
        public void Undo()
        {
            if (history.Count == 0)
            {
                InGameLogger.Log("  取り消せるコマンドがありません", LogColor.Red);
                return;
            }

            int lastIndex = history.Count - 1;
            ICommand command = history[lastIndex];
            history.RemoveAt(lastIndex);

            command.Undo();
            redoStack.Add(command);
        }

        /// <summary>
        /// 最後に取り消したコマンドを再実行する
        /// 再実行したコマンドは履歴に戻される
        /// </summary>
        public void Redo()
        {
            if (redoStack.Count == 0)
            {
                InGameLogger.Log("  やり直せるコマンドがありません", LogColor.Red);
                return;
            }

            int lastIndex = redoStack.Count - 1;
            ICommand command = redoStack[lastIndex];
            redoStack.RemoveAt(lastIndex);

            command.Execute();
            history.Add(command);
        }

        /// <summary>
        /// コマンド履歴のログ文字列を生成して返す
        /// </summary>
        /// <returns>履歴の一覧テキスト</returns>
        public string GetHistoryLog()
        {
            if (history.Count == 0)
            {
                return "履歴なし";
            }

            logBuilder.Clear();
            logBuilder.Append("履歴: ");
            for (int i = 0; i < history.Count; i++)
            {
                if (i > 0)
                {
                    logBuilder.Append(" → ");
                }
                logBuilder.Append(history[i].Description);
            }
            return logBuilder.ToString();
        }
    }
}
