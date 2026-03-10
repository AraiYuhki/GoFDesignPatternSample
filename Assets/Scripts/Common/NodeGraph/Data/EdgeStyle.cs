namespace DesignPatterns.NodeGraph {
    /// <summary>
    /// エッジの矢印の方向を表す列挙型
    /// </summary>
    public enum EdgeDirection {
        /// <summary>矢印なし（単純な線）</summary>
        None,
        /// <summary>接続先方向に矢印</summary>
        Forward,
        /// <summary>接続元方向に矢印</summary>
        Backward,
        /// <summary>双方向矢印</summary>
        Bidirectional
    }

    /// <summary>
    /// エッジの描画スタイルを定義する構造体
    /// </summary>
    public struct EdgeStyleData {
        /// <summary>矢印の方向</summary>
        public EdgeDirection Direction;
        /// <summary>破線で描画するかどうか</summary>
        public bool IsDashed;
        /// <summary>線の太さ</summary>
        public float Thickness;

        /// <summary>
        /// デフォルトスタイルを生成する（実線・矢印あり・太さ2px）
        /// </summary>
        /// <returns>デフォルトのエッジスタイル</returns>
        public static EdgeStyleData DefaultForward() {
            return new EdgeStyleData {
                Direction = EdgeDirection.Forward,
                IsDashed = false,
                Thickness = 2f
            };
        }

        /// <summary>
        /// 破線スタイルを生成する（矢印なし・太さ2px）
        /// </summary>
        /// <returns>破線のエッジスタイル</returns>
        public static EdgeStyleData Dashed() {
            return new EdgeStyleData {
                Direction = EdgeDirection.None,
                IsDashed = true,
                Thickness = 2f
            };
        }
    }
}
