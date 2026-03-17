using GoFPatterns.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GoFPatterns.UI {
    /// <summary>
    /// パターン一覧画面
    /// PatternRepositoryからパターンカードを動的生成し、選択で詳細画面へ遷移する
    /// </summary>
    public class PatternListScreen : BaseScreen {
        /// <summary>パターンカードを並べる親Transform</summary>
        [SerializeField]
        private Transform cardContainer;
        /// <summary>パターンカードのプレハブ</summary>
        [SerializeField]
        private GameObject cardPrefab;
        /// <summary>カテゴリフィルタボタン群の親</summary>
        [SerializeField]
        private Transform filterContainer;

        /// <summary>現在のフィルタカテゴリ（nullなら全表示）</summary>
        private PatternCategory? currentFilter;

        /// <summary>
        /// 画面表示時にカード一覧を構築する
        /// </summary>
        /// <param name="context">未使用</param>
        protected override void OnShow(object context) {
            RebuildCards();
        }

        /// <summary>
        /// カード一覧を再構築する
        /// </summary>
        private void RebuildCards() {
            // 既存のカードを削除する
            for (int i = cardContainer.childCount - 1; i >= 0; i--) {
                Destroy(cardContainer.GetChild(i).gameObject);
            }

            var repository = DemoManager.Instance.Repository;
            if (repository == null) {
                return;
            }

            var definitions = currentFilter.HasValue
                ? repository.GetByCategory(currentFilter.Value)
                : repository.GetAllDefinitions();

            foreach (var def in definitions) {
                CreateCard(def);
            }
        }

        /// <summary>
        /// パターンカード1枚を生成する
        /// </summary>
        /// <param name="definition">パターン定義</param>
        private void CreateCard(PatternDefinition definition) {
            if (cardPrefab == null || cardContainer == null) {
                return;
            }

            var cardGo = Instantiate(cardPrefab, cardContainer);
            // カード内のテキスト要素を設定する
            var texts = cardGo.GetComponentsInChildren<TMP_Text>(true);
            if (texts.Length >= 1) {
                texts[0].text = definition.DisplayName;
            }
            if (texts.Length >= 2) {
                texts[1].text = definition.Summary;
            }

            // カードクリックで詳細画面へ遷移する
            var button = cardGo.GetComponent<Button>();
            if (button != null) {
                string patternId = definition.PatternId;
                button.onClick.AddListener(() => {
                    ScreenManager.Instance.NavigateTo("detail", patternId);
                });
            }
        }

        /// <summary>
        /// カテゴリフィルタを設定する（UIボタンから呼ばれる）
        /// </summary>
        /// <param name="categoryIndex">カテゴリインデックス（-1で全表示）</param>
        public void SetFilter(int categoryIndex) {
            currentFilter = categoryIndex >= 0 ? (PatternCategory?)categoryIndex : null;
            RebuildCards();
        }
    }
}
