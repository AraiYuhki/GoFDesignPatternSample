using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace DesignPatterns.Behavioral.ChainOfResponsibility
{
    /// <summary>
    /// Chain of Responsibilityパターンのデモシーンを制御するクラス
    ///
    /// 【デモの内容】
    /// - 攻撃ダメージがDodge→Armor→Resistanceの順に処理される
    /// - 各ハンドラがダメージを軽減する様子をログで確認できる
    /// - 異なるダメージ量で連鎖処理の効果を比較できる
    /// </summary>
    public sealed class ChainOfResponsibilityDemo : PatternDemoBase
    {
        /// <summary>ダメージ10の攻撃ボタン</summary>
        [SerializeField]
        private Button attack10Button;

        /// <summary>ダメージ30の攻撃ボタン</summary>
        [SerializeField]
        private Button attack30Button;

        /// <summary>ダメージ100の攻撃ボタン</summary>
        [SerializeField]
        private Button attack100Button;

        /// <summary>チェーン構成表示ボタン</summary>
        [SerializeField]
        private Button showChainButton;

        /// <summary>ダメージ処理チェーンの先頭ハンドラ</summary>
        private DamageHandler chainHead;

        /// <summary>回避ハンドラ</summary>
        private DodgeHandler dodgeHandler;

        /// <summary>防具ハンドラ</summary>
        private ArmorHandler armorHandler;

        /// <summary>耐性ハンドラ</summary>
        private ResistanceHandler resistanceHandler;

        /// <summary>ログ生成用のStringBuilder</summary>
        private readonly StringBuilder logBuilder = new StringBuilder();

        /// <inheritdoc/>
        protected override string PatternName
        {
            get { return "Chain of Responsibility"; }
        }

        /// <inheritdoc/>
        protected override PatternCategory Category
        {
            get { return PatternCategory.Behavioral; }
        }

        /// <inheritdoc/>
        protected override string Description
        {
            get { return "要求を処理するオブジェクトの連鎖を作り、要求を順次処理する"; }
        }

        /// <inheritdoc/>
        protected override void OnDemoStart()
        {
            BuildChain();

            if (attack10Button != null)
            {
                attack10Button.onClick.AddListener(OnAttack10);
            }
            if (attack30Button != null)
            {
                attack30Button.onClick.AddListener(OnAttack30);
            }
            if (attack100Button != null)
            {
                attack100Button.onClick.AddListener(OnAttack100);
            }
            if (showChainButton != null)
            {
                showChainButton.onClick.AddListener(OnShowChain);
            }

            InGameLogger.Log("攻撃ボタンを押してダメージ処理の連鎖を確認してください", LogColor.Yellow);
        }

        /// <summary>
        /// ダメージ処理チェーンを構築する
        /// Dodge → Armor → Resistance の順にハンドラを連結する
        /// </summary>
        private void BuildChain()
        {
            dodgeHandler = new DodgeHandler();
            armorHandler = new ArmorHandler();
            resistanceHandler = new ResistanceHandler();

            dodgeHandler.SetNext(armorHandler).SetNext(resistanceHandler);
            chainHead = dodgeHandler;
        }

        /// <summary>ダメージ10の攻撃を実行する</summary>
        private void OnAttack10()
        {
            ProcessAttack(10);
        }

        /// <summary>ダメージ30の攻撃を実行する</summary>
        private void OnAttack30()
        {
            ProcessAttack(30);
        }

        /// <summary>ダメージ100の攻撃を実行する</summary>
        private void OnAttack100()
        {
            ProcessAttack(100);
        }

        /// <summary>
        /// 攻撃ダメージをチェーンで処理し、結果をログに表示する
        /// </summary>
        /// <param name="damage">元のダメージ量</param>
        private void ProcessAttack(int damage)
        {
            InGameLogger.Log($"--- 攻撃! 元ダメージ: {damage} ---", LogColor.Yellow);
            int finalDamage = chainHead.Handle(damage);
            InGameLogger.Log($"  最終ダメージ: {finalDamage} ({damage} → {finalDamage})", CategoryColor);
        }

        /// <summary>
        /// 現在のチェーン構成をログに表示する
        /// </summary>
        private void OnShowChain()
        {
            InGameLogger.Log("--- チェーン構成 ---", LogColor.Yellow);
            logBuilder.Clear();
            logBuilder.Append("  ");
            logBuilder.Append(dodgeHandler.HandlerName);
            logBuilder.Append(" → ");
            logBuilder.Append(armorHandler.HandlerName);
            logBuilder.Append(" → ");
            logBuilder.Append(resistanceHandler.HandlerName);
            InGameLogger.Log(logBuilder.ToString(), CategoryColor);
            InGameLogger.Log("  Dodge: 20%で全回避 → Armor: 30%カット → Resistance: 固定5軽減", LogColor.White);
        }
    }
}
