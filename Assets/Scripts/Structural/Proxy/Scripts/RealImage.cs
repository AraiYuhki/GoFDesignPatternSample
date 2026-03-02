namespace DesignPatterns.Structural.Proxy {
    /// <summary>
    /// 実際の画像クラス（RealSubject）
    /// 生成時に重い読み込み処理が発生する
    /// </summary>
    public sealed class RealImage : IImage {
        /// <inheritdoc/>
        public string FileName { get; }

        /// <summary>画像のサイズ（シミュレーション用）</summary>
        private readonly int sizeKb;

        /// <summary>
        /// 実画像を生成し、ロード処理を実行する
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <param name="sizeKb">画像サイズ（KB）</param>
        public RealImage(string fileName, int sizeKb) {
            FileName = fileName;
            this.sizeKb = sizeKb;
            LoadFromDisk();
        }

        /// <summary>
        /// ディスクから画像をロードする（シミュレーション）
        /// </summary>
        private void LoadFromDisk() {
            InGameLogger.Log($"  [RealImage] {FileName} をロード中... ({sizeKb} KB)", LogColor.White);
            InGameLogger.Log($"  [RealImage] {FileName} のロード完了", LogColor.White);
        }

        /// <inheritdoc/>
        public void Display() {
            InGameLogger.Log($"  [RealImage] {FileName} を表示中 ({sizeKb} KB)", LogColor.White);
        }

        /// <inheritdoc/>
        public string GetInfo() {
            return $"{FileName} [ロード済み] ({sizeKb} KB)";
        }
    }
}
