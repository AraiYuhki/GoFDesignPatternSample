using System;
using System.Collections;
using UnityEngine;

namespace GoFPatterns.Patterns.Visualization {
    /// <summary>
    /// コルーチンベースのシンプルなトゥイーンユーティリティ
    /// 位置・スケール・色・不透明度のアニメーションを提供する
    /// </summary>
    public static class TweenUtility {
        /// <summary>
        /// 位置を指定時間で移動するコルーチン
        /// </summary>
        public static IEnumerator MoveTo(Transform target, Vector3 to, float duration, Action onComplete = null) {
            Vector3 from = target.localPosition;
            float elapsed = 0f;
            while (elapsed < duration) {
                elapsed += Time.deltaTime;
                float t = EaseInOutQuad(Mathf.Clamp01(elapsed / duration));
                target.localPosition = Vector3.Lerp(from, to, t);
                yield return null;
            }
            target.localPosition = to;
            onComplete?.Invoke();
        }

        /// <summary>
        /// スケールを指定時間で変更するコルーチン
        /// </summary>
        public static IEnumerator ScaleTo(Transform target, Vector3 to, float duration, Action onComplete = null) {
            Vector3 from = target.localScale;
            float elapsed = 0f;
            while (elapsed < duration) {
                elapsed += Time.deltaTime;
                float t = EaseOutBack(Mathf.Clamp01(elapsed / duration));
                target.localScale = Vector3.Lerp(from, to, t);
                yield return null;
            }
            target.localScale = to;
            onComplete?.Invoke();
        }

        /// <summary>
        /// SpriteRendererの色をパルスアニメーションさせるコルーチン
        /// </summary>
        public static IEnumerator PulseColor(SpriteRenderer renderer, Color pulseColor, float duration) {
            Color original = renderer.color;
            float half = duration * 0.5f;
            float elapsed = 0f;

            while (elapsed < half) {
                elapsed += Time.deltaTime;
                float t = EaseInOutQuad(Mathf.Clamp01(elapsed / half));
                renderer.color = Color.Lerp(original, pulseColor, t);
                yield return null;
            }
            elapsed = 0f;
            while (elapsed < half) {
                elapsed += Time.deltaTime;
                float t = EaseInOutQuad(Mathf.Clamp01(elapsed / half));
                renderer.color = Color.Lerp(pulseColor, original, t);
                yield return null;
            }
            renderer.color = original;
        }

        /// <summary>
        /// SpriteRendererの色を指定時間で変更するコルーチン
        /// </summary>
        public static IEnumerator ColorTo(SpriteRenderer renderer, Color to, float duration) {
            Color from = renderer.color;
            float elapsed = 0f;
            while (elapsed < duration) {
                elapsed += Time.deltaTime;
                float t = EaseInOutQuad(Mathf.Clamp01(elapsed / duration));
                renderer.color = Color.Lerp(from, to, t);
                yield return null;
            }
            renderer.color = to;
        }

        private static float EaseInOutQuad(float t) {
            return t < 0.5f ? 2f * t * t : 1f - Mathf.Pow(-2f * t + 2f, 2f) * 0.5f;
        }

        private static float EaseOutBack(float t) {
            const float c1 = 1.70158f;
            const float c3 = c1 + 1f;
            return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
        }
    }
}
