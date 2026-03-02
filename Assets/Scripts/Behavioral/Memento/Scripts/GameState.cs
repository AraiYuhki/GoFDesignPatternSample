namespace DesignPatterns.Behavioral.Memento {
    /// <summary>
    /// ゲームの状態を管理するOriginatorクラス
    /// Mementoパターンにおける「Originator」に相当する
    /// 自身の状態をMementoとして保存・復元できる
    /// </summary>
    public sealed class GameState {
        /// <summary>プレイヤー名</summary>
        private string playerName;

        /// <summary>レベル</summary>
        private int level;

        /// <summary>HP</summary>
        private int hp;

        /// <summary>所持金</summary>
        private int gold;

        /// <summary>プレイヤー名を取得・設定する</summary>
        public string PlayerName {
            get { return playerName; }
            set { playerName = value; }
        }

        /// <summary>レベルを取得・設定する</summary>
        public int Level {
            get { return level; }
            set { level = value; }
        }

        /// <summary>HPを取得・設定する</summary>
        public int Hp {
            get { return hp; }
            set { hp = value; }
        }

        /// <summary>所持金を取得・設定する</summary>
        public int Gold {
            get { return gold; }
            set { gold = value; }
        }

        /// <summary>
        /// GameStateを生成する
        /// </summary>
        /// <param name="playerName">プレイヤー名</param>
        /// <param name="level">初期レベル</param>
        /// <param name="hp">初期HP</param>
        /// <param name="gold">初期所持金</param>
        public GameState(string playerName, int level, int hp, int gold) {
            this.playerName = playerName;
            this.level = level;
            this.hp = hp;
            this.gold = gold;
        }

        /// <summary>
        /// 現在の状態をMementoとして保存する
        /// </summary>
        /// <returns>現在の状態のスナップショット</returns>
        public GameStateMemento Save() {
            return new GameStateMemento(playerName, level, hp, gold);
        }

        /// <summary>
        /// Mementoから状態を復元する
        /// </summary>
        /// <param name="memento">復元元のMemento</param>
        public void Restore(GameStateMemento memento) {
            playerName = memento.PlayerName;
            level = memento.Level;
            hp = memento.Hp;
            gold = memento.Gold;
        }

        /// <summary>
        /// 状態の文字列表現を返す
        /// </summary>
        /// <returns>フォーマットされた状態文字列</returns>
        public override string ToString() {
            return $"[{playerName}] Lv.{level} HP:{hp} Gold:{gold}";
        }
    }
}
