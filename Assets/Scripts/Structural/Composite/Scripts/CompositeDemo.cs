using UnityEngine;
using UnityEngine.UI;

namespace DesignPatterns.Structural.Composite
{
    /// <summary>
    /// Compositeパターンのデモシーンを制御するクラス
    ///
    /// 【デモの内容】
    /// - フォルダとファイルのツリー構造を構築
    /// - フォルダのサイズが子要素の合計として再帰的に計算されることを確認
    /// - ツリー構造の表示で再帰的な処理を体感
    /// </summary>
    public sealed class CompositeDemo : PatternDemoBase
    {
        /// <summary>ツリー構造表示ボタン</summary>
        [SerializeField]
        private Button showTreeButton;

        /// <summary>サイズ計算ボタン</summary>
        [SerializeField]
        private Button calcSizeButton;

        /// <summary>ファイル追加ボタン</summary>
        [SerializeField]
        private Button addFileButton;

        /// <summary>ルートフォルダ</summary>
        private Folder root;

        /// <summary>ファイル追加先フォルダ</summary>
        private Folder documentsFolder;

        /// <summary>追加ファイルのカウンター</summary>
        private int fileCounter;

        /// <inheritdoc/>
        protected override string PatternName
        {
            get { return "Composite"; }
        }

        /// <inheritdoc/>
        protected override PatternCategory Category
        {
            get { return PatternCategory.Structural; }
        }

        /// <inheritdoc/>
        protected override string Description
        {
            get { return "個々のオブジェクトとその集合を同一視し、ツリー構造で再帰的に操作する"; }
        }

        /// <inheritdoc/>
        protected override void OnDemoStart()
        {
            BuildFileTree();

            if (showTreeButton != null)
            {
                showTreeButton.onClick.AddListener(OnShowTree);
            }
            if (calcSizeButton != null)
            {
                calcSizeButton.onClick.AddListener(OnCalcSize);
            }
            if (addFileButton != null)
            {
                addFileButton.onClick.AddListener(OnAddFile);
            }

            InGameLogger.Log("ツリー構造を表示してCompositeの動作を確認してください", LogColor.Yellow);
        }

        /// <summary>
        /// サンプルのファイルツリーを構築する
        /// </summary>
        private void BuildFileTree()
        {
            root = new Folder("root");

            documentsFolder = new Folder("Documents");
            documentsFolder.Add(new File("readme.txt", 1024));
            documentsFolder.Add(new File("design.pdf", 5120));

            var imagesFolder = new Folder("Images");
            imagesFolder.Add(new File("photo.png", 20480));
            imagesFolder.Add(new File("icon.svg", 2048));

            var srcFolder = new Folder("src");
            srcFolder.Add(new File("main.cs", 4096));
            srcFolder.Add(new File("utils.cs", 2048));

            root.Add(documentsFolder);
            root.Add(imagesFolder);
            root.Add(srcFolder);
            root.Add(new File("config.json", 512));
        }

        /// <summary>
        /// ツリー構造を表示する
        /// </summary>
        private void OnShowTree()
        {
            InGameLogger.Log("=== ファイルツリー ===", LogColor.Yellow);
            root.Display("");
        }

        /// <summary>
        /// ルートのサイズを再帰的に計算する
        /// </summary>
        private void OnCalcSize()
        {
            InGameLogger.Log("=== サイズ計算（再帰） ===", LogColor.Yellow);
            InGameLogger.Log($"root全体のサイズ: {root.GetSize()} bytes", LogColor.Green);
            InGameLogger.Log("→ フォルダも単一ファイルも同じGetSize()で統一的に扱える", LogColor.Green);
        }

        /// <summary>
        /// Documentsフォルダにファイルを追加する
        /// </summary>
        private void OnAddFile()
        {
            fileCounter++;
            int size = 1024 * fileCounter;
            string fileName = $"new_file_{fileCounter}.txt";
            documentsFolder.Add(new File(fileName, size));
            InGameLogger.Log($"Documents/ に {fileName} ({size} bytes) を追加しました", LogColor.Green);
        }
    }
}
