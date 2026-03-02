namespace DesignPatterns.Structural.Adapter {
    /// <summary>
    /// 新しいオーディオシステムのインターフェース（Target）
    ///
    /// 【Adapterパターンにおける役割】
    /// クライアントが期待するインターフェース
    /// アダプターがこのインターフェースを実装し、旧システムとの橋渡しをする
    /// </summary>
    public interface IModernAudio {
        /// <summary>
        /// 音声を再生する
        /// </summary>
        /// <param name="trackName">トラック名</param>
        /// <param name="volume">音量（0.0〜1.0）</param>
        void Play(string trackName, float volume);

        /// <summary>
        /// 再生を停止する
        /// </summary>
        void Stop();

        /// <summary>
        /// 現在の再生状態を取得する
        /// </summary>
        /// <returns>再生状態の文字列</returns>
        string GetStatus();
    }
}
