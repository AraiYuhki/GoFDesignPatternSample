namespace DesignPatterns.Behavioral.ChainOfResponsibility {
    /// <summary>
    /// ダメージ処理の連鎖を構成する抽象ハンドラ
    /// Chain of Responsibilityパターンにおける「Handler」に相当する
    /// 各ハンドラはダメージを加工し、次のハンドラへ渡す
    /// </summary>
    public abstract class DamageHandler {
        /// <summary>次のハンドラへの参照</summary>
        private DamageHandler nextHandler;

        /// <summary>ハンドラの名前</summary>
        public abstract string HandlerName { get; }

        /// <summary>
        /// 次のハンドラを設定する
        /// メソッドチェーンで連結できるように自身を返す
        /// </summary>
        /// <param name="next">次のハンドラ</param>
        /// <returns>設定された次のハンドラ</returns>
        public DamageHandler SetNext(DamageHandler next) {
            nextHandler = next;
            return next;
        }

        /// <summary>
        /// ダメージを処理し、次のハンドラへ渡す
        /// サブクラスはProcessメソッドで実際の処理を行う
        /// </summary>
        /// <param name="damage">受けたダメージ量</param>
        /// <returns>処理後のダメージ量</returns>
        public int Handle(int damage) {
            int processed = Process(damage);
            if (nextHandler != null) {
                return nextHandler.Handle(processed);
            }
            return processed;
        }

        /// <summary>
        /// ハンドラ固有のダメージ処理を行う
        /// サブクラスで具体的な処理を実装する
        /// </summary>
        /// <param name="damage">受けたダメージ量</param>
        /// <returns>処理後のダメージ量</returns>
        protected abstract int Process(int damage);
    }

    /// <summary>
    /// 防具によるダメージ軽減ハンドラ
    /// ダメージを30%カットする
    /// </summary>
    public sealed class ArmorHandler : DamageHandler {
        /// <summary>防具によるダメージ軽減率</summary>
        private const float ReductionRate = 0.3f;

        /// <inheritdoc/>
        public override string HandlerName {
            get { return "Armor（防具）"; }
        }

        /// <summary>
        /// ダメージを30%軽減する
        /// </summary>
        /// <param name="damage">受けたダメージ量</param>
        /// <returns>軽減後のダメージ量</returns>
        protected override int Process(int damage) {
            int reduced = (int)(damage * ReductionRate);
            int result = damage - reduced;
            InGameLogger.Log($"  [{HandlerName}] {damage} → {result} (30%カット: -{reduced})", LogColor.Orange);
            return result;
        }
    }

    /// <summary>
    /// 耐性によるダメージ軽減ハンドラ
    /// ダメージを固定値5だけ軽減する
    /// </summary>
    public sealed class ResistanceHandler : DamageHandler {
        /// <summary>耐性による固定ダメージ軽減値</summary>
        private const int FlatReduction = 5;

        /// <inheritdoc/>
        public override string HandlerName {
            get { return "Resistance（耐性）"; }
        }

        /// <summary>
        /// ダメージを固定値5だけ軽減する（最低0）
        /// </summary>
        /// <param name="damage">受けたダメージ量</param>
        /// <returns>軽減後のダメージ量</returns>
        protected override int Process(int damage) {
            int result = damage - FlatReduction;
            if (result < 0) {
                result = 0;
            }
            InGameLogger.Log($"  [{HandlerName}] {damage} → {result} (固定軽減: -{FlatReduction})", LogColor.Orange);
            return result;
        }
    }

    /// <summary>
    /// 回避によるダメージ無効化ハンドラ
    /// 20%の確率でダメージを0にする
    /// </summary>
    public sealed class DodgeHandler : DamageHandler {
        /// <summary>回避確率（0.0〜1.0）</summary>
        private const float DodgeChance = 0.2f;

        /// <inheritdoc/>
        public override string HandlerName {
            get { return "Dodge（回避）"; }
        }

        /// <summary>
        /// 20%の確率でダメージを0にする
        /// </summary>
        /// <param name="damage">受けたダメージ量</param>
        /// <returns>回避成功時は0、失敗時は元のダメージ量</returns>
        protected override int Process(int damage) {
            float roll = UnityEngine.Random.Range(0f, 1f);
            if (roll < DodgeChance) {
                InGameLogger.Log($"  [{HandlerName}] 回避成功! {damage} → 0 (判定: {roll:F2} < {DodgeChance})", LogColor.Yellow);
                return 0;
            }
            InGameLogger.Log($"  [{HandlerName}] 回避失敗 {damage} → {damage} (判定: {roll:F2} >= {DodgeChance})", LogColor.Orange);
            return damage;
        }
    }
}
