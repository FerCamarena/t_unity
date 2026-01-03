using UnityEngine.UI;
using UnityEngine;

namespace App.Game.UI {
    public class UpdatableUISlider : Slider, IUpdatableUI<Slider> {
    // ? DEBUG======================================================================================================================================
    [SerializeField] protected bool DEBUG = false;

    // ? PARAMETERS=================================================================================================================================
        // * REFERENCES
        
        // * ATTRIBUTES
        
        // * INTERNAL
        public int ElementID => this.GetInstanceID();
        Component IUpdatableUI.Target => this;
        public Slider Target => this;

    // ? BASE METHODS===============================================================================================================================
        // * Snippet stored to correctly manage Slider suscribe calls.
        // protected override void OnEnable() {
        //     if (DEBUG) Debug.Log("Suscribe specific requested of: " + this.GetInstanceID() + ", as SyncType: " + this.SyncType.ToString());
        //     Events.UI.SubscribeUpdatableUIElement?.Invoke(this.SyncType, this);
            
        //     base.OnEnable();
        // }

        // * Snippet stored to correctly manage Slider unsuscribe calls.
        // protected override void OnDisable() {
        //     if (DEBUG) Debug.Log("Unsuscribe specific requested of: " + this.GetInstanceID() + ", as SyncType: " + this.SyncType.ToString());
        //      Events.UI.UnsubscribeUpdatableUIElement?.Invoke(this.SyncType, this);

        //     base.OnDisable();
        // }

    // ? CUSTOM METHODS=============================================================================================================================
        public SyncType SyncType => SyncType.slider;
        public virtual void SyncUI() {}

    // ? EVENT METHODS==============================================================================================================================

    }
}