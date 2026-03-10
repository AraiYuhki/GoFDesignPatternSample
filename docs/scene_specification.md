# デモシーン仕様書

## 目次

1. [共通レイアウト仕様](#共通レイアウト仕様)
2. [共通UIプレハブ](#共通uiプレハブ)
3. [メインメニューシーン](#メインメニューシーン)
4. [生成パターン（5シーン）](#生成パターン5シーン)
5. [構造パターン（7シーン）](#構造パターン7シーン)
6. [振る舞いパターン（11シーン）](#振る舞いパターン11シーン)
7. [Build Settingsのシーン登録順](#build-settingsのシーン登録順)

---

## 共通レイアウト仕様

### 解像度と基準
- **Canvas基準解像度**: 1920 x 1080
- **Canvas Scaler**: Scale With Screen Size（Match = 0.5）
- **UI Scale Mode**: Constant Pixel Size は使用しない

### 画面3分割レイアウト

全デモシーンは以下の共通レイアウトを使用する。

```
┌──────────────────────────────────────────────────────────┐
│ ■ ヘッダー（上部固定 / 高さ 100px）                        │
│  [← 戻る]  パターン名  カテゴリ                             │
├──────────┬─────────────────────┬─────────────────────────┤
│ 操作パネル │  グラフパネル（中央）  │  ログパネル（右）         │
│ 幅: 20%  │  幅: 40%            │  幅: 40%                │
│          │                     │                         │
│ ・ボタン群 │  ・NodeGraphView     │  ・InGameLogger          │
│ ・固有UI  │  ・パターン構造の     │  ・スクロール可能          │
│          │    ノード＆エッジ表示  │                         │
│          │  ※未使用時は非表示    │                         │
│          │  → 操作+ログの2列に   │                         │
├──────────┴─────────────────────┴─────────────────────────┤
│ ■ フッター（下部固定 / 高さ 60px）                          │
│  パターンの説明文                                           │
└──────────────────────────────────────────────────────────┘
```

### カラーテーマ

| 要素 | 色コード | 用途 |
|-----|---------|------|
| 背景（共通） | `#1E1E2E` | シーン全体の背景 |
| ヘッダー背景 | `#2D2D44` | ヘッダーバー |
| 操作パネル背景 | `#252538` | 左側パネル |
| グラフパネル背景 | `#1A1A2E` | 中央グラフエリア |
| ログパネル背景 | `#1A1A2E` | 右側ログエリア |
| 生成パターン アクセント | `#5B9BD5`（青） | ヘッダーライン、ボタン |
| 構造パターン アクセント | `#70AD47`（緑） | ヘッダーライン、ボタン |
| 振る舞いパターン アクセント | `#ED7D31`（オレンジ） | ヘッダーライン、ボタン |
| ボタン通常 | `#3A3A5C` | ボタン背景 |
| ボタンホバー | `#4A4A6C` | ホバー時 |
| ボタン押下 | `#2A2A4C` | クリック時 |
| テキスト通常 | `#E0E0E0` | 通常テキスト |
| テキスト補助 | `#A0A0B0` | 説明文テキスト |

### フォント設定
- **フォント**: TextMeshPro デフォルト（日本語フォールバック設定必要）
- **ヘッダー パターン名**: サイズ 36、Bold
- **ヘッダー カテゴリ**: サイズ 20、Regular
- **ボタンテキスト**: サイズ 22、Regular
- **ログテキスト**: サイズ 18、Regular、等幅推奨
- **説明文テキスト**: サイズ 20、Regular

---

## 共通UIプレハブ

### 1. DemoSceneCanvas（プレハブ）

各デモシーンで使用するCanvas全体のプレハブ。

**配置場所**: `Assets/_Common/Prefabs/DemoSceneCanvas.prefab`

**階層構造**:
```
DemoSceneCanvas                     [Canvas + CanvasScaler + GraphicRaycaster]
├── Header                          [Image: #2D2D44]
│   ├── AccentLine                  [Image: カテゴリ色、高さ4px、上端]
│   ├── BackButton                  [Button + BackToMenuButton]
│   │   └── Text (TMP)             "← 戻る"
│   ├── PatternNameText (TMP)       パターン名
│   └── CategoryText (TMP)          カテゴリ名
├── ContentArea                     [HorizontalLayoutGroup]
│   ├── ControlPanel                [Image: #252538, VerticalLayoutGroup, 幅: 20%]
│   │   └── ButtonContainer         [VerticalLayoutGroup, Spacing: 12]
│   │       └── (各シーン固有のボタンを配置)
│   ├── GraphPanel                  [Image: #1A1A2E, 幅: 40%]
│   │   └── NodeGraphView           [NodeGraphView.prefab のインスタンス]
│   └── LogPanel                    [Image: #1A1A2E, 幅: 40%]
│       └── LogScrollView           [ScrollRect]
│           └── Viewport
│               └── Content
│                   └── LogText (TMP)  [InGameLoggerのlogTextに接続]
├── Footer                          [Image: #2D2D44]
│   └── DescriptionText (TMP)       パターンの説明文
└── InGameLogger                    [InGameLoggerコンポーネント]
```

**コンポーネント接続**:
- `InGameLogger.logText` → `LogText (TMP)`
- `InGameLogger.maxLines` → `30`

**ContentArea 設定**:
- HorizontalLayoutGroup: Child Force Expand Height = true
- ControlPanel: Layout Element → Preferred Width: 20%（Min Width: 250）
- GraphPanel: Layout Element → Flexible Width: 1
- LogPanel: Layout Element → Flexible Width: 1

**GraphPanel 初期状態**:
- GraphPanel は初期状態で **非アクティブ** にしておくこと
- NodeGraphView.Initialize() が呼ばれた際に自動で有効化される
- グラフ未使用のデモシーンでは、GraphPanel が非表示になり ControlPanel + LogPanel の2カラム構成になる
  - この場合 ControlPanel: 40%、LogPanel: 60% の比率で表示される

### 2. PatternInfoPanel（プレハブ）

**配置場所**: `Assets/_Common/Prefabs/PatternInfoPanel.prefab`

Headerの子要素として配置。PatternDemoBaseの `infoPanel` に接続する。

**コンポーネント接続**:
- `PatternInfoPanel.patternNameText` → `PatternNameText (TMP)`
- `PatternInfoPanel.categoryText` → `CategoryText (TMP)`
- `PatternInfoPanel.descriptionText` → `DescriptionText (TMP)`（Footer内）

### 3. DemoButton（プレハブ）

**配置場所**: `Assets/_Common/Prefabs/DemoButton.prefab`

各操作ボタンのテンプレート。

**構造**:
```
DemoButton                          [Button + Image: #3A3A5C, 角丸8px]
└── Text (TMP)                      ボタンラベル、サイズ22
```

**サイズ**: 幅 Stretch（親に追従）、高さ 50px
**Transition**: Color Tint（Normal→Hover→Pressed）

### 4. NodeGraphView（プレハブ）

**配置場所**: `Assets/_Common/Prefabs/NodeGraphView.prefab`

パターンの構造をノードとエッジで可視化するグラフビューのプレハブ。
ControlPanelの代わりに左側パネルに配置する。

**階層構造**:
```
NodeGraphView                       [RectTransform + NodeGraphView コンポーネント]
└── GraphContainer                  [RectTransform: Stretch全方向、Padding 20px]
    └── (実行時にノード・エッジが動的生成される)
```

**コンポーネント設定（NodeGraphView）**:
| フィールド | 接続先 | 値 |
|-----------|-------|-----|
| `graphContainer` | GraphContainer | - |
| `nodePrefab` | GraphNode.prefab | - |
| `edgePrefab` | GraphEdge.prefab | - |
| `defaultNodeColor` | - | `#808080` (グレー) |
| `activeNodeColor` | - | `#5B9BD5` (青) |
| `defaultEdgeColor` | - | `#999999` (ライトグレー) |

**RectTransform設定**:
- Anchor: Stretch-Stretch
- Left/Right/Top/Bottom: 0

### 5. GraphNode（プレハブ）

**配置場所**: `Assets/_Common/Prefabs/GraphNode.prefab`

グラフ上のノード（クラスやオブジェクト）1つを表す矩形UI。

**階層構造**:
```
GraphNode                           [RectTransform + CanvasGroup + GraphNodeView コンポーネント]
├── Border                          [Image: #808080, Outline風]
├── Background                      [Image: #333333E6, 角丸なし]
├── NameLabel (TMP)                 クラス名 / オブジェクト名
└── StateLabel (TMP)                状態テキスト（例: "HP: 100"）
```

**サイズ**: 幅 160px、高さ 60px（Pivot: 0.5, 0.5）

**コンポーネント設定（GraphNodeView）**:
| フィールド | 接続先 |
|-----------|-------|
| `backgroundImage` | Background |
| `borderImage` | Border |
| `nameLabel` | NameLabel (TMP) |
| `stateLabel` | StateLabel (TMP) |

**NameLabel設定**:
- Font Size: 16、Bold
- Alignment: Center-Middle
- Color: `#E0E0E0`
- RectTransform: 上半分（高さの60%）、左右Padding 8px

**StateLabel設定**:
- Font Size: 12、Regular
- Alignment: Center-Middle
- Color: `#A0A0B0`
- RectTransform: 下半分（高さの40%）、左右Padding 8px
- 初期状態: 非アクティブ（空文字の場合スクリプト側で非表示にする）

**Border実装方法（2通りから選択）**:

方法A: Outline コンポーネント使用
- Background に `Outline` コンポーネントを追加
- Effect Distance: (2, 2)
- Effect Color: `#808080`
- この場合 Border の Image は不要

方法B: 2重Image方式
- Border: Image `#808080`、サイズ = GraphNode と同じ（Stretch）
- Background: Image `#333333E6`、上下左右に 2px のマージン
- Border を Background より後ろ（Hierarchy上で先）に配置

### 6. GraphEdge（プレハブ）

**配置場所**: `Assets/_Common/Prefabs/GraphEdge.prefab`

ノード間の関係（依存・通知・継承など）を線と矢印で表す。

**階層構造**:
```
GraphEdge                           [RectTransform + GraphEdgeView コンポーネント]
├── LineRenderer                    [UILineRenderer コンポーネント (カスタムGraphic)]
└── Label (TMP)                     エッジラベル（例: "notify"）
```

**コンポーネント設定（GraphEdgeView）**:
| フィールド | 接続先 |
|-----------|-------|
| `lineRenderer` | LineRenderer の UILineRenderer コンポーネント |
| `labelText` | Label (TMP) |

**RectTransform設定**:
- GraphEdge: Stretch-Stretch（GraphContainerと同じ領域）
- GraphEdge の RectTransform は親（GraphContainer）にフィットさせる

**LineRenderer (UILineRenderer) 設定**:
- Raycast Target: **false**（クリック判定不要）
- Color: `#999999`

**Label設定**:
- Font Size: 11、Regular
- Alignment: Center-Middle
- Color: `#A0A0B0`
- 初期状態: 非アクティブ

### 7. EventSystem

**各シーンに必須**: EventSystem + InputSystemUIInputModule

---

## メインメニューシーン

**シーン名**: `MainMenu`
**配置場所**: `Assets/Scenes/MainMenu.unity`

### 画面レイアウト

```
┌─────────────────────────────────────────────────┐
│                                                 │
│        GoF デザインパターンサンプル                 │
│        〜 Unity で学ぶ 23 のパターン 〜            │
│                                                 │
├─────────────────────────────────────────────────┤
│                                                 │
│  ■ 生成パターン (Creational)        [青アクセント]  │
│  ┌──────┐┌──────┐┌──────┐┌──────┐┌──────┐       │
│  │Single││Facto ││Abstr ││Build ││Proto │       │
│  │ton   ││ryMeth││actFac││er    ││type  │       │
│  └──────┘└──────┘└──────┘└──────┘└──────┘       │
│                                                 │
│  ■ 構造パターン (Structural)        [緑アクセント]  │
│  ┌──────┐┌──────┐┌──────┐┌──────┐               │
│  │Adapte││Bridge││Compos││Decora│               │
│  │r     ││      ││ite   ││tor   │               │
│  └──────┘└──────┘└──────┘└──────┘               │
│  ┌──────┐┌──────┐┌──────┐                       │
│  │Facade││Flywei││Proxy │                       │
│  │      ││ght   ││      │                       │
│  └──────┘└──────┘└──────┘                       │
│                                                 │
│  ■ 振る舞いパターン (Behavioral)    [橙アクセント]  │
│  ┌──────┐┌──────┐┌──────┐┌──────┐               │
│  │Observ││Strate││State ││Comman│               │
│  │er    ││gy    ││      ││d     │               │
│  └──────┘└──────┘└──────┘└──────┘               │
│  ┌──────┐┌──────┐┌──────┐┌──────┐               │
│  │Chain ││Media ││Mement││Iterat│               │
│  │OfResp││tor   ││o     ││or    │               │
│  └──────┘└──────┘└──────┘└──────┘               │
│  ┌──────┐┌──────┐┌──────┐                       │
│  │Templa││Visito││Inter │                       │
│  │teMeth││r     ││preter│                       │
│  └──────┘└──────┘└──────┘                       │
│                                                 │
└─────────────────────────────────────────────────┘
```

### Hierarchy 構成

```
MainMenu
├── Main Camera
├── EventSystem
└── Canvas
    ├── Background                  [Image: #1E1E2E, Stretch]
    ├── TitleArea
    │   ├── TitleText (TMP)         "GoF デザインパターンサンプル" サイズ48 Bold
    │   └── SubtitleText (TMP)      "〜 Unity で学ぶ 23 のパターン 〜" サイズ24
    ├── ScrollView                  [ScrollRect, Vertical]
    │   └── Viewport
    │       └── Content             [VerticalLayoutGroup]
    │           ├── CreationalSection
    │           │   ├── SectionHeader   "■ 生成パターン (Creational)" [青ライン]
    │           │   └── ButtonGrid      [GridLayoutGroup: 5列]
    │           │       ├── SingletonButton      → "SingletonDemo" シーン
    │           │       ├── FactoryMethodButton   → "FactoryMethodDemo" シーン
    │           │       ├── AbstractFactoryButton → "AbstractFactoryDemo" シーン
    │           │       ├── BuilderButton         → "BuilderDemo" シーン
    │           │       └── PrototypeButton       → "PrototypeDemo" シーン
    │           ├── StructuralSection
    │           │   ├── SectionHeader   "■ 構造パターン (Structural)" [緑ライン]
    │           │   └── ButtonGrid      [GridLayoutGroup: 4列]
    │           │       ├── AdapterButton         → "AdapterDemo" シーン
    │           │       ├── BridgeButton          → "BridgeDemo" シーン
    │           │       ├── CompositeButton       → "CompositeDemo" シーン
    │           │       ├── DecoratorButton       → "DecoratorDemo" シーン
    │           │       ├── FacadeButton          → "FacadeDemo" シーン
    │           │       ├── FlyweightButton       → "FlyweightDemo" シーン
    │           │       └── ProxyButton           → "ProxyDemo" シーン
    │           └── BehavioralSection
    │               ├── SectionHeader   "■ 振る舞いパターン (Behavioral)" [橙ライン]
    │               └── ButtonGrid      [GridLayoutGroup: 4列]
    │                   ├── ObserverButton            → "ObserverDemo" シーン
    │                   ├── StrategyButton             → "StrategyDemo" シーン
    │                   ├── StateButton                → "StateDemo" シーン
    │                   ├── CommandButton               → "CommandDemo" シーン
    │                   ├── ChainOfResponsibilityButton → "ChainOfResponsibilityDemo" シーン
    │                   ├── MediatorButton              → "MediatorDemo" シーン
    │                   ├── MementoButton               → "MementoDemo" シーン
    │                   ├── IteratorButton              → "IteratorDemo" シーン
    │                   ├── TemplateMethodButton        → "TemplateMethodDemo" シーン
    │                   ├── VisitorButton               → "VisitorDemo" シーン
    │                   └── InterpreterButton           → "InterpreterDemo" シーン
    └── VersionText (TMP)           右下小さく "v1.0"
```

### パターン選択ボタンの仕様

| 属性 | 値 |
|------|-----|
| サイズ | 200 x 80 px |
| 背景色 | カテゴリ色の暗い版（20%透明度） |
| ボーダー | カテゴリ色、2px |
| テキスト上段 | パターン名（英語）サイズ 22 Bold |
| テキスト下段 | パターン名（日本語）サイズ 14 |
| クリック | `SceneNavigator.LoadScene("XXXDemo")` |

---

## 全デモシーン共通の SerializeField 追加

NodeGraphView の導入に伴い、全デモシーンの Demo コンポーネント（PatternDemoBase 派生クラス）に以下のフィールドが追加されている。

| フィールド | 接続先 | 備考 |
|-----------|-------|------|
| `nodeGraphView` | DemoSceneCanvas > ContentArea > GraphPanel > NodeGraphView | グラフ未使用のデモでは null のまま |

各シーンごとの接続作業は以下のとおり:
1. DemoSceneCanvas プレハブ内の NodeGraphView を、シーン上の Demo コンポーネントの `nodeGraphView` フィールドにドラッグ接続する
2. グラフを使用しないデモでは接続不要（null のままで問題なし）

---

## 生成パターン（5シーン）

### 1. SingletonDemo

**シーン名**: `SingletonDemo`
**配置場所**: `Assets/Creational/Singleton/Scenes/SingletonDemo.unity`

#### Hierarchy

```
SingletonDemo
├── Main Camera
├── EventSystem
├── GameManager                     [GameManager コンポーネント]
├── DemoSceneCanvas (プレハブインスタンス)
│   └── ContentArea
│       └── ControlPanel
│           └── ButtonContainer
│               ├── AddScoreButton          "スコア +10"
│               ├── ResetScoreButton        "スコアリセット"
│               ├── DuplicateTestButton     "重複生成テスト"
│               └── CheckInstanceButton     "インスタンス確認"
└── SingletonDemo                   [SingletonDemo コンポーネント]
```

#### SerializeField接続

| フィールド | 接続先 |
|-----------|-------|
| `infoPanel` | DemoSceneCanvas > PatternInfoPanel |
| `gameManagerPrefab` | null（コード内でAddComponent） |
| `addScoreButton` | AddScoreButton |
| `resetScoreButton` | ResetScoreButton |
| `duplicateTestButton` | DuplicateTestButton |
| `checkInstanceButton` | CheckInstanceButton |

---

### 2. FactoryMethodDemo

**シーン名**: `FactoryMethodDemo`
**配置場所**: `Assets/Creational/FactoryMethod/Scenes/FactoryMethodDemo.unity`

#### Hierarchy

```
FactoryMethodDemo
├── Main Camera
├── EventSystem
├── DemoSceneCanvas
│   └── ControlPanel > ButtonContainer
│       ├── CreateSlimeButton       "スライム生成"
│       ├── CreateGoblinButton      "ゴブリン生成"
│       ├── CreateDragonButton      "ドラゴン生成"
│       └── AttackButton            "攻撃！"
└── FactoryMethodDemo               [FactoryMethodDemo コンポーネント]
```

#### SerializeField接続

| フィールド | 接続先 |
|-----------|-------|
| `infoPanel` | PatternInfoPanel |
| `createSlimeButton` | CreateSlimeButton |
| `createGoblinButton` | CreateGoblinButton |
| `createDragonButton` | CreateDragonButton |
| `attackButton` | AttackButton |

---

### 3. AbstractFactoryDemo

**シーン名**: `AbstractFactoryDemo`
**配置場所**: `Assets/Creational/AbstractFactory/Scenes/AbstractFactoryDemo.unity`

#### Hierarchy

```
AbstractFactoryDemo
├── Main Camera
├── EventSystem
├── DemoSceneCanvas
│   └── ControlPanel > ButtonContainer
│       ├── DarkThemeButton         "ダークテーマ"
│       ├── LightThemeButton        "ライトテーマ"
│       ├── RetroThemeButton        "レトロテーマ"
│       ├── Spacer
│       └── ThemePreviewArea        [Image: テーマプレビュー用]
│           ├── SamplePanel         [Image: 背景プレビュー]
│           ├── SampleButton1       [Image: ボタンプレビュー]
│           ├── SampleButton2       [Image: ボタンプレビュー]
│           └── SampleButton3       [Image: ボタンプレビュー]
└── AbstractFactoryDemo             [AbstractFactoryDemo コンポーネント]
```

#### SerializeField接続

| フィールド | 接続先 |
|-----------|-------|
| `infoPanel` | PatternInfoPanel |
| `samplePanel` | SamplePanel |
| `sampleButtons` | [SampleButton1, SampleButton2, SampleButton3] |
| `darkThemeButton` | DarkThemeButton |
| `lightThemeButton` | LightThemeButton |
| `retroThemeButton` | RetroThemeButton |

---

### 4. BuilderDemo

**シーン名**: `BuilderDemo`
**配置場所**: `Assets/Creational/Builder/Scenes/BuilderDemo.unity`

#### Hierarchy

```
BuilderDemo
├── Main Camera
├── EventSystem
├── DemoSceneCanvas
│   └── ControlPanel > ButtonContainer
│       ├── SectionLabel (TMP)      "全装備構築"
│       ├── BuildWarriorFullButton  "戦士（全装備）"
│       ├── BuildMageFullButton     "魔法使い（全装備）"
│       ├── BuildThiefFullButton    "盗賊（全装備）"
│       ├── Spacer
│       ├── SectionLabel2 (TMP)     "最低装備構築"
│       └── BuildWarriorMinButton   "戦士（最低装備）"
└── BuilderDemo                     [BuilderDemo コンポーネント]
```

#### SerializeField接続

| フィールド | 接続先 |
|-----------|-------|
| `infoPanel` | PatternInfoPanel |
| `buildWarriorFullButton` | BuildWarriorFullButton |
| `buildMageFullButton` | BuildMageFullButton |
| `buildThiefFullButton` | BuildThiefFullButton |
| `buildWarriorMinButton` | BuildWarriorMinButton |

---

### 5. PrototypeDemo

**シーン名**: `PrototypeDemo`
**配置場所**: `Assets/Creational/Prototype/Scenes/PrototypeDemo.unity`

#### Hierarchy

```
PrototypeDemo
├── Main Camera
├── EventSystem
├── DemoSceneCanvas
│   └── ControlPanel > ButtonContainer
│       ├── CloneSoldierButton      "兵士を複製"
│       ├── CloneArcherButton       "弓兵を複製"
│       ├── Spacer
│       ├── ModifyCloneButton       "複製を強化"
│       └── CompareButton           "オリジナルと比較"
└── PrototypeDemo                   [PrototypeDemo コンポーネント]
```

#### SerializeField接続

| フィールド | 接続先 |
|-----------|-------|
| `infoPanel` | PatternInfoPanel |
| `cloneSoldierButton` | CloneSoldierButton |
| `cloneArcherButton` | CloneArcherButton |
| `modifyCloneButton` | ModifyCloneButton |
| `compareButton` | CompareButton |

---

## 構造パターン（7シーン）

### 6. AdapterDemo

**シーン名**: `AdapterDemo`
**配置場所**: `Assets/Structural/Adapter/Scenes/AdapterDemo.unity`

#### Hierarchy

```
AdapterDemo
├── Main Camera
├── EventSystem
├── DemoSceneCanvas
│   └── ControlPanel > ButtonContainer
│       ├── PlayBattleButton        "バトルBGM再生"
│       ├── PlayFieldButton         "フィールドBGM再生"
│       ├── StopButton              "停止"
│       └── StatusButton            "状態確認"
└── AdapterDemo                     [AdapterDemo コンポーネント]
```

#### SerializeField接続

| フィールド | 接続先 |
|-----------|-------|
| `infoPanel` | PatternInfoPanel |
| `playBattleButton` | PlayBattleButton |
| `playFieldButton` | PlayFieldButton |
| `stopButton` | StopButton |
| `statusButton` | StatusButton |

---

### 7. BridgeDemo

**シーン名**: `BridgeDemo`
**配置場所**: `Assets/Structural/Bridge/Scenes/BridgeDemo.unity`

#### Hierarchy

```
BridgeDemo
├── Main Camera
├── EventSystem
├── DemoSceneCanvas
│   └── ControlPanel > ButtonContainer
│       ├── RedCircleButton         "赤い円"
│       ├── BlueRectButton          "青い四角"
│       ├── GreenCircleButton       "緑の円"
│       └── ShowAllButton           "全組み合わせ表示"
└── BridgeDemo                      [BridgeDemo コンポーネント]
```

#### SerializeField接続

| フィールド | 接続先 |
|-----------|-------|
| `infoPanel` | PatternInfoPanel |
| `redCircleButton` | RedCircleButton |
| `blueRectButton` | BlueRectButton |
| `greenCircleButton` | GreenCircleButton |
| `showAllButton` | ShowAllButton |

---

### 8. CompositeDemo

**シーン名**: `CompositeDemo`
**配置場所**: `Assets/Structural/Composite/Scenes/CompositeDemo.unity`

#### Hierarchy

```
CompositeDemo
├── Main Camera
├── EventSystem
├── DemoSceneCanvas
│   └── ControlPanel > ButtonContainer
│       ├── ShowTreeButton          "ツリー表示"
│       ├── CalcSizeButton          "サイズ計算"
│       └── AddFileButton           "ファイル追加"
└── CompositeDemo                   [CompositeDemo コンポーネント]
```

#### SerializeField接続

| フィールド | 接続先 |
|-----------|-------|
| `infoPanel` | PatternInfoPanel |
| `showTreeButton` | ShowTreeButton |
| `calcSizeButton` | CalcSizeButton |
| `addFileButton` | AddFileButton |

---

### 9. DecoratorDemo

**シーン名**: `DecoratorDemo`
**配置場所**: `Assets/Structural/Decorator/Scenes/DecoratorDemo.unity`

#### Hierarchy

```
DecoratorDemo
├── Main Camera
├── EventSystem
├── DemoSceneCanvas
│   └── ControlPanel > ButtonContainer
│       ├── SectionLabel (TMP)      "武器を選択"
│       ├── CreateSwordButton       "剣を作成"
│       ├── CreateBowButton         "弓を作成"
│       ├── Spacer
│       ├── SectionLabel2 (TMP)     "エンチャント"
│       ├── AddFireButton           "🔥 炎"
│       ├── AddIceButton            "❄️ 氷"
│       └── AddPoisonButton         "☠️ 毒"
└── DecoratorDemo                   [DecoratorDemo コンポーネント]
```

#### SerializeField接続

| フィールド | 接続先 |
|-----------|-------|
| `infoPanel` | PatternInfoPanel |
| `createSwordButton` | CreateSwordButton |
| `createBowButton` | CreateBowButton |
| `addFireButton` | AddFireButton |
| `addIceButton` | AddIceButton |
| `addPoisonButton` | AddPoisonButton |

---

### 10. FacadeDemo

**シーン名**: `FacadeDemo`
**配置場所**: `Assets/Structural/Facade/Scenes/FacadeDemo.unity`

#### Hierarchy

```
FacadeDemo
├── Main Camera
├── EventSystem
├── DemoSceneCanvas
│   └── ControlPanel > ButtonContainer
│       ├── FullInitButton          "フル初期化"
│       └── QuickStartButton        "クイックスタート"
└── FacadeDemo                      [FacadeDemo コンポーネント]
```

#### SerializeField接続

| フィールド | 接続先 |
|-----------|-------|
| `infoPanel` | PatternInfoPanel |
| `fullInitButton` | FullInitButton |
| `quickStartButton` | QuickStartButton |

---

### 11. FlyweightDemo

**シーン名**: `FlyweightDemo`
**配置場所**: `Assets/Structural/Flyweight/Scenes/FlyweightDemo.unity`

#### Hierarchy

```
FlyweightDemo
├── Main Camera
├── EventSystem
├── DemoSceneCanvas
│   └── ControlPanel > ButtonContainer
│       ├── SpawnNormalButton        "通常弾 x50"
│       ├── SpawnFireButton          "炎弾 x50"
│       ├── SpawnIceButton           "氷弾 x50"
│       └── StatsButton              "統計表示"
└── FlyweightDemo                   [FlyweightDemo コンポーネント]
```

#### SerializeField接続

| フィールド | 接続先 |
|-----------|-------|
| `infoPanel` | PatternInfoPanel |
| `spawnNormalButton` | SpawnNormalButton |
| `spawnFireButton` | SpawnFireButton |
| `spawnIceButton` | SpawnIceButton |
| `statsButton` | StatsButton |

---

### 12. ProxyDemo

**シーン名**: `ProxyDemo`
**配置場所**: `Assets/Structural/Proxy/Scenes/ProxyDemo.unity`

#### Hierarchy

```
ProxyDemo
├── Main Camera
├── EventSystem
├── DemoSceneCanvas
│   └── ControlPanel > ButtonContainer
│       ├── DisplayImage1Button     "画像1: hero_portrait"
│       ├── DisplayImage2Button     "画像2: world_map"
│       ├── DisplayImage3Button     "画像3: title_screen"
│       └── CheckStatusButton       "全画像の状態確認"
└── ProxyDemo                       [ProxyDemo コンポーネント]
```

#### SerializeField接続

| フィールド | 接続先 |
|-----------|-------|
| `infoPanel` | PatternInfoPanel |
| `displayImage1Button` | DisplayImage1Button |
| `displayImage2Button` | DisplayImage2Button |
| `displayImage3Button` | DisplayImage3Button |
| `checkStatusButton` | CheckStatusButton |

---

## 振る舞いパターン（11シーン）

### 13. ObserverDemo

**シーン名**: `ObserverDemo`
**配置場所**: `Assets/Behavioral/Observer/Scenes/ObserverDemo.unity`

#### Hierarchy

```
ObserverDemo
├── Main Camera
├── EventSystem
├── DemoSceneCanvas
│   └── ControlPanel > ButtonContainer
│       ├── DamageButton            "15 ダメージ"
│       ├── HealButton              "20 回復"
│       └── BigDamageButton         "50 大ダメージ!!"
└── ObserverDemo                    [ObserverDemo コンポーネント]
```

#### SerializeField接続

| フィールド | 接続先 |
|-----------|-------|
| `infoPanel` | PatternInfoPanel |
| `damageButton` | DamageButton |
| `healButton` | HealButton |
| `bigDamageButton` | BigDamageButton |

---

### 14. StrategyDemo

**シーン名**: `StrategyDemo`
**配置場所**: `Assets/Behavioral/Strategy/Scenes/StrategyDemo.unity`

#### Hierarchy

```
StrategyDemo
├── Main Camera
├── EventSystem
├── DemoSceneCanvas
│   └── ControlPanel > ButtonContainer
│       ├── SectionLabel (TMP)      "戦略を選択"
│       ├── AggressiveButton        "攻撃型"
│       ├── DefensiveButton         "防御型"
│       ├── FleeButton              "逃走型"
│       ├── Spacer
│       └── ExecuteButton           "戦略を実行！"
└── StrategyDemo                    [StrategyDemo コンポーネント]
```

#### SerializeField接続

| フィールド | 接続先 |
|-----------|-------|
| `infoPanel` | PatternInfoPanel |
| `aggressiveButton` | AggressiveButton |
| `defensiveButton` | DefensiveButton |
| `fleeButton` | FleeButton |
| `executeButton` | ExecuteButton |

---

### 15. StateDemo

**シーン名**: `StateDemo`
**配置場所**: `Assets/Behavioral/State/Scenes/StateDemo.unity`

#### Hierarchy

```
StateDemo
├── Main Camera
├── EventSystem
├── DemoSceneCanvas
│   └── ControlPanel > ButtonContainer
│       ├── SectionLabel (TMP)      "入力アクション"
│       ├── MoveButton              "移動"
│       ├── AttackButton            "攻撃"
│       ├── DamageButton            "被ダメージ"
│       ├── IdleButton              "待機"
│       ├── Spacer
│       └── UpdateButton            "Update実行"
└── StateDemo                       [StateDemo コンポーネント]
```

#### SerializeField接続

| フィールド | 接続先 |
|-----------|-------|
| `infoPanel` | PatternInfoPanel |
| `moveButton` | MoveButton |
| `attackButton` | AttackButton |
| `damageButton` | DamageButton |
| `idleButton` | IdleButton |
| `updateButton` | UpdateButton |

---

### 16. CommandDemo

**シーン名**: `CommandDemo`
**配置場所**: `Assets/Behavioral/Command/Scenes/CommandDemo.unity`

#### Hierarchy

```
CommandDemo
├── Main Camera
├── EventSystem
├── DemoSceneCanvas
│   └── ControlPanel > ButtonContainer
│       ├── SectionLabel (TMP)      "移動"
│       ├── MoveUpButton            "↑ 上"
│       ├── MoveRow                 [HorizontalLayoutGroup]
│       │   ├── MoveLeftButton      "← 左"
│       │   └── MoveRightButton     "→ 右"
│       ├── MoveDownButton          "↓ 下"
│       ├── Spacer
│       ├── SectionLabel2 (TMP)     "履歴操作"
│       ├── UndoButton              "取り消し (Undo)"
│       └── RedoButton              "やり直し (Redo)"
└── CommandDemo                     [CommandDemo コンポーネント]
```

#### SerializeField接続

| フィールド | 接続先 |
|-----------|-------|
| `infoPanel` | PatternInfoPanel |
| `moveUpButton` | MoveUpButton |
| `moveDownButton` | MoveDownButton |
| `moveLeftButton` | MoveLeftButton |
| `moveRightButton` | MoveRightButton |
| `undoButton` | UndoButton |
| `redoButton` | RedoButton |

---

### 17. ChainOfResponsibilityDemo

**シーン名**: `ChainOfResponsibilityDemo`
**配置場所**: `Assets/Behavioral/ChainOfResponsibility/Scenes/ChainOfResponsibilityDemo.unity`

#### Hierarchy

```
ChainOfResponsibilityDemo
├── Main Camera
├── EventSystem
├── DemoSceneCanvas
│   └── ControlPanel > ButtonContainer
│       ├── Attack10Button          "攻撃力 10"
│       ├── Attack30Button          "攻撃力 30"
│       ├── Attack100Button         "攻撃力 100"
│       └── ShowChainButton         "チェーン構成を表示"
└── ChainOfResponsibilityDemo       [ChainOfResponsibilityDemo コンポーネント]
```

#### SerializeField接続

| フィールド | 接続先 |
|-----------|-------|
| `infoPanel` | PatternInfoPanel |
| `attack10Button` | Attack10Button |
| `attack30Button` | Attack30Button |
| `attack100Button` | Attack100Button |
| `showChainButton` | ShowChainButton |

---

### 18. MediatorDemo

**シーン名**: `MediatorDemo`
**配置場所**: `Assets/Behavioral/Mediator/Scenes/MediatorDemo.unity`

#### Hierarchy

```
MediatorDemo
├── Main Camera
├── EventSystem
├── DemoSceneCanvas
│   └── ControlPanel > ButtonContainer
│       ├── AliceSendButton         "Alice が送信"
│       ├── BobSendButton           "Bob が送信"
│       └── CharlieSendButton       "Charlie が送信"
└── MediatorDemo                    [MediatorDemo コンポーネント]
```

#### SerializeField接続

| フィールド | 接続先 |
|-----------|-------|
| `infoPanel` | PatternInfoPanel |
| `aliceSendButton` | AliceSendButton |
| `bobSendButton` | BobSendButton |
| `charlieSendButton` | CharlieSendButton |

---

### 19. MementoDemo

**シーン名**: `MementoDemo`
**配置場所**: `Assets/Behavioral/Memento/Scenes/MementoDemo.unity`

#### Hierarchy

```
MementoDemo
├── Main Camera
├── EventSystem
├── DemoSceneCanvas
│   └── ControlPanel > ButtonContainer
│       ├── SectionLabel (TMP)      "ゲーム操作"
│       ├── LevelUpButton           "レベルアップ"
│       ├── TakeDamageButton        "ダメージを受ける"
│       ├── EarnGoldButton          "ゴールド獲得"
│       ├── Spacer
│       ├── SectionLabel2 (TMP)     "セーブ/ロード"
│       ├── SaveButton              "セーブ"
│       ├── LoadLastButton          "最後のセーブをロード"
│       └── ShowCurrentButton       "現在の状態を表示"
└── MementoDemo                     [MementoDemo コンポーネント]
```

#### SerializeField接続

| フィールド | 接続先 |
|-----------|-------|
| `infoPanel` | PatternInfoPanel |
| `levelUpButton` | LevelUpButton |
| `takeDamageButton` | TakeDamageButton |
| `earnGoldButton` | EarnGoldButton |
| `saveButton` | SaveButton |
| `loadLastButton` | LoadLastButton |
| `showCurrentButton` | ShowCurrentButton |

---

### 20. IteratorDemo

**シーン名**: `IteratorDemo`
**配置場所**: `Assets/Behavioral/Iterator/Scenes/IteratorDemo.unity`

#### Hierarchy

```
IteratorDemo
├── Main Camera
├── EventSystem
├── DemoSceneCanvas
│   └── ControlPanel > ButtonContainer
│       ├── NextItemButton          "次のアイテム"
│       ├── ResetButton             "リセット"
│       └── ShowAllButton           "全アイテム表示"
└── IteratorDemo                    [IteratorDemo コンポーネント]
```

#### SerializeField接続

| フィールド | 接続先 |
|-----------|-------|
| `infoPanel` | PatternInfoPanel |
| `nextItemButton` | NextItemButton |
| `resetButton` | ResetButton |
| `showAllButton` | ShowAllButton |

---

### 21. TemplateMethodDemo

**シーン名**: `TemplateMethodDemo`
**配置場所**: `Assets/Behavioral/TemplateMethod/Scenes/TemplateMethodDemo.unity`

#### Hierarchy

```
TemplateMethodDemo
├── Main Camera
├── EventSystem
├── DemoSceneCanvas
│   └── ControlPanel > ButtonContainer
│       ├── WarriorTurnButton       "戦士のターン"
│       ├── MageTurnButton          "魔法使いのターン"
│       └── HealerTurnButton        "ヒーラーのターン"
└── TemplateMethodDemo              [TemplateMethodDemo コンポーネント]
```

#### SerializeField接続

| フィールド | 接続先 |
|-----------|-------|
| `infoPanel` | PatternInfoPanel |
| `warriorTurnButton` | WarriorTurnButton |
| `mageTurnButton` | MageTurnButton |
| `healerTurnButton` | HealerTurnButton |

---

### 22. VisitorDemo

**シーン名**: `VisitorDemo`
**配置場所**: `Assets/Behavioral/Visitor/Scenes/VisitorDemo.unity`

#### Hierarchy

```
VisitorDemo
├── Main Camera
├── EventSystem
├── DemoSceneCanvas
│   └── ControlPanel > ButtonContainer
│       ├── CalculateCostButton     "コスト計算"
│       └── CalculateEffectsButton  "効果計算"
└── VisitorDemo                     [VisitorDemo コンポーネント]
```

#### SerializeField接続

| フィールド | 接続先 |
|-----------|-------|
| `infoPanel` | PatternInfoPanel |
| `calculateCostButton` | CalculateCostButton |
| `calculateEffectsButton` | CalculateEffectsButton |

---

### 23. InterpreterDemo

**シーン名**: `InterpreterDemo`
**配置場所**: `Assets/Behavioral/Interpreter/Scenes/InterpreterDemo.unity`

#### Hierarchy

```
InterpreterDemo
├── Main Camera
├── EventSystem
├── DemoSceneCanvas
│   └── ControlPanel > ButtonContainer
│       ├── MoveUpButton            "MOVE UP 3"
│       ├── MoveLeftButton          "MOVE LEFT 2"
│       ├── StatusButton            "STATUS"
│       └── RepeatButton            "REPEAT 3 MOVE RIGHT 1"
└── InterpreterDemo                 [InterpreterDemo コンポーネント]
```

#### SerializeField接続

| フィールド | 接続先 |
|-----------|-------|
| `infoPanel` | PatternInfoPanel |
| `moveUpButton` | MoveUpButton |
| `moveLeftButton` | MoveLeftButton |
| `statusButton` | StatusButton |
| `repeatButton` | RepeatButton |

---

## Build Settingsのシーン登録順

| # | シーン名 | パス |
|---|---------|------|
| 0 | MainMenu | Assets/Scenes/MainMenu.unity |
| 1 | SingletonDemo | Assets/Creational/Singleton/Scenes/SingletonDemo.unity |
| 2 | FactoryMethodDemo | Assets/Creational/FactoryMethod/Scenes/FactoryMethodDemo.unity |
| 3 | AbstractFactoryDemo | Assets/Creational/AbstractFactory/Scenes/AbstractFactoryDemo.unity |
| 4 | BuilderDemo | Assets/Creational/Builder/Scenes/BuilderDemo.unity |
| 5 | PrototypeDemo | Assets/Creational/Prototype/Scenes/PrototypeDemo.unity |
| 6 | AdapterDemo | Assets/Structural/Adapter/Scenes/AdapterDemo.unity |
| 7 | BridgeDemo | Assets/Structural/Bridge/Scenes/BridgeDemo.unity |
| 8 | CompositeDemo | Assets/Structural/Composite/Scenes/CompositeDemo.unity |
| 9 | DecoratorDemo | Assets/Structural/Decorator/Scenes/DecoratorDemo.unity |
| 10 | FacadeDemo | Assets/Structural/Facade/Scenes/FacadeDemo.unity |
| 11 | FlyweightDemo | Assets/Structural/Flyweight/Scenes/FlyweightDemo.unity |
| 12 | ProxyDemo | Assets/Structural/Proxy/Scenes/ProxyDemo.unity |
| 13 | ObserverDemo | Assets/Behavioral/Observer/Scenes/ObserverDemo.unity |
| 14 | StrategyDemo | Assets/Behavioral/Strategy/Scenes/StrategyDemo.unity |
| 15 | StateDemo | Assets/Behavioral/State/Scenes/StateDemo.unity |
| 16 | CommandDemo | Assets/Behavioral/Command/Scenes/CommandDemo.unity |
| 17 | ChainOfResponsibilityDemo | Assets/Behavioral/ChainOfResponsibility/Scenes/ChainOfResponsibilityDemo.unity |
| 18 | MediatorDemo | Assets/Behavioral/Mediator/Scenes/MediatorDemo.unity |
| 19 | MementoDemo | Assets/Behavioral/Memento/Scenes/MementoDemo.unity |
| 20 | IteratorDemo | Assets/Behavioral/Iterator/Scenes/IteratorDemo.unity |
| 21 | TemplateMethodDemo | Assets/Behavioral/TemplateMethod/Scenes/TemplateMethodDemo.unity |
| 22 | VisitorDemo | Assets/Behavioral/Visitor/Scenes/VisitorDemo.unity |
| 23 | InterpreterDemo | Assets/Behavioral/Interpreter/Scenes/InterpreterDemo.unity |
