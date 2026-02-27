using System.Collections.Generic;

namespace DesignPatterns.Behavioral.Iterator {
    /// <summary>
    /// インベントリ内のアイテムを表すクラス
    /// </summary>
    public sealed class Item {
        /// <summary>アイテム名</summary>
        private readonly string name;

        /// <summary>アイテムのレアリティ</summary>
        private readonly string rarity;

        /// <summary>アイテムの価値</summary>
        private readonly int value;

        /// <summary>アイテム名を取得する</summary>
        public string Name {
            get { return name; }
        }

        /// <summary>アイテムのレアリティを取得する</summary>
        public string Rarity {
            get { return rarity; }
        }

        /// <summary>アイテムの価値を取得する</summary>
        public int Value {
            get { return value; }
        }

        /// <summary>
        /// Itemを生成する
        /// </summary>
        /// <param name="name">アイテム名</param>
        /// <param name="rarity">レアリティ</param>
        /// <param name="value">価値</param>
        public Item(string name, string rarity, int value) {
            this.name = name;
            this.rarity = rarity;
            this.value = value;
        }

        /// <summary>
        /// アイテムの文字列表現を返す
        /// </summary>
        /// <returns>フォーマットされたアイテム情報</returns>
        public override string ToString() {
            return $"{name} [{rarity}] (価値: {value}G)";
        }
    }

    /// <summary>
    /// アイテムのコレクションを管理するインベントリクラス
    /// Iteratorパターンにおける「ConcreteAggregate」に相当する
    /// 内部のリスト構造を公開せずにイテレータを提供する
    /// </summary>
    public sealed class Inventory : IAggregate<Item> {
        /// <summary>アイテムの内部リスト</summary>
        private readonly List<Item> items = new List<Item>();

        /// <summary>インベントリ内のアイテム数を取得する</summary>
        public int Count {
            get { return items.Count; }
        }

        /// <summary>
        /// アイテムをインベントリに追加する
        /// </summary>
        /// <param name="item">追加するアイテム</param>
        public void Add(Item item) {
            items.Add(item);
        }

        /// <summary>
        /// 指定インデックスのアイテムを取得する
        /// </summary>
        /// <param name="index">アイテムのインデックス</param>
        /// <returns>指定されたアイテム</returns>
        public Item GetItem(int index) {
            return items[index];
        }

        /// <summary>
        /// インベントリのイテレータを生成する
        /// </summary>
        /// <returns>新しいInventoryIteratorインスタンス</returns>
        public IIterator<Item> CreateIterator() {
            return new InventoryIterator(this);
        }
    }

    /// <summary>
    /// インベントリを走査するイテレータクラス
    /// Iteratorパターンにおける「ConcreteIterator」に相当する
    /// インベントリの内部構造に依存せず順次アクセスを提供する
    /// </summary>
    public sealed class InventoryIterator : IIterator<Item> {
        /// <summary>走査対象のインベントリ</summary>
        private readonly Inventory inventory;

        /// <summary>現在のインデックス位置</summary>
        private int currentIndex;

        /// <summary>
        /// 次の要素が存在するかどうかを取得する
        /// </summary>
        public bool HasNext {
            get { return currentIndex < inventory.Count; }
        }

        /// <summary>
        /// 現在の要素を取得する
        /// </summary>
        public Item Current {
            get {
                if (currentIndex <= 0 || currentIndex > inventory.Count) {
                    return null;
                }
                return inventory.GetItem(currentIndex - 1);
            }
        }

        /// <summary>
        /// InventoryIteratorを生成する
        /// </summary>
        /// <param name="inventory">走査対象のインベントリ</param>
        public InventoryIterator(Inventory inventory) {
            this.inventory = inventory;
            currentIndex = 0;
        }

        /// <summary>
        /// 次の要素に移動し、その要素を返す
        /// </summary>
        /// <returns>次のアイテム</returns>
        public Item Next() {
            Item item = inventory.GetItem(currentIndex);
            currentIndex++;
            return item;
        }

        /// <summary>
        /// イテレータを先頭にリセットする
        /// </summary>
        public void Reset() {
            currentIndex = 0;
        }
    }
}
