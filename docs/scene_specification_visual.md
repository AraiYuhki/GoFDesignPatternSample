# Scene Specification (Visual Guide)

このファイルは `scene_specification.md` のレイアウト情報を、図と一覧で把握しやすくした補助資料です。

## 1. 共通デモ画面レイアウト（全パターン共通）

```text
+--------------------------------------------------------------------------------------------------+
| Header: [BackButton] [PatternNameText] [CategoryText]                                            |
+-----------------------------------------------+--------------------------------------------------+
| ControlPanel (Scene-specific UI)              | LogPanel (InGameLogger)                          |
| - Buttons / Labels / Spacer                   | - LogScrollView                                  |
| - Vertical layout                             | - Runtime logs                                   |
+-----------------------------------------------+--------------------------------------------------+
| Footer: [DescriptionText]                                                                       |
+--------------------------------------------------------------------------------------------------+
```

### 共通構成（Hierarchy の骨組み）

```text
DemoSceneCanvas
├─ Header
│  ├─ BackButton
│  ├─ PatternNameText
│  └─ CategoryText
├─ ContentArea
│  ├─ ControlPanel
│  │  └─ ButtonContainer
│  └─ LogPanel
│     └─ LogScrollView
└─ Footer
   └─ DescriptionText
```

## 2. MainMenu レイアウト

```text
+--------------------------------------------------------------------------------------------------+
|                               Title Area (Title / Subtitle)                                       |
+--------------------------------------------------------------------------------------------------+
| ScrollView                                                                                         |
|  [Creational section: 5 buttons]                                                                  |
|  [Structural section: 7 buttons]                                                                  |
|  [Behavioral section: 11 buttons]                                                                 |
+--------------------------------------------------------------------------------------------------+
| VersionText                                                                                        |
+--------------------------------------------------------------------------------------------------+
```

## 3. シーン別コントロールパネル（視覚一覧）

下図は各 Demo シーンで差し替わる左ペインの内容です。

### SingletonDemo

```text
+----------------------------------+
| ControlPanel > ButtonContainer   |
+----------------------------------+
| AddScoreButton                 |
| CheckInstanceButton            |
| DuplicateTestButton            |
| ResetScoreButton               |
+----------------------------------+
```

### FactoryMethodDemo

```text
+----------------------------------+
| ControlPanel > ButtonContainer   |
+----------------------------------+
| AttackButton                   |
| CreateDragonButton             |
| CreateGoblinButton             |
| CreateSlimeButton              |
+----------------------------------+
```

### AbstractFactoryDemo

```text
+----------------------------------+
| ControlPanel > ButtonContainer   |
+----------------------------------+
| DarkThemeButton                |
| LightThemeButton               |
| RetroThemeButton               |
+----------------------------------+
```

### BuilderDemo

```text
+----------------------------------+
| ControlPanel > ButtonContainer   |
+----------------------------------+
| BuildMageFullButton            |
| BuildThiefFullButton           |
| BuildWarriorFullButton         |
| BuildWarriorMinButton          |
+----------------------------------+
```

### PrototypeDemo

```text
+----------------------------------+
| ControlPanel > ButtonContainer   |
+----------------------------------+
| CloneArcherButton              |
| CloneSoldierButton             |
| CompareButton                  |
| ModifyCloneButton              |
+----------------------------------+
```

### AdapterDemo

```text
+----------------------------------+
| ControlPanel > ButtonContainer   |
+----------------------------------+
| PlayBattleButton               |
| PlayFieldButton                |
| StatusButton                   |
| StopButton                     |
+----------------------------------+
```

### BridgeDemo

```text
+----------------------------------+
| ControlPanel > ButtonContainer   |
+----------------------------------+
| BlueRectButton                 |
| GreenCircleButton              |
| RedCircleButton                |
| ShowAllButton                  |
+----------------------------------+
```

### CompositeDemo

```text
+----------------------------------+
| ControlPanel > ButtonContainer   |
+----------------------------------+
| AddFileButton                  |
| CalcSizeButton                 |
| ShowTreeButton                 |
+----------------------------------+
```

### DecoratorDemo

```text
+----------------------------------+
| ControlPanel > ButtonContainer   |
+----------------------------------+
| AddFireButton                  |
| AddIceButton                   |
| AddPoisonButton                |
| CreateBowButton                |
| CreateSwordButton              |
+----------------------------------+
```

### FacadeDemo

```text
+----------------------------------+
| ControlPanel > ButtonContainer   |
+----------------------------------+
| FullInitButton                 |
| QuickStartButton               |
+----------------------------------+
```

### FlyweightDemo

```text
+----------------------------------+
| ControlPanel > ButtonContainer   |
+----------------------------------+
| SpawnFireButton                |
| SpawnIceButton                 |
| SpawnNormalButton              |
| StatsButton                    |
+----------------------------------+
```

### ProxyDemo

```text
+----------------------------------+
| ControlPanel > ButtonContainer   |
+----------------------------------+
| CheckStatusButton              |
| DisplayImage1Button            |
| DisplayImage2Button            |
| DisplayImage3Button            |
+----------------------------------+
```

### ObserverDemo

```text
+----------------------------------+
| ControlPanel > ButtonContainer   |
+----------------------------------+
| BigDamageButton                |
| DamageButton                   |
| HealButton                     |
+----------------------------------+
```

### StrategyDemo

```text
+----------------------------------+
| ControlPanel > ButtonContainer   |
+----------------------------------+
| AggressiveButton               |
| DefensiveButton                |
| ExecuteButton                  |
| FleeButton                     |
+----------------------------------+
```

### StateDemo

```text
+----------------------------------+
| ControlPanel > ButtonContainer   |
+----------------------------------+
| AttackButton                   |
| DamageButton                   |
| IdleButton                     |
| MoveButton                     |
| UpdateButton                   |
+----------------------------------+
```

### CommandDemo

```text
+----------------------------------+
| ControlPanel > ButtonContainer   |
+----------------------------------+
| MoveDownButton                 |
| MoveLeftButton                 |
| MoveRightButton                |
| MoveUpButton                   |
| RedoButton                     |
| UndoButton                     |
+----------------------------------+
```

### ChainOfResponsibilityDemo

```text
+----------------------------------+
| ControlPanel > ButtonContainer   |
+----------------------------------+
| Attack100Button                |
| Attack10Button                 |
| Attack30Button                 |
| ShowChainButton                |
+----------------------------------+
```

### MediatorDemo

```text
+----------------------------------+
| ControlPanel > ButtonContainer   |
+----------------------------------+
| AliceSendButton                |
| BobSendButton                  |
| CharlieSendButton              |
+----------------------------------+
```

### MementoDemo

```text
+----------------------------------+
| ControlPanel > ButtonContainer   |
+----------------------------------+
| EarnGoldButton                 |
| LevelUpButton                  |
| LoadLastButton                 |
| SaveButton                     |
| ShowCurrentButton              |
| TakeDamageButton               |
+----------------------------------+
```

### IteratorDemo

```text
+----------------------------------+
| ControlPanel > ButtonContainer   |
+----------------------------------+
| NextItemButton                 |
| ResetButton                    |
| ShowAllButton                  |
+----------------------------------+
```

### TemplateMethodDemo

```text
+----------------------------------+
| ControlPanel > ButtonContainer   |
+----------------------------------+
| HealerTurnButton               |
| MageTurnButton                 |
| WarriorTurnButton              |
+----------------------------------+
```

### VisitorDemo

```text
+----------------------------------+
| ControlPanel > ButtonContainer   |
+----------------------------------+
| CalculateCostButton            |
| CalculateEffectsButton         |
+----------------------------------+
```

### InterpreterDemo

```text
+----------------------------------+
| ControlPanel > ButtonContainer   |
+----------------------------------+
| MoveLeftButton                 |
| MoveUpButton                   |
| RepeatButton                   |
| StatusButton                   |
+----------------------------------+
```

## 4. 使い方

1. まず「共通デモ画面レイアウト」で全シーン共通の骨組みを把握する。
2. 次に「シーン別コントロールパネル」で、各シーン固有の操作UIだけを見る。
3. 詳細な SerializeField 参照は元ファイル `scene_specification.md` を参照する。

