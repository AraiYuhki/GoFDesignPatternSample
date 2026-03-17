using UnityEngine;
using UnityEngine.UI;

namespace DesignPatterns.Visualization
{
    /// <summary>
    /// 2Dビジュアライゼーション用のカメラとRenderTextureを管理するコンポーネント
    /// 専用のOrthographicカメラで2Dオブジェクトをレンダリングし、
    /// RawImageを通じてCanvas内に表示する
    /// </summary>
    public class VisualizationRenderer : MonoBehaviour
    {
        /// <summary>レンダリング結果を表示するRawImage</summary>
        [SerializeField]
        private RawImage targetRawImage;

        /// <summary>カメラの表示範囲（Orthographic Size）</summary>
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
        /// <summary>ビジュアライゼーション空間のオフセット（他のシーンオブジェクトと干渉しないようにする）</summary>
        private static readonly Vector3 WorldOffset = new Vector3(0f, 100f, 0f);

        /// <summary>ビジュアライゼーション要素を配置する親Transformを取得する</summary>
        public Transform VisualRoot => visualRoot;

        /// <summary>カメラの表示範囲を取得する</summary>
        public float CameraSize => cameraSize;

        private void Awake()
        {
            SetupVisualRoot();
            SetupCamera();
            SetupRenderTexture();
        }

        /// <summary>
        /// ビジュアライゼーション空間のルートオブジェクトを作成する
        /// </summary>
        private void SetupVisualRoot()
        {
            var rootGo = new GameObject("VisualizationRoot");
            rootGo.transform.position = WorldOffset;
            visualRoot = rootGo.transform;
        }

        /// <summary>
        /// レンダリング用のOrthographicカメラを作成する
        /// </summary>
        private void SetupCamera()
        {
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
            // メインカメラとして扱わないようにする
            renderCamera.depth = -10;
            renderCamera.tag = "Untagged";
        }

        /// <summary>
        /// RenderTextureを作成し、カメラとRawImageに接続する
        /// </summary>
        private void SetupRenderTexture()
        {
            renderTexture = new RenderTexture(TextureWidth, TextureHeight, 16);
            renderTexture.antiAliasing = 2;
            renderCamera.targetTexture = renderTexture;

            if (targetRawImage != null)
            {
                targetRawImage.texture = renderTexture;
            }
        }

        /// <summary>
        /// ビジュアライゼーション空間内の全オブジェクトを削除する
        /// </summary>
        public void ClearAll()
        {
            if (visualRoot == null)
            {
                return;
            }
            for (int i = visualRoot.childCount - 1; i >= 0; i--)
            {
                var child = visualRoot.GetChild(i);
                // カメラは削除しない
                if (child.GetComponent<Camera>() != null)
                {
                    continue;
                }
                Destroy(child.gameObject);
            }
        }

        private void OnDestroy()
        {
            if (renderTexture != null)
            {
                renderTexture.Release();
                Destroy(renderTexture);
            }
            if (visualRoot != null)
            {
                Destroy(visualRoot.gameObject);
            }
        }
    }
}
