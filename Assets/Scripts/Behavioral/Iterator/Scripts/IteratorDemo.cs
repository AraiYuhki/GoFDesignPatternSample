using UnityEngine;
using UnityEngine.UI;

namespace DesignPatterns.Behavioral.Iterator
{
    /// <summary>
    /// Iteratorパターンのデモシーンを制御するクラス
    ///
    /// 【デモの内容】
    /// - インベントリ内のアイテムをイテレータで順次アクセスする
    /// - Nextボタンで次のアイテムを表示
    /// - Resetボタンでイテレータを先頭に戻す
    /// - Show Allボタンで全アイテムを一覧表示する
    /// </summary>
    public sealed class IteratorDemo : PatternDemoBase
    {
        /// <summary>次のアイテムを表示するボタン</summary>
        [SerializeField]
        private Button nextItemButton;

        /// <summary>イテレータをリセットするボタン</summary>
        [SerializeField]
        private Button resetButton;

        /// <summary>全アイテムを表示するボタン</summary>
        [SerializeField]
        private Button showAllButton;

        /// <summary>アイテムのインベントリ</summary>
        private Inventory inventory;

        /// <summary>インベントリのイテレータ</summary>
        private IIterator<Item> iterator;

        /// <inheritdoc/>
        protected override string PatternName
        {
            get { return "Iterator"; }
        }

        /// <inheritdoc/>
        protected override PatternCategory Category
        {
            get { return PatternCategory.Behavioral; }
        }

        /// <inheritdoc/>
        protected override string Description
        {
            get { return "集合の内部表現を公開せず、要素を順次アクセスする方法を提供する"; }
        }

        /// <inheritdoc/>
        protected override void OnDemoStart()
        {
            BuildInventory();
            iterator = inventory.CreateIterator();

            if (nextItemButton != null)
            {
                nextItemButton.onClick.AddListener(OnNextItem);
            }
            if (resetButton != null)
            {
                resetButton.onClick.AddListener(OnReset);
            }
            if (showAllButton != null)
            {
                showAllButton.onClick.AddListener(OnShowAll);
            }

            InGameLogger.Log($"インベントリに{inventory.Count}個のアイテムがあります", LogColor.White);
            InGameLogger.Log("Nextボタンでアイテムを順に確認してください", LogColor.Yellow);
        }

        /// <summary>
        /// インベントリにサンプルアイテムを追加する
        /// </summary>
        private void BuildInventory()
        {
            inventory = new Inventory();
            inventory.Add(new Item("鉄の剣", "Common", 100));
            inventory.Add(new Item("魔法のローブ", "Rare", 500));
            inventory.Add(new Item("伝説の盾", "Legendary", 2000));
            inventory.Add(new Item("回復薬", "Common", 30));
            inventory.Add(new Item("エリクサー", "Epic", 1000));
            inventory.Add(new Item("竜の鱗", "Rare", 800));
        }

        /// <summary>次のアイテムを表示する</summary>
        private void OnNextItem()
        {
            if (!iterator.HasNext)
            {
                InGameLogger.Log("  全てのアイテムを走査しました（末尾に到達）", LogColor.Red);
                InGameLogger.Log("  Resetボタンで先頭に戻してください", LogColor.Yellow);
                return;
            }

            Item item = iterator.Next();
            InGameLogger.Log($"  → {item}", CategoryColor);
        }

        /// <summary>イテレータを先頭にリセットする</summary>
        private void OnReset()
        {
            iterator.Reset();
            InGameLogger.Log("--- イテレータをリセット ---", LogColor.Yellow);
            InGameLogger.Log("  先頭に戻りました。Nextで再度走査できます", LogColor.White);
        }

        /// <summary>全アイテムを一覧表示する</summary>
        private void OnShowAll()
        {
            InGameLogger.Log("--- 全アイテム一覧 ---", LogColor.Yellow);
            IIterator<Item> allIterator = inventory.CreateIterator();
            int index = 0;
            while (allIterator.HasNext)
            {
                Item item = allIterator.Next();
                InGameLogger.Log($"  [{index}] {item}", CategoryColor);
                index++;
            }
            InGameLogger.Log($"  合計: {inventory.Count}個", LogColor.White);
        }
    }
}
