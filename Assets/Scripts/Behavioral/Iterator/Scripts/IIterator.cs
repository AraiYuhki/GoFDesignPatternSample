namespace DesignPatterns.Behavioral.Iterator
{
    /// <summary>
    /// コレクションの要素を順次アクセスするためのイテレータインターフェース
    /// Iteratorパターンにおける「Iterator」に相当する
    /// コレクションの内部表現を公開せずに要素を走査する
    /// </summary>
    /// <typeparam name="T">要素の型</typeparam>
    public interface IIterator<T>
    {
        /// <summary>次の要素が存在するかどうかを取得する</summary>
        bool HasNext { get; }

        /// <summary>現在の要素を取得する</summary>
        T Current { get; }

        /// <summary>
        /// 次の要素に移動し、その要素を返す
        /// </summary>
        /// <returns>次の要素</returns>
        T Next();

        /// <summary>
        /// イテレータを先頭にリセットする
        /// </summary>
        void Reset();
    }
}
