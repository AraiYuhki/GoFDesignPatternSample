using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using Xeon.Common.FlyweightScrollView.Model;

namespace DesignPatterns
{
    /// <summary>
    /// 画面内にログを表示するコンポーネント
    /// Debug.Logの代わりに使用し、パターンの動作を画面上で可視化する
    /// </summary>
    public sealed class InGameLogger : MonoBehaviour
    {

        /// <summary>シングルトンインスタンス（シーン内に1つ）</summary>
        private static InGameLogger instance;

        /// <summary>ログエントリのリスト</summary>
        private CircularBuffer<LogEntry> entries;

        public static InGameLogger Instance => instance;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }

        public void SetEntries(CircularBuffer<LogEntry> entries)
        {
            this.entries = entries;
        }

        private void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }

        /// <summary>
        /// ログメッセージを追加する
        /// </summary>
        /// <param name="message">表示するメッセージ</param>
        /// <param name="color">メッセージの色</param>
        public static void Log(string message, LogColor color = LogColor.White)
        {
            if (instance == null)
            {
                Debug.Log(message);
                return;
            }
            instance.AddEntry(new LogEntry(message, color));
        }

        /// <summary>
        /// 全てのログをクリアする
        /// </summary>
        public static void Clear()
        {
            if (instance == null)
            {
                return;
            }
            instance.entries.Clear();
        }

        /// <summary>
        /// ログエントリを追加して表示を更新する
        /// </summary>
        /// <param name="entry">追加するログエントリ</param>
        private void AddEntry(LogEntry entry)
        {
            entries.PushFront(entry);
        }
    }
}
