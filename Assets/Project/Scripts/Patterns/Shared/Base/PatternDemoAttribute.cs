using System;

namespace GoFPatterns.Patterns {
    /// <summary>
    /// DemoManagerへの自動登録対象としてデモクラスをマークする属性
    /// クラスに付与するとリフレクションによってデモが検出される
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class PatternDemoAttribute : Attribute {
        /// <summary>パターンの一意なID</summary>
        public string PatternId { get; }

        /// <summary>
        /// PatternDemoAttributeを生成する
        /// </summary>
        /// <param name="patternId">パターンの一意なID</param>
        public PatternDemoAttribute(string patternId) {
            PatternId = patternId;
        }
    }
}
