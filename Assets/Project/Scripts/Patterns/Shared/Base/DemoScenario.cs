using System.Collections.Generic;

namespace GoFPatterns.Patterns {
    /// <summary>
    /// デモのステップ列を管理するシナリオクラス
    /// ステップの追加・進行・リセットを提供する
    /// </summary>
    public class DemoScenario {
        /// <summary>ステップのリスト</summary>
        private readonly List<DemoStep> steps = new List<DemoStep>();
        /// <summary>現在のステップインデックス</summary>
        private int currentIndex = -1;

        /// <summary>現在のステップインデックスを取得する</summary>
        public int CurrentIndex => currentIndex;
        /// <summary>全ステップ数を取得する</summary>
        public int TotalSteps => steps.Count;
        /// <summary>次のステップが存在するかどうか</summary>
        public bool HasNext => currentIndex < steps.Count - 1;
        /// <summary>現在のステップを取得する（範囲外の場合はnull）</summary>
        public DemoStep CurrentStep => currentIndex >= 0 && currentIndex < steps.Count ? steps[currentIndex] : null;

        /// <summary>
        /// ステップを追加する
        /// </summary>
        /// <param name="step">追加するステップ</param>
        public void AddStep(DemoStep step) {
            steps.Add(step);
        }

        /// <summary>
        /// 次のステップを実行する
        /// </summary>
        /// <returns>実行されたステップ（次がない場合はnull）</returns>
        public DemoStep ExecuteNext() {
            if (!HasNext) {
                return null;
            }
            currentIndex++;
            var step = steps[currentIndex];
            step.Execute?.Invoke();
            return step;
        }

        /// <summary>
        /// シナリオを初期状態にリセットする
        /// </summary>
        public void Reset() {
            currentIndex = -1;
        }

        /// <summary>
        /// 全ステップをクリアする
        /// </summary>
        public void Clear() {
            steps.Clear();
            currentIndex = -1;
        }
    }
}
