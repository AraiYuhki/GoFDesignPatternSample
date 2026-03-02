using UnityEngine;
using UnityEngine.UI;

namespace DesignPatterns.Behavioral.Interpreter {
    /// <summary>
    /// Interpreterパターンのデモシーンを制御するクラス
    ///
    /// 【デモの内容】
    /// - 簡易コマンド言語を解析・実行
    /// - MOVE/STATUS/REPEATコマンドをボタンで実行し、
    ///   文法の解釈と実行の仕組みを確認できる
    /// </summary>
    public sealed class InterpreterDemo : PatternDemoBase {
        /// <summary>MOVE UP 3 コマンドのボタン</summary>
        [SerializeField]
        private Button moveUpButton;

        /// <summary>MOVE LEFT 2 コマンドのボタン</summary>
        [SerializeField]
        private Button moveLeftButton;

        /// <summary>STATUS コマンドのボタン</summary>
        [SerializeField]
        private Button statusButton;

        /// <summary>REPEAT 3 MOVE RIGHT 1 コマンドのボタン</summary>
        [SerializeField]
        private Button repeatButton;

        /// <summary>ゲームコンテキスト</summary>
        private GameContext context;

        /// <summary>MOVE UP コマンド文字列</summary>
        private const string MoveUpCommand = "MOVE UP 3";

        /// <summary>MOVE LEFT コマンド文字列</summary>
        private const string MoveLeftCommand = "MOVE LEFT 2";

        /// <summary>STATUS コマンド文字列</summary>
        private const string StatusCommand = "STATUS";

        /// <summary>REPEAT コマンド文字列</summary>
        private const string RepeatCommand = "REPEAT 3 MOVE RIGHT 1";

        /// <summary>キャラクター名</summary>
        private const string CharacterName = "勇者";

        /// <summary>初期X座標</summary>
        private const int InitialX = 0;

        /// <summary>初期Y座標</summary>
        private const int InitialY = 0;

        /// <inheritdoc/>
        protected override string PatternName {
            get { return "Interpreter"; }
        }

        /// <inheritdoc/>
        protected override PatternCategory Category {
            get { return PatternCategory.Behavioral; }
        }

        /// <inheritdoc/>
        protected override string Description {
            get { return "簡易的な言語の文法を定義し、文を解釈する仕組みを提供する"; }
        }

        /// <inheritdoc/>
        protected override void OnDemoStart() {
            context = new GameContext(CharacterName, InitialX, InitialY);

            if (moveUpButton != null) {
                moveUpButton.onClick.AddListener(OnMoveUp);
            }
            if (moveLeftButton != null) {
                moveLeftButton.onClick.AddListener(OnMoveLeft);
            }
            if (statusButton != null) {
                statusButton.onClick.AddListener(OnStatus);
            }
            if (repeatButton != null) {
                repeatButton.onClick.AddListener(OnRepeat);
            }

            InGameLogger.Log($"{CharacterName} が位置 ({InitialX}, {InitialY}) にいます", LogColor.Orange);
            InGameLogger.Log("コマンドボタンを押して、Interpreterの動作を確認してください", LogColor.Yellow);
        }

        /// <summary>MOVE UP 3 コマンドを実行する</summary>
        private void OnMoveUp() {
            ExecuteCommand(MoveUpCommand);
        }

        /// <summary>MOVE LEFT 2 コマンドを実行する</summary>
        private void OnMoveLeft() {
            ExecuteCommand(MoveLeftCommand);
        }

        /// <summary>STATUS コマンドを実行する</summary>
        private void OnStatus() {
            ExecuteCommand(StatusCommand);
        }

        /// <summary>REPEAT 3 MOVE RIGHT 1 コマンドを実行する</summary>
        private void OnRepeat() {
            ExecuteCommand(RepeatCommand);
        }

        /// <summary>
        /// コマンド文字列を解析・実行してログに表示する
        /// </summary>
        /// <param name="commandText">コマンド文字列</param>
        private void ExecuteCommand(string commandText) {
            InGameLogger.Log($"--- コマンド: {commandText} ---", LogColor.Yellow);

            IExpression expression = CommandParser.Parse(commandText);
            if (expression == null) {
                InGameLogger.Log("コマンドを解析できませんでした", LogColor.Red);
                return;
            }

            string result = expression.Interpret(context);
            InGameLogger.Log(result, LogColor.Orange);
        }
    }
}
