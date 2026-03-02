using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Xeon.Common;


namespace DesignPatterns
{
    public class LogEntryItem : MonoBehaviour, IBindable<LogEntry>
    {
        /// <summary>ログの色に対応するカラーコード</summary>
        private static readonly Dictionary<LogColor, string> ColorCodes = new Dictionary<LogColor, string> {
            { LogColor.White, "#FFFFFF" },
            { LogColor.Blue, "#5B9BD5" },
            { LogColor.Green, "#70AD47" },
            { LogColor.Orange, "#ED7D31" },
            { LogColor.Yellow, "#FFC000" },
            { LogColor.Red, "#FF4444" }
        };

        [SerializeField]
        private TMP_Text label;

        public event Action<LogEntry> OnSelect;

        public void Bind(LogEntry data)
        {
            var colorCode = ColorCodes[data.Color];
            label.text = $"<color={colorCode}>{data.Message}</color>";
        }
    }
}
