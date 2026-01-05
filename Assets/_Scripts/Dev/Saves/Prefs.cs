#if UNITY_EDITOR
    using UnityEngine;
    using UnityEditor;

    namespace Dev {
        /// <summary>
        /// 
        /// </summary>
        public static class Prefs {
            /// <summary>
            /// 
            /// </summary>
            [MenuItem("Tools/DEV/Reset everything")]
            [Tooltip("")]
            public static void ResetEverything() {
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
            }

            /// <summary>
            /// 
            /// </summary>
            [MenuItem("Tools/DEV/Reset each stat")]
            [Tooltip("")]
            public static void ResetAllStats() {
                PlayerPrefs.Save();
            }

            /// <summary>
            /// 
            /// </summary>
            [MenuItem("Tools/DEV/Unlock all stats")]
            [Tooltip("")]
            public static void UnlockAllStats() {
                PlayerPrefs.Save();
            }
        }
    }
#endif