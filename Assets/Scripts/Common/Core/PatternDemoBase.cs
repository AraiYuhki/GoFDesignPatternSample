using UnityEngine;

namespace DesignPatterns {
    /// <summary>
    /// 各デザインパターンのデモシーンで使用する基底クラス
    /// パターン情報の表示とログの初期化を共通化する
    /// </summary>
    public abstract class PatternDemoBase : MonoBehaviour {
        /// <summary>パターン情報を表示するパネル</summary>
        [SerializeField]
        private PatternInfoPanel infoPanel;

        /// <summary>パターン名を返す</summary>
        protected abstract string PatternName { get; }

        /// <summary>パターンのカテゴリを返す</summary>
        protected abstract PatternCategory Category { get; }

        /// <summary>パターンの概要説明を返す</summary>
        protected abstract string Description { get; }

        /// <summary>
        /// カテゴリに対応するログの色を返す
        /// </summary>
        protected LogColor CategoryColor {
            get {
                switch (Category) {
                    case PatternCategory.Creational:
                        return LogColor.Blue;
                    case PatternCategory.Structural:
                        return LogColor.Green;
                    case PatternCategory.Behavioral:
                        return LogColor.Orange;
                    default:
                        return LogColor.White;
                }
            }
        }

        protected virtual void Start() {
            InitializeDemo();
        }

        /// <summary>
        /// デモの初期化処理を行う
        /// パターン情報パネルの設定とウェルカムログを表示する
        /// </summary>
        private void InitializeDemo() {
            if (infoPanel != null) {
                infoPanel.SetInfo(PatternName, GetCategoryDisplayName(), Description);
            }

            InGameLogger.Clear();
            InGameLogger.Log($"=== {PatternName} パターン ===", CategoryColor);
            InGameLogger.Log(Description, LogColor.White);
            InGameLogger.Log("---", LogColor.White);

            OnDemoStart();
        }

        /// <summary>
        /// デモ固有の初期化処理
        /// サブクラスでオーバーライドして使用する
        /// </summary>
        protected virtual void OnDemoStart() {
        }

        /// <summary>
        /// カテゴリの表示名を返す
        /// </summary>
        /// <returns>カテゴリの日本語表示名</returns>
        private string GetCategoryDisplayName() {
            switch (Category) {
                case PatternCategory.Creational:
                    return "生成パターン (Creational)";
                case PatternCategory.Structural:
                    return "構造パターン (Structural)";
                case PatternCategory.Behavioral:
                    return "振る舞いパターン (Behavioral)";
                default:
                    return "不明";
            }
        }
    }
}
