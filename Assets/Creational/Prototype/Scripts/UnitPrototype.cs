using System;
using System.Text;

namespace DesignPatterns.Creational.Prototype {
    /// <summary>
    /// 複製可能なユニットの基底クラス（Prototype）
    ///
    /// 【Prototypeパターンにおける役割】
    /// 自分自身をコピーするためのインターフェースを定義する
    /// サブクラスが具体的なクローン処理を実装する
    /// </summary>
    public abstract class UnitPrototype : ICloneable {
        /// <summary>ユニット名</summary>
        public string Name { get; set; }

        /// <summary>HP</summary>
        public int Hp { get; set; }

        /// <summary>攻撃力</summary>
        public int Attack { get; set; }

        /// <summary>レベル</summary>
        public int Level { get; set; }

        /// <summary>
        /// ユニットの浅いコピーを作成する（ShallowCopy）
        /// </summary>
        /// <returns>コピーされたオブジェクト</returns>
        public object Clone() {
            return MemberwiseClone();
        }

        /// <summary>
        /// ユニットの深いコピーを作成する（DeepCopy）
        /// サブクラスがオーバーライドして独自の深いコピー処理を実装する
        /// </summary>
        /// <returns>深いコピーされたユニット</returns>
        public abstract UnitPrototype DeepClone();

        /// <summary>
        /// ユニット情報を文字列で返す
        /// </summary>
        /// <returns>ユニットの詳細情報</returns>
        public override string ToString() {
            var sb = new StringBuilder();
            sb.Append($"{Name} Lv.{Level}");
            sb.Append($" (HP:{Hp} ATK:{Attack})");
            return sb.ToString();
        }
    }

    /// <summary>
    /// 兵士ユニット（ConcretePrototype）
    /// </summary>
    public sealed class SoldierUnit : UnitPrototype {
        /// <summary>装備している武器名</summary>
        public string WeaponName { get; set; }

        /// <inheritdoc/>
        public override UnitPrototype DeepClone() {
            var clone = new SoldierUnit {
                Name = Name,
                Hp = Hp,
                Attack = Attack,
                Level = Level,
                WeaponName = WeaponName
            };
            return clone;
        }

        /// <inheritdoc/>
        public override string ToString() {
            return $"{base.ToString()} 武器:{WeaponName}";
        }
    }

    /// <summary>
    /// 弓兵ユニット（ConcretePrototype）
    /// </summary>
    public sealed class ArcherUnit : UnitPrototype {
        /// <summary>射程距離</summary>
        public int Range { get; set; }

        /// <inheritdoc/>
        public override UnitPrototype DeepClone() {
            var clone = new ArcherUnit {
                Name = Name,
                Hp = Hp,
                Attack = Attack,
                Level = Level,
                Range = Range
            };
            return clone;
        }

        /// <inheritdoc/>
        public override string ToString() {
            return $"{base.ToString()} 射程:{Range}";
        }
    }
}
