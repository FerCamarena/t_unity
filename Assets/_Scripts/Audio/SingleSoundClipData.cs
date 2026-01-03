using UnityEngine;

namespace App.Game.Audio {
    [CreateAssetMenu(menuName = "Audio/Sound Clip/New Single Sound Clip Data", fileName = "SingleSoundClipData")]
    public class SingleSoundClipData : SoundClipData {
    // ? DEBUG======================================================================================================================================
        //[SerializeField] private bool DEBUG = false;

    // ? PARAMETERS=================================================================================================================================
        // * REFERENCES
        
        // * ATTRIBUTES
        
        // * INTERNAL
        // TODO: Update to work with universal ActionSound enum
        [SerializeField] private string id;
        [SerializeField] private AudioClip audioClip;
        //public string[] effects;

    // ? BASE METHODS===============================================================================================================================
    
    // ? CUSTOM METHODS=============================================================================================================================

    // ? EVENT METHODS==============================================================================================================================
        public string ID => this.id;
        //public AudioClip AudioClip => this.audioClip;
        public override AudioClip GetClip() => this.audioClip;
    }
}