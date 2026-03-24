namespace GoFPatterns.Patterns {
    // ---- Subsystems ----

    /// <summary>
    /// オーディオ再生を担当するサブシステム
    /// BGMやSEの再生・停止を管理する
    /// </summary>
    public class AudioSystem {
        /// <summary>最後に実行された操作の説明</summary>
        private string lastAction;

        /// <summary>最後に実行された操作を取得する</summary>
        public string LastAction => lastAction;

        /// <summary>
        /// BGMを再生する
        /// </summary>
        /// <param name="trackName">再生するトラック名</param>
        public void Play(string trackName) {
            lastAction = $"BGM \"{trackName}\" を再生";
        }

        /// <summary>
        /// オーディオを停止する
        /// </summary>
        public void Stop() {
            lastAction = "全オーディオを停止";
        }
    }

    /// <summary>
    /// グラフィックス描画を担当するサブシステム
    /// シーンのロードと描画を管理する
    /// </summary>
    public class GraphicsSystem {
        /// <summary>最後に実行された操作の説明</summary>
        private string lastAction;

        /// <summary>最後に実行された操作を取得する</summary>
        public string LastAction => lastAction;

        /// <summary>
        /// シーンをロードする
        /// </summary>
        /// <param name="sceneName">ロードするシーン名</param>
        public void LoadScene(string sceneName) {
            lastAction = $"シーン \"{sceneName}\" をロード";
        }

        /// <summary>
        /// 画面をクリアする
        /// </summary>
        public void ClearScreen() {
            lastAction = "画面をクリア";
        }
    }

    /// <summary>
    /// 入力制御を担当するサブシステム
    /// コントローラーやキーボード入力の有効化・無効化を管理する
    /// </summary>
    public class InputSystem {
        /// <summary>最後に実行された操作の説明</summary>
        private string lastAction;

        /// <summary>最後に実行された操作を取得する</summary>
        public string LastAction => lastAction;

        /// <summary>
        /// 入力制御を有効化する
        /// </summary>
        public void EnableControls() {
            lastAction = "入力制御を有効化";
        }

        /// <summary>
        /// 入力制御を無効化する
        /// </summary>
        public void DisableControls() {
            lastAction = "入力制御を無効化";
        }
    }

    /// <summary>
    /// セーブデータを担当するサブシステム
    /// 進行状況の保存と読み込みを管理する
    /// </summary>
    public class SaveSystem {
        /// <summary>最後に実行された操作の説明</summary>
        private string lastAction;

        /// <summary>最後に実行された操作を取得する</summary>
        public string LastAction => lastAction;

        /// <summary>
        /// 進行状況をロードする
        /// </summary>
        public void LoadProgress() {
            lastAction = "セーブデータをロード";
        }

        /// <summary>
        /// 進行状況を保存する
        /// </summary>
        public void SaveProgress() {
            lastAction = "セーブデータを保存";
        }
    }

    // ---- Facade ----

    /// <summary>
    /// Facadeパターンのファサードクラス
    /// 複数のサブシステムを統合し、簡潔なインターフェースを提供する
    /// </summary>
    public class GameFacade {
        /// <summary>オーディオサブシステム</summary>
        private readonly AudioSystem audio;

        /// <summary>グラフィックスサブシステム</summary>
        private readonly GraphicsSystem graphics;

        /// <summary>入力サブシステム</summary>
        private readonly InputSystem input;

        /// <summary>セーブサブシステム</summary>
        private readonly SaveSystem save;

        /// <summary>オーディオサブシステムを取得する</summary>
        public AudioSystem Audio => audio;

        /// <summary>グラフィックスサブシステムを取得する</summary>
        public GraphicsSystem Graphics => graphics;

        /// <summary>入力サブシステムを取得する</summary>
        public InputSystem Input => input;

        /// <summary>セーブサブシステムを取得する</summary>
        public SaveSystem Save => save;

        /// <summary>
        /// GameFacadeを生成する
        /// </summary>
        /// <param name="audio">オーディオサブシステム</param>
        /// <param name="graphics">グラフィックスサブシステム</param>
        /// <param name="input">入力サブシステム</param>
        /// <param name="save">セーブサブシステム</param>
        public GameFacade(AudioSystem audio, GraphicsSystem graphics, InputSystem input, SaveSystem save) {
            this.audio = audio;
            this.graphics = graphics;
            this.input = input;
            this.save = save;
        }

        /// <summary>
        /// ゲームを開始する（全サブシステムを連携起動）
        /// </summary>
        public void StartGame() {
            save.LoadProgress();
            graphics.LoadScene("MainWorld");
            audio.Play("MainTheme");
            input.EnableControls();
        }

        /// <summary>
        /// セーブして終了する（全サブシステムを連携停止）
        /// </summary>
        public void SaveAndQuit() {
            input.DisableControls();
            save.SaveProgress();
            audio.Stop();
            graphics.ClearScreen();
        }
    }

    // ---- Demo ----

    /// <summary>
    /// Facadeパターンのデモ
    /// 複数のサブシステムをファサードで統合し、簡潔な操作で連携動作させる流れを示す
    /// </summary>
    [PatternDemo("facade")]
    public class FacadeDemo : BasePatternDemo {
        /// <summary>デモのパターンID</summary>
        public override string PatternId => "facade";

        /// <summary>デモの表示名</summary>
        public override string DisplayName => "Facade";

        /// <summary>オーディオサブシステム</summary>
        private AudioSystem audio;

        /// <summary>グラフィックスサブシステム</summary>
        private GraphicsSystem graphics;

        /// <summary>入力サブシステム</summary>
        private InputSystem input;

        /// <summary>セーブサブシステム</summary>
        private SaveSystem save;

        /// <summary>ファサード</summary>
        private GameFacade facade;

        /// <summary>
        /// リセット時にドメインオブジェクトをクリアする
        /// </summary>
        protected override void OnReset() {
            audio = null;
            graphics = null;
            input = null;
            save = null;
            facade = null;
        }

        /// <summary>
        /// Facadeパターンのシナリオを構築する
        /// </summary>
        /// <param name="scenario">ステップを追加するシナリオ</param>
        protected override void BuildScenario(DemoScenario scenario) {
            audio = new AudioSystem();
            graphics = new GraphicsSystem();
            input = new InputSystem();
            save = new SaveSystem();

            scenario.AddStep(new DemoStep(
                "各サブシステムを個別に操作する（AudioSystem）",
                () => {
                    audio.Play("BattleBGM");
                    Log("AudioSystem", "Play(\"BattleBGM\")", audio.LastAction);
                }
            ));

            scenario.AddStep(new DemoStep(
                "各サブシステムを個別に操作する（GraphicsSystem）",
                () => {
                    graphics.LoadScene("Dungeon");
                    Log("GraphicsSystem", "LoadScene(\"Dungeon\")", graphics.LastAction);
                }
            ));

            scenario.AddStep(new DemoStep(
                "各サブシステムを個別に操作する（InputSystem, SaveSystem）",
                () => {
                    input.EnableControls();
                    save.LoadProgress();
                    Log("InputSystem", "EnableControls()", input.LastAction);
                    Log("SaveSystem", "LoadProgress()", save.LastAction);
                }
            ));

            scenario.AddStep(new DemoStep(
                "GameFacadeを作成して全サブシステムを統合する",
                () => {
                    facade = new GameFacade(audio, graphics, input, save);
                    Log("Client", "new GameFacade(...)", "4つのサブシステムを統合");
                }
            ));

            scenario.AddStep(new DemoStep(
                "Facade.StartGame()で全サブシステムを一括起動する",
                () => {
                    facade.StartGame();
                    Log("GameFacade", "StartGame()", "全サブシステムを連携起動");
                    Log("→ SaveSystem", save.LastAction, "");
                    Log("→ GraphicsSystem", graphics.LastAction, "");
                    Log("→ AudioSystem", audio.LastAction, "");
                    Log("→ InputSystem", input.LastAction, "");
                }
            ));

            scenario.AddStep(new DemoStep(
                "Facade.SaveAndQuit()で全サブシステムを一括停止する",
                () => {
                    facade.SaveAndQuit();
                    Log("GameFacade", "SaveAndQuit()", "全サブシステムを連携停止");
                    Log("→ InputSystem", input.LastAction, "");
                    Log("→ SaveSystem", save.LastAction, "");
                    Log("→ AudioSystem", audio.LastAction, "");
                    Log("→ GraphicsSystem", graphics.LastAction, "");
                }
            ));
        }
    }
}
