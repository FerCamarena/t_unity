using System;
using UnityEngine;

namespace App.Game.Audio {
    public abstract class SoundClipData : ScriptableObject {
    // ? DEBUG======================================================================================================================================
        //[SerializeField] private bool DEBUG = false;

    // ? PARAMETERS=================================================================================================================================
        // * REFERENCES
        
        // * ATTRIBUTES
        
        // * INTERNAL
        [SerializeField] protected PlaybackScope scope;
        [SerializeField] protected float volume = 1.0f;
        [SerializeField] protected bool ignoreTimescale = false;
        //[SerializeField] protected string[] effects;

    // ? BASE METHODS===============================================================================================================================
    
    // ? CUSTOM METHODS=============================================================================================================================

    // ? EVENT METHODS==============================================================================================================================
        public PlaybackScope Scope => this.scope;
        public float Volume => this.volume;
        public bool IgnoreTimescale => this.ignoreTimescale;
        //public string[] Effects => effects;
        public abstract AudioClip GetClip();

    }

    [Serializable]
    public enum PlaybackScope {
        Local,
        Global,
        Universal
    }
}