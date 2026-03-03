using UnityEngine;
using UnityEngine.UI;

namespace DesignPatterns.Behavioral.State
{
    /// <summary>
    /// Stateパターンのデモシーンを制御するクラス
    ///
    /// 【デモの内容】
    /// - キャラクターの状態（待機・移動・攻撃・被弾）を入力で切り替え
    /// - 各状態で異なる振る舞い（Enter/Update）を確認
    /// - 状態遷移のロジックが各状態オブジェクトに委譲されることを体験する
    /// </summary>
    public sealed class StateDemo : PatternDemoBase
    {
        /// <summary>移動入力ボタン</summary>
        [SerializeField]
        private Button moveButton;

        /// <summary>攻撃入力ボタン</summary>
        [SerializeField]
        private Button attackButton;

        /// <summary>ダメージ入力ボタン</summary>
        [SerializeField]
        private Button damageButton;

        /// <summary>待機入力ボタン</summary>
        [SerializeField]
        private Button idleButton;

        /// <summary>更新実行ボタン</summary>
        [SerializeField]
        private Button updateButton;

        /// <summary>キャラクターのステートマシン</summary>
        private CharacterStateMachine stateMachine;

        /// <inheritdoc/>
        protected override string PatternName
        {
            get { return "State"; }
        }

        /// <inheritdoc/>
        protected override PatternCategory Category
        {
            get { return PatternCategory.Behavioral; }
        }

        /// <inheritdoc/>
        protected override string Description
        {
            get { return "オブジェクトの内部状態に応じて振る舞いを変更する。状態をオブジェクトとして表現する"; }
        }

        /// <inheritdoc/>
        protected override void OnDemoStart()
        {
            stateMachine = new CharacterStateMachine(new IdleState());

            if (moveButton != null)
            {
                moveButton.onClick.AddListener(OnMoveInput);
            }
            if (attackButton != null)
            {
                attackButton.onClick.AddListener(OnAttackInput);
            }
            if (damageButton != null)
            {
                damageButton.onClick.AddListener(OnDamageInput);
            }
            if (idleButton != null)
            {
                idleButton.onClick.AddListener(OnIdleInput);
            }
            if (updateButton != null)
            {
                updateButton.onClick.AddListener(OnUpdate);
            }

            InGameLogger.Log("入力ボタンで状態を切り替え、Updateで現在の状態の動作を確認してください", LogColor.Yellow);
        }

        /// <summary>移動入力を処理する</summary>
        private void OnMoveInput()
        {
            stateMachine.ProcessInput("move");
        }

        /// <summary>攻撃入力を処理する</summary>
        private void OnAttackInput()
        {
            stateMachine.ProcessInput("attack");
        }

        /// <summary>ダメージ入力を処理する</summary>
        private void OnDamageInput()
        {
            stateMachine.ProcessInput("damage");
        }

        /// <summary>待機入力を処理する</summary>
        private void OnIdleInput()
        {
            stateMachine.ProcessInput("idle");
        }

        /// <summary>現在の状態のUpdate処理を実行する</summary>
        private void OnUpdate()
        {
            InGameLogger.Log($"--- Update ({stateMachine.CurrentStateName}) ---", LogColor.Yellow);
            stateMachine.UpdateState();
        }
    }
}
