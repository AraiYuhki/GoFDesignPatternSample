namespace DesignPatterns.Structural.Proxy {
    /// <summary>
    /// 画像のプロキシ（Proxy）
    /// 実際の画像の読み込みを遅延させ、必要になるまでロードしない
    ///
    /// 【Proxyパターンにおける役割】
    /// RealSubjectへのアクセスを制御する代理オブジェクト
    /// ここではVirtual Proxyとして遅延ロードを実現する
    /// </summary>
    public sealed class ImageProxy : IImage {
        /// <inheritdoc/>
        public string FileName { get; }

        /// <summary>画像のサイズ（KB）</summary>
        private readonly int sizeKb;

        /// <summary>実画像（遅延初期化）</summary>
        private RealImage realImage;

        /// <summary>
        /// 画像プロキシを生成する（この時点ではロードしない）
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <param name="sizeKb">画像サイズ（KB）</param>
        public ImageProxy(string fileName, int sizeKb) {
            FileName = fileName;
            this.sizeKb = sizeKb;
            InGameLogger.Log($"  [Proxy] {fileName} のプロキシを作成（まだロードしない）", LogColor.Green);
        }

        /// <inheritdoc/>
        public void Display() {
            if (realImage == null) {
                InGameLogger.Log($"  [Proxy] 初回表示のため {FileName} をロードします", LogColor.Green);
                realImage = new RealImage(FileName, sizeKb);
            } else {
                InGameLogger.Log($"  [Proxy] {FileName} はキャッシュ済み", LogColor.Green);
            }
            realImage.Display();
        }

        /// <inheritdoc/>
        public string GetInfo() {
            if (realImage != null) {
                return $"{FileName} [ロード済み] ({sizeKb} KB)";
            }
            return $"{FileName} [未ロード] ({sizeKb} KB)";
        }
    }
}
