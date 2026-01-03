using UnityEngine;

namespace App.Tools {
    public static class Audio {
        /// <summary>
        /// Convierte un valor lineal a decibelios (escala de -80dB a 0dB).
        /// </summary>
        public static float LinearToDecibel(float linear) => linear <= 0.0001f ? -80f : Mathf.Log10(linear) * 20f;

        /// <summary>
        /// Convierte un valor en decibelios a lineal (escala de 0.0001 a 1.0).
        /// </summary>
        public static float DecibelToLinear(float dB) => Mathf.Pow(10f, dB / 20f);
    
        public enum MixerChannel {
            Master,
            Music,
            UI,
            SFX,
            Atmosphere,
            Voice
        }
        
        public static bool ApproximatelyEqual(Data.VolumesSnapshot a, Data.VolumesSnapshot b) {
            return Mathf.Approximately(a.masterVolume, b.masterVolume)
                && Mathf.Approximately(a.musicVolume, b.musicVolume)
                && Mathf.Approximately(a.uiVolume, b.uiVolume)
                && Mathf.Approximately(a.sfxVolume, b.sfxVolume)
                && Mathf.Approximately(a.atmosphereVolume, b.atmosphereVolume)
                && Mathf.Approximately(a.voiceVolume, b.voiceVolume);
        }
    }
}