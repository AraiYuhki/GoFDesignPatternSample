namespace DesignPatterns.Structural.Facade
{
    /// <summary>
    /// オーディオサブシステム
    /// 複雑な内部処理を持つシステムの一つ
    /// </summary>
    public sealed class AudioSubSystem
    {
        /// <summary>
        /// オーディオシステムを初期化する
        /// </summary>
        public void Initialize()
        {
            InGameLogger.Log("  [Audio] ドライバ初期化...", LogColor.White);
            InGameLogger.Log("  [Audio] BGMチャンネル確保...", LogColor.White);
            InGameLogger.Log("  [Audio] SEチャンネル確保...", LogColor.White);
        }

        /// <summary>
        /// BGMを再生する
        /// </summary>
        /// <param name="trackName">トラック名</param>
        public void PlayBgm(string trackName)
        {
            InGameLogger.Log($"  [Audio] BGM再生: {trackName}", LogColor.White);
        }
    }

    /// <summary>
    /// グラフィックスサブシステム
    /// </summary>
    public sealed class GraphicsSubSystem
    {
        /// <summary>
        /// グラフィックスシステムを初期化する
        /// </summary>
        public void Initialize()
        {
            InGameLogger.Log("  [Graphics] レンダラー初期化...", LogColor.White);
            InGameLogger.Log("  [Graphics] シェーダーコンパイル...", LogColor.White);
            InGameLogger.Log("  [Graphics] ポストプロセス設定...", LogColor.White);
        }

        /// <summary>
        /// 画面解像度を設定する
        /// </summary>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        public void SetResolution(int width, int height)
        {
            InGameLogger.Log($"  [Graphics] 解像度設定: {width}x{height}", LogColor.White);
        }
    }

    /// <summary>
    /// 入力サブシステム
    /// </summary>
    public sealed class InputSubSystem
    {
        /// <summary>
        /// 入力システムを初期化する
        /// </summary>
        public void Initialize()
        {
            InGameLogger.Log("  [Input] キーボード検出...", LogColor.White);
            InGameLogger.Log("  [Input] マウス検出...", LogColor.White);
            InGameLogger.Log("  [Input] キーバインド読み込み...", LogColor.White);
        }
    }

    /// <summary>
    /// ネットワークサブシステム
    /// </summary>
    public sealed class NetworkSubSystem
    {
        /// <summary>
        /// ネットワーク接続を確立する
        /// </summary>
        public void Connect()
        {
            InGameLogger.Log("  [Network] サーバー接続中...", LogColor.White);
            InGameLogger.Log("  [Network] 認証完了", LogColor.White);
        }

        /// <summary>
        /// プレイヤーデータを読み込む
        /// </summary>
        public void LoadPlayerData()
        {
            InGameLogger.Log("  [Network] セーブデータ取得中...", LogColor.White);
            InGameLogger.Log("  [Network] セーブデータ取得完了", LogColor.White);
        }
    }
}
