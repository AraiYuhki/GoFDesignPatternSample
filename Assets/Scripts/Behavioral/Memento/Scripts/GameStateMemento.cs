namespace DesignPatterns.Behavioral.Memento {
    /// <summary>
    /// ゲーム状態のスナップショットを保持する不変クラス
    /// Mementoパターンにおける「Memento」に相当する
    /// Originatorの内部状態を外部に公開せず保存する
    /// </summary>
    public sealed class GameStateMemento {
        /// <summary>プレイヤー名</summary>
        private readonly string playerName;

        /// <summary>レベル</summary>
        private readonly int level;

        /// <summary>HP</summary>
        private readonly int hp;

        /// <summary>所持金</summary>
        private readonly int gold;

        /// <summary>プレイヤー名を取得する</summary>
        public string PlayerName {
            get { return playerName; }
        }

        /// <summary>レベルを取得する</summary>
        public int Level {
            get { return level; }
        }

        /// <summary>HPを取得する</summary>
        public int Hp {
            get { return hp; }
        }

        /// <summary>所持金を取得する</summary>
        public int Gold {
            get { return gold; }
        }

        /// <summary>
        /// GameStateMementoを生成する
        /// </summary>
        /// <param name="playerName">プレイヤー名</param>
        /// <param name="level">レベル</param>
        /// <param name="hp">HP</param>
        /// <param name="gold">所持金</param>
        public GameStateMemento(string playerName, int level, int hp, int gold) {
            this.playerName = playerName;
            this.level = level;
            this.hp = hp;
            this.gold = gold;
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
