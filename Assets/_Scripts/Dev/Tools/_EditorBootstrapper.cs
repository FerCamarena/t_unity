using UnityEngine;

#if UNITY_EDITOR
    namespace App.Tools {
        public class _EditorBootstrapper : MonoBehaviour {
        // ? DEBUG======================================================================================================================================
            //[Header("Debug)]
            /// <summary>
            /// 
            /// </summary>
            private static bool DEBUG => false;
        // ? PARAMETERS=================================================================================================================================
            // * REFERENCES
            [SerializeField] private GameObject appManagerPrefab;
            
            // * INTERNAL
            
            // * ATTRIBUTES
                
        // ? BASE METHODS===============================================================================================================================
            private void Awake() {
                //Ensuring AppLoader instance creation from any scene build index != 0
                App.Managers._AppManager.appManagerPrefab = appManagerPrefab;
                App.Managers._AppManager.OnEnsureInstance();

                
                DestroyImmediate(this.gameObject);
            }

        // ? CUSTOM METHODS=============================================================================================================================

        // ? EVENT METHODS==============================================================================================================================
        }
    }
#endif