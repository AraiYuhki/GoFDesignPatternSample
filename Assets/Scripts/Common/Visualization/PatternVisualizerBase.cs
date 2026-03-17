using System.Collections.Generic;
using UnityEngine;

namespace DesignPatterns.Visualization
{
    /// <summary>
    /// パターン固有のビジュアライゼーションを実装するための基底クラス
    /// VisualElementとVisualArrowの生成・管理を行い、
    /// 各パターンデモで2Dアニメーションを構築するためのヘルパーメソッドを提供する
    /// </summary>
    public abstract class PatternVisualizerBase : MonoBehaviour
    {
        /// <summary>ビジュアライゼーションレンダラーへの参照</summary>
        [SerializeField]
        private VisualizationRenderer visualizationRenderer;

        /// <summary>作成済みの要素をIDで管理する辞書</summary>
        private readonly Dictionary<string, VisualElement> elements = new Dictionary<string, VisualElement>();
        /// <summary>作成済みの矢印リスト</summary>
        private readonly List<VisualArrow> arrows = new List<VisualArrow>();

        /// <summary>レンダラーへのアクセサ</summary>
        protected VisualizationRenderer Renderer => visualizationRenderer;
        /// <summary>ビジュアライゼーション要素を配置する親Transform</summary>
        protected Transform VisualRoot => visualizationRenderer != null ? visualizationRenderer.VisualRoot : null;

        /// <summary>
        /// 円形の要素を作成する
        /// </summary>
        /// <param name="id">一意な識別子</param>
        /// <param name="label">表示ラベル</param>
        /// <param name="position">位置（ワールドオフセット相対）</param>
        /// <param name="radius">半径</param>
        /// <param name="color">色</param>
        /// <returns>作成されたVisualElement</returns>
        protected VisualElement CreateCircle(string id, string label, Vector2 position, float radius, Color color)
        {
            if (VisualRoot == null)
            {
                return null;
            }
            var element = VisualElement.CreateCircle(VisualRoot, id, label, position, radius, color);
            elements[id] = element;
            return element;
        }

        /// <summary>
        /// 矩形の要素を作成する
        /// </summary>
        /// <param name="id">一意な識別子</param>
        /// <param name="label">表示ラベル</param>
        /// <param name="position">位置（ワールドオフセット相対）</param>
        /// <param name="size">サイズ（幅, 高さ）</param>
        /// <param name="color">色</param>
        /// <returns>作成されたVisualElement</returns>
        protected VisualElement CreateRect(string id, string label, Vector2 position, Vector2 size, Color color)
        {
            if (VisualRoot == null)
            {
                return null;
            }
            var element = VisualElement.CreateRect(VisualRoot, id, label, position, size, color);
            elements[id] = element;
            return element;
        }

        /// <summary>
        /// 2つの要素間に矢印を作成する
        /// </summary>
        /// <param name="from">接続元要素</param>
        /// <param name="to">接続先要素</param>
        /// <param name="color">矢印の色</param>
        /// <param name="hasArrowHead">矢印頭を表示するかどうか</param>
        /// <returns>作成されたVisualArrow</returns>
        protected VisualArrow CreateArrow(VisualElement from, VisualElement to, Color color, bool hasArrowHead = true)
        {
            if (VisualRoot == null)
            {
                return null;
            }
            var arrow = VisualArrow.Create(VisualRoot, from, to, color, hasArrowHead);
            arrows.Add(arrow);
            return arrow;
        }

        /// <summary>
        /// IDから要素を取得する
        /// </summary>
        /// <param name="id">要素のID</param>
        /// <returns>見つかったVisualElement（見つからない場合はnull）</returns>
        protected VisualElement GetElement(string id)
        {
            elements.TryGetValue(id, out var element);
            return element;
        }

        /// <summary>
        /// 指定IDの要素を削除する
        /// </summary>
        /// <param name="id">削除する要素のID</param>
        protected void RemoveElement(string id)
        {
            if (!elements.TryGetValue(id, out var element))
            {
                return;
            }
            elements.Remove(id);

            // 関連する矢印も削除する
            for (int i = arrows.Count - 1; i >= 0; i--)
            {
                if (arrows[i] == null)
                {
                    arrows.RemoveAt(i);
                    continue;
                }
                // 矢印の接続先が削除された要素の場合、矢印も削除する
                // VisualArrow内部でnullチェックされるが、明示的に削除する
            }

            if (element != null)
            {
                Destroy(element.gameObject);
            }
        }

        /// <summary>
        /// 全ての要素と矢印をクリアする
        /// </summary>
        protected void ClearAll()
        {
            foreach (var pair in elements)
            {
                if (pair.Value != null)
                {
                    Destroy(pair.Value.gameObject);
                }
            }
            elements.Clear();

            foreach (var arrow in arrows)
            {
                if (arrow != null)
                {
                    Destroy(arrow.gameObject);
                }
            }
            arrows.Clear();
        }

        /// <summary>
        /// ビジュアライゼーションの初期化を行う
        /// サブクラスでオーバーライドして、パターン固有の要素を作成する
        /// </summary>
        public abstract void Setup();

        private void OnDestroy()
        {
            ClearAll();
        }
    }
}
