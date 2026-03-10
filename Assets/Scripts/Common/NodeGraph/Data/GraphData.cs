using System;
using System.Collections.Generic;

namespace DesignPatterns.NodeGraph {
    /// <summary>
    /// グラフ全体のデータモデル
    /// ノードとエッジの追加・削除を管理し、変更をイベントで通知する
    /// </summary>
    public class GraphData {
        /// <summary>ノードIDをキーとするノードデータの辞書</summary>
        private readonly Dictionary<string, NodeData> nodes = new Dictionary<string, NodeData>();
        /// <summary>エッジデータのリスト</summary>
        private readonly List<EdgeData> edges = new List<EdgeData>();

        /// <summary>登録されている全ノードデータ</summary>
        public IReadOnlyDictionary<string, NodeData> Nodes => nodes;
        /// <summary>登録されている全エッジデータ</summary>
        public IReadOnlyList<EdgeData> Edges => edges;

        /// <summary>ノードが追加されたときに発火するイベント</summary>
        public event Action<NodeData> OnNodeAdded;
        /// <summary>ノードが削除されたときに発火するイベント</summary>
        public event Action<string> OnNodeRemoved;
        /// <summary>エッジが追加されたときに発火するイベント</summary>
        public event Action<EdgeData> OnEdgeAdded;
        /// <summary>エッジが削除されたときに発火するイベント</summary>
        public event Action<string, string> OnEdgeRemoved;
        /// <summary>全データがクリアされたときに発火するイベント</summary>
        public event Action OnCleared;

        /// <summary>
        /// ノードを追加する
        /// </summary>
        /// <param name="node">追加するノードデータ</param>
        public void AddNode(NodeData node) {
            if (nodes.ContainsKey(node.Id)) {
                return;
            }
            nodes[node.Id] = node;
            OnNodeAdded?.Invoke(node);
        }

        /// <summary>
        /// 指定IDのノードを削除する
        /// </summary>
        /// <param name="id">削除するノードのID</param>
        public void RemoveNode(string id) {
            if (!nodes.Remove(id)) {
                return;
            }
            // 関連するエッジも削除する
            for (int i = edges.Count - 1; i >= 0; i--) {
                if (edges[i].FromNodeId == id || edges[i].ToNodeId == id) {
                    var edge = edges[i];
                    edges.RemoveAt(i);
                    OnEdgeRemoved?.Invoke(edge.FromNodeId, edge.ToNodeId);
                }
            }
            OnNodeRemoved?.Invoke(id);
        }

        /// <summary>
        /// エッジを追加する
        /// </summary>
        /// <param name="edge">追加するエッジデータ</param>
        public void AddEdge(EdgeData edge) {
            edges.Add(edge);
            OnEdgeAdded?.Invoke(edge);
        }

        /// <summary>
        /// 指定の接続元・接続先のエッジを削除する
        /// </summary>
        /// <param name="fromId">接続元ノードのID</param>
        /// <param name="toId">接続先ノードのID</param>
        public void RemoveEdge(string fromId, string toId) {
            for (int i = edges.Count - 1; i >= 0; i--) {
                if (edges[i].FromNodeId == fromId && edges[i].ToNodeId == toId) {
                    edges.RemoveAt(i);
                    OnEdgeRemoved?.Invoke(fromId, toId);
                    return;
                }
            }
        }

        /// <summary>
        /// 指定IDのノードデータを取得する
        /// </summary>
        /// <param name="id">ノードのID</param>
        /// <returns>ノードデータ（存在しない場合はnull）</returns>
        public NodeData GetNode(string id) {
            nodes.TryGetValue(id, out var node);
            return node;
        }

        /// <summary>
        /// 全ノードとエッジをクリアする
        /// </summary>
        public void Clear() {
            nodes.Clear();
            edges.Clear();
            OnCleared?.Invoke();
        }
    }
}
