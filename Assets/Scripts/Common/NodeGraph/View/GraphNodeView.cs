using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DesignPatterns.NodeGraph {
    /// <summary>
    /// グラフ上のノード1つを表示するUIコンポーネント
    /// クラスやオブジェクトを矩形で表現し、名前と状態テキストを表示する
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class GraphNodeView : MonoBehaviour {
        /// <summary>ノードの背景画像</summary>
        [SerializeField]
        private Image backgroundImage;
        /// <summary>ノードのボーダー画像</summary>
        [SerializeField]
        private Image borderImage;
        /// <summary>ノード名を表示するラベル</summary>
        [SerializeField]
        private TMP_Text nameLabel;
        /// <summary>状態テキストを表示するラベル</summary>
        [SerializeField]
        private TMP_Text stateLabel;

        /// <summary>デフォルトの背景色</summary>
        private static readonly Color DefaultBackgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.9f);
        /// <summary>デフォルトのボーダー色</summary>
        private static readonly Color DefaultBorderColor = new Color(0.5f, 0.5f, 0.5f, 1f);
        /// <summary>強調時のボーダー色</summary>
        private static readonly Color HighlightedBorderColor = new Color(1f, 1f, 1f, 1f);
        /// <summary>非強調時の不透明度</summary>
        private const float DimmedAlpha = 0.3f;

        /// <summary>このノードのID</summary>
        private string nodeId;
        /// <summary>CanvasGroupコンポーネント</summary>
        private CanvasGroup canvasGroup;

        /// <summary>ノードのIDを取得する</summary>
        public string NodeId => nodeId;

        /// <summary>RectTransformを取得する</summary>
        public RectTransform RectTrans => (RectTransform)transform;

        private void Awake() {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        /// <summary>
        /// ノードデータをバインドし、表示を更新する
        /// </summary>
        /// <param name="data">バインドするノードデータ</param>
        public void Bind(NodeData data) {
            nodeId = data.Id;
            nameLabel.text = data.DisplayName;

            if (stateLabel != null) {
                stateLabel.text = data.StateText;
                stateLabel.gameObject.SetActive(!string.IsNullOrEmpty(data.StateText));
            }

            RectTrans.anchoredPosition = data.Position;
        }

        /// <summary>
        /// ノードの視覚状態を更新する
        /// </summary>
        /// <param name="state">新しい状態</param>
        /// <param name="categoryColor">カテゴリに対応する色</param>
        public void SetState(NodeState state, Color categoryColor) {
            switch (state) {
                case NodeState.Default:
                    SetBackgroundColor(DefaultBackgroundColor);
                    SetBorderColor(DefaultBorderColor);
                    SetAlpha(1f);
                    break;
                case NodeState.Active:
                    SetBackgroundColor(categoryColor * 0.3f + DefaultBackgroundColor * 0.7f);
                    SetBorderColor(categoryColor);
                    SetAlpha(1f);
                    break;
                case NodeState.Highlighted:
                    SetBackgroundColor(categoryColor * 0.5f + DefaultBackgroundColor * 0.5f);
                    SetBorderColor(HighlightedBorderColor);
                    SetAlpha(1f);
                    break;
                case NodeState.Dimmed:
                    SetBackgroundColor(DefaultBackgroundColor);
                    SetBorderColor(DefaultBorderColor);
                    SetAlpha(DimmedAlpha);
                    break;
                case NodeState.Creating:
                case NodeState.Destroying:
                    // アニメーション側で制御する
                    break;
            }
        }

        /// <summary>
        /// 状態テキストを更新する
        /// </summary>
        /// <param name="text">表示するテキスト</param>
        public void SetStateText(string text) {
            if (stateLabel == null) {
                return;
            }
            stateLabel.text = text;
            stateLabel.gameObject.SetActive(!string.IsNullOrEmpty(text));
        }

        /// <summary>
        /// 背景色を設定する
        /// </summary>
        /// <param name="bgColor">背景色</param>
        public void SetBackgroundColor(Color bgColor) {
            if (backgroundImage != null) {
                backgroundImage.color = bgColor;
            }
        }

        /// <summary>
        /// ボーダー色を設定する
        /// </summary>
        /// <param name="bdrColor">ボーダー色</param>
        public void SetBorderColor(Color bdrColor) {
            if (borderImage != null) {
                borderImage.color = bdrColor;
            }
        }

        /// <summary>
        /// 不透明度を設定する
        /// </summary>
        /// <param name="alpha">不透明度（0〜1）</param>
        public void SetAlpha(float alpha) {
            if (canvasGroup != null) {
                canvasGroup.alpha = alpha;
            }
        }

        /// <summary>
        /// スケールを設定する
        /// </summary>
        /// <param name="scale">スケール値</param>
        public void SetScale(Vector3 scale) {
            transform.localScale = scale;
        }
    }
}
