using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Structural.Composite {
    /// <summary>
    /// ファイルシステムの構成要素（Component）
    ///
    /// 【Compositeパターンの意図】
    /// 個々のオブジェクト（ファイル）とその集合（フォルダ）を同一視し、
    /// ツリー構造で再帰的に操作できるようにする
    /// </summary>
    public abstract class FileSystemComponent {
        /// <summary>コンポーネントの名前</summary>
        public string Name { get; }

        /// <summary>
        /// コンポーネントを生成する
        /// </summary>
        /// <param name="name">名前</param>
        protected FileSystemComponent(string name) {
            Name = name;
        }

        /// <summary>
        /// コンポーネントのサイズを返す
        /// </summary>
        /// <returns>サイズ（バイト）</returns>
        public abstract int GetSize();

        /// <summary>
        /// ツリー構造を文字列で表示する
        /// </summary>
        /// <param name="indent">インデント</param>
        public abstract void Display(string indent);
    }

    /// <summary>
    /// ファイル（Leaf）
    /// 子を持たない末端要素
    /// </summary>
    public sealed class File : FileSystemComponent {
        /// <summary>ファイルサイズ</summary>
        private readonly int size;

        /// <summary>
        /// ファイルを生成する
        /// </summary>
        /// <param name="name">ファイル名</param>
        /// <param name="size">ファイルサイズ</param>
        public File(string name, int size) : base(name) {
            this.size = size;
        }

        /// <inheritdoc/>
        public override int GetSize() {
            return size;
        }

        /// <inheritdoc/>
        public override void Display(string indent) {
            InGameLogger.Log($"{indent}📄 {Name} ({size} bytes)", LogColor.White);
        }
    }

    /// <summary>
    /// フォルダ（Composite）
    /// 子要素を保持し、再帰的に操作を委譲する
    /// </summary>
    public sealed class Folder : FileSystemComponent {
        /// <summary>子要素のリスト</summary>
        private readonly List<FileSystemComponent> children = new List<FileSystemComponent>();

        /// <summary>
        /// フォルダを生成する
        /// </summary>
        /// <param name="name">フォルダ名</param>
        public Folder(string name) : base(name) {
        }

        /// <summary>
        /// 子要素を追加する
        /// </summary>
        /// <param name="component">追加する子要素</param>
        public void Add(FileSystemComponent component) {
            children.Add(component);
        }

        /// <inheritdoc/>
        public override int GetSize() {
            int total = 0;
            for (int i = 0; i < children.Count; i++) {
                total += children[i].GetSize();
            }
            return total;
        }

        /// <inheritdoc/>
        public override void Display(string indent) {
            InGameLogger.Log($"{indent}📁 {Name}/ ({GetSize()} bytes)", LogColor.Green);
            for (int i = 0; i < children.Count; i++) {
                children[i].Display(indent + "  ");
            }
        }
    }
}
