using System;

namespace App.Events {
    public static class Saves {
        // * GENERAL
        public static Action<string> OnSaveCreated;
        public static Action<string> OnGameSaved;
        public static Action<string> OnGameLoaded;
        public static Action<string> OnSaveDeleted;
        //public static Action<string> OnAutoSaved;
        
        // * CLOUD
        //public static Action OnCloudSyncCompleted;
        //public static Action OnCloudSyncEnabled;
        //public static Action OnCloudSyncDisabled;
    }
}