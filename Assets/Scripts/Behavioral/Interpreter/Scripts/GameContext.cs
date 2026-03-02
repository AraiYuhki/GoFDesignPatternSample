namespace DesignPatterns.Behavioral.Interpreter {
    /// <summary>
    /// コマンド解釈に使用するゲームコンテキスト
    /// キャラクターの名前と現在位置を保持する
    /// </summary>
    public sealed class GameContext {
        /// <summary>キャラクター名</summary>
        private readonly string characterName;

        /// <summary>X座標</summary>
        private int x;

        /// <summary>Y座標</summary>
        private int y;

        /// <summary>
        /// GameContextを生成する
        /// </summary>
        /// <param name="characterName">キャラクター名</param>
        /// <param name="x">初期X座標</param>
        /// <param name="y">初期Y座標</param>
        public GameContext(string characterName, int x, int y) {
            this.characterName = characterName;
            this.x = x;
            this.y = y;
        }

        /// <summary>キャラクター名を取得する</summary>
        public string CharacterName {
            get { return characterName; }
        }

        /// <summary>X座標を取得・設定する</summary>
        public int X {
            get { return x; }
            set { x = value; }
        }

        /// <summary>Y座標を取得・設定する</summary>
        public int Y {
            get { return y; }
            set { y = value; }
        }
    }
}
