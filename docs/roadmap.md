# GoF デザインパターン実装ロードマップ

## フェーズ概要

| フェーズ | 内容 | パターン数 |
|---------|------|-----------|
| Phase 0 | 基盤構築 | - |
| Phase 1 | 生成パターン | 5 |
| Phase 2 | 構造パターン | 7 |
| Phase 3 | 振る舞いパターン | 11 |
| Phase 4 | ノードグラフ可視化システム | - |
| Phase 5 | 統合・改善 | - |

---

## Phase 0: 基盤構築

### 目標
プロジェクトの基盤となる共通システムを構築する

### タスク

- [x] **0.1** プロジェクト構造のセットアップ
  - フォルダ構成の作成
  - Assembly Definitionの設定

- [x] **0.2** 共通UIシステム
  - InGameLogger（画面内ログ表示）
  - PatternInfoPanel（パターン説明パネル）
  - BackToMenuButton（メニュー戻りボタン）

- [x] **0.3** シーン遷移システム
  - SceneNavigator（シーン遷移ユーティリティ）

- [x] **0.4** 共通基盤クラス
  - PatternDemoBase（デモの基底クラス）
  - PatternCategory / LogEntry / LogColor

---

## Phase 1: 生成パターン (Creational)

### 1.1 Singleton
- [x] GameManager実装
- [x] 視覚化：インスタンス生成の試行と結果表示
- [x] デモ：複数箇所からのアクセス確認

### 1.2 Factory Method
- [x] 敵キャラクター工場の実装（Slime/Goblin/Dragon）
- [x] 視覚化：異なるファクトリからの生成アニメーション
- [x] デモ：スライム、ゴブリン、ドラゴンの生成

### 1.3 Abstract Factory
- [x] UIテーマ工場の実装（ダーク/ライト/レトロ）
- [x] 視覚化：テーマ切り替えによるUI変化
- [x] デモ：背景、ボタン、テキスト、アクセントの一括変更

### 1.4 Builder
- [x] キャラクタービルダーの実装（戦士/魔法使い/盗賊）
- [x] Director による構築手順の管理
- [x] デモ：全装備/最低装備の2パターン構築

### 1.5 Prototype
- [x] ユニット複製システムの実装（兵士/弓兵）
- [x] DeepCloneによる独立した複製
- [x] デモ：複製→変更→オリジナルとの比較

---

## Phase 2: 構造パターン (Structural)

### 2.1 Adapter
- [x] 旧式サウンドシステム → 新インターフェースへの変換
- [x] 視覚化：メソッド名・引数の変換過程をログ表示
- [x] デモ：BGM再生/停止をアダプター経由で操作

### 2.2 Bridge
- [x] 形状（円/四角）× 色（赤/青/緑）の描画システム
- [x] 視覚化：抽象と実装の分離を全組み合わせで表示
- [x] デモ：2形状 × 3色 = 6通りをサブクラスなしで実現

### 2.3 Composite
- [x] ファイルシステムのツリー構造（File/Folder）
- [x] 視覚化：ツリー構造の表示と再帰的サイズ計算
- [x] デモ：ファイル追加とツリー表示・サイズ計算

### 2.4 Decorator
- [x] 武器エンチャントシステム（剣/弓 + 炎/氷/毒）
- [x] 視覚化：デコレーター重ねがけで攻撃力変化を表示
- [x] デモ：エンチャントの動的追加

### 2.5 Facade
- [x] ゲーム初期化ファサード（Audio/Graphics/Input/Network）
- [x] 視覚化：サブシステム呼び出しフローをログ表示
- [x] デモ：フル初期化 / クイックスタート

### 2.6 Flyweight
- [x] 弾幕システム（通常弾/炎弾/氷弾）
- [x] 視覚化：弾インスタンス数 vs 弾タイプ数の比較
- [x] デモ：大量弾生成とメモリ共有統計

### 2.7 Proxy
- [x] 画像遅延ロードプロキシ
- [x] 視覚化：未ロード/ロード済みの状態変化
- [x] デモ：初回表示時のロードとキャッシュ確認

---

## Phase 3: 振る舞いパターン (Behavioral)

### 3.1 Observer
- [x] HP監視システム（HealthSystem + 3つのObserver）
- [x] 視覚化：HPバー、数値表示、警告の同期更新
- [x] デモ：ダメージ/回復で全オブザーバー一斉通知

### 3.2 Strategy
- [x] 敵AI戦略（攻撃的/防御的/逃走）
- [x] 視覚化：選択中の戦略と実行結果を表示
- [x] デモ：戦略の切り替えと実行

### 3.3 State
- [x] キャラクター状態マシン（待機/移動/攻撃/被ダメージ）
- [x] 視覚化：状態遷移をログ表示
- [x] デモ：入力による状態遷移

### 3.4 Command
- [x] Undo/Redoシステム（移動コマンド）
- [x] 視覚化：コマンド履歴のスタック表示
- [x] デモ：上下左右移動の取り消し/やり直し

### 3.5 Chain of Responsibility
- [x] ダメージ計算チェーン（回避→防御→耐性）
- [x] 視覚化：各ハンドラーの処理をフロー表示
- [x] デモ：攻撃力10/30/100のダメージ処理

### 3.6 Mediator
- [x] チャットルーム仲介者（Alice/Bob/Charlie）
- [x] 視覚化：仲介者経由のメッセージ配信
- [x] デモ：各ユーザーからのメッセージ送信

### 3.7 Memento
- [x] ゲーム状態のセーブ/ロード
- [x] 視覚化：状態スナップショットの保存/復元
- [x] デモ：レベルアップ/ダメージ/ゴールド→保存→復元

### 3.8 Iterator
- [x] インベントリイテレーター
- [x] 視覚化：現在位置のハイライト表示
- [x] デモ：アイテムの順次アクセスとリセット

### 3.9 Template Method
- [x] ターン制バトルテンプレート（戦士/魔法使い/ヒーラー）
- [x] 視覚化：共通処理フロー+オーバーライド部分
- [x] デモ：各キャラクターのターン実行

### 3.10 Visitor
- [x] スキルツリービジター（コスト計算/効果計算）
- [x] 視覚化：各ノード訪問時の計算結果
- [x] デモ：攻撃/防御/魔法スキルの統計

### 3.11 Interpreter
- [x] 簡易コマンド言語（MOVE/STATUS/REPEAT）
- [x] 視覚化：コマンド解釈と位置変化
- [x] デモ：「MOVE UP 3」「REPEAT 3 MOVE RIGHT 1」等の実行

---

## Phase 4: ノードグラフ可視化システム

パターンの構造をノード（矩形）とエッジ（線・矢印）で視覚的に表示するシステム。
スクリプトは実装済み。Unity Editor でのプレハブ作成・レイアウト変更が必要。

### 4.1 プレハブ作成（Unity Editor 作業）

以下を順番に作成する。詳細な仕様は `scene_specification.md` の「共通UIプレハブ」セクションを参照。

- [ ] **4.1.1** GraphNode.prefab を作成する
  - 配置場所: `Assets/_Common/Prefabs/GraphNode.prefab`
  - 空の GameObject を作成し、RectTransform + CanvasGroup を追加
  - `GraphNodeView` コンポーネントを追加
  - 子要素に Border (Image)、Background (Image)、NameLabel (TMP_Text)、StateLabel (TMP_Text) を作成
  - GraphNodeView の SerializeField を各子要素に接続
  - サイズ: 160 x 60、Pivot: (0.5, 0.5)

- [ ] **4.1.2** GraphEdge.prefab を作成する
  - 配置場所: `Assets/_Common/Prefabs/GraphEdge.prefab`
  - 空の GameObject を作成し、RectTransform を追加
  - `GraphEdgeView` コンポーネントを追加
  - 子要素に LineRenderer (空の GameObject + `UILineRenderer` コンポーネント)、Label (TMP_Text) を作成
  - GraphEdgeView の SerializeField を各子要素に接続
  - LineRenderer の Raycast Target を false に設定

- [ ] **4.1.3** NodeGraphView.prefab を作成する
  - 配置場所: `Assets/_Common/Prefabs/NodeGraphView.prefab`
  - 空の GameObject を作成し、RectTransform (Stretch-Stretch) を設定
  - `NodeGraphView` コンポーネントを追加
  - 子要素に GraphContainer (RectTransform: Stretch-Stretch, Padding 20px) を作成
  - NodeGraphView の SerializeField を接続:
    - `graphContainer` → GraphContainer
    - `nodePrefab` → GraphNode.prefab
    - `edgePrefab` → GraphEdge.prefab

### 4.2 DemoSceneCanvas レイアウト変更（Unity Editor 作業）

既存の DemoSceneCanvas.prefab を変更して、3カラムレイアウトにする。

- [ ] **4.2.1** ContentArea を3カラム構成にする
  - ContentArea に HorizontalLayoutGroup を設定（Child Force Expand Height = true）
  - ControlPanel: Layout Element → 幅 20%（Min Width: 250）
  - GraphPanel: Layout Element → Flexible Width: 1（幅 40%相当）
  - LogPanel: Layout Element → Flexible Width: 1（幅 40%相当）

- [ ] **4.2.2** GraphPanel を ContentArea に追加する
  - ControlPanel と LogPanel の **間** に GraphPanel (Image: `#1A1A2E`) を作成
  - GraphPanel の子に NodeGraphView.prefab をインスタンス化
  - GraphPanel を **非アクティブ** に設定（初期状態で非表示）

- [ ] **4.2.3** GraphPanel 非表示時のフォールバックを確認する
  - GraphPanel が非アクティブの場合、ControlPanel(40%) + LogPanel(60%) の2カラムになることを確認

### 4.3 各デモシーンの SerializeField 接続（Unity Editor 作業）

PatternDemoBase に `nodeGraphView` フィールドが追加されたため、グラフを使用するデモシーンで接続が必要。

- [ ] **4.3.1** まず1つのデモシーンで動作確認する（推奨: AbstractFactoryDemo）
  - AbstractFactoryDemo の `nodeGraphView` フィールドに NodeGraphView を接続
  - Play モードで正常にグラフが表示されることを確認
  - ノードの表示、エッジの描画、アニメーションが動作することを確認

- [ ] **4.3.2** 残りのデモシーンで順次接続する
  - 各デモの XXXDemo コンポーネントの `nodeGraphView` フィールドに NodeGraphView を接続
  - グラフ未使用のデモでは null のまま（接続不要）

### 4.4 動作確認チェックリスト

- [ ] GraphNode が正しく表示される（名前・状態テキスト・色）
- [ ] GraphEdge の線と矢印が正しく描画される
- [ ] 破線エッジが正しく描画される
- [ ] ノードパルスアニメーションが動作する
- [ ] エッジパルスアニメーションが動作する
- [ ] ノード生成アニメーション（スケール 0→1）が動作する
- [ ] ノード破棄アニメーション（フェードアウト）が動作する
- [ ] GraphPanel 非表示時に ControlPanel が全領域を使用する
- [ ] 既存のログ表示が正常に動作する（右パネル）

---

## Phase 5: 統合・改善

### 5.1 統合
- [ ] 全パターンのナビゲーション整備
- [ ] パターン間の比較機能
- [ ] 検索・フィルター機能

### 5.2 ドキュメント
- [ ] 各パターンの詳細説明追加
- [ ] コード解説の充実
- [ ] 使用例・応用例の追加

### 5.3 品質向上
- [ ] パフォーマンス最適化
- [ ] UIの一貫性確認
- [ ] バグ修正・改善

---

## 進捗トラッキング

### 完了パターン数
```
生成パターン:    [x] [x] [x] [x] [x]  (5/5)
構造パターン:    [x] [x] [x] [x] [x] [x] [x]  (7/7)
振る舞いパターン: [x] [x] [x] [x] [x] [x] [x] [x] [x] [x] [x]  (11/11)

総進捗: 23/23 パターン完了（スクリプト実装）
```

### マイルストーン

| マイルストーン | 目標 | 状態 |
|--------------|------|------|
| M1: 基盤完成 | Phase 0完了 | 完了 |
| M2: 生成パターン完了 | Phase 1完了 | 完了 |
| M3: 構造パターン完了 | Phase 2完了 | 完了 |
| M4: 振る舞いパターン完了 | Phase 3完了 | 完了 |
| M5: ノードグラフ可視化 | Phase 4完了 | 作業中（スクリプト実装済み、GUI作業待ち） |
| M6: プロジェクト完成 | Phase 5完了 | 未着手 |

---

## 優先度ガイド

### 学習効果が高い順（推奨実装順）

**初級（まず理解すべき）:**
1. Singleton - 最も基本的
2. Observer - Unityで頻出
3. Factory Method - 生成の基本
4. Strategy - 切り替えの基本
5. State - ゲームで必須

**中級（応用として）:**
6. Command - Undo実装に必須
7. Decorator - 柔軟な拡張
8. Composite - 階層構造
9. Adapter - 既存コード活用
10. Template Method - 共通処理の定義

**上級（理解を深める）:**
11. 残りのパターン
