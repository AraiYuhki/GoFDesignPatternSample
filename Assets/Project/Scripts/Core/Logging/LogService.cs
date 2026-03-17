using System;
using System.Collections.Generic;

namespace GoFPatterns.Core {
    /// <summary>
    /// デモ実行ログの蓄積と通知を管理するサービス
    /// UIのLogPanelと連携してログを画面表示する
    /// </summary>
    public class LogService {
        /// <summary>蓄積されたログエントリ</summary>
        private readonly List<string> entries = new List<string>();
        /// <summary>ログの最大保持件数</summary>
        private const int MaxEntries = 500;

        /// <summary>ログが追加されたときに発火するイベント</summary>
        public event Action<string> OnLogAdded;
        /// <summary>ログがクリアされたときに発火するイベント</summary>
        public event Action OnLogCleared;

        /// <summary>蓄積されたログの読み取り専用リスト</summary>
        public IReadOnlyList<string> Entries => entries;

        /// <summary>
        /// ログを追加する
        /// </summary>
        /// <param name="message">ログメッセージ</param>
        public void Log(string message) {
            if (entries.Count >= MaxEntries) {
                entries.RemoveAt(0);
            }
            entries.Add(message);
            OnLogAdded?.Invoke(message);
        }

        /// <summary>
        /// アクター・アクション・結果の形式でログを追加する
        /// </summary>
        /// <param name="stepIndex">ステップ番号</param>
        /// <param name="actor">アクター名</param>
        /// <param name="action">アクション名</param>
        /// <param name="result">結果の説明</param>
        public void LogStep(int stepIndex, string actor, string action, string result = "") {
            string entry = string.IsNullOrEmpty(result)
                ? $"[Step {stepIndex:D2}] {actor} -> {action}"
                : $"[Step {stepIndex:D2}] {actor} -> {action} -> {result}";
            Log(entry);
        }

        /// <summary>
        /// 全ログをクリアする
        /// </summary>
        public void Clear() {
            entries.Clear();
            OnLogCleared?.Invoke();
        }
    }
}
