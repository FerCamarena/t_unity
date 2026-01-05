using System.Collections.Generic;
using UnityEngine;
using System;

namespace App.Managers {
    public sealed class UIManager : MonoBehaviour {
    // ? DEBUG======================================================================================================================================
        //[Header("Debug")]
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        private static bool DEBUG => false;

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
        private static readonly Dictionary<App.Tools.Data.SyncCategory, Dictionary<int, App.Game.UI.IUpdatableUI>> registry = new Dictionary<App.Tools.Data.SyncCategory, Dictionary<int, App.Game.UI.IUpdatableUI>>();
        
    // ? BASE METHODS===============================================================================================================================
        /// <summary>
        /// 
        /// </summary>
        private void OnEnable() {
            Events.UI.SubscribeUpdatableUIElement += Register;
            Events.UI.UnsubscribeUpdatableUIElement += Unregister;

            Events.UI.OnSyncGroupRequested += this.SyncAll;
            Events.UI.OnSyncElementRequested += this.SyncSpecific;
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnDisable() {
            Events.UI.SubscribeUpdatableUIElement -= Register;
            Events.UI.UnsubscribeUpdatableUIElement -= Unregister;
            
            Events.UI.OnSyncGroupRequested -= this.SyncAll;
            Events.UI.OnSyncElementRequested -= this.SyncSpecific;
        }

    // ? CUSTOM METHODS=============================================================================================================================
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instanceID"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool TryGet(App.Tools.Data.SyncCategory type, int instanceID, out App.Game.UI.IUpdatableUI element) {
            if (registry.TryGetValue(type, out var group)) return group.TryGetValue(instanceID, out element);
            element = null;

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<App.Game.UI.IUpdatableUI> GetAll(App.Tools.Data.SyncCategory type) {
            return registry.TryGetValue(type, out Dictionary<int, App.Game.UI.IUpdatableUI> group) ? group.Values : Array.Empty<App.Game.UI.IUpdatableUI>();
        }

    // ? EVENT METHODS==============================================================================================================================
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="element"></param>
        public static void Register(App.Tools.Data.SyncCategory type, App.Game.UI.IUpdatableUI element) {
            if (DEBUG) Debug.Log("Suscribe specific requested of: " + element + ", as: " + type.ToString());

            //Preventing assignations on empty
            if (!registry.ContainsKey(type)) registry[type] = new Dictionary<int, App.Game.UI.IUpdatableUI>();

            if (DEBUG) Debug.Log("Sync specific processed" + element.Target.GetInstanceID() + ", as: " + type.ToString());
            int id = element.Target.GetInstanceID();
            registry[type][id] = element;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="element"></param>
        public static void Unregister(App.Tools.Data.SyncCategory type, App.Game.UI.IUpdatableUI element) {
            if (registry.TryGetValue(type, out var group)) {
                int id = element.Target.GetInstanceID();
                group.Remove(id);

                if (group.Count == 0) registry.Remove(type);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        private void SyncAll(App.Tools.Data.SyncCategory type) {
            if (!registry.TryGetValue(type, out var elements)) return;

            foreach (var element in elements.Values) element.SyncUI();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        private void SyncSpecific(App.Tools.Data.SyncCategory type, int id) {
            if (registry.TryGetValue(type, out Dictionary<int, App.Game.UI.IUpdatableUI> elements) && elements.TryGetValue(id, out App.Game.UI.IUpdatableUI element)) {
                if (DEBUG) Debug.Log("Specific sync correctly found in registry");
                element.SyncUI();
            } else if (DEBUG) Debug.Log("Specific sync not found in registry");
        }
    }
}