//Libraries
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;
using App.Tools;

//Base class
namespace App.Game.Managers {
    /// <summary>
    /// 
    /// </summary>
    public sealed class PauseManager : MonoBehaviour {
        // TODO: Add quality of life to prevent unreferenced variables
        // TODO: Add multiple debug, warning and error handling conditionals
        // TODO: Verify local data structures are stored nested
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
        [SerializeField] private GameObject devEventSystem;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private GameObject menuButton;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private GameObject restartButton;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private GameObject confirmationPrompt;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private Slider masterChannelSlider;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private Slider musicChannelSlider;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private Slider uiChannelSlider;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private Slider sfxChannelSlider;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private Slider atmosphereChannelSlider;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private Slider voiceChannelSlider;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private Toggle masterChannelToggle;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private Toggle musicChannelToggle;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private Toggle uiChannelToggle;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private Toggle sfxChannelToggle;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private Toggle atmosphereChannelToggle;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private Toggle voiceChannelToggle;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private TMP_Dropdown qualityDropdown;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private TMP_Dropdown fpsDropdown;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private TMP_Dropdown localeDropdown;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private Button applyButton;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private Button localeButton;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private App.Tools.Data.SoundClip uiClickSounds;

        // * ATTRIBUTES
        [Header("Attributes")]
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private bool genQualityOptions = true;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private bool genFPSOptions = false;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private bool genLanguageOptions = true;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private bool nativeLanguageNaming = false;

        // * INTERNALS
        [Header("Internals")]
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private Coroutine userConfirmCoroutine;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        private App.Tools.Data.SettingsData cachedSettings;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        private App.Tools.Data.MenuAction nextAction = 0;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        private bool isActionVerified = false;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        private bool isActionAgreed = false;

    // ? BASE METHODS===============================================================================================================================
        /// <summary>
        /// 
        /// </summary>
        private void OnEnable() {
            App.Events.Settings.OnSettingsChanged += this.VerifyBeforeApply;
            App.Events.Settings.OnVolumesChanged += this.SilentLoadVolumes;
        }
        
        /// <summary>
        /// 
        /// </summary>
        private void OnDisable() {
            App.Events.Settings.OnSettingsChanged -= this.VerifyBeforeApply;
            App.Events.Settings.OnVolumesChanged -= this.SilentLoadVolumes;
        }

        /// <summary>
        /// 
        /// </summary>
        private void Awake() {
            #if UNITY_EDITOR
                // DEV: Forces single event system
                this.devEventSystem?.SetActive(false); //PlayerPrefs.GetInt("SettingsOpen", 0) == 1
            #endif

            if (App.Tools._SystemManager.GetAvailableLocales() == null) {
                if (this.localeDropdown) {
                    this.localeDropdown.gameObject.SetActive(false);
                    this.localeDropdown.interactable = App.Tools._SystemManager.GetAvailableLocales().Count > 0; 
                }

                if (this.localeButton) this.localeButton.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void Start() {
            this.Initialize();
        }

    // ? CUSTOM METHODS=============================================================================================================================
        /// <summary>
        /// 
        /// </summary>
        private void VerifyBeforeApply() => this.applyButton.interactable = this.CompareSettings();

        /// <summary>
        /// 
        /// </summary>
        private void CaptureInitialSettings() => this.cachedSettings = this.GetCurrentDisplayedSettings();
        
        /// <summary>
        /// 
        /// </summary>
        private void Initialize() {
            //Sync value updated by reference since lifespan is same as Manager
            // TODO: Update to persistent data requested to Saves Manager
            this.menuButton.SetActive(SceneManager.GetActiveScene().name == "Default");
            this.restartButton.SetActive(SceneManager.GetActiveScene().name == "Default");

            // Debug.Log();
            
            //Load settings default values
            this.LoadUnitySettingValues();
            
            //Saving loaded values as initial values
            this.CaptureInitialSettings();

            //Marking changes without apply
            this.VerifyBeforeApply();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CompareSettings() {
            App.Tools.Data.SettingsData displayed = this.GetCurrentDisplayedSettings();

            // TODO: Implement comparison Equal in SettingsData class to return value directly
            if (!App.Tools.Audio.ApproximatelyEqual(displayed.volumesSnapshot, this.cachedSettings.volumesSnapshot)) return true;

            if (displayed.qualityIndex != this.cachedSettings.qualityIndex) return true;

            if (displayed.localeIndex != this.cachedSettings.localeIndex) return true;

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadUnitySettingValues() {
            //Requesting current volume snapshot from Mixer to Audio Manager
            App.Tools.Data.VolumesSnapshot snapshot = App.Events.Audio.OnRequestVolumes?.Invoke() ?? App.Tools.Data.VolumesSnapshot.Default();
            
            // * Volumes
            //Setting default display value
            this.SilentLoadVolumes(snapshot);

            // * Quality
            //Requesting current quality preset cached in Saves Manager or setting defaults
            //Setting default display value

            // * FPS
            //Requesting current FPS limit cached in Saves Manager or setting defaults
            //Setting default display value

            // * Language
            //Requesting current available locales from Unity settings
            if (App.Tools._SystemManager.GetAvailableLocales() == null) {
                //Sync value updated by reference since lifespan is same as Manager
                this.localeDropdown.gameObject.SetActive(false);
            } else {
                this.PopulateLocaleOptions();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dropdown"></param>
        private void PopulateQualityOptions() {
            //Clearing options
            this.qualityDropdown.ClearOptions();
            
            //Populating dropdown
            List<string> options = new List<string>();
            foreach (string quality in App.Tools._SystemManager.GetAvailableQualityLevels()) options.Add(quality);
            this.qualityDropdown.AddOptions(options);

            //Set current quality
            this.qualityDropdown.SetValueWithoutNotify(App.Tools._SystemManager.GetSelectedQualityIndex());
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopulateLocaleOptions() {
            //Clearing options
            this.localeDropdown.ClearOptions();

            //Populating dropdown
            List<string> displayNames = new List<string>();
            foreach (UnityEngine.Localization.Locale locale in App.Tools._SystemManager.GetAvailableLocales()) displayNames.Add(locale.LocaleName);
            this.localeDropdown.AddOptions(displayNames);
        
            //Setting default display value
            this.localeDropdown.SetValueWithoutNotify(App.Tools._SystemManager.GetSelectedLocaleIndex());
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="volumeSnapshot"></param>
        private void SilentLoadVolumes(App.Tools.Data.VolumesSnapshot volumeSnapshot) {
            this.masterChannelSlider.SetValueWithoutNotify(volumeSnapshot.masterVolume);
            this.musicChannelSlider.SetValueWithoutNotify(volumeSnapshot.musicVolume);
            this.uiChannelSlider.SetValueWithoutNotify(volumeSnapshot.uiVolume);
            this.sfxChannelSlider.SetValueWithoutNotify(volumeSnapshot.sfxVolume);
            this.atmosphereChannelSlider.SetValueWithoutNotify(volumeSnapshot.atmosphereVolume);
            this.voiceChannelSlider.SetValueWithoutNotify(volumeSnapshot.voiceVolume);

            this.masterChannelToggle.SetIsOnWithoutNotify(Mathf.Approximately(volumeSnapshot.masterVolume, 0.0f));
            this.musicChannelToggle.SetIsOnWithoutNotify(Mathf.Approximately(volumeSnapshot.musicVolume, 0.0f));
            this.uiChannelToggle.SetIsOnWithoutNotify(Mathf.Approximately(volumeSnapshot.uiVolume, 0.0f));
            this.sfxChannelToggle.SetIsOnWithoutNotify(Mathf.Approximately(volumeSnapshot.sfxVolume, 0.0f));
            this.atmosphereChannelToggle.SetIsOnWithoutNotify(Mathf.Approximately(volumeSnapshot.atmosphereVolume, 0.0f));
            this.voiceChannelToggle.SetIsOnWithoutNotify(Mathf.Approximately(volumeSnapshot.voiceVolume, 0.0f));
        }

        /// <summary>
        /// 
        /// </summary>       
        private void SilentLoadLocale(int localeIndex) {
            if (App.Tools._SystemManager.GetAvailableLocales() == null) return;

            this.localeDropdown.SetValueWithoutNotify(localeIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private App.Tools.Data.SettingsData GetCurrentDisplayedSettings() {
            App.Tools.Data.VolumesSnapshot snapshot = new App.Tools.Data.VolumesSnapshot {
                masterVolume = this.masterChannelSlider.value,
                musicVolume = this.musicChannelSlider.value,
                uiVolume = this.uiChannelSlider.value,
                sfxVolume = this.sfxChannelSlider.value,
                atmosphereVolume = this.atmosphereChannelSlider.value,
                voiceVolume = this.voiceChannelSlider.value
            };
            App.Tools.Data.SettingsData settings = new App.Tools.Data.SettingsData(snapshot, this.qualityDropdown.value, this.localeDropdown.value);

            return settings;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ResetFactorySettings() {
            if (DEBUG) Debug.Log("[PM] Factory settings reset");

            //Loading default values
            App.Tools.Data.VolumesSnapshot defaults = App.Tools.Data.VolumesSnapshot.Default();
            
            // * Volumes
            this.SilentLoadVolumes(defaults);
            App.Events.Audio.OnApplyVolumesSnapshot?.Invoke(this.GetCurrentDisplayedSettings().volumesSnapshot);

            // * Quality

            // * FPS
            // ! Which value is default FPS value?
            this.fpsDropdown.SetValueWithoutNotify(30);

            // * Language
            if (App.Tools._SystemManager.GetAvailableLocales() == null) {
                Debug.LogWarning("No stored locale or out of range, selecting default value. Are locale references missing?");
            } else {
                int defaultLocaleIndex = App.Tools._SystemManager.default_locale_index; 
                App.Tools._SystemManager.SetLocale(defaultLocaleIndex);

                this.localeDropdown.SetValueWithoutNotify(defaultLocaleIndex);
            }

            App.Events.Settings.OnSettingsReseted?.Invoke();
            App.Events.Settings.OnSettingsChanged?.Invoke();
        }

        /// <summary>
        /// 
        /// </summary>
        private void RestoreCachedSettings() {
            // * Restoring all volume channels configs from previous sessions or starter values
            this.SilentLoadVolumes(this.cachedSettings.volumesSnapshot);

            // * Restoring current quality preset configs from previous sessions or starter values

            // * Restoring current FPS limit configs from previous sessions or starter values
            
            // * Restoring current language configs from previous sessions or starter values
            this.SilentLoadLocale(this.cachedSettings.localeIndex);

            //Restoring values
            App.Tools.Data.SettingsData restoredSettings = this.GetCurrentDisplayedSettings();
            
            App.Events.Audio.OnApplyVolumesSnapshot?.Invoke(restoredSettings.volumesSnapshot);
            App.Tools._SystemManager.SetLocale(restoredSettings.localeIndex);

            this.VerifyBeforeApply();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitUserConfirmation() {
            this.isActionVerified = false;
            while (!this.isActionVerified) yield return null;
            
            //If exit without save was confirmed
            if (this.isActionAgreed) {
                switch (this.nextAction) {
                    //Close settings
                    default:
                    case App.Tools.Data.MenuAction.none:
                        this.RestoreCachedSettings();
                        App.Events.Settings.OnSettingsToggled?.Invoke();
                    break;
                    //Got to menu
                    case App.Tools.Data.MenuAction.menu:
                        this.RestoreCachedSettings();
                        PlayerPrefs.SetInt("InGame", 0);
                        App.Events.Settings.OnSettingsToggled?.Invoke();
                        App.Events.Application.OnNewGameSession?.Invoke();
                    break;
                    //Retry level
                    case App.Tools.Data.MenuAction.retry:
                        this.RestoreCachedSettings();
                        App.Events.Settings.OnSettingsToggled?.Invoke();
                        App.Events.Application.OnNewGameSession?.Invoke();
                    break;
                    //Factory reset
                    case App.Tools.Data.MenuAction.defaults:
                        this.ResetFactorySettings();
                    break;
                }
            }
            
            if (this.userConfirmCoroutine != null) {
                this.StopCoroutine(this.userConfirmCoroutine);
                this.userConfirmCoroutine = null;
            }
        }

    // ? EVENT METHODS==============================================================================================================================
        /// <summary>
        /// 
        /// </summary>
        public void OnUpdateQualityIndex() {
            //Verify quality index is in range

            int index = this.qualityDropdown.value;

            //Set quality level
            
            App.Events.Settings.OnQualityUpdated?.Invoke();
            App.Events.Settings.OnSettingsChanged?.Invoke();
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnUpdateLocaleIndex() {
            //Verifying if was updated
            bool success = App.Tools._SystemManager.SetLocale(this.localeDropdown.value);
            if (!success) return;

            App.Events.Settings.OnLocaleUpdated?.Invoke();
            App.Events.Settings.OnSettingsChanged?.Invoke();
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void OnCycleLocale() {
            //Cycle locale using centralized logic
            App.Tools._SystemManager.CycleLocale();

            //Updating related dropdown
            this.localeDropdown.value = App.Tools._SystemManager.GetSelectedLocaleIndex();
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnUpdateMasterVolume() {
            App.Events.Audio.OnApplyChannelVolume(App.Tools.Data.MixerChannel.Master, this.masterChannelSlider.value);

            //Visual toggle update triggered from slider
            this.masterChannelToggle.SetIsOnWithoutNotify(Mathf.Approximately(this.masterChannelSlider.value, 0.0f));

            //Turn on
            if (Mathf.Approximately(this.masterChannelSlider.value, 0.0f)) {
                //Music
                if (this.musicChannelSlider.value > 0.0f) {
                    PlayerPrefs.SetInt("musicChangedByMaster", 1);
                    PlayerPrefs.SetFloat("previousMusicVolume", this.musicChannelSlider.value);
                    this.musicChannelToggle.isOn = true;
                }
                //UI
                if (this.uiChannelSlider.value > 0.0f) {
                    PlayerPrefs.SetInt("uiChangedByMaster", 1);
                    PlayerPrefs.SetFloat("previousUIVolume", this.uiChannelSlider.value);
                    this.uiChannelToggle.isOn = true;
                }
                //SFX
                if (this.sfxChannelSlider.value > 0.0f) {
                    PlayerPrefs.SetInt("sfxChangedByMaster", 1);
                    PlayerPrefs.SetFloat("previousSFXVolume", this.sfxChannelSlider.value);
                    this.sfxChannelToggle.isOn = true;
                }
                //Atmosphere
                if (this.atmosphereChannelSlider.value > 0.0f) {
                    PlayerPrefs.SetInt("atmosphereChangedByMaster", 1);
                    PlayerPrefs.SetFloat("previousAtmosphereVolume", this.atmosphereChannelSlider.value);
                    this.atmosphereChannelToggle.isOn = true;
                }
                //Voice
                if (this.voiceChannelSlider.value > 0.0f) {
                    PlayerPrefs.SetInt("voiceChangedByMaster", 1);
                    PlayerPrefs.SetFloat("previousVoiceVolume", this.voiceChannelSlider.value);
                    this.voiceChannelToggle.isOn = true;
                }
            //Turn off
            } else {
                //Music
                if (Mathf.Approximately(this.musicChannelSlider.value, 0.0f) && PlayerPrefs.GetInt("musicChangedByMaster") == 1) {
                    PlayerPrefs.SetInt("musicChangedByMaster", 0);
                    this.musicChannelToggle.isOn = false;
                }
                //UI
                if (Mathf.Approximately(this.uiChannelSlider.value, 0.0f) && PlayerPrefs.GetInt("uiChangedByMaster") == 1) {
                    PlayerPrefs.SetInt("uiChangedByMaster", 0);
                    this.uiChannelToggle.isOn = false;
                }
                //SFX
                if (Mathf.Approximately(this.sfxChannelSlider.value, 0.0f) && PlayerPrefs.GetInt("sfxChangedByMaster") == 1) {
                    PlayerPrefs.SetInt("sfxChangedByMaster", 0);
                    this.sfxChannelToggle.isOn = false;
                }
                //Atmosphere
                if (Mathf.Approximately(this.atmosphereChannelSlider.value, 0.0f) && PlayerPrefs.GetInt("atmosphereChangedByMaster") == 1) {
                    PlayerPrefs.SetInt("atmosphereChangedByMaster", 0);
                    this.atmosphereChannelToggle.isOn = false;
                }
                // Voice
                if (Mathf.Approximately(this.voiceChannelSlider.value, 0.0f) && PlayerPrefs.GetInt("voiceChangedByMaster") == 1) {
                    PlayerPrefs.SetInt("voiceChangedByMaster", 0);
                    this.voiceChannelToggle.isOn = false;
                }
            }

            App.Events.Settings.OnMasterVolumeChanged?.Invoke(this.masterChannelSlider.value);
            App.Events.Settings.OnSettingsChanged?.Invoke();
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnToggleMasterVolume() {
            //Turn on
            if (Mathf.Approximately(this.masterChannelSlider.value, 0.0f)) {
                this.masterChannelSlider.value = PlayerPrefs.GetFloat("previousMasterVolume", 0.5f);
            //Turn off
            } else {
                PlayerPrefs.SetFloat("previousMasterVolume", this.masterChannelSlider.value);
                this.masterChannelSlider.value = 0.0f;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnUpdateMusicVolume() {
            App.Events.Audio.OnApplyChannelVolume?.Invoke(App.Tools.Data.MixerChannel.Music, this.musicChannelSlider.value);
            
            //Visual toggle update triggered from slider
            this.musicChannelToggle.SetIsOnWithoutNotify(Mathf.Approximately(this.musicChannelSlider.value, 0.0f));

            //Restoring master channel volume
            if (this.musicChannelSlider.value > 0.0f && Mathf.Approximately(this.masterChannelSlider.value, 0.0f)) {
                // TODO: Add toggle channel dependency
                this.masterChannelToggle.isOn = false;
            }

            App.Events.Settings.OnMusicVolumeChanged?.Invoke(this.musicChannelSlider.value);
            App.Events.Settings.OnSettingsChanged?.Invoke();
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnToggleMusicVolume() {
            //Turn on
            if (Mathf.Approximately(this.musicChannelSlider.value, 0.0f)) {
                this.musicChannelSlider.value = PlayerPrefs.GetFloat("previousMusicVolume", 0.5f);

                //Restoring master channel volume
                // TODO: Add toggle channel dependency
                if (Mathf.Approximately(this.masterChannelSlider.value, 0.0f)) this.masterChannelToggle.isOn = false;
            //Turn off
            } else {
                PlayerPrefs.SetFloat("previousMusicVolume", this.musicChannelSlider.value);
                this.musicChannelSlider.value = 0.0f;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void OnUpdateUIVolume() {
            App.Events.Audio.OnApplyChannelVolume(App.Tools.Data.MixerChannel.UI, this.uiChannelSlider.value);

            //Visual toggle update triggered from slider
            this.uiChannelToggle.SetIsOnWithoutNotify(Mathf.Approximately(this.uiChannelSlider.value, 0.0f));

            //Restoring master channel volume
            if (this.uiChannelSlider.value > 0.0f && Mathf.Approximately(this.masterChannelSlider.value, 0.0f)) {
                // TODO: Add toggle channel dependency
                this.masterChannelToggle.isOn = false;
            }

            App.Events.Settings.OnUIVolumeChanged?.Invoke(this.uiChannelSlider.value);
            App.Events.Settings.OnSettingsChanged?.Invoke();
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnToggleUIVolume() {
            //Turn on
            if (Mathf.Approximately(this.uiChannelSlider.value, 0.0f)) {
                this.uiChannelSlider.value = PlayerPrefs.GetFloat("previousUIVolume", 0.5f);

                //Restoring master channel volume
                // TODO: Add toggle channel dependency
                if (Mathf.Approximately(this.masterChannelSlider.value, 0.0f)) this.masterChannelToggle.isOn = false;
            //Turn off
            } else {
                PlayerPrefs.SetFloat("previousUIVolume", this.uiChannelSlider.value);
                this.uiChannelSlider.value = 0.0f;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnUpdateSFXVolume() {
            App.Events.Audio.OnApplyChannelVolume(App.Tools.Data.MixerChannel.SFX, this.sfxChannelSlider.value);
            
            //Visual toggle update triggered from slider
            this.sfxChannelToggle.SetIsOnWithoutNotify(Mathf.Approximately(this.sfxChannelSlider.value, 0.0f));

            //Restoring master channel volume
            if (this.sfxChannelSlider.value > 0.0f && Mathf.Approximately(this.masterChannelSlider.value, 0.0f)) {
                // TODO: Add toggle channel dependency
                this.masterChannelToggle.isOn = false;
            }

            App.Events.Settings.OnSFXVolumeUpdated?.Invoke(this.sfxChannelSlider.value);
            App.Events.Settings.OnSettingsChanged?.Invoke();
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnToggleSFXVolume() {
            //Turn on
            if (Mathf.Approximately(this.sfxChannelSlider.value, 0.0f)) {
                this.sfxChannelSlider.value = PlayerPrefs.GetFloat("previousSFXVolume", 0.5f);
                
                //Restoring master channel volume
                // TODO: Add toggle channel dependency
                if (Mathf.Approximately(this.masterChannelSlider.value, 0.0f)) this.masterChannelToggle.isOn = false;
            //Turn off
            } else {
                PlayerPrefs.SetFloat("previousSFXVolume", this.sfxChannelSlider.value);
                this.sfxChannelSlider.value = 0.0f;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnUpdateAtmosphereVolume() {
            App.Events.Audio.OnApplyChannelVolume(App.Tools.Data.MixerChannel.Atmosphere, this.atmosphereChannelSlider.value);

            //Visual toggle update triggered from slider
            this.atmosphereChannelToggle.SetIsOnWithoutNotify(Mathf.Approximately(this.atmosphereChannelSlider.value, 0.0f));

            //Restoring master channel volume
            if (this.atmosphereChannelSlider.value > 0.0f && Mathf.Approximately(this.masterChannelSlider.value, 0.0f)) {
                // TODO: Add toggle channel dependency
                this.masterChannelToggle.isOn = false;
            }

            App.Events.Settings.OnAtmosphereVolumeChanged?.Invoke(this.atmosphereChannelSlider.value);
            App.Events.Settings.OnSettingsChanged?.Invoke();
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnToggleAtmosphereVolume() {
            //Turn on
            if (Mathf.Approximately(this.atmosphereChannelSlider.value, 0.0f)) {
                this.atmosphereChannelSlider.value = PlayerPrefs.GetFloat("previousAtmosphereVolume", 0.5f);
                
                //Restoring master channel volume
                // TODO: Add toggle channel dependency
                if (Mathf.Approximately(this.masterChannelSlider.value, 0.0f)) this.masterChannelToggle.isOn = false;
            //Turn off
            } else {
                PlayerPrefs.SetFloat("previousAtmosphereVolume", this.atmosphereChannelSlider.value);
                this.atmosphereChannelSlider.value = 0.0f;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnUpdateVoiceVolume() {
            App.Events.Audio.OnApplyChannelVolume(App.Tools.Data.MixerChannel.Voice, this.voiceChannelSlider.value);

            //Visual toggle update triggered from slider
            this.voiceChannelToggle.SetIsOnWithoutNotify(Mathf.Approximately(this.voiceChannelSlider.value, 0.0f));

            //Restoring master channel volume
            if (this.voiceChannelSlider.value > 0.0f && Mathf.Approximately(this.masterChannelSlider.value, 0.0f)) {
                // TODO: Add toggle channel dependency
                this.masterChannelToggle.isOn = false;
            }

            App.Events.Settings.OnVoiceVolumeChanged?.Invoke(this.voiceChannelSlider.value);
            App.Events.Settings.OnSettingsChanged?.Invoke();
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnToggleVoiceVolume() {
            //Turn on
            if (Mathf.Approximately(this.voiceChannelSlider.value, 0.0f)) {
                this.voiceChannelSlider.value = PlayerPrefs.GetFloat("previousVoiceVolume", 0.5f);
                
                //Restoring master channel volume
                // TODO: Add toggle channel dependency
                if (Mathf.Approximately(this.masterChannelSlider.value, 0.0f)) this.masterChannelToggle.isOn = false;
            //Turn off
            } else {
                PlayerPrefs.SetFloat("previousVoiceVolume", this.voiceChannelSlider.value);
                this.voiceChannelSlider.value = 0.0f;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnApplySettings() {
            if (DEBUG) Debug.Log("Apply button pressed");

            //Saving settings
            App.Events.Settings.OnSettingsSaved?.Invoke();

            //Setting new cached
            this.CaptureInitialSettings();

            //Updating apply button
            this.VerifyBeforeApply();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionIndex"></param>
        public void OnRequestAction(int actionIndex) {
            // TODO: Handle user requesting action not listed in enum
            //Cast since Unity UI system standard UI event calls doesn't support enums
            this.nextAction = (App.Tools.Data.MenuAction)actionIndex;
            
            if (this.nextAction == App.Tools.Data.MenuAction.defaults ? true : this.CompareSettings()) {
                this.confirmationPrompt.SetActive(true);
                
                if (this.userConfirmCoroutine != null) this.StopCoroutine(this.userConfirmCoroutine);
                this.userConfirmCoroutine = this.StartCoroutine(this.WaitUserConfirmation());
            } else if(this.nextAction == App.Tools.Data.MenuAction.menu) {
                App.Events.Application.OnMenuLoad?.Invoke();
            } else App.Events.Settings.OnSettingsToggled?.Invoke();
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnActionAgree() {
            this.isActionVerified = true;
            this.isActionAgreed = true;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void OnActionDecline() {
            this.isActionVerified = true;
            this.isActionAgreed = false;

            //Reseting for later use
            this.nextAction = App.Tools.Data.MenuAction.none;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void OnPlayUISound() {
            if (this.uiClickSounds) {
                if (this.uiClickSounds.Scope != App.Tools.Data.PlaybackScope.Universal) Debug.LogWarning("Is UI sound clip data marked as Universal scope to prevent automatic stop caused by deletion when unloading scene?");
                App.Events.Audio.OnPlayClipUniversally?.Invoke(this.uiClickSounds);
            }
        }
    }
}

namespace App.Tools.Data {
    /// <summary>
    /// 
    /// </summary>
    public enum MenuAction {
        none,
        menu,
        retry,
        defaults
    }
}