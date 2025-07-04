//Libraries
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

//Base class
namespace App.Game.Managers {
    public class _PauseManager : MonoBehaviour {
    // ? DEBUG======================================================================================================================================
    [SerializeField] private bool DEBUG = false;

    // ? PARAMETERS=================================================================================================================================
        // * RERERENCES
        [SerializeField] private _RouterManager RouterManager;
        [SerializeField] private GameObject localEventSystem;
        [SerializeField] private GameObject mainMenuButton;
        [SerializeField] private GameObject restartGameButton;
        [SerializeField] private GameObject confirmationPromt;
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider uiVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private Slider atmosphereVolumeSlider;
        [SerializeField] private Slider voiceVolumeSlider;
        [SerializeField] private Toggle masterVolumeButton;
        [SerializeField] private Toggle musicVolumeButton;
        [SerializeField] private Toggle uiVolumeButton;
        [SerializeField] private Toggle sfxVolumeButton;
        [SerializeField] private Toggle atmosphereVolumeButton;
        [SerializeField] private Toggle voiceVolumeButton;
        [SerializeField] private Button applyButton;
        [SerializeField] private TMP_Dropdown qualityDropdown;
        [SerializeField] private TMP_Dropdown languageDropdown;
        [SerializeField] private Coroutine userConfirmCoroutine;

        // * INTERNAL
        [SerializeField] private MenuAction nextAction = 0;
        [SerializeField] private bool isActionConfirmed = false;

        // ! temp
        // NUEVO: Almacena los valores de inicio para saber si fueron modificados
        private float initialMasterVolume;
        private float initialMusicVolume;
        private float initialUIVolume;
        private float initialSFXVolume;
        private float initialAtmosphereVolume;
        private float initialVoiceVolume;
        // TODO: Move UI related updates to UIManager for handling it
        // TODO: Add events to update signals among visual updaters

        // TODO: Add quality of life to prevent unreferenced variables
        // TODO: Add multiple debug, warning and error handling conditionals

        // TODO: Use Mathf to approximatelly 0 results
        // TODO: Implement logaritmic adjustment to sliders from 0-1 values
        // TODO: Update scene references to use enum 
        // ? BASE METHODS===============================================================================================================================
        private void OnEnable() {
            Events.Settings.OnSettingsChanged += this.ChangesMade;
        }
        
        private void OnDisable() {
            Events.Settings.OnSettingsChanged -= this.ChangesMade;
        }

        private void Awake() {
            this.RouterManager = GameObject.Find("Managers").GetComponent<_RouterManager>();   
        }

        private void Start() {
            this.InitialUpdate(); // ! TEMP
            this.LoadPreviousSettings();
        }

    // ? CUSTOM METHODS=============================================================================================================================
        private void ChangesMade() {
            this.applyButton.interactable = this.SettingsChanged();
        }

        private void ResetFactorySettings() {
            //Loading all factory volume config
            this.masterVolumeSlider.value = 0.5f;
            this.musicVolumeSlider.value = 0.5f;
            this.uiVolumeSlider.value = 0.5f;
            this.sfxVolumeSlider.value = 0.5f;
            this.atmosphereVolumeSlider.value = 0.5f;
            this.voiceVolumeSlider.value = 0.5f;

            
            PlayerPrefs.SetFloat("previousMasterVolume", this.masterVolumeSlider.value);
            PlayerPrefs.SetFloat("previousMusicVolume", this.musicVolumeSlider.value);
            PlayerPrefs.SetFloat("previousUIVolume", this.uiVolumeSlider.value);
            PlayerPrefs.SetFloat("previousSFXVolume", this.sfxVolumeSlider.value);
            PlayerPrefs.SetFloat("previousAtmosphereVolume", this.atmosphereVolumeSlider.value);
            PlayerPrefs.SetFloat("previousVoiceVolume", this.voiceVolumeSlider.value);

            // TODO: Loading all factory accesibility config
            //qualityDropdown.value = PlayerPrefs.GetInt("qualityIndex", 0);
            //languageDropdown.value = PlayerPrefs.GetInt("languageIndex", 0);
        }

        private void LoadPreviousSettings() {
            //Loading all volume channels configs from previous sessions
            this.masterVolumeSlider.value = PlayerPrefs.GetFloat("previousMasterVolume", 0.5f);
            this.musicVolumeSlider.value = PlayerPrefs.GetFloat("previousMusicVolume", 0.5f);
            this.sfxVolumeSlider.value = PlayerPrefs.GetFloat("previousSFXVolume", 0.5f);
            this.uiVolumeSlider.value = PlayerPrefs.GetFloat("previousUIVolume", 0.5f);
            this.atmosphereVolumeSlider.value = PlayerPrefs.GetFloat("previousAtmosphereVolume", 0.5f);
            this.voiceVolumeSlider.value = PlayerPrefs.GetFloat("previousVoiceVolume", 0.5f);

            // TODO: Loading all accesibility configs from previous settings
            //qualityDropdown.value = PlayerPrefs.GetInt("qualityIndex", 0);
            //languageDropdown.value = PlayerPrefs.GetInt("languageIndex", 0);

            this.SaveNewDefaults();
        }

        private void SaveNewDefaults() {
            this.initialMasterVolume = this.masterVolumeSlider.value;
            this.initialMusicVolume = this.musicVolumeSlider.value;
            this.initialSFXVolume = this.sfxVolumeSlider.value;
            this.initialUIVolume = this.uiVolumeSlider.value;
            this.initialAtmosphereVolume = this.atmosphereVolumeSlider.value;
            this.initialVoiceVolume = this.voiceVolumeSlider.value;
            
            this.ChangesMade();
        }
        
        private void InitialUpdate() {
            this.localEventSystem.SetActive(PlayerPrefs.GetInt("SettingsOpen", 0) == 0); // * DEV: Forces single event system

            this.mainMenuButton.SetActive(PlayerPrefs.GetInt("InGame", 0) == 1);
            this.restartGameButton.SetActive(PlayerPrefs.GetInt("InGame", 0) == 1);
            this.confirmationPromt.SetActive(false);
        }
        
        private bool SettingsChanged() {
            return !(
                Mathf.Approximately(this.masterVolumeSlider.value, this.initialMasterVolume) &&
                Mathf.Approximately(this.musicVolumeSlider.value, this.initialMusicVolume) &&
                Mathf.Approximately(this.sfxVolumeSlider.value, this.initialSFXVolume) &&
                Mathf.Approximately(this.uiVolumeSlider.value, this.initialUIVolume) &&
                Mathf.Approximately(this.atmosphereVolumeSlider.value, this.initialAtmosphereVolume) &&
                Mathf.Approximately(this.voiceVolumeSlider.value, this.initialVoiceVolume)
            );
        }

        private void ApplySettings() {
            this.SaveNewDefaults();

            PlayerPrefs.Save();
        }
        
        private bool NeedsConfirmation(MenuAction action) {
            switch (action) {
                case MenuAction.menu:
                case MenuAction.retry:
                    return true;
                case MenuAction.defaults:
                case MenuAction.none:
                    return this.SettingsChanged();
                default:
                    return false;
            }
        }

        private IEnumerator WaitUserConfirmation() {
            while (!this.isActionConfirmed) yield return null;
            
            switch (this.nextAction) {
                default:
                case MenuAction.none:
                    this.LoadPreviousSettings();
                    this.RouterManager.ToggleSettings();
                break;
                case MenuAction.menu:
                    this.LoadPreviousSettings();
                    this.RouterManager.ToggleSettings();
                    if (this.nextAction == MenuAction.menu) PlayerPrefs.SetInt("InGame", 0);
                    this.RouterManager.ChangeScene(this.RouterManager.menuLoaderSceneBuildIndex);
                break;
                case MenuAction.retry:
                    this.LoadPreviousSettings();
                    this.RouterManager.ToggleSettings();
                    this.RouterManager.GameStart();
                break;
                case MenuAction.defaults:
                    this.ResetFactorySettings();
                break;
            }
            
            Time.timeScale = 1.0f;
            
            if (this.userConfirmCoroutine != null) {
                this.StopCoroutine(this.userConfirmCoroutine);
                this.userConfirmCoroutine = null;
            }
        }

    // ? EVENT METHODS==============================================================================================================================
        private void UpdateQualityIndex() {
            // TODO: Modify to allow real quality update dynamically
            QualitySettings.SetQualityLevel(this.qualityDropdown.value);
            PlayerPrefs.SetInt("qualityIndex", this.qualityDropdown.value);
            
            Events.Settings.OnQualityUpdated?.Invoke();
            Events.Settings.OnSettingsChanged?.Invoke();
        }

        private void UpdateLanguageIndex() {
            // TODO: Modify to allow complete integration with Localization plugin
            PlayerPrefs.SetInt("languageIndex", this.languageDropdown.value);

            Events.Settings.OnLanguageUpdated?.Invoke();
            Events.Settings.OnSettingsChanged?.Invoke();
        }

        public void UpdateMasterVolume() {
            PlayerPrefs.SetFloat("masterVolume", this.masterVolumeSlider.value);

            if (this.masterVolumeSlider.value == 0.0f) {
                // Music
                if (this.musicVolumeSlider.value > 0.0f) {
                    PlayerPrefs.SetInt("musicChangedByMaster", 1);
                    PlayerPrefs.SetFloat("previousMusicVolume", this.musicVolumeSlider.value);
                }
                // UI
                if (this.uiVolumeSlider.value > 0.0f) {
                    PlayerPrefs.SetInt("uiChangedByMaster", 1);
                    PlayerPrefs.SetFloat("previousUIVolume", this.uiVolumeSlider.value);
                }
                // SFX
                if (this.sfxVolumeSlider.value > 0.0f) {
                    PlayerPrefs.SetInt("sfxChangedByMaster", 1);
                    PlayerPrefs.SetFloat("previousSFXVolume", this.sfxVolumeSlider.value);
                }
                // Atmosphere
                if (this.atmosphereVolumeSlider.value > 0.0f) {
                    PlayerPrefs.SetInt("atmosphereChangedByMaster", 1);
                    PlayerPrefs.SetFloat("previousAtmosphereVolume", this.atmosphereVolumeSlider.value);
                }
                // Voice
                if (this.voiceVolumeSlider.value > 0.0f) {
                    PlayerPrefs.SetInt("voiceChangedByMaster", 1);
                    PlayerPrefs.SetFloat("previousVoiceVolume", this.voiceVolumeSlider.value);
                }
                
                //Forcing null channel volume
                this.musicVolumeSlider.value = 0.0f;
                this.uiVolumeSlider.value = 0.0f;
                this.musicVolumeSlider.value = 0.0f;
                this.atmosphereVolumeSlider.value = 0.0f;
                this.voiceVolumeSlider.value = 0.0f;
            } else {
                // Music
                if (musicVolumeSlider.value == 0 && PlayerPrefs.GetInt("musicChangedByMaster") == 1) {
                    PlayerPrefs.SetInt("musicChangedByMaster", 0);
                    this.ToggleMusicVolume();
                }
                // UI
                if (this.uiVolumeSlider.value == 0 && PlayerPrefs.GetInt("uiChangedByMaster") == 1) {
                    PlayerPrefs.SetInt("uiChangedByMaster", 0);
                    this.ToggleUIVolume();
                }
                // SFX
                if (this.sfxVolumeSlider.value == 0 && PlayerPrefs.GetInt("sfxChangedByMaster") == 1) {
                    PlayerPrefs.SetInt("sfxChangedByMaster", 0);
                    this.ToggleSFXVolume();
                }
                // Atmosphere
                if (this.atmosphereVolumeSlider.value == 0 && PlayerPrefs.GetInt("atmosphereChangedByMaster") == 1) {
                    PlayerPrefs.SetInt("atmosphereChangedByMaster", 0);
                    this.ToggleAtmosphereVolume();
                }
                // Voice
                if (this.voiceVolumeSlider.value == 0 && PlayerPrefs.GetInt("voiceChangedByMaster") == 1) {
                    PlayerPrefs.SetInt("voiceChangedByMaster", 0);
                    this.ToggleVoiceVolume();
                }
            }

            Events.Settings.OnMasterVolumeUpdated?.Invoke(this.masterVolumeSlider.value);
            Events.Settings.OnSettingsChanged?.Invoke();
        }

        public void ToggleMasterVolume() {
            // Turn off
            if (this.masterVolumeSlider.value > 0.0f) {
                if (this.musicVolumeSlider.value > 0.0f) {
                    PlayerPrefs.SetInt("musicChangedByMaster", 1);
                    this.ToggleMusicVolume();
                }
                if (this.uiVolumeSlider.value > 0.0f) {
                    PlayerPrefs.SetInt("uiChangedByMaster", 1);
                    this.ToggleUIVolume();
                }
                if (this.sfxVolumeSlider.value > 0.0f) {
                    PlayerPrefs.SetInt("sfxChangedByMaster", 1);
                    this.ToggleSFXVolume();
                }
                if (this.atmosphereVolumeSlider.value > 0.0f) {
                    PlayerPrefs.SetInt("atmosphereChangedByMaster", 1);
                    this.ToggleAtmosphereVolume();
                }
                if (this.voiceVolumeSlider.value > 0.0f) {
                    PlayerPrefs.SetInt("voiceChangedByMaster", 1);
                    this.ToggleVoiceVolume();
                }
                
                PlayerPrefs.SetFloat("previousMasterVolume", this.masterVolumeSlider.value);
                this.masterVolumeSlider.value = 0.0f;
            // Turn on
            } else {
                if (this.musicVolumeSlider.value == 0.0f && PlayerPrefs.GetInt("musicChangedByMaster") == 1) {
                    PlayerPrefs.SetInt("musicChangedByMaster", 0);
                    this.ToggleMusicVolume();
                }
                if (this.uiVolumeSlider.value == 0.0f && PlayerPrefs.GetInt("uiuiChangedByMaster") == 1) {
                    PlayerPrefs.SetInt("uiChangedByMaster", 0);
                    this.ToggleUIVolume();
                }
                if (this.sfxVolumeSlider.value == 0.0f && PlayerPrefs.GetInt("sfxChangedByMaster") == 1) {
                    PlayerPrefs.SetInt("sfxChangedByMaster", 0);
                    this.ToggleSFXVolume();
                }
                if (this.atmosphereVolumeSlider.value == 0.0f && PlayerPrefs.GetInt("atmosphereChangedByMaster") == 1) {
                    PlayerPrefs.SetInt("atmosphereChangedByMaster", 0);
                    this.ToggleAtmosphereVolume();
                }
                if (this.voiceVolumeSlider.value == 0.0f && PlayerPrefs.GetInt("voiceChangedByMaster") == 1) {
                    PlayerPrefs.SetInt("voiceChangedByMaster", 0);
                    this.ToggleVoiceVolume();
                }

                this.masterVolumeSlider.value = PlayerPrefs.GetFloat("previousMasterVolume", 0.5f);
            }
        }

        public void UpdateMusicVolume() {
            PlayerPrefs.SetFloat("musicVolume", this.musicVolumeSlider.value);
            
            if (this.musicVolumeSlider.value > 0.0f && this.masterVolumeSlider.value == 0.0f) this.ToggleMasterVolume();

            Events.Settings.OnMusicVolumeUpdated?.Invoke(this.musicVolumeSlider.value);
            Events.Settings.OnSettingsChanged?.Invoke();
        }

        public void ToggleMusicVolume() {
            if (this.musicVolumeSlider.value > 0.0f) {
                PlayerPrefs.SetFloat("previousMusicVolume", this.musicVolumeSlider.value);
                this.musicVolumeSlider.value = 0.0f;
            } else {
                this.musicVolumeSlider.value = PlayerPrefs.GetFloat("previousMusicVolume", 0.5f);
                if (this.masterVolumeSlider.value == 0.0f) this.masterVolumeSlider.value = PlayerPrefs.GetFloat("previousMasterVolume", 0.5f);
            }
        }
        
        public void UpdateUIVolume() {
            PlayerPrefs.SetFloat("uiVolume", this.uiVolumeSlider.value);

            if (this.uiVolumeSlider.value > 0.0f && this.masterVolumeSlider.value == 0.0f) this.ToggleMasterVolume();

            Events.Settings.OnUIVolumeUpdated?.Invoke(this.uiVolumeSlider.value);
            Events.Settings.OnSettingsChanged?.Invoke();
        }

        public void ToggleUIVolume() {
            if (this.uiVolumeSlider.value > 0.0f) {
                PlayerPrefs.SetFloat("previousUIVolume", this.uiVolumeSlider.value);
                this.uiVolumeSlider.value = 0.0f;
            } else {
                this.uiVolumeSlider.value = PlayerPrefs.GetFloat("previousUIVolume", 0.5f);
                if (this.masterVolumeSlider.value == 0.0f) this.masterVolumeSlider.value = PlayerPrefs.GetFloat("previousMasterVolume", 0.5f);
            }
        }

        public void UpdateSFXVolume() {
            PlayerPrefs.SetFloat("sfxVolume", this.sfxVolumeSlider.value);
            
            if (this.sfxVolumeSlider.value > 0.0f && this.masterVolumeSlider.value == 0.0f) this.ToggleMasterVolume();

            Events.Settings.OnSFXVolumeUpdated?.Invoke(this.sfxVolumeSlider.value);
            Events.Settings.OnSettingsChanged?.Invoke();
        }

        public void ToggleSFXVolume() {
            if (this.sfxVolumeSlider.value > 0.0f) {
                PlayerPrefs.SetFloat("previousSFXVolume", this.sfxVolumeSlider.value);
                this.sfxVolumeSlider.value = 0.0f;
            } else {
                this.sfxVolumeSlider.value = PlayerPrefs.GetFloat("previousSFXVolume", 0.5f);
                if (this.masterVolumeSlider.value == 0.0f) this.masterVolumeSlider.value = PlayerPrefs.GetFloat("previousMasterVolume", 0.5f);
            }
        }

        public void UpdateAtmosphereVolume() {
            PlayerPrefs.SetFloat("atmosphereVolume", this.atmosphereVolumeSlider.value);

            if (this.atmosphereVolumeSlider.value > 0.0f && this.masterVolumeSlider.value == 0.0f) this.ToggleMasterVolume();

            Events.Settings.OnAtmosphereVolumeUpdated?.Invoke(this.atmosphereVolumeSlider.value);
            Events.Settings.OnSettingsChanged?.Invoke();
        }

        public void ToggleAtmosphereVolume() {
            if (this.atmosphereVolumeSlider.value > 0.0f) {
                PlayerPrefs.SetFloat("previousAtmosphereVolume", this.atmosphereVolumeSlider.value);
                this.atmosphereVolumeSlider.value = 0.0f;
            } else {
                this.atmosphereVolumeSlider.value = PlayerPrefs.GetFloat("previousAtmosphereVolume", 0.5f);
                if (this.masterVolumeSlider.value == 0.0f) this.masterVolumeSlider.value = PlayerPrefs.GetFloat("previousMasterVolume", 0.5f);
            }
        }

        public void UpdateVoiceVolume() {
            PlayerPrefs.SetFloat("voiceVolume", this.voiceVolumeSlider.value);

            if (this.voiceVolumeSlider.value > 0.0f && this.masterVolumeSlider.value == 0.0f) this.ToggleMasterVolume();

            Events.Settings.OnVoiceVolumeUpdated?.Invoke(this.voiceVolumeSlider.value);
            Events.Settings.OnSettingsChanged?.Invoke();
        }

        public void ToggleVoiceVolume() {
            if (this.voiceVolumeSlider.value > 0.0f) {
                PlayerPrefs.SetFloat("previousVoiceVolume", this.voiceVolumeSlider.value);
                this.voiceVolumeSlider.value = 0.0f;
            } else {
                this.voiceVolumeSlider.value = PlayerPrefs.GetFloat("previousVoiceVolume", 0.5f);
                if (this.masterVolumeSlider.value == 0.0f) this.masterVolumeSlider.value = PlayerPrefs.GetFloat("previousMasterVolume", 0.5f);
            }
        }

        public void ConfirmationPrompt(int actionIndex) {
            this.nextAction = (MenuAction)actionIndex;

            bool shouldShow = NeedsConfirmation(this.nextAction);
            this.confirmationPromt.SetActive(shouldShow);
            this.isActionConfirmed = !shouldShow;

            if (this.userConfirmCoroutine != null) this.StopCoroutine(this.userConfirmCoroutine);
            this.userConfirmCoroutine = this.StartCoroutine(this.WaitUserConfirmation());
        }

        public void CompleteAction() {
            this.isActionConfirmed = true;
        }

        public void DeclineAction() {
            this.nextAction = MenuAction.none;
            if (this.userConfirmCoroutine != null) this.StopCoroutine(this.userConfirmCoroutine);
        }
    }
}

public enum MenuAction {
    none,
    menu,
    retry,
    defaults
}