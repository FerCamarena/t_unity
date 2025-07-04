using System;

namespace App.Events {
    public static class Settings {
        // * GENERAL
        public static Action OnSettingsOpened;
        public static Action OnSettingsClosed;
        public static Action OnSettingsToggled;
        public static Action OnSettingsReseted;
        public static Action OnSettingsSaved;
        public static Action OnSettingsChanged;
        
        // * VOLUME
        public static Action<float> OnMasterVolumeChanged;
        public static Action<float> OnMusicVolumeChanged;
        public static Action<float> OnUIVolumeChanged;
        public static Action<float> OnSFXVolumeUpdated;
        public static Action<float> OnAtmosphereVolumeChanged;
        public static Action<float> OnVoiceVolumeChanged;
        
        // * GRAPHICS
        public static Action OnQualityUpdated;

        // * ACCESIBILITY
        public static Action OnLanguageUpdated;
        //public static Action OnInputChanged;
    }
}