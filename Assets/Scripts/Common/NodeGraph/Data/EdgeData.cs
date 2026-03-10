using UnityEngine;

namespace DesignPatterns.NodeGraph {
    /// <summary>
    /// エッジの表示データを保持するクラス
    /// ノード間の関係（依存・継承・通知など）を表現する
    /// </summary>
    public class EdgeData {
        /// <summary>接続元ノードのID</summary>
        public readonly string FromNodeId;
        /// <summary>接続先ノードのID</summary>
        public readonly string ToNodeId;
        /// <summary>エッジに表示するラベル（例: "notify"）</summary>
        public string Label;
        /// <summary>エッジの描画スタイル</summary>
        public EdgeStyleData Style;
        /// <summary>カスタム色（Color.clearの場合はデフォルト色を使用）</summary>
        public Color CustomColor;

        /// <summary>
        /// EdgeDataを生成する
        /// </summary>
        /// <param name="fromNodeId">接続元ノードのID</param>
        /// <param name="toNodeId">接続先ノードのID</param>
        /// <param name="label">ラベルテキスト</param>
        /// <param name="style">描画スタイル</param>
        public EdgeData(string fromNodeId, string toNodeId, string label = "", EdgeStyleData style = default) {
            FromNodeId = fromNodeId;
            ToNodeId = toNodeId;
            Label = label;
            Style = style.Thickness <= 0f ? EdgeStyleData.DefaultForward() : style;
            CustomColor = Color.clear;
        }
    }
}
