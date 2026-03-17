using System.Collections;
using UnityEngine;

namespace DesignPatterns.Visualization
{
    /// <summary>
    /// 2Dビジュアライゼーション上の矢印/接続線
    /// 2つのVisualElement間、または2つの座標間を線と矢印で接続する
    /// LineRendererを使用してワールドスペースで描画する
    /// </summary>
    public class VisualArrow : MonoBehaviour
    {
        /// <summary>線を描画するLineRenderer</summary>
        private LineRenderer lineRenderer;
        /// <summary>矢印頭のSpriteRenderer</summary>
        private SpriteRenderer arrowHead;
        /// <summary>接続元のVisualElement（nullの場合は固定座標）</summary>
        private VisualElement fromElement;
        /// <summary>接続先のVisualElement（nullの場合は固定座標）</summary>
        private VisualElement toElement;
        /// <summary>接続元の固定座標</summary>
        private Vector3 fromPosition;
        /// <summary>接続先の固定座標</summary>
        private Vector3 toPosition;
        /// <summary>矢印を表示するかどうか</summary>
        private bool showArrowHead;
        /// <summary>要素を追従するかどうか</summary>
        private bool trackElements;
        /// <summary>矢印頭のサイズ</summary>
        private const float ArrowHeadSize = 0.2f;
        /// <summary>デフォルトの線の太さ</summary>
        private const float DefaultWidth = 0.04f;

        /// <summary>LineRendererの色を取得する</summary>
        public Color CurrentColor => lineRenderer != null ? lineRenderer.startColor : Color.white;

        /// <summary>
        /// 2つのVisualElement間に矢印を生成する
        /// </summary>
        /// <param name="parent">親Transform</param>
        /// <param name="from">接続元要素</param>
        /// <param name="to">接続先要素</param>
        /// <param name="color">線の色</param>
        /// <param name="hasArrow">矢印頭を表示するかどうか</param>
        /// <returns>生成されたVisualArrow</returns>
        public static VisualArrow Create(Transform parent, VisualElement from, VisualElement to, Color color, bool hasArrow = true)
        {
            var go = new GameObject($"Arrow_{from.Id}_to_{to.Id}");
            go.transform.SetParent(parent, false);

            var arrow = go.AddComponent<VisualArrow>();
            arrow.fromElement = from;
            arrow.toElement = to;
            arrow.showArrowHead = hasArrow;
            arrow.trackElements = true;
            arrow.SetupLineRenderer(color);
            if (hasArrow)
            {
                arrow.SetupArrowHead(color);
            }
            arrow.UpdatePositions();
            return arrow;
        }

        /// <summary>
        /// 2つの座標間に矢印を生成する
        /// </summary>
        /// <param name="parent">親Transform</param>
        /// <param name="from">始点座標</param>
        /// <param name="to">終点座標</param>
        /// <param name="color">線の色</param>
        /// <param name="hasArrow">矢印頭を表示するかどうか</param>
        /// <returns>生成されたVisualArrow</returns>
        public static VisualArrow CreateFromPositions(Transform parent, Vector3 from, Vector3 to, Color color, bool hasArrow = true)
        {
            var go = new GameObject("Arrow_pos");
            go.transform.SetParent(parent, false);

            var arrow = go.AddComponent<VisualArrow>();
            arrow.fromPosition = from;
            arrow.toPosition = to;
            arrow.showArrowHead = hasArrow;
            arrow.trackElements = false;
            arrow.SetupLineRenderer(color);
            if (hasArrow)
            {
                arrow.SetupArrowHead(color);
            }
            arrow.UpdatePositions();
            return arrow;
        }

        /// <summary>
        /// 線の色を設定する
        /// </summary>
        /// <param name="color">設定する色</param>
        public void SetColor(Color color)
        {
            if (lineRenderer != null)
            {
                lineRenderer.startColor = color;
                lineRenderer.endColor = color;
            }
            if (arrowHead != null)
            {
                arrowHead.color = color;
            }
        }

        /// <summary>
        /// 色をパルスアニメーションさせる（変化して元に戻る）
        /// </summary>
        /// <param name="pulseColor">パルスのピーク色</param>
        /// <param name="duration">パルス全体の時間（秒）</param>
        /// <returns>コルーチン</returns>
        public Coroutine Pulse(Color pulseColor, float duration)
        {
            return StartCoroutine(PulseCoroutine(pulseColor, duration));
        }

        /// <summary>
        /// 不透明度を即座に設定する
        /// </summary>
        /// <param name="alpha">不透明度（0〜1）</param>
        public void SetAlpha(float alpha)
        {
            if (lineRenderer != null)
            {
                Color c = lineRenderer.startColor;
                c.a = alpha;
                lineRenderer.startColor = c;
                lineRenderer.endColor = c;
            }
            if (arrowHead != null)
            {
                Color c = arrowHead.color;
                c.a = alpha;
                arrowHead.color = c;
            }
        }

        private void LateUpdate()
        {
            if (trackElements)
            {
                UpdatePositions();
            }
        }

        /// <summary>
        /// 線と矢印の位置を更新する
        /// </summary>
        private void UpdatePositions()
        {
            Vector3 start;
            Vector3 end;

            if (trackElements)
            {
                if (fromElement == null || toElement == null)
                {
                    return;
                }
                start = fromElement.WorldPosition;
                end = toElement.WorldPosition;
            }
            else
            {
                start = fromPosition;
                end = toPosition;
            }

            Vector3 direction = (end - start).normalized;
            float distance = Vector3.Distance(start, end);

            // 要素の半径分だけ内側にオフセットする
            float startOffset = trackElements && fromElement != null ? GetElementRadius(fromElement) : 0f;
            float endOffset = trackElements && toElement != null ? GetElementRadius(toElement) : 0f;

            if (showArrowHead)
            {
                endOffset += ArrowHeadSize;
            }

            if (distance > startOffset + endOffset)
            {
                start += direction * startOffset;
                end -= direction * endOffset;
            }

            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);

            if (arrowHead != null)
            {
                Vector3 arrowPos = end + direction * ArrowHeadSize * 0.5f;
                arrowHead.transform.position = arrowPos;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                arrowHead.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }
        }

        /// <summary>
        /// VisualElementの概算半径を取得する
        /// </summary>
        /// <param name="element">対象の要素</param>
        /// <returns>概算半径</returns>
        private static float GetElementRadius(VisualElement element)
        {
            Vector3 scale = element.transform.localScale;
            return Mathf.Max(scale.x, scale.y) * 0.5f;
        }

        /// <summary>
        /// LineRendererのセットアップを行う
        /// </summary>
        /// <param name="color">線の色</param>
        private void SetupLineRenderer(Color color)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.positionCount = 2;
            lineRenderer.startWidth = DefaultWidth;
            lineRenderer.endWidth = DefaultWidth;
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
            lineRenderer.useWorldSpace = true;
            lineRenderer.sortingOrder = 0;

            // デフォルトのスプライトマテリアルを使用する
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        }

        /// <summary>
        /// 矢印頭のセットアップを行う
        /// </summary>
        /// <param name="color">矢印の色</param>
        private void SetupArrowHead(Color color)
        {
            var arrowGo = new GameObject("ArrowHead");
            arrowGo.transform.SetParent(transform, false);
            arrowGo.transform.localScale = new Vector3(ArrowHeadSize, ArrowHeadSize, 1f);

            arrowHead = arrowGo.AddComponent<SpriteRenderer>();
            arrowHead.sprite = ShapeFactory.GetTriangleSprite();
            arrowHead.color = color;
            arrowHead.sortingOrder = 0;
        }

        /// <summary>
        /// パルスアニメーションのコルーチン
        /// </summary>
        private IEnumerator PulseCoroutine(Color pulseColor, float duration)
        {
            Color original = CurrentColor;
            float half = duration * 0.5f;

            float elapsed = 0f;
            while (elapsed < half)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / half);
                SetColor(Color.Lerp(original, pulseColor, t));
                yield return null;
            }

            elapsed = 0f;
            while (elapsed < half)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / half);
                SetColor(Color.Lerp(pulseColor, original, t));
                yield return null;
            }
            SetColor(original);
        }
    }
}
