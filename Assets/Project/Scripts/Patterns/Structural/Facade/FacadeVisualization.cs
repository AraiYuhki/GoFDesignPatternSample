using UnityEngine;

namespace GoFPatterns.Patterns.Visualization {
    /// <summary>
    /// Facadeパターンのビジュアライゼーション
    /// 複数サブシステムをFacadeで統合し、簡潔な操作で連携動作させる流れを可視化する
    /// </summary>
    [PatternVisualization("facade")]
    public class FacadeVisualization : BasePatternVisualization {
        /// <summary>Clientの表示位置</summary>
        private static readonly Vector2 ClientPosition = new Vector2(-5.0f, 0f);

        /// <summary>Facadeの表示位置</summary>
        private static readonly Vector2 FacadePosition = new Vector2(1.0f, 0f);

        /// <summary>AudioSystemの表示位置</summary>
        private static readonly Vector2 AudioPosition = new Vector2(-0.5f, 2.5f);

        /// <summary>GraphicsSystemの表示位置</summary>
        private static readonly Vector2 GraphicsPosition = new Vector2(2.5f, 2.5f);

        /// <summary>InputSystemの表示位置</summary>
        private static readonly Vector2 InputPosition = new Vector2(-0.5f, -2.5f);

        /// <summary>SaveSystemの表示位置</summary>
        private static readonly Vector2 SavePosition = new Vector2(2.5f, -2.5f);

        /// <summary>Clientの円半径</summary>
        private const float ClientRadius = 0.7f;

        /// <summary>Facadeの矩形サイズ</summary>
        private static readonly Vector2 FacadeSize = new Vector2(5.5f, 7.0f);

        /// <summary>サブシステムの矩形サイズ</summary>
        private static readonly Vector2 SubsystemSize = new Vector2(2.2f, 1.2f);

        /// <summary>サブシステムの色</summary>
        private static readonly Color SubsystemColor = new Color(0.4f, 0.5f, 0.7f, 1f);

        /// <summary>Facadeの色</summary>
        private static readonly Color FacadeColor = new Color(0.2f, 0.3f, 0.4f, 0.6f);

        /// <summary>
        /// バインド時に初期レイアウトを構築する
        /// </summary>
        /// <param name="demo">バインドされたデモ</param>
        protected override void OnBind(IPatternDemo demo) {
            VisualElement facade = AddRect("facade", "", FacadePosition, FacadeSize, FacadeColor);
            facade.SetVisible(false);

            VisualElement audio = AddRect("audio", "Audio", AudioPosition, SubsystemSize, SubsystemColor);
            VisualElement graphics = AddRect("graphics", "Graphics", GraphicsPosition, SubsystemSize, SubsystemColor);
            VisualElement input = AddRect("input", "Input", InputPosition, SubsystemSize, SubsystemColor);
            VisualElement save = AddRect("save", "Save", SavePosition, SubsystemSize, SubsystemColor);

            VisualElement client = AddCircle("client", "Client", ClientPosition, ClientRadius, new Color(0.4f, 0.7f, 0.9f, 1f));

            AddArrow("clientToFacade", client, facade, ArrowColor);
            GetArrow("clientToFacade").SetColor(DimColor);

            AddArrow("clientToAudio", client, audio, ArrowColor);
            AddArrow("clientToGraphics", client, graphics, ArrowColor);
            AddArrow("clientToInput", client, input, ArrowColor);
            AddArrow("clientToSave", client, save, ArrowColor);
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
        /// Step0: AudioSystemを個別に操作する
        /// </summary>
        private void RefreshStep0() {
            GetElement("client").Pulse(HighlightColor, 0.5f);
            GetArrow("clientToAudio").Pulse(PulseColor, 0.6f);
            GetElement("audio").Pulse(HighlightColor, 0.6f);
            GetElement("audio").SetLabel("Audio\nPlay()");
        }

        /// <summary>
        /// Step1: GraphicsSystemを個別に操作する
        /// </summary>
        private void RefreshStep1() {
            GetElement("client").Pulse(HighlightColor, 0.5f);
            GetArrow("clientToGraphics").Pulse(PulseColor, 0.6f);
            GetElement("graphics").Pulse(HighlightColor, 0.6f);
            GetElement("graphics").SetLabel("Graphics\nLoadScene()");
        }

        /// <summary>
        /// Step2: InputSystemとSaveSystemを個別に操作する
        /// </summary>
        private void RefreshStep2() {
            GetElement("client").Pulse(HighlightColor, 0.5f);
            GetArrow("clientToInput").Pulse(PulseColor, 0.6f);
            GetArrow("clientToSave").Pulse(PulseColor, 0.6f);
            GetElement("input").Pulse(HighlightColor, 0.6f);
            GetElement("input").SetLabel("Input\nEnable()");
            GetElement("save").Pulse(HighlightColor, 0.6f);
            GetElement("save").SetLabel("Save\nLoad()");
        }

        /// <summary>
        /// Step3: GameFacadeを作成して全サブシステムを統合する
        /// </summary>
        private void RefreshStep3() {
            VisualElement facade = GetElement("facade");
            facade.SetVisible(true);
            facade.SetLabel("GameFacade");
            facade.Pulse(HighlightColor, 0.6f);

            GetArrow("clientToAudio").SetColor(DimColor);
            GetArrow("clientToGraphics").SetColor(DimColor);
            GetArrow("clientToInput").SetColor(DimColor);
            GetArrow("clientToSave").SetColor(DimColor);

            GetArrow("clientToFacade").SetColor(ArrowColor);
            GetArrow("clientToFacade").Pulse(PulseColor, 0.6f);

            GetElement("audio").SetLabel("Audio");
            GetElement("graphics").SetLabel("Graphics");
            GetElement("input").SetLabel("Input");
            GetElement("save").SetLabel("Save");
        }

        /// <summary>
        /// Step4: Facade.StartGame()で全サブシステムを一括起動する
        /// </summary>
        private void RefreshStep4() {
            GetElement("client").Pulse(PulseColor, 0.5f);
            GetArrow("clientToFacade").Pulse(PulseColor, 0.6f);
            GetElement("facade").SetLabel("GameFacade\nStartGame()");
            GetElement("facade").Pulse(PulseColor, 0.6f);

            GetElement("audio").Pulse(PulseColor, 0.5f);
            GetElement("audio").SetLabel("Audio\nPlay()");
            GetElement("graphics").Pulse(PulseColor, 0.5f);
            GetElement("graphics").SetLabel("Graphics\nLoadScene()");
            GetElement("input").Pulse(PulseColor, 0.5f);
            GetElement("input").SetLabel("Input\nEnable()");
            GetElement("save").Pulse(PulseColor, 0.5f);
            GetElement("save").SetLabel("Save\nLoad()");
        }

        /// <summary>
        /// Step5: Facade.SaveAndQuit()で全サブシステムを一括停止する
        /// </summary>
        private void RefreshStep5() {
            GetElement("client").Pulse(HighlightColor, 0.5f);
            GetArrow("clientToFacade").Pulse(HighlightColor, 0.6f);
            GetElement("facade").SetLabel("GameFacade\nSaveAndQuit()");
            GetElement("facade").Pulse(HighlightColor, 0.6f);

            GetElement("audio").Pulse(HighlightColor, 0.5f);
            GetElement("audio").SetLabel("Audio\nStop()");
            GetElement("graphics").Pulse(HighlightColor, 0.5f);
            GetElement("graphics").SetLabel("Graphics\nClear()");
            GetElement("input").Pulse(HighlightColor, 0.5f);
            GetElement("input").SetLabel("Input\nDisable()");
            GetElement("save").Pulse(HighlightColor, 0.5f);
            GetElement("save").SetLabel("Save\nSave()");
        }

        /// <summary>
        /// Step6: FacadeDemo完了後の状態表示 (追加保護ステップ)
        /// </summary>
        private void RefreshStep6() {
            GetElement("facade").SetLabel("GameFacade");
            GetElement("facade").SetColorImmediate(PulseColor);
            GetElement("facade").Pulse(PulseColor, 0.6f);
        }
    }
}
