namespace GoFPatterns.Patterns {
    /// <summary>
    /// パターンのビジュアライゼーションを提供するインターフェース
    /// デモの状態に応じて2D表現を更新する
    /// </summary>
    public interface IPatternVisualization {
        /// <summary>
        /// デモにバインドして初期表示を構築する
        /// </summary>
        /// <param name="demo">バインドするデモ</param>
        void Bind(IPatternDemo demo);
        /// <summary>
        /// 現在のデモ状態に合わせて表示を更新する
        /// </summary>
        void Refresh();
        /// <summary>
        /// 指定の要素をハイライトする
        /// </summary>
        /// <param name="targetId">ハイライト対象の識別子</param>
        void Highlight(string targetId);
        /// <summary>
        /// ビジュアライゼーションをリセットする
        /// </summary>
        void Clear();
    }
}
