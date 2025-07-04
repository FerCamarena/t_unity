using UnityEngine;

namespace App.Game.Managers {
    public class _MainMenuManager : MonoBehaviour {
    // ? DEBUG======================================================================================================================================
    [SerializeField] private bool DEBUG = false;

    // ? PARAMETERS=================================================================================================================================
        // * REFERENCES
        
        // * INTERNAL
        
        // * ATTRIBUTES
    // ? BASE METHODS===============================================================================================================================
        private void Awake() {
            //Ensuring AppLoader instance creation from any scene build index != 0
            #if UNITY_EDITOR
                _AppManager.EnsureInstance();
            #endif
        }

    // ? CUSTOM METHODS=============================================================================================================================

    // ? EVENT METHODS==============================================================================================================================
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