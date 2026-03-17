# ClaudeCode向けロードマップ（チェックリスト版）

## プロジェクト名
Unityで学ぶGoFデザインパターン視覚化プロジェクト

## 凡例
- `[ ]` 未着手
- `[→]` 作業中
- `[x]` 完了

---

## Phase 1: Foundation（基盤構築）

### M0: プロジェクト初期化

既存プロジェクトをベースに、実装計画書のディレクトリ構成に合わせて再構築する。

- [x] M0-1: `Assets/Project/` 配下のディレクトリ構成を作成する
  - `Scripts/Core/{Bootstrap,Common,Logging,Navigation}`
  - `Scripts/Patterns/Shared/{Interfaces,Base,Data,Visualization}`
  - `Scripts/Patterns/{Singleton,FactoryMethod,Observer,Strategy,State,Command}/{Domain,Application,Presentation,Data}`
  - `Scripts/UI/{Screens,Components,ViewModels}`
  - `ScriptableObjects/{PatternDefinitions,DemoScenarios}`
  - `Prefabs/{UI,Patterns,Common}`
  - `Art/{Icons,Sprites}`
  - `Scenes/`
- [x] M0-2: Assembly Definition を新構成に合わせて再配置する
- [ ] M0-3: Main シーンを `Assets/Project/Scenes/Main.unity` に作成する（※Unity Editor作業）
- [x] M0-4: README.md の雛形を作成する

**完了条件**: プロジェクトが起動し、Mainシーンが開ける

---

### M1: 共通フレームワーク構築

#### M1-A: データモデル

- [x] M1-A1: `PatternCategory` enum を実装する（Creational / Structural / Behavioral）
- [x] M1-A2: `PatternDefinition` ScriptableObject を実装する
  - PatternId, 表示名, カテゴリ, 概要, いつ使うか, メリット, デメリット, 関連パターン, ステップ説明文
- [x] M1-A3: `PatternRepository`（MetadataProvider）を実装する
  - 全定義の取得, ID指定取得, カテゴリ指定フィルタ
- [ ] M1-A4: MVP 6パターン分の PatternDefinition アセットを作成する（※Unity Editor作業）
  - Singleton, Factory Method, Observer, Strategy, State, Command

#### M1-B: 画面遷移基盤

- [x] M1-B1: 画面遷移管理の `ScreenManager` を実装する（1シーン内パネル切り替え方式）
- [x] M1-B2: `BaseScreen` 基底クラスを実装する（Show/Hide/Initialize）
- [ ] M1-B3: グローバル Canvas と共通パネルレイアウトを作成する（※Unity Editor作業）

#### M1-C: 画面UI

- [x] M1-C1: `PatternListScreen` を実装する
  - パターンカード一覧表示（名前・カテゴリ・一言説明）
  - カテゴリタブまたはフィルタ
  - カード選択で詳細画面へ遷移
- [x] M1-C2: `PatternDetailScreen` を実装する
  - PatternDefinition の情報表示（概要・使いどころ・メリット・注意点）
  - デモ開始ボタン → デモ画面へ遷移
  - 戻るボタン → 一覧へ遷移
- [x] M1-C3: `DemoScreen` のコンテナを実装する
  - パターン名・状態表示
  - 再生/停止/リセット/ステップ送りボタン
  - 自動再生速度スライダー
  - ログ表示領域
  - 図示エリア（2Dビジュアライゼーション用 RawImage）
  - ステップ説明文表示
  - 戻るボタン → 詳細画面へ遷移

**完了条件**: ScriptableObject のパターン情報が一覧→詳細→デモ画面の順に表示・遷移できる

---

### M2: デモ実行基盤

#### M2-A: コアインターフェースと基底クラス

- [x] M2-A1: `IPatternDemo` インターフェースを実装する
  - PatternId, DisplayName, Initialize, Play, Pause, Resume, Stop, ResetDemo, CanStep, StepForward, GetCurrentLogs
- [x] M2-A2: `BasePatternDemo` 抽象クラスを実装する（IPatternDemo の共通実装）
- [x] M2-A3: `DemoStep` クラスを実装する（1ステップの処理定義）
- [x] M2-A4: `DemoScenario` クラスを実装する（ステップ列の管理）

#### M2-B: 実行制御

- [x] M2-B1: `DemoManager` を実装する（デモの生成・開始・停止を統括）
- [x] M2-B2: StepForward 機構を実装する（1ステップずつ進行）
- [x] M2-B3: AutoPlay 機構を実装する（自動再生＋速度制御）
- [x] M2-B4: Reset 機構を実装する（初期状態に戻す）

#### M2-C: ログサービス

- [x] M2-C1: `LogService` を実装する（ログ蓄積・通知）
  - 形式: [Step XX] Actor -> Action -> Result
- [x] M2-C2: `LogPanel` UIを実装する（DemoScreen内のログ表示として統合済み）

#### M2-D: ビジュアライゼーション基盤

- [x] M2-D1: `IPatternVisualization` インターフェースを実装する
  - Bind, Refresh, Highlight
- [x] M2-D2: `VisualizationRenderer`（RenderTexture + 2Dカメラ）を実装する
- [x] M2-D3: `VisualElement`（2D図形 + ラベル + アニメーション）を実装する
- [x] M2-D4: `VisualArrow`（要素間の矢印）を実装する
- [ ] M2-D5: ダミーデモで基盤全体の動作確認を行う（※Unity Editor作業）

**完了条件**: ダミーデモで Play/Stop/Reset/StepForward が動作し、ログが表示される

---

## Phase 2: MVP Delivery（6パターン実装）

### M3: Singleton デモ

最初の実動デモ。共通基盤の妥当性を検証する。

- [ ] M3-1: Singleton ドメインモデルを実装する（GameManager の唯一インスタンス管理）
- [ ] M3-2: Singleton デモシナリオを実装する（ステップ列）
  - 初回インスタンス取得、2回目以降の重複試行、既存参照の返却
- [ ] M3-3: Singleton ビジュアライゼーションを実装する
  - インスタンス生成の視覚表示、重複拒否の視覚フィードバック
- [ ] M3-4: PatternDefinition データを整備する（説明文・ステップ説明）
- [ ] M3-5: 動作確認：再生・リセット・ステップ実行が成立すること

**完了条件**: 単一インスタンス性が視覚的に理解でき、デモとして成立する

---

### M4: Factory Method / Observer デモ

生成系と通知系の代表パターン。

#### Factory Method

- [ ] M4-F1: Factory Method ドメインモデルを実装する
  - IEnemy, Slime/Goblin/Dragon, EnemyFactory, ConcreteCreator群
- [ ] M4-F2: Factory Method デモシナリオを実装する
  - UI選択 → Factory経由で生成 → 生成物の型差異をログ表示
- [ ] M4-F3: Factory Method ビジュアライゼーションを実装する
  - 異なる生成物が異なる見た目で表示される
- [ ] M4-F4: PatternDefinition データを整備する

#### Observer

- [ ] M4-O1: Observer ドメインモデルを実装する
  - Subject (HealthSystem), Observer群 (HPBar, Sound, Effect)
- [ ] M4-O2: Observer デモシナリオを実装する
  - 値変更 → 複数Observer通知 → 購読解除テスト
- [ ] M4-O3: Observer ビジュアライゼーションを実装する
  - Subject→Observer への通知伝播アニメーション
- [ ] M4-O4: PatternDefinition データを整備する

**完了条件**: 生成物の違い・通知伝播が視覚的に理解できる

---

### M5: Strategy / State / Command デモ

振る舞いパターンの3種。

#### Strategy

- [ ] M5-S1: Strategy ドメインモデルを実装する
  - Context, IStrategy, ConcreteStrategy群（攻撃的/防御的/逃走）
- [ ] M5-S2: Strategy デモシナリオを実装する
  - 戦略選択 → 同一文脈で振る舞い変化
- [ ] M5-S3: Strategy ビジュアライゼーションを実装する
  - 戦略切り替えと結果差分の視覚化
- [ ] M5-S4: PatternDefinition データを整備する

#### State

- [ ] M5-T1: State ドメインモデルを実装する
  - Context, IState, ConcreteState群（Idle/Move/Attack/Dead）
- [ ] M5-T2: State デモシナリオを実装する
  - 入力イベント → 状態遷移 → 遷移ログ
- [ ] M5-T3: State ビジュアライゼーションを実装する
  - 現在状態ハイライト、遷移アニメーション
- [ ] M5-T4: PatternDefinition データを整備する

#### Command

- [ ] M5-C1: Command ドメインモデルを実装する
  - ICommand, ConcreteCommand群, Invoker, Receiver
- [ ] M5-C2: Command デモシナリオを実装する
  - コマンド投入 → 実行キュー表示 → Undo
- [ ] M5-C3: Command ビジュアライゼーションを実装する
  - 実行履歴リスト、Undo の逆操作表示
- [ ] M5-C4: PatternDefinition データを整備する

**完了条件**: 3パターンで差分と責務が視覚的に理解でき、ステップ実行が動作する

---

## Phase 3: Expansion Ready（統合・品質・拡張準備）

### M6: MVP統合と品質向上

- [ ] M6-1: 一覧画面のレイアウト・デザインを整頓する
- [ ] M6-2: 詳細画面の説明文を教材として調整する
- [ ] M6-3: 画面遷移の導線を確認・改善する（迷わず到達できるか）
- [ ] M6-4: 6パターン全体のUI一貫性を確認する
- [ ] M6-5: Nullチェック・例外耐性を改善する
- [ ] M6-6: 命名規則の統一を確認する
- [ ] M6-7: 不要コードを削除する
- [ ] M6-8: ログ文言を教材向けに改善する

**完了条件**: 初見ユーザーが一覧→デモまで迷わず到達でき、6パターンが安定動作する

---

### M7: ドキュメント整備

- [ ] M7-1: README.md を整備する
  - プロジェクト概要, Unityバージョン, セットアップ手順, 起動方法, デモ追加方法, ディレクトリ構成, 既知の制限
- [ ] M7-2: Docs/Architecture.md を作成する
  - レイヤ構成, コアインターフェース, データフロー, デモ実行モデル
- [ ] M7-3: Docs/ExpansionGuide.md を作成する
  - 新パターン追加手順, テンプレート, 接続ポイント

**完了条件**: 第三者が README を読んで起動でき、新パターン追加方法が文書化されている

---

## 進捗サマリー

| マイルストーン | タスク数 | 完了 | 状態 |
|--------------|---------|------|------|
| M0: プロジェクト初期化 | 4 | 3 | Unity Editor作業残り |
| M1: 共通フレームワーク | 10 | 8 | Unity Editor作業残り |
| M2: デモ実行基盤 | 12 | 11 | Unity Editor作業残り |
| M3: Singleton | 5 | 0 | 未着手 |
| M4: Factory Method / Observer | 8 | 0 | 未着手 |
| M5: Strategy / State / Command | 12 | 0 | 未着手 |
| M6: MVP統合・品質 | 8 | 0 | 未着手 |
| M7: ドキュメント | 3 | 0 | 未着手 |
| **合計** | **62** | **22** | |

---

## 参考: 既存資産の再利用方針

現在のリポジトリには全23パターンのドメインモデル実装が存在する。
新アーキテクチャへの移行にあたり、以下の方針で既存コードを扱う。

| 既存資産 | 方針 |
|---------|------|
| 各パターンの Domain 層スクリプト（敵生成、HP管理等） | 新構成に移動し、IPatternDemo に適合するよう **リファクタして再利用** |
| PatternDemoBase / InGameLogger / FlyweightScrollView | 新アーキテクチャの LogService / DemoManager に **置き換え** |
| NodeGraph 系スクリプト | **廃止**（2Dビジュアライゼーション方式に統一） |
| Visualization 系スクリプト（ShapeFactory等） | IPatternVisualization に適合するよう **リファクタして再利用** |
| 既存シーン・プレハブ | 新構成の1シーン方式に **作り直し** |

---

## 参考: MVP後の拡張順序

MVP（6パターン）完成後、以下の順で全23パターンへ拡張する。

### Stage A: 追加しやすい代表パターン
- Decorator, Adapter, Template Method, Builder

### Stage B: 関係性の理解に強いパターン
- Mediator, Composite, Proxy, Facade

### Stage C: 難易度が上がるパターン
- Abstract Factory, Prototype, Bridge, Flyweight, Chain of Responsibility, Iterator, Visitor, Interpreter, Memento

### Stage D: 全23パターン完成
- 残りを整理して全網羅、カテゴリ別学習導線の強化
