using UnityEngine;
using UnityEngine.UI;

namespace DesignPatterns.Structural.Adapter
{
    /// <summary>
    /// Adapterパターンのデモシーンを制御するクラス
    ///
    /// 【デモの内容】
    /// - 旧式サウンドシステムをアダプター経由で新インターフェースから操作
    /// - メソッド名・引数の変換過程をログで可視化
    /// - アダプターが仲介することで互換性のないシステムが連携することを確認
    /// </summary>
    public sealed class AdapterDemo : PatternDemoBase
    {
        /// <summary>バトルBGM再生ボタン</summary>
        [SerializeField]
        private Button playBattleButton;

        /// <summary>フィールドBGM再生ボタン</summary>
        [SerializeField]
        private Button playFieldButton;

        /// <summary>停止ボタン</summary>
        [SerializeField]
        private Button stopButton;

        /// <summary>状態確認ボタン</summary>
        [SerializeField]
        private Button statusButton;

        /// <summary>アダプターを適用した新インターフェース</summary>
        private IModernAudio audioSystem;

        /// <inheritdoc/>
        protected override string PatternName
        {
            get { return "Adapter"; }
        }

        /// <inheritdoc/>
        protected override PatternCategory Category
        {
            get { return PatternCategory.Structural; }
        }

        /// <inheritdoc/>
        protected override string Description
        {
            get { return "互換性のないインターフェースを持つクラスを、既存のインターフェースに適合させる"; }
        }

        /// <inheritdoc/>
        protected override void OnDemoStart()
        {
            var legacySystem = new LegacySoundSystem();
            audioSystem = new SoundAdapter(legacySystem);

            if (playBattleButton != null)
            {
                playBattleButton.onClick.AddListener(OnPlayBattle);
            }
            if (playFieldButton != null)
            {
                playFieldButton.onClick.AddListener(OnPlayField);
            }
            if (stopButton != null)
            {
                stopButton.onClick.AddListener(OnStop);
            }
            if (statusButton != null)
            {
                statusButton.onClick.AddListener(OnCheckStatus);
            }

            InGameLogger.Log("新インターフェース経由で旧システムを操作します", LogColor.Yellow);
        }

        /// <summary>
        /// バトルBGMを再生する
        /// </summary>
        private void OnPlayBattle()
        {
            InGameLogger.Log("--- バトルBGM再生 ---", LogColor.Yellow);
            audioSystem.Play("BGM_Battle", 0.8f);
        }

        /// <summary>
        /// フィールドBGMを再生する
        /// </summary>
        private void OnPlayField()
        {
            InGameLogger.Log("--- フィールドBGM再生 ---", LogColor.Yellow);
            audioSystem.Play("BGM_Field", 0.6f);
        }

        /// <summary>
        /// 再生を停止する
        /// </summary>
        private void OnStop()
        {
            InGameLogger.Log("--- 停止 ---", LogColor.Yellow);
            audioSystem.Stop();
        }

        /// <summary>
        /// 現在のステータスを確認する
        /// </summary>
        private void OnCheckStatus()
        {
            string status = audioSystem.GetStatus();
            InGameLogger.Log($"状態: {status}", LogColor.Green);
        }
    }
}
