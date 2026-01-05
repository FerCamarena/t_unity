using App.Game.Audio;
using System;

namespace App.Events {
    public static class Audio {
        // * GENERAL
        
        // * VOLUME
        public static Func<Tools.Data.VolumesSnapshot> OnRequestVolumes;
        public static Action<Tools.Data.VolumesSnapshot> OnApplyVolumes;
        public static Action<SoundClipData> OnPlayClipUniversally;
        public static Action<string, float> OnApplyChannelVolume;
        
        // * GRAPHICS

        // * ACCESIBILITY
    }
}