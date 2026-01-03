using System.Linq;
using UnityEngine;

namespace App.Game.Audio {
    
    [CreateAssetMenu(menuName = "Audio/Sound Clip/New Multiple Sound Clip Data", fileName = "MultipleSoundClipData")]
    public class MultipleSoundClipData : SoundClipData {
    // ? DEBUG======================================================================================================================================
        //[SerializeField] private bool DEBUG = false;

    // ? PARAMETERS=================================================================================================================================
        // * REFERENCES
        
        // * ATTRIBUTES
        
        // * INTERNAL
        // ? Should work with category or category is implicit when used?
        //[SerializeField] private string category;
        [SerializeField] private AudioClip[] clipsPool;

    // ? BASE METHODS===============================================================================================================================
    
    // ? CUSTOM METHODS=============================================================================================================================

    // ? EVENT METHODS==============================================================================================================================
        //public string Category => category;
        //public AudioClip[] ClipsPool => clipsPool;
        public override AudioClip GetClip() => this.clipsPool[Random.Range(0, this.clipsPool.Length)];
    }
}