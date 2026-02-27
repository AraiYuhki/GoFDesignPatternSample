using UnityEngine;
using UnityEngine.UI;

namespace DesignPatterns.Behavioral.TemplateMethod {
    /// <summary>
    /// Template Methodパターンのデモシーンを制御するクラス
    ///
    /// 【デモの内容】
    /// - 戦士・魔法使い・回復役のターンを実行
    /// - テンプレートメソッドにより共通のターン構造（開始→行動→終了）を維持しつつ、
    ///   行動フェーズの内容をサブクラスごとに変更する様子を確認できる
    /// </summary>
    public sealed class TemplateMethodDemo : PatternDemoBase {
        /// <summary>戦士のターンを実行するボタン</summary>
        [SerializeField]
        private Button warriorTurnButton;

        /// <summary>魔法使いのターンを実行するボタン</summary>
        [SerializeField]
        private Button mageTurnButton;

        /// <summary>回復役のターンを実行するボタン</summary>
        [SerializeField]
        private Button healerTurnButton;

        /// <summary>戦士のターン処理</summary>
        private WarriorTurn warriorTurn;

        /// <summary>魔法使いのターン処理</summary>
        private MageTurn mageTurn;

        /// <summary>回復役のターン処理</summary>
        private HealerTurn healerTurn;

        /// <inheritdoc/>
        protected override string PatternName {
            get { return "Template Method"; }
        }

        /// <inheritdoc/>
        protected override PatternCategory Category {
            get { return PatternCategory.Behavioral; }
        }

        /// <inheritdoc/>
        protected override string Description {
            get { return "処理の骨格を基底クラスで定義し、具体的な処理をサブクラスに委ねる"; }
        }

        /// <inheritdoc/>
        protected override void OnDemoStart() {
            warriorTurn = new WarriorTurn();
            mageTurn = new MageTurn();
            healerTurn = new HealerTurn();

            if (warriorTurnButton != null) {
                warriorTurnButton.onClick.AddListener(OnWarriorTurn);
            }
            if (mageTurnButton != null) {
                mageTurnButton.onClick.AddListener(OnMageTurn);
            }
            if (healerTurnButton != null) {
                healerTurnButton.onClick.AddListener(OnHealerTurn);
            }

            InGameLogger.Log("各キャラクターのターンボタンを押して、テンプレートメソッドの動作を確認してください", LogColor.Yellow);
        }

        /// <summary>戦士のターンを実行する</summary>
        private void OnWarriorTurn() {
            InGameLogger.Log("--- 戦士のターン ---", LogColor.Yellow);
            warriorTurn.ExecuteTurn();
        }

        /// <summary>魔法使いのターンを実行する</summary>
        private void OnMageTurn() {
            InGameLogger.Log("--- 魔法使いのターン ---", LogColor.Yellow);
            mageTurn.ExecuteTurn();
        }

        /// <summary>回復役のターンを実行する</summary>
        private void OnHealerTurn() {
            InGameLogger.Log("--- 回復役のターン ---", LogColor.Yellow);
            healerTurn.ExecuteTurn();
        }
    }
}
