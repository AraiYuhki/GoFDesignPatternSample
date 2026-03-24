using System.Collections.Generic;

namespace GoFPatterns.Patterns {
    // ---- Memento ----

    /// <summary>
    /// Mementoパターンのメメント
    /// TextEditorの状態スナップショットを不変に保持する
    /// </summary>
    public class EditorMemento {
        /// <summary>保存時のコンテンツ</summary>
        private readonly string content;

        /// <summary>保存時のコンテンツを取得する</summary>
        public string Content => content;

        /// <summary>
        /// EditorMementoを生成する
        /// </summary>
        /// <param name="content">保存するコンテンツ</param>
        public EditorMemento(string content) {
            this.content = content;
        }
    }

    // ---- Originator ----

    /// <summary>
    /// Mementoパターンのオリジネーター
    /// 編集中のコンテンツを保持し、メメントによるスナップショットの保存と復元を行う
    /// </summary>
    public class TextEditor {
        /// <summary>現在のコンテンツ</summary>
        private string content;

        /// <summary>現在のコンテンツを取得する</summary>
        public string Content => content;

        /// <summary>
        /// TextEditorを生成する
        /// </summary>
        public TextEditor() {
            content = "";
        }

        /// <summary>
        /// テキストを追記する
        /// </summary>
        /// <param name="text">追記するテキスト</param>
        public void Type(string text) {
            content += text;
        }

        /// <summary>
        /// 現在の状態をメメントとして保存する
        /// </summary>
        /// <returns>状態のスナップショット</returns>
        public EditorMemento Save() {
            return new EditorMemento(content);
        }

        /// <summary>
        /// メメントから状態を復元する
        /// </summary>
        /// <param name="memento">復元元のメメント</param>
        public void Restore(EditorMemento memento) {
            content = memento.Content;
        }
    }

    // ---- Caretaker ----

    /// <summary>
    /// Mementoパターンのケアテイカー
    /// メメントの履歴を管理してUndo機能を提供する
    /// </summary>
    public class EditorHistory {
        /// <summary>メメントのUndoスタック</summary>
        private readonly Stack<EditorMemento> undoStack = new Stack<EditorMemento>();

        /// <summary>Undoが可能かどうかを取得する</summary>
        public bool CanUndo => undoStack.Count > 0;
        /// <summary>保存されたスナップショット数を取得する</summary>
        public int Count => undoStack.Count;

        /// <summary>
        /// メメントを保存する
        /// </summary>
        /// <param name="memento">保存するメメント</param>
        public void Push(EditorMemento memento) {
            undoStack.Push(memento);
        }

        /// <summary>
        /// 最後に保存したメメントを取り出す
        /// </summary>
        /// <returns>最後のメメント（なければnull）</returns>
        public EditorMemento Pop() {
            if (!CanUndo) {
                return null;
            }
            return undoStack.Pop();
        }
    }

    // ---- Demo ----

    /// <summary>
    /// Mementoパターンのデモ
    /// TextEditorとEditorHistoryを例に状態のスナップショット保存と復元の仕組みを示す
    /// </summary>
    [PatternDemo("memento")]
    public class MementoDemo : BasePatternDemo {
        /// <summary>デモのパターンID</summary>
        public override string PatternId => "memento";

        /// <summary>デモの表示名</summary>
        public override string DisplayName => "Memento";

        /// <summary>編集対象のテキストエディタ</summary>
        private TextEditor editor;
        /// <summary>メメント履歴を管理するケアテイカー</summary>
        private EditorHistory history;

        /// <summary>
        /// リセット時にエディタと履歴を再生成する
        /// </summary>
        protected override void OnReset() {
            editor = null;
            history = null;
        }

        /// <summary>
        /// Mementoパターンのシナリオを構築する
        /// </summary>
        /// <param name="scenario">ステップを追加するシナリオ</param>
        protected override void BuildScenario(DemoScenario scenario) {
            editor = new TextEditor();
            history = new EditorHistory();

            scenario.AddStep(new DemoStep(
                "\"Hello\" と入力して保存する",
                () => {
                    editor.Type("Hello");
                    history.Push(editor.Save());
                    Log("TextEditor", "Type(Hello) + Save()", $"内容: \"{editor.Content}\" (履歴数: {history.Count})");
                }
            ));

            scenario.AddStep(new DemoStep(
                "\" World\" と入力して保存する",
                () => {
                    editor.Type(" World");
                    history.Push(editor.Save());
                    Log("TextEditor", "Type( World) + Save()", $"内容: \"{editor.Content}\" (履歴数: {history.Count})");
                }
            ));

            scenario.AddStep(new DemoStep(
                "\"!!!\" と入力する（保存しない）",
                () => {
                    editor.Type("!!!");
                    Log("TextEditor", "Type(!!!)", $"内容: \"{editor.Content}\" (未保存)");
                }
            ));

            scenario.AddStep(new DemoStep(
                "Undo — 最後の保存状態 \"Hello World\" に復元する",
                () => {
                    EditorMemento memento = history.Pop();
                    editor.Restore(memento);
                    Log("EditorHistory", "Undo()", $"復元後: \"{editor.Content}\" (履歴数: {history.Count})");
                }
            ));

            scenario.AddStep(new DemoStep(
                "Undo — さらに前の保存状態 \"Hello\" に復元する",
                () => {
                    EditorMemento memento = history.Pop();
                    editor.Restore(memento);
                    Log("EditorHistory", "Undo()", $"復元後: \"{editor.Content}\" (履歴数: {history.Count})");
                }
            ));

            scenario.AddStep(new DemoStep(
                "状態が正しく復元されていることを確認する",
                () => {
                    Log("TextEditor", "状態確認", $"最終内容: \"{editor.Content}\"");
                    Log("EditorHistory", "状態確認", $"残り履歴: {history.Count}, Undo可能: {history.CanUndo}");
                }
            ));
        }
    }
}
