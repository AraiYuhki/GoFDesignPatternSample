namespace DesignPatterns.Creational.FactoryMethod {
    /// <summary>
    /// 敵キャラクターを生成する抽象ファクトリ（Creator）
    ///
    /// 【Factory Methodパターンにおける役割】
    /// オブジェクトの生成をサブクラスに委ねるためのメソッドを定義する
    /// サブクラスがどのクラスをインスタンス化するかを決定する
    /// </summary>
    public abstract class EnemyFactory {
        /// <summary>ファクトリの名前</summary>
        public abstract string FactoryName { get; }

        /// <summary>
        /// 敵キャラクターを生成する（Factory Method）
        /// サブクラスが具体的な生成処理をオーバーライドする
        /// </summary>
        /// <returns>生成された敵キャラクター</returns>
        public abstract IEnemy CreateEnemy();
    }

    /// <summary>
    /// スライムを生成するファクトリ（ConcreteCreator）
    /// </summary>
    public sealed class SlimeFactory : EnemyFactory {
        /// <inheritdoc/>
        public override string FactoryName { get { return "スライム工場"; } }

        /// <inheritdoc/>
        public override IEnemy CreateEnemy() {
            return new Slime();
        }
    }

    /// <summary>
    /// ゴブリンを生成するファクトリ（ConcreteCreator）
    /// </summary>
    public sealed class GoblinFactory : EnemyFactory {
        /// <inheritdoc/>
        public override string FactoryName { get { return "ゴブリン工場"; } }

        /// <inheritdoc/>
        public override IEnemy CreateEnemy() {
            return new Goblin();
        }
    }

    /// <summary>
    /// ドラゴンを生成するファクトリ（ConcreteCreator）
    /// </summary>
    public sealed class DragonFactory : EnemyFactory {
        /// <inheritdoc/>
        public override string FactoryName { get { return "ドラゴン工場"; } }

        /// <inheritdoc/>
        public override IEnemy CreateEnemy() {
            return new Dragon();
        }
    }
}
