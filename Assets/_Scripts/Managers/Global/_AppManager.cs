using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace App.Game.Managers {
    // TODO: Add loading animation
    // TODO: Add press to start text prompt
    public class _AppManager : MonoBehaviour {
    // ? DEBUG======================================================================================================================================
    [SerializeField] private static bool DEBUG = false;

    // ? PARAMETERS=================================================================================================================================
        // * REFERENCES
        [SerializeField] public static GameObject appManagerPrefab;
        [SerializeField] private GameObject savesManagerPrefab;
        [SerializeField] private GameObject uiManagerPrefab;
        [SerializeField] private GameObject routerManagerPrefab;
        [SerializeField] private GameObject audioManagerPrefab;

        // * ATTRIBUTES
        [SerializeField] private bool inputTimeout = true;
        [SerializeField] private bool timeTimeout = false;
        [SerializeField] private float timeoutSeconds = 1.0f;
        
        // * INTERNAL
        [SerializeField] private _SavesManager SavesManager;
        [SerializeField] private _UIManager UIManager;
        [SerializeField] private _RouterManager RouterManager;
        [SerializeField] private _AudioManager AudioManager;

    // ? BASE METHODS===============================================================================================================================
        public static _AppManager Instance { get; private set; }
        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this.gameObject);

            this.InitializeManagers();

            if (!this.inputTimeout && !this.timeTimeout) Debug.LogWarning("Current menu load config only allows external events to continue. Is this intended?");
        }

        private void Start() {
            this.StartCoroutine(this.DelayedLoad());
        }

    // ? CUSTOM METHODS=============================================================================================================================
        private void InitializeManagers() {
            if (this.SavesManager == null) {
                this.SavesManager = GetComponentInChildren<_SavesManager>(true);

                if (this.SavesManager == null) {
                    this.SavesManager = Instantiate(this.savesManagerPrefab, this.transform).GetComponent<_SavesManager>();
                }
            }

            if (this.UIManager == null) {
                this.UIManager = GetComponentInChildren<_UIManager>(true);

                if (this.UIManager == null) {
                    this.UIManager = Instantiate(this.uiManagerPrefab, this.transform).GetComponent<_UIManager>();
                }
            }
            
            if (this.RouterManager == null) {
                this.RouterManager = GetComponentInChildren<_RouterManager>(true);
                if (this.RouterManager == null) {
                    this.RouterManager = Instantiate(this.routerManagerPrefab, this.transform).GetComponent<_RouterManager>();
                }
            }

            if (this.AudioManager == null) {
                this.AudioManager = GetComponentInChildren<_AudioManager>(true);

                if (this.AudioManager == null) {
                    this.AudioManager = Instantiate(this.audioManagerPrefab, this.transform).GetComponent<_AudioManager>();
                }
            }
        }

        private IEnumerator DelayedLoad() {
            float timer = 0.0f;
            while (timer < this.timeoutSeconds) {
                if (this.inputTimeout) InputSystem.onAnyButtonPress.CallOnce(ctrl => { timer = this.timeoutSeconds; });
                if (this.timeTimeout) timer += Time.deltaTime;

                yield return null;
            }
            

            //Default. A frame after
            yield return null;

            // TODO: Add logic to handle already started games or just load MainMenu
            // ! Also, this is a direct call and reference, may be better to load from events as OnAppLoaded/Resumed
            // TODO: Adapt router to receive OnAppLoaded/Resumed events and remove strict calls
            this.RouterManager?.LoadMenu();
        }

    // ? EVENT METHODS==============================================================================================================================
        public static void EnsureInstance() {
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
    }
}