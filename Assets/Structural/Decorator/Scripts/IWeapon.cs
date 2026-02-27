namespace DesignPatterns.Structural.Decorator {
    /// <summary>
    /// 武器のインターフェース（Component）
    ///
    /// 【Decoratorパターンの意図】
    /// オブジェクトに動的に機能を追加する
    /// サブクラスによる拡張の代わりに、デコレーターで柔軟に機能を積み重ねる
    /// </summary>
    public interface IWeapon {
        /// <summary>武器の名前</summary>
        string Name { get; }

        /// <summary>攻撃力</summary>
        int AttackPower { get; }

        /// <summary>
        /// 武器の説明を返す
        /// </summary>
        /// <returns>武器の詳細説明</returns>
        string GetDescription();
    }
}
