using System.Collections.Generic;

namespace GoFPatterns.Patterns {
    // ---- Observer interface ----

    /// <summary>
    /// Observerパターンのオブザーバーインターフェース
    /// プレイヤーのHP変化通知を受け取る
    /// </summary>
    public interface IHealthObserver {
        /// <summary>オブザーバーの名前を取得する</summary>
        string Name { get; }
        /// <summary>
        /// HP変化時に呼ばれる通知メソッド
        /// </summary>
        /// <param name="oldHp">変化前のHP</param>
        /// <param name="newHp">変化後のHP</param>
        void OnHealthChanged(int oldHp, int newHp);
    }

    // ---- ConcreteObservers ----

    /// <summary>HUD表示を更新するオブザーバー</summary>
    public class HudObserver : IHealthObserver {
        /// <summary>オブザーバーの名前</summary>
        public string Name => "HUD";
        /// <summary>
        /// HP変化時にHUD表示を更新する
        /// </summary>
        /// <param name="oldHp">変化前のHP</param>
        /// <param name="newHp">変化後のHP</param>
        public void OnHealthChanged(int oldHp, int newHp) {
            // HUD描画ロジック（デモでは省略）
        }
        /// <summary>最後に受け取った通知の説明を返す</summary>
        /// <param name="oldHp">変化前のHP</param>
        /// <param name="newHp">変化後のHP</param>
        /// <returns>通知の説明文</returns>
        public string FormatNotification(int oldHp, int newHp) =>
            $"HUD: HP バー更新 {oldHp} → {newHp}";
    }

    /// <summary>サウンドを再生するオブザーバー</summary>
    public class SoundObserver : IHealthObserver {
        /// <summary>オブザーバーの名前</summary>
        public string Name => "Sound";
        /// <summary>
        /// HP変化時にサウンドを再生する
        /// </summary>
        /// <param name="oldHp">変化前のHP</param>
        /// <param name="newHp">変化後のHP</param>
        public void OnHealthChanged(int oldHp, int newHp) {
            // サウンド再生ロジック（デモでは省略）
        }
        /// <summary>最後に受け取った通知の説明を返す</summary>
        /// <param name="oldHp">変化前のHP</param>
        /// <param name="newHp">変化後のHP</param>
        /// <returns>通知の説明文</returns>
        public string FormatNotification(int oldHp, int newHp) =>
            newHp <= 0 ? "Sound: 死亡SE を再生" : "Sound: ダメージSE を再生";
    }

    /// <summary>実績解除を担当するオブザーバー</summary>
    public class AchievementObserver : IHealthObserver {
        /// <summary>低HPしきい値</summary>
        private const int LowHpThreshold = 20;
        /// <summary>オブザーバーの名前</summary>
        public string Name => "Achievement";
        /// <summary>
        /// HP変化時に実績条件を確認する
        /// </summary>
        /// <param name="oldHp">変化前のHP</param>
        /// <param name="newHp">変化後のHP</param>
        public void OnHealthChanged(int oldHp, int newHp) {
            // 実績チェックロジック（デモでは省略）
        }
        /// <summary>最後に受け取った通知の説明を返す</summary>
        /// <param name="oldHp">変化前のHP</param>
        /// <param name="newHp">変化後のHP</param>
        /// <returns>通知の説明文</returns>
        public string FormatNotification(int oldHp, int newHp) =>
            newHp <= LowHpThreshold ? "Achievement: 低HP実績を確認" : "Achievement: 変化を記録";
    }

    // ---- Subject ----

    /// <summary>
    /// Observerパターンのサブジェクト
    /// HPの変化をオブザーバーに通知する
    /// </summary>
    public class PlayerHealth {
        /// <summary>初期HP</summary>
        private const int InitialHp = 100;
        /// <summary>登録されたオブザーバー一覧</summary>
        private readonly List<IHealthObserver> observers = new List<IHealthObserver>();
        /// <summary>現在のHP</summary>
        private int currentHp;

        /// <summary>現在のHPを取得する</summary>
        public int CurrentHp => currentHp;

        /// <summary>
        /// PlayerHealthを生成する
        /// </summary>
        public PlayerHealth() {
            currentHp = InitialHp;
        }

        /// <summary>
        /// オブザーバーを登録する
        /// </summary>
        /// <param name="observer">登録するオブザーバー</param>
        public void RegisterObserver(IHealthObserver observer) {
            observers.Add(observer);
        }

        /// <summary>
        /// オブザーバーの登録を解除する
        /// </summary>
        /// <param name="observer">解除するオブザーバー</param>
        public void UnregisterObserver(IHealthObserver observer) {
            observers.Remove(observer);
        }

        /// <summary>
        /// ダメージを受けてオブザーバーに通知する
        /// </summary>
        /// <param name="damage">ダメージ量</param>
        public void TakeDamage(int damage) {
            int oldHp = currentHp;
            currentHp -= damage;
            if (currentHp < 0) {
                currentHp = 0;
            }
            NotifyObservers(oldHp, currentHp);
        }

        /// <summary>
        /// 登録中のオブザーバー数を取得する
        /// </summary>
        /// <returns>オブザーバー数</returns>
        public int GetObserverCount() => observers.Count;

        /// <summary>
        /// 全オブザーバーに通知する
        /// </summary>
        /// <param name="oldHp">変化前のHP</param>
        /// <param name="newHp">変化後のHP</param>
        private void NotifyObservers(int oldHp, int newHp) {
            foreach (var observer in observers) {
                observer.OnHealthChanged(oldHp, newHp);
            }
        }
    }

    // ---- Demo ----

    /// <summary>
    /// Observerパターンのデモ
    /// PlayerHealthを例にオブザーバーの登録・通知・解除の流れを示す
    /// </summary>
    [PatternDemo("observer")]
    public class ObserverDemo : BasePatternDemo {
        /// <summary>デモのパターンID</summary>
        public override string PatternId => "observer";

        /// <summary>デモの表示名</summary>
        public override string DisplayName => "Observer";

        /// <summary>サブジェクト（観察対象）</summary>
        private PlayerHealth playerHealth;
        /// <summary>HUDオブザーバー</summary>
        private HudObserver hudObserver;
        /// <summary>サウンドオブザーバー</summary>
        private SoundObserver soundObserver;
        /// <summary>実績オブザーバー</summary>
        private AchievementObserver achievementObserver;

        /// <summary>
        /// リセット時にサブジェクトを再生成する
        /// </summary>
        protected override void OnReset() {
            playerHealth = null;
        }

        /// <summary>
        /// Observerパターンのシナリオを構築する
        /// </summary>
        /// <param name="scenario">ステップを追加するシナリオ</param>
        protected override void BuildScenario(DemoScenario scenario) {
            playerHealth = new PlayerHealth();
            hudObserver = new HudObserver();
            soundObserver = new SoundObserver();
            achievementObserver = new AchievementObserver();

            scenario.AddStep(new DemoStep(
                "HudObserverをPlayerHealthに登録する",
                () => {
                    playerHealth.RegisterObserver(hudObserver);
                    Log("PlayerHealth", "RegisterObserver(HUD)", $"登録数: {playerHealth.GetObserverCount()}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "SoundObserverをPlayerHealthに登録する",
                () => {
                    playerHealth.RegisterObserver(soundObserver);
                    Log("PlayerHealth", "RegisterObserver(Sound)", $"登録数: {playerHealth.GetObserverCount()}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "AchievementObserverをPlayerHealthに登録する",
                () => {
                    playerHealth.RegisterObserver(achievementObserver);
                    Log("PlayerHealth", "RegisterObserver(Achievement)", $"登録数: {playerHealth.GetObserverCount()}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "プレイヤーが30ダメージを受ける — 3つのオブザーバーに通知される",
                () => {
                    int oldHp = playerHealth.CurrentHp;
                    playerHealth.TakeDamage(30);
                    Log("PlayerHealth", $"TakeDamage(30)", $"HP: {oldHp} → {playerHealth.CurrentHp}");
                    Log("→ HUD", "OnHealthChanged", hudObserver.FormatNotification(oldHp, playerHealth.CurrentHp));
                    Log("→ Sound", "OnHealthChanged", soundObserver.FormatNotification(oldHp, playerHealth.CurrentHp));
                    Log("→ Achievement", "OnHealthChanged", achievementObserver.FormatNotification(oldHp, playerHealth.CurrentHp));
                }
            ));

            scenario.AddStep(new DemoStep(
                "SoundObserverを登録解除する",
                () => {
                    playerHealth.UnregisterObserver(soundObserver);
                    Log("PlayerHealth", "UnregisterObserver(Sound)", $"登録数: {playerHealth.GetObserverCount()}");
                }
            ));

            scenario.AddStep(new DemoStep(
                "再び30ダメージ — 今度はHUDとAchievementの2つだけに通知される",
                () => {
                    int oldHp = playerHealth.CurrentHp;
                    playerHealth.TakeDamage(30);
                    Log("PlayerHealth", $"TakeDamage(30)", $"HP: {oldHp} → {playerHealth.CurrentHp}");
                    Log("→ HUD", "OnHealthChanged", hudObserver.FormatNotification(oldHp, playerHealth.CurrentHp));
                    Log("→ Achievement", "OnHealthChanged", achievementObserver.FormatNotification(oldHp, playerHealth.CurrentHp));
                    Log("Sound", "(登録解除済み — 通知なし)", "");
                }
            ));
        }
    }
}
