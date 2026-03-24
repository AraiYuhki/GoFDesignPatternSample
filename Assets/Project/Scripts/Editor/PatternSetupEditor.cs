using System.IO;
using GoFPatterns.Core;
using GoFPatterns.Patterns;
using GoFPatterns.UI;
using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.UI;

namespace GoFPatterns.Editor {
    /// <summary>
    /// パターン定義アセットとカードPrefabを生成するエディタユーティリティ
    /// コンパイル後に自動実行され、Tools/GoF Patterns/Setup Pattern Assets メニューからも手動実行可能
    /// </summary>
    public static class PatternSetupEditor {
        /// <summary>
        /// コンパイル後に PatternDefinition アセットが未生成の場合はセットアップを予約する
        /// </summary>
        [InitializeOnLoadMethod]
        public static void AutoSetup() {
            var existing = AssetDatabase.LoadAssetAtPath<PatternDefinition>($"{DefinitionsDir}/visitor.asset");
            if (existing == null) {
                EditorApplication.update += RunSetupOnNextUpdate;
            }
        }

        /// <summary>
        /// 次のエディタ更新フレームでSetupAllを実行し、自身を購読解除する
        /// </summary>
        private static void RunSetupOnNextUpdate() {
            EditorApplication.update -= RunSetupOnNextUpdate;
            SetupAll();
        }
        /// <summary>PatternDefinitionアセットの保存ディレクトリ</summary>
        private const string DefinitionsDir = "Assets/Project/ScriptableObjects/PatternDefinitions";
        /// <summary>PatternRepositoryアセットのパス</summary>
        private const string RepositoryPath = "Assets/Project/ScriptableObjects/PatternRepository.asset";
        /// <summary>カードPrefabの保存パス</summary>
        private const string CardPrefabPath = "Assets/Project/Prefabs/PatternCard.prefab";
        /// <summary>Prefabの保存ディレクトリ</summary>
        private const string PrefabsDir = "Assets/Project/Prefabs";

        /// <summary>
        /// パターン定義アセット・カードPrefab・シーン内参照を一括セットアップする
        /// </summary>
        [MenuItem("Tools/GoF Patterns/Setup Pattern Assets")]
        public static void SetupAll() {
            Debug.Log("[PatternSetupEditor] SetupAll 開始");
            EnsureDirectory(DefinitionsDir);
            EnsureDirectory(PrefabsDir);
            AssetDatabase.Refresh();

            var definitions = new PatternDefinition[] {
                // === 生成パターン (Creational) ===
                CreateOrLoadDefinition("singleton", "Singleton", "シングルトン",
                    PatternCategory.Creational,
                    "クラスのインスタンスを1つに制限し、グローバルアクセスポイントを提供する",
                    "Singletonパターンはクラスが持つインスタンスを唯一の1個に制限する設計パターンです。privateコンストラクタで外部からの生成を禁止し、staticなInstanceプロパティを通じてのみアクセスを許可します。",
                    "複数のオブジェクトが同じリソースや状態を共有する必要がある場面。例: GameManager、設定管理、ログシステム",
                    "グローバル状態の共有が必要な場合、複数インスタンスが矛盾を起こす恐れがある場合",
                    "インスタンス管理が単純化される、グローバルアクセスが容易、初期化タイミングの制御が可能",
                    "グローバル状態によりテストが困難になる、依存関係が隠れる、並列処理で注意が必要"
                ),
                CreateOrLoadDefinition("factory-method", "Factory Method", "ファクトリーメソッド",
                    PatternCategory.Creational,
                    "オブジェクト生成のインターフェースを定義し、サブクラスが生成するクラスを決定する",
                    "Factory Methodパターンはオブジェクト生成のための仮想コンストラクタを提供します。スーパークラスでオブジェクト生成のインターフェースを定義し、サブクラスがどのクラスのインスタンスを生成するかを決定します。",
                    "異なる種類のオブジェクトを同じインターフェースで生成したい場面。例: 敵キャラの生成、UIウィジェットの生成",
                    "生成するオブジェクトの型をサブクラスに委ねたい場合、クライアントに具体的な型を意識させたくない場合",
                    "オープン/クローズドの原則に従う、新しい製品を追加しやすい、具体的なクラスへの依存を排除できる",
                    "クラス階層が深くなりやすい、シンプルな生成には過剰な場合がある"
                ),
                CreateOrLoadDefinition("abstract-factory", "Abstract Factory", "アブストラクトファクトリー",
                    PatternCategory.Creational,
                    "関連するオブジェクト群を具象クラスを指定せずに生成するインターフェースを提供する",
                    "Abstract Factoryパターンは関連する一連のオブジェクトを、その具象クラスを指定することなく生成するためのインターフェースを提供します。ファクトリを切り替えるだけで、生成される製品群全体が一貫して変わります。",
                    "関連するオブジェクト群を一貫性をもって生成したい場面。例: UIテーマ、クロスプラットフォームUI",
                    "製品ファミリーの一貫性が必要な場合、具体的なクラスからクライアントを分離したい場合",
                    "製品ファミリー間の一貫性を保証できる、具象クラスの分離、ファクトリの交換が容易",
                    "新しい種類の製品追加が困難、クラス数が増加しやすい"
                ),
                CreateOrLoadDefinition("builder", "Builder", "ビルダー",
                    PatternCategory.Creational,
                    "複雑なオブジェクトの構築過程を分離し、同じ構築手順で異なる表現を生成する",
                    "Builderパターンは複雑なオブジェクトの構築をステップごとに行い、同じ構築プロセスで異なる表現のオブジェクトを生成できるようにします。Directorが構築手順を管理し、Builderが具体的な組み立てを担当します。",
                    "複雑なオブジェクトを段階的に構築する場面。例: キャラクタービルド、ドキュメント生成、クエリ構築",
                    "コンストラクタの引数が多い場合、同じ構築手順で異なるオブジェクトを作りたい場合",
                    "構築過程を細かく制御できる、同じ手順で異なる製品を生成可能、不完全なオブジェクト生成を防げる",
                    "クラス数が増加する、単純なオブジェクトには過剰"
                ),
                CreateOrLoadDefinition("prototype", "Prototype", "プロトタイプ",
                    PatternCategory.Creational,
                    "既存のオブジェクトをコピーして新しいオブジェクトを生成する",
                    "Prototypeパターンは既存のインスタンスをプロトタイプとして複製（Clone）することで新しいオブジェクトを生成します。newによる生成コストを避け、実行時にオブジェクトの種類を動的に増やせます。",
                    "オブジェクトの生成コストが高い場合や、実行時に生成する型を決定したい場面。例: 敵のバリエーション生成、設定テンプレート",
                    "生成コストが高い場合、クラス階層を増やさずにバリエーションを作りたい場合",
                    "生成コストを削減できる、実行時にオブジェクトの追加・削除が可能、クラス数を抑えられる",
                    "深いコピーが複雑になる場合がある、循環参照の処理が必要な場合がある"
                ),
                // === 構造パターン (Structural) ===
                CreateOrLoadDefinition("adapter", "Adapter", "アダプター",
                    PatternCategory.Structural,
                    "互換性のないインターフェース同士を接続し、協調動作させる",
                    "Adapterパターンは既存クラスのインターフェースをクライアントが期待する別のインターフェースに変換します。互換性のないクラス同士を組み合わせて動作させることができます。",
                    "既存のクラスを変更せずに新しいインターフェースで使いたい場面。例: レガシーAPI統合、サードパーティライブラリ連携",
                    "既存クラスのインターフェースが要件と合わない場合、既存コードを変更できない場合",
                    "既存クラスを再利用できる、単一責任の原則に従う、オープン/クローズドの原則に従う",
                    "全体の複雑さが増す、直接修正した方が単純な場合もある"
                ),
                CreateOrLoadDefinition("bridge", "Bridge", "ブリッジ",
                    PatternCategory.Structural,
                    "抽象と実装を分離し、それぞれを独立に変更可能にする",
                    "Bridgeパターンは抽象部分と実装部分を別々のクラス階層に分離し、両者を独立に拡張できるようにします。「has-a」関係で実装を保持することで、継承の爆発を防ぎます。",
                    "抽象と実装を独立に拡張したい場面。例: 図形×描画方式、プラットフォーム×機能",
                    "継承による組み合わせ爆発を避けたい場合、実行時に実装を切り替えたい場合",
                    "抽象と実装を独立に拡張可能、実装の詳細を隠蔽できる、実行時に実装を切り替え可能",
                    "設計が複雑になる、1つのクラスで十分な場合には過剰"
                ),
                CreateOrLoadDefinition("composite", "Composite", "コンポジット",
                    PatternCategory.Structural,
                    "オブジェクトをツリー構造に組み立て、個々と集合を同一視して扱う",
                    "Compositeパターンはオブジェクトをツリー構造で構成し、個々のオブジェクト（Leaf）と複合オブジェクト（Composite）を同じインターフェースで扱えるようにします。再帰的な構造を統一的に操作できます。",
                    "ツリー構造のデータを扱う場面。例: ファイルシステム、UIコンポーネント階層、組織図",
                    "部分-全体の階層構造がある場合、個々と集合を同じように扱いたい場合",
                    "ツリー構造を統一的に扱える、新しいコンポーネントの追加が容易、クライアントコードが単純化される",
                    "設計が過度に汎用化される場合がある、特定コンポーネントの制約が難しい"
                ),
                CreateOrLoadDefinition("decorator", "Decorator", "デコレーター",
                    PatternCategory.Structural,
                    "オブジェクトに動的に新しい責任を追加し、機能を柔軟に拡張する",
                    "Decoratorパターンは既存オブジェクトをラップして新しい振る舞いを動的に追加します。継承を使わずに、実行時にオブジェクトの機能を柔軟に拡張できます。",
                    "オブジェクトに動的に機能を追加したい場面。例: 武器の強化、ストリームのバッファリング/暗号化",
                    "サブクラス化による静的な拡張が不便な場合、実行時に機能の追加・削除をしたい場合",
                    "継承より柔軟な拡張が可能、単一責任の原則に従う、機能の組み合わせが自由",
                    "ラッパーの層が深くなると複雑、小さなオブジェクトが多数生成される"
                ),
                CreateOrLoadDefinition("facade", "Facade", "ファサード",
                    PatternCategory.Structural,
                    "複雑なサブシステムに対して統一された簡潔なインターフェースを提供する",
                    "Facadeパターンは複雑なサブシステム群に対してシンプルな統一インターフェースを提供します。クライアントはFacadeを通じてサブシステムを利用するため、サブシステムの複雑さを意識する必要がありません。",
                    "複雑なサブシステムを簡単に使いたい場面。例: ゲーム起動処理、メディア変換、フレームワーク初期化",
                    "複雑なシステムへのシンプルなアクセスが必要な場合、サブシステム間の依存を減らしたい場合",
                    "サブシステムの利用が簡単になる、クライアントとサブシステムの結合を低減、レイヤー化を促進",
                    "Facadeが神オブジェクトになるリスク、サブシステムへの直接アクセスを完全には防げない"
                ),
                CreateOrLoadDefinition("flyweight", "Flyweight", "フライウェイト",
                    PatternCategory.Structural,
                    "多数の類似オブジェクトで共有可能な部分を抽出し、メモリを節約する",
                    "Flyweightパターンはオブジェクトの内部状態（intrinsic）を共有し、外部状態（extrinsic）を分離することで、大量のオブジェクトを効率的にメモリ管理します。",
                    "同じようなオブジェクトが大量に必要な場面。例: 森の木々、文字のフォント、パーティクル",
                    "大量の類似オブジェクトがメモリを圧迫する場合、共有可能な状態がある場合",
                    "メモリ使用量を大幅に削減できる、大量オブジェクトの管理が効率化される",
                    "コードが複雑になる、外部状態の管理が必要、スレッドセーフティに注意が必要"
                ),
                CreateOrLoadDefinition("proxy", "Proxy", "プロキシ",
                    PatternCategory.Structural,
                    "他のオブジェクトへのアクセスを制御する代理オブジェクトを提供する",
                    "Proxyパターンは別のオブジェクトの代理として機能し、アクセス制御、遅延初期化、ログ記録などの追加機能を透過的に提供します。クライアントは実体と同じインターフェースでProxyを使用します。",
                    "オブジェクトへのアクセスを制御したい場面。例: 遅延読み込み、アクセス制御、リモートプロキシ",
                    "重いオブジェクトの遅延生成、アクセス権の制御、リモートオブジェクトの透過的利用が必要な場合",
                    "クライアントが気づかずに制御を追加できる、遅延初期化でパフォーマンス向上、オープン/クローズドの原則に従う",
                    "応答が遅延する場合がある、コードが複雑になる"
                ),
                // === 振る舞いパターン (Behavioral) ===
                CreateOrLoadDefinition("observer", "Observer", "オブザーバー",
                    PatternCategory.Behavioral,
                    "オブジェクトの状態変化を複数のオブジェクトへ自動的に通知・更新する",
                    "Observerパターンはオブジェクト（Subject）の状態変化を、それに依存する複数のオブジェクト（Observer）へ自動的に通知する設計パターンです。SubjectはObserverの具体的なクラスを知らずに通知できます。",
                    "あるオブジェクトの状態変化を複数の他のオブジェクトに反映したい場面。例: HP変化→HUD更新+SE再生+実績確認",
                    "1対多の依存関係を設定し、一方の変化が他に伝わるようにしたい場合",
                    "SubjectとObserverが疎結合になる、Observerの追加・削除が動的に行える、再利用性が高い",
                    "通知の連鎖で予期しない更新が起きる場合がある、Observerが増えるとパフォーマンスに影響することがある"
                ),
                CreateOrLoadDefinition("strategy", "Strategy", "ストラテジー",
                    PatternCategory.Behavioral,
                    "アルゴリズムのファミリーを定義し、実行時に交換可能にする",
                    "Strategyパターンはアルゴリズムをそれぞれのクラスにカプセル化し、同じインターフェースを持つオブジェクトとして交換可能にする設計パターンです。コンテキストは使用する戦略を実行時に変更できます。",
                    "同一の処理に複数のアルゴリズムが必要で、実行時に切り替えたい場面。例: ソートアルゴリズム、攻撃パターン、経路探索",
                    "アルゴリズムの変更がコンテキストに影響しないようにしたい場合、if/elseの分岐を減らしたい場合",
                    "アルゴリズムの交換が容易、コンテキストとアルゴリズムが疎結合、新戦略の追加が容易",
                    "戦略クラスが増加しやすい、クライアントが各戦略の違いを把握する必要がある"
                ),
                CreateOrLoadDefinition("state", "State", "ステート",
                    PatternCategory.Behavioral,
                    "オブジェクトの内部状態に応じて振る舞いを変化させる",
                    "Stateパターンはオブジェクトの内部状態が変化したとき、そのオブジェクトのクラスが変わったように振る舞いを変える設計パターンです。状態ごとのロジックをStateクラスに分離することでswitch文の肥大化を防ぎます。",
                    "オブジェクトが状態によって振る舞いが大きく変わる場面。例: キャラクターのAI状態、UIのモード切り替え",
                    "状態ごとに複雑な条件分岐が発生している場合、状態遷移が多数ある場合",
                    "状態ごとのコードが明確に分離される、状態追加が容易、switch/if-elseの肥大化を防げる",
                    "クラス数が増える、単純な状態管理には過剰な場合がある"
                ),
                CreateOrLoadDefinition("command", "Command", "コマンド",
                    PatternCategory.Behavioral,
                    "操作をオブジェクトとしてカプセル化し、Undo/Redoや操作のキュー化を可能にする",
                    "Commandパターンは操作をオブジェクトとしてカプセル化する設計パターンです。Invokerが具体的な処理を知らなくてもコマンドを実行でき、履歴管理によるUndo/Redoやマクロ記録が容易になります。",
                    "操作の取り消し・やり直し、操作のキュー化やログ記録が必要な場面。例: エディタ操作、ゲームコマンド",
                    "Undo/Redoが必要な場合、操作を後から実行・再実行したい場合、操作の記録や再生が必要な場合",
                    "Undo/Redoが容易、操作のキュー化・スケジューリングが可能、InvokerとReceiverが疎結合",
                    "コマンドクラスが多数になりやすい、シンプルな操作には過剰な場合がある"
                ),
                CreateOrLoadDefinition("chain-of-responsibility", "Chain of Responsibility", "チェーンオブレスポンシビリティ",
                    PatternCategory.Behavioral,
                    "リクエストを処理するオブジェクトのチェーンを構成し、順に処理を委譲する",
                    "Chain of Responsibilityパターンはリクエストの送信者と受信者を分離し、複数のハンドラオブジェクトを連鎖させて、いずれかが処理するまでリクエストを順に渡していきます。",
                    "複数のオブジェクトがリクエストを処理する可能性がある場面。例: サポートチケット、イベント処理、承認フロー",
                    "リクエストの処理者を動的に決定したい場合、複数の処理候補がある場合",
                    "送信者と受信者の結合を低減、処理の順序を柔軟に変更可能、単一責任の原則に従う",
                    "リクエストが処理されない可能性がある、チェーンが長くなるとデバッグが困難"
                ),
                CreateOrLoadDefinition("iterator", "Iterator", "イテレーター",
                    PatternCategory.Behavioral,
                    "コレクションの内部構造を公開せずに要素に順次アクセスする方法を提供する",
                    "Iteratorパターンはコレクションの内部表現を公開することなく、その要素に順次アクセスする手段を提供します。異なるデータ構造に対して統一的な走査インターフェースを実現します。",
                    "コレクションの走査を統一したい場面。例: リスト、ツリー、グラフの走査",
                    "コレクションの内部構造を隠蔽したい場合、複数の走査方法が必要な場合",
                    "コレクションと走査ロジックを分離できる、複数の走査を同時に実行可能、統一インターフェース",
                    "単純なコレクションには過剰、イテレータの生成コスト"
                ),
                CreateOrLoadDefinition("mediator", "Mediator", "メディエーター",
                    PatternCategory.Behavioral,
                    "オブジェクト間の複雑な相互作用を仲介者に集約し、疎結合を実現する",
                    "Mediatorパターンはオブジェクト間の直接的な参照を排除し、仲介者を通じてのみ通信させることで、オブジェクト間の結合を低減します。N対Nの関係を1対Nに単純化します。",
                    "複数のオブジェクトが複雑に相互作用する場面。例: チャットルーム、GUIコンポーネント連携、航空管制",
                    "オブジェクト間の依存関係が複雑になりすぎた場合、多対多の関係を整理したい場合",
                    "オブジェクト間の結合を低減、相互作用を一箇所に集約、オブジェクトの再利用が容易",
                    "Mediatorが複雑になりやすい（God Object化）、間接的な通信によるオーバーヘッド"
                ),
                CreateOrLoadDefinition("memento", "Memento", "メメント",
                    PatternCategory.Behavioral,
                    "オブジェクトの内部状態を保存・復元し、Undo機能などを実現する",
                    "Mementoパターンはオブジェクトの内部状態のスナップショットを保存し、後から復元できるようにします。カプセル化を破壊せずに状態の保存と復元を実現します。",
                    "オブジェクトの状態を保存・復元したい場面。例: テキストエディタのUndo、ゲームのセーブ/ロード",
                    "Undo/Redo機能が必要な場合、チェックポイントからの復元が必要な場合",
                    "カプセル化を維持しつつ状態を保存できる、Originator の実装が単純化される",
                    "メモリ消費が大きくなる場合がある、状態の保存コスト"
                ),
                CreateOrLoadDefinition("template-method", "Template Method", "テンプレートメソッド",
                    PatternCategory.Behavioral,
                    "アルゴリズムの骨格を定義し、一部のステップをサブクラスに委ねる",
                    "Template Methodパターンはアルゴリズムの構造をスーパークラスで定義し、具体的なステップの実装をサブクラスに委譲します。アルゴリズムの骨格を変えずに、個々のステップをカスタマイズできます。",
                    "アルゴリズムの構造は共通だが、一部のステップが異なる場面。例: データマイニング、ドキュメント生成",
                    "重複コードを排除したい場合、アルゴリズムの特定ステップのみ変更したい場合",
                    "重複コードの排除、アルゴリズムの構造を統一、拡張ポイントを明確化",
                    "ステップが多いと理解しづらい、Liskovの置換原則に違反しやすい"
                ),
                CreateOrLoadDefinition("interpreter", "Interpreter", "インタープリター",
                    PatternCategory.Behavioral,
                    "文法規則をクラスで表現し、文や式を解釈・評価する仕組みを提供する",
                    "Interpreterパターンは言語の文法をクラス階層で表現し、その文法に基づく文を解釈する仕組みを提供します。式をツリー構造として構築し、再帰的に評価します。",
                    "簡単な言語やDSLを解釈する場面。例: 数式評価、正規表現、設定ファイル解析",
                    "単純な文法の繰り返し解釈が必要な場合、DSLを実装する場合",
                    "文法の変更・拡張が容易、式の評価がツリー走査で実現できる",
                    "複雑な文法には不向き、クラス数が文法規則の数だけ増加する"
                ),
                CreateOrLoadDefinition("visitor", "Visitor", "ビジター",
                    PatternCategory.Behavioral,
                    "データ構造と処理を分離し、新しい処理を既存クラスを変更せずに追加する",
                    "Visitorパターンはオブジェクト構造の要素に対して実行する操作を別クラスに分離します。ダブルディスパッチにより、要素の型に応じた処理を追加でき、既存クラスの変更なしに新しい操作を導入できます。",
                    "データ構造に対して多様な処理を追加したい場面。例: AST解析、ドキュメント変換、図形の面積計算",
                    "データ構造は安定しているが、操作を頻繁に追加したい場合",
                    "新しい操作の追加が容易、関連する操作が一箇所に集約される、異なる型を横断して処理できる",
                    "新しい要素型の追加が困難、カプセル化が弱まる場合がある"
                ),
            };

            UpdatePatternRepository(definitions);
            CreateOrUpdateCardPrefab();
            WireSceneReferences();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("[PatternSetupEditor] セットアップ完了");
        }

        /// <summary>
        /// PatternDefinitionアセットを生成または読み込む
        /// </summary>
        /// <param name="id">パターンID</param>
        /// <param name="name">英語名</param>
        /// <param name="nameJp">日本語名</param>
        /// <param name="category">カテゴリ</param>
        /// <param name="summary">一言説明</param>
        /// <param name="description">概要</param>
        /// <param name="problemToSolve">解決する課題</param>
        /// <param name="whenToUse">使いどころ</param>
        /// <param name="advantages">メリット</param>
        /// <param name="disadvantages">デメリット</param>
        /// <returns>PatternDefinitionアセット</returns>
        private static PatternDefinition CreateOrLoadDefinition(
            string id, string name, string nameJp,
            PatternCategory category,
            string summary, string description,
            string problemToSolve, string whenToUse,
            string advantages, string disadvantages) {

            string assetPath = $"{DefinitionsDir}/{id}.asset";
            var existing = AssetDatabase.LoadAssetAtPath<PatternDefinition>(assetPath);
            if (existing != null) {
                return existing;
            }

            var def = ScriptableObject.CreateInstance<PatternDefinition>();
            var so = new SerializedObject(def);
            SetStr(so, "patternId", id);
            SetStr(so, "displayName", name);
            SetStr(so, "displayNameJp", nameJp);
            so.FindProperty("category").enumValueIndex = (int)category;
            SetStr(so, "summary", summary);
            SetStr(so, "description", description);
            SetStr(so, "problemToSolve", problemToSolve);
            SetStr(so, "whenToUse", whenToUse);
            SetStr(so, "advantages", advantages);
            SetStr(so, "disadvantages", disadvantages);
            so.ApplyModifiedProperties();

            AssetDatabase.CreateAsset(def, assetPath);
            return def;
        }

        /// <summary>
        /// PatternRepositoryのdefinitions配列を更新する
        /// </summary>
        /// <param name="definitions">登録するPatternDefinition配列</param>
        private static void UpdatePatternRepository(PatternDefinition[] definitions) {
            var repo = AssetDatabase.LoadAssetAtPath<PatternRepository>(RepositoryPath);
            if (repo == null) {
                Debug.LogWarning("[PatternSetupEditor] PatternRepository not found at: " + RepositoryPath);
                return;
            }
            var so = new SerializedObject(repo);
            var arrayProp = so.FindProperty("definitions");
            arrayProp.arraySize = definitions.Length;
            for (int i = 0; i < definitions.Length; i++) {
                arrayProp.GetArrayElementAtIndex(i).objectReferenceValue = definitions[i];
            }
            so.ApplyModifiedProperties();
        }

        /// <summary>
        /// パターンカードのPrefabを生成または更新する
        /// </summary>
        private static void CreateOrUpdateCardPrefab() {
            var existing = AssetDatabase.LoadAssetAtPath<GameObject>(CardPrefabPath);
            if (existing != null) {
                return;
            }

            var cardGo = new GameObject("PatternCard");
            var rt = cardGo.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(0f, 100f);

            cardGo.AddComponent<Image>().color = new Color(0.15f, 0.15f, 0.22f, 1f);
            cardGo.AddComponent<Button>();

            var nameTextGo = CreateChildText(cardGo.transform, "NameText",
                new Vector2(0f, 0.45f), new Vector2(1f, 1f), 22f, FontStyles.Bold);

            var summaryTextGo = CreateChildText(cardGo.transform, "SummaryText",
                new Vector2(0f, 0f), new Vector2(1f, 0.45f), 16f, FontStyles.Normal);
            summaryTextGo.GetComponent<TextMeshProUGUI>().color = new Color(0.8f, 0.8f, 0.8f, 1f);

            PrefabUtility.SaveAsPrefabAsset(cardGo, CardPrefabPath);
            Object.DestroyImmediate(cardGo);
        }

        /// <summary>
        /// シーン内のPatternListScreenにcardPrefabを設定し、フィルタボタンを配線する
        /// </summary>
        private static void WireSceneReferences() {
            var listScreen = Object.FindFirstObjectByType<PatternListScreen>(FindObjectsInactive.Include);
            if (listScreen == null) {
                Debug.LogWarning("[PatternSetupEditor] PatternListScreen がシーンに見つかりません。Main.unity を開いてから再実行してください。");
                return;
            }

            var cardPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(CardPrefabPath);
            if (cardPrefab != null) {
                var so = new SerializedObject(listScreen);
                var prop = so.FindProperty("cardPrefab");
                if (prop != null) {
                    prop.objectReferenceValue = cardPrefab;
                    so.ApplyModifiedProperties();
                }
            }

            WireFilterButtons(listScreen);

            EditorUtility.SetDirty(listScreen);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(listScreen.gameObject.scene);
        }

        /// <summary>
        /// PatternListPanel配下のフィルタボタンにSetFilter()イベントを配線する
        /// </summary>
        /// <param name="screen">対象のPatternListScreen</param>
        private static void WireFilterButtons(PatternListScreen screen) {
            var transform = screen.transform;
            var filterContainer = transform.Find("FilterContainer");
            if (filterContainer == null) {
                return;
            }

            WireFilterButton(filterContainer, "AllButton", screen, -1);
            WireFilterButton(filterContainer, "CreationalButton", screen, 0);
            WireFilterButton(filterContainer, "StructuralButton", screen, 1);
            WireFilterButton(filterContainer, "BehavioralButton", screen, 2);
        }

        /// <summary>
        /// 指定ボタンにSetFilterイベントを追加する
        /// </summary>
        /// <param name="parent">ボタンの親Transform</param>
        /// <param name="buttonName">ボタン名</param>
        /// <param name="screen">対象のPatternListScreen</param>
        /// <param name="filterIndex">フィルタインデックス</param>
        private static void WireFilterButton(Transform parent, string buttonName,
            PatternListScreen screen, int filterIndex) {
            var buttonTransform = parent.Find(buttonName);
            if (buttonTransform == null) {
                return;
            }
            var button = buttonTransform.GetComponent<Button>();
            if (button == null) {
                return;
            }

            // 既存のリスナーをクリアして再登録する
            button.onClick.RemoveAllListeners();
            UnityEventTools.AddIntPersistentListener(
                button.onClick,
                screen.SetFilter,
                filterIndex);
        }

        /// <summary>
        /// カード内のTextMeshProUGUI子オブジェクトを生成する
        /// </summary>
        /// <param name="parent">親Transform</param>
        /// <param name="name">オブジェクト名</param>
        /// <param name="anchorMin">アンカー最小値</param>
        /// <param name="anchorMax">アンカー最大値</param>
        /// <param name="fontSize">フォントサイズ</param>
        /// <param name="fontStyle">フォントスタイル</param>
        /// <returns>生成したGameObject</returns>
        private static GameObject CreateChildText(Transform parent, string name,
            Vector2 anchorMin, Vector2 anchorMax, float fontSize, FontStyles fontStyle) {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            var rt = go.AddComponent<RectTransform>();
            rt.anchorMin = anchorMin;
            rt.anchorMax = anchorMax;
            rt.offsetMin = new Vector2(8f, 2f);
            rt.offsetMax = new Vector2(-8f, -2f);
            var tmp = go.AddComponent<TextMeshProUGUI>();
            tmp.fontSize = fontSize;
            tmp.fontStyle = fontStyle;
            tmp.color = Color.white;
            tmp.textWrappingMode = TextWrappingModes.NoWrap;
            tmp.overflowMode = TextOverflowModes.Ellipsis;
            return go;
        }

        /// <summary>
        /// SerializedObjectのstringプロパティを設定するヘルパー
        /// </summary>
        /// <param name="so">対象のSerializedObject</param>
        /// <param name="propertyName">プロパティ名</param>
        /// <param name="value">設定する値</param>
        private static void SetStr(SerializedObject so, string propertyName, string value) {
            var prop = so.FindProperty(propertyName);
            if (prop != null) {
                prop.stringValue = value;
            }
        }

        /// <summary>
        /// ディレクトリが存在しない場合は作成する
        /// </summary>
        /// <param name="path">作成するディレクトリパス</param>
        private static void EnsureDirectory(string path) {
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
        }
    }
}
