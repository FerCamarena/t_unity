using UnityEngine.UI;
using UnityEngine;

namespace App.Game.UI {
    /// <summary>
    /// 
    /// </summary>
    public class UpdatableUIButton : Button, IUpdatableUI<Button> {
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
        public App.Tools.Data.SyncCategory SyncType => App.Tools.Data.SyncCategory.button;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        public Button Target => this;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        public int ElementID => this.GetInstanceID();
        
        // * INTERNALS
        //[Header("Internals")]

    // ? BASE METHODS===============================================================================================================================
        // * Snippet stored to correctly manage independent Button suscribe calls.
        // protected override void OnEnable() {
        //     if (DEBUG) Debug.Log("Suscribe specific requested of : " + this.GetInstanceID() + ", as SyncType: " + this.SyncType.ToString());
        //     Events.UI.SubscribeUpdatableUIElement?.Invoke(this.SyncType, this);
            
        //     base.OnEnable();
        // }

        // * Snippet stored to correctly manage independent Button unsuscribe calls.
        // protected override void OnDisable() {
        //     if (DEBUG) Debug.Log("Unsuscribe specific requested of: " + this.GetInstanceID() + ", as SyncType: " + this.SyncType.ToString());
        //     Events.UI.UnsubscribeUpdatableUIElement?.Invoke(this.SyncType, this);

        //     base.OnDisable();
        // }

    // ? CUSTOM METHODS=============================================================================================================================
        /// <summary>
        /// 
        /// </summary>
        public virtual void SyncUI() {
            if (DEBUG) Debug.Log("Sync specific processed on: " + this + ", type: " + this.SyncType.ToString());
        }

    // ? EVENT METHODS==============================================================================================================================
    
    }
}