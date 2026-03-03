using UnityEngine;
using UnityEngine.UI;

namespace DesignPatterns
{
    /// <summary>
    /// メインメニューへ戻るボタンのコンポーネント
    /// 各デモシーンに配置して使用する
    /// </summary>
    [RequireComponent(typeof(Button))]
    public sealed class BackToMenuButton : MonoBehaviour
    {
        private void Start()
        {
            Button button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        /// <summary>
        /// ボタンクリック時にメインメニューへ遷移する
        /// </summary>
        private void OnClick()
        {
            SceneNavigator.ReturnToMainMenu();
        }
    }
}
