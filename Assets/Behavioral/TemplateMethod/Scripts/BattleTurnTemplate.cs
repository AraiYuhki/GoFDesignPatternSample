namespace DesignPatterns.Behavioral.TemplateMethod {
    /// <summary>
    /// バトルターンの処理手順を定義するテンプレートクラス
    /// Template Methodパターンにおける AbstractClass に相当し、
    /// ターンの骨格（開始 → 行動 → 終了）をテンプレートメソッドとして定義する
    /// </summary>
    public abstract class BattleTurnTemplate {
        /// <summary>キャラクター名</summary>
        private readonly string characterName;

        /// <summary>
        /// BattleTurnTemplateを生成する
        /// </summary>
        /// <param name="characterName">キャラクター名</param>
        protected BattleTurnTemplate(string characterName) {
            this.characterName = characterName;
        }

        /// <summary>キャラクター名を取得する</summary>
        protected string CharacterName {
            get { return characterName; }
        }

        /// <summary>
        /// ターンを実行するテンプレートメソッド
        /// StartPhase → ActionPhase → EndPhase の順に処理を呼び出す
        /// </summary>
        public void ExecuteTurn() {
            StartPhase();
            ActionPhase();
            EndPhase();
        }

        /// <summary>
        /// ターン開始フェーズの既定処理
        /// サブクラスでオーバーライド可能
        /// </summary>
        protected virtual void StartPhase() {
            InGameLogger.Log($"[{characterName}] のターン開始", LogColor.Orange);
        }

        /// <summary>
        /// 行動フェーズの処理
        /// サブクラスで具体的な行動を実装する
        /// </summary>
        protected abstract void ActionPhase();

        /// <summary>
        /// ターン終了フェーズの既定処理
        /// サブクラスでオーバーライド可能
        /// </summary>
        protected virtual void EndPhase() {
            InGameLogger.Log($"[{characterName}] のターン終了", LogColor.Orange);
        }
    }
}
