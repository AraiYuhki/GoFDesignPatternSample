using UnityEngine;

namespace GoFPatterns.Patterns.Visualization {
    /// <summary>
    /// Flyweightパターンのビジュアライゼーション
    /// TreeTypeの共有とTree個体の関係をステップ実行で可視化する
    /// </summary>
    [PatternVisualization("flyweight")]
    public class FlyweightVisualization : BasePatternVisualization {
        /// <summary>Oak TreeTypeの表示位置</summary>
        private static readonly Vector2 OakTypePosition = new Vector2(-1.5f, 0f);

        /// <summary>Pine TreeTypeの表示位置</summary>
        private static readonly Vector2 PineTypePosition = new Vector2(1.5f, 0f);

        /// <summary>1本目のOakの表示位置</summary>
        private static readonly Vector2 Oak1Position = new Vector2(-4.5f, 3.0f);

        /// <summary>2本目のOakの表示位置</summary>
        private static readonly Vector2 Oak2Position = new Vector2(-4.5f, -3.0f);

        /// <summary>Pineの木の表示位置</summary>
        private static readonly Vector2 Pine1Position = new Vector2(4.5f, 3.0f);

        /// <summary>統計情報ラベルの表示位置</summary>
        private static readonly Vector2 StatsPosition = new Vector2(0f, -4.2f);

        /// <summary>TreeTypeの矩形サイズ</summary>
        private static readonly Vector2 TypeSize = new Vector2(2.5f, 1.5f);

        /// <summary>Treeの円半径</summary>
        private const float TreeRadius = 0.6f;

        /// <summary>統計ラベルの矩形サイズ</summary>
        private static readonly Vector2 StatsSize = new Vector2(6.0f, 0.8f);

        /// <summary>Oak TreeTypeの色</summary>
        private static readonly Color OakColor = new Color(0.3f, 0.7f, 0.3f, 1f);

        /// <summary>Pine TreeTypeの色</summary>
        private static readonly Color PineColor = new Color(0.2f, 0.5f, 0.2f, 1f);

        /// <summary>Treeの色</summary>
        private static readonly Color TreeColor = new Color(0.5f, 0.8f, 0.5f, 1f);

        /// <summary>
        /// バインド時に初期レイアウトを構築する
        /// </summary>
        /// <param name="demo">バインドされたデモ</param>
        protected override void OnBind(IPatternDemo demo) {
            VisualElement oakType = AddRect("oakType", "Oak\n(TreeType)", OakTypePosition, TypeSize, OakColor);
            VisualElement pineType = AddRect("pineType", "Pine\n(TreeType)", PineTypePosition, TypeSize, PineColor);
            VisualElement oak1 = AddCircle("oak1", "Oak\n(10,20)", Oak1Position, TreeRadius, TreeColor);
            VisualElement oak2 = AddCircle("oak2", "Oak\n(30,40)", Oak2Position, TreeRadius, TreeColor);
            VisualElement pine1 = AddCircle("pine1", "Pine\n(50,60)", Pine1Position, TreeRadius, TreeColor);
            VisualElement stats = AddRect("stats", "", StatsPosition, StatsSize, DimColor);

            oakType.SetVisible(false);
            pineType.SetVisible(false);
            oak1.SetVisible(false);
            oak2.SetVisible(false);
            pine1.SetVisible(false);
            stats.SetVisible(false);

            AddArrow("oak1ToType", oak1, oakType, ArrowColor);
            AddArrow("oak2ToType", oak2, oakType, ArrowColor);
            AddArrow("pine1ToType", pine1, pineType, ArrowColor);

            GetArrow("oak1ToType").SetColor(DimColor);
            GetArrow("oak2ToType").SetColor(DimColor);
            GetArrow("pine1ToType").SetColor(DimColor);
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
        /// Step0: Oak TreeTypeを生成する
        /// </summary>
        private void RefreshStep0() {
            VisualElement oakType = GetElement("oakType");
            oakType.SetVisible(true);
            oakType.Pulse(HighlightColor, 0.6f);
        }

        /// <summary>
        /// Step1: 1本目のOakを植える
        /// </summary>
        private void RefreshStep1() {
            VisualElement oak1 = GetElement("oak1");
            oak1.SetVisible(true);
            oak1.Pulse(HighlightColor, 0.6f);

            VisualArrow arrow = GetArrow("oak1ToType");
            arrow.SetColor(ArrowColor);
            arrow.Pulse(PulseColor, 0.6f);
        }

        /// <summary>
        /// Step2: 2本目のOakを植える（TreeTypeを再利用）
        /// </summary>
        private void RefreshStep2() {
            VisualElement oak2 = GetElement("oak2");
            oak2.SetVisible(true);
            oak2.Pulse(HighlightColor, 0.6f);

            VisualArrow arrow = GetArrow("oak2ToType");
            arrow.SetColor(ArrowColor);
            arrow.Pulse(PulseColor, 0.6f);

            GetElement("oakType").Pulse(PulseColor, 0.5f);
        }

        /// <summary>
        /// Step3: 2つのOakが同一TreeTypeを共有していることを検証する
        /// </summary>
        private void RefreshStep3() {
            GetElement("oakType").SetLabel("Oak\n(TreeType)\n[Shared]");
            GetElement("oakType").Pulse(HighlightColor, 0.6f);
            GetArrow("oak1ToType").Pulse(HighlightColor, 0.6f);
            GetArrow("oak2ToType").Pulse(HighlightColor, 0.6f);
            GetElement("oak1").Pulse(HighlightColor, 0.5f);
            GetElement("oak2").Pulse(HighlightColor, 0.5f);
        }

        /// <summary>
        /// Step4: Pine TreeTypeを生成する
        /// </summary>
        private void RefreshStep4() {
            VisualElement pineType = GetElement("pineType");
            pineType.SetVisible(true);
            pineType.Pulse(HighlightColor, 0.6f);
        }

        /// <summary>
        /// Step5: Pineの木を植える
        /// </summary>
        private void RefreshStep5() {
            VisualElement pine1 = GetElement("pine1");
            pine1.SetVisible(true);
            pine1.Pulse(HighlightColor, 0.6f);

            VisualArrow arrow = GetArrow("pine1ToType");
            arrow.SetColor(ArrowColor);
            arrow.Pulse(PulseColor, 0.6f);
        }

        /// <summary>
        /// Step6: TreeType数 vs Tree数の統計を表示する
        /// </summary>
        private void RefreshStep6() {
            VisualElement stats = GetElement("stats");
            stats.SetVisible(true);
            stats.SetLabel("TreeType: 2  /  Tree: 3");
            stats.SetColorImmediate(PulseColor);
            stats.Pulse(PulseColor, 0.6f);

            GetElement("oakType").Pulse(PulseColor, 0.5f);
            GetElement("pineType").Pulse(PulseColor, 0.5f);
            GetElement("oak1").Pulse(PulseColor, 0.5f);
            GetElement("oak2").Pulse(PulseColor, 0.5f);
            GetElement("pine1").Pulse(PulseColor, 0.5f);
        }
    }
}
