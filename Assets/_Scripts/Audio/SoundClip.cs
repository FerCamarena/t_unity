using UnityEngine;

namespace App.Tools.Data {
    /// <summary>
    /// 
    /// </summary>
    public abstract class SoundClip : ScriptableObject {
    // ? DEBUG======================================================================================================================================
        //[Header("Debug")]
        //[SerializeField] private readonly bool DEBUG = false;

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
        [SerializeField] protected MixerChannel channel = MixerChannel.Master;
        /// <summary>
        /// 
        /// </summary>
        [SerializeField] protected PlaybackScope scope = PlaybackScope.Local;
        /// <summary>
        /// 
        /// </summary>
        [SerializeField] protected GenericTag tag;
        /// <summary>
        /// 
        /// </summary>
        [SerializeField, Range(0.0f, 1.0f)] protected float volume = 1.0f;
        /// <summary>
        /// 
        /// </summary>
        [SerializeField] protected bool ignoreTimescale = false;
        /// <summary>
        /// 
        /// </summary>
        [SerializeField] protected bool loop = false;
        //[SerializeField] protected string[] effects;

    // ? BASE METHODS===============================================================================================================================
        /// <summary>
        /// 
        /// </summary>
        //protected virtual void OnEnable() { }
        
        /// <summary>
        /// 
        /// </summary>
        //protected virtual void OnDisable() { }
        
        /// <summary>
        /// 
        /// </summary>
        //protected virtual void OnValidate() { }
        
        /// <summary>
        /// 
        /// </summary>
        //protected virtual void OnDestroy() { }
    // ? CUSTOM METHODS=============================================================================================================================

    // ? EVENT METHODS==============================================================================================================================
        /// <summary>
        /// 
        /// </summary>
        public abstract AudioClip GetClip();
        /// <summary>
        /// 
        /// </summary>
        public MixerChannel Channel => this.channel;
        /// <summary>
        /// 
        /// </summary>
        public PlaybackScope Scope => this.scope;
        /// <summary>
        /// 
        /// </summary>
        public GenericTag Tag => this.tag;
        /// <summary>
        /// 
        /// </summary>
        public float Volume => this.volume;
        /// <summary>
        /// 
        /// </summary>
        public bool IgnoreTimescale => this.ignoreTimescale;
        /// <summary>
        /// 
        /// </summary>
        public bool Loop => this.loop;
        //public string[] Effects => effects;
    }
    
    [System.Serializable]
    /// <summary>
    /// 
    /// </summary>
    public enum PlaybackScope {
        Local,
        Global,
        Universal
    }
    
    [System.Serializable]
    /// <summary>
    /// 
    /// </summary>
    public enum GenericTag {
        Soundtrack,
        Atmosphere,
        UI
    }

    // ! TODO: Add system to warn or update dinamically from current user created Audio channels on Mixers
    [System.Serializable]
    /// <summary>
    /// 
    /// </summary>
    public enum MixerChannel {
        Master,
        Music,
        UI,
        SFX,
        Atmosphere,
        Voice
    }
}