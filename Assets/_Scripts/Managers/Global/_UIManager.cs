using System.Collections.Generic;
using UnityEngine;
using App.Game.UI;
using System;

namespace App.Game.Managers {
    public class _UIManager : MonoBehaviour {
    // ? DEBUG======================================================================================================================================
    [SerializeField] private bool DEBUG = false;

    // ? PARAMETERS=================================================================================================================================
        // * REFERENCES
        
        // * INTERNAL
        
        // * ATTRIBUTES
    // ? BASE METHODS===============================================================================================================================
        private void OnEnable() {
            Events.UI.SubscribeUpdatableUIElement += Register;
            Events.UI.UnsubscribeUpdatableUIElement += Unregister;

            Events.UI.OnSyncElementRequested += this.SyncSpecific;
            Events.UI.OnSyncGroupRequested += this.SyncAll;
        }

        private void OnDisable() {
            Events.UI.SubscribeUpdatableUIElement -= Register;
            Events.UI.UnsubscribeUpdatableUIElement -= Unregister;
            
            Events.UI.OnSyncElementRequested -= this.SyncSpecific;
            Events.UI.OnSyncGroupRequested -= this.SyncAll;
        }

    // ? CUSTOM METHODS=============================================================================================================================
        private static readonly Dictionary<SyncType, Dictionary<int, IUpdatableUI>> registry = new Dictionary<SyncType, Dictionary<int, IUpdatableUI>>();
        public static bool TryGet(SyncType type, int instanceID, out IUpdatableUI element) {
            if (registry.TryGetValue(type, out var group)) return group.TryGetValue(instanceID, out element);
            element = null;

            return false;
        }

        public static IEnumerable<IUpdatableUI> GetAll(SyncType type) {
            return registry.TryGetValue(type, out var group) ? group.Values : Array.Empty<IUpdatableUI>();
        }

    // ? EVENT METHODS==============================================================================================================================

        public static void Register(SyncType type, IUpdatableUI element) {
            if (!registry.ContainsKey(type)) registry[type] = new Dictionary<int, IUpdatableUI>();

            Debug.Log("reg" + type.ToString() + element.Target.name + element.Target.GetInstanceID());

            int id = element.Target.GetInstanceID();
            registry[type][id] = element;
        }

        private void SyncAll(SyncType type) {
            if (!registry.TryGetValue(type, out var elements)) return;

            foreach (var element in elements.Values) element.SyncUI();
        }

        private void SyncSpecific(SyncType type, int id) {
            if (registry.TryGetValue(type, out var elements) && elements.TryGetValue(id, out var element)) {
                element.SyncUI();
            }
        }

        public static void Unregister(SyncType type, IUpdatableUI element) {
            if (registry.TryGetValue(type, out var group)) {
                int id = element.Target.GetInstanceID();
                group.Remove(id);

                if (group.Count == 0) registry.Remove(type);
            }
        }
    }
}