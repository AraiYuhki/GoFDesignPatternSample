using System.Collections;
using TMPro;
using UnityEngine;

namespace DesignPatterns.Visualization
{
    /// <summary>
    /// ビジュアライゼーション上の2D要素
    /// スプライト（円/矩形）とラベルを持ち、アニメーション機能を提供する
    /// パターンの動作を視覚的に表現するための基本単位
    /// </summary>
    public class VisualElement : MonoBehaviour
    {
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
        /// <param name="parent">親Transform</param>
        /// <param name="id">一意な識別子</param>
        /// <param name="label">表示ラベル</param>
        /// <param name="position">ローカル座標</param>
        /// <param name="radius">半径</param>
        /// <param name="color">色</param>
        /// <returns>生成されたVisualElement</returns>
        public static VisualElement CreateCircle(Transform parent, string id, string label, Vector2 position, float radius, Color color)
        {
            var go = new GameObject($"VE_{id}");
            go.transform.SetParent(parent, false);
            go.transform.localPosition = new Vector3(position.x, position.y, 0f);

            var element = go.AddComponent<VisualElement>();
            element.elementId = id;
            element.SetupSprite(ShapeFactory.GetCircleSprite(), color, new Vector3(radius * 2f, radius * 2f, 1f));
            element.SetupLabel(label);
            return element;
        }

        /// <summary>
        /// 矩形の要素を生成する
        /// </summary>
        /// <param name="parent">親Transform</param>
        /// <param name="id">一意な識別子</param>
        /// <param name="label">表示ラベル</param>
        /// <param name="position">ローカル座標</param>
        /// <param name="size">サイズ（幅, 高さ）</param>
        /// <param name="color">色</param>
        /// <returns>生成されたVisualElement</returns>
        public static VisualElement CreateRect(Transform parent, string id, string label, Vector2 position, Vector2 size, Color color)
        {
            var go = new GameObject($"VE_{id}");
            go.transform.SetParent(parent, false);
            go.transform.localPosition = new Vector3(position.x, position.y, 0f);

            var element = go.AddComponent<VisualElement>();
            element.elementId = id;
            element.SetupSprite(ShapeFactory.GetRectSprite(), color, new Vector3(size.x, size.y, 1f));
            element.SetupLabel(label);
            return element;
        }

        /// <summary>
        /// 位置をアニメーション付きで移動する
        /// </summary>
        /// <param name="target">目標位置（ローカル座標）</param>
        /// <param name="duration">移動時間（秒）</param>
        /// <returns>コルーチン</returns>
        public Coroutine MoveTo(Vector2 target, float duration)
        {
            return StartCoroutine(TweenUtility.MoveTo(transform, new Vector3(target.x, target.y, 0f), duration));
        }

        /// <summary>
        /// スケールをアニメーション付きで変更する
        /// </summary>
        /// <param name="scale">目標スケール</param>
        /// <param name="duration">変更時間（秒）</param>
        /// <returns>コルーチン</returns>
        public Coroutine ScaleTo(float scale, float duration)
        {
            return StartCoroutine(TweenUtility.ScaleTo(transform, Vector3.one * scale, duration));
        }

        /// <summary>
        /// 色をアニメーション付きで変更する
        /// </summary>
        /// <param name="color">目標色</param>
        /// <param name="duration">変更時間（秒）</param>
        /// <returns>コルーチン</returns>
        public Coroutine SetColor(Color color, float duration)
        {
            return StartCoroutine(TweenUtility.ColorTo(spriteRenderer, color, duration));
        }

        /// <summary>
        /// 色をパルスアニメーションさせる（変化して元に戻る）
        /// </summary>
        /// <param name="pulseColor">パルスのピーク色</param>
        /// <param name="duration">パルス全体の時間（秒）</param>
        /// <returns>コルーチン</returns>
        public Coroutine Pulse(Color pulseColor, float duration)
        {
            return StartCoroutine(TweenUtility.PulseColor(spriteRenderer, pulseColor, duration));
        }

        /// <summary>
        /// フェードインアニメーションを実行する
        /// </summary>
        /// <param name="duration">フェード時間（秒）</param>
        /// <returns>コルーチン</returns>
        public Coroutine FadeIn(float duration)
        {
            return StartCoroutine(FadeInAll(duration));
        }

        /// <summary>
        /// フェードアウトアニメーションを実行する
        /// </summary>
        /// <param name="duration">フェード時間（秒）</param>
        /// <returns>コルーチン</returns>
        public Coroutine FadeOut(float duration)
        {
            return StartCoroutine(FadeOutAll(duration));
        }

        /// <summary>
        /// 色を即座に設定する
        /// </summary>
        /// <param name="color">設定する色</param>
        public void SetColorImmediate(Color color)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = color;
            }
        }

        /// <summary>
        /// ラベルテキストを更新する
        /// </summary>
        /// <param name="text">表示するテキスト</param>
        public void SetLabel(string text)
        {
            if (labelText != null)
            {
                labelText.text = text;
            }
        }

        /// <summary>
        /// スプライトのセットアップを行う
        /// </summary>
        /// <param name="sprite">使用するSprite</param>
        /// <param name="color">初期色</param>
        /// <param name="scale">スケール</param>
        private void SetupSprite(Sprite sprite, Color color, Vector3 scale)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
            spriteRenderer.color = color;
            spriteRenderer.sortingOrder = 1;
            transform.localScale = scale;
        }

        /// <summary>
        /// ラベルのセットアップを行う
        /// </summary>
        /// <param name="text">ラベルテキスト</param>
        private void SetupLabel(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            var labelGo = new GameObject("Label");
            labelGo.transform.SetParent(transform, false);
            // スプライトのスケールを打ち消すためにラベルのスケールを逆算する
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

        /// <summary>
        /// スプライトとラベルを同時にフェードインさせるコルーチン
        /// </summary>
        private IEnumerator FadeInAll(float duration)
        {
            SetAlphaImmediate(0f);
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                SetAlphaImmediate(t);
                yield return null;
            }
            SetAlphaImmediate(1f);
        }

        /// <summary>
        /// スプライトとラベルを同時にフェードアウトさせるコルーチン
        /// </summary>
        private IEnumerator FadeOutAll(float duration)
        {
            float startAlpha = spriteRenderer != null ? spriteRenderer.color.a : 1f;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                SetAlphaImmediate(Mathf.Lerp(startAlpha, 0f, t));
                yield return null;
            }
            SetAlphaImmediate(0f);
        }

        /// <summary>
        /// スプライトとラベルの不透明度を即座に設定する
        /// </summary>
        /// <param name="alpha">不透明度（0〜1）</param>
        private void SetAlphaImmediate(float alpha)
        {
            if (spriteRenderer != null)
            {
                Color c = spriteRenderer.color;
                c.a = alpha;
                spriteRenderer.color = c;
            }
            if (labelText != null)
            {
                Color c = labelText.color;
                c.a = alpha;
                labelText.color = c;
            }
        }
    }
}
