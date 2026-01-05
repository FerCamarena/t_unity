using System;

namespace App.Events {
    /// <summary>
    /// 
    /// </summary>
    public static class Saves {
        // * GENERAL
        /// <summary>
        /// 
        /// </summary>
        public static Action<string> OnSaveCreated;
        /// <summary>
        /// 
        /// </summary>
        public static Action<string> OnGameSaved;
        /// <summary>
        /// 
        /// </summary>
        public static Action<string> OnGameLoaded;
        /// <summary>
        /// 
        /// </summary>
        public static Action<string> OnSaveDeleted;
        /// <summary>
        /// 
        /// </summary>
        public static Func<App.Tools.Data.SettingsData> OnRequestCurrentSettings;
        /// <summary>
        /// 
        /// </summary>
        //public static Action<string> OnAutoSaved;
        
        // * CLOUD
        /// <summary>
        /// 
        /// </summary>
        //public static Action OnCloudSyncCompleted;
        /// <summary>
        /// 
        /// </summary>
        //public static Action OnCloudSyncEnabled;
        /// <summary>
        /// 
        /// </summary>
        //public static Action OnCloudSyncDisabled;
    }
}