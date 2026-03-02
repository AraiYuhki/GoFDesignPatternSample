namespace DesignPatterns.Structural.Proxy {
    /// <summary>
    /// 画像のインターフェース（Subject）
    ///
    /// 【Proxyパターンの意図】
    /// あるオブジェクトへのアクセスを制御するための代理オブジェクトを提供する
    /// ここでは遅延ロード（Virtual Proxy）を実装する
    /// </summary>
    public interface IImage {
        /// <summary>ファイル名</summary>
        string FileName { get; }

        /// <summary>
        /// 画像を表示する
        /// </summary>
        void Display();

        /// <summary>
        /// 画像の情報を返す
        /// </summary>
        /// <returns>画像の状態情報</returns>
        string GetInfo();
    }
}
