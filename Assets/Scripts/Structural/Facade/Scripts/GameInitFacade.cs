namespace DesignPatterns.Structural.Facade
{
    /// <summary>
    /// ゲーム初期化のファサード（Facade）
    ///
    /// 【Facadeパターンの意図】
    /// サブシステム群への統一的なインターフェースを提供する
    /// クライアントは複雑な内部システムを意識せず、シンプルなAPIで操作できる
    /// </summary>
    public sealed class GameInitFacade
    {
        /// <summary>オーディオサブシステム</summary>
        private readonly AudioSubSystem audio;

        /// <summary>グラフィックスサブシステム</summary>
        private readonly GraphicsSubSystem graphics;

        /// <summary>入力サブシステム</summary>
        private readonly InputSubSystem input;

        /// <summary>ネットワークサブシステム</summary>
        private readonly NetworkSubSystem network;

        /// <summary>
        /// ファサードを生成する
        /// </summary>
        public GameInitFacade()
        {
            audio = new AudioSubSystem();
            graphics = new GraphicsSubSystem();
            input = new InputSubSystem();
            network = new NetworkSubSystem();
        }

        /// <summary>
        /// ゲームの全初期化を一括で実行する
        /// 複雑なサブシステムの初期化順序をファサードが管理する
        /// </summary>
        public void InitializeGame()
        {
            InGameLogger.Log("▶ グラフィックス初期化", LogColor.Green);
            graphics.Initialize();
            graphics.SetResolution(1920, 1080);

            InGameLogger.Log("▶ オーディオ初期化", LogColor.Green);
            audio.Initialize();

            InGameLogger.Log("▶ 入力システム初期化", LogColor.Green);
            input.Initialize();

            InGameLogger.Log("▶ ネットワーク接続", LogColor.Green);
            network.Connect();
            network.LoadPlayerData();

            InGameLogger.Log("▶ タイトルBGM再生", LogColor.Green);
            audio.PlayBgm("TitleTheme");
        }

        /// <summary>
        /// クイックスタート（最低限の初期化のみ）
        /// </summary>
        public void QuickStart()
        {
            InGameLogger.Log("▶ グラフィックス初期化", LogColor.Green);
            graphics.Initialize();

            InGameLogger.Log("▶ 入力システム初期化", LogColor.Green);
            input.Initialize();
        }
    }
}
