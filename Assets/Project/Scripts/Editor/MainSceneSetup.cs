using System.IO;
using GoFPatterns.Core;
using GoFPatterns.Patterns;
using GoFPatterns.Patterns.Visualization;
using GoFPatterns.UI;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GoFPatterns.Editor {
    /// <summary>
    /// Assets/Project/Scenes/Main.unity を自動生成するエディタユーティリティ
    /// 未生成時に InitializeOnLoad により自動実行される
    /// </summary>
    [InitializeOnLoad]
    static class MainSceneSetup {
        private const string ScenePath = "Assets/Project/Scenes/Main.unity";
        private const string RepositoryPath = "Assets/Project/ScriptableObjects/PatternRepository.asset";

        /// <summary>起動時にシーンが存在しなければ生成を予約する</summary>
        static MainSceneSetup() {
            if (!File.Exists(ScenePath)) {
                EditorApplication.delayCall += CreateMainScene;
            }
        }

        /// <summary>Mainシーンを生成して保存する（メニューから手動再実行可）</summary>
        [MenuItem("Tools/GoF Patterns/Recreate Main Scene")]
        static void CreateMainScene() {
            EnsureDirectory("Assets/Project/Scenes");
            EnsureDirectory("Assets/Project/ScriptableObjects");
            EnsureDirectory("Assets/Project/ScriptableObjects/PatternDefinitions");

            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            // EventSystem
            var eventSystemGo = new GameObject("EventSystem");
            eventSystemGo.AddComponent<EventSystem>();
            var inputType = System.Type.GetType(
                "UnityEngine.InputSystem.UI.InputSystemUIInputModule, Unity.InputSystem");
            if (inputType != null) {
                eventSystemGo.AddComponent(inputType);
            } else {
                eventSystemGo.AddComponent<StandaloneInputModule>();
            }

            // Canvas
            var canvasGo = new GameObject("Canvas");
            var canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasGo.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;
            canvasGo.AddComponent<GraphicRaycaster>();
            var screenManager = canvasGo.AddComponent<ScreenManager>();
            canvasGo.AddComponent<AppBootstrap>();

            // Managers
            var managersGo = new GameObject("[Managers]");
            var demoManager = managersGo.AddComponent<DemoManager>();
            var vizRenderer = managersGo.AddComponent<VisualizationRenderer>();
            SetObjField(demoManager, "visualizationRenderer", vizRenderer);

            // Panels
            var listPanel = CreatePatternListPanel(canvasGo.transform);
            var detailPanel = CreatePatternDetailPanel(canvasGo.transform);
            var demoPanel = CreateDemoPanel(canvasGo.transform);
            listPanel.SetActive(false);
            detailPanel.SetActive(false);
            demoPanel.SetActive(false);

            // ScreenManager.screens を設定する
            var smSo = new SerializedObject(screenManager);
            var screensProp = smSo.FindProperty("screens");
            screensProp.arraySize = 3;
            screensProp.GetArrayElementAtIndex(0).objectReferenceValue = listPanel.GetComponent<BaseScreen>();
            screensProp.GetArrayElementAtIndex(1).objectReferenceValue = detailPanel.GetComponent<BaseScreen>();
            screensProp.GetArrayElementAtIndex(2).objectReferenceValue = demoPanel.GetComponent<BaseScreen>();
            smSo.ApplyModifiedProperties();

            // PatternRepository を作成してDemoManagerにセットする
            var repository = GetOrCreatePatternRepository();
            SetObjField(demoManager, "patternRepository", repository);

            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.Refresh();
            Debug.Log($"[MainSceneSetup] Created: {ScenePath}");
        }

        /// <summary>PatternListPanel を生成して返す</summary>
        static GameObject CreatePatternListPanel(Transform parent) {
            var panel = CreatePanel(parent, "PatternListPanel");
            var screen = panel.AddComponent<PatternListScreen>();

            // フィルタ行
            var filterGo = CreateUIObj("FilterContainer", panel.transform,
                new Vector2(0f, 0.93f), new Vector2(1f, 1f));
            var hlg = filterGo.AddComponent<HorizontalLayoutGroup>();
            hlg.childForceExpandWidth = true;
            hlg.childForceExpandHeight = true;
            hlg.childControlWidth = true;
            hlg.spacing = 4f;
            CreateButton(filterGo.transform, "AllButton", "全て");
            CreateButton(filterGo.transform, "CreationalButton", "生成");
            CreateButton(filterGo.transform, "StructuralButton", "構造");
            CreateButton(filterGo.transform, "BehavioralButton", "振る舞い");

            // カードスクロールビュー
            var scrollGo = CreateScrollView(panel.transform, "CardScrollView",
                new Vector2(0f, 0f), new Vector2(1f, 0.93f));
            var contentGo = scrollGo.transform.Find("Viewport/Content").gameObject;

            SetStrField(screen, "screenId", "list");
            SetObjField(screen, "cardContainer", contentGo.GetComponent<RectTransform>());
            SetObjField(screen, "filterContainer", filterGo.GetComponent<RectTransform>());
            return panel;
        }

        /// <summary>PatternDetailPanel を生成して返す</summary>
        static GameObject CreatePatternDetailPanel(Transform parent) {
            var panel = CreatePanel(parent, "PatternDetailPanel");
            var screen = panel.AddComponent<PatternDetailScreen>();

            var backBtn = CreateButton(panel.transform, "BackButton", "< 戻る",
                new Vector2(0f, 0.93f), new Vector2(0.15f, 1f));
            var nameLabel = CreateLabel(panel.transform, "PatternNameLabel",
                new Vector2(0.15f, 0.93f), new Vector2(1f, 1f), 30f);
            var categoryLabel = CreateLabel(panel.transform, "CategoryLabel",
                new Vector2(0f, 0.87f), new Vector2(1f, 0.93f), 20f);
            var descText = CreateLabel(panel.transform, "DescriptionText",
                new Vector2(0f, 0.7f), new Vector2(1f, 0.87f), 18f);
            var whenToUseText = CreateLabel(panel.transform, "WhenToUseText",
                new Vector2(0f, 0.55f), new Vector2(1f, 0.7f), 18f);
            var advantagesText = CreateLabel(panel.transform, "AdvantagesText",
                new Vector2(0f, 0.4f), new Vector2(1f, 0.55f), 18f);
            var disadvantagesText = CreateLabel(panel.transform, "DisadvantagesText",
                new Vector2(0f, 0.25f), new Vector2(1f, 0.4f), 18f);
            var startBtn = CreateButton(panel.transform, "StartDemoButton", "デモ開始",
                new Vector2(0.3f, 0.05f), new Vector2(0.7f, 0.2f));

            SetStrField(screen, "screenId", "detail");
            SetObjField(screen, "patternNameLabel", nameLabel.GetComponent<TMP_Text>());
            SetObjField(screen, "categoryLabel", categoryLabel.GetComponent<TMP_Text>());
            SetObjField(screen, "descriptionText", descText.GetComponent<TMP_Text>());
            SetObjField(screen, "whenToUseText", whenToUseText.GetComponent<TMP_Text>());
            SetObjField(screen, "advantagesText", advantagesText.GetComponent<TMP_Text>());
            SetObjField(screen, "disadvantagesText", disadvantagesText.GetComponent<TMP_Text>());
            SetObjField(screen, "startDemoButton", startBtn.GetComponent<Button>());
            SetObjField(screen, "backButton", backBtn.GetComponent<Button>());
            return panel;
        }

        /// <summary>DemoPanel を生成して返す</summary>
        static GameObject CreateDemoPanel(Transform parent) {
            var panel = CreatePanel(parent, "DemoPanel");
            var screen = panel.AddComponent<DemoScreen>();

            var backBtn = CreateButton(panel.transform, "BackButton", "< 戻る",
                new Vector2(0f, 0.93f), new Vector2(0.12f, 1f));
            var nameLabel = CreateLabel(panel.transform, "PatternNameLabel",
                new Vector2(0.12f, 0.93f), new Vector2(0.78f, 1f), 26f);
            var stepLabel = CreateLabel(panel.transform, "StepLabel",
                new Vector2(0.78f, 0.93f), new Vector2(1f, 1f), 20f);
            var stepDescLabel = CreateLabel(panel.transform, "StepDescriptionLabel",
                new Vector2(0f, 0.87f), new Vector2(1f, 0.93f), 17f);

            // ビジュアライゼーション表示（左側）
            var vizGo = CreateUIObj("VisualizationDisplay",
                panel.transform, new Vector2(0f, 0.3f), new Vector2(0.6f, 0.87f));
            var rawImage = vizGo.AddComponent<RawImage>();
            rawImage.color = new Color(0.05f, 0.05f, 0.15f, 1f);

            // ログパネル（右側）
            var logScrollGo = CreateScrollView(panel.transform, "LogScrollRect",
                new Vector2(0.6f, 0.3f), new Vector2(1f, 0.87f));
            var logContent = logScrollGo.transform.Find("Viewport/Content").gameObject;
            var logTextGo = CreateLabel(logContent.transform, "LogText",
                new Vector2(0f, 0f), new Vector2(1f, 1f), 14f);
            var logTmp = logTextGo.GetComponent<TextMeshProUGUI>();
            logTmp.textWrappingMode = TextWrappingModes.NoWrap;
            logTmp.overflowMode = TextOverflowModes.Overflow;
            var logCsf = logTextGo.AddComponent<ContentSizeFitter>();
            logCsf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            // 制御ボタン行
            var playBtn = CreateButton(panel.transform, "PlayButton", "▶",
                new Vector2(0f, 0.18f), new Vector2(0.2f, 0.3f));
            var pauseBtn = CreateButton(panel.transform, "PauseButton", "⏸",
                new Vector2(0.2f, 0.18f), new Vector2(0.4f, 0.3f));
            var stepBtn = CreateButton(panel.transform, "StepButton", "→|",
                new Vector2(0.4f, 0.18f), new Vector2(0.6f, 0.3f));
            var resetBtn = CreateButton(panel.transform, "ResetButton", "⟳",
                new Vector2(0.6f, 0.18f), new Vector2(0.8f, 0.3f));
            var speedSliderGo = CreateSlider(panel.transform, "SpeedSlider",
                new Vector2(0f, 0.05f), new Vector2(1f, 0.18f));

            SetStrField(screen, "screenId", "demo");
            SetObjField(screen, "patternNameLabel", nameLabel.GetComponent<TMP_Text>());
            SetObjField(screen, "stepLabel", stepLabel.GetComponent<TMP_Text>());
            SetObjField(screen, "stepDescriptionLabel", stepDescLabel.GetComponent<TMP_Text>());
            SetObjField(screen, "logText", logTmp);
            SetObjField(screen, "logScrollRect", logScrollGo.GetComponent<ScrollRect>());
            SetObjField(screen, "playButton", playBtn.GetComponent<Button>());
            SetObjField(screen, "pauseButton", pauseBtn.GetComponent<Button>());
            SetObjField(screen, "stepButton", stepBtn.GetComponent<Button>());
            SetObjField(screen, "resetButton", resetBtn.GetComponent<Button>());
            SetObjField(screen, "backButton", backBtn.GetComponent<Button>());
            SetObjField(screen, "speedSlider", speedSliderGo.GetComponent<Slider>());
            SetObjField(screen, "visualizationDisplay", rawImage);
            return panel;
        }

        // ---- UI ヘルパー ----

        /// <summary>背景色付きのフルスクリーンパネルを生成する</summary>
        static GameObject CreatePanel(Transform parent, string name) {
            var go = CreateUIObj(name, parent, Vector2.zero, Vector2.one);
            go.AddComponent<Image>().color = new Color(0.08f, 0.08f, 0.12f, 1f);
            return go;
        }

        /// <summary>アンカー指定付きのRectTransform GOを生成する</summary>
        static GameObject CreateUIObj(string name, Transform parent,
            Vector2 anchorMin, Vector2 anchorMax) {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            var rt = go.AddComponent<RectTransform>();
            rt.anchorMin = anchorMin;
            rt.anchorMax = anchorMax;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            return go;
        }

        /// <summary>TextMeshProUGUI ラベルを生成する</summary>
        static GameObject CreateLabel(Transform parent, string name,
            Vector2 anchorMin, Vector2 anchorMax, float fontSize = 24f) {
            var go = CreateUIObj(name, parent, anchorMin, anchorMax);
            var tmp = go.AddComponent<TextMeshProUGUI>();
            tmp.color = Color.white;
            tmp.fontSize = fontSize;
            tmp.textWrappingMode = TextWrappingModes.Normal;
            return go;
        }

        /// <summary>アンカー自動指定なしのボタンを生成する（LayoutGroup内用）</summary>
        static GameObject CreateButton(Transform parent, string name, string label) {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.AddComponent<RectTransform>();
            go.AddComponent<Image>().color = new Color(0.2f, 0.4f, 0.85f, 1f);
            go.AddComponent<Button>();
            var textGo = CreateUIObj("Text", go.transform, Vector2.zero, Vector2.one);
            var tmp = textGo.AddComponent<TextMeshProUGUI>();
            tmp.text = label;
            tmp.color = Color.white;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.fontSize = 20f;
            return go;
        }

        /// <summary>アンカー指定付きのボタンを生成する</summary>
        static GameObject CreateButton(Transform parent, string name, string label,
            Vector2 anchorMin, Vector2 anchorMax) {
            var go = CreateUIObj(name, parent, anchorMin, anchorMax);
            go.AddComponent<Image>().color = new Color(0.2f, 0.4f, 0.85f, 1f);
            go.AddComponent<Button>();
            var textGo = CreateUIObj("Text", go.transform, Vector2.zero, Vector2.one);
            var tmp = textGo.AddComponent<TextMeshProUGUI>();
            tmp.text = label;
            tmp.color = Color.white;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.fontSize = 20f;
            return go;
        }

        /// <summary>垂直スクロールビューを生成してルートGOを返す</summary>
        static GameObject CreateScrollView(Transform parent, string name,
            Vector2 anchorMin, Vector2 anchorMax) {
            var go = CreateUIObj(name, parent, anchorMin, anchorMax);
            go.AddComponent<Image>().color = new Color(0.05f, 0.05f, 0.08f, 0.8f);

            var viewport = CreateUIObj("Viewport", go.transform, Vector2.zero, Vector2.one);
            viewport.AddComponent<Image>().color = Color.clear;
            viewport.AddComponent<RectMask2D>();

            var content = new GameObject("Content");
            content.transform.SetParent(viewport.transform, false);
            var contentRt = content.AddComponent<RectTransform>();
            contentRt.anchorMin = new Vector2(0f, 1f);
            contentRt.anchorMax = new Vector2(1f, 1f);
            contentRt.pivot = new Vector2(0.5f, 1f);
            contentRt.offsetMin = Vector2.zero;
            contentRt.offsetMax = Vector2.zero;
            var vlg = content.AddComponent<VerticalLayoutGroup>();
            vlg.childForceExpandWidth = true;
            vlg.childForceExpandHeight = false;
            vlg.childControlWidth = true;
            vlg.spacing = 4f;
            vlg.padding = new RectOffset(4, 4, 4, 4);
            var csf = content.AddComponent<ContentSizeFitter>();
            csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            var scrollRect = go.AddComponent<ScrollRect>();
            scrollRect.content = contentRt;
            scrollRect.viewport = viewport.GetComponent<RectTransform>();
            scrollRect.horizontal = false;
            scrollRect.vertical = true;
            scrollRect.scrollSensitivity = 30f;
            return go;
        }

        /// <summary>水平スライダーを生成する</summary>
        static GameObject CreateSlider(Transform parent, string name,
            Vector2 anchorMin, Vector2 anchorMax) {
            var go = CreateUIObj(name, parent, anchorMin, anchorMax);

            var bgGo = CreateUIObj("Background", go.transform,
                new Vector2(0f, 0.25f), new Vector2(1f, 0.75f));
            bgGo.AddComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f, 1f);

            var fillAreaGo = CreateUIObj("Fill Area", go.transform,
                new Vector2(0f, 0.25f), new Vector2(1f, 0.75f));
            fillAreaGo.GetComponent<RectTransform>().offsetMin = new Vector2(5f, 0f);
            fillAreaGo.GetComponent<RectTransform>().offsetMax = new Vector2(-15f, 0f);

            var fillGo = CreateUIObj("Fill", fillAreaGo.transform,
                Vector2.zero, new Vector2(0f, 1f));
            fillGo.AddComponent<Image>().color = new Color(0.2f, 0.6f, 0.9f, 1f);

            var handleAreaGo = CreateUIObj("Handle Slide Area", go.transform,
                Vector2.zero, Vector2.one);
            var handleGo = CreateUIObj("Handle", handleAreaGo.transform,
                Vector2.zero, new Vector2(0f, 1f));
            handleGo.GetComponent<RectTransform>().sizeDelta = new Vector2(20f, 0f);
            handleGo.AddComponent<Image>().color = Color.white;

            var slider = go.AddComponent<Slider>();
            slider.fillRect = fillGo.GetComponent<RectTransform>();
            slider.handleRect = handleGo.GetComponent<RectTransform>();
            slider.direction = Slider.Direction.LeftToRight;
            slider.minValue = 0f;
            slider.maxValue = 1f;
            slider.value = 0.5f;
            return go;
        }

        // ---- アセット / SerializedObject ヘルパー ----

        /// <summary>PatternRepository アセットを取得または生成する</summary>
        static PatternRepository GetOrCreatePatternRepository() {
            var existing = AssetDatabase.LoadAssetAtPath<PatternRepository>(RepositoryPath);
            if (existing != null) {
                return existing;
            }
            var repo = ScriptableObject.CreateInstance<PatternRepository>();
            AssetDatabase.CreateAsset(repo, RepositoryPath);
            AssetDatabase.SaveAssets();
            return repo;
        }

        /// <summary>SerializedObject を介して string フィールドを設定する</summary>
        static void SetStrField(Object target, string propertyName, string value) {
            var so = new SerializedObject(target);
            var prop = so.FindProperty(propertyName);
            if (prop != null) {
                prop.stringValue = value;
                so.ApplyModifiedProperties();
            } else {
                Debug.LogWarning(
                    $"[MainSceneSetup] Property '{propertyName}' not found on {target.GetType().Name}");
            }
        }

        /// <summary>SerializedObject を介して Object 参照フィールドを設定する</summary>
        static void SetObjField(Object target, string propertyName, Object value) {
            var so = new SerializedObject(target);
            var prop = so.FindProperty(propertyName);
            if (prop != null) {
                prop.objectReferenceValue = value;
                so.ApplyModifiedProperties();
            } else {
                Debug.LogWarning(
                    $"[MainSceneSetup] Property '{propertyName}' not found on {target.GetType().Name}");
            }
        }

        /// <summary>ディレクトリが存在しない場合は作成する</summary>
        static void EnsureDirectory(string path) {
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
        }
    }
}
