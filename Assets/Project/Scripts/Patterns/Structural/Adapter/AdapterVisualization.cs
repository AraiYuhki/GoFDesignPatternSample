using UnityEngine;

namespace GoFPatterns.Patterns.Visualization {
    /// <summary>
    /// Adapterパターンのビジュアライゼーション
    /// Client→Adapter→OldLoggerの変換チェーンをステップ実行で可視化する
    /// </summary>
    [PatternVisualization("adapter")]
    public class AdapterVisualization : BasePatternVisualization {
        /// <summary>Clientの表示位置</summary>
        private static readonly Vector2 ClientPosition = new Vector2(-4.5f, 0f);

        /// <summary>Adapterの表示位置</summary>
        private static readonly Vector2 AdapterPosition = new Vector2(0f, 0f);

        /// <summary>OldLoggerの表示位置</summary>
        private static readonly Vector2 OldLoggerPosition = new Vector2(4.5f, 0f);

        /// <summary>IModernLoggerインターフェースラベルの表示位置</summary>
        private static readonly Vector2 InterfacePosition = new Vector2(-2.25f, 2.5f);

        /// <summary>要素の矩形サイズ</summary>
        private static readonly Vector2 RectSize = new Vector2(2.8f, 1.5f);

        /// <summary>インターフェースラベルの矩形サイズ</summary>
        private static readonly Vector2 InterfaceSize = new Vector2(3.2f, 1.0f);

        /// <summary>
        /// バインド時に初期レイアウトを構築する
        /// </summary>
        /// <param name="demo">バインドされたデモ</param>
        protected override void OnBind(IPatternDemo demo) {
            VisualElement client = AddRect("client", "Client", ClientPosition, RectSize, DimColor);
            VisualElement adapter = AddRect("adapter", "Adapter", AdapterPosition, RectSize, DimColor);
            VisualElement oldLogger = AddRect("oldLogger", "OldLogger", OldLoggerPosition, RectSize, DimColor);
            VisualElement interfaceLabel = AddRect("interface", "IModernLogger", InterfacePosition, InterfaceSize, DimColor);

            client.SetVisible(false);
            adapter.SetVisible(false);
            interfaceLabel.SetVisible(false);

            AddArrow("clientToAdapter", client, adapter, ArrowColor);
            AddArrow("adapterToOldLogger", adapter, oldLogger, ArrowColor);

            GetArrow("clientToAdapter").SetColor(DimColor);
            GetArrow("adapterToOldLogger").SetColor(DimColor);
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
        /// Step0: OldLoggerの旧APIを表示する
        /// </summary>
        private void RefreshStep0() {
            VisualElement oldLogger = GetElement("oldLogger");
            oldLogger.SetColorImmediate(HighlightColor);
            oldLogger.SetLabel("OldLogger\nWriteLog()");
            oldLogger.Pulse(HighlightColor, 0.6f);
        }

        /// <summary>
        /// Step1: IModernLoggerインターフェースを表示する
        /// </summary>
        private void RefreshStep1() {
            VisualElement interfaceLabel = GetElement("interface");
            interfaceLabel.SetVisible(true);
            interfaceLabel.SetColorImmediate(new Color(0.3f, 0.6f, 0.9f, 1f));
            interfaceLabel.SetLabel("IModernLogger\nLog(level, msg)");
            interfaceLabel.Pulse(HighlightColor, 0.6f);
        }

        /// <summary>
        /// Step2: Adapterが接続される様子を表示する
        /// </summary>
        private void RefreshStep2() {
            VisualElement adapter = GetElement("adapter");
            VisualElement client = GetElement("client");
            adapter.SetVisible(true);
            client.SetVisible(true);

            adapter.SetColorImmediate(PulseColor);
            adapter.SetLabel("LoggerAdapter\n(implements\nIModernLogger)");
            adapter.Pulse(PulseColor, 0.6f);

            client.SetColorImmediate(new Color(0.4f, 0.5f, 0.8f, 1f));
            client.SetLabel("Client");

            GetArrow("clientToAdapter").SetColor(ArrowColor);
            GetArrow("adapterToOldLogger").SetColor(ArrowColor);
        }

        /// <summary>
        /// Step3: Client→Adapterへメッセージが流れる
        /// </summary>
        private void RefreshStep3() {
            VisualElement client = GetElement("client");
            client.Pulse(HighlightColor, 0.5f);
            GetArrow("clientToAdapter").Pulse(PulseColor, 0.6f);

            VisualElement adapter = GetElement("adapter");
            adapter.Pulse(HighlightColor, 0.5f);
        }

        /// <summary>
        /// Step4: Adapter→OldLoggerへ変換されたメッセージが流れる
        /// </summary>
        private void RefreshStep4() {
            GetArrow("adapterToOldLogger").Pulse(PulseColor, 0.6f);

            VisualElement oldLogger = GetElement("oldLogger");
            oldLogger.Pulse(HighlightColor, 0.5f);
            oldLogger.SetLabel("OldLogger\n受信確認");
        }

        /// <summary>
        /// Step5: 全体チェーンが透過的に動作することを示す
        /// </summary>
        private void RefreshStep5() {
            GetElement("client").Pulse(PulseColor, 0.5f);
            GetArrow("clientToAdapter").Pulse(PulseColor, 0.6f);
            GetElement("adapter").Pulse(PulseColor, 0.5f);
            GetArrow("adapterToOldLogger").Pulse(PulseColor, 0.6f);
            GetElement("oldLogger").Pulse(PulseColor, 0.5f);
        }
    }
}
