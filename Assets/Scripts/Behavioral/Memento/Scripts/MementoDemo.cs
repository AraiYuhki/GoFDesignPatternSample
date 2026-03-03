using UnityEngine;
using UnityEngine.UI;

namespace DesignPatterns.Behavioral.Memento
{
    /// <summary>
    /// Mementoパターンのデモシーンを制御するクラス
    ///
    /// 【デモの内容】
    /// - ゲーム状態（レベル、HP、ゴールド）を変更できる
    /// - 状態をセーブし、後からロードして復元できる
    /// - セーブ/ロードの仕組みをMementoパターンで実現している
    /// </summary>
    public sealed class MementoDemo : PatternDemoBase
    {
        /// <summary>レベルアップボタン</summary>
        [SerializeField]
        private Button levelUpButton;

        /// <summary>ダメージを受けるボタン</summary>
        [SerializeField]
        private Button takeDamageButton;

        /// <summary>ゴールド獲得ボタン</summary>
        [SerializeField]
        private Button earnGoldButton;

        /// <summary>セーブボタン</summary>
        [SerializeField]
        private Button saveButton;

        /// <summary>最後のセーブをロードするボタン</summary>
        [SerializeField]
        private Button loadLastButton;

        /// <summary>現在の状態を表示するボタン</summary>
        [SerializeField]
        private Button showCurrentButton;

        /// <summary>ゲーム状態（Originator）</summary>
        private GameState gameState;

        /// <summary>セーブデータ管理（Caretaker）</summary>
        private GameStateCaretaker caretaker;

        /// <summary>レベルアップ時のHP増加量</summary>
        private const int HpPerLevel = 20;

        /// <summary>被ダメージ量</summary>
        private const int DamageAmount = 30;

        /// <summary>ゴールド獲得量</summary>
        private const int GoldAmount = 50;

        /// <summary>初期レベル</summary>
        private const int InitialLevel = 1;

        /// <summary>初期HP</summary>
        private const int InitialHp = 100;

        /// <summary>初期ゴールド</summary>
        private const int InitialGold = 0;

        /// <summary>プレイヤー名</summary>
        private const string PlayerName = "勇者";

        /// <inheritdoc/>
        protected override string PatternName
        {
            get { return "Memento"; }
        }

        /// <inheritdoc/>
        protected override PatternCategory Category
        {
            get { return PatternCategory.Behavioral; }
        }

        /// <inheritdoc/>
        protected override string Description
        {
            get { return "オブジェクトの状態を保存し、後から復元できるようにする"; }
        }

        /// <inheritdoc/>
        protected override void OnDemoStart()
        {
            gameState = new GameState(PlayerName, InitialLevel, InitialHp, InitialGold);
            caretaker = new GameStateCaretaker();

            if (levelUpButton != null)
            {
                levelUpButton.onClick.AddListener(OnLevelUp);
            }
            if (takeDamageButton != null)
            {
                takeDamageButton.onClick.AddListener(OnTakeDamage);
            }
            if (earnGoldButton != null)
            {
                earnGoldButton.onClick.AddListener(OnEarnGold);
            }
            if (saveButton != null)
            {
                saveButton.onClick.AddListener(OnSave);
            }
            if (loadLastButton != null)
            {
                loadLastButton.onClick.AddListener(OnLoadLast);
            }
            if (showCurrentButton != null)
            {
                showCurrentButton.onClick.AddListener(OnShowCurrent);
            }

            InGameLogger.Log($"初期状態: {gameState}", LogColor.White);
            InGameLogger.Log("状態を変更してからセーブ・ロードを試してください", LogColor.Yellow);
        }

        /// <summary>レベルアップを実行する</summary>
        private void OnLevelUp()
        {
            gameState.Level++;
            gameState.Hp += HpPerLevel;
            InGameLogger.Log("--- レベルアップ! ---", LogColor.Yellow);
            InGameLogger.Log($"  Lv.{gameState.Level - 1} → Lv.{gameState.Level} (HP+{HpPerLevel})", LogColor.Orange);
            InGameLogger.Log($"  現在: {gameState}", LogColor.White);
        }

        /// <summary>ダメージを受ける</summary>
        private void OnTakeDamage()
        {
            int previousHp = gameState.Hp;
            gameState.Hp -= DamageAmount;
            if (gameState.Hp < 0)
            {
                gameState.Hp = 0;
            }
            InGameLogger.Log("--- ダメージを受けた! ---", LogColor.Yellow);
            InGameLogger.Log($"  HP: {previousHp} → {gameState.Hp} (-{DamageAmount})", LogColor.Red);
            InGameLogger.Log($"  現在: {gameState}", LogColor.White);
        }

        /// <summary>ゴールドを獲得する</summary>
        private void OnEarnGold()
        {
            gameState.Gold += GoldAmount;
            InGameLogger.Log("--- ゴールド獲得! ---", LogColor.Yellow);
            InGameLogger.Log($"  Gold: +{GoldAmount} (合計: {gameState.Gold})", LogColor.Orange);
            InGameLogger.Log($"  現在: {gameState}", LogColor.White);
        }

        /// <summary>現在の状態をセーブする</summary>
        private void OnSave()
        {
            GameStateMemento memento = gameState.Save();
            caretaker.SaveState(memento);
            int slotIndex = caretaker.GetSaveCount() - 1;
            InGameLogger.Log("--- セーブ実行 ---", LogColor.Yellow);
            InGameLogger.Log($"  スロット[{slotIndex}]に保存: {memento}", LogColor.Orange);
        }

        /// <summary>最後のセーブデータをロードする</summary>
        private void OnLoadLast()
        {
            int lastIndex = caretaker.GetSaveCount() - 1;
            if (lastIndex < 0)
            {
                InGameLogger.Log("  セーブデータがありません", LogColor.Red);
                return;
            }

            GameStateMemento memento = caretaker.LoadState(lastIndex);
            InGameLogger.Log("--- ロード実行 ---", LogColor.Yellow);
            InGameLogger.Log($"  ロード前: {gameState}", LogColor.White);
            gameState.Restore(memento);
            InGameLogger.Log($"  ロード後: {gameState}", LogColor.Orange);
        }

        /// <summary>現在のゲーム状態を表示する</summary>
        private void OnShowCurrent()
        {
            InGameLogger.Log("--- 現在の状態 ---", LogColor.Yellow);
            InGameLogger.Log($"  {gameState}", CategoryColor);
            InGameLogger.Log($"  セーブデータ数: {caretaker.GetSaveCount()}", LogColor.White);
        }
    }
}
