using System.Collections;
using UnityEngine;

namespace App.Game.Managers {
    public class _AppManager : MonoBehaviour {
    // ? DEBUG======================================================================================================================================
    [SerializeField] private bool DEBUG = false;

    // ? PARAMETERS=================================================================================================================================

    // ? BASE METHODS===============================================================================================================================
        public static _AppManager Instance { get; private set; }
        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this.gameObject);

        }

        private void Start() {
            this.StartCoroutine(this.DelayedLoad());
        }

    // ? CUSTOM METHODS=============================================================================================================================
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