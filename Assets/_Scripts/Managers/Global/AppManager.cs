using UnityEngine.InputSystem.Utilities;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;
using System;
using App.Tools;
using UnityEngine.UI;

namespace App.Managers {
    /// <summary>
    /// 
    /// </summary>
    public sealed class _AppManager : MonoBehaviour {
        // TODO: Add loading animation
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
        [SerializeField] public static GameObject appManagerPrefab;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private GameObject savesManagerPrefab;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private GameObject uiManagerPrefab;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private GameObject routerManagerPrefab;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private GameObject audioManagerPrefab;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private GameObject narrativeManagerPrefab;

        // * ATTRIBUTES
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        public static _AppManager Instance { get; private set; }
        [Header("Attributes")]
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private GameObject promptText;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private Button localeButton;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private float timeoutSeconds = 1.0f;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private bool inputTimeout = true;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private bool timeTimeout = false;
        
        // * INTERNALS
        [Header("Internals")]
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private SavesManager savesManager;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private UIManager uiManager;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private RouterManager routerManager;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private AudioManager audioManager;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private NarrativeManager narrativeManager;
        /// <summary>
        /// 
        /// </summary>
        // [Tooltip("")]
        // [SerializeField] private _InputManager inputManager;

    // ? BASE METHODS===============================================================================================================================
        
        /// <summary>
        /// 
        /// </summary>
        private void OnEnable() {
            App.Events.Settings.OnLocaleCycled += this.CycleLocale;
        }
        
        /// <summary>
        /// 
        /// </summary>
        private void OnDisable() {
            App.Events.Settings.OnLocaleCycled -= this.CycleLocale;
        }

        /// <summary>
        /// 
        /// </summary>
        private void Awake() {
            this.Boot();

            // ? Setting no vsync to apply max frame rate limits
            QualitySettings.vSyncCount = 0;

            //Custom warning for locale switching
            if (App.Tools._SystemManager.GetAvailableLocales() == null) Debug.LogWarning("[AL] Locale switching options disabled. Are locale references missing?");

            this.ValidateReferences();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Start() {
            //Run prompt logic to load main menu
            // TODO: Add handling for app crash detection
            // TODO: Add handling for settings app open from editor side 
            this.StartCoroutine(this.RunDelayedLoad());
            
            //Loading settings
            this.LoadAccessibilitySettings();

            //Delegating audio settings to AudioManager
            App.Events.Application.OnAppOpened?.Invoke();
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
        private void Boot() {
            //Initializing singleton
            if (Instance != null && Instance != this) {
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
            
            //Making Universal object
            DontDestroyOnLoad(this.gameObject);
            
            //Loading global managers
            // TODO: Evolve to Service Locator pattern later
            this.ArchitecturalLoad();
        }
        
        /// <summary>
        /// 
        /// </summary>
        private void ArchitecturalLoad() {
            if (this.savesManager == null) {
                this.savesManager = GetComponentInChildren<SavesManager>(true);

                if (this.savesManager == null) {
                    this.savesManager = Instantiate(this.savesManagerPrefab, this.transform).GetComponent<SavesManager>();
                }
            }

            if (this.audioManager == null) {
                this.audioManager = GetComponentInChildren<AudioManager>(true);

                if (this.audioManager == null) {
                    this.audioManager = Instantiate(this.audioManagerPrefab, this.transform).GetComponent<AudioManager>();
                }
            }

            // TODO: implement Input manager initialization

            if (this.narrativeManager == null) {
                this.narrativeManager = GetComponentInChildren<NarrativeManager>(true);

                if (this.narrativeManager == null) {
                    this.narrativeManager = Instantiate(this.narrativeManagerPrefab, this.transform).GetComponent<NarrativeManager>();
                }
            }
            
            if (this.routerManager == null) {
                this.routerManager = GetComponentInChildren<RouterManager>(true);
                if (this.routerManager == null) {
                    this.routerManager = Instantiate(this.routerManagerPrefab, this.transform).GetComponent<RouterManager>();
                }
            }

            if (this.uiManager == null) {
                this.uiManager = GetComponentInChildren<UIManager>(true);

                if (this.uiManager == null) {
                    this.uiManager = Instantiate(this.uiManagerPrefab, this.transform).GetComponent<UIManager>();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadAccessibilitySettings() {
            //Requesting current settings to SavesManager
            App.Tools.Data.SettingsData settings = App.Events.Saves.OnRequestCurrentSettings?.Invoke();

            //Handling no event return
            if (settings == null) {
                Debug.LogError("[AL] No settings found or no listener to request!");
                return;
            }

            //Quality
            QualitySettings.SetQualityLevel(settings.qualityIndex);

            //Language
            List<UnityEngine.Localization.Locale> locales = UnityEngine.Localization.Settings.LocalizationSettings.AvailableLocales.Locales;

            //Ensuring Locale is available even when settings had one saved
            if (settings.localeIndex >= 0) {
                //Loading Locale from settings
                if (settings.localeIndex < locales.Count) UnityEngine.Localization.Settings.LocalizationSettings.SelectedLocale = locales[settings.localeIndex];
                else Debug.LogWarning("[AL] Couldn't load Locale. Locale out of range?");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ValidateReferences() {
            //Handling wrong delayed menu load config
            if (!this.inputTimeout && !this.timeTimeout) Debug.LogWarning("[AL] Current menu load config only allows external events to continue. Is this intended?");

            if (this.promptText) this.promptText.gameObject.SetActive(this.inputTimeout);

            if (this.localeButton) {
                if (_SystemManager.GetAvailableLocales() == null) this.localeButton.gameObject.SetActive(false);
                else this.localeButton.interactable = _SystemManager.GetAvailableLocales().Count > 0;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IEnumerator RunDelayedLoad() {
            float timer = this.timeoutSeconds;
            while (timer > 0.0f) {
                if (this.inputTimeout) InputSystem.onAnyButtonPress.CallOnce(ctrl => { timer = 0.0f; });
                if (this.timeTimeout) timer -= Time.deltaTime;

                yield return null;
            }

            //Default. A frame after
            yield return new WaitForEndOfFrame();

            // TODO: Add logic to handle already started games or just load MainMenu
            // ! Also, this is a direct call and reference, may be better to load from events as OnAppLoaded/Resumed
            // TODO: Adapt router to receive OnAppLoaded/Resumed events and remove strict calls
            this.routerManager?.OnLoadMenu();
        }

    // ? EVENT METHODS==============================================================================================================================
        /// <summary>
        /// 
        /// </summary>
        public static void OnEnsureInstance() {
            if (Instance != null) return;

            GameObject existing = GameObject.Find("Managers");
            if (existing != null && existing.GetComponent<_AppManager>() != null) {
                if (Instance == null)
                    Instance = existing.GetComponent<_AppManager>();
                return;
            }

            GameObject managers = Instantiate(appManagerPrefab);
            managers.name = "Managers";
        }

        /// <summary>
        /// 
        /// </summary>
        public void CycleLocale() {
            //Request settings
            Tools.Data.SettingsData settings = App.Events.Saves.OnRequestCurrentSettings?.Invoke();
            if (settings == null) {
                Debug.LogError("No settings found or no listener to request!");
                return;
            }

            //Cycling locale
            App.Tools._SystemManager.CycleLocale();
            App.Events.Settings.OnLocaleUpdated?.Invoke();
        }
        
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