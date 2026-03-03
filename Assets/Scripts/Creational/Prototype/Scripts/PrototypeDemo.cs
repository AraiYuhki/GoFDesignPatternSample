using UnityEngine;
using UnityEngine.UI;

namespace DesignPatterns.Creational.Prototype
{
    /// <summary>
    /// Prototypeパターンのデモシーンを制御するクラス
    ///
    /// 【デモの内容】
    /// - プロトタイプからユニットを複製
    /// - 複製後にプロパティを変更し、オリジナルに影響がないことを確認
    /// - 浅いコピーと深いコピーの違いを体験
    /// </summary>
    public sealed class PrototypeDemo : PatternDemoBase
    {
        /// <summary>兵士の複製ボタン</summary>
        [SerializeField]
        private Button cloneSoldierButton;

        /// <summary>弓兵の複製ボタン</summary>
        [SerializeField]
        private Button cloneArcherButton;

        /// <summary>複製したユニットを変更するボタン</summary>
        [SerializeField]
        private Button modifyCloneButton;

        /// <summary>オリジナルと複製の比較ボタン</summary>
        [SerializeField]
        private Button compareButton;

        /// <summary>兵士のプロトタイプ</summary>
        private SoldierUnit soldierPrototype;

        /// <summary>弓兵のプロトタイプ</summary>
        private ArcherUnit archerPrototype;

        /// <summary>最後に複製されたユニット</summary>
        private UnitPrototype lastClone;

        /// <summary>最後に複製されたユニットのオリジナル</summary>
        private UnitPrototype lastOriginal;

        /// <inheritdoc/>
        protected override string PatternName
        {
            get { return "Prototype"; }
        }

        /// <inheritdoc/>
        protected override PatternCategory Category
        {
            get { return PatternCategory.Creational; }
        }

        /// <inheritdoc/>
        protected override string Description
        {
            get { return "既存のオブジェクトをプロトタイプとしてコピーし、新しいオブジェクトを生成する"; }
        }

        /// <inheritdoc/>
        protected override void OnDemoStart()
        {
            soldierPrototype = new SoldierUnit
            {
                Name = "兵士",
                Hp = 100,
                Attack = 20,
                Level = 5,
                WeaponName = "鉄の剣"
            };

            archerPrototype = new ArcherUnit
            {
                Name = "弓兵",
                Hp = 70,
                Attack = 25,
                Level = 3,
                Range = 10
            };

            if (cloneSoldierButton != null)
            {
                cloneSoldierButton.onClick.AddListener(OnCloneSoldier);
            }
            if (cloneArcherButton != null)
            {
                cloneArcherButton.onClick.AddListener(OnCloneArcher);
            }
            if (modifyCloneButton != null)
            {
                modifyCloneButton.onClick.AddListener(OnModifyClone);
            }
            if (compareButton != null)
            {
                compareButton.onClick.AddListener(OnCompare);
            }

            InGameLogger.Log("プロトタイプを複製して独立したオブジェクトを作成してみましょう", LogColor.Yellow);
            InGameLogger.Log($"原型(兵士): {soldierPrototype}", LogColor.White);
            InGameLogger.Log($"原型(弓兵): {archerPrototype}", LogColor.White);
        }

        /// <summary>
        /// 兵士プロトタイプを複製する
        /// </summary>
        private void OnCloneSoldier()
        {
            InGameLogger.Log("--- 兵士を複製 ---", LogColor.Yellow);
            lastOriginal = soldierPrototype;
            lastClone = soldierPrototype.DeepClone();
            lastClone.Name = "兵士(複製)";
            InGameLogger.Log($"複製完了: {lastClone}", LogColor.Blue);
        }

        /// <summary>
        /// 弓兵プロトタイプを複製する
        /// </summary>
        private void OnCloneArcher()
        {
            InGameLogger.Log("--- 弓兵を複製 ---", LogColor.Yellow);
            lastOriginal = archerPrototype;
            lastClone = archerPrototype.DeepClone();
            lastClone.Name = "弓兵(複製)";
            InGameLogger.Log($"複製完了: {lastClone}", LogColor.Blue);
        }

        /// <summary>
        /// 複製したユニットのステータスを変更する
        /// </summary>
        private void OnModifyClone()
        {
            if (lastClone == null)
            {
                InGameLogger.Log("まだ複製されたユニットがありません", LogColor.Red);
                return;
            }

            InGameLogger.Log("--- 複製ユニットを強化 ---", LogColor.Yellow);
            lastClone.Level += 5;
            lastClone.Attack += 10;
            lastClone.Hp += 50;
            InGameLogger.Log($"強化後: {lastClone}", LogColor.Blue);
        }

        /// <summary>
        /// オリジナルと複製を比較する
        /// </summary>
        private void OnCompare()
        {
            if (lastClone == null || lastOriginal == null)
            {
                InGameLogger.Log("複製してから比較してください", LogColor.Red);
                return;
            }

            InGameLogger.Log("--- オリジナルと複製の比較 ---", LogColor.Yellow);
            InGameLogger.Log($"オリジナル: {lastOriginal}", LogColor.White);
            InGameLogger.Log($"複製:       {lastClone}", LogColor.Blue);
            bool isIndependent = lastOriginal.Hp != lastClone.Hp || lastOriginal.Attack != lastClone.Attack;
            if (isIndependent)
            {
                InGameLogger.Log("→ 複製は独立しており、オリジナルに影響なし", LogColor.Green);
            }
            else
            {
                InGameLogger.Log("→ 同じ値です（まだ変更されていません）", LogColor.White);
            }
        }
    }
}
