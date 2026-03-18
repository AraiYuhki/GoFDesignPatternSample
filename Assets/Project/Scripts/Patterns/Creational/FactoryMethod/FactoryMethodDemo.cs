namespace GoFPatterns.Patterns {
    // ---- Product ----

    /// <summary>
    /// Factory Methodパターンの製品インターフェース
    /// すべての敵キャラクターが実装する共通契約
    /// </summary>
    public interface IEnemy {
        /// <summary>敵の名前を取得する</summary>
        string Name { get; }
        /// <summary>敵の攻撃力を取得する</summary>
        int AttackPower { get; }
        /// <summary>攻撃を実行する</summary>
        /// <returns>攻撃の説明文</returns>
        string Attack();
    }

    /// <summary>森の敵: ゴブリン</summary>
    public class Goblin : IEnemy {
        /// <summary>敵の名前</summary>
        public string Name => "Goblin";
        /// <summary>攻撃力</summary>
        public int AttackPower => 15;
        /// <summary>
        /// ゴブリンの攻撃を実行する
        /// </summary>
        /// <returns>攻撃の説明文</returns>
        public string Attack() => "Goblin が剣で斬りかかる (15 dmg)";
    }

    /// <summary>ダンジョンの敵: オーク</summary>
    public class Orc : IEnemy {
        /// <summary>敵の名前</summary>
        public string Name => "Orc";
        /// <summary>攻撃力</summary>
        public int AttackPower => 30;
        /// <summary>
        /// オークの攻撃を実行する
        /// </summary>
        /// <returns>攻撃の説明文</returns>
        public string Attack() => "Orc が大剣で強打する (30 dmg)";
    }

    // ---- Creator ----

    /// <summary>
    /// Factory Methodパターンの抽象クリエイター
    /// CreateEnemyを実装することでサブクラスが生成する敵の種類を決定する
    /// </summary>
    public abstract class EnemyCreator {
        /// <summary>クリエイターの名前を取得する</summary>
        public abstract string CreatorName { get; }

        /// <summary>
        /// 敵を生成するファクトリーメソッド（サブクラスで実装）
        /// </summary>
        /// <returns>生成された敵インスタンス</returns>
        public abstract IEnemy CreateEnemy();

        /// <summary>
        /// 敵を生成して出現させる
        /// </summary>
        /// <returns>出現した敵の説明文</returns>
        public string SpawnEnemy() {
            var enemy = CreateEnemy();
            return $"{CreatorName}: {enemy.Name} が出現 (Attack={enemy.AttackPower})";
        }

        /// <summary>
        /// 敵を生成して攻撃させる
        /// </summary>
        /// <returns>攻撃の説明文</returns>
        public string SpawnAndAttack() {
            var enemy = CreateEnemy();
            return enemy.Attack();
        }
    }

    /// <summary>森エリアの敵クリエイター — ゴブリンを生成する</summary>
    public class ForestEnemyCreator : EnemyCreator {
        /// <summary>クリエイターの名前</summary>
        public override string CreatorName => "ForestCreator";
        /// <summary>
        /// ゴブリンを生成する
        /// </summary>
        /// <returns>新しいGoblinインスタンス</returns>
        public override IEnemy CreateEnemy() => new Goblin();
    }

    /// <summary>ダンジョンエリアの敵クリエイター — オークを生成する</summary>
    public class DungeonEnemyCreator : EnemyCreator {
        /// <summary>クリエイターの名前</summary>
        public override string CreatorName => "DungeonCreator";
        /// <summary>
        /// オークを生成する
        /// </summary>
        /// <returns>新しいOrcインスタンス</returns>
        public override IEnemy CreateEnemy() => new Orc();
    }

    // ---- Demo ----

    /// <summary>
    /// Factory Methodパターンのデモ
    /// EnemyCreatorを例にサブクラスが生成するオブジェクトの種類を決める仕組みを示す
    /// </summary>
    [PatternDemo("factory-method")]
    public class FactoryMethodDemo : BasePatternDemo {
        /// <summary>デモのパターンID</summary>
        public override string PatternId => "factory-method";

        /// <summary>デモの表示名</summary>
        public override string DisplayName => "Factory Method";

        /// <summary>現在使用中のクリエイター</summary>
        private EnemyCreator currentCreator;

        /// <summary>
        /// Factory Methodパターンのシナリオを構築する
        /// </summary>
        /// <param name="scenario">ステップを追加するシナリオ</param>
        protected override void BuildScenario(DemoScenario scenario) {
            scenario.AddStep(new DemoStep(
                "森エリア用のForestEnemyCreatorを生成する",
                () => {
                    currentCreator = new ForestEnemyCreator();
                    Log("Client", "new ForestEnemyCreator()", "Creator 設定完了");
                }
            ));

            scenario.AddStep(new DemoStep(
                "ForestCreatorのSpawnEnemy()で敵を生成する（クライアントは種類を知らない）",
                () => {
                    string result = currentCreator.SpawnEnemy();
                    Log("ForestCreator", "CreateEnemy()", result);
                }
            ));

            scenario.AddStep(new DemoStep(
                "SpawnAndAttack()で敵を生成して攻撃させる",
                () => {
                    string result = currentCreator.SpawnAndAttack();
                    Log("ForestCreator", "SpawnAndAttack()", result);
                }
            ));

            scenario.AddStep(new DemoStep(
                "ダンジョン用のDungeonEnemyCreatorに切り替える（Creatorのみ変更）",
                () => {
                    currentCreator = new DungeonEnemyCreator();
                    Log("Client", "new DungeonEnemyCreator()", "Creator 切り替え");
                }
            ));

            scenario.AddStep(new DemoStep(
                "同じSpawnEnemy()呼び出しでも今度はOrcが生成される",
                () => {
                    string result = currentCreator.SpawnEnemy();
                    Log("DungeonCreator", "CreateEnemy()", result);
                }
            ));

            scenario.AddStep(new DemoStep(
                "SpawnAndAttack()を呼ぶと今度はOrcが攻撃する（クライアントコードは同一）",
                () => {
                    string result = currentCreator.SpawnAndAttack();
                    Log("DungeonCreator", "SpawnAndAttack()", result);
                }
            ));
        }
    }
}
