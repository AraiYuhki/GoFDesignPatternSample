using System.Collections;
using TMPro;
using UnityEngine;

namespace GoFPatterns.Patterns.Visualization {
    /// <summary>
    /// ビジュアライゼーション上の2D要素
    /// スプライト（円/矩形）とラベルを持ち、アニメーション機能を提供する
    /// </summary>
    public class VisualElement : MonoBehaviour {
        /// <summary>メインのスプライトレンダラー</summary>
        private SpriteRenderer spriteRenderer;
        /// <summary>ラベル表示用のTextMeshPro</summary>
        private TextMeshPro labelText;
        /// <summary>要素の識別子</summary>
        private string elementId;

        /// <summary>要素のIDを取得する</summary>
        public string Id => elementId;
        /// <summary>ワールド座標での位置を取得する</summary>
        public Vector3 WorldPosition => transform.position;
        /// <summary>SpriteRendererへのアクセサ</summary>
        public SpriteRenderer Renderer => spriteRenderer;

        /// <summary>
        /// 円形の要素を生成する
        /// </summary>
        public static VisualElement CreateCircle(Transform parent, string id, string label, Vector2 position, float radius, Color color) {
            var go = new GameObject($"VE_{id}");
            go.transform.SetParent(parent, false);
            go.transform.localPosition = new Vector3(position.x, position.y, 0f);

            var element = go.AddComponent<VisualElement>();
            element.elementId = id;
            element.SetupSprite(ShapeFactory.GetCircle(), color, new Vector3(radius * 2f, radius * 2f, 1f));
            element.SetupLabel(label);
            return element;
        }

        /// <summary>
        /// 矩形の要素を生成する
        /// </summary>
        public static VisualElement CreateRect(Transform parent, string id, string label, Vector2 position, Vector2 size, Color color) {
            var go = new GameObject($"VE_{id}");
            go.transform.SetParent(parent, false);
            go.transform.localPosition = new Vector3(position.x, position.y, 0f);

            var element = go.AddComponent<VisualElement>();
            element.elementId = id;
            element.SetupSprite(ShapeFactory.GetRect(), color, new Vector3(size.x, size.y, 1f));
            element.SetupLabel(label);
            return element;
        }

        /// <summary>位置をアニメーション付きで移動する</summary>
        public Coroutine MoveTo(Vector2 target, float duration) {
            return StartCoroutine(TweenUtility.MoveTo(transform, new Vector3(target.x, target.y, 0f), duration));
        }

        /// <summary>色をパルスアニメーションさせる</summary>
        public Coroutine Pulse(Color pulseColor, float duration) {
            return StartCoroutine(TweenUtility.PulseColor(spriteRenderer, pulseColor, duration));
        }

        /// <summary>色を即座に設定する</summary>
        public void SetColorImmediate(Color color) {
            if (spriteRenderer != null) {
                spriteRenderer.color = color;
            }
        }

        /// <summary>ラベルテキストを更新する</summary>
        public void SetLabel(string text) {
            if (labelText != null) {
                labelText.text = text;
            }
        }

        /// <summary>表示/非表示を切り替える</summary>
        public void SetVisible(bool visible) {
            gameObject.SetActive(visible);
        }

        private void SetupSprite(Sprite sprite, Color color, Vector3 scale) {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
            spriteRenderer.color = color;
            spriteRenderer.sortingOrder = 1;
            transform.localScale = scale;
        }

        private void SetupLabel(string text) {
            if (string.IsNullOrEmpty(text)) {
                return;
            }
            var labelGo = new GameObject("Label");
            labelGo.transform.SetParent(transform, false);
            Vector3 parentScale = transform.localScale;
            float invScaleX = parentScale.x > 0.001f ? 1f / parentScale.x : 1f;
            float invScaleY = parentScale.y > 0.001f ? 1f / parentScale.y : 1f;
            labelGo.transform.localScale = new Vector3(invScaleX, invScaleY, 1f);
            labelGo.transform.localPosition = Vector3.zero;

            labelText = labelGo.AddComponent<TextMeshPro>();
            labelText.text = text;
            labelText.fontSize = 3f;
            labelText.alignment = TextAlignmentOptions.Center;
            labelText.color = Color.white;
            labelText.sortingOrder = 2;
            labelText.rectTransform.sizeDelta = new Vector2(4f, 1.5f);
        }
    }
}
