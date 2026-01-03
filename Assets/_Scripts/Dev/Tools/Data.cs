#if UNITY_EDITOR
    using UnityEngine;
    using UnityEditor;

    namespace App.Tools {
        public static class Prefs {
            [MenuItem("Tools/DEV/Reset everything ")]
            public static void ResetEverything() {
                //Deleting all data
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
            }

            [MenuItem("Tools/DEV/Reset all stats")]
            public static void ResetAllStats() {
                //Set default states for each...
                PlayerPrefs.Save();
            }

            [MenuItem("Tools/DEV/Unlock every stat")]
            public static void UnlockAllStats() {
                //Set custom state for each...
                PlayerPrefs.Save();
            }
        }
    }
#endif