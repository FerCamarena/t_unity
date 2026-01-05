using UnityEngine;

namespace App.Game.UI.Custom {
    public class ContinueButton : UpdatableUIButton {
    // ? DEBUG======================================================================================================================================
        //[Header("Debug")]

    // ? PARAMETERS=================================================================================================================================
        // * REFERENCES
        //[Header("References")]

        // * ATTRIBUTES
        [Header("Attributes")]
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private bool enable = false;
        
        // * INTERNALS
        //[Header("Internal")]

    // ? BASE METHODS===============================================================================================================================
        /// <summary>
        /// 
        /// </summary>
        // * Currently managing independent and self Button suscription since Main Menu scene only requires a single GameObject sync update.
        protected override void OnEnable() {
            if (DEBUG) Debug.Log("Suscribe specific requested of : " + this.GetInstanceID() + ", as SyncType: " + this.SyncType.ToString());
            Events.UI.SubscribeUpdatableUIElement?.Invoke(this.SyncType, this);
            
            base.OnEnable();
        }

        /// <summary>
        /// 
        /// </summary>
        // * Currently managing independent and self Button unsuscription since Main Menu scene only requires a single GameObject sync update.
        protected override void OnDisable() {
            if (DEBUG) Debug.Log("Unsuscribe specific requested of: " + this.GetInstanceID() + ", as SyncType: " + this.SyncType.ToString());
            Events.UI.UnsubscribeUpdatableUIElement?.Invoke(this.SyncType, this);

            base.OnDisable();
        }

    // ? CUSTOM METHODS=============================================================================================================================
        public override void SyncUI() {
            //TODO: Update to use SavesManager info
            this.interactable = this.enable; 

            base.SyncUI();   
        }

    // ? EVENT METHODS==============================================================================================================================
    
    }
}