using System;

namespace GoFPatterns.Patterns.Visualization {
    /// <summary>
    /// パターンのビジュアライゼーション実装を自動検出するための属性
    /// BasePatternVisualizationのサブクラスに付与してパターンIDと関連付ける
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class PatternVisualizationAttribute : Attribute {
        /// <summary>対応するパターンID</summary>
        public string PatternId { get; }

        /// <summary>
        /// パターンIDを指定してビジュアライゼーション属性を生成する
        /// </summary>
        /// <param name="patternId">対応するパターンのID</param>
        public PatternVisualizationAttribute(string patternId) {
            PatternId = patternId;
        }
    }
}
