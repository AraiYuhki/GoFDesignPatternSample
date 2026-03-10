namespace DesignPatterns.NodeGraph {
    /// <summary>
    /// ノードの視覚的な状態を表す列挙型
    /// 状態に応じてノードの色や不透明度が変化する
    /// </summary>
    public enum NodeState {
        /// <summary>通常状態（グレー）</summary>
        Default,
        /// <summary>アクティブ状態（カテゴリ色でハイライト）</summary>
        Active,
        /// <summary>生成中（スケールアニメーション）</summary>
        Creating,
        /// <summary>破棄中（フェードアウトアニメーション）</summary>
        Destroying,
        /// <summary>強調表示（明るいボーダー）</summary>
        Highlighted,
        /// <summary>非強調表示（半透明）</summary>
        Dimmed
    }
}
