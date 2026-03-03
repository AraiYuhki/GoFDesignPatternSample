namespace DesignPatterns.Creational.Builder
{
    /// <summary>
    /// 戦士キャラクターのビルダー（ConcreteBuilder）
    /// 高HP・高防御の近接戦闘型キャラクターを構築する
    /// </summary>
    public sealed class WarriorBuilder : ICharacterBuilder
    {
        /// <summary>構築中のキャラクターデータ</summary>
        private CharacterData character = new CharacterData();

        /// <inheritdoc/>
        public string BuilderName { get { return "戦士ビルダー"; } }

        /// <inheritdoc/>
        public ICharacterBuilder SetBasicInfo()
        {
            character.Name = "勇者アーサー";
            character.Job = "戦士";
            return this;
        }

        /// <inheritdoc/>
        public ICharacterBuilder SetWeapon()
        {
            character.Weapon = "両手剣";
            return this;
        }

        /// <inheritdoc/>
        public ICharacterBuilder SetArmor()
        {
            character.Armor = "重鎧";
            return this;
        }

        /// <inheritdoc/>
        public ICharacterBuilder SetSkill()
        {
            character.Skill = "豪快斬り";
            return this;
        }

        /// <inheritdoc/>
        public ICharacterBuilder CalculateStats()
        {
            character.Hp = 200;
            character.Attack = 30;
            character.Defense = 40;
            return this;
        }

        /// <inheritdoc/>
        public CharacterData Build()
        {
            CharacterData result = character;
            character = new CharacterData();
            return result;
        }
    }

    /// <summary>
    /// 魔法使いキャラクターのビルダー（ConcreteBuilder）
    /// 高攻撃力・低防御の魔法攻撃型キャラクターを構築する
    /// </summary>
    public sealed class MageBuilder : ICharacterBuilder
    {
        /// <summary>構築中のキャラクターデータ</summary>
        private CharacterData character = new CharacterData();

        /// <inheritdoc/>
        public string BuilderName { get { return "魔法使いビルダー"; } }

        /// <inheritdoc/>
        public ICharacterBuilder SetBasicInfo()
        {
            character.Name = "賢者メルリン";
            character.Job = "魔法使い";
            return this;
        }

        /// <inheritdoc/>
        public ICharacterBuilder SetWeapon()
        {
            character.Weapon = "魔法の杖";
            return this;
        }

        /// <inheritdoc/>
        public ICharacterBuilder SetArmor()
        {
            character.Armor = "ローブ";
            return this;
        }

        /// <inheritdoc/>
        public ICharacterBuilder SetSkill()
        {
            character.Skill = "ファイアボール";
            return this;
        }

        /// <inheritdoc/>
        public ICharacterBuilder CalculateStats()
        {
            character.Hp = 100;
            character.Attack = 50;
            character.Defense = 15;
            return this;
        }

        /// <inheritdoc/>
        public CharacterData Build()
        {
            CharacterData result = character;
            character = new CharacterData();
            return result;
        }
    }

    /// <summary>
    /// 盗賊キャラクターのビルダー（ConcreteBuilder）
    /// バランス型の素早いキャラクターを構築する
    /// </summary>
    public sealed class ThiefBuilder : ICharacterBuilder
    {
        /// <summary>構築中のキャラクターデータ</summary>
        private CharacterData character = new CharacterData();

        /// <inheritdoc/>
        public string BuilderName { get { return "盗賊ビルダー"; } }

        /// <inheritdoc/>
        public ICharacterBuilder SetBasicInfo()
        {
            character.Name = "影のシド";
            character.Job = "盗賊";
            return this;
        }

        /// <inheritdoc/>
        public ICharacterBuilder SetWeapon()
        {
            character.Weapon = "短剣（二刀流）";
            return this;
        }

        /// <inheritdoc/>
        public ICharacterBuilder SetArmor()
        {
            character.Armor = "軽装鎧";
            return this;
        }

        /// <inheritdoc/>
        public ICharacterBuilder SetSkill()
        {
            character.Skill = "バックスタブ";
            return this;
        }

        /// <inheritdoc/>
        public ICharacterBuilder CalculateStats()
        {
            character.Hp = 120;
            character.Attack = 35;
            character.Defense = 25;
            return this;
        }

        /// <inheritdoc/>
        public CharacterData Build()
        {
            CharacterData result = character;
            character = new CharacterData();
            return result;
        }
    }
}
