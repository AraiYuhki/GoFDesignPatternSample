using UnityEngine;
using UnityEngine.UI;

namespace DesignPatterns.Structural.Decorator {
    /// <summary>
    /// Decoratorパターンのデモシーンを制御するクラス
    ///
    /// 【デモの内容】
    /// - 基本武器にエンチャントを重ねがけ
    /// - デコレーターを追加するごとに攻撃力が変化することを確認
    /// - 複数のデコレーターの積み重ねを体験
    /// </summary>
    public sealed class DecoratorDemo : PatternDemoBase {
        /// <summary>剣作成ボタン</summary>
        [SerializeField]
        private Button createSwordButton;

        /// <summary>弓作成ボタン</summary>
        [SerializeField]
        private Button createBowButton;

        /// <summary>炎エンチャントボタン</summary>
        [SerializeField]
        private Button addFireButton;

        /// <summary>氷エンチャントボタン</summary>
        [SerializeField]
        private Button addIceButton;

        /// <summary>毒エンチャントボタン</summary>
        [SerializeField]
        private Button addPoisonButton;

        /// <summary>現在の武器</summary>
        private IWeapon currentWeapon;

        /// <inheritdoc/>
        protected override string PatternName {
            get { return "Decorator"; }
        }

        /// <inheritdoc/>
        protected override PatternCategory Category {
            get { return PatternCategory.Structural; }
        }

        /// <inheritdoc/>
        protected override string Description {
            get { return "オブジェクトに動的に機能を追加する。サブクラス化の代わりに柔軟な拡張を実現する"; }
        }

        /// <inheritdoc/>
        protected override void OnDemoStart() {
            if (createSwordButton != null) {
                createSwordButton.onClick.AddListener(OnCreateSword);
            }
            if (createBowButton != null) {
                createBowButton.onClick.AddListener(OnCreateBow);
            }
            if (addFireButton != null) {
                addFireButton.onClick.AddListener(OnAddFire);
            }
            if (addIceButton != null) {
                addIceButton.onClick.AddListener(OnAddIce);
            }
            if (addPoisonButton != null) {
                addPoisonButton.onClick.AddListener(OnAddPoison);
            }

            InGameLogger.Log("武器を作成し、エンチャントを重ねがけしてください", LogColor.Yellow);
        }

        /// <summary>剣を作成する</summary>
        private void OnCreateSword() {
            currentWeapon = new Sword();
            InGameLogger.Log("--- 剣を作成 ---", LogColor.Yellow);
            InGameLogger.Log(currentWeapon.GetDescription(), LogColor.Green);
        }

        /// <summary>弓を作成する</summary>
        private void OnCreateBow() {
            currentWeapon = new Bow();
            InGameLogger.Log("--- 弓を作成 ---", LogColor.Yellow);
            InGameLogger.Log(currentWeapon.GetDescription(), LogColor.Green);
        }

        /// <summary>炎エンチャントを追加する</summary>
        private void OnAddFire() {
            if (!HasWeapon()) { return; }
            currentWeapon = new FireEnchantment(currentWeapon);
            InGameLogger.Log("🔥 炎エンチャント追加!", LogColor.Yellow);
            InGameLogger.Log(currentWeapon.GetDescription(), LogColor.Green);
        }

        /// <summary>氷エンチャントを追加する</summary>
        private void OnAddIce() {
            if (!HasWeapon()) { return; }
            currentWeapon = new IceEnchantment(currentWeapon);
            InGameLogger.Log("❄️ 氷エンチャント追加!", LogColor.Yellow);
            InGameLogger.Log(currentWeapon.GetDescription(), LogColor.Green);
        }

        /// <summary>毒エンチャントを追加する</summary>
        private void OnAddPoison() {
            if (!HasWeapon()) { return; }
            currentWeapon = new PoisonEnchantment(currentWeapon);
            InGameLogger.Log("☠️ 毒エンチャント追加!", LogColor.Yellow);
            InGameLogger.Log(currentWeapon.GetDescription(), LogColor.Green);
        }

        /// <summary>
        /// 武器が存在するかチェックする
        /// </summary>
        /// <returns>武器が存在すればtrue</returns>
        private bool HasWeapon() {
            if (currentWeapon == null) {
                InGameLogger.Log("まず武器を作成してください", LogColor.Red);
                return false;
            }
            return true;
        }
    }
}
