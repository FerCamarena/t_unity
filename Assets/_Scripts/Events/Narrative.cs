using System;

namespace App.Events {
    /// <summary>
    /// 
    /// </summary>
    public static class Narrative {
        // * SUBSCRIPTION
        /// <summary>
        /// 
        /// </summary>
        public static Action<int> OnSubtitleDisplayRegistered;
        /// <summary>
        /// 
        /// </summary>
        public static Action OnSubtitleDisplayUnregistered;

        // * ACTIVATION
        /// <summary>
        /// 
        /// </summary>
        public static Action<App.Tools.Data.NarrativeSequenceID> OnReproduceSequenceById;
        /// <summary>
        /// 
        /// </summary>
        public static Action<App.Tools.Data.NarrativeEntryID> OnReproduceEntryById;
        /// <summary>
        /// 
        /// </summary>
        public static Action<App.Tools.Data.NarrativeEntry> OnReproduceCustomEntry;
        /// <summary>
        /// 
        /// </summary>
        public static Action<App.Tools.Data.BaseSubtitleData> OnSubtitleShow;
        /// <summary>
        /// 
        /// </summary>
        public static Action<float> OnSubtitleFadeIn;
        /// <summary>
        /// 
        /// </summary>
        public static Action<float> OnSubtitleFadeOut;
        /// <summary>
        /// 
        /// </summary>
        public static Action OnSubtitleHide;
        

        // ! temp calls
        /// <summary>
        /// 
        /// </summary>
        public static Action OnPlayerSleep;
        /// <summary>
        /// 
        /// </summary>
        public static Action OnPlayerWakeUp;
        /// <summary>
        /// 
        /// </summary>
        public static Action OnBatHit;
    }
}