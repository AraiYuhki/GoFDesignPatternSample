using UnityEngine;

namespace GoFPatterns.Patterns.Visualization {
    /// <summary>
    /// Iteratorパターンのビジュアライゼーション
    /// アイテムを一列に配置し、カーソル移動で順方向・逆方向の走査を可視化する
    /// </summary>
    [PatternVisualization("iterator")]
    public class IteratorVisualization : BasePatternVisualization {
        /// <summary>アイテム名の配列</summary>
        private static readonly string[] ItemNames = { "剣", "盾", "ポーション", "鍵" };
        /// <summary>アイテム列の開始X座標</summary>
        private const float StartX = -4.5f;
        /// <summary>アイテム間のX方向間隔</summary>
        private const float Spacing = 3f;
        /// <summary>アイテムのY座標</summary>
        private const float ItemY = 0f;
        /// <summary>カーソルのY座標オフセット</summary>
        private const float CursorOffsetY = -2f;
        /// <summary>アイテム矩形のサイズ</summary>
        private static readonly Vector2 ItemSize = new Vector2(2.2f, 1.2f);
        /// <summary>カーソルの半径</summary>
        private const float CursorRadius = 0.5f;
        /// <summary>アイテムの色</summary>
        private static readonly Color ItemColor = new Color(0.4f, 0.6f, 0.7f, 1f);
        /// <summary>カーソルの色</summary>
        private static readonly Color CursorColor = new Color(0.8f, 0.7f, 0.3f, 1f);
        /// <summary>方向ラベルの配置位置</summary>
        private static readonly Vector2 DirectionLabelPosition = new Vector2(0f, 3.5f);
        /// <summary>方向ラベル矩形のサイズ</summary>
        private static readonly Vector2 DirectionLabelSize = new Vector2(4f, 1f);

        /// <summary>
        /// バインド時にアイテム要素とカーソルを配置して初期表示を構築する
        /// </summary>
        /// <param name="demo">バインドされたデモ</param>
        protected override void OnBind(IPatternDemo demo) {
            for (int i = 0; i < ItemNames.Length; i++) {
                float x = StartX + i * Spacing;
                AddRect($"item{i}", ItemNames[i], new Vector2(x, ItemY), ItemSize, ItemColor);
            }

            AddCircle("cursor", "▼", new Vector2(StartX, ItemY + CursorOffsetY), CursorRadius, CursorColor);
            AddRect("direction", "順方向 →", DirectionLabelPosition, DirectionLabelSize, DimColor);

            GetElement("cursor")?.SetVisible(false);
            GetElement("direction")?.SetVisible(false);
        }

        /// <summary>
        /// ステップに応じてカーソル移動のアニメーションを更新する
        /// </summary>
        /// <param name="stepIndex">現在のステップインデックス</param>
        protected override void OnRefresh(int stepIndex) {
            VisualElement cursor = GetElement("cursor");
            VisualElement direction = GetElement("direction");

            switch (stepIndex) {
                case 0:
                    for (int i = 0; i < ItemNames.Length; i++) {
                        GetElement($"item{i}")?.Pulse(PulseColor, 0.5f);
                    }
                    break;
                case 1:
                    cursor.SetVisible(true);
                    direction.SetVisible(true);
                    direction.SetLabel("順方向 →");
                    direction.Pulse(HighlightColor, 0.5f);
                    HighlightItemAtIndex(0);
                    break;
                case 2:
                    HighlightForwardSequence();
                    break;
                case 3:
                    direction.SetLabel("← 逆方向");
                    direction.Pulse(HighlightColor, 0.5f);
                    HighlightItemAtIndex(ItemNames.Length - 1);
                    break;
                case 4:
                    HighlightReverseSequence();
                    break;
                case 5:
                    DimAllItems();
                    for (int i = 0; i < ItemNames.Length; i++) {
                        GetElement($"item{i}")?.SetColorImmediate(ItemColor);
                    }
                    cursor.SetVisible(false);
                    direction.SetLabel("走査完了");
                    direction.Pulse(PulseColor, 0.5f);
                    break;
            }
        }

        /// <summary>
        /// 指定インデックスのアイテムをハイライトしてカーソルを移動する
        /// </summary>
        /// <param name="index">ハイライトするアイテムのインデックス</param>
        private void HighlightItemAtIndex(int index) {
            DimAllItems();
            VisualElement item = GetElement($"item{index}");
            if (item != null) {
                item.SetColorImmediate(ItemColor);
                item.Pulse(HighlightColor, 0.5f);
            }

            VisualElement cursor = GetElement("cursor");
            float x = StartX + index * Spacing;
            cursor.MoveTo(new Vector2(x, ItemY + CursorOffsetY), 0.3f);
            cursor.Pulse(PulseColor, 0.5f);
        }

        /// <summary>
        /// 順方向の走査を一括でハイライトする
        /// </summary>
        private void HighlightForwardSequence() {
            for (int i = 0; i < ItemNames.Length; i++) {
                VisualElement item = GetElement($"item{i}");
                item?.SetColorImmediate(ItemColor);
                item?.Pulse(PulseColor, 0.5f);
            }

            VisualElement cursor = GetElement("cursor");
            float lastX = StartX + (ItemNames.Length - 1) * Spacing;
            cursor.MoveTo(new Vector2(lastX, ItemY + CursorOffsetY), 0.5f);
        }

        /// <summary>
        /// 逆方向の走査を一括でハイライトする
        /// </summary>
        private void HighlightReverseSequence() {
            for (int i = ItemNames.Length - 1; i >= 0; i--) {
                VisualElement item = GetElement($"item{i}");
                item?.SetColorImmediate(ItemColor);
                item?.Pulse(PulseColor, 0.5f);
            }

            VisualElement cursor = GetElement("cursor");
            cursor.MoveTo(new Vector2(StartX, ItemY + CursorOffsetY), 0.5f);
        }

        /// <summary>
        /// 全アイテムをDim状態にする
        /// </summary>
        private void DimAllItems() {
            for (int i = 0; i < ItemNames.Length; i++) {
                GetElement($"item{i}")?.SetColorImmediate(DimColor);
            }
        }
    }
}
