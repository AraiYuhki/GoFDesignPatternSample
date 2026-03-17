using System;

namespace GoFPatterns.Patterns {
    /// <summary>
    /// デモの1ステップを表すクラス
    /// ステップ実行時のアクションと説明文を保持する
    /// </summary>
    public class DemoStep {
        /// <summary>ステップの説明文</summary>
        public string Description { get; }
        /// <summary>ステップ実行時に呼ばれるアクション</summary>
        public Action Execute { get; }
        /// <summary>ステップのアクター（実行主体の名前）</summary>
        public string Actor { get; }
        /// <summary>ステップのアクション名</summary>
        public string ActionName { get; }

        /// <summary>
        /// DemoStepを生成する
        /// </summary>
        /// <param name="description">ステップの説明文</param>
        /// <param name="execute">実行時のアクション</param>
        /// <param name="actor">実行主体の名前</param>
        /// <param name="actionName">アクション名</param>
        public DemoStep(string description, Action execute, string actor = "", string actionName = "") {
            Description = description;
            Execute = execute;
            Actor = actor;
            ActionName = actionName;
        }
    }
}
