using System.Collections;
using UnityEngine;

namespace App.Game.Managers {
    public class _AppManager : MonoBehaviour {
    // ? DEBUG======================================================================================================================================
    [SerializeField] private bool DEBUG = false;

    // ? PARAMETERS=================================================================================================================================
        public _RouterManager RouterManager;
        public _AudioManager AudioManager;
        public _UIManager UIManager;

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
        }

        private void Start() {
            this.StartCoroutine(this.DelayedLoad());
        }

    // ? CUSTOM METHODS=============================================================================================================================
        private void InitializeManagers() {
            if (this.RouterManager == null) {
                this.RouterManager = GetComponentInChildren<_RouterManager>(true);
                if (this.RouterManager == null) {
                    var routerManagerprefab = Resources.Load<GameObject>("RouterManager");
                    this.RouterManager = Instantiate(routerManagerprefab, this.transform).GetComponent<_RouterManager>();
                }
            }

            if (this.AudioManager == null) {
                this.AudioManager = GetComponentInChildren<_AudioManager>(true);

                if (this.AudioManager == null) {
                    var audioManagerPrefab = Resources.Load<GameObject>("AudioManager");
                    this.AudioManager = Instantiate(audioManagerPrefab, this.transform).GetComponent<_AudioManager>();
                }
            }

            if (this.UIManager == null) {
                this.UIManager = GetComponentInChildren<_UIManager>(true);

                if (this.UIManager == null) {
                    var uiManagerPrefab = Resources.Load<GameObject>("UIManager");
                    this.UIManager = Instantiate(uiManagerPrefab, this.transform).GetComponent<_UIManager>();
                }
            }
        }

        private IEnumerator DelayedLoad() {
            // TODO: Add conditional logic to give option to wait after input or time
            // Time based
            //yield return new WaitForSeconds(3);
            // Input based
            /*
            while (timer < timeoutSeconds) {
                if (Input.anyKeyDown) {
                    yield break; // Salir de la corrutina inmediatamente
                }

                timer += Time.deltaTime;
                yield return null;
            }
            */

            //Default. A frame after
            yield return null;

            // TODO: Add logic to handle already started games or just load MainMenu
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

            var appManagerPrefab = Resources.Load<GameObject>("AppManager");
            Instantiate(appManagerPrefab);
        }
    }
}