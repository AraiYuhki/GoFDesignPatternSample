namespace GoFPatterns.Patterns {
    // ---- Strategy interface ----

    /// <summary>
    /// Strategyパターンの攻撃戦略インターフェース
    /// </summary>
    public interface IAttackStrategy {
        /// <summary>戦略の名前を取得する</summary>
        string Name { get; }
        /// <summary>
        /// 攻撃を実行する
        /// </summary>
        /// <param name="attackerName">攻撃者の名前</param>
        /// <returns>攻撃の結果を説明する文字列</returns>
        string Execute(string attackerName);
    }

    // ---- ConcreteStrategies ----

    /// <summary>高火力を優先する積極的攻撃戦略</summary>
    public class AggressiveStrategy : IAttackStrategy {
        /// <summary>戦略の名前</summary>
        public string Name => "Aggressive";
        /// <summary>
        /// 高火力の攻撃を実行する
        /// </summary>
        /// <param name="attackerName">攻撃者の名前</param>
        /// <returns>攻撃の説明文</returns>
        public string Execute(string attackerName) =>
            $"{attackerName} が猛攻撃! ダメージ: 50 (防御無視)";
    }

    /// <summary>防御を重視しながら反撃する防御的戦略</summary>
    public class DefensiveStrategy : IAttackStrategy {
        /// <summary>戦略の名前</summary>
        public string Name => "Defensive";
        /// <summary>
        /// 防御的な反撃を実行する
        /// </summary>
        /// <param name="attackerName">攻撃者の名前</param>
        /// <returns>攻撃の説明文</returns>
        public string Execute(string attackerName) =>
            $"{attackerName} がガードしながら反撃. ダメージ: 20 (防御+50%)";
    }

    /// <summary>攻防バランスを取る均衡戦略</summary>
    public class BalancedStrategy : IAttackStrategy {
        /// <summary>戦略の名前</summary>
        public string Name => "Balanced";
        /// <summary>
        /// バランス型の攻撃を実行する
        /// </summary>
        /// <param name="attackerName">攻撃者の名前</param>
        /// <returns>攻撃の説明文</returns>
        public string Execute(string attackerName) =>
            $"{attackerName} がバランス攻撃. ダメージ: 35 (コンボボーナス)";
    }

    // ---- Context ----

    /// <summary>
    /// Strategyパターンのコンテキスト
    /// 実行時に攻撃戦略を切り替えることができるキャラクター
    /// </summary>
    public class BattleCharacter {
        /// <summary>キャラクターの名前</summary>
        private readonly string name;
        /// <summary>現在の攻撃戦略</summary>
        private IAttackStrategy strategy;

        /// <summary>キャラクターの名前を取得する</summary>
        public string Name => name;
        /// <summary>現在の戦略名を取得する</summary>
        public string CurrentStrategyName => strategy?.Name ?? "なし";

        /// <summary>
        /// BattleCharacterを生成する
        /// </summary>
        /// <param name="name">キャラクター名</param>
        public BattleCharacter(string name) {
            this.name = name;
        }

        /// <summary>
        /// 攻撃戦略を設定する（実行時に切り替え可能）
        /// </summary>
        /// <param name="attackStrategy">設定する戦略</param>
        public void SetStrategy(IAttackStrategy attackStrategy) {
            strategy = attackStrategy;
        }

        /// <summary>
        /// 現在の戦略で攻撃する
        /// </summary>
        /// <returns>攻撃の結果</returns>
        public string Attack() {
            if (strategy == null) {
                return $"{name}: 戦略が設定されていない";
            }
            return strategy.Execute(name);
        }
    }

    // ---- Demo ----

    /// <summary>
    /// Strategyパターンのデモ
    /// BattleCharacterを例に実行時に攻撃アルゴリズムを交換する仕組みを示す
    /// </summary>
    [PatternDemo("strategy")]
    public class StrategyDemo : BasePatternDemo {
        /// <summary>デモのパターンID</summary>
        public override string PatternId => "strategy";

        /// <summary>デモの表示名</summary>
        public override string DisplayName => "Strategy";

        /// <summary>戦略を保持するキャラクター</summary>
        private BattleCharacter character;

        /// <summary>
        /// リセット時にキャラクターを再生成する
        /// </summary>
        protected override void OnReset() {
            character = null;
        }

        /// <summary>
        /// Strategyパターンのシナリオを構築する
        /// </summary>
        /// <param name="scenario">ステップを追加するシナリオ</param>
        protected override void BuildScenario(DemoScenario scenario) {
            character = new BattleCharacter("勇者");

            scenario.AddStep(new DemoStep(
                "AggressiveStrategyを設定する",
                () => {
                    character.SetStrategy(new AggressiveStrategy());
                    Log("Client", $"SetStrategy({character.CurrentStrategyName})", "戦略切り替え完了");
                }
            ));

            scenario.AddStep(new DemoStep(
                "AggressiveStrategyで攻撃する（高火力）",
                () => {
                    string result = character.Attack();
                    Log(character.Name, "Attack()", result);
                }
            ));

            scenario.AddStep(new DemoStep(
                "DefensiveStrategyに切り替える",
                () => {
                    character.SetStrategy(new DefensiveStrategy());
                    Log("Client", $"SetStrategy({character.CurrentStrategyName})", "戦略切り替え完了");
                }
            ));

            scenario.AddStep(new DemoStep(
                "DefensiveStrategyで攻撃する（防御重視）",
                () => {
                    string result = character.Attack();
                    Log(character.Name, "Attack()", result);
                }
            ));

            scenario.AddStep(new DemoStep(
                "BalancedStrategyに切り替える",
                () => {
                    character.SetStrategy(new BalancedStrategy());
                    Log("Client", $"SetStrategy({character.CurrentStrategyName})", "戦略切り替え完了");
                }
            ));

            scenario.AddStep(new DemoStep(
                "BalancedStrategyで攻撃する（バランス型）",
                () => {
                    string result = character.Attack();
                    Log(character.Name, "Attack()", result);
                }
            ));
        }
    }
}
