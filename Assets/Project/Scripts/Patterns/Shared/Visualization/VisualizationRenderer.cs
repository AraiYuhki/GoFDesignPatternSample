using UnityEngine;
using UnityEngine.UI;

namespace GoFPatterns.Patterns.Visualization {
    /// <summary>
    /// 2Dビジュアライゼーション用のカメラとRenderTextureを管理するコンポーネント
    /// 専用カメラで2Dオブジェクトをレンダリングし、RawImageを通じてCanvas内に表示する
    /// </summary>
    public class VisualizationRenderer : MonoBehaviour {
        /// <summary>レンダリング結果を表示するRawImage</summary>
        [SerializeField]
        private RawImage targetRawImage;
        /// <summary>カメラの表示範囲</summary>
        [SerializeField]
        private float cameraSize = 5f;
        /// <summary>背景色</summary>
        [SerializeField]
        private Color backgroundColor = new Color(0.1f, 0.1f, 0.18f, 1f);

        /// <summary>レンダリング用カメラ</summary>
        private Camera renderCamera;
        /// <summary>レンダリング先のRenderTexture</summary>
        private RenderTexture renderTexture;
        /// <summary>ビジュアライゼーション要素の親Transform</summary>
        private Transform visualRoot;

        /// <summary>RenderTextureの幅</summary>
        private const int TextureWidth = 1024;
        /// <summary>RenderTextureの高さ</summary>
        private const int TextureHeight = 768;
        /// <summary>ワールド空間でのオフセット</summary>
        private static readonly Vector3 WorldOffset = new Vector3(0f, 100f, 0f);

        /// <summary>ビジュアライゼーション要素を配置する親Transformを取得する</summary>
        public Transform VisualRoot => visualRoot;

        /// <summary>
        /// 起動時にビジュアライゼーション空間を初期化する
        /// </summary>
        private void Awake() {
            SetupVisualRoot();
            SetupCamera();
            SetupRenderTexture();
        }

        /// <summary>
        /// 実行時にRawImageをバインドしてRenderTextureを接続する
        /// </summary>
        /// <param name="rawImage">表示先のRawImage</param>
        public void SetTargetImage(RawImage rawImage) {
            targetRawImage = rawImage;
            if (targetRawImage != null && renderTexture != null) {
                targetRawImage.texture = renderTexture;
            }
        }

        /// <summary>ビジュアライゼーション空間の全オブジェクトを削除する</summary>
        public void ClearAll() {
            if (visualRoot == null) {
                return;
            }
            for (int i = visualRoot.childCount - 1; i >= 0; i--) {
                var child = visualRoot.GetChild(i);
                if (child.GetComponent<Camera>() != null) {
                    continue;
                }
                Destroy(child.gameObject);
            }
        }

        /// <summary>
        /// ビジュアライゼーション要素の親GameObjectを生成する
        /// </summary>
        private void SetupVisualRoot() {
            var rootGo = new GameObject("VisualizationRoot");
            rootGo.transform.position = WorldOffset;
            visualRoot = rootGo.transform;
        }

        /// <summary>
        /// 専用の正射影カメラを生成して設定する
        /// </summary>
        private void SetupCamera() {
            var cameraGo = new GameObject("VisualizationCamera");
            cameraGo.transform.SetParent(visualRoot, false);
            cameraGo.transform.localPosition = new Vector3(0f, 0f, -10f);

            renderCamera = cameraGo.AddComponent<Camera>();
            renderCamera.orthographic = true;
            renderCamera.orthographicSize = cameraSize;
            renderCamera.backgroundColor = backgroundColor;
            renderCamera.clearFlags = CameraClearFlags.SolidColor;
            renderCamera.nearClipPlane = 0.1f;
            renderCamera.farClipPlane = 100f;
            renderCamera.depth = -10;
            renderCamera.tag = "Untagged";
        }

        /// <summary>
        /// RenderTextureを生成してカメラとRawImageに紐付ける
        /// </summary>
        private void SetupRenderTexture() {
            renderTexture = new RenderTexture(TextureWidth, TextureHeight, 16);
            renderTexture.antiAliasing = 2;
            renderCamera.targetTexture = renderTexture;

            if (targetRawImage != null) {
                targetRawImage.texture = renderTexture;
            }
        }

        /// <summary>
        /// 破棄時にRenderTextureとビジュアライゼーション空間を解放する
        /// </summary>
        private void OnDestroy() {
            if (renderTexture != null) {
                renderTexture.Release();
                Destroy(renderTexture);
            }
            if (visualRoot != null) {
                Destroy(visualRoot.gameObject);
            }
        }
    }
}
