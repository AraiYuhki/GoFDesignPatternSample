using UnityEngine;
using UnityEngine.UI;

namespace DesignPatterns.Structural.Facade
{
    /// <summary>
    /// Facadeパターンのデモシーンを制御するクラス
    ///
    /// 【デモの内容】
    /// - ワンクリックで複数サブシステムの初期化を一括実行
    /// - 内部で呼ばれるサブシステムの処理をログで可視化
    /// - ファサードなしとありの比較
    /// </summary>
    public sealed class FacadeDemo : PatternDemoBase
    {
        /// <summary>フル初期化ボタン</summary>
        [SerializeField]
        private Button fullInitButton;

        /// <summary>クイックスタートボタン</summary>
        [SerializeField]
        private Button quickStartButton;

        /// <summary>ファサード</summary>
        private GameInitFacade facade;

        /// <inheritdoc/>
        protected override string PatternName
        {
            get { return "Facade"; }
        }

        /// <inheritdoc/>
        protected override PatternCategory Category
        {
            get { return PatternCategory.Structural; }
        }

        /// <inheritdoc/>
        protected override string Description
        {
            get { return "複雑なサブシステム群への統一的なインターフェースを提供し、使いやすくする"; }
        }

        /// <inheritdoc/>
        protected override void OnDemoStart()
        {
            facade = new GameInitFacade();

            if (fullInitButton != null)
            {
                fullInitButton.onClick.AddListener(OnFullInit);
            }
            if (quickStartButton != null)
            {
                quickStartButton.onClick.AddListener(OnQuickStart);
            }

            InGameLogger.Log("ボタンを押して初期化処理の流れを確認してください", LogColor.Yellow);
        }

        /// <summary>
        /// フル初期化を実行する
        /// </summary>
        private void OnFullInit()
        {
            InGameLogger.Log("=== フル初期化（ファサード経由） ===", LogColor.Yellow);
            facade.InitializeGame();
            InGameLogger.Log("✓ 全初期化完了！", LogColor.Green);
        }

        /// <summary>
        /// クイックスタートを実行する
        /// </summary>
        private void OnQuickStart()
        {
            InGameLogger.Log("=== クイックスタート（ファサード経由） ===", LogColor.Yellow);
            facade.QuickStart();
            InGameLogger.Log("✓ クイック初期化完了！", LogColor.Green);
        }
    }
}
