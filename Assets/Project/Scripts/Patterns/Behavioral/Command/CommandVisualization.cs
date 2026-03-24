using UnityEngine;

namespace GoFPatterns.Patterns.Visualization {
    /// <summary>
    /// Commandパターンのビジュアライゼーション
    /// プレイヤー、コマンドスタック、インボーカーを配置し、実行・Undo・Redoを可視化する
    /// </summary>
    [PatternVisualization("command")]
    public class CommandVisualization : BasePatternVisualization {
        /// <summary>プレイヤーの配置位置</summary>
        private static readonly Vector2 PlayerPosition = new Vector2(-4.5f, 0f);
        /// <summary>インボーカーの配置位置</summary>
        private static readonly Vector2 InvokerPosition = new Vector2(0f, 4f);
        /// <summary>コマンドスタックの基準位置</summary>
        private static readonly Vector2 StackBasePosition = new Vector2(0f, -2.5f);
        /// <summary>コマンド矩形のサイズ</summary>
        private static readonly Vector2 CommandSize = new Vector2(3f, 1f);
        /// <summary>インボーカー矩形のサイズ</summary>
        private static readonly Vector2 InvokerSize = new Vector2(3f, 1.2f);
        /// <summary>コマンド間の垂直間隔</summary>
        private const float StackSpacing = 1.3f;
        /// <summary>プレイヤーの半径</summary>
        private const float PlayerRadius = 1f;
        /// <summary>プレイヤーの色</summary>
        private static readonly Color PlayerColor = new Color(0.3f, 0.6f, 0.8f, 1f);
        /// <summary>インボーカーの色</summary>
        private static readonly Color InvokerColor = new Color(0.6f, 0.5f, 0.7f, 1f);
        /// <summary>コマンドの色</summary>
        private static readonly Color CommandColor = new Color(0.4f, 0.7f, 0.5f, 1f);
        /// <summary>スタック内のコマンド数</summary>
        private int stackCount;

        /// <summary>
        /// バインド時にプレイヤーとインボーカーを配置して初期表示を構築する
        /// </summary>
        /// <param name="demo">バインドされたデモ</param>
        protected override void OnBind(IPatternDemo demo) {
            AddCircle("player", "Player\nPos:(0,0)\nHP:80", PlayerPosition, PlayerRadius, PlayerColor);
            AddRect("invoker", "ActionInvoker", InvokerPosition, InvokerSize, InvokerColor);

            stackCount = 0;
        }

        /// <summary>
        /// ステップに応じてコマンドの追加・Undo・Redoのアニメーションを更新する
        /// </summary>
        /// <param name="stepIndex">現在のステップインデックス</param>
        protected override void OnRefresh(int stepIndex) {
            VisualElement player = GetElement("player");
            VisualElement invoker = GetElement("invoker");

            switch (stepIndex) {
                case 0:
                    PushCommand("cmd0", "Move North");
                    player.SetLabel("Player\nPos:(0,1)\nHP:80");
                    player.Pulse(PulseColor, 0.5f);
                    invoker.Pulse(PulseColor, 0.5f);
                    break;
                case 1:
                    PushCommand("cmd1", "Move East");
                    player.SetLabel("Player\nPos:(1,1)\nHP:80");
                    player.Pulse(PulseColor, 0.5f);
                    invoker.Pulse(PulseColor, 0.5f);
                    break;
                case 2:
                    PushCommand("cmd2", "Heal 20");
                    player.SetLabel("Player\nPos:(1,1)\nHP:100");
                    player.Pulse(PulseColor, 0.5f);
                    invoker.Pulse(PulseColor, 0.5f);
                    break;
                case 3:
                    PopCommand("cmd2");
                    player.SetLabel("Player\nPos:(1,1)\nHP:80");
                    player.Pulse(HighlightColor, 0.5f);
                    invoker.Pulse(HighlightColor, 0.5f);
                    break;
                case 4:
                    PopCommand("cmd1");
                    player.SetLabel("Player\nPos:(0,1)\nHP:80");
                    player.Pulse(HighlightColor, 0.5f);
                    invoker.Pulse(HighlightColor, 0.5f);
                    break;
                case 5:
                    PushCommand("cmd1-redo", "Move East");
                    player.SetLabel("Player\nPos:(1,1)\nHP:80");
                    player.Pulse(PulseColor, 0.5f);
                    invoker.Pulse(PulseColor, 0.5f);
                    break;
            }
        }

        /// <summary>
        /// コマンドをスタックに追加して表示する
        /// </summary>
        /// <param name="commandId">コマンドの識別子</param>
        /// <param name="label">コマンドのラベル</param>
        private void PushCommand(string commandId, string label) {
            Vector2 position = StackBasePosition + new Vector2(0f, stackCount * StackSpacing);
            VisualElement command = AddRect(commandId, label, position, CommandSize, CommandColor);
            command.SetVisible(true);
            command.Pulse(PulseColor, 0.5f);
            stackCount++;
        }

        /// <summary>
        /// コマンドをスタックから除去して非表示にする
        /// </summary>
        /// <param name="commandId">コマンドの識別子</param>
        private void PopCommand(string commandId) {
            VisualElement command = GetElement(commandId);
            if (command != null) {
                command.SetColorImmediate(DimColor);
                command.Pulse(HighlightColor, 0.5f);
                command.SetVisible(false);
            }
            stackCount--;
            if (stackCount < 0) {
                stackCount = 0;
            }
        }
    }
}
