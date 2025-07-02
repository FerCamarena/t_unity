using System;

namespace App.Events {
    public static class Settings {
        // * GENERAL
        public static Action OnSettingsOpened;
        public static Action OnSettingsClosed;
        public static Action OnSettingsReseted;
        public static Action OnSettingsSaved;
        public static Action OnSettingsChanged;
        //public static Action OnSettingsLoaded;
        
        // * VOLUME
        public static Action<float> OnMasterVolumeUpdated;
        public static Action<float> OnMusicVolumeUpdated;
        public static Action<float> OnUIVolumeUpdated;
        public static Action<float> OnSFXVolumeUpdated;
        public static Action<float> OnAtmosphereVolumeUpdated;
        public static Action<float> OnVoiceVolumeUpdated;
        
        // * GRAPHICS
        public static Action OnQualityUpdated;

        // * ACCESIBILITY
        public static Action OnLanguageUpdated;
        //public static Action OnInputChanged;
    }
}