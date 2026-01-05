using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Text;

namespace App.Game.Narrative {
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class SubtitleUI : MonoBehaviour, App.Game.UI.IUpdatableUI {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI subtitleText;
        [SerializeField] private CanvasGroup blackScreen;
        [SerializeField] private CanvasGroup dialogArea;
        [SerializeField] private Image iconImage;

        Component App.Game.UI.IUpdatableUI.Target => this;
        public App.Tools.Data.SyncCategory SyncType => App.Tools.Data.SyncCategory.custom;
        public TextMeshProUGUI Target => subtitleText;
        public int ElementID => this.GetInstanceID();
        private Coroutine fadeRoutine;

        private void OnEnable() {
            App.Events.Narrative.OnSubtitleShow += this.SetSubtitleLine;
            App.Events.Narrative.OnSubtitleFadeIn += this.FadeInSubtitleLine;
            App.Events.Narrative.OnSubtitleFadeOut += this.FadeOutSubtitleLine;
            App.Events.Narrative.OnSubtitleHide += this.ClearSubtitleLine;
        }

        private void OnDisable() {
            App.Events.Narrative.OnSubtitleShow -= this.SetSubtitleLine;
            App.Events.Narrative.OnSubtitleFadeIn -= this.FadeInSubtitleLine;
            App.Events.Narrative.OnSubtitleFadeOut -= this.FadeOutSubtitleLine;
            App.Events.Narrative.OnSubtitleHide -= this.ClearSubtitleLine;
        }
        
        private void SetSubtitleLine(App.Tools.Data.BaseSubtitleData subtitle) {
            this.ResetFeatures();
            this.subtitleText.text = subtitle.author + ": " + subtitle.message;
            this.iconImage.sprite = subtitle.icon;
        }

        private void FadeInSubtitleLine(float duration) {
            this.Target.alpha = 0.0f;
            this.iconImage.enabled = true;
            this.DoFade(1, duration);
        }
        
        private void FadeOutSubtitleLine(float duration) {
            this.Target.alpha = 1.0f;
            this.DoFade(0, duration);
        }
        
        private void ClearSubtitleLine() {
            // subtitleText.text = "";
            this.iconImage.enabled = false;
        }
        
        private void ResetFeatures() {
            this.subtitleText.text = "";
            this.iconImage.sprite = null;
            this.subtitleText.alpha = 0.0f;
            this.iconImage.enabled = false;
        }
        
        private void DoFade(float endValue, float duration) {
            if (this.fadeRoutine != null) this.StopCoroutine(this.fadeRoutine);

            this.fadeRoutine = this.StartCoroutine(this.FadeRoutine(endValue, duration));
        }

        IEnumerator FadeRoutine(float target, float duration) {
            float start = this.Target.alpha;
            float elapsed = 0.0f;

            while (elapsed < duration) {
                elapsed += Time.deltaTime;
                this.Target.alpha = Mathf.Lerp(start, target, elapsed / duration);
                yield return null;
            }

            this.Target.alpha = target;
        }

        // ! Temporary making it centralized, but better applied if each UI component handles its own SyncUI and applies its own values
        public void SyncUI() {
            throw new System.NotImplementedException();
        }
    }  
}