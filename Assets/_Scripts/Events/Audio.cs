using System;

namespace App.Events {
    /// <summary>
    /// 
    /// </summary>
    public static class Audio {
        // * GENERAL
        
        // * VOLUME
        /// <summary>
        /// 
        /// </summary>
        public static Func<App.Tools.Data.VolumesSnapshot> OnRequestVolumes;
        /// <summary>
        /// 
        /// </summary>
        public static Action<App.Tools.Data.VolumesSnapshot> OnApplyVolumesSnapshot;
        /// <summary>
        /// 
        /// </summary>
        public static Action<App.Tools.Data.MixerChannel, float> OnApplyChannelVolume;
        
        // * GRAPHICS

        // * UNIVERSAL AUDIO
        /// <summary>
        /// 
        /// </summary>
        public static Action OnStopAllUniversal;
        /// <summary>
        /// 
        /// </summary>
        public static Action<App.Tools.Data.GenericTag, bool, float> OnStopUniversalByTag;
        /// <summary>
        /// 
        /// </summary>
        public static Action<App.Tools.Data.SoundClip> OnPlayClipUniversally;
    }
}