using UnityEngine;

namespace GoFPatterns.Patterns.Visualization {
    /// <summary>
    /// Mementoパターンのビジュアライゼーション
    /// エディタとスナップショット履歴スタックを配置し、保存・復元の流れを可視化する
    /// </summary>
    [PatternVisualization("memento")]
    public class MementoVisualization : BasePatternVisualization {
        /// <summary>エディタの配置位置</summary>
        private static readonly Vector2 EditorPosition = new Vector2(-3.5f, 0f);
        /// <summary>履歴スタックの基準位置</summary>
        private static readonly Vector2 HistoryBasePosition = new Vector2(3f, -2.5f);
        /// <summary>エディタ矩形のサイズ</summary>
        private static readonly Vector2 EditorSize = new Vector2(3.5f, 2f);
        /// <summary>メメント矩形のサイズ</summary>
        private static readonly Vector2 MementoSize = new Vector2(3f, 1f);
        /// <summary>メメント間の垂直間隔</summary>
        private const float MementoSpacing = 1.3f;
        /// <summary>エディタの色</summary>
        private static readonly Color EditorColor = new Color(0.3f, 0.5f, 0.8f, 1f);
        /// <summary>メメントの色</summary>
        private static readonly Color MementoColor = new Color(0.4f, 0.7f, 0.5f, 1f);
        /// <summary>履歴ラベルの配置位置</summary>
        private static readonly Vector2 HistoryLabelPosition = new Vector2(3f, 3.5f);
        /// <summary>履歴ラベルのサイズ</summary>
        private static readonly Vector2 HistoryLabelSize = new Vector2(3f, 1f);
        /// <summary>スタック内のメメント数</summary>
        private int mementoCount;

        /// <summary>
        /// バインド時にエディタと履歴ラベルを配置して初期表示を構築する
        /// </summary>
        /// <param name="demo">バインドされたデモ</param>
        protected override void OnBind(IPatternDemo demo) {
            AddRect("editor", "TextEditor\n(空)", EditorPosition, EditorSize, EditorColor);
            AddRect("history-label", "EditorHistory", HistoryLabelPosition, HistoryLabelSize, DimColor);

            mementoCount = 0;
        }

        /// <summary>
        /// ステップに応じて保存・復元のアニメーションを更新する
        /// </summary>
        /// <param name="stepIndex">現在のステップインデックス</param>
        protected override void OnRefresh(int stepIndex) {
            VisualElement editor = GetElement("editor");

            switch (stepIndex) {
                case 0:
                    editor.SetLabel("TextEditor\n\"Hello\"");
                    editor.Pulse(PulseColor, 0.5f);
                    PushMemento("memento0", "Hello");
                    break;
                case 1:
                    editor.SetLabel("TextEditor\n\"Hello World\"");
                    editor.Pulse(PulseColor, 0.5f);
                    PushMemento("memento1", "Hello World");
                    break;
                case 2:
                    editor.SetLabel("TextEditor\n\"Hello World!!!\"");
                    editor.Pulse(HighlightColor, 0.5f);
                    break;
                case 3:
                    PopMemento("memento1");
                    editor.SetLabel("TextEditor\n\"Hello World\"");
                    editor.Pulse(PulseColor, 0.5f);
                    break;
                case 4:
                    PopMemento("memento0");
                    editor.SetLabel("TextEditor\n\"Hello\"");
                    editor.Pulse(PulseColor, 0.5f);
                    break;
                case 5:
                    editor.Pulse(HighlightColor, 0.5f);
                    GetElement("history-label")?.Pulse(HighlightColor, 0.5f);
                    break;
            }
        }

        /// <summary>
        /// メメントを履歴スタックに追加して表示する
        /// </summary>
        /// <param name="mementoId">メメントの識別子</param>
        /// <param name="content">保存するコンテンツ</param>
        private void PushMemento(string mementoId, string content) {
            Vector2 position = HistoryBasePosition + new Vector2(0f, mementoCount * MementoSpacing);
            VisualElement memento = AddRect(mementoId, content, position, MementoSize, MementoColor);
            memento.SetVisible(true);
            memento.Pulse(PulseColor, 0.5f);
            mementoCount++;

            GetElement("history-label")?.SetLabel($"EditorHistory ({mementoCount})");
        }

        /// <summary>
        /// メメントを履歴スタックから除去して非表示にする
        /// </summary>
        /// <param name="mementoId">メメントの識別子</param>
        private void PopMemento(string mementoId) {
            VisualElement memento = GetElement(mementoId);
            if (memento != null) {
                memento.Pulse(HighlightColor, 0.5f);
                memento.SetVisible(false);
            }
            mementoCount--;
            if (mementoCount < 0) {
                mementoCount = 0;
            }

            GetElement("history-label")?.SetLabel($"EditorHistory ({mementoCount})");
        }
    }
}
