using System.Collections.Generic;

namespace GoFPatterns.Patterns {
    // ---- Flyweight (intrinsic state) ----

    /// <summary>
    /// Flyweightパターンの共有オブジェクト
    /// 名前・色・テクスチャなどの内因的（共有可能な）状態を保持する
    /// </summary>
    public class TreeType {
        /// <summary>木の種類名</summary>
        private readonly string name;

        /// <summary>木の色</summary>
        private readonly string color;

        /// <summary>テクスチャ名</summary>
        private readonly string texture;

        /// <summary>木の種類名を取得する</summary>
        public string Name => name;

        /// <summary>木の色を取得する</summary>
        public string Color => color;

        /// <summary>テクスチャ名を取得する</summary>
        public string Texture => texture;

        /// <summary>
        /// TreeTypeを生成する
        /// </summary>
        /// <param name="name">木の種類名</param>
        /// <param name="color">木の色</param>
        /// <param name="texture">テクスチャ名</param>
        public TreeType(string name, string color, string texture) {
            this.name = name;
            this.color = color;
            this.texture = texture;
        }
    }

    // ---- Context (extrinsic state) ----

    /// <summary>
    /// 個々の木を表すコンテキストクラス
    /// 座標などの外因的（個別の）状態とTreeTypeへの参照を保持する
    /// </summary>
    public class Tree {
        /// <summary>X座標</summary>
        private readonly int x;

        /// <summary>Y座標</summary>
        private readonly int y;

        /// <summary>共有されるTreeType</summary>
        private readonly TreeType treeType;

        /// <summary>X座標を取得する</summary>
        public int X => x;

        /// <summary>Y座標を取得する</summary>
        public int Y => y;

        /// <summary>共有されるTreeTypeを取得する</summary>
        public TreeType Type => treeType;

        /// <summary>
        /// Treeを生成する
        /// </summary>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        /// <param name="treeType">共有されるTreeType</param>
        public Tree(int x, int y, TreeType treeType) {
            this.x = x;
            this.y = y;
            this.treeType = treeType;
        }
    }

    // ---- Flyweight Factory ----

    /// <summary>
    /// TreeTypeのキャッシュと再利用を管理するファクトリ
    /// 同じ種類のTreeTypeは1つのインスタンスを共有する
    /// </summary>
    public class TreeFactory {
        /// <summary>種類名をキーとするTreeTypeのキャッシュ</summary>
        private readonly Dictionary<string, TreeType> treeTypes = new Dictionary<string, TreeType>();

        /// <summary>キャッシュされているTreeTypeの数を取得する</summary>
        public int TypeCount => treeTypes.Count;

        /// <summary>
        /// 指定された種類のTreeTypeを取得する（キャッシュがあれば再利用）
        /// </summary>
        /// <param name="name">木の種類名</param>
        /// <param name="color">木の色</param>
        /// <param name="texture">テクスチャ名</param>
        /// <returns>TreeTypeのインスタンス</returns>
        public TreeType GetTreeType(string name, string color, string texture) {
            if (!treeTypes.ContainsKey(name)) {
                treeTypes[name] = new TreeType(name, color, texture);
            }
            return treeTypes[name];
        }
    }

    // ---- Demo ----

    /// <summary>
    /// Flyweightパターンのデモ
    /// TreeTypeの共有によりメモリ効率を向上させる仕組みを示す
    /// </summary>
    [PatternDemo("flyweight")]
    public class FlyweightDemo : BasePatternDemo {
        /// <summary>デモのパターンID</summary>
        public override string PatternId => "flyweight";

        /// <summary>デモの表示名</summary>
        public override string DisplayName => "Flyweight";

        /// <summary>TreeTypeファクトリ</summary>
        private TreeFactory factory;

        /// <summary>植えられた木のリスト</summary>
        private List<Tree> trees;

        /// <summary>最初のOak TreeType参照</summary>
        private TreeType oakType1;

        /// <summary>2番目のOak TreeType参照</summary>
        private TreeType oakType2;

        /// <summary>Pine TreeType参照</summary>
        private TreeType pineType;

        /// <summary>1番目のOakのX座標</summary>
        private const int Oak1X = 10;

        /// <summary>1番目のOakのY座標</summary>
        private const int Oak1Y = 20;

        /// <summary>2番目のOakのX座標</summary>
        private const int Oak2X = 30;

        /// <summary>2番目のOakのY座標</summary>
        private const int Oak2Y = 40;

        /// <summary>PineのX座標</summary>
        private const int PineX = 50;

        /// <summary>PineのY座標</summary>
        private const int PineY = 60;

        /// <summary>
        /// リセット時にドメインオブジェクトをクリアする
        /// </summary>
        protected override void OnReset() {
            factory = null;
            trees = null;
            oakType1 = null;
            oakType2 = null;
            pineType = null;
        }

        /// <summary>
        /// Flyweightパターンのシナリオを構築する
        /// </summary>
        /// <param name="scenario">ステップを追加するシナリオ</param>
        protected override void BuildScenario(DemoScenario scenario) {
            factory = new TreeFactory();
            trees = new List<Tree>();

            scenario.AddStep(new DemoStep(
                "TreeFactoryでOak TreeTypeを取得する",
                () => {
                    oakType1 = factory.GetTreeType("Oak", "Green", "oak_bark");
                    Log("TreeFactory", "GetTreeType(\"Oak\", ...)", $"新規生成 — TypeCount: {factory.TypeCount}");
                }
            ));

            scenario.AddStep(new DemoStep(
                $"Oakを座標({Oak1X},{Oak1Y})に植える",
                () => {
                    var tree = new Tree(Oak1X, Oak1Y, oakType1);
                    trees.Add(tree);
                    Log("Client", $"new Tree({Oak1X}, {Oak1Y}, oakType)", $"Tree数: {trees.Count}");
                }
            ));

            scenario.AddStep(new DemoStep(
                $"もう1本のOakを座標({Oak2X},{Oak2Y})に植える",
                () => {
                    oakType2 = factory.GetTreeType("Oak", "Green", "oak_bark");
                    var tree = new Tree(Oak2X, Oak2Y, oakType2);
                    trees.Add(tree);
                    Log("TreeFactory", "GetTreeType(\"Oak\", ...)", $"キャッシュ再利用 — TypeCount: {factory.TypeCount}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "2つのOak TreeTypeが同一インスタンスであることを確認する",
                () => {
                    bool same = ReferenceEquals(oakType1, oakType2);
                    Log("検証", "ReferenceEquals(oakType1, oakType2)", same ? "True — 同一インスタンス" : "False");
                }
            ));

            scenario.AddStep(new DemoStep(
                "Pine TreeTypeを取得する",
                () => {
                    pineType = factory.GetTreeType("Pine", "DarkGreen", "pine_bark");
                    Log("TreeFactory", "GetTreeType(\"Pine\", ...)", $"新規生成 — TypeCount: {factory.TypeCount}");
                }
            ));

            scenario.AddStep(new DemoStep(
                $"Pineを座標({PineX},{PineY})に植える",
                () => {
                    var tree = new Tree(PineX, PineY, pineType);
                    trees.Add(tree);
                    Log("Client", $"new Tree({PineX}, {PineY}, pineType)", $"Tree数: {trees.Count}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "TreeType数 vs Tree数を比較する",
                () => {
                    Log("統計", "Flyweight効果", $"TreeType: {factory.TypeCount}個, Tree: {trees.Count}本 — メモリ共有で効率化");
                }
            ));
        }
    }
}
