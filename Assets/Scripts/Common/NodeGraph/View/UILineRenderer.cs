using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DesignPatterns.NodeGraph {
    /// <summary>
    /// uGUI上で線と矢印を描画するカスタムGraphicコンポーネント
    /// Canvas内のUI要素としてエッジを表現する
    /// </summary>
    [RequireComponent(typeof(CanvasRenderer))]
    public class UILineRenderer : Graphic {
        /// <summary>線の始点（ローカル座標）</summary>
        private Vector2 startPoint;
        /// <summary>線の終点（ローカル座標）</summary>
        private Vector2 endPoint;
        /// <summary>線の太さ</summary>
        private float thickness = 2f;
        /// <summary>矢印を表示するかどうか</summary>
        private bool showArrow = true;
        /// <summary>矢印のサイズ</summary>
        private float arrowSize = 10f;
        /// <summary>破線で描画するかどうか</summary>
        private bool isDashed;
        /// <summary>破線1区間の長さ</summary>
        private float dashLength = 8f;

        /// <summary>
        /// 線の始点と終点を設定する
        /// </summary>
        /// <param name="start">始点のローカル座標</param>
        /// <param name="end">終点のローカル座標</param>
        public void SetPoints(Vector2 start, Vector2 end) {
            startPoint = start;
            endPoint = end;
            SetVerticesDirty();
        }

        /// <summary>
        /// 線の太さを設定する
        /// </summary>
        /// <param name="value">太さ（ピクセル）</param>
        public void SetThickness(float value) {
            thickness = value;
            SetVerticesDirty();
        }

        /// <summary>
        /// 矢印の表示設定を行う
        /// </summary>
        /// <param name="show">表示するかどうか</param>
        /// <param name="size">矢印のサイズ</param>
        public void SetArrow(bool show, float size = 10f) {
            showArrow = show;
            arrowSize = size;
            SetVerticesDirty();
        }

        /// <summary>
        /// 破線の設定を行う
        /// </summary>
        /// <param name="dashed">破線にするかどうか</param>
        /// <param name="length">破線1区間の長さ</param>
        public void SetDashed(bool dashed, float length = 8f) {
            isDashed = dashed;
            dashLength = length;
            SetVerticesDirty();
        }

        /// <summary>
        /// メッシュを構築する
        /// Graphicのオーバーライドにより、Canvas描画パイプラインに統合される
        /// </summary>
        /// <param name="vh">頂点ヘルパー</param>
        protected override void OnPopulateMesh(VertexHelper vh) {
            vh.Clear();

            Vector2 direction = endPoint - startPoint;
            float lineLength = direction.magnitude;
            if (lineLength < 0.01f) {
                return;
            }

            Vector2 normalizedDir = direction / lineLength;
            Vector2 actualEnd = endPoint;

            // 矢印がある場合、線の終端を矢印の根元まで短くする
            if (showArrow) {
                actualEnd = endPoint - normalizedDir * arrowSize;
            }

            if (isDashed) {
                GenerateDashedLineMesh(vh, startPoint, actualEnd);
            } else {
                GenerateLineMesh(vh, startPoint, actualEnd);
            }

            if (showArrow) {
                GenerateArrowMesh(vh, actualEnd, endPoint);
            }
        }

        /// <summary>
        /// 実線のメッシュを生成する
        /// </summary>
        /// <param name="vh">頂点ヘルパー</param>
        /// <param name="start">始点</param>
        /// <param name="end">終点</param>
        private void GenerateLineMesh(VertexHelper vh, Vector2 start, Vector2 end) {
            Vector2 direction = (end - start).normalized;
            Vector2 perpendicular = new Vector2(-direction.y, direction.x) * thickness * 0.5f;

            int vertexOffset = vh.currentVertCount;
            vh.AddVert(start + perpendicular, color, Vector4.zero);
            vh.AddVert(start - perpendicular, color, Vector4.zero);
            vh.AddVert(end - perpendicular, color, Vector4.zero);
            vh.AddVert(end + perpendicular, color, Vector4.zero);

            vh.AddTriangle(vertexOffset, vertexOffset + 1, vertexOffset + 2);
            vh.AddTriangle(vertexOffset, vertexOffset + 2, vertexOffset + 3);
        }

        /// <summary>
        /// 破線のメッシュを生成する
        /// </summary>
        /// <param name="vh">頂点ヘルパー</param>
        /// <param name="start">始点</param>
        /// <param name="end">終点</param>
        private void GenerateDashedLineMesh(VertexHelper vh, Vector2 start, Vector2 end) {
            Vector2 direction = end - start;
            float totalLength = direction.magnitude;
            if (totalLength < 0.01f) {
                return;
            }

            Vector2 normalizedDir = direction / totalLength;
            float gapLength = dashLength * 0.5f;
            float segmentLength = dashLength + gapLength;
            float currentPos = 0f;

            while (currentPos < totalLength) {
                float dashEnd = Mathf.Min(currentPos + dashLength, totalLength);
                Vector2 dashStart = start + normalizedDir * currentPos;
                Vector2 dashEndPoint = start + normalizedDir * dashEnd;
                GenerateLineMesh(vh, dashStart, dashEndPoint);
                currentPos += segmentLength;
            }
        }

        /// <summary>
        /// 矢印のメッシュを生成する
        /// </summary>
        /// <param name="vh">頂点ヘルパー</param>
        /// <param name="arrowBase">矢印の根元</param>
        /// <param name="arrowTip">矢印の先端</param>
        private void GenerateArrowMesh(VertexHelper vh, Vector2 arrowBase, Vector2 arrowTip) {
            Vector2 direction = (arrowTip - arrowBase).normalized;
            Vector2 perpendicular = new Vector2(-direction.y, direction.x) * arrowSize * 0.5f;

            int vertexOffset = vh.currentVertCount;
            vh.AddVert(arrowTip, color, Vector4.zero);
            vh.AddVert(arrowBase + perpendicular, color, Vector4.zero);
            vh.AddVert(arrowBase - perpendicular, color, Vector4.zero);

            vh.AddTriangle(vertexOffset, vertexOffset + 1, vertexOffset + 2);
        }
    }
}
