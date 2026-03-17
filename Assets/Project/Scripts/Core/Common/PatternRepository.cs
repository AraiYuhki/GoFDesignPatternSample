using System.Collections.Generic;
using UnityEngine;

namespace GoFPatterns.Core {
    /// <summary>
    /// PatternDefinitionの検索・取得を提供するリポジトリ
    /// Resources または SerializeField 経由で定義データを保持する
    /// </summary>
    [CreateAssetMenu(fileName = "PatternRepository", menuName = "GoFPatterns/Pattern Repository")]
    public class PatternRepository : ScriptableObject {
        /// <summary>登録されたパターン定義一覧</summary>
        [SerializeField]
        private PatternDefinition[] definitions;

        /// <summary>IDをキーとする検索用辞書</summary>
        private Dictionary<string, PatternDefinition> definitionMap;

        /// <summary>
        /// 全パターン定義を取得する
        /// </summary>
        /// <returns>パターン定義の読み取り専用リスト</returns>
        public IReadOnlyList<PatternDefinition> GetAllDefinitions() {
            return definitions;
        }

        /// <summary>
        /// 指定IDのパターン定義を取得する
        /// </summary>
        /// <param name="patternId">パターンID</param>
        /// <returns>パターン定義（見つからない場合はnull）</returns>
        public PatternDefinition GetDefinition(string patternId) {
            EnsureMap();
            definitionMap.TryGetValue(patternId, out var definition);
            return definition;
        }

        /// <summary>
        /// 指定カテゴリのパターン定義を取得する
        /// </summary>
        /// <param name="category">フィルタするカテゴリ</param>
        /// <returns>該当するパターン定義のリスト</returns>
        public List<PatternDefinition> GetByCategory(PatternCategory category) {
            var result = new List<PatternDefinition>();
            foreach (var def in definitions) {
                if (def.Category == category) {
                    result.Add(def);
                }
            }
            return result;
        }

        /// <summary>
        /// 検索用辞書を遅延構築する
        /// </summary>
        private void EnsureMap() {
            if (definitionMap != null) {
                return;
            }
            definitionMap = new Dictionary<string, PatternDefinition>();
            foreach (var def in definitions) {
                if (def != null && !string.IsNullOrEmpty(def.PatternId)) {
                    definitionMap[def.PatternId] = def;
                }
            }
        }
    }
}
