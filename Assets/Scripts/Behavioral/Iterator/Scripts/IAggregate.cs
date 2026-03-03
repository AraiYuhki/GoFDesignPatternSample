namespace DesignPatterns.Behavioral.Iterator
{
    /// <summary>
    /// イテレータを生成するための集合体インターフェース
    /// Iteratorパターンにおける「Aggregate」に相当する
    /// コレクションの走査手段を提供する
    /// </summary>
    /// <typeparam name="T">要素の型</typeparam>
    public interface IAggregate<T>
    {
        /// <summary>
        /// コレクションのイテレータを生成する
        /// </summary>
        /// <returns>新しいイテレータインスタンス</returns>
        IIterator<T> CreateIterator();
    }
}
