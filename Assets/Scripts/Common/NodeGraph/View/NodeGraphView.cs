using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPatterns.NodeGraph {
    /// <summary>
    /// ノードグラフ全体を管理するメインコンポーネント
    /// ノードとエッジの生成・破棄・アニメーションを統括する
    /// デモシーンの画面左側に配置し、パターンの構造を可視化する
    /// </summary>
    public class NodeGraphView : MonoBehaviour {
        /// <summary>ノードとエッジを配置する親RectTransform</summary>
        [SerializeField]
        private RectTransform graphContainer;
        /// <summary>ノードのプレハブ</summary>
        [SerializeField]
        private GraphNodeView nodePrefab;
        /// <summary>エッジのプレハブ</summary>
        [SerializeField]
        private GraphEdgeView edgePrefab;
        /// <summary>デフォルトのノード色</summary>
        [SerializeField]
        private Color defaultNodeColor = new Color(0.5f, 0.5f, 0.5f, 1f);
        /// <summary>アクティブ時のノード色</summary>
        [SerializeField]
        private Color activeNodeColor = new Color(0.36f, 0.61f, 0.84f, 1f);
        /// <summary>デフォルトのエッジ色</summary>
        [SerializeField]
        private Color defaultEdgeColor = new Color(0.6f, 0.6f, 0.6f, 1f);

        /// <summary>ノードIDをキーとするノードビューの辞書</summary>
        private readonly Dictionary<string, GraphNodeView> nodeViews = new Dictionary<string, GraphNodeView>();
        /// <summary>エッジビューのリスト</summary>
        private readonly List<GraphEdgeView> edgeViews = new List<GraphEdgeView>();
        /// <summary>バインドされたグラフデータ</summary>
        private GraphData graphData;
        /// <summary>アニメーション実行用</summary>
        private GraphAnimator animator;
        /// <summary>ノード配置戦略</summary>
        private IGraphLayoutStrategy layoutStrategy;
        /// <summary>カテゴリ色（外部から設定）</summary>
        private Color categoryColor = Color.white;

        /// <summary>
        /// グラフデータとレイアウト戦略を指定して初期化する
        /// </summary>
        /// <param name="data">表示するグラフデータ</param>
        /// <param name="strategy">ノード配置戦略</param>
        public void Initialize(GraphData data, IGraphLayoutStrategy strategy = null)
        {
            Clear();
            graphData = data;
            layoutStrategy = strategy ?? new ManualLayoutStrategy();
            animator = new GraphAnimator(this);

            // イベントを購読する
            graphData.OnNodeAdded += OnNodeAdded;
            graphData.OnNodeRemoved += OnNodeRemoved;
            graphData.OnEdgeAdded += OnEdgeAdded;
            graphData.OnEdgeRemoved += OnEdgeRemoved;
            graphData.OnCleared += OnDataCleared;

            gameObject.SetActive(true);
            Rebuild();
        }

        /// <summary>
        /// カテゴリに対応する色を設定する
        /// ノードのアクティブ状態やアニメーションで使用される
        /// </summary>
        /// <param name="color">カテゴリ色</param>
        public void SetCategoryColor(Color color) {
            categoryColor = color;
            activeNodeColor = color;
        }

        /// <summary>
        /// グラフデータから全ノード・エッジを再構築する
        /// </summary>
        public void Rebuild() {
            ClearViews();

            if (graphData == null) {
                return;
            }

            // ノードを生成する
            foreach (var pair in graphData.Nodes) {
                CreateNodeView(pair.Value);
            }

            // エッジを生成する
            foreach (var edge in graphData.Edges) {
                CreateEdgeView(edge);
            }
        }

        /// <summary>
        /// 指定ノードの状態を更新する
        /// </summary>
        /// <param name="id">ノードのID</param>
        /// <param name="state">新しい状態</param>
        public void UpdateNodeState(string id, NodeState state) {
            if (!nodeViews.TryGetValue(id, out var nodeView)) {
                return;
            }
            var nodeData = graphData.GetNode(id);
            if (nodeData != null) {
                nodeData.State = state;
            }
            nodeView.SetState(state, categoryColor);
        }

        /// <summary>
        /// 指定ノードの状態テキストを更新する
        /// </summary>
        /// <param name="id">ノードのID</param>
        /// <param name="stateText">新しい状態テキスト</param>
        public void UpdateNodeText(string id, string stateText) {
            if (!nodeViews.TryGetValue(id, out var nodeView)) {
                return;
            }
            var nodeData = graphData.GetNode(id);
            if (nodeData != null) {
                nodeData.StateText = stateText;
            }
            nodeView.SetStateText(stateText);
        }

        /// <summary>
        /// ノードをパルスアニメーションさせる
        /// </summary>
        /// <param name="id">ノードのID</param>
        /// <param name="pulseColor">パルスの色</param>
        /// <param name="duration">アニメーション時間（秒）</param>
        /// <returns>実行中のコルーチン</returns>
        public Coroutine AnimateNodePulse(string id, Color pulseColor, float duration = 0.4f) {
            if (!nodeViews.TryGetValue(id, out var nodeView)) {
                return null;
            }
            return animator.PulseNode(nodeView, pulseColor, duration);
        }

        /// <summary>
        /// エッジをパルスアニメーションさせる
        /// </summary>
        /// <param name="fromId">接続元ノードのID</param>
        /// <param name="toId">接続先ノードのID</param>
        /// <param name="pulseColor">パルスの色</param>
        /// <param name="duration">アニメーション時間（秒）</param>
        /// <returns>実行中のコルーチン</returns>
        public Coroutine AnimateEdgePulse(string fromId, string toId, Color pulseColor, float duration = 0.5f) {
            var edgeView = FindEdgeView(fromId, toId);
            if (edgeView == null) {
                return null;
            }
            return animator.PulseEdge(edgeView, pulseColor, duration);
        }

        /// <summary>
        /// ノードの生成アニメーションを実行する
        /// </summary>
        /// <param name="id">ノードのID</param>
        /// <returns>実行中のコルーチン</returns>
        public Coroutine AnimateNodeCreation(string id) {
            if (!nodeViews.TryGetValue(id, out var nodeView)) {
                return null;
            }
            return animator.AnimateCreation(nodeView);
        }

        /// <summary>
        /// ノードの破棄アニメーションを実行する
        /// </summary>
        /// <param name="id">ノードのID</param>
        /// <returns>実行中のコルーチン</returns>
        public Coroutine AnimateNodeDestruction(string id) {
            if (!nodeViews.TryGetValue(id, out var nodeView)) {
                return null;
            }
            return animator.AnimateDestruction(nodeView);
        }

        /// <summary>
        /// 複数のノードを順にハイライトする経路アニメーション
        /// </summary>
        /// <param name="nodeIds">ハイライトするノードIDのリスト</param>
        /// <returns>実行中のコルーチン</returns>
        public Coroutine HighlightPath(List<string> nodeIds) {
            var steps = new List<(GraphEdgeView, float)>();
            for (int i = 0; i < nodeIds.Count - 1; i++) {
                var edgeView = FindEdgeView(nodeIds[i], nodeIds[i + 1]);
                if (edgeView != null) {
                    steps.Add((edgeView, 0.3f));
                }
            }

            if (steps.Count == 0) {
                return null;
            }
            return animator.HighlightSequence(steps);
        }

        /// <summary>
        /// 全てのノード・エッジビューとデータの購読を解除し、グラフをクリアする
        /// </summary>
        public void Clear() {
            if (animator != null) {
                animator.StopAll();
            }

            if (graphData != null) {
                graphData.OnNodeAdded -= OnNodeAdded;
                graphData.OnNodeRemoved -= OnNodeRemoved;
                graphData.OnEdgeAdded -= OnEdgeAdded;
                graphData.OnEdgeRemoved -= OnEdgeRemoved;
                graphData.OnCleared -= OnDataCleared;
                graphData = null;
            }

            ClearViews();
        }

        /// <summary>
        /// ノードビューを生成する
        /// </summary>
        /// <param name="data">ノードデータ</param>
        /// <returns>生成されたノードビュー</returns>
        private GraphNodeView CreateNodeView(NodeData data) {
            var nodeView = Instantiate(nodePrefab, graphContainer);
            nodeView.Bind(data);
            nodeView.SetState(data.State, categoryColor);

            if (data.CustomColor != Color.clear) {
                nodeView.SetBorderColor(data.CustomColor);
            }

            nodeViews[data.Id] = nodeView;
            return nodeView;
        }

        /// <summary>
        /// エッジビューを生成する
        /// </summary>
        /// <param name="data">エッジデータ</param>
        /// <returns>生成されたエッジビュー（端点のノードが見つからない場合はnull）</returns>
        private GraphEdgeView CreateEdgeView(EdgeData data) {
            if (!nodeViews.TryGetValue(data.FromNodeId, out var fromNode)) {
                return null;
            }
            if (!nodeViews.TryGetValue(data.ToNodeId, out var toNode)) {
                return null;
            }

            var edgeView = Instantiate(edgePrefab, graphContainer);
            // エッジはノードより背面に描画する
            edgeView.transform.SetAsFirstSibling();

            var endpoints = CalculateEdgeEndpoints(fromNode, toNode);
            edgeView.Bind(data, endpoints.start, endpoints.end);

            if (data.CustomColor == Color.clear) {
                edgeView.SetColor(defaultEdgeColor);
            }

            edgeViews.Add(edgeView);
            return edgeView;
        }

        /// <summary>
        /// ノードの矩形境界からエッジの端点を計算する
        /// </summary>
        /// <param name="fromNode">接続元ノード</param>
        /// <param name="toNode">接続先ノード</param>
        /// <returns>エッジの始点と終点</returns>
        private (Vector2 start, Vector2 end) CalculateEdgeEndpoints(GraphNodeView fromNode, GraphNodeView toNode) {
            Vector2 fromPos = fromNode.RectTrans.anchoredPosition;
            Vector2 toPos = toNode.RectTrans.anchoredPosition;
            Vector2 fromSize = fromNode.RectTrans.sizeDelta;
            Vector2 toSize = toNode.RectTrans.sizeDelta;

            Vector2 direction = (toPos - fromPos).normalized;

            // ノードの矩形境界との交点を計算する
            Vector2 start = CalculateRectIntersection(fromPos, direction, fromSize);
            Vector2 end = CalculateRectIntersection(toPos, -direction, toSize);

            return (start, end);
        }

        /// <summary>
        /// 矩形の中心から指定方向への境界交点を計算する
        /// </summary>
        /// <param name="center">矩形の中心座標</param>
        /// <param name="direction">方向ベクトル（正規化済み）</param>
        /// <param name="size">矩形のサイズ</param>
        /// <returns>交点座標</returns>
        private static Vector2 CalculateRectIntersection(Vector2 center, Vector2 direction, Vector2 size) {
            float halfWidth = size.x * 0.5f;
            float halfHeight = size.y * 0.5f;

            if (Mathf.Abs(direction.x) < 0.001f && Mathf.Abs(direction.y) < 0.001f) {
                return center;
            }

            float scaleX = Mathf.Abs(direction.x) > 0.001f ? halfWidth / Mathf.Abs(direction.x) : float.MaxValue;
            float scaleY = Mathf.Abs(direction.y) > 0.001f ? halfHeight / Mathf.Abs(direction.y) : float.MaxValue;
            float scale = Mathf.Min(scaleX, scaleY);

            return center + direction * scale;
        }

        /// <summary>
        /// 指定のノードID組に一致するエッジビューを検索する
        /// </summary>
        /// <param name="fromId">接続元ノードID</param>
        /// <param name="toId">接続先ノードID</param>
        /// <returns>見つかったエッジビュー（見つからない場合はnull）</returns>
        private GraphEdgeView FindEdgeView(string fromId, string toId) {
            foreach (var edgeView in edgeViews) {
                if (edgeView.Matches(fromId, toId)) {
                    return edgeView;
                }
            }
            return null;
        }

        /// <summary>
        /// 全ビューを破棄する
        /// </summary>
        private void ClearViews() {
            foreach (var pair in nodeViews) {
                if (pair.Value != null) {
                    Destroy(pair.Value.gameObject);
                }
            }
            nodeViews.Clear();

            foreach (var edgeView in edgeViews) {
                if (edgeView != null) {
                    Destroy(edgeView.gameObject);
                }
            }
            edgeViews.Clear();
        }

        /// <summary>
        /// ノード追加イベントのハンドラ
        /// </summary>
        /// <param name="data">追加されたノードデータ</param>
        private void OnNodeAdded(NodeData data) {
            CreateNodeView(data);
        }

        /// <summary>
        /// ノード削除イベントのハンドラ
        /// </summary>
        /// <param name="id">削除されたノードのID</param>
        private void OnNodeRemoved(string id) {
            if (nodeViews.TryGetValue(id, out var nodeView)) {
                Destroy(nodeView.gameObject);
                nodeViews.Remove(id);
            }

            // 関連エッジも削除する
            for (int i = edgeViews.Count - 1; i >= 0; i--) {
                if (edgeViews[i].FromNodeId == id || edgeViews[i].ToNodeId == id) {
                    Destroy(edgeViews[i].gameObject);
                    edgeViews.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// エッジ追加イベントのハンドラ
        /// </summary>
        /// <param name="data">追加されたエッジデータ</param>
        private void OnEdgeAdded(EdgeData data) {
            CreateEdgeView(data);
        }

        /// <summary>
        /// エッジ削除イベントのハンドラ
        /// </summary>
        /// <param name="fromId">接続元ノードID</param>
        /// <param name="toId">接続先ノードID</param>
        private void OnEdgeRemoved(string fromId, string toId) {
            for (int i = edgeViews.Count - 1; i >= 0; i--) {
                if (edgeViews[i].Matches(fromId, toId)) {
                    Destroy(edgeViews[i].gameObject);
                    edgeViews.RemoveAt(i);
                    return;
                }
            }
        }

        /// <summary>
        /// データクリアイベントのハンドラ
        /// </summary>
        private void OnDataCleared() {
            ClearViews();
        }

        private void OnDestroy() {
            Clear();
        }
    }
}
