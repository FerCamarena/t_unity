using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine;

namespace App.Game.Managers {
    public class _RouterManager : MonoBehaviour {
    // ? DEBUG======================================================================================================================================
    [SerializeField] private bool DEBUG = false;

    // ? PARAMETERS=================================================================================================================================
        // * REFERENCES
        [SerializeField] private AudioMixer audioMixer;
        // * INTERNAL

        // * ATTRIBUTES
        [SerializeField] public int pauseSceneBuildIndex;
        [SerializeField] public int menuLoaderSceneBuildIndex;
        [SerializeField] public int gameLoaderSceneBuildIndex;
        [SerializeField] public int resultsSceneBuildIndex;

        // TODO: Move UI related updates to UIManager for handling it
        // TODO: Manage scene build index references just from friend classes
        // TODO: Add quality of life to prevent unreferenced variables
        // TODO: Add multiple debug, warning and error handling conditionals
    // ? BASE METHODS===============================================================================================================================
        private void OnEnable() {
            Events.InGame.OnGameOver += GameOver;
        }
        
        private void OnDisable() {
            Events.InGame.OnGameOver -= GameOver;
        }

        private void Awake() {
            if (!VerifiedManagers()) DontDestroyOnLoad(this.gameObject);
            else Destroy(this.gameObject);
        }

    // ? CUSTOM METHODS=============================================================================================================================
        private bool VerifiedManagers() {
            GameObject managerInstance = GameObject.Find("Managers");

            if (!managerInstance.GetComponent<_RouterManager>() && !managerInstance.CompareTag("GameController")) {
                Destroy(managerInstance);
                return false;
            }

            return false;
        }
    // ? EVENT METHODS==============================================================================================================================
        public void GameStart() {
            PlayerPrefs.SetInt("InGame", 1);
            
            this.ChangeScene(gameLoaderSceneBuildIndex);
        }

        public void GameOver() {
            PlayerPrefs.SetInt("InGame", 0);
            
            this.ChangeScene(resultsSceneBuildIndex);
        }

        public void QuitApp() {
            //Enabling Editor playmode exitting
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

        public void SettingsMenu() {
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