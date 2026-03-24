using UnityEngine;

namespace GoFPatterns.Patterns.Visualization {
    /// <summary>
    /// Decoratorパターンのビジュアライゼーション
    /// 武器にエンチャントを重ねがけするネスト構造をステップ実行で可視化する
    /// </summary>
    [PatternVisualization("decorator")]
    public class DecoratorVisualization : BasePatternVisualization {
        /// <summary>中心位置</summary>
        private static readonly Vector2 CenterPosition = new Vector2(0f, 0f);

        /// <summary>BasicSwordの矩形サイズ</summary>
        private static readonly Vector2 BasicSwordSize = new Vector2(2.5f, 2.0f);

        /// <summary>FireEnchantmentの矩形サイズ</summary>
        private static readonly Vector2 FireSize = new Vector2(4.5f, 3.5f);

        /// <summary>PoisonEnchantmentの矩形サイズ</summary>
        private static readonly Vector2 PoisonSize = new Vector2(6.5f, 5.0f);

        /// <summary>ダメージ表示ラベルの表示位置</summary>
        private static readonly Vector2 DamageLabelPosition = new Vector2(0f, -3.5f);

        /// <summary>説明表示ラベルの表示位置</summary>
        private static readonly Vector2 DescLabelPosition = new Vector2(0f, 3.5f);

        /// <summary>ラベルの矩形サイズ</summary>
        private static readonly Vector2 LabelSize = new Vector2(5.0f, 0.8f);

        /// <summary>BasicSwordの色</summary>
        private static readonly Color SwordColor = new Color(0.5f, 0.5f, 0.55f, 1f);

        /// <summary>FireEnchantmentの色</summary>
        private static readonly Color FireColor = new Color(0.9f, 0.4f, 0.1f, 0.7f);

        /// <summary>PoisonEnchantmentの色</summary>
        private static readonly Color PoisonColor = new Color(0.6f, 0.2f, 0.8f, 0.7f);

        /// <summary>
        /// バインド時に初期レイアウトを構築する
        /// </summary>
        /// <param name="demo">バインドされたデモ</param>
        protected override void OnBind(IPatternDemo demo) {
            VisualElement poison = AddRect("poison", "", CenterPosition, PoisonSize, PoisonColor);
            VisualElement fire = AddRect("fire", "", CenterPosition, FireSize, FireColor);
            VisualElement sword = AddRect("sword", "BasicSword", CenterPosition, BasicSwordSize, SwordColor);
            VisualElement damageLabel = AddRect("damageLabel", "", DamageLabelPosition, LabelSize, DimColor);
            VisualElement descLabel = AddRect("descLabel", "", DescLabelPosition, LabelSize, DimColor);

            poison.SetVisible(false);
            fire.SetVisible(false);
            sword.SetVisible(false);
            damageLabel.SetVisible(false);
            descLabel.SetVisible(false);
        }

        /// <summary>
        /// ステップごとの表示更新を行う
        /// </summary>
        /// <param name="stepIndex">現在のステップインデックス</param>
        protected override void OnRefresh(int stepIndex) {
            switch (stepIndex) {
                case 0:
                    RefreshStep0();
                    break;
                case 1:
                    RefreshStep1();
                    break;
                case 2:
                    RefreshStep2();
                    break;
                case 3:
                    RefreshStep3();
                    break;
                case 4:
                    RefreshStep4();
                    break;
                case 5:
                    RefreshStep5();
                    break;
                case 6:
                    RefreshStep6();
                    break;
            }
        }

        /// <summary>
        /// Step0: BasicSwordを作成する
        /// </summary>
        private void RefreshStep0() {
            VisualElement sword = GetElement("sword");
            sword.SetVisible(true);
            sword.SetLabel("BasicSword");
            sword.Pulse(HighlightColor, 0.6f);
        }

        /// <summary>
        /// Step1: BasicSwordの基本ダメージを確認する
        /// </summary>
        private void RefreshStep1() {
            VisualElement damageLabel = GetElement("damageLabel");
            damageLabel.SetVisible(true);
            damageLabel.SetLabel("Damage: 10");
            damageLabel.Pulse(HighlightColor, 0.6f);

            GetElement("sword").Pulse(PulseColor, 0.5f);
        }

        /// <summary>
        /// Step2: FireEnchantmentを追加する
        /// </summary>
        private void RefreshStep2() {
            VisualElement fire = GetElement("fire");
            fire.SetVisible(true);
            fire.SetLabel("Fire");
            fire.Pulse(HighlightColor, 0.6f);

            VisualElement descLabel = GetElement("descLabel");
            descLabel.SetVisible(true);
            descLabel.SetLabel("BasicSword + Fire");

            GetElement("sword").SetLabel("BasicSword\n(inner)");
        }

        /// <summary>
        /// Step3: FireEnchantment適用後のダメージを確認する
        /// </summary>
        private void RefreshStep3() {
            VisualElement damageLabel = GetElement("damageLabel");
            damageLabel.SetLabel("Damage: 10 + 5 = 15");
            damageLabel.Pulse(HighlightColor, 0.6f);

            GetElement("fire").Pulse(PulseColor, 0.5f);
            GetElement("sword").Pulse(PulseColor, 0.5f);
        }

        /// <summary>
        /// Step4: PoisonEnchantmentを重ねがけする
        /// </summary>
        private void RefreshStep4() {
            VisualElement poison = GetElement("poison");
            poison.SetVisible(true);
            poison.SetLabel("Poison");
            poison.Pulse(HighlightColor, 0.6f);

            GetElement("descLabel").SetLabel("BasicSword + Fire + Poison");
            GetElement("fire").SetLabel("Fire\n(middle)");
        }

        /// <summary>
        /// Step5: 二重デコレーション後のダメージを確認する
        /// </summary>
        private void RefreshStep5() {
            VisualElement damageLabel = GetElement("damageLabel");
            damageLabel.SetLabel("Damage: 10 + 5 + 3 = 18");
            damageLabel.Pulse(HighlightColor, 0.6f);

            GetElement("poison").Pulse(PulseColor, 0.5f);
            GetElement("fire").Pulse(PulseColor, 0.5f);
            GetElement("sword").Pulse(PulseColor, 0.5f);
        }

        /// <summary>
        /// Step6: 説明チェーンの全体を表示する
        /// </summary>
        private void RefreshStep6() {
            GetElement("descLabel").SetLabel("BasicSword + Fire + Poison");
            GetElement("damageLabel").SetLabel("Total Damage: 18");

            GetElement("poison").SetColorImmediate(PoisonColor);
            GetElement("fire").SetColorImmediate(FireColor);
            GetElement("sword").SetColorImmediate(SwordColor);

            GetElement("poison").Pulse(PulseColor, 0.6f);
            GetElement("fire").Pulse(PulseColor, 0.6f);
            GetElement("sword").Pulse(PulseColor, 0.6f);
        }
    }
}
