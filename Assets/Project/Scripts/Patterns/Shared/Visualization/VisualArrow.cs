using System.Collections;
using UnityEngine;

namespace GoFPatterns.Patterns.Visualization {
    /// <summary>
    /// 2Dビジュアライゼーション上の矢印/接続線
    /// 2つのVisualElement間を線と矢印で接続する
    /// </summary>
    public class VisualArrow : MonoBehaviour {
        /// <summary>線を描画するLineRenderer</summary>
        private LineRenderer lineRenderer;
        /// <summary>矢印頭のSpriteRenderer</summary>
        private SpriteRenderer arrowHead;
        /// <summary>接続元のVisualElement</summary>
        private VisualElement fromElement;
        /// <summary>接続先のVisualElement</summary>
        private VisualElement toElement;
        /// <summary>矢印頭のサイズ</summary>
        private const float ArrowHeadSize = 0.2f;
        /// <summary>デフォルトの線の太さ</summary>
        private const float DefaultWidth = 0.04f;

        /// <summary>現在の線の色を取得する</summary>
        public Color CurrentColor => lineRenderer != null ? lineRenderer.startColor : Color.white;

        /// <summary>
        /// 2つのVisualElement間に矢印を生成する
        /// </summary>
        public static VisualArrow Create(Transform parent, VisualElement from, VisualElement to, Color color, bool hasArrow = true) {
            var go = new GameObject($"Arrow_{from.Id}_to_{to.Id}");
            go.transform.SetParent(parent, false);

            var arrow = go.AddComponent<VisualArrow>();
            arrow.fromElement = from;
            arrow.toElement = to;
            arrow.SetupLineRenderer(color);
            if (hasArrow) {
                arrow.SetupArrowHead(color);
            }
            arrow.UpdatePositions();
            return arrow;
        }

        /// <summary>線の色を設定する</summary>
        public void SetColor(Color color) {
            if (lineRenderer != null) {
                lineRenderer.startColor = color;
                lineRenderer.endColor = color;
            }
            if (arrowHead != null) {
                arrowHead.color = color;
            }
        }

        /// <summary>色をパルスアニメーションさせる</summary>
        public Coroutine Pulse(Color pulseColor, float duration) {
            return StartCoroutine(PulseCoroutine(pulseColor, duration));
        }

        private void LateUpdate() {
            UpdatePositions();
        }

        private void UpdatePositions() {
            if (fromElement == null || toElement == null) {
                return;
            }
            Vector3 start = fromElement.WorldPosition;
            Vector3 end = toElement.WorldPosition;
            Vector3 direction = (end - start).normalized;
            float distance = Vector3.Distance(start, end);

            float startOffset = GetElementRadius(fromElement);
            float endOffset = GetElementRadius(toElement) + ArrowHeadSize;

            if (distance > startOffset + endOffset) {
                start += direction * startOffset;
                end -= direction * endOffset;
            }

            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);

            if (arrowHead != null) {
                arrowHead.transform.position = end + direction * ArrowHeadSize * 0.5f;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                arrowHead.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }
        }

        private static float GetElementRadius(VisualElement element) {
            Vector3 scale = element.transform.localScale;
            return Mathf.Max(scale.x, scale.y) * 0.5f;
        }

        private void SetupLineRenderer(Color color) {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.positionCount = 2;
            lineRenderer.startWidth = DefaultWidth;
            lineRenderer.endWidth = DefaultWidth;
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
            lineRenderer.useWorldSpace = true;
            lineRenderer.sortingOrder = 0;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        }

        private void SetupArrowHead(Color color) {
            var arrowGo = new GameObject("ArrowHead");
            arrowGo.transform.SetParent(transform, false);
            arrowGo.transform.localScale = new Vector3(ArrowHeadSize, ArrowHeadSize, 1f);
            arrowHead = arrowGo.AddComponent<SpriteRenderer>();
            arrowHead.sprite = ShapeFactory.GetTriangle();
            arrowHead.color = color;
            arrowHead.sortingOrder = 0;
        }

        private IEnumerator PulseCoroutine(Color pulseColor, float duration) {
            Color original = CurrentColor;
            float half = duration * 0.5f;
            float elapsed = 0f;

            while (elapsed < half) {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / half);
                SetColor(Color.Lerp(original, pulseColor, t));
                yield return null;
            }
            elapsed = 0f;
            while (elapsed < half) {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / half);
                SetColor(Color.Lerp(pulseColor, original, t));
                yield return null;
            }
            SetColor(original);
        }
    }
}
