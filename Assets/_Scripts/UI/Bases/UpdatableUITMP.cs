using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace App.Game.UI {
    public class UpdatableUITPM : TextMeshProUGUI, IUpdatableUI<TextMeshProUGUI> {
    // ? DEBUG======================================================================================================================================
    [SerializeField] protected bool DEBUG = false;

    // ? PARAMETERS=================================================================================================================================
        // * REFERENCES
        
        // * ATTRIBUTES
        
        // * INTERNAL
        Component IUpdatableUI.Target => this;
        public SyncType SyncType => SyncType.text;
        public TextMeshProUGUI Target => this;
        public int ElementID => this.GetInstanceID();

    // ? BASE METHODS===============================================================================================================================
        // * Snippet stored to correctly manage Button suscribe calls.
        // protected override void OnEnable() {
        //     if (DEBUG) Debug.Log("Suscribe specific requested of : " + this.GetInstanceID() + ", as SyncType: " + this.SyncType.ToString());
        //     Events.UI.SubscribeUpdatableUIElement?.Invoke(this.SyncType, this);
            
        //     base.OnEnable();
        // }

        // * Snippet stored to correctly manage Button unsuscribe calls.
        // protected override void OnDisable() {
        //     if (DEBUG) Debug.Log("Unsuscribe specific requested of: " + this.GetInstanceID() + ", as SyncType: " + this.SyncType.ToString());
        //     Events.UI.UnsubscribeUpdatableUIElement?.Invoke(this.SyncType, this);

        //     base.OnDisable();
        // }

    // ? CUSTOM METHODS=============================================================================================================================
        public virtual void SyncUI() {
            if (DEBUG) Debug.Log("Sync specific processed on: " + this + ", type: " + this.SyncType.ToString());
        }

    // ? EVENT METHODS==============================================================================================================================
    
    }
}