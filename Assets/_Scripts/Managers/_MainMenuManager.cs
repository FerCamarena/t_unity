using App.Game.Audio;
using App.Game.UI;
using UnityEngine;

namespace App.Game.Managers {
    public class _MainMenuManager : MonoBehaviour {
    // ? DEBUG======================================================================================================================================
    [SerializeField] private bool DEBUG = false;

    // ? PARAMETERS=================================================================================================================================
        // * REFERENCES
        
        // * ATTRIBUTES
        [SerializeField] private UpdatableUIButton continueButton;
        [SerializeField] private SingleSoundClipData uiSound;
        
        // * INTERNAL

    // ? BASE METHODS===============================================================================================================================
        private void Awake() {
            //Ensuring AppLoader instance creation from any scene build index != 0
            #if UNITY_EDITOR
                _AppManager.EnsureInstance();
            #endif

            this.ValidateReferences();
        }

        private void Start() {
            if (DEBUG) Debug.Log("Sync specific requested: " + this.continueButton.GetInstanceID());
            Events.UI.OnSyncElementRequested.Invoke(SyncType.button, this.continueButton.GetInstanceID());
        }

        private void OnValidate() {
            this.ValidateReferences();
        }

    // ? CUSTOM METHODS=============================================================================================================================
        private void ValidateReferences() {
            if (!this.continueButton) Debug.LogError("Reference missing: Continue Button!");
        }

    // ? EVENT METHODS==============================================================================================================================
        // * AUDIO
        //Generic method for playing UI sounds universally since still has no override pool nor needs custom sound per UI component instance or action
        //Usually could check if global or universal to handle local sounds by itself instead of sending it to AudioManager
        public void PlayUISound() {
            if (this.uiSound.Scope != PlaybackScope.Universal) Debug.LogWarning("Is UI sound clip data marked as Universal scope to prevent automatic stop caused by deletion when unloading scene?");
            Events.Audio.OnPlayClipUniversally.Invoke(this.uiSound);
        }

        // * NAVIGATION
        public void StartGame() {
            Events.Application.OnNewGameSession?.Invoke();
        }

        public void ResumeGame() {
            Events.Application.OnSessionResumed?.Invoke();
        }
        
        public void OpenSettings() {
            Events.Settings.OnSettingsToggled?.Invoke();
        }
        
        public void QuitGame() {
            Events.Application.OnAppClosed?.Invoke();
        }
    }
}