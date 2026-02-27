using UnityEngine;
using UnityEngine.UI;

namespace DesignPatterns.Behavioral.Mediator {
    /// <summary>
    /// Mediatorパターンのデモシーンを制御するクラス
    ///
    /// 【デモの内容】
    /// - チャットルーム（Mediator）を介してユーザー間でメッセージを送受信する
    /// - 各ユーザーは他のユーザーを直接参照せず、仲介者を通じて通信する
    /// - 送信者以外の全ユーザーにメッセージが配信される様子を確認できる
    /// </summary>
    public sealed class MediatorDemo : PatternDemoBase {
        /// <summary>Aliceがメッセージを送信するボタン</summary>
        [SerializeField]
        private Button aliceSendButton;

        /// <summary>Bobがメッセージを送信するボタン</summary>
        [SerializeField]
        private Button bobSendButton;

        /// <summary>Charlieがメッセージを送信するボタン</summary>
        [SerializeField]
        private Button charlieSendButton;

        /// <summary>チャットルーム（仲介者）</summary>
        private ChatRoom chatRoom;

        /// <summary>ユーザー: Alice</summary>
        private ChatUser alice;

        /// <summary>ユーザー: Bob</summary>
        private ChatUser bob;

        /// <summary>ユーザー: Charlie</summary>
        private ChatUser charlie;

        /// <summary>Aliceのメッセージ送信回数</summary>
        private int aliceMessageCount;

        /// <summary>Bobのメッセージ送信回数</summary>
        private int bobMessageCount;

        /// <summary>Charlieのメッセージ送信回数</summary>
        private int charlieMessageCount;

        /// <summary>Aliceの送信メッセージリスト</summary>
        private static readonly string[] AliceMessages = {
            "みんな元気?",
            "今日はいい天気だね",
            "ゲームしない?"
        };

        /// <summary>Bobの送信メッセージリスト</summary>
        private static readonly string[] BobMessages = {
            "やあ!",
            "そうだね!",
            "いいね、やろう!"
        };

        /// <summary>Charlieの送信メッセージリスト</summary>
        private static readonly string[] CharlieMessages = {
            "こんにちは!",
            "何してる?",
            "僕も参加するよ!"
        };

        /// <inheritdoc/>
        protected override string PatternName {
            get { return "Mediator"; }
        }

        /// <inheritdoc/>
        protected override PatternCategory Category {
            get { return PatternCategory.Behavioral; }
        }

        /// <inheritdoc/>
        protected override string Description {
            get { return "オブジェクト間の相互作用を仲介者に集約し、オブジェクト同士の直接参照を避ける"; }
        }

        /// <inheritdoc/>
        protected override void OnDemoStart() {
            chatRoom = new ChatRoom();

            alice = new ChatUser("Alice", chatRoom);
            bob = new ChatUser("Bob", chatRoom);
            charlie = new ChatUser("Charlie", chatRoom);

            InGameLogger.Log("--- チャットルームを作成 ---", LogColor.Yellow);
            chatRoom.AddUser(alice);
            chatRoom.AddUser(bob);
            chatRoom.AddUser(charlie);

            aliceMessageCount = 0;
            bobMessageCount = 0;
            charlieMessageCount = 0;

            if (aliceSendButton != null) {
                aliceSendButton.onClick.AddListener(OnAliceSend);
            }
            if (bobSendButton != null) {
                bobSendButton.onClick.AddListener(OnBobSend);
            }
            if (charlieSendButton != null) {
                charlieSendButton.onClick.AddListener(OnCharlieSend);
            }

            InGameLogger.Log("各ユーザーのボタンを押してメッセージを送信してください", LogColor.Yellow);
        }

        /// <summary>Aliceがメッセージを送信する</summary>
        private void OnAliceSend() {
            string message = AliceMessages[aliceMessageCount % AliceMessages.Length];
            aliceMessageCount++;
            InGameLogger.Log("--- Alice が送信 ---", LogColor.Yellow);
            alice.Send(message);
        }

        /// <summary>Bobがメッセージを送信する</summary>
        private void OnBobSend() {
            string message = BobMessages[bobMessageCount % BobMessages.Length];
            bobMessageCount++;
            InGameLogger.Log("--- Bob が送信 ---", LogColor.Yellow);
            bob.Send(message);
        }

        /// <summary>Charlieがメッセージを送信する</summary>
        private void OnCharlieSend() {
            string message = CharlieMessages[charlieMessageCount % CharlieMessages.Length];
            charlieMessageCount++;
            InGameLogger.Log("--- Charlie が送信 ---", LogColor.Yellow);
            charlie.Send(message);
        }
    }
}
