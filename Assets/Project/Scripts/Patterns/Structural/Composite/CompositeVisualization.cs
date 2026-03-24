using UnityEngine;

namespace GoFPatterns.Patterns.Visualization {
    /// <summary>
    /// Compositeパターンのビジュアライゼーション
    /// ファイル/ディレクトリのツリー構造を段階的に構築してサイズ計算を可視化する
    /// </summary>
    [PatternVisualization("composite")]
    public class CompositeVisualization : BasePatternVisualization {
        /// <summary>ルートディレクトリの表示位置</summary>
        private static readonly Vector2 RootPosition = new Vector2(0f, 3.5f);

        /// <summary>ファイルAの表示位置</summary>
        private static readonly Vector2 FileAPosition = new Vector2(-3.5f, 0.5f);

        /// <summary>ファイルBの表示位置</summary>
        private static readonly Vector2 FileBPosition = new Vector2(0f, 0.5f);

        /// <summary>サブディレクトリの表示位置</summary>
        private static readonly Vector2 SubDirPosition = new Vector2(3.5f, 0.5f);

        /// <summary>ファイルCの表示位置</summary>
        private static readonly Vector2 FileCPosition = new Vector2(3.5f, -2.5f);

        /// <summary>ディレクトリの矩形サイズ</summary>
        private static readonly Vector2 DirSize = new Vector2(2.8f, 1.4f);

        /// <summary>ファイルの矩形サイズ</summary>
        private static readonly Vector2 FileSize = new Vector2(2.4f, 1.2f);

        /// <summary>ディレクトリの色</summary>
        private static readonly Color DirColor = new Color(0.3f, 0.5f, 0.8f, 1f);

        /// <summary>ファイルの色</summary>
        private static readonly Color FileColor = new Color(0.5f, 0.7f, 0.4f, 1f);

        /// <summary>
        /// バインド時に初期レイアウトを構築する
        /// </summary>
        /// <param name="demo">バインドされたデモ</param>
        protected override void OnBind(IPatternDemo demo) {
            VisualElement fileA = AddRect("fileA", "readme.txt\n100B", FileAPosition, FileSize, FileColor);
            VisualElement fileB = AddRect("fileB", "data.csv\n250B", FileBPosition, FileSize, FileColor);
            VisualElement root = AddRect("root", "root/", RootPosition, DirSize, DirColor);
            VisualElement subDir = AddRect("subDir", "images/", SubDirPosition, DirSize, DirColor);
            VisualElement fileC = AddRect("fileC", "photo.png\n400B", FileCPosition, FileSize, FileColor);

            fileA.SetVisible(false);
            fileB.SetVisible(false);
            root.SetVisible(false);
            subDir.SetVisible(false);
            fileC.SetVisible(false);

            AddArrow("rootToFileA", root, fileA, ArrowColor, false);
            AddArrow("rootToFileB", root, fileB, ArrowColor, false);
            AddArrow("rootToSubDir", root, subDir, ArrowColor, false);
            AddArrow("subDirToFileC", subDir, fileC, ArrowColor, false);

            GetArrow("rootToFileA").SetColor(DimColor);
            GetArrow("rootToFileB").SetColor(DimColor);
            GetArrow("rootToSubDir").SetColor(DimColor);
            GetArrow("subDirToFileC").SetColor(DimColor);
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
        /// Step0: ファイルを作成する
        /// </summary>
        private void RefreshStep0() {
            VisualElement fileA = GetElement("fileA");
            VisualElement fileB = GetElement("fileB");
            fileA.SetVisible(true);
            fileB.SetVisible(true);
            fileA.Pulse(HighlightColor, 0.6f);
            fileB.Pulse(HighlightColor, 0.6f);
        }

        /// <summary>
        /// Step1: ルートディレクトリを作成する
        /// </summary>
        private void RefreshStep1() {
            VisualElement root = GetElement("root");
            root.SetVisible(true);
            root.SetLabel("root/\n(空)");
            root.Pulse(HighlightColor, 0.6f);
        }

        /// <summary>
        /// Step2: ルートにファイルを追加して接続線を表示する
        /// </summary>
        private void RefreshStep2() {
            GetArrow("rootToFileA").SetColor(ArrowColor);
            GetArrow("rootToFileB").SetColor(ArrowColor);
            GetArrow("rootToFileA").Pulse(PulseColor, 0.6f);
            GetArrow("rootToFileB").Pulse(PulseColor, 0.6f);

            VisualElement root = GetElement("root");
            root.SetLabel("root/\n350B");
            root.Pulse(PulseColor, 0.5f);

            GetElement("fileA").Pulse(PulseColor, 0.5f);
            GetElement("fileB").Pulse(PulseColor, 0.5f);
        }

        /// <summary>
        /// Step3: サブディレクトリを作成しファイルCを追加する
        /// </summary>
        private void RefreshStep3() {
            VisualElement subDir = GetElement("subDir");
            VisualElement fileC = GetElement("fileC");
            subDir.SetVisible(true);
            fileC.SetVisible(true);

            subDir.SetLabel("images/\n400B");
            subDir.Pulse(HighlightColor, 0.6f);
            fileC.Pulse(HighlightColor, 0.6f);

            GetArrow("subDirToFileC").SetColor(ArrowColor);
            GetArrow("subDirToFileC").Pulse(PulseColor, 0.6f);
        }

        /// <summary>
        /// Step4: サブディレクトリをルートにネストする
        /// </summary>
        private void RefreshStep4() {
            GetArrow("rootToSubDir").SetColor(ArrowColor);
            GetArrow("rootToSubDir").Pulse(PulseColor, 0.6f);

            VisualElement root = GetElement("root");
            root.Pulse(PulseColor, 0.5f);

            GetElement("subDir").Pulse(PulseColor, 0.5f);
        }

        /// <summary>
        /// Step5: ルートのGetSize()で再帰計算を可視化する
        /// </summary>
        private void RefreshStep5() {
            GetElement("fileA").Pulse(PulseColor, 0.5f);
            GetElement("fileB").Pulse(PulseColor, 0.5f);
            GetElement("fileC").Pulse(PulseColor, 0.5f);
            GetElement("subDir").Pulse(PulseColor, 0.5f);

            GetArrow("rootToFileA").Pulse(HighlightColor, 0.6f);
            GetArrow("rootToFileB").Pulse(HighlightColor, 0.6f);
            GetArrow("rootToSubDir").Pulse(HighlightColor, 0.6f);
            GetArrow("subDirToFileC").Pulse(HighlightColor, 0.6f);

            VisualElement root = GetElement("root");
            root.SetLabel("root/\n750B");
            root.Pulse(HighlightColor, 0.6f);
        }

        /// <summary>
        /// Step6: 完成したツリー構造を表示する
        /// </summary>
        private void RefreshStep6() {
            GetElement("root").SetColorImmediate(PulseColor);
            GetElement("root").SetLabel("root/\n750B (Total)");
            GetElement("fileA").SetColorImmediate(FileColor);
            GetElement("fileB").SetColorImmediate(FileColor);
            GetElement("fileC").SetColorImmediate(FileColor);
            GetElement("subDir").SetColorImmediate(DirColor);

            GetElement("root").Pulse(PulseColor, 0.6f);
        }
    }
}
