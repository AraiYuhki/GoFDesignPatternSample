using System.Collections.Generic;
using System.Text;

namespace GoFPatterns.Patterns {
    // ---- Component interface ----

    /// <summary>
    /// Compositeパターンのコンポーネントインターフェース
    /// ファイルとディレクトリを統一的に扱うための共通契約
    /// </summary>
    public interface IFileSystemEntry {
        /// <summary>
        /// エントリの名前を取得する
        /// </summary>
        /// <returns>エントリ名</returns>
        string GetName();

        /// <summary>
        /// エントリのサイズを取得する（ディレクトリの場合は子の合計）
        /// </summary>
        /// <returns>サイズ（バイト単位）</returns>
        int GetSize();
    }

    // ---- Leaf ----

    /// <summary>
    /// Compositeパターンのリーフ
    /// 子を持たない単一ファイルを表す
    /// </summary>
    public class FileEntry : IFileSystemEntry {
        /// <summary>ファイル名</summary>
        private readonly string name;

        /// <summary>ファイルサイズ（バイト）</summary>
        private readonly int size;

        /// <summary>
        /// FileEntryを生成する
        /// </summary>
        /// <param name="name">ファイル名</param>
        /// <param name="size">ファイルサイズ（バイト）</param>
        public FileEntry(string name, int size) {
            this.name = name;
            this.size = size;
        }

        /// <summary>
        /// ファイル名を取得する
        /// </summary>
        /// <returns>ファイル名</returns>
        public string GetName() {
            return name;
        }

        /// <summary>
        /// ファイルサイズを取得する
        /// </summary>
        /// <returns>ファイルサイズ（バイト）</returns>
        public int GetSize() {
            return size;
        }
    }

    // ---- Composite ----

    /// <summary>
    /// Compositeパターンのコンポジット
    /// 子エントリを持つディレクトリを表す
    /// </summary>
    public class DirectoryEntry : IFileSystemEntry {
        /// <summary>ディレクトリ名</summary>
        private readonly string name;

        /// <summary>子エントリのリスト</summary>
        private readonly List<IFileSystemEntry> children = new List<IFileSystemEntry>();

        /// <summary>子エントリの数を取得する</summary>
        public int ChildCount => children.Count;

        /// <summary>
        /// DirectoryEntryを生成する
        /// </summary>
        /// <param name="name">ディレクトリ名</param>
        public DirectoryEntry(string name) {
            this.name = name;
        }

        /// <summary>
        /// ディレクトリ名を取得する
        /// </summary>
        /// <returns>ディレクトリ名</returns>
        public string GetName() {
            return name;
        }

        /// <summary>
        /// ディレクトリ全体のサイズを取得する（子エントリの合計を再帰的に計算）
        /// </summary>
        /// <returns>合計サイズ（バイト）</returns>
        public int GetSize() {
            int total = 0;
            foreach (IFileSystemEntry child in children) {
                total += child.GetSize();
            }
            return total;
        }

        /// <summary>
        /// 子エントリを追加する
        /// </summary>
        /// <param name="entry">追加するエントリ</param>
        public void Add(IFileSystemEntry entry) {
            children.Add(entry);
        }

        /// <summary>
        /// ディレクトリツリーを文字列として構築する
        /// </summary>
        /// <param name="indent">インデントの深さ</param>
        /// <returns>ツリー構造を表す文字列</returns>
        public string ToTreeString(int indent = 0) {
            var builder = new StringBuilder();
            string prefix = new string(' ', indent * 2);
            builder.Append($"{prefix}[Dir] {name}/ ({GetSize()} bytes)");
            foreach (IFileSystemEntry child in children) {
                builder.Append("\n");
                if (child is DirectoryEntry dir) {
                    builder.Append(dir.ToTreeString(indent + 1));
                } else {
                    builder.Append($"{prefix}  [File] {child.GetName()} ({child.GetSize()} bytes)");
                }
            }
            return builder.ToString();
        }
    }

    // ---- Demo ----

    /// <summary>
    /// Compositeパターンのデモ
    /// ファイルとディレクトリを同一のインターフェースで扱い、再帰的なサイズ計算を示す
    /// </summary>
    [PatternDemo("composite")]
    public class CompositeDemo : BasePatternDemo {
        /// <summary>デモのパターンID</summary>
        public override string PatternId => "composite";

        /// <summary>デモの表示名</summary>
        public override string DisplayName => "Composite";

        /// <summary>ルートディレクトリ</summary>
        private DirectoryEntry root;

        /// <summary>サブディレクトリ</summary>
        private DirectoryEntry subDir;

        /// <summary>ファイルA</summary>
        private FileEntry fileA;

        /// <summary>ファイルB</summary>
        private FileEntry fileB;

        /// <summary>サブディレクトリ内のファイルC</summary>
        private FileEntry fileC;

        /// <summary>ファイルAのサイズ（バイト）</summary>
        private const int FileASize = 100;

        /// <summary>ファイルBのサイズ（バイト）</summary>
        private const int FileBSize = 250;

        /// <summary>ファイルCのサイズ（バイト）</summary>
        private const int FileCSize = 400;

        /// <summary>
        /// リセット時にドメインオブジェクトをクリアする
        /// </summary>
        protected override void OnReset() {
            root = null;
            subDir = null;
            fileA = null;
            fileB = null;
            fileC = null;
        }

        /// <summary>
        /// Compositeパターンのシナリオを構築する
        /// </summary>
        /// <param name="scenario">ステップを追加するシナリオ</param>
        protected override void BuildScenario(DemoScenario scenario) {
            scenario.AddStep(new DemoStep(
                "ファイルを作成する",
                () => {
                    fileA = new FileEntry("readme.txt", FileASize);
                    fileB = new FileEntry("data.csv", FileBSize);
                    Log("Client", "FileEntry を生成", $"{fileA.GetName()}({FileASize}B), {fileB.GetName()}({FileBSize}B)");
                }
            ));

            scenario.AddStep(new DemoStep(
                "ルートディレクトリを作成する",
                () => {
                    root = new DirectoryEntry("root");
                    Log("Client", "new DirectoryEntry(\"root\")", $"子要素数: {root.ChildCount}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "ルートディレクトリにファイルを追加する",
                () => {
                    root.Add(fileA);
                    root.Add(fileB);
                    Log("root", "Add(readme.txt), Add(data.csv)", $"子要素数: {root.ChildCount}, サイズ: {root.GetSize()}B");
                }
            ));

            scenario.AddStep(new DemoStep(
                "サブディレクトリを作成しファイルを追加する",
                () => {
                    subDir = new DirectoryEntry("images");
                    fileC = new FileEntry("photo.png", FileCSize);
                    subDir.Add(fileC);
                    Log("Client", "images/ に photo.png を追加", $"images サイズ: {subDir.GetSize()}B");
                }
            ));

            scenario.AddStep(new DemoStep(
                "サブディレクトリをルートにネストする",
                () => {
                    root.Add(subDir);
                    Log("root", "Add(images/)", $"子要素数: {root.ChildCount}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "ルートディレクトリのGetSize()で合計を再帰計算する",
                () => {
                    int totalSize = root.GetSize();
                    int expectedTotal = FileASize + FileBSize + FileCSize;
                    Log("root", "GetSize()", $"合計: {totalSize}B (期待値: {expectedTotal}B)");
                }
            ));

            scenario.AddStep(new DemoStep(
                "ディレクトリツリーを表示する",
                () => {
                    string tree = root.ToTreeString();
                    Log("root", "ToTreeString()", tree);
                }
            ));
        }
    }
}
