using System.Collections.Generic;
using UnityEngine;

namespace DesignPatterns.NodeGraph {
    /// <summary>
    /// グラフのノード配置戦略を定義するインターフェース
    /// 異なるレイアウトアルゴリズム（手動配置、グリッド、ツリーなど）を差し替え可能にする
    /// </summary>
    public interface IGraphLayoutStrategy {
        /// <summary>
        /// グラフデータと利用可能な領域から、各ノードの配置座標を計算する
        /// </summary>
        /// <param name="graphData">グラフデータ</param>
        /// <param name="availableArea">配置可能な領域（ローカル座標）</param>
        /// <returns>ノードIDをキーとする配置座標の辞書</returns>
        Dictionary<string, Vector2> CalculatePositions(GraphData graphData, Rect availableArea);
    }
}
