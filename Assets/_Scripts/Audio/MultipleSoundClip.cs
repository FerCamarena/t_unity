using UnityEngine;

namespace App.Tools.Data {
    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(menuName = "Audio/Sound Clip/New Multiple Sound Clip Data", fileName = "MultipleSoundClipData")]
    public class MultipleSoundClip : SoundClip {
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
        [SerializeField] private AudioClip[] clipsPool;

    // ? BASE METHODS===============================================================================================================================
    
    // ? CUSTOM METHODS=============================================================================================================================

    // ? EVENT METHODS==============================================================================================================================
        /// <summary>
        /// 
        /// </summary>
        public override AudioClip GetClip() => this.clipsPool[Random.Range(0, this.clipsPool.Length)];
    }
}