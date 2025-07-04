using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine;

namespace App.Game.Managers {
    public class _RouterManager : MonoBehaviour {
    // ? DEBUG======================================================================================================================================
    [SerializeField] private bool DEBUG = false;

    // ? PARAMETERS=================================================================================================================================
        // * REFERENCES
        
        // * INTERNAL

        // * ATTRIBUTES
        [SerializeField] public int pauseSceneBuildIndex;
        [SerializeField] public int mainMenuSceneBuildIndex;
        [SerializeField] public int menuLoaderSceneBuildIndex;
        [SerializeField] public int gameSceneBuildIndex;
        [SerializeField] public int gameLoaderSceneBuildIndex;
        [SerializeField] public int resultsSceneBuildIndex;

        // TODO: Move UI related updates to UIManager for handling it
        // TODO: Manage scene build index references just from friend classes
        // TODO: Add quality of life to prevent unreferenced variables
        // TODO: Add multiple debug, warning and error handling conditionals
    // ? BASE METHODS===============================================================================================================================
        private void OnEnable() {
            //Starting the app with uncompleted game or pressing over Continue on Main menu
            Events.Application.OnSessionResumed += InitializeGame;

            //Pressing over NewGame on Main menu
            Events.Application.OnNewGameSession += InitializeGame;

            Events.InGame.OnGameOver += EndGame;
            Events.Application.OnAppClosed += ExitApp;
            Events.Settings.OnSettingsToggled += ToggleSettings;
        }
        
        private void OnDisable() {
            Events.Application.OnSessionResumed -= InitializeGame;
            Events.Application.OnNewGameSession -= InitializeGame;
            Events.InGame.OnGameOver -= EndGame;
            Events.Application.OnAppClosed -= ExitApp;
            Events.Settings.OnSettingsToggled -= ToggleSettings;
        }

    // ? CUSTOM METHODS=============================================================================================================================
        
    // ? EVENT METHODS==============================================================================================================================
        public void LoadMenu() {
            if (SceneManager.GetActiveScene().buildIndex == 0) this.ChangeScene(this.mainMenuSceneBuildIndex);
        }

        private void StartNewGame() {
            PlayerPrefs.SetInt("InGame", 1);
            
            // ? Open game scene instead of loader, loader is opened from menu instead
            this.ChangeScene(this.gameSceneBuildIndex);
        }

        private void InitializeGame() {
            this.ChangeScene(this.gameLoaderSceneBuildIndex);
        }

        public void EndGame() {
            PlayerPrefs.SetInt("InGame", 0);
            
            this.ChangeScene(this.resultsSceneBuildIndex);
        }

        public void ExitApp() {
            //Enabling Editor playmode exitting
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

        public void ToggleSettings() {
            if (PlayerPrefs.GetInt("SettingsOpen") == 1) {
                PlayerPrefs.SetInt("SettingsOpen", 0);
                if(SceneManager.GetSceneByBuildIndex(this.pauseSceneBuildIndex).isLoaded)
                    SceneManager.UnloadSceneAsync(this.pauseSceneBuildIndex);

                Time.timeScale = 1.0f; //Pause
            } else {
                PlayerPrefs.SetInt("SettingsOpen", 1);
                SceneManager.LoadSceneAsync(this.pauseSceneBuildIndex, LoadSceneMode.Additive);
                
                Time.timeScale = 0.0f; //Unpause
            }
        }
        
        public void ChangeScene(int sceneIndex) {
            SceneManager.LoadScene(sceneIndex);
        }
    }
}