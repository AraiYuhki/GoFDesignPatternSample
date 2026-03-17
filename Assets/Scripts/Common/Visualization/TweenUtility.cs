using System;
using System.Collections;
using UnityEngine;

namespace DesignPatterns.Visualization
{
    /// <summary>
    /// コルーチンベースのシンプルなトゥイーンユーティリティ
    /// 位置・スケール・色・不透明度のアニメーションを提供する
    /// </summary>
    public static class TweenUtility
    {
        /// <summary>
        /// 位置を指定時間で移動するコルーチン
        /// </summary>
        /// <param name="target">移動対象のTransform</param>
        /// <param name="to">目標位置（ローカル座標）</param>
        /// <param name="duration">移動時間（秒）</param>
        /// <param name="onComplete">完了時コールバック</param>
        /// <returns>コルーチン</returns>
        public static IEnumerator MoveTo(Transform target, Vector3 to, float duration, Action onComplete = null)
        {
            Vector3 from = target.localPosition;
            float elapsed = 0f;
            while (elapsed < duration)
            {
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
        /// <param name="target">対象のTransform</param>
        /// <param name="to">目標スケール</param>
        /// <param name="duration">変更時間（秒）</param>
        /// <param name="onComplete">完了時コールバック</param>
        /// <returns>コルーチン</returns>
        public static IEnumerator ScaleTo(Transform target, Vector3 to, float duration, Action onComplete = null)
        {
            Vector3 from = target.localScale;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = EaseOutBack(Mathf.Clamp01(elapsed / duration));
                target.localScale = Vector3.Lerp(from, to, t);
                yield return null;
            }
            target.localScale = to;
            onComplete?.Invoke();
        }

        /// <summary>
        /// SpriteRendererの色を指定時間で変更するコルーチン
        /// </summary>
        /// <param name="renderer">対象のSpriteRenderer</param>
        /// <param name="to">目標色</param>
        /// <param name="duration">変更時間（秒）</param>
        /// <param name="onComplete">完了時コールバック</param>
        /// <returns>コルーチン</returns>
        public static IEnumerator ColorTo(SpriteRenderer renderer, Color to, float duration, Action onComplete = null)
        {
            Color from = renderer.color;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = EaseInOutQuad(Mathf.Clamp01(elapsed / duration));
                renderer.color = Color.Lerp(from, to, t);
                yield return null;
            }
            renderer.color = to;
            onComplete?.Invoke();
        }

        /// <summary>
        /// SpriteRendererの色をパルスアニメーションさせるコルーチン（変化して元に戻る）
        /// </summary>
        /// <param name="renderer">対象のSpriteRenderer</param>
        /// <param name="pulseColor">パルスのピーク色</param>
        /// <param name="duration">パルス全体の時間（秒）</param>
        /// <returns>コルーチン</returns>
        public static IEnumerator PulseColor(SpriteRenderer renderer, Color pulseColor, float duration)
        {
            Color original = renderer.color;
            float half = duration * 0.5f;

            float elapsed = 0f;
            while (elapsed < half)
            {
                elapsed += Time.deltaTime;
                float t = EaseInOutQuad(Mathf.Clamp01(elapsed / half));
                renderer.color = Color.Lerp(original, pulseColor, t);
                yield return null;
            }

            elapsed = 0f;
            while (elapsed < half)
            {
                elapsed += Time.deltaTime;
                float t = EaseInOutQuad(Mathf.Clamp01(elapsed / half));
                renderer.color = Color.Lerp(pulseColor, original, t);
                yield return null;
            }
            renderer.color = original;
        }

        /// <summary>
        /// フェードインアニメーション（アルファ 0→1）のコルーチン
        /// </summary>
        /// <param name="renderer">対象のSpriteRenderer</param>
        /// <param name="duration">フェード時間（秒）</param>
        /// <returns>コルーチン</returns>
        public static IEnumerator FadeIn(SpriteRenderer renderer, float duration)
        {
            Color color = renderer.color;
            color.a = 0f;
            renderer.color = color;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                color.a = t;
                renderer.color = color;
                yield return null;
            }
            color.a = 1f;
            renderer.color = color;
        }

        /// <summary>
        /// フェードアウトアニメーション（アルファ 1→0）のコルーチン
        /// </summary>
        /// <param name="renderer">対象のSpriteRenderer</param>
        /// <param name="duration">フェード時間（秒）</param>
        /// <param name="onComplete">完了時コールバック</param>
        /// <returns>コルーチン</returns>
        public static IEnumerator FadeOut(SpriteRenderer renderer, float duration, Action onComplete = null)
        {
            Color color = renderer.color;
            float startAlpha = color.a;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                color.a = Mathf.Lerp(startAlpha, 0f, t);
                renderer.color = color;
                yield return null;
            }
            color.a = 0f;
            renderer.color = color;
            onComplete?.Invoke();
        }

        /// <summary>
        /// 指定秒数待機するコルーチン
        /// </summary>
        /// <param name="seconds">待機秒数</param>
        /// <returns>コルーチン</returns>
        public static IEnumerator Wait(float seconds)
        {
            yield return new WaitForSeconds(seconds);
        }

        /// <summary>
        /// EaseInOutQuad補間関数
        /// </summary>
        /// <param name="t">進行度（0〜1）</param>
        /// <returns>補間された値</returns>
        private static float EaseInOutQuad(float t)
        {
            return t < 0.5f ? 2f * t * t : 1f - Mathf.Pow(-2f * t + 2f, 2f) * 0.5f;
        }

        /// <summary>
        /// EaseOutBack補間関数（オーバーシュートあり）
        /// </summary>
        /// <param name="t">進行度（0〜1）</param>
        /// <returns>補間された値</returns>
        private static float EaseOutBack(float t)
        {
            const float c1 = 1.70158f;
            const float c3 = c1 + 1f;
            return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
        }
    }
}
