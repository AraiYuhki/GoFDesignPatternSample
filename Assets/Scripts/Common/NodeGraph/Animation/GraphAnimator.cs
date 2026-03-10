using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPatterns.NodeGraph {
    /// <summary>
    /// グラフのノードとエッジにアニメーションを適用するユーティリティクラス
    /// コルーチンベースでパルス・生成・破棄・シーケンスアニメーションを実行する
    /// </summary>
    public class GraphAnimator {
        /// <summary>コルーチン実行用のMonoBehaviourホスト</summary>
        private readonly MonoBehaviour host;
        /// <summary>実行中のコルーチンのリスト</summary>
        private readonly List<Coroutine> activeCoroutines = new List<Coroutine>();

        /// <summary>デフォルトのアニメーション時間（秒）</summary>
        private const float DefaultDuration = 0.4f;
        /// <summary>生成アニメーションのオーバーシュート量</summary>
        private const float CreationOvershoot = 1.15f;

        /// <summary>
        /// GraphAnimatorを生成する
        /// </summary>
        /// <param name="host">コルーチンを実行するMonoBehaviour</param>
        public GraphAnimator(MonoBehaviour host) {
            this.host = host;
        }

        /// <summary>
        /// ノードをパルスアニメーションさせる（色が変わって元に戻る）
        /// </summary>
        /// <param name="nodeView">対象のノードビュー</param>
        /// <param name="targetColor">パルスのピーク色</param>
        /// <param name="duration">アニメーション時間（秒）</param>
        /// <returns>実行中のコルーチン</returns>
        public Coroutine PulseNode(GraphNodeView nodeView, Color targetColor, float duration = DefaultDuration) {
            var coroutine = host.StartCoroutine(PulseNodeCoroutine(nodeView, targetColor, duration));
            activeCoroutines.Add(coroutine);
            return coroutine;
        }

        /// <summary>
        /// エッジをパルスアニメーションさせる（色と太さが変わって元に戻る）
        /// </summary>
        /// <param name="edgeView">対象のエッジビュー</param>
        /// <param name="targetColor">パルスのピーク色</param>
        /// <param name="duration">アニメーション時間（秒）</param>
        /// <returns>実行中のコルーチン</returns>
        public Coroutine PulseEdge(GraphEdgeView edgeView, Color targetColor, float duration = DefaultDuration) {
            var coroutine = host.StartCoroutine(PulseEdgeCoroutine(edgeView, targetColor, duration));
            activeCoroutines.Add(coroutine);
            return coroutine;
        }

        /// <summary>
        /// ノードの生成アニメーションを実行する（スケール0→1、フェードイン）
        /// </summary>
        /// <param name="nodeView">対象のノードビュー</param>
        /// <param name="duration">アニメーション時間（秒）</param>
        /// <returns>実行中のコルーチン</returns>
        public Coroutine AnimateCreation(GraphNodeView nodeView, float duration = DefaultDuration) {
            var coroutine = host.StartCoroutine(CreationCoroutine(nodeView, duration));
            activeCoroutines.Add(coroutine);
            return coroutine;
        }

        /// <summary>
        /// ノードの破棄アニメーションを実行する（スケール1→0、フェードアウト）
        /// </summary>
        /// <param name="nodeView">対象のノードビュー</param>
        /// <param name="duration">アニメーション時間（秒）</param>
        /// <param name="onComplete">完了時のコールバック</param>
        /// <returns>実行中のコルーチン</returns>
        public Coroutine AnimateDestruction(GraphNodeView nodeView, float duration = DefaultDuration, Action onComplete = null) {
            var coroutine = host.StartCoroutine(DestructionCoroutine(nodeView, duration, onComplete));
            activeCoroutines.Add(coroutine);
            return coroutine;
        }

        /// <summary>
        /// 複数のエッジを順番にハイライトするシーケンスアニメーション
        /// </summary>
        /// <param name="steps">エッジビューと表示時間のペアのリスト</param>
        /// <returns>実行中のコルーチン</returns>
        public Coroutine HighlightSequence(List<(GraphEdgeView edgeView, float duration)> steps) {
            var coroutine = host.StartCoroutine(HighlightSequenceCoroutine(steps));
            activeCoroutines.Add(coroutine);
            return coroutine;
        }

        /// <summary>
        /// 実行中のすべてのアニメーションを停止する
        /// </summary>
        public void StopAll() {
            foreach (var coroutine in activeCoroutines) {
                if (coroutine != null) {
                    host.StopCoroutine(coroutine);
                }
            }
            activeCoroutines.Clear();
        }

        /// <summary>
        /// ノードパルスアニメーションのコルーチン
        /// </summary>
        private IEnumerator PulseNodeCoroutine(GraphNodeView nodeView, Color targetColor, float duration) {
            float halfDuration = duration * 0.5f;
            Color originalBg = new Color(0.2f, 0.2f, 0.2f, 0.9f);

            // 前半：ターゲット色へ
            float elapsed = 0f;
            while (elapsed < halfDuration) {
                elapsed += Time.deltaTime;
                float t = EaseInOut(Mathf.Clamp01(elapsed / halfDuration));
                nodeView.SetBackgroundColor(Color.Lerp(originalBg, targetColor, t));
                yield return null;
            }

            // 後半：元の色へ
            elapsed = 0f;
            while (elapsed < halfDuration) {
                elapsed += Time.deltaTime;
                float t = EaseInOut(Mathf.Clamp01(elapsed / halfDuration));
                nodeView.SetBackgroundColor(Color.Lerp(targetColor, originalBg, t));
                yield return null;
            }

            nodeView.SetBackgroundColor(originalBg);
        }

        /// <summary>
        /// エッジパルスアニメーションのコルーチン
        /// </summary>
        private IEnumerator PulseEdgeCoroutine(GraphEdgeView edgeView, Color targetColor, float duration) {
            float halfDuration = duration * 0.5f;
            Color originalColor = new Color(0.6f, 0.6f, 0.6f, 1f);

            float elapsed = 0f;
            while (elapsed < halfDuration) {
                elapsed += Time.deltaTime;
                float t = EaseInOut(Mathf.Clamp01(elapsed / halfDuration));
                edgeView.SetColor(Color.Lerp(originalColor, targetColor, t));
                yield return null;
            }

            elapsed = 0f;
            while (elapsed < halfDuration) {
                elapsed += Time.deltaTime;
                float t = EaseInOut(Mathf.Clamp01(elapsed / halfDuration));
                edgeView.SetColor(Color.Lerp(targetColor, originalColor, t));
                yield return null;
            }

            edgeView.SetColor(originalColor);
        }

        /// <summary>
        /// 生成アニメーションのコルーチン
        /// </summary>
        private IEnumerator CreationCoroutine(GraphNodeView nodeView, float duration) {
            nodeView.SetScale(Vector3.zero);
            nodeView.SetAlpha(0f);

            float elapsed = 0f;
            while (elapsed < duration) {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                float scale = EaseOutBack(t, CreationOvershoot);
                nodeView.SetScale(Vector3.one * scale);
                nodeView.SetAlpha(EaseInOut(t));
                yield return null;
            }

            nodeView.SetScale(Vector3.one);
            nodeView.SetAlpha(1f);
        }

        /// <summary>
        /// 破棄アニメーションのコルーチン
        /// </summary>
        private IEnumerator DestructionCoroutine(GraphNodeView nodeView, float duration, Action onComplete) {
            float elapsed = 0f;
            while (elapsed < duration) {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                float scale = 1f - EaseInOut(t);
                nodeView.SetScale(Vector3.one * scale);
                nodeView.SetAlpha(1f - t);
                yield return null;
            }

            onComplete?.Invoke();
        }

        /// <summary>
        /// シーケンスハイライトアニメーションのコルーチン
        /// </summary>
        private IEnumerator HighlightSequenceCoroutine(List<(GraphEdgeView edgeView, float duration)> steps) {
            foreach (var (edgeView, duration) in steps) {
                yield return PulseEdgeCoroutine(edgeView, Color.white, duration);
            }
        }

        /// <summary>
        /// EaseInOut補間関数
        /// </summary>
        /// <param name="t">進行度（0〜1）</param>
        /// <returns>補間された値</returns>
        private static float EaseInOut(float t) {
            return t < 0.5f ? 2f * t * t : 1f - Mathf.Pow(-2f * t + 2f, 2f) * 0.5f;
        }

        /// <summary>
        /// EaseOutBack補間関数（オーバーシュートあり）
        /// </summary>
        /// <param name="t">進行度（0〜1）</param>
        /// <param name="overshoot">オーバーシュート量</param>
        /// <returns>補間された値</returns>
        private static float EaseOutBack(float t, float overshoot) {
            float c = overshoot - 1f;
            return 1f + c * Mathf.Pow(t - 1f, 3f) + (overshoot - 1f) * Mathf.Pow(t - 1f, 2f);
        }
    }
}
