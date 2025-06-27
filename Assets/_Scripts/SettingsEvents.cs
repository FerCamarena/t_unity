using System;

namespace App.Events {
    public static class Settings {
        // * GENERAL
        public static Action OnSettingsOpened;
        public static Action OnSettingsClosed;
        public static Action OnSettingsReseted;
        public static Action OnSettingsSaved;
        //public static Action OnSettingsLoaded;
        
        // * VOLUME
        public static Action OnMasterVolumeUpdated;
        public static Action OnMasterVolumeToggled;
        public static Action OnMusicVolumeUpdated;
        public static Action OnMusicVolumeToggled;
        public static Action OnUIVolumeUpdated;
        public static Action OnUIVolumeToggled;
        public static Action OnSFXVolumeUpdated;
        public static Action OnSFXVolumeToggled;
        public static Action OnAtmosphereVolumeUpdated;
        public static Action OnAtmosphereVolumeToggled;
        public static Action OnDialogVolumeUpdated;
        public static Action OnDialogVolumeToggled;
        
        // * GRAPHICS
        public static Action OnQualityUpdated;

        // * ACCESIBILITY
        public static Action OnLanguageUpdated;
        //public static Action OnInputChanged;
    }
}