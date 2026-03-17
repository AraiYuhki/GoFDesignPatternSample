using System.Collections.Generic;
using UnityEngine;

namespace GoFPatterns.Core {
    /// <summary>
    /// パターン1つ分の説明データを保持するScriptableObject
    /// Inspector上でパターンのメタ情報を編集・管理する
    /// </summary>
    [CreateAssetMenu(fileName = "NewPatternDefinition", menuName = "GoFPatterns/Pattern Definition")]
    public class PatternDefinition : ScriptableObject {
        /// <summary>パターンの一意な識別子（例: "singleton"）</summary>
        [SerializeField]
        private string patternId;

        /// <summary>表示名（例: "Singleton"）</summary>
        [SerializeField]
        private string displayName;

        /// <summary>表示名（日本語）（例: "シングルトン"）</summary>
        [SerializeField]
        private string displayNameJp;

        /// <summary>カテゴリ</summary>
        [SerializeField]
        private PatternCategory category;

        /// <summary>一言説明</summary>
        [SerializeField, TextArea(1, 2)]
        private string summary;

        /// <summary>概要（詳細画面用）</summary>
        [SerializeField, TextArea(3, 6)]
        private string description;

        /// <summary>解決したい課題</summary>
        [SerializeField, TextArea(2, 4)]
        private string problemToSolve;

        /// <summary>使いどころ</summary>
        [SerializeField, TextArea(2, 4)]
        private string whenToUse;

        /// <summary>メリット</summary>
        [SerializeField, TextArea(2, 4)]
        private string advantages;

        /// <summary>注意点・デメリット</summary>
        [SerializeField, TextArea(2, 4)]
        private string disadvantages;

        /// <summary>主要な登場人物（クラス名など）</summary>
        [SerializeField]
        private string[] participants;

        /// <summary>関連パターンのID一覧</summary>
        [SerializeField]
        private string[] relatedPatternIds;

        /// <summary>デモの各ステップの説明文</summary>
        [SerializeField, TextArea(1, 3)]
        private string[] stepDescriptions;

        /// <summary>パターンIDを取得する</summary>
        public string PatternId => patternId;
        /// <summary>表示名を取得する</summary>
        public string DisplayName => displayName;
        /// <summary>日本語表示名を取得する</summary>
        public string DisplayNameJp => displayNameJp;
        /// <summary>カテゴリを取得する</summary>
        public PatternCategory Category => category;
        /// <summary>一言説明を取得する</summary>
        public string Summary => summary;
        /// <summary>概要を取得する</summary>
        public string Description => description;
        /// <summary>解決したい課題を取得する</summary>
        public string ProblemToSolve => problemToSolve;
        /// <summary>使いどころを取得する</summary>
        public string WhenToUse => whenToUse;
        /// <summary>メリットを取得する</summary>
        public string Advantages => advantages;
        /// <summary>注意点を取得する</summary>
        public string Disadvantages => disadvantages;
        /// <summary>主要登場人物を取得する</summary>
        public IReadOnlyList<string> Participants => participants;
        /// <summary>関連パターンIDを取得する</summary>
        public IReadOnlyList<string> RelatedPatternIds => relatedPatternIds;
        /// <summary>ステップ説明文を取得する</summary>
        public IReadOnlyList<string> StepDescriptions => stepDescriptions;
    }
}
