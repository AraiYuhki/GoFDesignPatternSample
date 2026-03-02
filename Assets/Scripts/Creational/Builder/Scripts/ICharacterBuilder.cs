namespace DesignPatterns.Creational.Builder {
    /// <summary>
    /// キャラクター構築の手順を定義するインターフェース（Builder）
    ///
    /// 【Builderパターンにおける役割】
    /// プロダクトの各部品を生成するための抽象インターフェース
    /// 具象ビルダーがこのインターフェースを実装し、具体的な構築処理を提供する
    /// </summary>
    public interface ICharacterBuilder {
        /// <summary>ビルダーの名前</summary>
        string BuilderName { get; }

        /// <summary>
        /// 基本情報（名前・職業）を設定する
        /// </summary>
        /// <returns>メソッドチェーン用にビルダー自身を返す</returns>
        ICharacterBuilder SetBasicInfo();

        /// <summary>
        /// 武器を設定する
        /// </summary>
        /// <returns>メソッドチェーン用にビルダー自身を返す</returns>
        ICharacterBuilder SetWeapon();

        /// <summary>
        /// 防具を設定する
        /// </summary>
        /// <returns>メソッドチェーン用にビルダー自身を返す</returns>
        ICharacterBuilder SetArmor();

        /// <summary>
        /// スキルを設定する
        /// </summary>
        /// <returns>メソッドチェーン用にビルダー自身を返す</returns>
        ICharacterBuilder SetSkill();

        /// <summary>
        /// ステータスを計算・設定する
        /// </summary>
        /// <returns>メソッドチェーン用にビルダー自身を返す</returns>
        ICharacterBuilder CalculateStats();

        /// <summary>
        /// 構築を完了してキャラクターデータを返す
        /// </summary>
        /// <returns>構築されたキャラクターデータ</returns>
        CharacterData Build();
    }
}
