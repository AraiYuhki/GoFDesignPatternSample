namespace GoFPatterns.Patterns {
    // ---- Subject interface ----

    /// <summary>
    /// Proxyパターンのサブジェクトインターフェース
    /// 画像の表示操作を定義する
    /// </summary>
    public interface IImage {
        /// <summary>
        /// 画像を表示する
        /// </summary>
        /// <returns>表示結果の説明文</returns>
        string Display();
    }

    // ---- RealSubject ----

    /// <summary>
    /// 実際の画像クラス（生成コストが高い）
    /// ディスクからの読み込みをシミュレートする
    /// </summary>
    public class RealImage : IImage {
        /// <summary>画像のファイル名</summary>
        private readonly string fileName;

        /// <summary>画像のファイル名を取得する</summary>
        public string FileName => fileName;

        /// <summary>
        /// RealImageを生成する（ディスクからの読み込みを実行）
        /// </summary>
        /// <param name="fileName">画像のファイル名</param>
        public RealImage(string fileName) {
            this.fileName = fileName;
            LoadFromDisk();
        }

        /// <summary>
        /// 画像を表示する
        /// </summary>
        /// <returns>表示結果の説明文</returns>
        public string Display() {
            return $"RealImage: \"{fileName}\" を表示中";
        }

        /// <summary>
        /// ディスクからの読み込みをシミュレートする
        /// </summary>
        private void LoadFromDisk() {
            // 実際のアプリケーションでは重い処理がここで発生する
        }
    }

    // ---- Proxy ----

    /// <summary>
    /// 遅延読み込みプロキシ
    /// 初回のDisplay()呼び出しまでRealImageの生成を遅延させる
    /// </summary>
    public class ProxyImage : IImage {
        /// <summary>画像のファイル名</summary>
        private readonly string fileName;

        /// <summary>遅延生成されるRealImage</summary>
        private RealImage realImage;

        /// <summary>RealImageが生成済みかどうかを取得する</summary>
        public bool IsLoaded => realImage != null;

        /// <summary>画像のファイル名を取得する</summary>
        public string FileName => fileName;

        /// <summary>
        /// ProxyImageを生成する（この時点ではRealImageは生成しない）
        /// </summary>
        /// <param name="fileName">画像のファイル名</param>
        public ProxyImage(string fileName) {
            this.fileName = fileName;
        }

        /// <summary>
        /// 画像を表示する（初回呼び出し時にRealImageを遅延生成する）
        /// </summary>
        /// <returns>表示結果の説明文</returns>
        public string Display() {
            if (realImage == null) {
                realImage = new RealImage(fileName);
            }
            return realImage.Display();
        }
    }

    // ---- Demo ----

    /// <summary>
    /// Proxyパターンのデモ
    /// ProxyImageによる遅延読み込みでリソース生成コストを制御する仕組みを示す
    /// </summary>
    [PatternDemo("proxy")]
    public class ProxyDemo : BasePatternDemo {
        /// <summary>デモのパターンID</summary>
        public override string PatternId => "proxy";

        /// <summary>デモの表示名</summary>
        public override string DisplayName => "Proxy";

        /// <summary>プロキシ画像A</summary>
        private ProxyImage proxyA;

        /// <summary>プロキシ画像B</summary>
        private ProxyImage proxyB;

        /// <summary>画像Aのファイル名</summary>
        private const string FileNameA = "hero_portrait.png";

        /// <summary>画像Bのファイル名</summary>
        private const string FileNameB = "world_map.png";

        /// <summary>
        /// リセット時にドメインオブジェクトをクリアする
        /// </summary>
        protected override void OnReset() {
            proxyA = null;
            proxyB = null;
        }

        /// <summary>
        /// Proxyパターンのシナリオを構築する
        /// </summary>
        /// <param name="scenario">ステップを追加するシナリオ</param>
        protected override void BuildScenario(DemoScenario scenario) {
            scenario.AddStep(new DemoStep(
                "ProxyImageを作成する（まだ読み込みは発生しない）",
                () => {
                    proxyA = new ProxyImage(FileNameA);
                    proxyB = new ProxyImage(FileNameB);
                    Log("Client", $"new ProxyImage(\"{FileNameA}\")", $"IsLoaded: {proxyA.IsLoaded}");
                    Log("Client", $"new ProxyImage(\"{FileNameB}\")", $"IsLoaded: {proxyB.IsLoaded}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "ProxyA.Display()を初回呼び出しする（RealImageが生成される）",
                () => {
                    string result = proxyA.Display();
                    Log("ProxyImage", $"Display() — 初回", $"RealImage生成 → {result}");
                    Log("検証", "IsLoaded", $"{proxyA.IsLoaded}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "ProxyA.Display()を2回目呼び出しする（キャッシュ済みを使用）",
                () => {
                    string result = proxyA.Display();
                    Log("ProxyImage", $"Display() — 2回目", $"キャッシュ済み → {result}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "ProxyBはまだ未読み込みであることを確認する",
                () => {
                    Log("検証", $"proxyB.IsLoaded", $"{proxyB.IsLoaded} — 必要になるまで生成しない");
                }
            ));

            scenario.AddStep(new DemoStep(
                "ProxyB.Display()を呼び出して遅延読み込みを発動する",
                () => {
                    string result = proxyB.Display();
                    Log("ProxyImage", $"Display() — 初回", $"RealImage生成 → {result}");
                    Log("検証", "IsLoaded", $"{proxyB.IsLoaded}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "プロキシがアクセス制御を提供していることを確認する",
                () => {
                    Log("まとめ", "Proxy効果", $"A: IsLoaded={proxyA.IsLoaded}, B: IsLoaded={proxyB.IsLoaded}");
                    Log("まとめ", "遅延読み込み", "必要時まで重い処理を先送りし、以降はキャッシュを使用");
                }
            ));
        }
    }
}
