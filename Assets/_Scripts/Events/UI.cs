using System;

namespace App.Events {
    /// <summary>
    /// 
    /// </summary>
    public static class UI {
        // * SUBSCRIPTION
        /// <summary>
        /// 
        /// </summary>
        public static Action<App.Tools.Data.SyncCategory, App.Game.UI.IUpdatableUI> SubscribeUpdatableUIElement;
        /// <summary>
        /// 
        /// </summary>
        public static Action<App.Tools.Data.SyncCategory, App.Game.UI.IUpdatableUI> UnsubscribeUpdatableUIElement;
        
        // * REQUESTS
        /// <summary>
        /// 
        /// </summary>
        public static Action<App.Tools.Data.SyncCategory> OnSyncGroupRequested;
        /// <summary>
        /// 
        /// </summary>
        public static Action<App.Tools.Data.SyncCategory, int> OnSyncElementRequested; 
    }
}