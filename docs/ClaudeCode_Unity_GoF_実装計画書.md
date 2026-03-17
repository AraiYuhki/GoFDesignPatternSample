# ClaudeCode向け実装計画書
## プロジェクト名
Unityで学ぶGoFデザインパターン視覚化プロジェクト

## 1. この文書の目的
この文書は、ClaudeCode がそのまま実装に着手できるように、プロジェクトの目的、要求仕様、アーキテクチャ、ディレクトリ構成、実装順序、受け入れ条件を定義するための実装計画書である。

本プロジェクトの目的は、GoFデザインパターンを Unity 上で「動く・見える・比較できる」形で学べる教育用アプリケーションを構築することにある。
単なるサンプルコード集ではなく、以下を重視する。

- パターンの責務と意図が視覚的に理解できること
- パターン未適用時と適用時を比較できること
- 実行中にオブジェクト間の関係やイベント伝播が確認できること
- 将来的に23パターンすべてを段階的に追加できる拡張性があること

---

## 2. ClaudeCodeへの最重要指示
ClaudeCode は以下の方針を厳守して実装すること。

### 2.1 開発方針
- Unity 2022 LTS 以降を前提とする
- 言語は C#
- UI は uGUI でも UI Toolkit でもよいが、初期実装は工数の少ないほうを選ぶ
- MVP を最優先し、最初から23パターン全部は実装しない
- 最初は代表的な 6 パターンを確実に動かす
- 実装は「デモの追加が簡単な構造」を最優先する
- 教材アプリとして、コードの可読性と責務分離を重視する
- すべてのデモは共通インターフェース上で管理する
- デモごとに UI とロジックを過度に密結合させない
- ScriptableObject を積極的に使い、説明文・タイトル・カテゴリ・ステップ説明などはデータ化する
- 将来、23パターン追加時に既存コード修正を最小化できる構造にする

### 2.2 実装物の優先順位
最優先で作るもの:
1. アプリ全体の基本フレーム
2. パターン一覧画面
3. パターン詳細画面
4. デモ実行基盤
5. 代表6パターンの動作デモ
6. ステップ実行・ログ表示・関係図表示
7. README と拡張ガイド

後回しでよいもの:
- 高度なアニメーション演出
- 派手なエフェクト
- セーブデータ
- 多言語対応
- 音声ナレーション
- モバイル向け最適化
- Addressables 対応

### 2.3 実装対象のMVPパターン
MVPとして、まず以下の6パターンを実装すること。

- Singleton
- Factory Method
- Observer
- Strategy
- State
- Command

理由:
- 初学者への説明価値が高い
- Unityとの相性がよい
- 振る舞いの視覚化がしやすい
- 他パターンの基盤にもなりやすい

---

## 3. 成果物の定義
ClaudeCode は最終的に以下を成果物として出力すること。

### 必須成果物
- Unityプロジェクト一式
- 実行可能なメインシーン
- パターン一覧UI
- パターン詳細UI
- デモ再生UI
- MVP 6パターンの各デモ
- 共通デモ基盤
- 説明データ用 ScriptableObject 群
- README.md
- Docs/ExpansionGuide.md
- Docs/Architecture.md

### README に必ず含める内容
- プロジェクト概要
- Unity バージョン
- セットアップ手順
- 起動方法
- デモ追加方法
- パターン追加方法
- ディレクトリ構成
- 既知の制限

---

## 4. アプリケーション要件
## 4.1 想定ユーザー
- GoFデザインパターンを学びたい初学者
- Unity/C# を学ぶ学生
- 研修教材を作りたい教育者
- 面接対策・設計学習をしたいエンジニア

## 4.2 ユースケース
1. ユーザーはアプリ起動後、パターン一覧を見る
2. パターンを選択すると概要、用途、メリット、注意点を見る
3. デモを開始すると、オブジェクトやイベントが可視化される
4. 自動再生またはステップ実行で理解を深める
5. ログと図を見ながら挙動を比較する
6. 別のパターンに切り替える

## 4.3 機能要件
### 必須
- パターン一覧表示
- カテゴリ表示（生成・構造・振る舞い）
- パターン詳細表示
- デモ開始・停止・リセット
- ステップ送り
- 自動再生
- 実行ログ表示
- 関係図または構成図表示
- 各パターンの簡易説明表示
- 共通UIフレームで複数デモを切り替え可能

### 推奨
- 未適用版と適用版の比較表示
- スピード変更
- ステップ説明文表示
- ハイライト演出
- 現在アクティブなオブジェクト表示

### 任意
- クイズ機能
- コード断片表示
- UML風図の生成
- 進捗管理

---

## 5. 非機能要件
- 追加パターン実装時に既存コード変更量が少ないこと
- シーン分割より Prefab + データ駆動を優先すること
- デモ単位で責務が閉じること
- UI とドメインロジックを分離すること
- NullReference に依存した設計を避けること
- ログ出力をデバッグ用途だけでなく教育用途でも活用すること
- 命名規則を統一すること
- コメントは「なぜ」を説明し、「何をしているか」は命名で表すこと

---

## 6. 推奨アーキテクチャ
MVPでは過剰設計を避けつつ、拡張しやすい構造を採用すること。

### 6.1 レイヤ構成
- Presentation
  - 画面UI
  - ボタン入力
  - 表示更新
- Application
  - デモ実行制御
  - ステップ進行
  - ログ管理
  - データ読み込み
- Domain
  - 各パターンの純粋な概念モデル
  - 共通インターフェース
- Infrastructure
  - ScriptableObject データ
  - Unityコンポーネント橋渡し
  - アセット参照

### 6.2 中核インターフェース
以下のような共通インターフェースを作ること。

```csharp
public interface IPatternDemo
{
    string PatternId { get; }
    string DisplayName { get; }
    void Initialize();
    void Play();
    void Pause();
    void Resume();
    void Stop();
    void ResetDemo();
    bool CanStep { get; }
    void StepForward();
    IReadOnlyList<string> GetCurrentLogs();
}
```

必要に応じて以下も分離すること。

```csharp
public interface IPatternVisualization
{
    void Bind(IPatternDemo demo);
    void Refresh();
    void Highlight(string targetId);
}
```

```csharp
public interface IPatternMetadataProvider
{
    PatternDefinition GetDefinition(string patternId);
    IReadOnlyList<PatternDefinition> GetAllDefinitions();
}
```

### 6.3 デモ実行モデル
各デモは以下の構造を推奨する。

- PatternDefinition: タイトルや解説を持つデータ
- DemoController: 再生制御
- DemoScenario: 手順列
- DemoStep: 1ステップごとの処理
- DemoVisualizer: 視覚表現
- DemoLogService: ログ出力
- PatternDomainModel: パターンの概念モデル

### 6.4 データ駆動方針
ScriptableObject で以下を保持すること。

- PatternId
- 表示名
- カテゴリ
- 概要
- いつ使うか
- メリット
- デメリット
- 関連パターン
- ステップ説明文
- 実装メモ
- サンプルコード断片（任意）

---

## 7. 画面設計
## 7.1 画面一覧
最低限以下の画面を実装すること。

1. Home / Pattern List
2. Pattern Detail
3. Demo View

MVPではシーンを分けず、1シーン内のパネル切り替えでもよい。

## 7.2 Pattern List 画面
表示項目:
- タイトル
- カテゴリタブまたはフィルタ
- パターンカード一覧
- パターン概要の短文

パターンカード項目:
- パターン名
- カテゴリ
- 一言説明
- 難易度（任意）

## 7.3 Pattern Detail 画面
表示項目:
- パターン名
- カテゴリ
- 概要
- 解決したい課題
- 使いどころ
- メリット
- 注意点
- 主要登場人物
- デモ開始ボタン

## 7.4 Demo View 画面
表示項目:
- パターン名
- 状態表示
- 再生/停止/リセット
- ステップ送り
- 自動再生速度
- ログ表示領域
- 図示エリア
- ステップ説明文
- 戻るボタン

---

## 8. 視覚化ポリシー
各パターンは最低限、以下のどれかで視覚化すること。

- ノードと矢印
- キャラクターやアイコンの状態変化
- イベント伝播ログ
- コマンドキューの追加・実行表示
- 選択中戦略の切り替え表示
- 状態遷移図と現在状態のハイライト

視覚化は見た目の豪華さより、理解しやすさを優先する。

---

## 9. MVP対象パターンごとの実装要件
## 9.1 Singleton
題材例:
- GameManager / AudioManager の唯一インスタンス

表示要件:
- インスタンス生成試行を複数回行う
- 実際に存在するインスタンスは1つであることを表示
- 重複生成が拒否されるか、既存参照が返る様子をログに出す

教育ポイント:
- グローバルアクセス
- 唯一性
- 乱用注意

## 9.2 Factory Method
題材例:
- EnemyFactory が Slime / Goblin / Dragon を生成

表示要件:
- UIから生成対象選択
- Factory経由で生成
- 生成された型が異なる見た目で表示
- 生成ロジックの分岐をログで説明

教育ポイント:
- 生成処理の抽象化
- new の分離
- 拡張時の変更範囲縮小

## 9.3 Observer
題材例:
- HP変更通知で UI, Sound, Effect が更新

表示要件:
- Subject の値変更
- 複数 Observer へ通知
- 通知順または購読解除も確認可能
- UI/SE/Effect に見立てた要素が一斉更新

教育ポイント:
- 疎結合
- イベント通知
- 購読解除忘れの注意

## 9.4 Strategy
題材例:
- 敵AIの攻撃戦略切替
- 経路探索方法切替

表示要件:
- 現在戦略を UI から変更
- 同じ文脈で振る舞いが変化
- 戦略クラス差し替えを視覚的に表現

教育ポイント:
- if/switch の肥大化回避
- 振る舞いの差し替え
- OCPとの関係

## 9.5 State
題材例:
- PlayerState: Idle / Move / Attack / Dead

表示要件:
- 現在状態表示
- 入力やイベントで状態遷移
- 遷移元・遷移先がログに出る
- 現状態に応じた挙動差が見える

教育ポイント:
- 状態ごとの振る舞い分離
- 条件分岐の整理
- 状態遷移の見える化

## 9.6 Command
題材例:
- 移動、ジャンプ、攻撃をコマンド化
- Undo 可能な簡易操作列

表示要件:
- コマンド投入
- 実行キュー表示
- 実行履歴表示
- Undo で逆操作が見える

教育ポイント:
- リクエストのオブジェクト化
- Undo/Redo
- 入力と実行の分離

---

## 10. ディレクトリ構成
以下の構成を基本形として採用すること。

```text
Assets/
  Project/
    Scenes/
      Main.unity

    Scripts/
      Core/
        Bootstrap/
        Common/
        Logging/
        Navigation/

      Patterns/
        Shared/
          Interfaces/
          Base/
          Data/
          Visualization/

        Singleton/
          Domain/
          Application/
          Presentation/
          Data/

        FactoryMethod/
          Domain/
          Application/
          Presentation/
          Data/

        Observer/
          Domain/
          Application/
          Presentation/
          Data/

        Strategy/
          Domain/
          Application/
          Presentation/
          Data/

        State/
          Domain/
          Application/
          Presentation/
          Data/

        Command/
          Domain/
          Application/
          Presentation/
          Data/

      UI/
        Screens/
        Components/
        ViewModels/

    ScriptableObjects/
      PatternDefinitions/
      DemoScenarios/

    Prefabs/
      UI/
      Patterns/
      Common/

    Art/
      Icons/
      Sprites/

    Materials/
    Fonts/

  Docs/
    Architecture.md
    ExpansionGuide.md
```

---

## 11. クラス設計方針
### 11.1 共通基底
以下のような共通基底または抽象クラスを用意してよい。

- BasePatternDemo
- BaseDemoStep
- BasePatternVisualizer
- BaseScreenController

### 11.2 ログ
ログは Console だけでなく、画面上に表示する LogPanel へ流すこと。
ログ出力は以下を推奨。

- timestamp or step index
- actor
- action
- result

例:
- [Step 03] Subject -> Notify observers
- [Step 04] HPBarObserver -> Update HP to 70
- [Step 05] SoundObserver -> Play damaged sound

### 11.3 イベント
UnityEvent の乱用を避け、C# event または明確なイベント仲介クラスを用いる。
ただしUIボタンの入口のみ UnityEvent でもよい。

---

## 12. 実装ステップ
ClaudeCode は以下の順序で実装を進めること。

### Step 1: プロジェクト基盤構築
- Unity プロジェクト作成
- Main シーン作成
- 画面遷移の基本構築
- 共通UIルート作成
- ログパネル作成
- パターン定義 ScriptableObject 作成
- サンプルデータ投入

### Step 2: デモ共通基盤
- IPatternDemo
- BasePatternDemo
- DemoManager
- Step 実行機構
- AutoPlay 機構
- Reset 機構
- LogService
- MetadataProvider

### Step 3: 一覧画面と詳細画面
- パターン一覧UI
- カテゴリフィルタ
- 詳細表示UI
- デモ開始導線

### Step 4: MVP 6パターン実装
順番:
1. Singleton
2. Factory Method
3. Observer
4. Strategy
5. State
6. Command

### Step 5: 視覚化改善
- ハイライト
- ステップ説明文
- 関係図更新
- 色や強調演出の最小改善

### Step 6: ドキュメント整備
- README
- Architecture.md
- ExpansionGuide.md

---

## 13. 品質基準
以下を満たした場合に、MVP完成とみなす。

### 機能面
- 6パターンすべて起動できる
- デモ開始、停止、リセットが機能する
- 少なくとも4パターンでステップ実行が機能する
- すべてのデモでログが表示される
- Pattern List から各デモへ移動できる

### 設計面
- パターン追加時、既存UIの大規模改修が不要
- デモごとの責務が明確
- PatternDefinition がデータとして分離されている
- 主要クラスに最低限のコメントがある

### 利用面
- README を読めば起動できる
- 新しいパターン追加の指針がある
- 実装意図が Architecture.md に整理されている

---

## 14. 受け入れテスト項目
- アプリ起動時に一覧画面が開く
- 6パターンのカードが見える
- カード選択で詳細画面が開く
- 詳細画面からデモを開始できる
- 再生中にログが更新される
- リセットで初期状態へ戻る
- State デモで状態遷移が視覚的に分かる
- Observer デモで複数購読者の反応が分かる
- Command デモで履歴またはUndoが分かる
- Strategy デモで戦略切替の差が分かる
- Factory Method デモで生成物の違いが分かる
- Singleton デモで単一インスタンス性が分かる

---

## 15. 実装上の注意
- 最初から過剰なDIコンテナを入れない
- UniRx 等の外部依存は必須でない限り入れない
- 依存を増やすより、学習教材としての読みやすさを優先する
- Editor拡張は後回し
- まずはPC向けでよい
- 画像素材不足時は単色矩形とTextだけでも成立させる
- 視覚化は最低限でも「違いが分かる」ことを優先する

---

## 16. ClaudeCodeに依頼する実装単位
ClaudeCode は以下の単位でコミットまたはPR相当の粒度を意識して作業すること。

1. chore: project bootstrap and folder structure
2. feat: pattern definition data model and sample assets
3. feat: navigation and common screen framework
4. feat: demo execution framework and log panel
5. feat: singleton demo
6. feat: factory method demo
7. feat: observer demo
8. feat: strategy demo
9. feat: state demo
10. feat: command demo
11. docs: readme architecture and expansion guide
12. refactor: cleanup and naming consistency

---

## 17. 拡張方針
MVP完成後は、以下の順で拡張することを想定する。

次点候補:
- Decorator
- Adapter
- Template Method
- Builder
- Mediator
- Composite

最終的には GoF 23 パターンすべてに対応できるようにする。

---

## 18. ClaudeCodeへの最終指示
以下を満たすように実装を開始すること。

- まずは動くMVPを完成させる
- 設計の美しさより、教育効果と拡張性のバランスを取る
- 各パターンは「何が起きたか」が視覚的に分かることを最優先する
- 実装後、README と Docs を必ず整備する
- 不要な抽象化は避けつつ、追加しやすい共通基盤は先に作る
