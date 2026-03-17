# GoF デザインパターン視覚化プロジェクト

Unity で GoF (Gang of Four) デザインパターンを「動く・見える・比較できる」形で学べる教育用アプリケーション

## 概要

- パターンの責務と意図が視覚的に理解できる
- ステップ実行・自動再生でパターンの動作を確認できる
- 実行ログとビジュアライゼーションで挙動を把握できる
- 1シーン内のパネル切り替えで複数パターンを横断的に学習できる

## 動作環境

- **Unity**: 6000.0 以降 (URP)
- **プラットフォーム**: エディタ実行（PC）

## セットアップ

1. リポジトリをクローンする
2. Unity Hub でプロジェクトを開く
3. `Assets/Project/Scenes/Main.unity` を開く
4. Play ボタンで実行

## MVP対象パターン（6種）

| カテゴリ | パターン | 題材 |
|---------|---------|------|
| 生成 | Singleton | GameManager の唯一インスタンス |
| 生成 | Factory Method | 敵キャラクター生成工場 |
| 振る舞い | Observer | HP変化通知システム |
| 振る舞い | Strategy | 敵AI戦略切り替え |
| 振る舞い | State | キャラクター状態マシン |
| 振る舞い | Command | Undo/Redo 移動操作 |

## ディレクトリ構成

```
Assets/Project/
  Scripts/
    Core/          ... 基盤システム（ログ・画面遷移・起動処理）
    Patterns/
      Shared/      ... 共通インターフェース・基底クラス
      Singleton/   ... 各パターン実装
      ...
    UI/            ... 画面・UIコンポーネント
  ScriptableObjects/ ... パターン定義データ
  Prefabs/         ... UIプレハブ
  Scenes/          ... シーン
```

## パターン追加方法

TODO: Docs/ExpansionGuide.md に詳細を記載予定

## 既知の制限

- MVP版は6パターンのみ対応
- モバイル向け最適化は未実施
