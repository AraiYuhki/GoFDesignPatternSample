using UnityEngine;
using UnityEngine.UI;

namespace DesignPatterns.Behavioral.Observer {
    /// <summary>
    /// Observerパターンのデモシーンを制御するクラス
    ///
    /// 【デモの内容】
    /// - HP変更時に複数のオブザーバー（HPバー、数値、警告）に一斉通知
    /// - ダメージ/回復でHP変更 → 全オブザーバーが自動更新
    /// - オブザーバーの動的な追加・削除
    /// </summary>
    public sealed class ObserverDemo : PatternDemoBase {
        /// <summary>ダメージを与えるボタン</summary>
        [SerializeField]
        private Button damageButton;

        /// <summary>回復ボタン</summary>
        [SerializeField]
        private Button healButton;

        /// <summary>大ダメージボタン</summary>
        [SerializeField]
        private Button bigDamageButton;

        /// <summary>HPシステム</summary>
        private HealthSystem healthSystem;

        /// <summary>ダメージ量</summary>
        private const int DamageAmount = 15;

        /// <summary>回復量</summary>
        private const int HealAmount = 20;

        /// <summary>大ダメージ量</summary>
        private const int BigDamageAmount = 50;

        /// <summary>最大HP</summary>
        private const int MaxHpValue = 100;

        /// <inheritdoc/>
        protected override string PatternName {
            get { return "Observer"; }
        }

        /// <inheritdoc/>
        protected override PatternCategory Category {
            get { return PatternCategory.Behavioral; }
        }

        /// <inheritdoc/>
        protected override string Description {
            get { return "1対多の依存関係を定義し、状態変化を全ての依存オブジェクトに自動通知する"; }
        }

        /// <inheritdoc/>
        protected override void OnDemoStart() {
            healthSystem = new HealthSystem(MaxHpValue);

            healthSystem.Subscribe(new HpBarObserver());
            healthSystem.Subscribe(new HpNumberObserver());
            healthSystem.Subscribe(new WarningObserver());

            InGameLogger.Log("3つのオブザーバーを登録: HPバー, 数値, 警告", LogColor.Yellow);

            if (damageButton != null) {
                damageButton.onClick.AddListener(OnDamage);
            }
            if (healButton != null) {
                healButton.onClick.AddListener(OnHeal);
            }
            if (bigDamageButton != null) {
                bigDamageButton.onClick.AddListener(OnBigDamage);
            }

            InGameLogger.Log("HP操作で全オブザーバーが一斉更新されることを確認してください", LogColor.Yellow);
        }

        /// <summary>ダメージを与える</summary>
        private void OnDamage() {
            InGameLogger.Log($"--- {DamageAmount} ダメージ! ---", LogColor.Yellow);
            healthSystem.TakeDamage(DamageAmount);
        }

        /// <summary>回復する</summary>
        private void OnHeal() {
            InGameLogger.Log($"--- {HealAmount} 回復! ---", LogColor.Yellow);
            healthSystem.Heal(HealAmount);
        }

        /// <summary>大ダメージを与える</summary>
        private void OnBigDamage() {
            InGameLogger.Log($"--- {BigDamageAmount} 大ダメージ!! ---", LogColor.Yellow);
            healthSystem.TakeDamage(BigDamageAmount);
        }
    }
}
