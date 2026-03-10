using DesignPatterns.NodeGraph;
using NUnit.Framework;
using UnityEngine;

namespace DesignPatterns.Tests
{
    /// <summary>
    /// GraphDataのユニットテスト
    /// ノードとエッジの追加・削除・クリア操作とイベント発火を検証する
    /// </summary>
    [TestFixture]
    public class GraphDataTest
    {
        /// <summary>テスト対象</summary>
        private GraphData graphData;

        [SetUp]
        public void SetUp()
        {
            graphData = new GraphData();
        }

        [Test]
        public void AddNode_ノードが追加される()
        {
            var node = new NodeData("node1", "TestNode");
            graphData.AddNode(node);

            Assert.AreEqual(1, graphData.Nodes.Count);
            Assert.AreEqual(node, graphData.GetNode("node1"));
        }

        [Test]
        public void AddNode_重複IDは無視される()
        {
            var node1 = new NodeData("node1", "First");
            var node2 = new NodeData("node1", "Second");

            graphData.AddNode(node1);
            graphData.AddNode(node2);

            Assert.AreEqual(1, graphData.Nodes.Count);
            Assert.AreEqual("First", graphData.GetNode("node1").DisplayName);
        }

        [Test]
        public void AddNode_イベントが発火する()
        {
            NodeData received = null;
            graphData.OnNodeAdded += n => received = n;

            var node = new NodeData("node1", "TestNode");
            graphData.AddNode(node);

            Assert.AreEqual(node, received);
        }

        [Test]
        public void RemoveNode_ノードが削除される()
        {
            graphData.AddNode(new NodeData("node1", "TestNode"));
            graphData.RemoveNode("node1");

            Assert.AreEqual(0, graphData.Nodes.Count);
            Assert.IsNull(graphData.GetNode("node1"));
        }

        [Test]
        public void RemoveNode_関連エッジも削除される()
        {
            graphData.AddNode(new NodeData("node1", "Node1"));
            graphData.AddNode(new NodeData("node2", "Node2"));
            graphData.AddNode(new NodeData("node3", "Node3"));
            graphData.AddEdge(new EdgeData("node1", "node2"));
            graphData.AddEdge(new EdgeData("node2", "node3"));

            graphData.RemoveNode("node2");

            Assert.AreEqual(0, graphData.Edges.Count);
        }

        [Test]
        public void RemoveNode_イベントが発火する()
        {
            string removedId = null;
            graphData.OnNodeRemoved += id => removedId = id;

            graphData.AddNode(new NodeData("node1", "TestNode"));
            graphData.RemoveNode("node1");

            Assert.AreEqual("node1", removedId);
        }

        [Test]
        public void RemoveNode_存在しないIDは何もしない()
        {
            bool eventFired = false;
            graphData.OnNodeRemoved += _ => eventFired = true;

            graphData.RemoveNode("nonexistent");

            Assert.IsFalse(eventFired);
        }

        [Test]
        public void AddEdge_エッジが追加される()
        {
            var edge = new EdgeData("node1", "node2", "test");
            graphData.AddEdge(edge);

            Assert.AreEqual(1, graphData.Edges.Count);
            Assert.AreEqual(edge, graphData.Edges[0]);
        }

        [Test]
        public void AddEdge_イベントが発火する()
        {
            EdgeData received = null;
            graphData.OnEdgeAdded += e => received = e;

            var edge = new EdgeData("node1", "node2");
            graphData.AddEdge(edge);

            Assert.AreEqual(edge, received);
        }

        [Test]
        public void RemoveEdge_エッジが削除される()
        {
            graphData.AddEdge(new EdgeData("node1", "node2"));
            graphData.RemoveEdge("node1", "node2");

            Assert.AreEqual(0, graphData.Edges.Count);
        }

        [Test]
        public void RemoveEdge_イベントが発火する()
        {
            string fromId = null;
            string toId = null;
            graphData.OnEdgeRemoved += (f, t) =>
            {
                fromId = f;
                toId = t;
            };

            graphData.AddEdge(new EdgeData("node1", "node2"));
            graphData.RemoveEdge("node1", "node2");

            Assert.AreEqual("node1", fromId);
            Assert.AreEqual("node2", toId);
        }

        [Test]
        public void Clear_全データが削除される()
        {
            graphData.AddNode(new NodeData("node1", "Node1"));
            graphData.AddNode(new NodeData("node2", "Node2"));
            graphData.AddEdge(new EdgeData("node1", "node2"));

            graphData.Clear();

            Assert.AreEqual(0, graphData.Nodes.Count);
            Assert.AreEqual(0, graphData.Edges.Count);
        }

        [Test]
        public void Clear_イベントが発火する()
        {
            bool eventFired = false;
            graphData.OnCleared += () => eventFired = true;

            graphData.Clear();

            Assert.IsTrue(eventFired);
        }

        [Test]
        public void GetNode_存在しないIDはnullを返す()
        {
            Assert.IsNull(graphData.GetNode("nonexistent"));
        }

        [Test]
        public void NodeData_デフォルト値が正しい()
        {
            var node = new NodeData("id", "Name", "State", new Vector2(10f, 20f));

            Assert.AreEqual("id", node.Id);
            Assert.AreEqual("Name", node.DisplayName);
            Assert.AreEqual("State", node.StateText);
            Assert.AreEqual(new Vector2(10f, 20f), node.Position);
            Assert.AreEqual(NodeState.Default, node.State);
            Assert.AreEqual(Color.clear, node.CustomColor);
        }

        [Test]
        public void EdgeData_デフォルトスタイルが適用される()
        {
            var edge = new EdgeData("from", "to");

            Assert.AreEqual(EdgeDirection.Forward, edge.Style.Direction);
            Assert.IsFalse(edge.Style.IsDashed);
            Assert.IsTrue(edge.Style.Thickness > 0f);
        }

        [Test]
        public void ManualLayoutStrategy_既存座標をそのまま返す()
        {
            var strategy = new ManualLayoutStrategy();
            graphData.AddNode(new NodeData("a", "A", "", new Vector2(100f, 200f)));
            graphData.AddNode(new NodeData("b", "B", "", new Vector2(-50f, 75f)));

            var positions = strategy.CalculatePositions(graphData, new Rect(0, 0, 800, 600));

            Assert.AreEqual(new Vector2(100f, 200f), positions["a"]);
            Assert.AreEqual(new Vector2(-50f, 75f), positions["b"]);
        }
    }
}
