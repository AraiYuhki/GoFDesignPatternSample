using UnityEngine;

namespace DesignPatterns.Creational.AbstractFactory
{
    /// <summary>
    /// UIテーマの抽象ファクトリ（AbstractFactory）
    ///
    /// 【Abstract Factoryパターンにおける役割】
    /// 関連するオブジェクト群を生成するためのインターフェースを定義する
    /// 具象ファクトリがテーマに応じた一貫性のあるUI部品群を生成する
    /// </summary>
    public interface IThemeFactory
    {
        /// <summary>テーマ名</summary>
        string ThemeName { get; }

        /// <summary>
        /// ボタンの色を生成する
        /// </summary>
        /// <returns>ボタンの色</returns>
        Color CreateButtonColor();

        /// <summary>
        /// 背景色を生成する
        /// </summary>
        /// <returns>背景色</returns>
        Color CreateBackgroundColor();

        /// <summary>
        /// テキスト色を生成する
        /// </summary>
        /// <returns>テキスト色</returns>
        Color CreateTextColor();

        /// <summary>
        /// アクセントカラーを生成する
        /// </summary>
        /// <returns>アクセントカラー</returns>
        Color CreateAccentColor();
    }
}
