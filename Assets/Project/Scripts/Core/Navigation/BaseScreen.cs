using UnityEngine;

namespace GoFPatterns.Core {
    /// <summary>
    /// 全画面の基底クラス
    /// 表示・非表示・初期化の共通処理を提供する
    /// </summary>
    public abstract class BaseScreen : MonoBehaviour {
        /// <summary>画面の一意なID</summary>
        [SerializeField]
        private string screenId;

        /// <summary>画面IDを取得する</summary>
        public string ScreenId => screenId;

        /// <summary>
        /// 画面を表示する
        /// </summary>
        /// <param name="context">画面に渡すコンテキストデータ</param>
        public void Show(object context) {
            gameObject.SetActive(true);
            OnShow(context);
        }

        /// <summary>
        /// 画面を非表示にする
        /// </summary>
        public void Hide() {
            OnHide();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 画面表示時の処理（サブクラスでオーバーライド）
        /// </summary>
        /// <param name="context">コンテキストデータ</param>
        protected virtual void OnShow(object context) { }

        /// <summary>
        /// 画面非表示時の処理（サブクラスでオーバーライド）
        /// </summary>
        protected virtual void OnHide() { }
    }
}
