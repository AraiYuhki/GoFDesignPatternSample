using System.Text;

namespace DesignPatterns.Creational.Builder {
    /// <summary>
    /// ビルダーによって段階的に構築されるキャラクターデータ（Product）
    ///
    /// 【Builderパターンにおける役割】
    /// 複雑な構成要素を持つ最終的なプロダクト
    /// </summary>
    public sealed class CharacterData {
        /// <summary>キャラクター名</summary>
        public string Name { get; set; }

        /// <summary>職業</summary>
        public string Job { get; set; }

        /// <summary>武器</summary>
        public string Weapon { get; set; }

        /// <summary>防具</summary>
        public string Armor { get; set; }

        /// <summary>スキル</summary>
        public string Skill { get; set; }

        /// <summary>HP</summary>
        public int Hp { get; set; }

        /// <summary>攻撃力</summary>
        public int Attack { get; set; }

        /// <summary>防御力</summary>
        public int Defense { get; set; }

        /// <summary>
        /// キャラクターデータを文字列として返す
        /// </summary>
        /// <returns>キャラクターの詳細情報</returns>
        public override string ToString() {
            var sb = new StringBuilder();
            sb.Append($"【{Name}】 職業: {Job}");
            sb.Append($"\n  武器: {Weapon} / 防具: {Armor}");
            sb.Append($"\n  スキル: {Skill}");
            sb.Append($"\n  HP: {Hp} / 攻撃: {Attack} / 防御: {Defense}");
            return sb.ToString();
        }
    }
}
