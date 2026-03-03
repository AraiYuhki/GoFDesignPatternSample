using TMPro;
using UnityEngine;

namespace DesignPatterns
{
    /// <summary>
    /// パターンの説明情報を表示するUIパネル
    /// 各デモシーンに配置し、パターンの名前・カテゴリ・概要を表示する
    /// </summary>
    public sealed class PatternInfoPanel : MonoBehaviour
    {
        /// <summary>パターン名表示用テキスト</summary>
        [SerializeField]
        private TextMeshProUGUI patternNameText;

        /// <summary>カテゴリ表示用テキスト</summary>
        [SerializeField]
        private TextMeshProUGUI categoryText;

        /// <summary>概要説明表示用テキスト</summary>
        [SerializeField]
        private TextMeshProUGUI descriptionText;

        /// <summary>
        /// パターン情報を設定して表示を更新する
        /// </summary>
        /// <param name="patternName">パターン名</param>
        /// <param name="category">カテゴリ名</param>
        /// <param name="description">概要説明</param>
        public void SetInfo(string patternName, string category, string description)
        {
            if (patternNameText != null)
            {
                patternNameText.text = patternName;
            }
            if (categoryText != null)
            {
                categoryText.text = category;
            }
            if (descriptionText != null)
            {
                descriptionText.text = description;
            }
        }
    }
}
