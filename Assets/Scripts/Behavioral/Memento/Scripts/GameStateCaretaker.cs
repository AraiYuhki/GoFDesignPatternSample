using System.Collections.Generic;

namespace DesignPatterns.Behavioral.Memento
{
    /// <summary>
    /// Mementoの保存・管理を行うCaretakerクラス
    /// Mementoパターンにおける「Caretaker」に相当する
    /// Mementoの内容にはアクセスせず、保存と取得のみを行う
    /// </summary>
    public sealed class GameStateCaretaker
    {
        /// <summary>保存されたMementoのリスト</summary>
        private readonly List<GameStateMemento> saveSlots = new List<GameStateMemento>();

        /// <summary>
        /// 状態をセーブスロットに保存する
        /// </summary>
        /// <param name="memento">保存するMemento</param>
        public void SaveState(GameStateMemento memento)
        {
            saveSlots.Add(memento);
        }

        /// <summary>
        /// 指定インデックスのセーブデータを取得する
        /// </summary>
        /// <param name="index">セーブスロットのインデックス</param>
        /// <returns>指定されたMemento、インデックスが無効な場合はnull</returns>
        public GameStateMemento LoadState(int index)
        {
            if (index < 0 || index >= saveSlots.Count)
            {
                return null;
            }
            return saveSlots[index];
        }

        /// <summary>
        /// 保存されているセーブデータの数を取得する
        /// </summary>
        /// <returns>セーブデータの数</returns>
        public int GetSaveCount()
        {
            return saveSlots.Count;
        }
    }
}
