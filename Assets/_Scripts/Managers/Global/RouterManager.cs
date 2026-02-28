using UnityEngine.SceneManagement;
using UnityEngine;

namespace App.Managers {
    /// <summary>
    /// 
    /// </summary>
    public sealed class RouterManager : MonoBehaviour {
        // TODO: Move UI related updates to UIManager for handling it
        // TODO: Manage scene build index references just from friend classes
        // TODO: Add quality of life to prevent unreferenced variables
        // TODO: Add multiple debug, warning and error handling conditionals
    // ? DEBUG======================================================================================================================================
    //[Header("Debug")]
    /// <summary>
    /// 
    /// </summary>
    [Tooltip("")]
    private static bool DEBUG => false;

    // ? PARAMETERS=================================================================================================================================
        // * REFERENCES
        //[Header("References")]
        
        // * ATTRIBUTES
        //[Header("Attributes")]

        // * INTERNALS
        [Header("Internals")]
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private int pauseSceneBuildIndex;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private int mainMenuSceneBuildIndex;  
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private int menuLoaderSceneBuildIndex;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private int gameSceneBuildIndex;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private int gameLoaderSceneBuildIndex;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private int resultsSceneBuildIndex;

    // ? BASE METHODS===============================================================================================================================
        /// <summary>
        /// 
        /// </summary>
        private void OnEnable() {
            Events.Application.OnSessionResumed += this.OnInitializeGame;

            Events.Application.OnNewGameSession += this.OnInitializeGame;

            Events.Game.OnGameOver += this.OnEndGame;
            Events.Application.OnAppClosed += OnExitApp;
            Events.Settings.OnSettingsToggled += this.OnToggleSettings;

            Events.Application.OnMenuLoad += this.OnLoadMenu;
        }
        
        /// <summary>
        /// 
        /// </summary>
        private void OnDisable() {
            Events.Application.OnSessionResumed -= this.OnInitializeGame;

            Events.Application.OnNewGameSession -= this.OnInitializeGame;
            
            Events.Game.OnGameOver -= this.OnEndGame;
            Events.Application.OnAppClosed -= this.OnExitApp;
            Events.Settings.OnSettingsToggled -= this.OnToggleSettings;

            Events.Application.OnMenuLoad -= this.OnLoadMenu;
        }

    // ? CUSTOM METHODS=============================================================================================================================
        
    // ? EVENT METHODS==============================================================================================================================
        /// <summary>
        /// 
        /// </summary>
        public void OnLoadMenu() {
            // TODO: Pending to be handled with event calls
            this.OnChangeScene(this.mainMenuSceneBuildIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnStartNewGame() {
            PlayerPrefs.SetInt("InGame", 1);
            
            Events.Audio.OnStopAllUniversal?.Invoke();
            // ? Open game scene instead of loader, loader is opened from menu instead
            this.OnChangeScene(this.gameSceneBuildIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnInitializeGame() {
            this.OnChangeScene(this.gameLoaderSceneBuildIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnEndGame() {
            PlayerPrefs.SetInt("InGame", 0);
            
            this.OnChangeScene(this.resultsSceneBuildIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnExitApp() {
            //Enabling Editor playmode exitting
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif

            // TODO: Verify if settings is open and handle value in prefs
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnToggleSettings() {
            if (PlayerPrefs.GetInt("SettingsOpen") == 1) {
                PlayerPrefs.SetInt("SettingsOpen", 0);
                if(SceneManager.GetSceneByBuildIndex(this.pauseSceneBuildIndex).isLoaded)
                    SceneManager.UnloadSceneAsync(this.pauseSceneBuildIndex);

                Time.timeScale = 1.0f; //Unpause
            } else {
                PlayerPrefs.SetInt("SettingsOpen", 1);
                SceneManager.LoadSceneAsync(this.pauseSceneBuildIndex, LoadSceneMode.Additive);
                
                Time.timeScale = 0.0f; //Pause
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sceneIndex"></param>
        public void OnChangeScene(int sceneIndex) {
            SceneManager.LoadScene(sceneIndex);
        }
    }
}