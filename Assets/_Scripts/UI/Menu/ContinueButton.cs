using UnityEngine;

namespace App.Game.UI.Custom {
    public class ContinueButton : UpdatableUIButton {
    // ? DEBUG======================================================================================================================================

    // ? PARAMETERS=================================================================================================================================
        // * REFERENCES
        
        // * ATTRIBUTES
        
        // * INTERNAL
        [SerializeField] private bool enable = false;

    // ? BASE METHODS===============================================================================================================================
        // * Currently managing independent and self Button suscription since Main Menu scene only requires a single GameObject sync update.
        protected override void OnEnable() {
            if (DEBUG) Debug.Log("Suscribe specific requested of : " + this.GetInstanceID() + ", as SyncType: " + this.SyncType.ToString());
            Events.UI.SubscribeUpdatableUIElement?.Invoke(this.SyncType, this);
            
            base.OnEnable();
        }

        // * Currently managing independent and self Button unsuscription since Main Menu scene only requires a single GameObject sync update.
        protected override void OnDisable() {
            if (DEBUG) Debug.Log("Unsuscribe specific requested of: " + this.GetInstanceID() + ", as SyncType: " + this.SyncType.ToString());
            Events.UI.UnsubscribeUpdatableUIElement?.Invoke(this.SyncType, this);

            base.OnDisable();
        }

    // ? CUSTOM METHODS=============================================================================================================================
        public override void SyncUI() {
            //TODO: Update to use SavesManager info
            // this.interactable = SavesManager.HasSave(); *example application
            this.interactable = this.enable; 

            base.SyncUI();   
        }

    // ? EVENT METHODS==============================================================================================================================
    
    }
}