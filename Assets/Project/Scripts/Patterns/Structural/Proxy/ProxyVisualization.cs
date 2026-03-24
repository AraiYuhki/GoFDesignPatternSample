using UnityEngine;

namespace GoFPatterns.Patterns.Visualization {
    /// <summary>
    /// Proxyパターンのビジュアライゼーション
    /// ProxyImageの遅延読み込みとキャッシュの仕組みをステップ実行で可視化する
    /// </summary>
    [PatternVisualization("proxy")]
    public class ProxyVisualization : BasePatternVisualization {
        /// <summary>Clientの表示位置</summary>
        private static readonly Vector2 ClientPosition = new Vector2(-5.5f, 0f);

        /// <summary>ProxyAの表示位置</summary>
        private static readonly Vector2 ProxyAPosition = new Vector2(-1.5f, 2.0f);

        /// <summary>ProxyBの表示位置</summary>
        private static readonly Vector2 ProxyBPosition = new Vector2(-1.5f, -2.0f);

        /// <summary>RealImageAの表示位置</summary>
        private static readonly Vector2 RealAPosition = new Vector2(3.5f, 2.0f);

        /// <summary>RealImageBの表示位置</summary>
        private static readonly Vector2 RealBPosition = new Vector2(3.5f, -2.0f);

        /// <summary>Clientの円半径</summary>
        private const float ClientRadius = 0.7f;

        /// <summary>Proxyの矩形サイズ</summary>
        private static readonly Vector2 ProxySize = new Vector2(2.8f, 1.4f);

        /// <summary>RealImageの矩形サイズ</summary>
        private static readonly Vector2 RealSize = new Vector2(2.8f, 1.4f);

        /// <summary>Proxyの色</summary>
        private static readonly Color ProxyColor = new Color(0.4f, 0.6f, 0.8f, 1f);

        /// <summary>RealImageの色</summary>
        private static readonly Color RealColor = new Color(0.3f, 0.8f, 0.5f, 1f);

        /// <summary>
        /// バインド時に初期レイアウトを構築する
        /// </summary>
        /// <param name="demo">バインドされたデモ</param>
        protected override void OnBind(IPatternDemo demo) {
            VisualElement client = AddCircle("client", "Client", ClientPosition, ClientRadius, new Color(0.4f, 0.7f, 0.9f, 1f));
            VisualElement proxyA = AddRect("proxyA", "ProxyA\nhero_portrait", ProxyAPosition, ProxySize, ProxyColor);
            VisualElement proxyB = AddRect("proxyB", "ProxyB\nworld_map", ProxyBPosition, ProxySize, ProxyColor);
            VisualElement realA = AddRect("realA", "RealImage A\nhero_portrait", RealAPosition, RealSize, RealColor);
            VisualElement realB = AddRect("realB", "RealImage B\nworld_map", RealBPosition, RealSize, RealColor);

            proxyA.SetVisible(false);
            proxyB.SetVisible(false);
            realA.SetVisible(false);
            realB.SetVisible(false);

            AddArrow("clientToProxyA", client, proxyA, ArrowColor);
            AddArrow("clientToProxyB", client, proxyB, ArrowColor);
            AddArrow("proxyAToRealA", proxyA, realA, ArrowColor);
            AddArrow("proxyBToRealB", proxyB, realB, ArrowColor);

            GetArrow("clientToProxyA").SetColor(DimColor);
            GetArrow("clientToProxyB").SetColor(DimColor);
            GetArrow("proxyAToRealA").SetColor(DimColor);
            GetArrow("proxyBToRealB").SetColor(DimColor);
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
            }
        }

        /// <summary>
        /// Step0: ProxyImageを作成する（RealImageはまだ生成しない）
        /// </summary>
        private void RefreshStep0() {
            VisualElement proxyA = GetElement("proxyA");
            VisualElement proxyB = GetElement("proxyB");
            proxyA.SetVisible(true);
            proxyB.SetVisible(true);
            proxyA.SetLabel("ProxyA\nhero_portrait\n(未読込)");
            proxyB.SetLabel("ProxyB\nworld_map\n(未読込)");
            proxyA.Pulse(HighlightColor, 0.6f);
            proxyB.Pulse(HighlightColor, 0.6f);

            GetArrow("clientToProxyA").SetColor(ArrowColor);
            GetArrow("clientToProxyB").SetColor(ArrowColor);
        }

        /// <summary>
        /// Step1: ProxyA.Display()初回呼び出しでRealImage Aが生成される
        /// </summary>
        private void RefreshStep1() {
            VisualElement realA = GetElement("realA");
            realA.SetVisible(true);
            realA.Pulse(HighlightColor, 0.6f);

            GetElement("client").Pulse(PulseColor, 0.5f);
            GetArrow("clientToProxyA").Pulse(PulseColor, 0.6f);
            GetElement("proxyA").SetLabel("ProxyA\nhero_portrait\n(読込済)");
            GetElement("proxyA").Pulse(PulseColor, 0.5f);

            VisualArrow arrow = GetArrow("proxyAToRealA");
            arrow.SetColor(PulseColor);
            arrow.Pulse(PulseColor, 0.6f);
        }

        /// <summary>
        /// Step2: ProxyA.Display()2回目呼び出し（キャッシュ済みを使用）
        /// </summary>
        private void RefreshStep2() {
            GetElement("client").Pulse(PulseColor, 0.5f);
            GetArrow("clientToProxyA").Pulse(PulseColor, 0.6f);

            GetElement("proxyA").SetLabel("ProxyA\nhero_portrait\n(Cache Hit)");
            GetElement("proxyA").Pulse(HighlightColor, 0.5f);

            GetArrow("proxyAToRealA").SetColor(ArrowColor);
            GetElement("realA").Pulse(PulseColor, 0.5f);
        }

        /// <summary>
        /// Step3: ProxyBはまだ未読み込みであることを確認する
        /// </summary>
        private void RefreshStep3() {
            GetElement("proxyB").SetLabel("ProxyB\nworld_map\n(未読込)");
            GetElement("proxyB").Pulse(HighlightColor, 0.6f);

            GetElement("proxyA").SetLabel("ProxyA\nhero_portrait\n(読込済)");
        }

        /// <summary>
        /// Step4: ProxyB.Display()初回呼び出しでRealImage Bが生成される
        /// </summary>
        private void RefreshStep4() {
            VisualElement realB = GetElement("realB");
            realB.SetVisible(true);
            realB.Pulse(HighlightColor, 0.6f);

            GetElement("client").Pulse(PulseColor, 0.5f);
            GetArrow("clientToProxyB").Pulse(PulseColor, 0.6f);
            GetElement("proxyB").SetLabel("ProxyB\nworld_map\n(読込済)");
            GetElement("proxyB").Pulse(PulseColor, 0.5f);

            VisualArrow arrow = GetArrow("proxyBToRealB");
            arrow.SetColor(PulseColor);
            arrow.Pulse(PulseColor, 0.6f);
        }

        /// <summary>
        /// Step5: 全体のまとめを表示する
        /// </summary>
        private void RefreshStep5() {
            GetElement("proxyA").SetLabel("ProxyA\n(読込済)");
            GetElement("proxyB").SetLabel("ProxyB\n(読込済)");

            GetElement("proxyA").Pulse(PulseColor, 0.5f);
            GetElement("proxyB").Pulse(PulseColor, 0.5f);
            GetElement("realA").Pulse(PulseColor, 0.5f);
            GetElement("realB").Pulse(PulseColor, 0.5f);

            GetArrow("proxyAToRealA").SetColor(ArrowColor);
            GetArrow("proxyBToRealB").SetColor(ArrowColor);
        }
    }
}
