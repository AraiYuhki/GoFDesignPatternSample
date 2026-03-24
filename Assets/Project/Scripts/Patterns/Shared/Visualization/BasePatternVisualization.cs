using System.Collections.Generic;
using UnityEngine;

namespace GoFPatterns.Patterns.Visualization {
    /// <summary>
    /// パターンビジュアライゼーションの共通基底クラス
    /// 要素辞書管理とAddCircle/AddRect/AddArrowヘルパーを提供する
    /// サブクラスはOnBind()とOnRefresh()をオーバーライドして具体的な表示を実装する
    /// </summary>
    public abstract class BasePatternVisualization : IPatternVisualization {
        /// <summary>バインド済みデモ参照</summary>
        private IPatternDemo boundDemo;
        /// <summary>ビジュアライゼーションレンダラー参照</summary>
        private VisualizationRenderer renderer;
        /// <summary>IDで管理する要素辞書</summary>
        private readonly Dictionary<string, VisualElement> elements = new Dictionary<string, VisualElement>();
        /// <summary>IDで管理する矢印辞書</summary>
        private readonly Dictionary<string, VisualArrow> arrows = new Dictionary<string, VisualArrow>();

        /// <summary>バインド済みデモを取得する</summary>
        protected IPatternDemo Demo => boundDemo;
        /// <summary>要素の親Transformを取得する</summary>
        protected Transform Root => renderer.VisualRoot;
        /// <summary>要素辞書への読み取りアクセス</summary>
        protected IReadOnlyDictionary<string, VisualElement> Elements => elements;
        /// <summary>矢印辞書への読み取りアクセス</summary>
        protected IReadOnlyDictionary<string, VisualArrow> Arrows => arrows;

        /// <summary>ハイライト時のデフォルト色</summary>
        protected static readonly Color HighlightColor = new Color(1f, 0.9f, 0.2f, 1f);
        /// <summary>非アクティブ状態のデフォルト色</summary>
        protected static readonly Color DimColor = new Color(0.3f, 0.3f, 0.4f, 1f);
        /// <summary>矢印のデフォルト色</summary>
        protected static readonly Color ArrowColor = new Color(0.6f, 0.6f, 0.7f, 1f);
        /// <summary>パルスアニメーション色</summary>
        protected static readonly Color PulseColor = new Color(0.2f, 1f, 0.4f, 1f);

        /// <summary>
        /// VisualizationRendererを設定する
        /// </summary>
        /// <param name="visualizationRenderer">使用するレンダラー</param>
        public void SetRenderer(VisualizationRenderer visualizationRenderer) {
            renderer = visualizationRenderer;
        }

        /// <summary>
        /// デモにバインドして初期表示を構築する
        /// </summary>
        /// <param name="demo">バインドするデモ</param>
        public void Bind(IPatternDemo demo) {
            boundDemo = demo;
            OnBind(demo);
        }

        /// <summary>
        /// 現在のデモ状態に合わせて表示を更新する
        /// </summary>
        public void Refresh() {
            if (boundDemo == null) {
                return;
            }
            OnRefresh(boundDemo.CurrentStepIndex);
        }

        /// <summary>
        /// 指定の要素をハイライトする
        /// </summary>
        /// <param name="targetId">ハイライト対象の識別子</param>
        public void Highlight(string targetId) {
            if (elements.TryGetValue(targetId, out VisualElement element)) {
                element.Pulse(HighlightColor, 0.5f);
            }
        }

        /// <summary>
        /// ビジュアライゼーションをリセットする
        /// </summary>
        public void Clear() {
            elements.Clear();
            arrows.Clear();
            if (renderer != null) {
                renderer.ClearAll();
            }
        }

        /// <summary>
        /// バインド時の初期表示を構築する（サブクラスで実装）
        /// </summary>
        /// <param name="demo">バインドされたデモ</param>
        protected abstract void OnBind(IPatternDemo demo);

        /// <summary>
        /// ステップ進行時の表示更新を行う（サブクラスで実装）
        /// </summary>
        /// <param name="stepIndex">現在のステップインデックス</param>
        protected abstract void OnRefresh(int stepIndex);

        /// <summary>
        /// 円形要素を追加する（既存IDの場合は既存要素を返す）
        /// </summary>
        /// <param name="id">要素の識別子</param>
        /// <param name="label">表示ラベル</param>
        /// <param name="position">配置位置</param>
        /// <param name="radius">半径</param>
        /// <param name="color">色</param>
        /// <returns>生成または既存のVisualElement</returns>
        protected VisualElement AddCircle(string id, string label, Vector2 position, float radius, Color color) {
            if (elements.TryGetValue(id, out VisualElement existing)) {
                return existing;
            }
            VisualElement element = VisualElement.CreateCircle(Root, id, label, position, radius, color);
            elements[id] = element;
            return element;
        }

        /// <summary>
        /// 矩形要素を追加する（既存IDの場合は既存要素を返す）
        /// </summary>
        /// <param name="id">要素の識別子</param>
        /// <param name="label">表示ラベル</param>
        /// <param name="position">配置位置</param>
        /// <param name="size">サイズ</param>
        /// <param name="color">色</param>
        /// <returns>生成または既存のVisualElement</returns>
        protected VisualElement AddRect(string id, string label, Vector2 position, Vector2 size, Color color) {
            if (elements.TryGetValue(id, out VisualElement existing)) {
                return existing;
            }
            VisualElement element = VisualElement.CreateRect(Root, id, label, position, size, color);
            elements[id] = element;
            return element;
        }

        /// <summary>
        /// 矢印を追加する（既存IDの場合は既存矢印を返す）
        /// </summary>
        /// <param name="id">矢印の識別子</param>
        /// <param name="from">接続元の要素</param>
        /// <param name="to">接続先の要素</param>
        /// <param name="color">色</param>
        /// <param name="hasArrow">矢印頭を表示するか</param>
        /// <returns>生成または既存のVisualArrow</returns>
        protected VisualArrow AddArrow(string id, VisualElement from, VisualElement to, Color color, bool hasArrow = true) {
            if (arrows.TryGetValue(id, out VisualArrow existing)) {
                return existing;
            }
            VisualArrow arrow = VisualArrow.Create(Root, from, to, color, hasArrow);
            arrows[id] = arrow;
            return arrow;
        }

        /// <summary>
        /// 要素を取得する（存在しない場合はnull）
        /// </summary>
        /// <param name="id">要素の識別子</param>
        /// <returns>VisualElementまたはnull</returns>
        protected VisualElement GetElement(string id) {
            elements.TryGetValue(id, out VisualElement element);
            return element;
        }

        /// <summary>
        /// 矢印を取得する（存在しない場合はnull）
        /// </summary>
        /// <param name="id">矢印の識別子</param>
        /// <returns>VisualArrowまたはnull</returns>
        protected VisualArrow GetArrow(string id) {
            arrows.TryGetValue(id, out VisualArrow arrow);
            return arrow;
        }
    }
}
