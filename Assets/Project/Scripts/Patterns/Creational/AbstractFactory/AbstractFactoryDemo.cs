namespace GoFPatterns.Patterns {
    // ---- Abstract Products ----

    /// <summary>
    /// ボタン製品の抽象インターフェース
    /// 各テーマのファクトリが生成する具象ボタンの共通契約
    /// </summary>
    public interface IButton {
        /// <summary>ボタンのラベルを取得する</summary>
        string Label { get; }
        /// <summary>ボタンのスタイル名を取得する</summary>
        string Style { get; }
        /// <summary>
        /// ボタンの描画結果を返す
        /// </summary>
        /// <returns>描画結果の説明文</returns>
        string Render();
    }

    /// <summary>
    /// ダイアログ製品の抽象インターフェース
    /// 各テーマのファクトリが生成する具象ダイアログの共通契約
    /// </summary>
    public interface IDialog {
        /// <summary>ダイアログのタイトルを取得する</summary>
        string Title { get; }
        /// <summary>ダイアログのスタイル名を取得する</summary>
        string Style { get; }
        /// <summary>
        /// ダイアログの描画結果を返す
        /// </summary>
        /// <returns>描画結果の説明文</returns>
        string Render();
    }

    // ---- Concrete Products (Dark) ----

    /// <summary>ダークテーマのボタン</summary>
    public class DarkButton : IButton {
        /// <summary>ボタンのラベル</summary>
        public string Label => "Dark Button";
        /// <summary>スタイル名</summary>
        public string Style => "Dark";
        /// <summary>
        /// ダークテーマでボタンを描画する
        /// </summary>
        /// <returns>描画結果の説明文</returns>
        public string Render() => "[Dark Button] 暗い背景に白い文字のボタン";
    }

    /// <summary>ダークテーマのダイアログ</summary>
    public class DarkDialog : IDialog {
        /// <summary>ダイアログのタイトル</summary>
        public string Title => "Dark Dialog";
        /// <summary>スタイル名</summary>
        public string Style => "Dark";
        /// <summary>
        /// ダークテーマでダイアログを描画する
        /// </summary>
        /// <returns>描画結果の説明文</returns>
        public string Render() => "[Dark Dialog] 暗い枠線とシャドウのダイアログ";
    }

    // ---- Concrete Products (Light) ----

    /// <summary>ライトテーマのボタン</summary>
    public class LightButton : IButton {
        /// <summary>ボタンのラベル</summary>
        public string Label => "Light Button";
        /// <summary>スタイル名</summary>
        public string Style => "Light";
        /// <summary>
        /// ライトテーマでボタンを描画する
        /// </summary>
        /// <returns>描画結果の説明文</returns>
        public string Render() => "[Light Button] 明るい背景に黒い文字のボタン";
    }

    /// <summary>ライトテーマのダイアログ</summary>
    public class LightDialog : IDialog {
        /// <summary>ダイアログのタイトル</summary>
        public string Title => "Light Dialog";
        /// <summary>スタイル名</summary>
        public string Style => "Light";
        /// <summary>
        /// ライトテーマでダイアログを描画する
        /// </summary>
        /// <returns>描画結果の説明文</returns>
        public string Render() => "[Light Dialog] 明るい枠線とフラットなダイアログ";
    }

    // ---- Abstract Factory ----

    /// <summary>
    /// UIファクトリの抽象インターフェース
    /// テーマごとに一貫したUI部品群を生成する契約を定義する
    /// </summary>
    public interface IUIFactory {
        /// <summary>ファクトリの名前を取得する</summary>
        string FactoryName { get; }
        /// <summary>
        /// ボタンを生成する
        /// </summary>
        /// <returns>テーマに合ったボタン</returns>
        IButton CreateButton();
        /// <summary>
        /// ダイアログを生成する
        /// </summary>
        /// <returns>テーマに合ったダイアログ</returns>
        IDialog CreateDialog();
    }

    // ---- Concrete Factories ----

    /// <summary>ダークテーマのUIファクトリ — DarkButtonとDarkDialogを生成する</summary>
    public class DarkUIFactory : IUIFactory {
        /// <summary>ファクトリの名前</summary>
        public string FactoryName => "DarkUIFactory";
        /// <summary>
        /// ダークテーマのボタンを生成する
        /// </summary>
        /// <returns>DarkButtonインスタンス</returns>
        public IButton CreateButton() => new DarkButton();
        /// <summary>
        /// ダークテーマのダイアログを生成する
        /// </summary>
        /// <returns>DarkDialogインスタンス</returns>
        public IDialog CreateDialog() => new DarkDialog();
    }

    /// <summary>ライトテーマのUIファクトリ — LightButtonとLightDialogを生成する</summary>
    public class LightUIFactory : IUIFactory {
        /// <summary>ファクトリの名前</summary>
        public string FactoryName => "LightUIFactory";
        /// <summary>
        /// ライトテーマのボタンを生成する
        /// </summary>
        /// <returns>LightButtonインスタンス</returns>
        public IButton CreateButton() => new LightButton();
        /// <summary>
        /// ライトテーマのダイアログを生成する
        /// </summary>
        /// <returns>LightDialogインスタンス</returns>
        public IDialog CreateDialog() => new LightDialog();
    }

    // ---- Demo ----

    /// <summary>
    /// Abstract Factoryパターンのデモ
    /// IUIFactoryを例にファクトリを切り替えるだけで一貫したテーマのUI部品が生成される仕組みを示す
    /// </summary>
    [PatternDemo("abstract-factory")]
    public class AbstractFactoryDemo : BasePatternDemo {
        /// <summary>デモのパターンID</summary>
        public override string PatternId => "abstract-factory";

        /// <summary>デモの表示名</summary>
        public override string DisplayName => "Abstract Factory";

        /// <summary>現在使用中のファクトリ</summary>
        private IUIFactory currentFactory;

        /// <summary>ダークファクトリで生成されたボタン</summary>
        private IButton darkButton;

        /// <summary>ダークファクトリで生成されたダイアログ</summary>
        private IDialog darkDialog;

        /// <summary>ライトファクトリで生成されたボタン</summary>
        private IButton lightButton;

        /// <summary>ライトファクトリで生成されたダイアログ</summary>
        private IDialog lightDialog;

        /// <summary>
        /// Abstract Factoryパターンのシナリオを構築する
        /// </summary>
        /// <param name="scenario">ステップを追加するシナリオ</param>
        protected override void BuildScenario(DemoScenario scenario) {
            scenario.AddStep(new DemoStep(
                "DarkUIFactoryを生成してダークテーマの部品作成を準備する",
                () => {
                    currentFactory = new DarkUIFactory();
                    Log("Client", "new DarkUIFactory()", $"ファクトリ生成: {currentFactory.FactoryName}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "DarkUIFactoryからボタンを生成する",
                () => {
                    darkButton = currentFactory.CreateButton();
                    Log(currentFactory.FactoryName, "CreateButton()", darkButton.Render());
                }
            ));

            scenario.AddStep(new DemoStep(
                "DarkUIFactoryからダイアログを生成する",
                () => {
                    darkDialog = currentFactory.CreateDialog();
                    Log(currentFactory.FactoryName, "CreateDialog()", darkDialog.Render());
                }
            ));

            scenario.AddStep(new DemoStep(
                "LightUIFactoryに切り替える（クライアントコードは変更不要）",
                () => {
                    currentFactory = new LightUIFactory();
                    Log("Client", "new LightUIFactory()", $"ファクトリ切り替え: {currentFactory.FactoryName}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "LightUIFactoryからボタンを生成する（同じCreateButton呼び出し）",
                () => {
                    lightButton = currentFactory.CreateButton();
                    Log(currentFactory.FactoryName, "CreateButton()", lightButton.Render());
                }
            ));

            scenario.AddStep(new DemoStep(
                "LightUIFactoryからダイアログを生成する（同じCreateDialog呼び出し）",
                () => {
                    lightDialog = currentFactory.CreateDialog();
                    Log(currentFactory.FactoryName, "CreateDialog()", lightDialog.Render());
                }
            ));

            scenario.AddStep(new DemoStep(
                "各ファクトリが一貫したテーマの部品を生成していることを検証する",
                () => {
                    bool darkConsistent = darkButton.Style == darkDialog.Style;
                    bool lightConsistent = lightButton.Style == lightDialog.Style;
                    bool themesDiffer = darkButton.Style != lightButton.Style;
                    Log("検証", "製品の一貫性チェック",
                        $"Dark一貫性={darkConsistent}, Light一貫性={lightConsistent}, テーマ差異={themesDiffer}");
                }
            ));
        }
    }
}
