namespace DesignPatterns.Creational.Builder
{
    /// <summary>
    /// キャラクター構築の手順を指揮するDirector
    ///
    /// 【Builderパターンにおける役割】
    /// ビルダーのインターフェースを使用して構築手順を定義する
    /// 具体的な構築内容はビルダーに委ね、構築の順序のみ管理する
    /// </summary>
    public sealed class CharacterDirector
    {
        /// <summary>
        /// 全装備付きのキャラクターを構築する
        /// </summary>
        /// <param name="builder">使用するビルダー</param>
        /// <returns>完全装備のキャラクターデータ</returns>
        public CharacterData ConstructFullEquipped(ICharacterBuilder builder)
        {
            return builder
                .SetBasicInfo()
                .SetWeapon()
                .SetArmor()
                .SetSkill()
                .CalculateStats()
                .Build();
        }

        /// <summary>
        /// 最低限の装備のみのキャラクターを構築する
        /// </summary>
        /// <param name="builder">使用するビルダー</param>
        /// <returns>最低装備のキャラクターデータ</returns>
        public CharacterData ConstructMinimal(ICharacterBuilder builder)
        {
            return builder
                .SetBasicInfo()
                .SetWeapon()
                .CalculateStats()
                .Build();
        }
    }
}
