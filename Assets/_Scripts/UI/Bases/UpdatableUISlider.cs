using UnityEngine.UI;
using UnityEngine;

namespace App.Game.UI {
    /// <summary>
    /// 
    /// </summary>
    public class UpdatableUISlider : Slider, IUpdatableUI<Slider> {
    // ? DEBUG======================================================================================================================================
        //[Header("Debug")]
        /// <summary>
        /// 
        /// </summary>
        protected bool DEBUG => false;

    // ? PARAMETERS=================================================================================================================================
        // * REFERENCES
        //[Header("References")]
        
        // * ATTRIBUTES
        //[Header("Attributes")]
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        Component IUpdatableUI.Target => this;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        public App.Tools.Data.SyncCategory SyncType => App.Tools.Data.SyncCategory.slider;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        public Slider Target => this;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        public int ElementID => this.GetInstanceID();
        
        // * INTERNALS
        //[Header("Internals")]

    // ? BASE METHODS===============================================================================================================================
        // * Snippet stored to correctly manage independent Slider suscribe calls.
        // protected override void OnEnable() {
        //     if (DEBUG) Debug.Log("Suscribe specific requested of: " + this.GetInstanceID() + ", as SyncType: " + this.SyncType.ToString());
        //     Events.UI.SubscribeUpdatableUIElement?.Invoke(this.SyncType, this);
            
        //     base.OnEnable();
        // }

        // * Snippet stored to correctly manage independent Slider unsuscribe calls.
        // protected override void OnDisable() {
        //     if (DEBUG) Debug.Log("Unsuscribe specific requested of: " + this.GetInstanceID() + ", as SyncType: " + this.SyncType.ToString());
        //      Events.UI.UnsubscribeUpdatableUIElement?.Invoke(this.SyncType, this);

        //     base.OnDisable();
        // }

    // ? CUSTOM METHODS=============================================================================================================================
        /// <summary>
        /// 
        /// </summary>
        public virtual void SyncUI() {}

    // ? EVENT METHODS==============================================================================================================================

    }
}