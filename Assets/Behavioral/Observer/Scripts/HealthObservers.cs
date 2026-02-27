namespace DesignPatterns.Behavioral.Observer {
    /// <summary>
    /// HPバー表示オブザーバー（ConcreteObserver）
    /// HPの割合をバーで表示する
    /// </summary>
    public sealed class HpBarObserver : IHealthObserver {
        /// <inheritdoc/>
        public void OnHealthChanged(HealthChangedEventData data) {
            float ratio = (float)data.NewHp / data.MaxHp;
            int barLength = 20;
            int filled = (int)(ratio * barLength);
            string bar = new string('█', filled) + new string('░', barLength - filled);
            InGameLogger.Log($"  [HPバー] [{bar}] {ratio * 100:F0}%", LogColor.Orange);
        }
    }

    /// <summary>
    /// 数値表示オブザーバー（ConcreteObserver）
    /// HPを数値で表示する
    /// </summary>
    public sealed class HpNumberObserver : IHealthObserver {
        /// <inheritdoc/>
        public void OnHealthChanged(HealthChangedEventData data) {
            int diff = data.NewHp - data.OldHp;
            string diffText = diff >= 0 ? $"+{diff}" : diff.ToString();
            InGameLogger.Log($"  [数値] HP: {data.NewHp}/{data.MaxHp} ({diffText})", LogColor.Orange);
        }
    }

    /// <summary>
    /// 警告オブザーバー（ConcreteObserver）
    /// HPが低下した際に警告を表示する
    /// </summary>
    public sealed class WarningObserver : IHealthObserver {
        /// <summary>警告を表示するHP割合の閾値</summary>
        private const float WarningThreshold = 0.3f;

        /// <summary>危険を表示するHP割合の閾値</summary>
        private const float DangerThreshold = 0.1f;

        /// <inheritdoc/>
        public void OnHealthChanged(HealthChangedEventData data) {
            float ratio = (float)data.NewHp / data.MaxHp;
            if (data.NewHp <= 0) {
                InGameLogger.Log("  [警告] ★★★ 戦闘不能！ ★★★", LogColor.Red);
            } else if (ratio <= DangerThreshold) {
                InGameLogger.Log("  [警告] !! 危険 !! HPが極めて低い！", LogColor.Red);
            } else if (ratio <= WarningThreshold) {
                InGameLogger.Log("  [警告] ! 注意 ! HPが低下しています", LogColor.Yellow);
            }
        }
    }
}
