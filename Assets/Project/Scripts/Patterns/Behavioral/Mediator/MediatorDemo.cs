using System.Collections.Generic;
using System.Text;

namespace GoFPatterns.Patterns {
    // ---- Mediator interface ----

    /// <summary>
    /// Mediatorパターンの仲介者インターフェース
    /// 同僚オブジェクト間のメッセージ中継を定義する
    /// </summary>
    public interface IChatMediator {
        /// <summary>
        /// メッセージを送信する
        /// </summary>
        /// <param name="sender">送信者</param>
        /// <param name="message">メッセージ内容</param>
        void SendMessage(ChatUser sender, string message);
        /// <summary>
        /// ユーザーを登録する
        /// </summary>
        /// <param name="user">登録するユーザー</param>
        void AddUser(ChatUser user);
    }

    // ---- Colleague ----

    /// <summary>
    /// Mediatorパターンの同僚クラス
    /// Mediatorを介して他のユーザーと通信するチャットユーザー
    /// </summary>
    public class ChatUser {
        /// <summary>ユーザー名</summary>
        private readonly string name;
        /// <summary>参照する仲介者</summary>
        private readonly IChatMediator mediator;
        /// <summary>受信したメッセージの一覧</summary>
        private readonly List<string> receivedMessages = new List<string>();

        /// <summary>ユーザー名を取得する</summary>
        public string Name => name;
        /// <summary>受信メッセージ数を取得する</summary>
        public int ReceivedCount => receivedMessages.Count;

        /// <summary>
        /// ChatUserを生成する
        /// </summary>
        /// <param name="name">ユーザー名</param>
        /// <param name="mediator">仲介者</param>
        public ChatUser(string name, IChatMediator mediator) {
            this.name = name;
            this.mediator = mediator;
        }

        /// <summary>
        /// メッセージを送信する（仲介者に委譲する）
        /// </summary>
        /// <param name="message">送信するメッセージ</param>
        public void Send(string message) {
            mediator.SendMessage(this, message);
        }

        /// <summary>
        /// メッセージを受信する
        /// </summary>
        /// <param name="senderName">送信者の名前</param>
        /// <param name="message">受信したメッセージ</param>
        public void Receive(string senderName, string message) {
            receivedMessages.Add($"{senderName}: {message}");
        }

        /// <summary>
        /// 最後に受信したメッセージを取得する
        /// </summary>
        /// <returns>最後の受信メッセージ（なければ空文字列）</returns>
        public string GetLastMessage() {
            if (receivedMessages.Count == 0) {
                return "";
            }
            return receivedMessages[receivedMessages.Count - 1];
        }
    }

    // ---- ConcreteMediator ----

    /// <summary>
    /// Mediatorパターンの具象仲介者
    /// 登録されたユーザー間のメッセージ中継を行うチャットルーム
    /// </summary>
    public class ChatRoom : IChatMediator {
        /// <summary>登録されたユーザー一覧</summary>
        private readonly List<ChatUser> users = new List<ChatUser>();

        /// <summary>登録ユーザー数を取得する</summary>
        public int UserCount => users.Count;

        /// <summary>
        /// ユーザーを登録する
        /// </summary>
        /// <param name="user">登録するユーザー</param>
        public void AddUser(ChatUser user) {
            users.Add(user);
        }

        /// <summary>
        /// 送信者以外の全ユーザーにメッセージを中継する
        /// </summary>
        /// <param name="sender">送信者</param>
        /// <param name="message">メッセージ内容</param>
        public void SendMessage(ChatUser sender, string message) {
            foreach (ChatUser user in users) {
                if (user != sender) {
                    user.Receive(sender.Name, message);
                }
            }
        }

        /// <summary>
        /// 送信時に受信したユーザー名の一覧を取得する
        /// </summary>
        /// <param name="sender">送信者</param>
        /// <returns>受信者名をカンマ区切りにした文字列</returns>
        public string GetReceiverNames(ChatUser sender) {
            var result = new StringBuilder();
            bool first = true;
            foreach (ChatUser user in users) {
                if (user != sender) {
                    if (!first) {
                        result.Append(", ");
                    }
                    result.Append(user.Name);
                    first = false;
                }
            }
            return result.ToString();
        }
    }

    // ---- Demo ----

    /// <summary>
    /// Mediatorパターンのデモ
    /// ChatRoomを例にユーザー間の疎結合なメッセージ通信の仕組みを示す
    /// </summary>
    [PatternDemo("mediator")]
    public class MediatorDemo : BasePatternDemo {
        /// <summary>デモのパターンID</summary>
        public override string PatternId => "mediator";

        /// <summary>デモの表示名</summary>
        public override string DisplayName => "Mediator";

        /// <summary>仲介者としてのチャットルーム</summary>
        private ChatRoom chatRoom;
        /// <summary>ユーザーAlice</summary>
        private ChatUser alice;
        /// <summary>ユーザーBob</summary>
        private ChatUser bob;
        /// <summary>ユーザーCharlie</summary>
        private ChatUser charlie;

        /// <summary>
        /// リセット時にチャットルームとユーザーを再生成する
        /// </summary>
        protected override void OnReset() {
            chatRoom = null;
            alice = null;
            bob = null;
            charlie = null;
        }

        /// <summary>
        /// Mediatorパターンのシナリオを構築する
        /// </summary>
        /// <param name="scenario">ステップを追加するシナリオ</param>
        protected override void BuildScenario(DemoScenario scenario) {
            chatRoom = new ChatRoom();

            scenario.AddStep(new DemoStep(
                "ChatRoom（仲介者）を生成する",
                () => {
                    Log("Client", "ChatRoom生成", "仲介者としてChatRoomを生成");
                }
            ));

            scenario.AddStep(new DemoStep(
                "3人のユーザーをChatRoomに登録する",
                () => {
                    alice = new ChatUser("Alice", chatRoom);
                    bob = new ChatUser("Bob", chatRoom);
                    charlie = new ChatUser("Charlie", chatRoom);
                    chatRoom.AddUser(alice);
                    chatRoom.AddUser(bob);
                    chatRoom.AddUser(charlie);
                    Log("ChatRoom", "AddUser x3", $"Alice, Bob, Charlie を登録 (ユーザー数: {chatRoom.UserCount})");
                }
            ));

            scenario.AddStep(new DemoStep(
                "AliceがメッセージをChatRoom経由で送信する",
                () => {
                    alice.Send("こんにちは、皆さん!");
                    string receivers = chatRoom.GetReceiverNames(alice);
                    Log("Alice", "Send(こんにちは、皆さん!)", $"受信者: {receivers}");
                    Log("→ Bob", "Receive", bob.GetLastMessage());
                    Log("→ Charlie", "Receive", charlie.GetLastMessage());
                }
            ));

            scenario.AddStep(new DemoStep(
                "BobがChatRoom経由で返信する",
                () => {
                    bob.Send("やあ、Alice!");
                    string receivers = chatRoom.GetReceiverNames(bob);
                    Log("Bob", "Send(やあ、Alice!)", $"受信者: {receivers}");
                    Log("→ Alice", "Receive", alice.GetLastMessage());
                    Log("→ Charlie", "Receive", charlie.GetLastMessage());
                }
            ));

            scenario.AddStep(new DemoStep(
                "ユーザー同士は互いを直接参照していないことを確認する",
                () => {
                    Log("Mediator", "疎結合の確認",
                        $"Alice受信数: {alice.ReceivedCount}, Bob受信数: {bob.ReceivedCount}, Charlie受信数: {charlie.ReceivedCount}");
                    Log("Mediator", "設計上の利点", "ユーザーはChatRoomのみ参照し、他ユーザーを直接知らない");
                }
            ));
        }
    }
}
