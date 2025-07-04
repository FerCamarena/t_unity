using UnityEngine;

namespace App.Tools {
    public class _EditorBootstrapper : MonoBehaviour {
    // ? DEBUG======================================================================================================================================
        [SerializeField] private bool DEBUG = false;
    // ? PARAMETERS=================================================================================================================================
        // * REFERENCES
        [SerializeField] private GameObject appManagerPrefab;
        
        // * INTERNAL
        
        // * ATTRIBUTES
            
    // ? BASE METHODS===============================================================================================================================
        private void Awake() {
            //Ensuring AppLoader instance creation from any scene build index != 0
            #if UNITY_EDITOR
                Game.Managers._AppManager.appManagerPrefab = appManagerPrefab;
                Game.Managers._AppManager.EnsureInstance();

            #endif
            
            DestroyImmediate(this.gameObject);
        }

    // ? CUSTOM METHODS=============================================================================================================================

    // ? EVENT METHODS==============================================================================================================================
    }
}