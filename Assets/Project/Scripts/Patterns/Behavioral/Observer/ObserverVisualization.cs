using UnityEngine;

namespace GoFPatterns.Patterns.Visualization {
    /// <summary>
    /// Observerパターンのビジュアライゼーション
    /// Subject（PlayerHealth）を中心にObserverを配置し、通知の流れを矢印で可視化する
    /// </summary>
    [PatternVisualization("observer")]
    public class ObserverVisualization : BasePatternVisualization {
        /// <summary>Subjectの配置位置</summary>
        private static readonly Vector2 SubjectPosition = new Vector2(-2f, 0f);
        /// <summary>HUDオブザーバーの配置位置</summary>
        private static readonly Vector2 HudPosition = new Vector2(3f, 3f);
        /// <summary>Soundオブザーバーの配置位置</summary>
        private static readonly Vector2 SoundPosition = new Vector2(3f, -3f);
        /// <summary>Achievementオブザーバーの配置位置</summary>
        private static readonly Vector2 AchievementPosition = new Vector2(4.5f, 0f);
        /// <summary>Subjectの半径</summary>
        private const float SubjectRadius = 1.2f;
        /// <summary>Observerの半径</summary>
        private const float ObserverRadius = 0.8f;
        /// <summary>Subjectの色</summary>
        private static readonly Color SubjectColor = new Color(0.3f, 0.5f, 0.8f, 1f);
        /// <summary>Observerの色</summary>
        private static readonly Color ObserverColor = new Color(0.4f, 0.7f, 0.5f, 1f);

        /// <summary>
        /// バインド時にSubjectとObserver要素を配置して初期表示を構築する
        /// </summary>
        /// <param name="demo">バインドされたデモ</param>
        protected override void OnBind(IPatternDemo demo) {
            AddCircle("subject", "PlayerHealth", SubjectPosition, SubjectRadius, SubjectColor);

            VisualElement hud = AddCircle("hud", "HUD", HudPosition, ObserverRadius, DimColor);
            VisualElement sound = AddCircle("sound", "Sound", SoundPosition, ObserverRadius, DimColor);
            VisualElement achievement = AddCircle("achievement", "Achievement", AchievementPosition, ObserverRadius, DimColor);

            hud.SetVisible(false);
            sound.SetVisible(false);
            achievement.SetVisible(false);
        }

        /// <summary>
        /// ステップに応じてObserverの登録・通知・解除のアニメーションを更新する
        /// </summary>
        /// <param name="stepIndex">現在のステップインデックス</param>
        protected override void OnRefresh(int stepIndex) {
            VisualElement subject = GetElement("subject");
            VisualElement hud = GetElement("hud");
            VisualElement sound = GetElement("sound");
            VisualElement achievement = GetElement("achievement");

            switch (stepIndex) {
                case 0:
                    hud.SetVisible(true);
                    hud.SetColorImmediate(ObserverColor);
                    hud.Pulse(HighlightColor, 0.5f);
                    AddArrow("subject-hud", subject, hud, ArrowColor);
                    break;
                case 1:
                    sound.SetVisible(true);
                    sound.SetColorImmediate(ObserverColor);
                    sound.Pulse(HighlightColor, 0.5f);
                    AddArrow("subject-sound", subject, sound, ArrowColor);
                    break;
                case 2:
                    achievement.SetVisible(true);
                    achievement.SetColorImmediate(ObserverColor);
                    achievement.Pulse(HighlightColor, 0.5f);
                    AddArrow("subject-achievement", subject, achievement, ArrowColor);
                    break;
                case 3:
                    subject.Pulse(PulseColor, 0.5f);
                    GetArrow("subject-hud")?.Pulse(PulseColor, 0.5f);
                    GetArrow("subject-sound")?.Pulse(PulseColor, 0.5f);
                    GetArrow("subject-achievement")?.Pulse(PulseColor, 0.5f);
                    hud.Pulse(PulseColor, 0.5f);
                    sound.Pulse(PulseColor, 0.5f);
                    achievement.Pulse(PulseColor, 0.5f);
                    break;
                case 4:
                    sound.SetColorImmediate(DimColor);
                    GetArrow("subject-sound")?.SetColor(DimColor);
                    break;
                case 5:
                    subject.Pulse(PulseColor, 0.5f);
                    GetArrow("subject-hud")?.Pulse(PulseColor, 0.5f);
                    GetArrow("subject-achievement")?.Pulse(PulseColor, 0.5f);
                    hud.Pulse(PulseColor, 0.5f);
                    achievement.Pulse(PulseColor, 0.5f);
                    break;
            }
        }
    }
}
