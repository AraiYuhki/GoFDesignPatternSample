using UnityEngine;
using UnityEngine.UI;

namespace DesignPatterns.Creational.Builder {
    /// <summary>
    /// Builderパターンのデモシーンを制御するクラス
    ///
    /// 【デモの内容】
    /// - Directorが同じ構築手順で異なるビルダーを使い、異なるキャラクターを生成
    /// - 全装備 / 最低装備の2つの構築手順を切り替え
    /// - 段階的な構築プロセスをログで可視化
    /// </summary>
    public sealed class BuilderDemo : PatternDemoBase {
        /// <summary>戦士の全装備構築ボタン</summary>
        [SerializeField]
        private Button buildWarriorFullButton;

        /// <summary>魔法使いの全装備構築ボタン</summary>
        [SerializeField]
        private Button buildMageFullButton;

        /// <summary>盗賊の全装備構築ボタン</summary>
        [SerializeField]
        private Button buildThiefFullButton;

        /// <summary>戦士の最低装備構築ボタン</summary>
        [SerializeField]
        private Button buildWarriorMinButton;

        /// <summary>ディレクター</summary>
        private readonly CharacterDirector director = new CharacterDirector();

        /// <inheritdoc/>
        protected override string PatternName {
            get { return "Builder"; }
        }

        /// <inheritdoc/>
        protected override PatternCategory Category {
            get { return PatternCategory.Creational; }
        }

        /// <inheritdoc/>
        protected override string Description {
            get { return "複雑なオブジェクトの構築過程を抽象化し、同じ構築過程で異なる表現を生成する"; }
        }

        /// <inheritdoc/>
        protected override void OnDemoStart() {
            if (buildWarriorFullButton != null) {
                buildWarriorFullButton.onClick.AddListener(() => BuildFull(new WarriorBuilder()));
            }
            if (buildMageFullButton != null) {
                buildMageFullButton.onClick.AddListener(() => BuildFull(new MageBuilder()));
            }
            if (buildThiefFullButton != null) {
                buildThiefFullButton.onClick.AddListener(() => BuildFull(new ThiefBuilder()));
            }
            if (buildWarriorMinButton != null) {
                buildWarriorMinButton.onClick.AddListener(() => BuildMinimal(new WarriorBuilder()));
            }

            InGameLogger.Log("ビルダーを選んでキャラクターを構築してください", LogColor.Yellow);
        }

        /// <summary>
        /// 全装備でキャラクターを構築する
        /// </summary>
        /// <param name="builder">使用するビルダー</param>
        private void BuildFull(ICharacterBuilder builder) {
            InGameLogger.Log($"--- {builder.BuilderName}: 全装備で構築 ---", LogColor.Yellow);
            CharacterData character = director.ConstructFullEquipped(builder);
            InGameLogger.Log(character.ToString(), LogColor.Blue);
        }

        /// <summary>
        /// 最低装備でキャラクターを構築する
        /// </summary>
        /// <param name="builder">使用するビルダー</param>
        private void BuildMinimal(ICharacterBuilder builder) {
            InGameLogger.Log($"--- {builder.BuilderName}: 最低装備で構築 ---", LogColor.Yellow);
            CharacterData character = director.ConstructMinimal(builder);
            InGameLogger.Log(character.ToString(), LogColor.Blue);
        }
    }
}
