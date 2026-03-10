using UnityEngine;

namespace DesignPatterns.NodeGraph {
    /// <summary>
    /// ノードの表示データを保持するクラス
    /// グラフ上の1つのクラスやオブジェクトを表現する
    /// </summary>
    public class NodeData {
        /// <summary>ノードの一意な識別子</summary>
        public readonly string Id;
        /// <summary>ノードに表示する名前</summary>
        public string DisplayName;
        /// <summary>ノードの状態テキスト（例: "HP: 100"）</summary>
        public string StateText;
        /// <summary>グラフ空間でのローカル座標</summary>
        public Vector2 Position;
        /// <summary>ノードの視覚状態</summary>
        public NodeState State;
        /// <summary>カスタム色（Color.clearの場合はデフォルト色を使用）</summary>
        public Color CustomColor;

        /// <summary>
        /// NodeDataを生成する
        /// </summary>
        /// <param name="id">一意な識別子</param>
        /// <param name="displayName">表示名</param>
        /// <param name="stateText">状態テキスト</param>
        /// <param name="position">グラフ空間での位置</param>
        public NodeData(string id, string displayName, string stateText = "", Vector2 position = default) {
            Id = id;
            DisplayName = displayName;
            StateText = stateText;
            Position = position;
            State = NodeState.Default;
            CustomColor = Color.clear;
        }
    }
}
