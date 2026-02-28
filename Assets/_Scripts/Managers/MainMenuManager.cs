using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using App.Tools;

namespace App.Game.Managers {
    /// <summary>
    /// 
    /// </summary>
    public class MainMenuManager : MonoBehaviour {
    // ? DEBUG======================================================================================================================================
        //[Header("Debug")]
        /// <summary>
        /// 
        /// </summary>
        private static bool DEBUG => false;

    // ? PARAMETERS=================================================================================================================================
        // * REFERENCES
        [Header("References")]
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private App.Game.UI.UpdatableUIButton continueButton;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private Image flagIcon;
        
        // * ATTRIBUTES
        [Header("Attributes")]
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private App.Tools.Data.SoundClip uiClickSound;
        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private App.Tools.Data.SoundClip soundTrack;
        
        // * INTERNALS
        //[Header("Internals")]

    // ? BASE METHODS===============================================================================================================================
        /// <summary>
        /// 
        /// </summary>
        private void OnEnable() {
            // App.Events.Input.OnSettingsPress += this.OnSettingsToggle();
        }
        
        /// <summary>
        /// 
        /// </summary>
        private void OnDisable() {
            // App.Events.Input.OnSettingsPress -= this.OnSettingsToggle();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Awake() {
            //Ensuring AppLoader instance creation from any scene build index != 0
            #if UNITY_EDITOR
                App.Managers._AppManager.OnEnsureInstance();
            #endif

            // ! Must implement SyncUI to handle itself its activation
            this.flagIcon.gameObject.SetActive(_SystemManager.GetAvailableLocales() != null && _SystemManager.GetAvailableLocales().Count > 0);

            this.ValidateReferences();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Start() {
            this.Initialize();

            App.Events.Audio.OnPlayClipUniversally?.Invoke(soundTrack);
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnValidate() {
            this.ValidateReferences();
        }

    // ? CUSTOM METHODS=============================================================================================================================
        /// <summary>
        /// 
        /// </summary>
        private void ValidateReferences() {
            if (!this.continueButton) Debug.LogError("Reference to Continue button missing!");
        }

        /// <summary>
        /// 
        /// </summary>
        private void Initialize() {
            //Updating values savestate-dependant
            if (DEBUG) Debug.Log("Sync specific requested: " + this.continueButton.GetInstanceID());
            App.Events.UI.OnSyncElementRequested?.Invoke(App.Tools.Data.SyncCategory.button, this.continueButton.GetInstanceID());
        }

    // ? EVENT METHODS==============================================================================================================================
        // * AUDIO
        /// <summary>
        /// 
        /// </summary>
        public void PlayUISound() {
            if (this.uiClickSound != null) {    
                if (this.uiClickSound.Scope != App.Tools.Data.PlaybackScope.Universal) Debug.LogWarning("UI sound clip not scoped as Universal, this allows sudden stops. Is this intended?");
                App.Events.Audio.OnPlayClipUniversally?.Invoke(this.uiClickSound);
            }
        }

        // * NAVIGATION
        /// <summary>
        /// 
        /// </summary>
        public void OnResumeGame() {
            App.Events.Application.OnSessionResumed?.Invoke();
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void OnStartGame() {
            App.Events.Application.OnNewGameSession?.Invoke();
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnSettingsToggle() {
            App.Events.Settings.OnSettingsToggled?.Invoke();
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void OnQuitGame() {
            App.Events.Application.OnAppClosed?.Invoke();
        }
    
        // * SETTINGS (Optional)
        /// <summary>
        /// 
        /// </summary>
        public void OnCycleLocale() {
            //Cycle locale using centralized logic
            App.Events.Settings.OnLocaleCycled?.Invoke();

            //Save settings
            App.Events.Settings.OnSettingsSaved?.Invoke();
        }
    }
}