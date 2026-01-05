using System;

namespace App.Events {
    /// <summary>
    /// 
    /// </summary>
    public static class Settings {
        // * GENERAL
        /// <summary>
        /// 
        /// </summary>
        public static Action OnSettingsOpened;
        /// <summary>
        /// 
        /// </summary>
        public static Action OnSettingsClosed;
        /// <summary>
        /// 
        /// </summary>
        public static Action OnSettingsToggled;
        /// <summary>
        /// 
        /// </summary>
        public static Action OnSettingsReseted;
        /// <summary>
        /// 
        /// </summary>
        public static Action OnSettingsSaved;
        /// <summary>
        /// 
        /// </summary>
        public static Action OnSettingsChanged;
        
        // * VOLUME
        /// <summary>
        /// 
        /// </summary>
        public static Action<App.Tools.Data.VolumesSnapshot> OnVolumesChanged;
        /// <summary>
        /// 
        /// </summary>
        public static Action<float> OnMasterVolumeChanged;
        /// <summary>
        /// 
        /// </summary>
        public static Action<float> OnMusicVolumeChanged;
        /// <summary>
        /// 
        /// </summary>
        public static Action<float> OnUIVolumeChanged;
        /// <summary>
        /// 
        /// </summary>
        public static Action<float> OnSFXVolumeUpdated;
        /// <summary>
        /// 
        /// </summary>
        public static Action<float> OnAtmosphereVolumeChanged;
        /// <summary>
        /// 
        /// </summary>
        public static Action<float> OnVoiceVolumeChanged;
        
        // * GRAPHICS
        /// <summary>
        /// 
        /// </summary>
        public static Action OnQualityUpdated;

        // * ACCESIBILITY
        /// <summary>
        /// 
        /// </summary>
        public static Action OnLocaleUpdated;
        /// <summary>
        /// 
        /// </summary>
        public static Action OnLocaleCycled;
        /// <summary>
        /// 
        /// </summary>
        //public static Action OnKeybindingChanged;
    }
}