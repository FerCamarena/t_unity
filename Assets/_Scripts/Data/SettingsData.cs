using UnityEngine;

namespace App.Tools.Data {
    [System.Serializable]
    /// <summary>
    /// 
    /// </summary>
    public class SettingsData {
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        public App.Tools.Data.VolumesSnapshot volumesSnapshot;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        public int localeIndex = 0;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        public int qualityIndex = 0;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lang_i"></param>
        /// <param name="quality_i"></param>
        public SettingsData(int lang_i, int quality_i) {
            this.volumesSnapshot = App.Tools.Data.VolumesSnapshot.Default();

            this.localeIndex = lang_i;
            this.qualityIndex = quality_i;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vols_s"></param>
        /// <param name="lang_i"></param>
        /// <param name="quality_i"></param>
        public SettingsData(App.Tools.Data.VolumesSnapshot vols_s, int lang_i, int quality_i) {
            this.volumesSnapshot = vols_s;

            this.localeIndex = lang_i;
            this.qualityIndex = quality_i;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newVolumes"></param>
        public void SetSnapshotValues(App.Tools.Data.VolumesSnapshot newVolumes) {
            this.volumesSnapshot = newVolumes;
        }
    }
}

namespace App.Tools.Data {
    /// <summary>
    /// 
    /// </summary>
    [System.Serializable]
    public struct VolumesSnapshot {
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        public float masterVolume;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        public float musicVolume;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        public float uiVolume;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        public float sfxVolume;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        public float atmosphereVolume;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        public float voiceVolume;

        // * Assigning 0.5f as default value, but unsure if its adecuate since 100% of volume may not always be the best
        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultValue"></param>
        public VolumesSnapshot(float defaultValue = 0.5f) {
            this.masterVolume = defaultValue;
            this.musicVolume = defaultValue;
            this.uiVolume = defaultValue;
            this.sfxVolume = defaultValue;
            this.atmosphereVolume = defaultValue;
            this.voiceVolume = defaultValue;
        }
        public static VolumesSnapshot Default() => new VolumesSnapshot(0.5f);
    }
}