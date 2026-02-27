using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace DesignPatterns {
    /// <summary>
    /// 画面内にログを表示するコンポーネント
    /// Debug.Logの代わりに使用し、パターンの動作を画面上で可視化する
    /// </summary>
    public sealed class InGameLogger : MonoBehaviour {
        /// <summary>ログ表示用のテキストコンポーネント</summary>
        [SerializeField]
        private TextMeshProUGUI logText;

        /// <summary>表示するログの最大行数</summary>
        [SerializeField]
        private int maxLines = 30;

        /// <summary>シングルトンインスタンス（シーン内に1つ）</summary>
        private static InGameLogger instance;

        /// <summary>ログエントリのリスト</summary>
        private readonly List<LogEntry> entries = new List<LogEntry>();

        /// <summary>ログ表示用のStringBuilder</summary>
        private readonly StringBuilder builder = new StringBuilder();

        /// <summary>ログの色に対応するカラーコード</summary>
        private static readonly Dictionary<LogColor, string> ColorCodes = new Dictionary<LogColor, string> {
            { LogColor.White, "#FFFFFF" },
            { LogColor.Blue, "#5B9BD5" },
            { LogColor.Green, "#70AD47" },
            { LogColor.Orange, "#ED7D31" },
            { LogColor.Yellow, "#FFC000" },
            { LogColor.Red, "#FF4444" }
        };

        private void Awake() {
            if (instance != null && instance != this) {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }

        private void OnDestroy() {
            if (instance == this) {
                instance = null;
            }
        }

        /// <summary>
        /// ログメッセージを追加する
        /// </summary>
        /// <param name="message">表示するメッセージ</param>
        /// <param name="color">メッセージの色</param>
        public static void Log(string message, LogColor color = LogColor.White) {
            if (instance == null) {
                Debug.Log(message);
                return;
            }
            instance.AddEntry(new LogEntry(message, color));
        }

        /// <summary>
        /// 全てのログをクリアする
        /// </summary>
        public static void Clear() {
            if (instance == null) {
                return;
            }
            instance.entries.Clear();
            instance.RefreshDisplay();
        }

        /// <summary>
        /// ログエントリを追加して表示を更新する
        /// </summary>
        /// <param name="entry">追加するログエントリ</param>
        private void AddEntry(LogEntry entry) {
            entries.Add(entry);
            while (entries.Count > maxLines) {
                entries.RemoveAt(0);
            }
            RefreshDisplay();
        }

        /// <summary>
        /// ログテキスト表示を更新する
        /// </summary>
        private void RefreshDisplay() {
            if (logText == null) {
                return;
            }

            builder.Clear();
            for (int i = 0; i < entries.Count; i++) {
                LogEntry entry = entries[i];
                string colorCode = ColorCodes[entry.Color];
                builder.Append("<color=").Append(colorCode).Append(">").Append(entry.Message).Append("</color>");
                if (i < entries.Count - 1) {
                    builder.Append("\n");
                }
            }
            logText.text = builder.ToString();
        }
    }
}
