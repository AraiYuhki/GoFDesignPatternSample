using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Xeon.Common.FlyweightScrollView;

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

        private void Start()
        {
            scrollViewController = new FlyweightScrollViewController<LogEntry, LogEntryItem>(itemPrefab, InGameLogger.Instance.Entries);
            scrollView.Setup(scrollViewController);
        }
    }
}
