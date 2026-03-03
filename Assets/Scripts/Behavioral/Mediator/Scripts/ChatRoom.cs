using System.Collections.Generic;

namespace DesignPatterns.Behavioral.Mediator
{
    /// <summary>
    /// チャットルームとして機能する具象仲介者クラス
    /// Mediatorパターンにおける「ConcreteMediator」に相当する
    /// 参加者のリストを管理し、メッセージの配信を行う
    /// </summary>
    public sealed class ChatRoom : IChatMediator
    {
        /// <summary>チャットに参加しているユーザーのリスト</summary>
        private readonly List<ChatUser> users = new List<ChatUser>();

        /// <summary>
        /// ユーザーをチャットルームに追加する
        /// </summary>
        /// <param name="user">追加するユーザー</param>
        public void AddUser(ChatUser user)
        {
            users.Add(user);
            InGameLogger.Log($"  {user.Name} がチャットルームに参加しました", LogColor.Orange);
        }

        /// <summary>
        /// メッセージを送信者以外の全参加者に配信する
        /// </summary>
        /// <param name="message">送信するメッセージ</param>
        /// <param name="sender">メッセージの送信者</param>
        public void SendMessage(string message, ChatUser sender)
        {
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i] != sender)
                {
                    users[i].Receive(message, sender.Name);
                }
            }
        }
    }
}
