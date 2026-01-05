using UnityEngine;

namespace App.Tools.Data {
    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(menuName = "Audio/Sound Clip/New Single Sound Clip Data", fileName = "SingleSoundClipData")]
    public class SingleSoundClip : SoundClip {
    // ? DEBUG======================================================================================================================================
        //[Header("Debug")]

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
        [SerializeField] private AudioClip audioClip;

    // ? BASE METHODS===============================================================================================================================
    
    // ? CUSTOM METHODS=============================================================================================================================

    // ? EVENT METHODS==============================================================================================================================
        /// <summary>
        /// 
        /// </summary>
        public override AudioClip GetClip() => this.audioClip;
    }
}