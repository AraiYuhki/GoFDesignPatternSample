using System.Collections.Generic;
using UnityEngine;

namespace DesignPatterns.NodeGraph {
    /// <summary>
    /// 手動配置レイアウト戦略
    /// NodeDataに設定されたPositionをそのまま使用する
    /// 各パターンのデモで位置を明示的に指定する場合に使用する
    /// </summary>
    public class ManualLayoutStrategy : IGraphLayoutStrategy {
        /// <summary>
        /// 各ノードの既存Position値をそのまま返す
        /// </summary>
        /// <param name="graphData">グラフデータ</param>
        /// <param name="availableArea">配置可能な領域（未使用）</param>
        /// <returns>ノードIDをキーとする配置座標の辞書</returns>
        public Dictionary<string, Vector2> CalculatePositions(GraphData graphData, Rect availableArea) {
            var positions = new Dictionary<string, Vector2>();
            foreach (var pair in graphData.Nodes) {
                positions[pair.Key] = pair.Value.Position;
            }
            return positions;
        }
    }
}
