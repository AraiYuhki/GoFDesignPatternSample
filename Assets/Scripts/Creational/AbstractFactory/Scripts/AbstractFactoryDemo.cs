using UnityEngine;
using UnityEngine.UI;

namespace DesignPatterns.Creational.AbstractFactory
{
    /// <summary>
    /// Abstract Factoryパターンのデモシーンを制御するクラス
    ///
    /// 【デモの内容】
    /// - ダーク/ライト/レトロの3テーマを切り替え
    /// - ファクトリを切り替えるだけで関連するUI部品が一括で変更されることを確認
    /// - 各ファクトリが一貫性のある色のセットを生成することを体験
    /// </summary>
    public sealed class AbstractFactoryDemo : PatternDemoBase
    {
        /// <summary>テーマが適用されるサンプルパネル</summary>
        [SerializeField]
        private Image samplePanel;

        /// <summary>テーマが適用されるサンプルボタン群</summary>
        [SerializeField]
        private Image[] sampleButtons;

        /// <summary>ダークテーマ切り替えボタン</summary>
        [SerializeField]
        private Button darkThemeButton;

        /// <summary>ライトテーマ切り替えボタン</summary>
        [SerializeField]
        private Button lightThemeButton;

        /// <summary>レトロテーマ切り替えボタン</summary>
        [SerializeField]
        private Button retroThemeButton;

        /// <inheritdoc/>
        protected override string PatternName
        {
            get { return "Abstract Factory"; }
        }

        /// <inheritdoc/>
        protected override PatternCategory Category
        {
            get { return PatternCategory.Creational; }
        }

        /// <inheritdoc/>
        protected override string Description
        {
            get { return "関連するオブジェクト群を、具象クラスを指定せずに生成するインターフェースを提供する"; }
        }

        /// <inheritdoc/>
        protected override void OnDemoStart()
        {
            if (darkThemeButton != null)
            {
                darkThemeButton.onClick.AddListener(() => ApplyTheme(new DarkThemeFactory()));
            }
            if (lightThemeButton != null)
            {
                lightThemeButton.onClick.AddListener(() => ApplyTheme(new LightThemeFactory()));
            }
            if (retroThemeButton != null)
            {
                retroThemeButton.onClick.AddListener(() => ApplyTheme(new RetroThemeFactory()));
            }

            InGameLogger.Log("テーマを選択してUI部品の一括変更を確認してください", LogColor.Yellow);
        }

        /// <summary>
        /// 指定されたテーマファクトリを使ってUIを更新する
        /// </summary>
        /// <param name="factory">適用するテーマファクトリ</param>
        private void ApplyTheme(IThemeFactory factory)
        {
            InGameLogger.Log($"--- {factory.ThemeName} を適用 ---", LogColor.Yellow);

            Color backgroundColor = factory.CreateBackgroundColor();
            Color buttonColor = factory.CreateButtonColor();
            Color textColor = factory.CreateTextColor();
            Color accentColor = factory.CreateAccentColor();

            if (samplePanel != null)
            {
                samplePanel.color = backgroundColor;
                InGameLogger.Log($"背景色を設定: {ColorToHex(backgroundColor)}", LogColor.Blue);
            }

            if (sampleButtons != null)
            {
                for (int i = 0; i < sampleButtons.Length; i++)
                {
                    if (sampleButtons[i] != null)
                    {
                        sampleButtons[i].color = buttonColor;
                    }
                }
                InGameLogger.Log($"ボタン色を設定: {ColorToHex(buttonColor)}", LogColor.Blue);
            }

            InGameLogger.Log($"テキスト色: {ColorToHex(textColor)}", LogColor.Blue);
            InGameLogger.Log($"アクセント色: {ColorToHex(accentColor)}", LogColor.Blue);
            InGameLogger.Log($"{factory.ThemeName} の適用が完了しました", LogColor.Blue);
        }

        /// <summary>
        /// Colorを16進数文字列に変換する
        /// </summary>
        /// <param name="color">変換する色</param>
        /// <returns>16進数カラーコード</returns>
        private static string ColorToHex(Color color)
        {
            return $"#{ColorUtility.ToHtmlStringRGB(color)}";
        }
    }
}
