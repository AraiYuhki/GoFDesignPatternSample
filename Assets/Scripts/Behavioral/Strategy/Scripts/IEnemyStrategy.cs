namespace DesignPatterns.Behavioral.Strategy
{
    /// <summary>
    /// 敵AIの戦略を定義するインターフェース
    /// Strategyパターンにおける「Strategy」に相当する
    /// アルゴリズムの共通インターフェースを提供し、具象戦略を差し替え可能にする
    /// </summary>
    public interface IEnemyStrategy
    {
        /// <summary>戦略の名称</summary>
        string StrategyName { get; }

        /// <summary>
        /// 戦略に基づいた行動を実行し、行動内容を返す
        /// </summary>
        /// <returns>実行した行動の説明テキスト</returns>
        string Execute();
    }
}
