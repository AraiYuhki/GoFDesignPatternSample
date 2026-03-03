using System.Collections.Generic;
using UnityEngine;

namespace DesignPatterns.Structural.Flyweight
{
    /// <summary>
    /// 弾の共有データ（Flyweight）
    /// 同じ種類の弾で共通する不変データ
    ///
    /// 【Flyweightパターンの意図】
    /// 多数のオブジェクトが存在する場合に、共有可能な部分を共有して
    /// メモリ使用量を削減する
    /// </summary>
    public sealed class BulletType
    {
        /// <summary>弾の種類名</summary>
        public readonly string Name;

        /// <summary>基本ダメージ</summary>
        public readonly int Damage;

        /// <summary>弾の色</summary>
        public readonly Color Color;

        /// <summary>弾の速度</summary>
        public readonly float Speed;

        /// <summary>
        /// 弾タイプを生成する
        /// </summary>
        /// <param name="name">種類名</param>
        /// <param name="damage">基本ダメージ</param>
        /// <param name="color">色</param>
        /// <param name="speed">速度</param>
        public BulletType(string name, int damage, Color color, float speed)
        {
            Name = name;
            Damage = damage;
            Color = color;
            Speed = speed;
        }
    }

    /// <summary>
    /// 弾のインスタンス（外部状態を持つコンテキスト）
    /// 位置や方向などインスタンス固有のデータを保持する
    /// </summary>
    public sealed class Bullet
    {
        /// <summary>共有される弾タイプ（Flyweight）</summary>
        public readonly BulletType Type;

        /// <summary>弾の現在位置（外部状態）</summary>
        public Vector2 Position;

        /// <summary>弾の進行方向（外部状態）</summary>
        public Vector2 Direction;

        /// <summary>
        /// 弾インスタンスを生成する
        /// </summary>
        /// <param name="type">弾タイプ（共有）</param>
        /// <param name="position">初期位置</param>
        /// <param name="direction">進行方向</param>
        public Bullet(BulletType type, Vector2 position, Vector2 direction)
        {
            Type = type;
            Position = position;
            Direction = direction;
        }
    }

    /// <summary>
    /// 弾タイプのFlyweightファクトリ
    /// 同じ種類の弾タイプを共有し、メモリ使用量を削減する
    /// </summary>
    public sealed class BulletFactory
    {
        /// <summary>弾タイプのキャッシュ</summary>
        private readonly Dictionary<string, BulletType> bulletTypes = new Dictionary<string, BulletType>();

        /// <summary>
        /// 弾タイプを取得する（キャッシュがなければ新規作成）
        /// </summary>
        /// <param name="name">種類名</param>
        /// <param name="damage">基本ダメージ</param>
        /// <param name="color">色</param>
        /// <param name="speed">速度</param>
        /// <returns>弾タイプ</returns>
        public BulletType GetBulletType(string name, int damage, Color color, float speed)
        {
            if (!bulletTypes.ContainsKey(name))
            {
                bulletTypes[name] = new BulletType(name, damage, color, speed);
                InGameLogger.Log($"  [Factory] 新規弾タイプ作成: {name}", LogColor.White);
            }
            return bulletTypes[name];
        }

        /// <summary>
        /// キャッシュされた弾タイプの数を返す
        /// </summary>
        /// <returns>弾タイプの種類数</returns>
        public int GetTypeCount()
        {
            return bulletTypes.Count;
        }
    }
}
