using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DesignPatterns.Structural.Flyweight
{
    /// <summary>
    /// Flyweightパターンのデモシーンを制御するクラス
    ///
    /// 【デモの内容】
    /// - 大量の弾を生成し、共有データとメモリ節約の効果を確認
    /// - 弾タイプ（共有データ）と弾インスタンス（個別データ）の分離を体験
    /// - 弾の数と弾タイプ数の比較で共有の効果を可視化
    /// </summary>
    public sealed class FlyweightDemo : PatternDemoBase
    {
        /// <summary>通常弾の大量生成ボタン</summary>
        [SerializeField]
        private Button spawnNormalButton;

        /// <summary>炎弾の大量生成ボタン</summary>
        [SerializeField]
        private Button spawnFireButton;

        /// <summary>氷弾の大量生成ボタン</summary>
        [SerializeField]
        private Button spawnIceButton;

        /// <summary>統計表示ボタン</summary>
        [SerializeField]
        private Button statsButton;

        /// <summary>弾のFlyweightファクトリ</summary>
        private readonly BulletFactory factory = new BulletFactory();

        /// <summary>生成された全弾のリスト</summary>
        private readonly List<Bullet> bullets = new List<Bullet>();

        /// <summary>1回の生成で作成する弾の数</summary>
        private const int SpawnCount = 50;

        /// <inheritdoc/>
        protected override string PatternName
        {
            get { return "Flyweight"; }
        }

        /// <inheritdoc/>
        protected override PatternCategory Category
        {
            get { return PatternCategory.Structural; }
        }

        /// <inheritdoc/>
        protected override string Description
        {
            get { return "多数のオブジェクトで共通データを共有し、メモリ使用量を削減する"; }
        }

        /// <inheritdoc/>
        protected override void OnDemoStart()
        {
            if (spawnNormalButton != null)
            {
                spawnNormalButton.onClick.AddListener(OnSpawnNormal);
            }
            if (spawnFireButton != null)
            {
                spawnFireButton.onClick.AddListener(OnSpawnFire);
            }
            if (spawnIceButton != null)
            {
                spawnIceButton.onClick.AddListener(OnSpawnIce);
            }
            if (statsButton != null)
            {
                statsButton.onClick.AddListener(OnShowStats);
            }

            InGameLogger.Log("弾を大量生成してメモリ共有の効果を確認してください", LogColor.Yellow);
        }

        /// <summary>通常弾を大量生成する</summary>
        private void OnSpawnNormal()
        {
            SpawnBullets("通常弾", 10, Color.white, 5f);
        }

        /// <summary>炎弾を大量生成する</summary>
        private void OnSpawnFire()
        {
            SpawnBullets("炎弾", 20, Color.red, 8f);
        }

        /// <summary>氷弾を大量生成する</summary>
        private void OnSpawnIce()
        {
            SpawnBullets("氷弾", 15, Color.cyan, 6f);
        }

        /// <summary>
        /// 指定タイプの弾を大量生成する
        /// </summary>
        /// <param name="name">弾タイプ名</param>
        /// <param name="damage">ダメージ</param>
        /// <param name="color">色</param>
        /// <param name="speed">速度</param>
        private void SpawnBullets(string name, int damage, Color color, float speed)
        {
            InGameLogger.Log($"--- {name} x{SpawnCount} 生成 ---", LogColor.Yellow);

            BulletType type = factory.GetBulletType(name, damage, color, speed);

            for (int i = 0; i < SpawnCount; i++)
            {
                var position = new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f));
                var direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                bullets.Add(new Bullet(type, position, direction));
            }

            InGameLogger.Log($"{SpawnCount}個の{name}を生成しました", LogColor.Green);
        }

        /// <summary>
        /// 統計情報を表示する
        /// </summary>
        private void OnShowStats()
        {
            InGameLogger.Log("=== メモリ共有の統計 ===", LogColor.Yellow);
            InGameLogger.Log($"弾インスタンス合計: {bullets.Count} 個", LogColor.Green);
            InGameLogger.Log($"弾タイプ（共有データ）: {factory.GetTypeCount()} 種類", LogColor.Green);
            InGameLogger.Log($"→ {bullets.Count}個の弾が {factory.GetTypeCount()}個の共有データを参照", LogColor.Green);
        }
    }
}
