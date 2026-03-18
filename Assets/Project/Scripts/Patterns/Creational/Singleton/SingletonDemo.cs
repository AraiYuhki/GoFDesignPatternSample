namespace GoFPatterns.Patterns {
    /// <summary>
    /// Singletonパターンの実装例であるゲーム進行管理クラス
    /// privateコンストラクタとstatic Instanceプロパティにより唯一のインスタンスを保証する
    /// </summary>
    public sealed class GameProgressManager {
        /// <summary>唯一のインスタンス</summary>
        private static GameProgressManager instance;
        /// <summary>現在のスコア</summary>
        private int score;
        /// <summary>現在のレベル</summary>
        private int level;

        /// <summary>唯一のインスタンスを取得する（初回アクセス時に生成）</summary>
        public static GameProgressManager Instance {
            get {
                if (instance == null) {
                    instance = new GameProgressManager();
                }
                return instance;
            }
        }

        /// <summary>現在のスコアを取得する</summary>
        public int Score => score;

        /// <summary>現在のレベルを取得する</summary>
        public int Level => level;

        /// <summary>外部からのインスタンス生成を禁止するプライベートコンストラクタ</summary>
        private GameProgressManager() {
            score = 0;
            level = 1;
        }

        /// <summary>
        /// スコアを加算する
        /// </summary>
        /// <param name="points">加算するポイント</param>
        public void AddScore(int points) {
            score += points;
        }

        /// <summary>
        /// レベルを1上げる
        /// </summary>
        public void NextLevel() {
            level++;
        }

        /// <summary>
        /// デモ用にインスタンスをリセットする
        /// </summary>
        internal static void ResetForDemo() {
            instance = null;
        }
    }

    /// <summary>
    /// Singletonパターンのデモ
    /// GameProgressManagerを例に単一インスタンスの保証と複数クライアント間での共有状態を示す
    /// </summary>
    [PatternDemo("singleton")]
    public class SingletonDemo : BasePatternDemo {
        /// <summary>デモのパターンID</summary>
        public override string PatternId => "singleton";

        /// <summary>デモの表示名</summary>
        public override string DisplayName => "Singleton";

        /// <summary>最初に取得した参照（Client A）</summary>
        private GameProgressManager refA;

        /// <summary>2番目に取得した参照（Client B）</summary>
        private GameProgressManager refB;

        /// <summary>
        /// リセット時にSingletonインスタンスをクリアする
        /// </summary>
        protected override void OnReset() {
            GameProgressManager.ResetForDemo();
        }

        /// <summary>
        /// Singletonパターンのシナリオを構築する
        /// </summary>
        /// <param name="scenario">ステップを追加するシナリオ</param>
        protected override void BuildScenario(DemoScenario scenario) {
            GameProgressManager.ResetForDemo();

            scenario.AddStep(new DemoStep(
                "Client Aが初めてGameProgressManager.Instanceにアクセスする",
                () => {
                    refA = GameProgressManager.Instance;
                    Log("Client A", "Instance を取得", "新規インスタンスを生成");
                }
            ));

            scenario.AddStep(new DemoStep(
                "Client BがGameProgressManager.Instanceにアクセスする",
                () => {
                    refB = GameProgressManager.Instance;
                    Log("Client B", "Instance を取得", "既存インスタンスを返す");
                }
            ));

            scenario.AddStep(new DemoStep(
                "両参照が同一インスタンスであることを確認する",
                () => {
                    bool same = ReferenceEquals(refA, refB);
                    Log("検証", "ReferenceEquals(A, B)", same ? "True — 同一インスタンス" : "False");
                }
            ));

            scenario.AddStep(new DemoStep(
                "Client AがAddScore(500)するとClient Bの参照でも同じ値になる",
                () => {
                    refA.AddScore(500);
                    Log("Client A", "AddScore(500)", $"B.Score = {refB.Score} (同一状態)");
                }
            ));

            scenario.AddStep(new DemoStep(
                "Client BがNextLevel()するとClient AのLevelにも反映される",
                () => {
                    refB.NextLevel();
                    Log("Client B", "NextLevel()", $"A.Level = {refA.Level} (同一状態)");
                }
            ));
        }
    }
}
