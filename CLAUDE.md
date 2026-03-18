# Unityで学ぶGoFデザインパターン視覚化プロジェクト

## プロジェクト名

GoF Design Pattern Sample

## 説明

GoF（Gang of Four）の23デザインパターンをUnity上で視覚的にデモンストレーションする教材プロジェクト。
各パターンのドメインモデルをステップ実行し、2Dアニメーションで動作を可視化する。

## 技術スタック

- **Unity 6** (6000.0.58f2, Built-in Render Pipeline)
- **対象プラットフォーム**: PC / エディタ
- **名前空間**: `GoFPatterns`（Core / Patterns / UI の3層）
- **エントリーポイント**: `AppBootstrap.cs`

## 主要ライブラリ

| ライブラリ | 用途 | 備考 |
|---|---|---|
| TextMeshPro | テキスト表示 | |
| Unity Coroutine | アニメーション・ステップ実行 | TweenUtility で薄くラップ |

## アセンブリ構成

| アセンブリ | パス | 依存 |
|---|---|---|
| `GoFPatterns.Core` | `Assets/Project/Scripts/Core/` | TextMeshPro |
| `GoFPatterns.Patterns` | `Assets/Project/Scripts/Patterns/` | Core |
| `GoFPatterns.UI` | `Assets/Project/Scripts/UI/` | Core, Patterns, TextMeshPro |

依存方向は Core ← Patterns ← UI の一方向のみ。逆方向参照禁止。

## フォルダ構成と編集ルール

### 変更可能

- `Assets/Project/Scripts/` — 全スクリプト（新規作成・修正 OK）
- `Assets/Project/ScriptableObjects/` — PatternDefinition アセット
- `docs/` — ロードマップ・実装計画書

### 変更禁止

- `Assets/Scripts/` — 旧実装（参照のみ、移植時はリファクタして再利用）
- `Assets/Plugins/` — 外部ライブラリ
- `ProjectSettings/` — プロジェクト設定ファイル
- `.prefab` / `.unity` ファイルの直接編集（Unity Editor で行う）
- `*.meta` — Unityメタデータファイル（削除・編集厳禁）

### テスト

- `Assets/Project/Tests/Editor/` — NUnit テスト
- テストファイル命名: `*Test.cs`
- `[TestFixture]` + `[Test]` パターン

## アーキテクチャ

### Core 層（`GoFPatterns.Core`）
- `PatternDefinition` ScriptableObject、`PatternRepository`
- `ScreenManager`（1シーン内パネル切り替え）、`BaseScreen`
- `LogService`（ステップログ蓄積・通知）
- `AppBootstrap`（起動時に PatternListScreen を表示）

### Patterns 層（`GoFPatterns.Patterns`）
- `IPatternDemo` / `BasePatternDemo` — デモ実行の共通基盤
- `DemoScenario` / `DemoStep` — ステップ列管理
- `DemoManager` — デモのライフサイクル統括（singleton MonoBehaviour）
- `VisualizationRenderer` — RenderTexture + 専用カメラで2D空間をCanvasに投影
- `VisualElement` / `VisualArrow` — 2D図形要素・矢印
- `TweenUtility` — コルーチンベースのアニメーションユーティリティ
- 各パターンの Domain / Visualization 実装

### UI 層（`GoFPatterns.UI`）
- `PatternListScreen` — パターンカード一覧
- `PatternDetailScreen` — パターン詳細・デモ開始
- `DemoScreen` — 再生制御・ログ表示・ビジュアライゼーション領域

## コーディング規約

### 非同期処理
- アニメーションや時間経過を伴う処理は **Coroutine（IEnumerator）** を使用する
- コルーチンの命名は `動詞 + Coroutine` サフィックスを付ける（例: `PulseCoroutine`）
- 外部公開する場合は `StartCoroutine()` でラップして `Coroutine` を返す

### 命名規約
- クラス名: PascalCase
- メソッド名: PascalCase
- 変数名: camelCase
- 定数: PascalCase
- プライベートフィールド: camelCase
- アクセス修飾子は必ず明示（`private` も省略しない）
- 一般的な略称でない場合、名前の省略禁止

### var の使用
- 型が明確な場合のみ `var` 使用可
- 型が推論できない場合は明示的に型を書く

### 定数
- マジックナンバー禁止 → `const` または `static readonly` を使用

### 制御構文
- `if` / `for` / `foreach` を1行で書かない
- `switch` の `case` が複雑な場合はメソッドに切り出す

### LINQ
- 可能なら `for` / `foreach` を優先
- 遅延実行を避けるため終端操作（`ToList()`, `First()` 等）を必ず付ける

### コメント (XML ドキュメント)
- クラス・インターフェイス・メソッド・フィールド・プロパティに XML コメント必須
- コメント末尾に句読点・疑問符は付けない

### partial クラス
- 新規の partial 使用禁止（自動生成コードを除く）

```csharp
/// <summary>説明</summary>
private int value;

/// <summary>
/// 説明
/// </summary>
internal sealed class MyClass { }

/// <summary>説明</summary>
/// <param name="arg">説明</param>
/// <returns>説明</returns>
public IEnumerator FadeOutCoroutine(float duration) { }
```

## コードスタイル
- インデント: スペース4つ
- 改行コード: LF
- 文字コード: UTF-8
- 中括弧: K&Rスタイル（同じ行に開く）
- using文は整理し、不要なものは削除する

## Unity固有の規約
- MonoBehaviourを継承するクラスは、Awake/Start/Update等のライフサイクルメソッドの順序を統一する
- `[SerializeField]` を使用する際は `private` を明示する
- MonoBehaviourを継承するクラスでは、コンストラクタを使用しない
- シングルトンには `DontDestroyOnLoad` を使用しない（1シーン構成のため不要）

## パフォーマンス考慮事項
- Update()内での重い処理は避ける
- string連結は StringBuilder を使用する
- Find系メソッドの頻繁な使用を避ける
- `Shader.Find()` は初期化時のみ使用する（毎フレーム呼び出し禁止）
