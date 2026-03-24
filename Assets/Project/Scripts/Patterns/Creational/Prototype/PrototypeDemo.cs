using System.Collections.Generic;

namespace GoFPatterns.Patterns {
    // ---- Prototype Interface ----

    /// <summary>
    /// Prototypeパターンのクローン可能インターフェース
    /// 自分自身の複製を返す契約を定義する
    /// </summary>
    public interface IEnemyPrototype {
        /// <summary>
        /// 自分自身のディープコピーを返す
        /// </summary>
        /// <returns>複製されたインスタンス</returns>
        IEnemyPrototype Clone();
    }

    // ---- Concrete Prototype ----

    /// <summary>
    /// スライムのプロトタイプ実装
    /// HP・攻撃力・色を持ちCloneでディープコピーを生成する
    /// </summary>
    public class SlimePrototype : IEnemyPrototype {
        /// <summary>HP</summary>
        public int Hp { get; set; }
        /// <summary>攻撃力</summary>
        public int Attack { get; set; }
        /// <summary>色の名前</summary>
        public string Color { get; set; }

        /// <summary>
        /// SlimePrototypeを生成する
        /// </summary>
        /// <param name="hp">HP値</param>
        /// <param name="attack">攻撃力</param>
        /// <param name="color">色の名前</param>
        public SlimePrototype(int hp, int attack, string color) {
            Hp = hp;
            Attack = attack;
            Color = color;
        }

        /// <summary>
        /// 自分自身のディープコピーを返す
        /// </summary>
        /// <returns>複製されたSlimePrototypeインスタンス</returns>
        public IEnemyPrototype Clone() {
            return new SlimePrototype(Hp, Attack, Color);
        }

        /// <summary>
        /// スライムの情報を文字列で返す
        /// </summary>
        /// <returns>色とステータスの文字列表現</returns>
        public override string ToString() {
            return $"{Color}スライム [HP={Hp}, ATK={Attack}]";
        }
    }

    // ---- Registry ----

    /// <summary>
    /// プロトタイプの登録と取得を管理するレジストリ
    /// 名前をキーにプロトタイプを保持し必要に応じてクローンを返す
    /// </summary>
    public class EnemyRegistry {
        /// <summary>プロトタイプの辞書</summary>
        private readonly Dictionary<string, IEnemyPrototype> prototypes = new Dictionary<string, IEnemyPrototype>();

        /// <summary>
        /// プロトタイプを登録する
        /// </summary>
        /// <param name="key">登録キー</param>
        /// <param name="prototype">登録するプロトタイプ</param>
        public void Register(string key, IEnemyPrototype prototype) {
            prototypes[key] = prototype;
        }

        /// <summary>
        /// 登録済みプロトタイプのクローンを取得する
        /// </summary>
        /// <param name="key">取得するプロトタイプのキー</param>
        /// <returns>クローンされたインスタンス（未登録の場合はnull）</returns>
        public IEnemyPrototype GetClone(string key) {
            if (prototypes.ContainsKey(key)) {
                return prototypes[key].Clone();
            }
            return null;
        }
    }

    // ---- Demo ----

    /// <summary>
    /// Prototypeパターンのデモ
    /// SlimePrototypeとEnemyRegistryを例にオブジェクトの複製と独立性を示す
    /// </summary>
    [PatternDemo("prototype")]
    public class PrototypeDemo : BasePatternDemo {
        /// <summary>デモのパターンID</summary>
        public override string PatternId => "prototype";

        /// <summary>デモの表示名</summary>
        public override string DisplayName => "Prototype";

        /// <summary>プロトタイプレジストリ</summary>
        private EnemyRegistry registry;

        /// <summary>オリジナルのスライムプロトタイプ</summary>
        private SlimePrototype originalSlime;

        /// <summary>クローンされたスライム</summary>
        private SlimePrototype clonedSlime;

        /// <summary>カスタマイズされたスライムバリアント</summary>
        private SlimePrototype variantSlime;

        /// <summary>スライムの初期HP</summary>
        private const int SlimeBaseHp = 50;

        /// <summary>スライムの初期攻撃力</summary>
        private const int SlimeBaseAttack = 10;

        /// <summary>クローン変更後のHP</summary>
        private const int CloneModifiedHp = 80;

        /// <summary>クローン変更後の攻撃力</summary>
        private const int CloneModifiedAttack = 20;

        /// <summary>バリアントのHP</summary>
        private const int VariantHp = 120;

        /// <summary>バリアントの攻撃力</summary>
        private const int VariantAttack = 35;

        /// <summary>
        /// Prototypeパターンのシナリオを構築する
        /// </summary>
        /// <param name="scenario">ステップを追加するシナリオ</param>
        protected override void BuildScenario(DemoScenario scenario) {
            scenario.AddStep(new DemoStep(
                "EnemyRegistryを作成しスライムプロトタイプを登録する",
                () => {
                    registry = new EnemyRegistry();
                    originalSlime = new SlimePrototype(SlimeBaseHp, SlimeBaseAttack, "緑");
                    registry.Register("slime", originalSlime);
                    Log("Registry", "Register(\"slime\", prototype)", $"登録完了: {originalSlime}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "レジストリからスライムのクローンを取得する",
                () => {
                    clonedSlime = (SlimePrototype)registry.GetClone("slime");
                    Log("Registry", "GetClone(\"slime\")", $"クローン取得: {clonedSlime}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "クローンがオリジナルとは別のインスタンスであることを確認する",
                () => {
                    bool isSeparate = !ReferenceEquals(originalSlime, clonedSlime);
                    Log("検証", "ReferenceEquals(original, clone)",
                        isSeparate ? "False — 別インスタンス" : "True — 同一インスタンス（エラー）");
                }
            ));

            scenario.AddStep(new DemoStep(
                "クローンのステータスを変更する（HP・攻撃力を強化）",
                () => {
                    clonedSlime.Hp = CloneModifiedHp;
                    clonedSlime.Attack = CloneModifiedAttack;
                    clonedSlime.Color = "赤";
                    Log("Client", "clone を変更", $"変更後: {clonedSlime}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "オリジナルのプロトタイプが変更されていないことを検証する",
                () => {
                    bool unchanged = originalSlime.Hp == SlimeBaseHp && originalSlime.Attack == SlimeBaseAttack;
                    Log("検証", "original の状態確認",
                        $"{originalSlime} — {(unchanged ? "変更なし（独立性OK）" : "変更あり（エラー）")}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "もう一つクローンを取得し異なるバリアントとしてカスタマイズする",
                () => {
                    variantSlime = (SlimePrototype)registry.GetClone("slime");
                    variantSlime.Hp = VariantHp;
                    variantSlime.Attack = VariantAttack;
                    variantSlime.Color = "金";
                    Log("Client", "別のクローンをカスタマイズ",
                        $"バリアント: {variantSlime} (原型: {originalSlime})");
                }
            ));
        }
    }
}
