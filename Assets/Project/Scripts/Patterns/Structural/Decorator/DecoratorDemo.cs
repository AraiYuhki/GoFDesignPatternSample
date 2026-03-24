namespace GoFPatterns.Patterns {
    // ---- Component interface ----

    /// <summary>
    /// Decoratorパターンのコンポーネントインターフェース
    /// 武器のダメージと説明を統一的に取得する
    /// </summary>
    public interface IWeapon {
        /// <summary>
        /// 武器のダメージ値を取得する
        /// </summary>
        /// <returns>ダメージ値</returns>
        int GetDamage();

        /// <summary>
        /// 武器の説明を取得する
        /// </summary>
        /// <returns>説明文</returns>
        string GetDescription();
    }

    // ---- ConcreteComponent ----

    /// <summary>
    /// 基本武器であるベーシックソード
    /// デコレーターで装飾される前の素の武器
    /// </summary>
    public class BasicSword : IWeapon {
        /// <summary>基本ダメージ値</summary>
        private const int BaseDamage = 10;

        /// <summary>
        /// 基本ダメージ値を取得する
        /// </summary>
        /// <returns>基本ダメージ値</returns>
        public int GetDamage() {
            return BaseDamage;
        }

        /// <summary>
        /// 武器の説明を取得する
        /// </summary>
        /// <returns>説明文</returns>
        public string GetDescription() {
            return "BasicSword";
        }
    }

    // ---- Decorators ----

    /// <summary>
    /// 炎のエンチャントを付与するデコレーター
    /// ラップした武器にボーナスダメージを追加する
    /// </summary>
    public class FireEnchantment : IWeapon {
        /// <summary>炎エンチャントの追加ダメージ</summary>
        private const int FireBonusDamage = 5;

        /// <summary>ラップ対象の武器</summary>
        private readonly IWeapon wrappedWeapon;

        /// <summary>
        /// FireEnchantmentを生成する
        /// </summary>
        /// <param name="weapon">ラップする武器</param>
        public FireEnchantment(IWeapon weapon) {
            wrappedWeapon = weapon;
        }

        /// <summary>
        /// 元のダメージに炎ボーナスを加算して返す
        /// </summary>
        /// <returns>合計ダメージ値</returns>
        public int GetDamage() {
            return wrappedWeapon.GetDamage() + FireBonusDamage;
        }

        /// <summary>
        /// 元の説明に炎エンチャントを追加して返す
        /// </summary>
        /// <returns>装飾された説明文</returns>
        public string GetDescription() {
            return $"{wrappedWeapon.GetDescription()} + Fire";
        }
    }

    /// <summary>
    /// 毒のエンチャントを付与するデコレーター
    /// ラップした武器にボーナスダメージを追加する
    /// </summary>
    public class PoisonEnchantment : IWeapon {
        /// <summary>毒エンチャントの追加ダメージ</summary>
        private const int PoisonBonusDamage = 3;

        /// <summary>ラップ対象の武器</summary>
        private readonly IWeapon wrappedWeapon;

        /// <summary>
        /// PoisonEnchantmentを生成する
        /// </summary>
        /// <param name="weapon">ラップする武器</param>
        public PoisonEnchantment(IWeapon weapon) {
            wrappedWeapon = weapon;
        }

        /// <summary>
        /// 元のダメージに毒ボーナスを加算して返す
        /// </summary>
        /// <returns>合計ダメージ値</returns>
        public int GetDamage() {
            return wrappedWeapon.GetDamage() + PoisonBonusDamage;
        }

        /// <summary>
        /// 元の説明に毒エンチャントを追加して返す
        /// </summary>
        /// <returns>装飾された説明文</returns>
        public string GetDescription() {
            return $"{wrappedWeapon.GetDescription()} + Poison";
        }
    }

    // ---- Demo ----

    /// <summary>
    /// Decoratorパターンのデモ
    /// 武器にエンチャントを重ねがけしてダメージと説明が積み上がる仕組みを示す
    /// </summary>
    [PatternDemo("decorator")]
    public class DecoratorDemo : BasePatternDemo {
        /// <summary>デモのパターンID</summary>
        public override string PatternId => "decorator";

        /// <summary>デモの表示名</summary>
        public override string DisplayName => "Decorator";

        /// <summary>現在の武器（デコレーターで順次ラップされる）</summary>
        private IWeapon weapon;

        /// <summary>
        /// リセット時にドメインオブジェクトをクリアする
        /// </summary>
        protected override void OnReset() {
            weapon = null;
        }

        /// <summary>
        /// Decoratorパターンのシナリオを構築する
        /// </summary>
        /// <param name="scenario">ステップを追加するシナリオ</param>
        protected override void BuildScenario(DemoScenario scenario) {
            scenario.AddStep(new DemoStep(
                "BasicSwordを作成する",
                () => {
                    weapon = new BasicSword();
                    Log("Client", "new BasicSword()", $"説明: {weapon.GetDescription()}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "BasicSwordの基本ダメージを確認する",
                () => {
                    Log("BasicSword", "GetDamage()", $"ダメージ: {weapon.GetDamage()}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "FireEnchantmentを追加する",
                () => {
                    weapon = new FireEnchantment(weapon);
                    Log("Client", "new FireEnchantment(weapon)", $"説明: {weapon.GetDescription()}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "FireEnchantment適用後のダメージを確認する",
                () => {
                    Log("FireEnchantment", "GetDamage()", $"ダメージ: {weapon.GetDamage()}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "さらにPoisonEnchantmentを重ねがけする",
                () => {
                    weapon = new PoisonEnchantment(weapon);
                    Log("Client", "new PoisonEnchantment(weapon)", $"説明: {weapon.GetDescription()}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "二重デコレーション後のダメージを確認する",
                () => {
                    Log("PoisonEnchantment", "GetDamage()", $"ダメージ: {weapon.GetDamage()}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "説明チェーンの全体を表示する",
                () => {
                    Log("Client", "GetDescription()", $"最終: {weapon.GetDescription()} (ダメージ: {weapon.GetDamage()})");
                }
            ));
        }
    }
}
