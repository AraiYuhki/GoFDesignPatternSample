using TMPro;
using UnityEngine;

namespace DesignPatterns.NodeGraph {
    /// <summary>
    /// グラフ上のエッジ1つを表示するUIコンポーネント
    /// ノード間の関係を線と矢印で表現する
    /// </summary>
    public class GraphEdgeView : MonoBehaviour {
        /// <summary>線を描画するUILineRenderer</summary>
        [SerializeField]
        private UILineRenderer lineRenderer;
        /// <summary>エッジのラベルテキスト（オプション）</summary>
        [SerializeField]
        private TMP_Text labelText;

        /// <summary>接続元ノードのID</summary>
        private string fromNodeId;
        /// <summary>接続先ノードのID</summary>
        private string toNodeId;

        /// <summary>接続元ノードのIDを取得する</summary>
        public string FromNodeId => fromNodeId;
        /// <summary>接続先ノードのIDを取得する</summary>
        public string ToNodeId => toNodeId;

        /// <summary>
        /// エッジデータをバインドし、表示を更新する
        /// </summary>
        /// <param name="data">バインドするエッジデータ</param>
        /// <param name="startPos">始点のローカル座標</param>
        /// <param name="endPos">終点のローカル座標</param>
        public void Bind(EdgeData data, Vector2 startPos, Vector2 endPos) {
            fromNodeId = data.FromNodeId;
            toNodeId = data.ToNodeId;

            lineRenderer.SetPoints(startPos, endPos);
            lineRenderer.SetThickness(data.Style.Thickness);
            lineRenderer.SetDashed(data.Style.IsDashed);

            bool hasForwardArrow = data.Style.Direction == EdgeDirection.Forward
                || data.Style.Direction == EdgeDirection.Bidirectional;
            lineRenderer.SetArrow(hasForwardArrow);

            if (data.CustomColor != Color.clear) {
                lineRenderer.color = data.CustomColor;
            }

            if (labelText != null) {
                labelText.text = data.Label;
                labelText.gameObject.SetActive(!string.IsNullOrEmpty(data.Label));

                if (!string.IsNullOrEmpty(data.Label)) {
                    Vector2 midPoint = (startPos + endPos) * 0.5f;
                    labelText.rectTransform.anchoredPosition = midPoint + Vector2.up * 10f;
                }
            }
        }

        /// <summary>
        /// エッジの端点を更新する
        /// </summary>
        /// <param name="startPos">新しい始点</param>
        /// <param name="endPos">新しい終点</param>
        public void UpdateEndpoints(Vector2 startPos, Vector2 endPos) {
            lineRenderer.SetPoints(startPos, endPos);

            if (labelText != null && labelText.gameObject.activeSelf) {
                Vector2 midPoint = (startPos + endPos) * 0.5f;
                labelText.rectTransform.anchoredPosition = midPoint + Vector2.up * 10f;
            }
        }

        /// <summary>
        /// エッジの色を設定する
        /// </summary>
        /// <param name="edgeColor">エッジの色</param>
        public void SetColor(Color edgeColor) {
            lineRenderer.color = edgeColor;
            if (labelText != null) {
                labelText.color = edgeColor;
            }
        }

        /// <summary>
        /// エッジの不透明度を設定する
        /// </summary>
        /// <param name="alpha">不透明度（0〜1）</param>
        public void SetAlpha(float alpha) {
            Color currentColor = lineRenderer.color;
            currentColor.a = alpha;
            lineRenderer.color = currentColor;

            if (labelText != null) {
                Color labelColor = labelText.color;
                labelColor.a = alpha;
                labelText.color = labelColor;
            }
        }

        /// <summary>
        /// 指定のノードID組に一致するかを判定する
        /// </summary>
        /// <param name="fromId">接続元ノードID</param>
        /// <param name="toId">接続先ノードID</param>
        /// <returns>一致する場合true</returns>
        public bool Matches(string fromId, string toId) {
            return fromNodeId == fromId && toNodeId == toId;
        }
    }
}
