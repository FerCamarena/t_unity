using System.Collections.Generic;
using UnityEngine;
using System;

namespace App.Events {
    public static class UI {
        // * SUBSCRIPTION
        public static Action<SyncType, Game.UI.IUpdatableUI> SubscribeUpdatableUIElement;
        public static Action<SyncType, Game.UI.IUpdatableUI> UnsubscribeUpdatableUIElement;
        
        // * REQUESTS
        public static Action<SyncType> OnSyncGroupRequested;
        public static Action<SyncType, int> OnSyncElementRequested; 
    }
}

public enum SyncType {
    slider,
    button,
    toggle,
    image,
    text
}