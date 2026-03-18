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
            var existing = AssetDatabase.LoadAssetAtPath<PatternDefinition>($"{DefinitionsDir}/singleton.asset");
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
            rt.sizeDelta = new Vector2(0f, 80f);

            cardGo.AddComponent<Image>().color = new Color(0.15f, 0.15f, 0.22f, 1f);
            cardGo.AddComponent<Button>();

            var nameTextGo = CreateChildText(cardGo.transform, "NameText",
                new Vector2(0f, 0.45f), new Vector2(1f, 1f), 22f, FontStyles.Bold);

            var summaryTextGo = CreateChildText(cardGo.transform, "SummaryText",
                new Vector2(0f, 0f), new Vector2(1f, 0.45f), 14f, FontStyles.Normal);
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
