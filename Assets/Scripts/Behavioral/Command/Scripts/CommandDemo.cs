using UnityEngine;
using UnityEngine.UI;

namespace DesignPatterns.Behavioral.Command
{
    /// <summary>
    /// Commandパターンのデモシーンを制御するクラス
    ///
    /// 【デモの内容】
    /// - 上下左右の移動コマンドを実行
    /// - Undoで直前のコマンドを取り消し
    /// - Redoで取り消したコマンドを再実行
    /// - コマンド履歴の確認
    /// </summary>
    public sealed class CommandDemo : PatternDemoBase
    {
        /// <summary>上に移動するボタン</summary>
        [SerializeField]
        private Button moveUpButton;

        /// <summary>下に移動するボタン</summary>
        [SerializeField]
        private Button moveDownButton;

        /// <summary>左に移動するボタン</summary>
        [SerializeField]
        private Button moveLeftButton;

        /// <summary>右に移動するボタン</summary>
        [SerializeField]
        private Button moveRightButton;

        /// <summary>取り消しボタン</summary>
        [SerializeField]
        private Button undoButton;

        /// <summary>やり直しボタン</summary>
        [SerializeField]
        private Button redoButton;

        /// <summary>コマンドの実行と管理を行うInvoker</summary>
        private CommandInvoker invoker;

        /// <summary>移動対象の名前</summary>
        private const string TargetName = "プレイヤー";

        /// <summary>1回の移動距離</summary>
        private const float MoveDistance = 1.0f;

        /// <inheritdoc/>
        protected override string PatternName
        {
            get { return "Command"; }
        }

        /// <inheritdoc/>
        protected override PatternCategory Category
        {
            get { return PatternCategory.Behavioral; }
        }

        /// <inheritdoc/>
        protected override string Description
        {
            get { return "要求をオブジェクトとしてカプセル化し、取り消しや再実行を可能にする"; }
        }

        /// <inheritdoc/>
        protected override void OnDemoStart()
        {
            invoker = new CommandInvoker();

            if (moveUpButton != null)
            {
                moveUpButton.onClick.AddListener(OnMoveUp);
            }
            if (moveDownButton != null)
            {
                moveDownButton.onClick.AddListener(OnMoveDown);
            }
            if (moveLeftButton != null)
            {
                moveLeftButton.onClick.AddListener(OnMoveLeft);
            }
            if (moveRightButton != null)
            {
                moveRightButton.onClick.AddListener(OnMoveRight);
            }
            if (undoButton != null)
            {
                undoButton.onClick.AddListener(OnUndo);
            }
            if (redoButton != null)
            {
                redoButton.onClick.AddListener(OnRedo);
            }

            InGameLogger.Log("移動ボタンでコマンドを実行し、Undo/Redoを試してください", LogColor.Yellow);
        }

        /// <summary>上方向への移動コマンドを実行する</summary>
        private void OnMoveUp()
        {
            ICommand command = new MoveCommand(TargetName, Vector2.up, MoveDistance);
            InGameLogger.Log("--- 上に移動 ---", LogColor.Yellow);
            invoker.ExecuteCommand(command);
            LogHistory();
        }

        /// <summary>下方向への移動コマンドを実行する</summary>
        private void OnMoveDown()
        {
            ICommand command = new MoveCommand(TargetName, Vector2.down, MoveDistance);
            InGameLogger.Log("--- 下に移動 ---", LogColor.Yellow);
            invoker.ExecuteCommand(command);
            LogHistory();
        }

        /// <summary>左方向への移動コマンドを実行する</summary>
        private void OnMoveLeft()
        {
            ICommand command = new MoveCommand(TargetName, Vector2.left, MoveDistance);
            InGameLogger.Log("--- 左に移動 ---", LogColor.Yellow);
            invoker.ExecuteCommand(command);
            LogHistory();
        }

        /// <summary>右方向への移動コマンドを実行する</summary>
        private void OnMoveRight()
        {
            ICommand command = new MoveCommand(TargetName, Vector2.right, MoveDistance);
            InGameLogger.Log("--- 右に移動 ---", LogColor.Yellow);
            invoker.ExecuteCommand(command);
            LogHistory();
        }

        /// <summary>直前のコマンドを取り消す</summary>
        private void OnUndo()
        {
            InGameLogger.Log("--- Undo ---", LogColor.Yellow);
            invoker.Undo();
            LogHistory();
        }

        /// <summary>取り消したコマンドを再実行する</summary>
        private void OnRedo()
        {
            InGameLogger.Log("--- Redo ---", LogColor.Yellow);
            invoker.Redo();
            LogHistory();
        }

        /// <summary>現在のコマンド履歴をログに表示する</summary>
        private void LogHistory()
        {
            InGameLogger.Log(invoker.GetHistoryLog(), LogColor.White);
        }
    }
}
