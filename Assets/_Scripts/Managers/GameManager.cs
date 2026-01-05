using UnityEngine;

namespace App.Game.Managers {
    /// <summary>
    /// 
    /// </summary>
    public sealed class GameManager : MonoBehaviour {
    // ? DEBUG======================================================================================================================================
        //[Header("Debug")]
        /// <summary>
        /// 
        /// </summary>
        private static bool DEBUG => false;

    // ? PARAMETERS=================================================================================================================================
        // * REFERENCES
        //[Header("References")]
        
        // * ATTRIBUTES
        [Header("Attributes")]
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private readonly App.Tools.Data.SoundClip levelSong;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private  readonly bool pauseMusicOnStart = false;
        
        // * INTERNALS
        //[Header("Internals")]

    // ? BASE METHODS===============================================================================================================================
        /// <summary>
        /// 
        /// </summary>
        private void Start() {
            if (this.pauseMusicOnStart) Events.Audio.OnStopUniversalByTag?.Invoke(App.Tools.Data.GenericTag.Soundtrack, false, 0.0f);
            else if (levelSong) {
                Events.Audio.OnStopUniversalByTag?.Invoke(App.Tools.Data.GenericTag.Soundtrack, true, 0.5f);
                Events.Audio.OnPlayClipUniversally?.Invoke(levelSong);
            }
        }

    // ? CUSTOM METHODS=============================================================================================================================
        
    // ? EVENT METHODS==============================================================================================================================
    
    }
}