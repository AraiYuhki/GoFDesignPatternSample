using System.Collections.Generic;

namespace GoFPatterns.Patterns {
    // ---- Iterator interface ----

    /// <summary>
    /// Iteratorパターンのイテレータインターフェース
    /// コレクションを順番に走査するための共通契約
    /// </summary>
    /// <typeparam name="T">要素の型</typeparam>
    public interface IIterator<T> {
        /// <summary>
        /// 次の要素が存在するかどうかを返す
        /// </summary>
        /// <returns>次の要素がある場合true</returns>
        bool HasNext();
        /// <summary>
        /// 次の要素を取得してカーソルを進める
        /// </summary>
        /// <returns>次の要素</returns>
        T Next();
        /// <summary>
        /// カーソルを先頭にリセットする
        /// </summary>
        void Reset();
    }

    // ---- Aggregate ----

    /// <summary>
    /// アイテムを保持するインベントリコレクション
    /// イテレータを生成してコレクション内部を公開せずに走査を可能にする
    /// </summary>
    public class Inventory {
        /// <summary>アイテムのリスト</summary>
        private readonly List<string> items = new List<string>();

        /// <summary>アイテム数を取得する</summary>
        public int Count => items.Count;

        /// <summary>
        /// アイテムを追加する
        /// </summary>
        /// <param name="item">追加するアイテム名</param>
        public void AddItem(string item) {
            items.Add(item);
        }

        /// <summary>
        /// 指定インデックスのアイテムを取得する
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>アイテム名</returns>
        public string GetItem(int index) {
            return items[index];
        }

        /// <summary>
        /// 順方向イテレータを生成する
        /// </summary>
        /// <returns>順方向イテレータ</returns>
        public IIterator<string> CreateForwardIterator() {
            return new InventoryIterator(this);
        }

        /// <summary>
        /// 逆方向イテレータを生成する
        /// </summary>
        /// <returns>逆方向イテレータ</returns>
        public IIterator<string> CreateReverseIterator() {
            return new ReverseInventoryIterator(this);
        }
    }

    // ---- ConcreteIterators ----

    /// <summary>
    /// Inventoryを先頭から末尾へ走査する順方向イテレータ
    /// </summary>
    public class InventoryIterator : IIterator<string> {
        /// <summary>走査対象のインベントリ</summary>
        private readonly Inventory inventory;
        /// <summary>現在のカーソル位置</summary>
        private int currentIndex;

        /// <summary>
        /// InventoryIteratorを生成する
        /// </summary>
        /// <param name="inventory">走査対象のインベントリ</param>
        public InventoryIterator(Inventory inventory) {
            this.inventory = inventory;
            currentIndex = 0;
        }

        /// <summary>
        /// 次の要素が存在するかどうかを返す
        /// </summary>
        /// <returns>次の要素がある場合true</returns>
        public bool HasNext() {
            return currentIndex < inventory.Count;
        }

        /// <summary>
        /// 次の要素を取得してカーソルを進める
        /// </summary>
        /// <returns>次のアイテム名</returns>
        public string Next() {
            string item = inventory.GetItem(currentIndex);
            currentIndex++;
            return item;
        }

        /// <summary>
        /// カーソルを先頭にリセットする
        /// </summary>
        public void Reset() {
            currentIndex = 0;
        }
    }

    /// <summary>
    /// Inventoryを末尾から先頭へ走査する逆方向イテレータ
    /// </summary>
    public class ReverseInventoryIterator : IIterator<string> {
        /// <summary>走査対象のインベントリ</summary>
        private readonly Inventory inventory;
        /// <summary>現在のカーソル位置</summary>
        private int currentIndex;

        /// <summary>
        /// ReverseInventoryIteratorを生成する
        /// </summary>
        /// <param name="inventory">走査対象のインベントリ</param>
        public ReverseInventoryIterator(Inventory inventory) {
            this.inventory = inventory;
            currentIndex = inventory.Count - 1;
        }

        /// <summary>
        /// 次の要素が存在するかどうかを返す
        /// </summary>
        /// <returns>次の要素がある場合true</returns>
        public bool HasNext() {
            return currentIndex >= 0;
        }

        /// <summary>
        /// 次の要素を取得してカーソルを戻す
        /// </summary>
        /// <returns>次のアイテム名</returns>
        public string Next() {
            string item = inventory.GetItem(currentIndex);
            currentIndex--;
            return item;
        }

        /// <summary>
        /// カーソルを末尾にリセットする
        /// </summary>
        public void Reset() {
            currentIndex = inventory.Count - 1;
        }
    }

    // ---- Demo ----

    /// <summary>
    /// Iteratorパターンのデモ
    /// Inventoryを例に順方向・逆方向イテレータによる走査の仕組みを示す
    /// </summary>
    [PatternDemo("iterator")]
    public class IteratorDemo : BasePatternDemo {
        /// <summary>デモのパターンID</summary>
        public override string PatternId => "iterator";

        /// <summary>デモの表示名</summary>
        public override string DisplayName => "Iterator";

        /// <summary>走査対象のインベントリ</summary>
        private Inventory inventory;

        /// <summary>
        /// リセット時にインベントリを再生成する
        /// </summary>
        protected override void OnReset() {
            inventory = null;
        }

        /// <summary>
        /// Iteratorパターンのシナリオを構築する
        /// </summary>
        /// <param name="scenario">ステップを追加するシナリオ</param>
        protected override void BuildScenario(DemoScenario scenario) {
            inventory = new Inventory();

            scenario.AddStep(new DemoStep(
                "Inventoryにアイテムを追加する",
                () => {
                    inventory.AddItem("剣");
                    inventory.AddItem("盾");
                    inventory.AddItem("ポーション");
                    inventory.AddItem("鍵");
                    Log("Inventory", "AddItem x4", "剣, 盾, ポーション, 鍵 を追加");
                }
            ));

            scenario.AddStep(new DemoStep(
                "順方向イテレータを生成する",
                () => {
                    IIterator<string> iterator = inventory.CreateForwardIterator();
                    Log("Inventory", "CreateForwardIterator()", $"順方向イテレータを生成 (要素数: {inventory.Count})");
                }
            ));

            scenario.AddStep(new DemoStep(
                "順方向イテレータで全アイテムを走査する",
                () => {
                    IIterator<string> iterator = inventory.CreateForwardIterator();
                    var result = new System.Text.StringBuilder();
                    int index = 0;
                    while (iterator.HasNext()) {
                        if (index > 0) {
                            result.Append(" → ");
                        }
                        result.Append(iterator.Next());
                        index++;
                    }
                    Log("ForwardIterator", "走査完了", result.ToString());
                }
            ));

            scenario.AddStep(new DemoStep(
                "逆方向イテレータを生成する",
                () => {
                    IIterator<string> iterator = inventory.CreateReverseIterator();
                    Log("Inventory", "CreateReverseIterator()", $"逆方向イテレータを生成 (要素数: {inventory.Count})");
                }
            ));

            scenario.AddStep(new DemoStep(
                "逆方向イテレータで全アイテムを走査する",
                () => {
                    IIterator<string> iterator = inventory.CreateReverseIterator();
                    var result = new System.Text.StringBuilder();
                    int index = 0;
                    while (iterator.HasNext()) {
                        if (index > 0) {
                            result.Append(" → ");
                        }
                        result.Append(iterator.Next());
                        index++;
                    }
                    Log("ReverseIterator", "走査完了", result.ToString());
                }
            ));

            scenario.AddStep(new DemoStep(
                "コレクション自体は変更されていないことを確認する",
                () => {
                    var result = new System.Text.StringBuilder();
                    for (int i = 0; i < inventory.Count; i++) {
                        if (i > 0) {
                            result.Append(", ");
                        }
                        result.Append(inventory.GetItem(i));
                    }
                    Log("Inventory", "状態確認", $"コレクション不変: [{result}] (要素数: {inventory.Count})");
                }
            ));
        }
    }
}
