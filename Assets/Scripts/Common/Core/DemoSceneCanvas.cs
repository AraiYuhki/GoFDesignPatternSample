using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Xeon.Common.FlyweightScrollView;
using Xeon.Common.FlyweightScrollView.Model;

namespace DesignPatterns
{
    public class DemoSceneCanvas : MonoBehaviour
    {
        [SerializeField]
        private Image accentLine;
        [SerializeField]
        private TMP_Text patternNameLabel;
        [SerializeField]
        private TMP_Text categoryNameLabel;
        [SerializeField]
        private Button backButton;
        [SerializeField]
        private TMP_Text detailLabel;
        [SerializeField]
        private FlyweightVerticalScrollView scrollView;
        [SerializeField]
        private LogEntryItem itemPrefab;

        private FlyweightScrollViewController<LogEntry, LogEntryItem> scrollViewController;

        /// <summary>ログエントリのリスト</summary>
        private CircularBuffer<LogEntry> entries = new CircularBuffer<LogEntry>(3000, true);

        private void Start()
        {
            InGameLogger.Instance.SetEntries(entries);
            scrollViewController = new FlyweightScrollViewController<LogEntry, LogEntryItem>(itemPrefab, entries);
            scrollView.Setup(scrollViewController);
        }
    }
}
