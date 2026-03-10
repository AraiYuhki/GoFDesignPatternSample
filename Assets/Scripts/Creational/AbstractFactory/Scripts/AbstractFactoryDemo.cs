using DesignPatterns.NodeGraph;
using UnityEngine;
using UnityEngine.UI;

namespace DesignPatterns.Creational.AbstractFactory
{
    /// <summary>
    /// Abstract Factoryパターンのデモシーンを制御するクラス
    ///
    /// 【デモの内容】
    /// - ダーク/ライト/レトロの3テーマを切り替え
    /// - ファクトリを切り替えるだけで関連するUI部品が一括で変更されることを確認
    /// - 各ファクトリが一貫性のある色のセットを生成することを体験
    /// </summary>
    public sealed class AbstractFactoryDemo : PatternDemoBase
    {
        /// <summary>テーマが適用されるサンプルパネル</summary>
        [SerializeField]
        private Image samplePanel;

        /// <summary>テーマが適用されるサンプルボタン群</summary>
        [SerializeField]
        private Image[] sampleButtons;

        /// <summary>ダークテーマ切り替えボタン</summary>
        [SerializeField]
        private Button darkThemeButton;

        /// <summary>ライトテーマ切り替えボタン</summary>
        [SerializeField]
        private Button lightThemeButton;

        /// <summary>レトロテーマ切り替えボタン</summary>
        [SerializeField]
        private Button retroThemeButton;

        /// <inheritdoc/>
        protected override string PatternName
        {
            get { return "Abstract Factory"; }
        }

        /// <inheritdoc/>
        protected override PatternCategory Category
        {
            get { return PatternCategory.Creational; }
        }

        /// <inheritdoc/>
        protected override string Description
        {
            get { return "関連するオブジェクト群を、具象クラスを指定せずに生成するインターフェースを提供する"; }
        }

        /// <summary>グラフデータ</summary>
        private GraphData graphData;

        /// <summary>ダークテーマファクトリのノードID</summary>
        private const string NodeIdDark = "dark";
        /// <summary>ライトテーマファクトリのノードID</summary>
        private const string NodeIdLight = "light";
        /// <summary>レトロテーマファクトリのノードID</summary>
        private const string NodeIdRetro = "retro";
        /// <summary>インターフェースのノードID</summary>
        private const string NodeIdInterface = "interface";
        /// <summary>背景色プロダクトのノードID</summary>
        private const string NodeIdBgColor = "bg";
        /// <summary>ボタン色プロダクトのノードID</summary>
        private const string NodeIdBtnColor = "btn";
        /// <summary>テキスト色プロダクトのノードID</summary>
        private const string NodeIdTxtColor = "txt";
        /// <summary>アクセント色プロダクトのノードID</summary>
        private const string NodeIdAccColor = "acc";

        /// <inheritdoc/>
        protected override void OnDemoStart()
        {
            SetupGraph();

            if (darkThemeButton != null)
            {
                darkThemeButton.onClick.AddListener(() => ApplyTheme(new DarkThemeFactory(), NodeIdDark));
            }
            if (lightThemeButton != null)
            {
                lightThemeButton.onClick.AddListener(() => ApplyTheme(new LightThemeFactory(), NodeIdLight));
            }
            if (retroThemeButton != null)
            {
                retroThemeButton.onClick.AddListener(() => ApplyTheme(new RetroThemeFactory(), NodeIdRetro));
            }

            InGameLogger.Log("テーマを選択してUI部品の一括変更を確認してください", LogColor.Yellow);
        }

        /// <summary>
        /// ノードグラフの初期構造を構築する
        /// IThemeFactory（インターフェース）→ 具象ファクトリ → プロダクト群の関係を表示する
        /// </summary>
        private void SetupGraph()
        {
            graphData = CreateGraphData();
            if (GraphView == null)
            {
                return;
            }

            // インターフェースノード（最上段）
            graphData.AddNode(new NodeData(NodeIdInterface, "IThemeFactory", "<<interface>>", new Vector2(0, 100)));

            // 具象ファクトリノード（中段）
            graphData.AddNode(new NodeData(NodeIdDark, "DarkTheme", "", new Vector2(-160, 0)));
            graphData.AddNode(new NodeData(NodeIdLight, "LightTheme", "", new Vector2(0, 0)));
            graphData.AddNode(new NodeData(NodeIdRetro, "RetroTheme", "", new Vector2(160, 0)));

            // プロダクトノード（下段）
            graphData.AddNode(new NodeData(NodeIdBgColor, "Background", "", new Vector2(-120, -100)));
            graphData.AddNode(new NodeData(NodeIdBtnColor, "Button", "", new Vector2(-40, -100)));
            graphData.AddNode(new NodeData(NodeIdTxtColor, "Text", "", new Vector2(40, -100)));
            graphData.AddNode(new NodeData(NodeIdAccColor, "Accent", "", new Vector2(120, -100)));

            // インターフェース ← 具象ファクトリ（実装関係は破線）
            var dashedStyle = EdgeStyleData.Dashed();
            graphData.AddEdge(new EdgeData(NodeIdDark, NodeIdInterface, "implements", dashedStyle));
            graphData.AddEdge(new EdgeData(NodeIdLight, NodeIdInterface, "implements", dashedStyle));
            graphData.AddEdge(new EdgeData(NodeIdRetro, NodeIdInterface, "implements", dashedStyle));

            // インターフェースノードを強調
            GraphView.UpdateNodeState(NodeIdInterface, NodeState.Highlighted);
        }

        /// <summary>
        /// 指定されたテーマファクトリを使ってUIを更新し、グラフのアニメーションを実行する
        /// </summary>
        /// <param name="factory">適用するテーマファクトリ</param>
        /// <param name="factoryNodeId">選択されたファクトリのノードID</param>
        private void ApplyTheme(IThemeFactory factory, string factoryNodeId)
        {
            InGameLogger.Log($"--- {factory.ThemeName} を適用 ---", LogColor.Yellow);

            // グラフ上で選択ファクトリをハイライトする
            UpdateFactoryHighlight(factoryNodeId);

            Color backgroundColor = factory.CreateBackgroundColor();
            Color buttonColor = factory.CreateButtonColor();
            Color textColor = factory.CreateTextColor();
            Color accentColor = factory.CreateAccentColor();

            if (samplePanel != null)
            {
                samplePanel.color = backgroundColor;
                InGameLogger.Log($"背景色を設定: {ColorToHex(backgroundColor)}", LogColor.Blue);
            }

            if (sampleButtons != null)
            {
                for (int i = 0; i < sampleButtons.Length; i++)
                {
                    if (sampleButtons[i] != null)
                    {
                        sampleButtons[i].color = buttonColor;
                    }
                }
                InGameLogger.Log($"ボタン色を設定: {ColorToHex(buttonColor)}", LogColor.Blue);
            }

            InGameLogger.Log($"テキスト色: {ColorToHex(textColor)}", LogColor.Blue);
            InGameLogger.Log($"アクセント色: {ColorToHex(accentColor)}", LogColor.Blue);
            InGameLogger.Log($"{factory.ThemeName} の適用が完了しました", LogColor.Blue);

            // プロダクトノードに生成された色を表示する
            UpdateProductNodes(backgroundColor, buttonColor, textColor, accentColor, factoryNodeId);
        }

        /// <summary>
        /// 選択中のファクトリノードをハイライトし、他のファクトリを通常状態に戻す
        /// </summary>
        /// <param name="activeFactoryId">アクティブにするファクトリのノードID</param>
        private void UpdateFactoryHighlight(string activeFactoryId)
        {
            if (GraphView == null)
            {
                return;
            }

            string[] factoryIds = { NodeIdDark, NodeIdLight, NodeIdRetro };
            foreach (string id in factoryIds)
            {
                GraphView.UpdateNodeState(id, id == activeFactoryId ? NodeState.Active : NodeState.Dimmed);
            }

            GraphView.AnimateNodePulse(activeFactoryId, new Color(0.36f, 0.61f, 0.84f, 0.6f));
        }

        /// <summary>
        /// プロダクトノードの状態テキストを更新し、生成エッジのアニメーションを実行する
        /// </summary>
        /// <param name="bgColor">背景色</param>
        /// <param name="btnColor">ボタン色</param>
        /// <param name="txtColor">テキスト色</param>
        /// <param name="accColor">アクセント色</param>
        /// <param name="factoryNodeId">生成元ファクトリのノードID</param>
        private void UpdateProductNodes(Color bgColor, Color btnColor, Color txtColor, Color accColor, string factoryNodeId)
        {
            if (GraphView == null)
            {
                return;
            }

            // 既存の生成エッジを削除する
            RemoveProductEdges();

            // プロダクトノードの状態テキストを更新する
            GraphView.UpdateNodeText(NodeIdBgColor, ColorToHex(bgColor));
            GraphView.UpdateNodeText(NodeIdBtnColor, ColorToHex(btnColor));
            GraphView.UpdateNodeText(NodeIdTxtColor, ColorToHex(txtColor));
            GraphView.UpdateNodeText(NodeIdAccColor, ColorToHex(accColor));

            // プロダクトノードをアクティブにする
            GraphView.UpdateNodeState(NodeIdBgColor, NodeState.Active);
            GraphView.UpdateNodeState(NodeIdBtnColor, NodeState.Active);
            GraphView.UpdateNodeState(NodeIdTxtColor, NodeState.Active);
            GraphView.UpdateNodeState(NodeIdAccColor, NodeState.Active);

            // ファクトリ → プロダクトの生成エッジを追加する
            graphData.AddEdge(new EdgeData(factoryNodeId, NodeIdBgColor, "creates"));
            graphData.AddEdge(new EdgeData(factoryNodeId, NodeIdBtnColor, "creates"));
            graphData.AddEdge(new EdgeData(factoryNodeId, NodeIdTxtColor, "creates"));
            graphData.AddEdge(new EdgeData(factoryNodeId, NodeIdAccColor, "creates"));

            // エッジのパルスアニメーション
            GraphView.AnimateEdgePulse(factoryNodeId, NodeIdBgColor, Color.white, 0.5f);
            GraphView.AnimateEdgePulse(factoryNodeId, NodeIdBtnColor, Color.white, 0.5f);
            GraphView.AnimateEdgePulse(factoryNodeId, NodeIdTxtColor, Color.white, 0.5f);
            GraphView.AnimateEdgePulse(factoryNodeId, NodeIdAccColor, Color.white, 0.5f);
        }

        /// <summary>
        /// 既存のファクトリ→プロダクト間のエッジを削除する
        /// </summary>
        private void RemoveProductEdges()
        {
            string[] factoryIds = { NodeIdDark, NodeIdLight, NodeIdRetro };
            string[] productIds = { NodeIdBgColor, NodeIdBtnColor, NodeIdTxtColor, NodeIdAccColor };

            foreach (string factoryId in factoryIds)
            {
                foreach (string productId in productIds)
                {
                    graphData.RemoveEdge(factoryId, productId);
                }
            }
        }

        /// <summary>
        /// Colorを16進数文字列に変換する
        /// </summary>
        /// <param name="color">変換する色</param>
        /// <returns>16進数カラーコード</returns>
        private static string ColorToHex(Color color)
        {
            return $"#{ColorUtility.ToHtmlStringRGB(color)}";
        }
    }
}
