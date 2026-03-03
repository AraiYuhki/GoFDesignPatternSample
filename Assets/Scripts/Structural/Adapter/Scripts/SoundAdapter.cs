using System.Collections.Generic;

namespace DesignPatterns.Structural.Adapter
{
    /// <summary>
    /// 旧式サウンドシステムを新しいインターフェースに適合させるアダプター（Adapter）
    ///
    /// 【Adapterパターンにおける役割】
    /// TargetインターフェースとAdapteeの間を仲介する
    /// クライアントはIModernAudioを通じて旧システムを操作できる
    /// </summary>
    public sealed class SoundAdapter : IModernAudio
    {
        /// <summary>ラップする旧システム</summary>
        private readonly LegacySoundSystem legacySystem;

        /// <summary>トラック名からサウンドIDへのマッピング</summary>
        private readonly Dictionary<string, int> trackMapping;

        /// <summary>現在のトラック名</summary>
        private string currentTrack;

        /// <summary>
        /// アダプターを生成する
        /// </summary>
        /// <param name="legacySystem">ラップする旧システム</param>
        public SoundAdapter(LegacySoundSystem legacySystem)
        {
            this.legacySystem = legacySystem;
            trackMapping = new Dictionary<string, int> {
                { "BGM_Battle", 1 },
                { "BGM_Field", 2 },
                { "SE_Attack", 10 },
                { "SE_Heal", 11 }
            };
        }

        /// <inheritdoc/>
        public void Play(string trackName, float volume)
        {
            InGameLogger.Log($"[アダプター] Play(\"{trackName}\", {volume:F1}) を変換中...", LogColor.Green);

            int soundId = 0;
            if (trackMapping.ContainsKey(trackName))
            {
                soundId = trackMapping[trackName];
            }

            int volumePercent = (int)(volume * 100);
            currentTrack = trackName;
            legacySystem.PlaySound(soundId, volumePercent);
        }

        /// <inheritdoc/>
        public void Stop()
        {
            InGameLogger.Log("[アダプター] Stop() を変換中...", LogColor.Green);
            legacySystem.StopSound();
            currentTrack = null;
        }

        /// <inheritdoc/>
        public string GetStatus()
        {
            if (legacySystem.IsPlaying())
            {
                return $"再生中: {currentTrack} (ID: {legacySystem.GetCurrentSoundId()})";
            }
            return "停止中";
        }
    }
}
