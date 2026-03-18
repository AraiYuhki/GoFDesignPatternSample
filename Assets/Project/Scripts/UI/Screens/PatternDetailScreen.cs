using GoFPatterns.Core;
using GoFPatterns.Patterns;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GoFPatterns.UI {
    /// <summary>
    /// パターン詳細画面
    /// PatternDefinitionの情報を表示し、デモ開始ボタンでデモ画面へ遷移する
    /// </summary>
    public class PatternDetailScreen : BaseScreen {
        /// <summary>パターン名ラベル</summary>
        [SerializeField]
        private TMP_Text patternNameLabel;
        /// <summary>カテゴリラベル</summary>
        [SerializeField]
        private TMP_Text categoryLabel;
        /// <summary>概要テキスト</summary>
        [SerializeField]
        private TMP_Text descriptionText;
        /// <summary>使いどころテキスト</summary>
        [SerializeField]
        private TMP_Text whenToUseText;
        /// <summary>メリットテキスト</summary>
        [SerializeField]
        private TMP_Text advantagesText;
        /// <summary>注意点テキスト</summary>
        [SerializeField]
        private TMP_Text disadvantagesText;
        /// <summary>デモ開始ボタン</summary>
        [SerializeField]
        private Button startDemoButton;
        /// <summary>戻るボタン</summary>
        [SerializeField]
        private Button backButton;

        /// <summary>現在表示中のパターンID</summary>
        private string currentPatternId;

        /// <summary>
        /// 起動時にボタンイベントを登録する
        /// </summary>
        private void Start() {
            if (startDemoButton != null) {
                startDemoButton.onClick.AddListener(OnStartDemoClicked);
            }
            if (backButton != null) {
                backButton.onClick.AddListener(OnBackClicked);
            }
        }

        /// <summary>
        /// 画面表示時にパターン情報をセットする
        /// </summary>
        /// <param name="context">パターンID（string）</param>
        protected override void OnShow(object context) {
            currentPatternId = context as string;
            if (string.IsNullOrEmpty(currentPatternId)) {
                return;
            }

            var definition = DemoManager.Instance.Repository.GetDefinition(currentPatternId);
            if (definition == null) {
                return;
            }

            SetText(patternNameLabel, $"{definition.DisplayName} ({definition.DisplayNameJp})");
            SetText(categoryLabel, GetCategoryDisplayName(definition.Category));
            SetText(descriptionText, definition.Description);
            SetText(whenToUseText, definition.WhenToUse);
            SetText(advantagesText, definition.Advantages);
            SetText(disadvantagesText, definition.Disadvantages);
        }

        /// <summary>
        /// デモ開始ボタン押下時の処理
        /// </summary>
        private void OnStartDemoClicked() {
            if (string.IsNullOrEmpty(currentPatternId)) {
                return;
            }
            ScreenManager.Instance.NavigateTo("demo", currentPatternId);
        }

        /// <summary>
        /// 戻るボタン押下時の処理
        /// </summary>
        private void OnBackClicked() {
            ScreenManager.Instance.GoBack();
        }

        /// <summary>
        /// テキストを安全にセットする
        /// </summary>
        /// <param name="label">対象のテキストコンポーネント</param>
        /// <param name="value">設定する値</param>
        private static void SetText(TMP_Text label, string value) {
            if (label != null) {
                label.text = value ?? "";
            }
        }

        /// <summary>
        /// カテゴリの表示名を返す
        /// </summary>
        /// <param name="category">カテゴリ</param>
        /// <returns>日本語表示名</returns>
        private static string GetCategoryDisplayName(PatternCategory category) {
            switch (category) {
                case PatternCategory.Creational:
                    return "生成パターン (Creational)";
                case PatternCategory.Structural:
                    return "構造パターン (Structural)";
                case PatternCategory.Behavioral:
                    return "振る舞いパターン (Behavioral)";
                default:
                    return "";
            }
        }
    }
}
