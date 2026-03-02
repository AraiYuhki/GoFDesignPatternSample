namespace DesignPatterns.Structural.Adapter {
    /// <summary>
    /// 旧式のサウンドシステム（Adaptee）
    /// 新しいインターフェースとは互換性のないメソッド名・引数を持つ
    ///
    /// 【Adapterパターンにおける役割】
    /// 既存の（変更できない）クラス
    /// アダプターがこのクラスをラップして新しいインターフェースに適合させる
    /// </summary>
    public sealed class LegacySoundSystem {
        /// <summary>現在再生中のサウンドID</summary>
        private int currentSoundId;

        /// <summary>再生中かどうか</summary>
        private bool isPlaying;

        /// <summary>
        /// サウンドIDを指定して再生を開始する
        /// </summary>
        /// <param name="soundId">サウンドID</param>
        /// <param name="volumePercent">音量（0〜100）</param>
        public void PlaySound(int soundId, int volumePercent) {
            currentSoundId = soundId;
            isPlaying = true;
            InGameLogger.Log(
                $"  [旧システム] PlaySound(id={soundId}, vol={volumePercent}%)",
                LogColor.White
            );
        }

        /// <summary>
        /// 再生を停止する
        /// </summary>
        public void StopSound() {
            isPlaying = false;
            InGameLogger.Log("  [旧システム] StopSound()", LogColor.White);
        }

        /// <summary>
        /// 再生中のサウンドIDを返す
        /// </summary>
        /// <returns>サウンドID（未再生時は-1）</returns>
        public int GetCurrentSoundId() {
            if (isPlaying) {
                return currentSoundId;
            }
            return -1;
        }

        /// <summary>
        /// 再生中かどうかを返す
        /// </summary>
        /// <returns>再生中ならtrue</returns>
        public bool IsPlaying() {
            return isPlaying;
        }
    }
}
