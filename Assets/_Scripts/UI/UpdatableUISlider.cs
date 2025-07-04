using UnityEngine.UI;
using UnityEngine;

namespace App.Game.UI {
    public class UpdatableUISlider : Slider, IUpdatableUI<Slider> {
    // ? DEBUG======================================================================================================================================
    [SerializeField] private bool DEBUG = false;

    // ? PARAMETERS=================================================================================================================================
        // * REFERENCES
        
        // * INTERNAL
        public string ElementID => this.GetInstanceID().ToString();
        Component IUpdatableUI.Target => this;
        public Slider Target => this;
        
        // * ATTRIBUTES
    // ? BASE METHODS===============================================================================================================================
        protected override void OnEnable() {
            Events.UI.SubscribeUpdatableUIElement?.Invoke(this.SyncType, this);
            
            base.OnEnable();
        }

        protected override void OnDisable() {
             Events.UI.UnsubscribeUpdatableUIElement?.Invoke(this.SyncType, this);

            base.OnDisable();
        }

    // ? CUSTOM METHODS=============================================================================================================================
        public SyncType SyncType => SyncType.slider;
        public virtual void SyncUI() {}

    // ? EVENT METHODS==============================================================================================================================
    }
}