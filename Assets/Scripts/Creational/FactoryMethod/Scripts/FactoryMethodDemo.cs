using UnityEngine;
using UnityEngine.UI;

namespace DesignPatterns.Creational.FactoryMethod
{
    /// <summary>
    /// Factory Methodパターンのデモシーンを制御するクラス
    ///
    /// 【デモの内容】
    /// - 3種類のファクトリ（スライム/ゴブリン/ドラゴン）から敵を生成
    /// - ファクトリの切り替えにより、生成されるオブジェクトが変わることを確認
    /// - 生成された敵に攻撃を実行させ、異なる振る舞いを確認
    /// </summary>
    public sealed class FactoryMethodDemo : PatternDemoBase
    {
        /// <summary>スライム生成ボタン</summary>
        [SerializeField]
        private Button createSlimeButton;

        /// <summary>ゴブリン生成ボタン</summary>
        [SerializeField]
        private Button createGoblinButton;

        /// <summary>ドラゴン生成ボタン</summary>
        [SerializeField]
        private Button createDragonButton;

        /// <summary>最後に生成された敵に攻撃させるボタン</summary>
        [SerializeField]
        private Button attackButton;

        /// <summary>最後に生成された敵</summary>
        private IEnemy lastCreatedEnemy;

        /// <inheritdoc/>
        protected override string PatternName
        {
            get { return "Factory Method"; }
        }

        /// <inheritdoc/>
        protected override PatternCategory Category
        {
            get { return PatternCategory.Creational; }
        }

        /// <inheritdoc/>
        protected override string Description
        {
            get { return "インスタンスの生成をサブクラスに委ね、どのクラスをインスタンス化するかをサブクラスが決定する"; }
        }

        /// <inheritdoc/>
        protected override void OnDemoStart()
        {
            if (createSlimeButton != null)
            {
                createSlimeButton.onClick.AddListener(() => CreateEnemy(new SlimeFactory()));
            }
            if (createGoblinButton != null)
            {
                createGoblinButton.onClick.AddListener(() => CreateEnemy(new GoblinFactory()));
            }
            if (createDragonButton != null)
            {
                createDragonButton.onClick.AddListener(() => CreateEnemy(new DragonFactory()));
            }
            if (attackButton != null)
            {
                attackButton.onClick.AddListener(OnAttack);
            }

            InGameLogger.Log("ファクトリを選んで敵を生成してください", LogColor.Yellow);
        }

        /// <summary>
        /// 指定されたファクトリを使って敵を生成する
        /// </summary>
        /// <param name="factory">使用するファクトリ</param>
        private void CreateEnemy(EnemyFactory factory)
        {
            InGameLogger.Log($"--- {factory.FactoryName} で生成 ---", LogColor.Yellow);

            lastCreatedEnemy = factory.CreateEnemy();

            InGameLogger.Log($"生成: {lastCreatedEnemy.Name}", LogColor.Blue);
            InGameLogger.Log($"  HP: {lastCreatedEnemy.Hp} / 攻撃力: {lastCreatedEnemy.Attack}", LogColor.White);
        }

        /// <summary>
        /// 最後に生成された敵に攻撃を実行させる
        /// </summary>
        private void OnAttack()
        {
            if (lastCreatedEnemy == null)
            {
                InGameLogger.Log("まだ敵が生成されていません", LogColor.Red);
                return;
            }

            string result = lastCreatedEnemy.PerformAttack();
            InGameLogger.Log(result, LogColor.Blue);
        }
    }
}
