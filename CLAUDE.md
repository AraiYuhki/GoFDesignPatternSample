# GoF デザインパターンサンプル

## プロジェクト名

GoFDesignPatternSample

## 説明

Gang of Four (GoF) の23種類のデザインパターンをUnityで視覚的にわかりやすく実装する学習用プロジェクト

## 技術スタック

- **Unity 6** (Universal Render Pipeline)
- **対象プラットフォーム**: エディタ実行（学習用途）
- **名前空間**: `DesignPatterns`, `DesignPatterns.Creational`, `DesignPatterns.Structural`, `DesignPatterns.Behavioral`
- **主要パッケージ**: Input System, TextMeshPro (uGUI), URP

## プロジェクト構成

```
Assets/
├── _Common/                    # 共通アセット・基盤クラス
├── Scenes/                     # メインメニュー・パターン選択
├── Creational/                 # 生成パターン (5種)
│   ├── AbstractFactory/
│   ├── Builder/
│   ├── FactoryMethod/
│   ├── Prototype/
│   └── Singleton/
├── Structural/                 # 構造パターン (7種)
│   ├── Adapter/
│   ├── Bridge/
│   ├── Composite/
│   ├── Decorator/
│   ├── Facade/
│   ├── Flyweight/
│   └── Proxy/
└── Behavioral/                 # 振る舞いパターン (11種)
    ├── ChainOfResponsibility/
    ├── Command/
    ├── Interpreter/
    ├── Iterator/
    ├── Mediator/
    ├── Memento/
    ├── Observer/
    ├── State/
    ├── Strategy/
    ├── TemplateMethod/
    └── Visitor/
```

各パターンフォルダ:
```
PatternName/
├── Scripts/                    # C#スクリプト
├── Prefabs/                    # プレハブ
├── Scenes/                     # デモシーン (PatternNameDemo.unity)
└── Materials/                  # マテリアル（必要に応じて）
```

## フォルダ構成と編集ルール

### 変更可能

- `Assets/_Common/Scripts/` — 共通基盤コード
- `Assets/Creational/`, `Assets/Structural/`, `Assets/Behavioral/` 配下の `Scripts/` — パターン実装コード
- `docs/` — ドキュメント

### 変更禁止

- `Assets/Settings/` — URP設定
- `ProjectSettings/` — プロジェクト設定ファイル
- `.prefab` / `.unity` ファイルの直接編集
- `*.meta` — Unityメタデータファイル（削除・編集厳禁）

### テスト

- `Assets/Tests/Editor/` — NUnit テスト
- テストファイル命名: `*Test.cs`
- `[TestFixture]` + `[Test]` パターン

## コーディング規約

### 命名規約
- クラス名: PascalCase
- メソッド名: PascalCase
- 変数名: camelCase
- 定数: PascalCase
- プライベートフィールド: camelCase
- アクセス修飾子は必ず明示（`private` も省略しない）
- 一般的な略称でない場合、名前の省略禁止
- region 使用禁止

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
- 学習用プロジェクトのため、パターンの意図を説明するコメントを積極的に記載する

コメントのフォーマットは以下のようにすること

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
internal int MyMethod(int arg) { }
```
### partial クラス
- partial 使用禁止

## コードスタイル
- インデント: スペース4つ
- 改行コード: LF
- 文字コード: UTF-8
- 中括弧: K&Rスタイル（同じ行に開く）
- using文は整理し、不要なものは削除する

## Unity固有の規約
- MonoBehaviourを継承するクラスは、Awake/Start/Update等のライフサイクルメソッドの順序を統一する
- SerializeFieldを使用する際は、適切なアクセス修飾子を設定する
- MonoBehaviourを継承するクラスでは、コンストラクタを使用しない
- Destroy()ではなく、適切にライフサイクルを管理する

## パフォーマンス考慮事項
- Update()内での重い処理は避ける
- string連結は StringBuilder を使用する
- Find系メソッドの使用は禁止

## デザインパターン実装方針

### 教材としての品質
- 各パターンは独立したデモシーンで動作させる
- パターンの意図と構造が視覚的に理解できるようにする
- インタラクティブな操作でパターンの動作を確認できるようにする
- コードは教材として読みやすく整理する

### 視覚化ガイドライン
- 色分け: 生成=青、構造=緑、振る舞い=オレンジ
- アニメーション: オブジェクト生成や状態変化を視覚的に表現
- 画面内にコンソールログを表示し、パターンの動作を可視化する

### デバッグコード
- `Debug.Log` は画面内ログ表示に置き換える（共通UIシステム使用）

