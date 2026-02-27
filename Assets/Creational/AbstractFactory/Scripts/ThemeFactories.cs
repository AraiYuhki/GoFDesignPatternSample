using UnityEngine;

namespace DesignPatterns.Creational.AbstractFactory {
    /// <summary>
    /// ダークテーマのファクトリ（ConcreteFactory）
    /// 暗い色調のUIパーツ群を生成する
    /// </summary>
    public sealed class DarkThemeFactory : IThemeFactory {
        /// <inheritdoc/>
        public string ThemeName { get { return "ダークテーマ"; } }

        /// <inheritdoc/>
        public Color CreateButtonColor() {
            return new Color(0.25f, 0.25f, 0.30f, 1f);
        }

        /// <inheritdoc/>
        public Color CreateBackgroundColor() {
            return new Color(0.12f, 0.12f, 0.15f, 1f);
        }

        /// <inheritdoc/>
        public Color CreateTextColor() {
            return new Color(0.90f, 0.90f, 0.90f, 1f);
        }

        /// <inheritdoc/>
        public Color CreateAccentColor() {
            return new Color(0.35f, 0.61f, 0.84f, 1f);
        }
    }

    /// <summary>
    /// ライトテーマのファクトリ（ConcreteFactory）
    /// 明るい色調のUIパーツ群を生成する
    /// </summary>
    public sealed class LightThemeFactory : IThemeFactory {
        /// <inheritdoc/>
        public string ThemeName { get { return "ライトテーマ"; } }

        /// <inheritdoc/>
        public Color CreateButtonColor() {
            return new Color(0.85f, 0.85f, 0.90f, 1f);
        }

        /// <inheritdoc/>
        public Color CreateBackgroundColor() {
            return new Color(0.95f, 0.95f, 0.97f, 1f);
        }

        /// <inheritdoc/>
        public Color CreateTextColor() {
            return new Color(0.15f, 0.15f, 0.15f, 1f);
        }

        /// <inheritdoc/>
        public Color CreateAccentColor() {
            return new Color(0.20f, 0.47f, 0.84f, 1f);
        }
    }

    /// <summary>
    /// レトロテーマのファクトリ（ConcreteFactory）
    /// レトロゲーム風の色調のUIパーツ群を生成する
    /// </summary>
    public sealed class RetroThemeFactory : IThemeFactory {
        /// <inheritdoc/>
        public string ThemeName { get { return "レトロテーマ"; } }

        /// <inheritdoc/>
        public Color CreateButtonColor() {
            return new Color(0.55f, 0.27f, 0.07f, 1f);
        }

        /// <inheritdoc/>
        public Color CreateBackgroundColor() {
            return new Color(0.18f, 0.15f, 0.10f, 1f);
        }

        /// <inheritdoc/>
        public Color CreateTextColor() {
            return new Color(0.0f, 1.0f, 0.0f, 1f);
        }

        /// <inheritdoc/>
        public Color CreateAccentColor() {
            return new Color(1.0f, 0.84f, 0.0f, 1f);
        }
    }
}
