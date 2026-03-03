using System.Collections.Generic;

namespace DesignPatterns.Behavioral.Observer
{
    /// <summary>
    /// HPの変更イベントデータ
    /// </summary>
    public readonly struct HealthChangedEventData
    {
        /// <summary>変更前のHP</summary>
        public readonly int OldHp;

        /// <summary>変更後のHP</summary>
        public readonly int NewHp;

        /// <summary>最大HP</summary>
        public readonly int MaxHp;

        /// <summary>
        /// イベントデータを生成する
        /// </summary>
        /// <param name="oldHp">変更前のHP</param>
        /// <param name="newHp">変更後のHP</param>
        /// <param name="maxHp">最大HP</param>
        public HealthChangedEventData(int oldHp, int newHp, int maxHp)
        {
            OldHp = oldHp;
            NewHp = newHp;
            MaxHp = maxHp;
        }
    }

    /// <summary>
    /// HPの変更を監視するオブザーバーインターフェース（Observer）
    /// </summary>
    public interface IHealthObserver
    {
        /// <summary>
        /// HP変更通知を受け取る
        /// </summary>
        /// <param name="data">HP変更データ</param>
        void OnHealthChanged(HealthChangedEventData data);
    }

    /// <summary>
    /// HP管理システム（Subject/Observable）
    ///
    /// 【Observerパターンの意図】
    /// オブジェクト間の1対多の依存関係を定義し、
    /// 一方の状態変化が全ての依存オブジェクトに自動的に通知・更新される
    /// </summary>
    public sealed class HealthSystem
    {
        /// <summary>最大HP</summary>
        private readonly int maxHp;

        /// <summary>現在のHP</summary>
        private int currentHp;

        /// <summary>登録されたオブザーバーのリスト</summary>
        private readonly List<IHealthObserver> observers = new List<IHealthObserver>();

        /// <summary>現在のHP</summary>
        public int CurrentHp { get { return currentHp; } }

        /// <summary>最大HP</summary>
        public int MaxHp { get { return maxHp; } }

        /// <summary>
        /// HPシステムを初期化する
        /// </summary>
        /// <param name="maxHp">最大HP</param>
        public HealthSystem(int maxHp)
        {
            this.maxHp = maxHp;
            currentHp = maxHp;
        }

        /// <summary>
        /// オブザーバーを登録する
        /// </summary>
        /// <param name="observer">登録するオブザーバー</param>
        public void Subscribe(IHealthObserver observer)
        {
            observers.Add(observer);
        }

        /// <summary>
        /// オブザーバーの登録を解除する
        /// </summary>
        /// <param name="observer">解除するオブザーバー</param>
        public void Unsubscribe(IHealthObserver observer)
        {
            observers.Remove(observer);
        }

        /// <summary>
        /// ダメージを受ける
        /// </summary>
        /// <param name="amount">ダメージ量</param>
        public void TakeDamage(int amount)
        {
            int oldHp = currentHp;
            currentHp = currentHp - amount;
            if (currentHp < 0)
            {
                currentHp = 0;
            }
            NotifyObservers(oldHp, currentHp);
        }

        /// <summary>
        /// HPを回復する
        /// </summary>
        /// <param name="amount">回復量</param>
        public void Heal(int amount)
        {
            int oldHp = currentHp;
            currentHp = currentHp + amount;
            if (currentHp > maxHp)
            {
                currentHp = maxHp;
            }
            NotifyObservers(oldHp, currentHp);
        }

        /// <summary>
        /// 全オブザーバーに通知する
        /// </summary>
        /// <param name="oldHp">変更前HP</param>
        /// <param name="newHp">変更後HP</param>
        private void NotifyObservers(int oldHp, int newHp)
        {
            var data = new HealthChangedEventData(oldHp, newHp, maxHp);
            for (int i = 0; i < observers.Count; i++)
            {
                observers[i].OnHealthChanged(data);
            }
        }
    }
}
