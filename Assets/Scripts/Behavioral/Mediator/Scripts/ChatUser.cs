namespace DesignPatterns.Behavioral.Mediator
{
    /// <summary>
    /// チャットに参加するユーザークラス
    /// Mediatorパターンにおける「Colleague」に相当する
    /// 仲介者を通じて他のユーザーと通信する
    /// </summary>
    public sealed class ChatUser
    {
        /// <summary>ユーザー名</summary>
        private readonly string name;

        /// <summary>チャットの仲介者への参照</summary>
        private readonly IChatMediator mediator;

        /// <summary>ユーザー名を取得する</summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// ChatUserを生成する
        /// </summary>
        /// <param name="name">ユーザー名</param>
        /// <param name="mediator">チャットの仲介者</param>
        public ChatUser(string name, IChatMediator mediator)
        {
            this.name = name;
            this.mediator = mediator;
        }

        /// <summary>
        /// 仲介者を通じてメッセージを送信する
        /// </summary>
        /// <param name="message">送信するメッセージ</param>
        public void Send(string message)
        {
            InGameLogger.Log($"  [{name}] 送信: \"{message}\"", LogColor.Orange);
            mediator.SendMessage(message, this);
        }

        /// <summary>
        /// 他のユーザーからのメッセージを受信する
        /// </summary>
        /// <param name="message">受信したメッセージ</param>
        /// <param name="senderName">送信者の名前</param>
        public void Receive(string message, string senderName)
        {
            InGameLogger.Log($"  [{name}] 受信 ({senderName}から): \"{message}\"", LogColor.White);
        }
    }
}
