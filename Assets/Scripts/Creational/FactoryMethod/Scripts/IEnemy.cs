namespace DesignPatterns.Creational.FactoryMethod
{
    /// <summary>
    /// 敵キャラクターのインターフェース（Product）
    ///
    /// 【Factory Methodパターンにおける役割】
    /// ファクトリが生成するプロダクトの共通インターフェースを定義する
    /// </summary>
    public interface IEnemy
    {
        /// <summary>敵の名前</summary>
        string Name { get; }

        /// <summary>HP（体力）</summary>
        int Hp { get; }

        /// <summary>攻撃力</summary>
        int Attack { get; }

        /// <summary>
        /// 攻撃を実行する
        /// </summary>
        /// <returns>攻撃の説明文</returns>
        string PerformAttack();
    }
}
