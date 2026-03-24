namespace GoFPatterns.Patterns {
    // ---- Product ----

    /// <summary>
    /// Builderパターンの生成物であるキャラクターデータ
    /// HP・攻撃力・防御力・名前を保持する
    /// </summary>
    public class Character {
        /// <summary>キャラクターの名前</summary>
        public string Name { get; set; }
        /// <summary>HP</summary>
        public int Hp { get; set; }
        /// <summary>攻撃力</summary>
        public int Attack { get; set; }
        /// <summary>防御力</summary>
        public int Defense { get; set; }

        /// <summary>
        /// キャラクターの情報を文字列で返す
        /// </summary>
        /// <returns>名前とステータスの文字列表現</returns>
        public override string ToString() {
            return $"{Name} [HP={Hp}, ATK={Attack}, DEF={Defense}]";
        }
    }

    // ---- Builder Interface ----

    /// <summary>
    /// キャラクタービルダーの抽象インターフェース
    /// 段階的にキャラクターを構築するメソッド群を定義する
    /// </summary>
    public interface ICharacterBuilder {
        /// <summary>ビルダーの名前を取得する</summary>
        string BuilderName { get; }
        /// <summary>
        /// キャラクター名を設定する
        /// </summary>
        /// <param name="name">設定する名前</param>
        /// <returns>メソッドチェーン用のビルダー自身</returns>
        ICharacterBuilder SetName(string name);
        /// <summary>
        /// HPを設定する
        /// </summary>
        /// <param name="hp">設定するHP値</param>
        /// <returns>メソッドチェーン用のビルダー自身</returns>
        ICharacterBuilder SetHp(int hp);
        /// <summary>
        /// 攻撃力を設定する
        /// </summary>
        /// <param name="attack">設定する攻撃力</param>
        /// <returns>メソッドチェーン用のビルダー自身</returns>
        ICharacterBuilder SetAttack(int attack);
        /// <summary>
        /// 防御力を設定する
        /// </summary>
        /// <param name="defense">設定する防御力</param>
        /// <returns>メソッドチェーン用のビルダー自身</returns>
        ICharacterBuilder SetDefense(int defense);
        /// <summary>
        /// 構築結果のキャラクターを取得する
        /// </summary>
        /// <returns>構築されたキャラクター</returns>
        Character Build();
    }

    // ---- Concrete Builders ----

    /// <summary>
    /// 戦士キャラクターのビルダー
    /// 高HPと高防御力を持つ前衛タイプのキャラクターを構築する
    /// </summary>
    public class WarriorBuilder : ICharacterBuilder {
        /// <summary>構築中のキャラクター</summary>
        private readonly Character character = new Character();

        /// <summary>ビルダーの名前</summary>
        public string BuilderName => "WarriorBuilder";

        /// <summary>
        /// キャラクター名を設定する
        /// </summary>
        /// <param name="name">設定する名前</param>
        /// <returns>メソッドチェーン用のビルダー自身</returns>
        public ICharacterBuilder SetName(string name) {
            character.Name = name;
            return this;
        }

        /// <summary>
        /// HPを設定する
        /// </summary>
        /// <param name="hp">設定するHP値</param>
        /// <returns>メソッドチェーン用のビルダー自身</returns>
        public ICharacterBuilder SetHp(int hp) {
            character.Hp = hp;
            return this;
        }

        /// <summary>
        /// 攻撃力を設定する
        /// </summary>
        /// <param name="attack">設定する攻撃力</param>
        /// <returns>メソッドチェーン用のビルダー自身</returns>
        public ICharacterBuilder SetAttack(int attack) {
            character.Attack = attack;
            return this;
        }

        /// <summary>
        /// 防御力を設定する
        /// </summary>
        /// <param name="defense">設定する防御力</param>
        /// <returns>メソッドチェーン用のビルダー自身</returns>
        public ICharacterBuilder SetDefense(int defense) {
            character.Defense = defense;
            return this;
        }

        /// <summary>
        /// 構築結果のキャラクターを取得する
        /// </summary>
        /// <returns>構築された戦士キャラクター</returns>
        public Character Build() => character;
    }

    /// <summary>
    /// 魔法使いキャラクターのビルダー
    /// 高攻撃力と低HPを持つ後衛タイプのキャラクターを構築する
    /// </summary>
    public class MageBuilder : ICharacterBuilder {
        /// <summary>構築中のキャラクター</summary>
        private readonly Character character = new Character();

        /// <summary>ビルダーの名前</summary>
        public string BuilderName => "MageBuilder";

        /// <summary>
        /// キャラクター名を設定する
        /// </summary>
        /// <param name="name">設定する名前</param>
        /// <returns>メソッドチェーン用のビルダー自身</returns>
        public ICharacterBuilder SetName(string name) {
            character.Name = name;
            return this;
        }

        /// <summary>
        /// HPを設定する
        /// </summary>
        /// <param name="hp">設定するHP値</param>
        /// <returns>メソッドチェーン用のビルダー自身</returns>
        public ICharacterBuilder SetHp(int hp) {
            character.Hp = hp;
            return this;
        }

        /// <summary>
        /// 攻撃力を設定する
        /// </summary>
        /// <param name="attack">設定する攻撃力</param>
        /// <returns>メソッドチェーン用のビルダー自身</returns>
        public ICharacterBuilder SetAttack(int attack) {
            character.Attack = attack;
            return this;
        }

        /// <summary>
        /// 防御力を設定する
        /// </summary>
        /// <param name="defense">設定する防御力</param>
        /// <returns>メソッドチェーン用のビルダー自身</returns>
        public ICharacterBuilder SetDefense(int defense) {
            character.Defense = defense;
            return this;
        }

        /// <summary>
        /// 構築結果のキャラクターを取得する
        /// </summary>
        /// <returns>構築された魔法使いキャラクター</returns>
        public Character Build() => character;
    }

    // ---- Director ----

    /// <summary>
    /// キャラクター構築のディレクター
    /// ビルダーに構築手順を指示し、一定の手順でキャラクターを組み立てる
    /// </summary>
    public class CharacterDirector {
        /// <summary>戦士の初期HP</summary>
        private const int WarriorHp = 150;
        /// <summary>戦士の攻撃力</summary>
        private const int WarriorAttack = 30;
        /// <summary>戦士の防御力</summary>
        private const int WarriorDefense = 50;
        /// <summary>魔法使いの初期HP</summary>
        private const int MageHp = 80;
        /// <summary>魔法使いの攻撃力</summary>
        private const int MageAttack = 60;
        /// <summary>魔法使いの防御力</summary>
        private const int MageDefense = 15;

        /// <summary>
        /// 戦士キャラクターを構築する
        /// </summary>
        /// <param name="builder">使用するビルダー</param>
        /// <returns>構築された戦士キャラクター</returns>
        public Character ConstructWarrior(ICharacterBuilder builder) {
            builder.SetName("Warrior")
                   .SetHp(WarriorHp)
                   .SetAttack(WarriorAttack)
                   .SetDefense(WarriorDefense);
            return builder.Build();
        }

        /// <summary>
        /// 魔法使いキャラクターを構築する
        /// </summary>
        /// <param name="builder">使用するビルダー</param>
        /// <returns>構築された魔法使いキャラクター</returns>
        public Character ConstructMage(ICharacterBuilder builder) {
            builder.SetName("Mage")
                   .SetHp(MageHp)
                   .SetAttack(MageAttack)
                   .SetDefense(MageDefense);
            return builder.Build();
        }
    }

    // ---- Demo ----

    /// <summary>
    /// Builderパターンのデモ
    /// CharacterDirectorを例に同じ構築手順で異なるビルダーを使い異なるキャラクターを生成する仕組みを示す
    /// </summary>
    [PatternDemo("builder")]
    public class BuilderDemo : BasePatternDemo {
        /// <summary>デモのパターンID</summary>
        public override string PatternId => "builder";

        /// <summary>デモの表示名</summary>
        public override string DisplayName => "Builder";

        /// <summary>ディレクター</summary>
        private CharacterDirector director;

        /// <summary>戦士ビルダー</summary>
        private WarriorBuilder warriorBuilder;

        /// <summary>魔法使いビルダー</summary>
        private MageBuilder mageBuilder;

        /// <summary>構築された戦士キャラクター</summary>
        private Character warrior;

        /// <summary>構築された魔法使いキャラクター</summary>
        private Character mage;

        /// <summary>
        /// Builderパターンのシナリオを構築する
        /// </summary>
        /// <param name="scenario">ステップを追加するシナリオ</param>
        protected override void BuildScenario(DemoScenario scenario) {
            scenario.AddStep(new DemoStep(
                "CharacterDirectorを生成して構築手順の管理者を準備する",
                () => {
                    director = new CharacterDirector();
                    Log("Client", "new CharacterDirector()", "Director 生成完了");
                }
            ));

            scenario.AddStep(new DemoStep(
                "WarriorBuilderを生成してDirectorに渡す",
                () => {
                    warriorBuilder = new WarriorBuilder();
                    Log("Client", "new WarriorBuilder()", $"Builder 生成: {warriorBuilder.BuilderName}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "DirectorがWarriorBuilderでSetName→SetHp→SetAttack→SetDefenseを実行する",
                () => {
                    warrior = director.ConstructWarrior(warriorBuilder);
                    Log("Director", "ConstructWarrior(builder)", "SetName→SetHp→SetAttack→SetDefense 完了");
                }
            ));

            scenario.AddStep(new DemoStep(
                "WarriorBuilderからBuild()で戦士キャラクターを取得する",
                () => {
                    Log("WarriorBuilder", "Build()", warrior.ToString());
                }
            ));

            scenario.AddStep(new DemoStep(
                "MageBuilderを生成して同じDirectorに渡す",
                () => {
                    mageBuilder = new MageBuilder();
                    Log("Client", "new MageBuilder()", $"Builder 生成: {mageBuilder.BuilderName}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "DirectorがMageBuilderで同じ手順（SetName→SetHp→SetAttack→SetDefense）を実行する",
                () => {
                    mage = director.ConstructMage(mageBuilder);
                    Log("Director", "ConstructMage(builder)", "SetName→SetHp→SetAttack→SetDefense 完了");
                }
            ));

            scenario.AddStep(new DemoStep(
                "MageBuilderからBuild()で魔法使いキャラクターを取得し戦士と比較する",
                () => {
                    Log("MageBuilder", "Build()", $"{mage} (戦士: {warrior})");
                }
            ));
        }
    }
}
