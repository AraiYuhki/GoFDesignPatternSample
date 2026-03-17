using System.Collections.Generic;

namespace GoFPatterns.Patterns {
    /// <summary>
    /// 全パターンデモが実装する共通インターフェース
    /// デモの再生制御・ステップ実行・ログ取得を統一的に扱う
    /// </summary>
    public interface IPatternDemo {
        /// <summary>パターンの一意なID</summary>
        string PatternId { get; }
        /// <summary>パターンの表示名</summary>
        string DisplayName { get; }
        /// <summary>デモを初期化する</summary>
        void Initialize();
        /// <summary>デモを再生する</summary>
        void Play();
        /// <summary>デモを一時停止する</summary>
        void Pause();
        /// <summary>一時停止から再開する</summary>
        void Resume();
        /// <summary>デモを停止する</summary>
        void Stop();
        /// <summary>デモを初期状態にリセットする</summary>
        void ResetDemo();
        /// <summary>ステップ実行が可能かどうか</summary>
        bool CanStep { get; }
        /// <summary>1ステップ進める</summary>
        void StepForward();
        /// <summary>現在のログ一覧を取得する</summary>
        IReadOnlyList<string> GetCurrentLogs();
        /// <summary>現在のステップインデックス</summary>
        int CurrentStepIndex { get; }
        /// <summary>全ステップ数</summary>
        int TotalSteps { get; }
    }
}
