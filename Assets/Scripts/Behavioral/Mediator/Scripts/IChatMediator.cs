namespace DesignPatterns.Behavioral.Mediator
{
    /// <summary>
    /// チャットの仲介者を定義するインターフェース
    /// Mediatorパターンにおける「Mediator」に相当する
    /// オブジェクト間の通信を仲介し、直接参照を避ける
    /// </summary>
    public interface IChatMediator
    {
        /// <summary>
        /// メッセージを送信する
        /// 仲介者が送信者以外の全参加者にメッセージを配信する
        /// </summary>
        /// <param name="message">送信するメッセージ</param>
        /// <param name="sender">メッセージの送信者</param>
        void SendMessage(string message, ChatUser sender);
    }
}
