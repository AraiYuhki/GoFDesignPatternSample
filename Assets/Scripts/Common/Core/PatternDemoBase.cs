using DesignPatterns.NodeGraph;
using UnityEngine;

namespace DesignPatterns
{
    /// <summary>
    /// 各デザインパターンのデモシーンで使用する基底クラス
    /// パターン情報の表示とログの初期化を共通化する
    /// </summary>
    public abstract class PatternDemoBase : MonoBehaviour
    {
        /// <summary>パターン情報を表示するパネル</summary>
        [SerializeField]
        private PatternInfoPanel infoPanel;

        /// <summary>ノードグラフ表示用のビュー（オプション）</summary>
        [SerializeField]
        private NodeGraphView nodeGraphView;

        /// <summary>パターン名を返す</summary>
        protected abstract string PatternName { get; }

        /// <summary>パターンのカテゴリを返す</summary>
        protected abstract PatternCategory Category { get; }

        /// <summary>パターンの概要説明を返す</summary>
        protected abstract string Description { get; }

        /// <summary>ノードグラフビューへのアクセサ</summary>
        protected NodeGraphView GraphView => nodeGraphView;

        /// <summary>
        /// カテゴリに対応するログの色を返す
        /// </summary>
        protected LogColor CategoryColor
        {
            get
            {
                switch (Category)
                {
                    case PatternCategory.Creational:
                        return LogColor.Blue;
                    case PatternCategory.Structural:
                        return LogColor.Green;
                    case PatternCategory.Behavioral:
                        return LogColor.Orange;
                    default:
                        return LogColor.White;
                }
            }
        }

        protected virtual void Start()
        {
            InitializeDemo();
        }

        /// <summary>
        /// デモの初期化処理を行う
        /// パターン情報パネルの設定とウェルカムログを表示する
        /// </summary>
        private void InitializeDemo()
        {
            if (infoPanel != null)
            {
                infoPanel.SetInfo(PatternName, GetCategoryDisplayName(), Description);
            }

            InGameLogger.Clear();
            InGameLogger.Log($"=== {PatternName} パターン ===", CategoryColor);
            InGameLogger.Log(Description, LogColor.White);
            InGameLogger.Log("---", LogColor.White);

            OnDemoStart();
        }

        /// <summary>
        /// デモ固有の初期化処理
        /// サブクラスでオーバーライドして使用する
        /// </summary>
        protected virtual void OnDemoStart()
        {
        }

        /// <summary>
        /// GraphDataを生成し、NodeGraphViewに初期化・カテゴリ色を設定して返す
        /// GraphViewがnullの場合はGraphDataのみ返す
        /// </summary>
        /// <param name="strategy">レイアウト戦略（省略時はManualLayoutStrategy）</param>
        /// <returns>生成したGraphData</returns>
        protected GraphData CreateGraphData(IGraphLayoutStrategy strategy = null)
        {
            var data = new GraphData();
            if (nodeGraphView != null)
            {
                nodeGraphView.SetCategoryColor(GetCategoryUnityColor());
                nodeGraphView.Initialize(data, strategy);
            }
            return data;
        }

        /// <summary>
        /// カテゴリに対応するUnityのColorを返す
        /// </summary>
        /// <returns>カテゴリ色</returns>
        private Color GetCategoryUnityColor()
        {
            switch (Category)
            {
                case PatternCategory.Creational:
                    return new Color(0.36f, 0.61f, 0.84f, 1f);
                case PatternCategory.Structural:
                    return new Color(0.44f, 0.68f, 0.28f, 1f);
                case PatternCategory.Behavioral:
                    return new Color(0.93f, 0.49f, 0.19f, 1f);
                default:
                    return Color.white;
            }
        }

        /// <summary>
        /// カテゴリの表示名を返す
        /// </summary>
        /// <returns>カテゴリの日本語表示名</returns>
        private string GetCategoryDisplayName()
        {
            switch (Category)
            {
                case PatternCategory.Creational:
                    return "生成パターン (Creational)";
                case PatternCategory.Structural:
                    return "構造パターン (Structural)";
                case PatternCategory.Behavioral:
                    return "振る舞いパターン (Behavioral)";
                default:
                    return "不明";
            }
        }
    }
}
