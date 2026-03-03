using UnityEngine;

namespace DesignPatterns.Behavioral.Command
{
    /// <summary>
    /// 移動コマンド
    /// 対象を指定方向に指定距離だけ移動させるコマンドを表す
    /// 実行と取り消し（逆方向への移動）をサポートする
    /// </summary>
    public sealed class MoveCommand : ICommand
    {
        /// <summary>移動対象の名前</summary>
        private readonly string targetName;

        /// <summary>移動方向</summary>
        private readonly Vector2 direction;

        /// <summary>移動距離</summary>
        private readonly float distance;

        /// <inheritdoc/>
        public string Description
        {
            get { return $"{targetName}を{GetDirectionName()}に{distance}移動"; }
        }

        /// <summary>
        /// MoveCommandを生成する
        /// </summary>
        /// <param name="targetName">移動対象の名前</param>
        /// <param name="direction">移動方向（正規化ベクトル）</param>
        /// <param name="distance">移動距離</param>
        public MoveCommand(string targetName, Vector2 direction, float distance)
        {
            this.targetName = targetName;
            this.direction = direction;
            this.distance = distance;
        }

        /// <inheritdoc/>
        public void Execute()
        {
            Vector2 movement = direction * distance;
            InGameLogger.Log(
                $"  実行: {targetName}を{GetDirectionName()}に{distance}移動 ({movement.x:F1}, {movement.y:F1})",
                LogColor.Orange
            );
        }

        /// <inheritdoc/>
        public void Undo()
        {
            Vector2 reverseMovement = -direction * distance;
            InGameLogger.Log(
                $"  取消: {targetName}を{GetDirectionName()}の逆方向に{distance}移動 ({reverseMovement.x:F1}, {reverseMovement.y:F1})",
                LogColor.Yellow
            );
        }

        /// <summary>
        /// 方向ベクトルに対応する方向名を返す
        /// </summary>
        /// <returns>方向の日本語名称</returns>
        private string GetDirectionName()
        {
            if (direction == Vector2.up)
            {
                return "上";
            }
            if (direction == Vector2.down)
            {
                return "下";
            }
            if (direction == Vector2.left)
            {
                return "左";
            }
            if (direction == Vector2.right)
            {
                return "右";
            }
            return $"({direction.x:F1}, {direction.y:F1})";
        }
    }
}
