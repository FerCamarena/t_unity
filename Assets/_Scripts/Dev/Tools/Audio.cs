using UnityEngine;

namespace Dev {
    /// <summary>
    /// 
    /// </summary>
    public static class Audio {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="linear"></param>
        /// <returns></returns>
        public static float LinearToDecibel(float linear) => linear <= 0.0f ? -80.0f : Mathf.Log10(linear) * 20.0f;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dB"></param>
        /// <returns></returns>
        public static float DecibelToLinear(float dB) => dB <= -80.0f ? 0.0f : Mathf.Pow(10.0f, dB / 20.0f);
    }
}

namespace App.Tools {
    /// <summary>
    /// 
    /// </summary>
    public static class Audio {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static bool ApproximatelyEqual(App.Tools.Data.VolumesSnapshot a, App.Tools.Data.VolumesSnapshot b) {
            return Mathf.Approximately(a.masterVolume, b.masterVolume)
                && Mathf.Approximately(a.musicVolume, b.musicVolume)
                && Mathf.Approximately(a.uiVolume, b.uiVolume)
                && Mathf.Approximately(a.sfxVolume, b.sfxVolume)
                && Mathf.Approximately(a.atmosphereVolume, b.atmosphereVolume)
                && Mathf.Approximately(a.voiceVolume, b.voiceVolume);
        }
    }
}