using UnityEngine.UI;
using UnityEngine;

namespace App.Game.UI {
    public class UpdatableUIButton : Button, IUpdatableUI<Button> {
    // ? DEBUG======================================================================================================================================
    [SerializeField] private bool DEBUG = false;

    // ? PARAMETERS=================================================================================================================================
        // * REFERENCES
        
        // * INTERNAL
        public string ElementID => this.GetInstanceID().ToString();
        Component IUpdatableUI.Target => this;
        public Button Target => this;
        
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
        public SyncType SyncType => SyncType.button;
        public virtual void SyncUI() {
            Debug.Log("updated!");
        }

    // ? EVENT METHODS==============================================================================================================================
    }
}