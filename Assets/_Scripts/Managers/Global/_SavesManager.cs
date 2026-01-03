using UnityEngine;
using System.IO;
using App.Tools.Data;
using UnityEngine.Localization.Settings;
using System;

namespace App.Game.Managers {
    public class _SavesManager : MonoBehaviour {
    // ? DEBUG======================================================================================================================================
    [SerializeField] private static bool DEBUG = false;

    // ? PARAMETERS=================================================================================================================================
        // * REFERENCES
        
        // * ATTRIBUTES
        
        // * INTERNAL
        private static readonly string settingsDataName = "settings_data";
        [SerializeField] private SettingsData currentSettings;

    // ? BASE METHODS===============================================================================================================================
        private void OnEnable() {
            Events.Settings.OnSettingsSaved += this.SaveSettingsData;
            Events.Saves.OnRequestCurrentSettings += this.GetCurrentSettings;
        }

        private void OnDisable() {
            Events.Settings.OnSettingsSaved -= this.SaveSettingsData;
            Events.Saves.OnRequestCurrentSettings -= this.GetCurrentSettings;
        }

        private void Awake() {
            this.LoadSettingsData();
        }

    // ? CUSTOM METHODS=============================================================================================================================
        private SettingsData GetDefaultsFromUnity() {
            Debug.Log("Defaults");
            
            int qualityIndex = QualitySettings.GetQualityLevel();

            var locales = LocalizationSettings.AvailableLocales.Locales;
            var current = LocalizationSettings.SelectedLocale;
            int languageIndex = locales.IndexOf(current);

            if (languageIndex < 0 || languageIndex >= locales.Count) {
                languageIndex = 0;
                Debug.LogWarning("Selecting default locale. No stored locale or out of range?");
            }
           
            VolumesSnapshot snapshot = Events.Audio.OnRequestVolumes?.Invoke() ?? new VolumesSnapshot(1.0f);

            if (DEBUG) Debug.Log("Default settings loaded from Unity defaults");

            return new SettingsData(snapshot, languageIndex, qualityIndex);;
        }

    // ? EVENT METHODS==============================================================================================================================
        public SettingsData GetCurrentSettings() => this.currentSettings;
        private void SaveSettingsData() {
            VolumesSnapshot snapshot = Events.Audio.OnRequestVolumes?.Invoke() ?? new VolumesSnapshot(1.0f);

            this.currentSettings.SetSnapshotValues(snapshot);

            string path = Path.Combine(Application.persistentDataPath, settingsDataName);
            string json = JsonUtility.ToJson(this.currentSettings, true);
            File.WriteAllText(path, json);

            if (DEBUG) Debug.Log("Game settings data saved and stored in: " + path);
        }
        
        private void LoadSettingsData() {
            string path = Path.Combine(Application.persistentDataPath, settingsDataName);

            if (File.Exists(path)) {
                string json = File.ReadAllText(path);
                this.currentSettings = JsonUtility.FromJson<SettingsData>(json);

                if (DEBUG) Debug.Log("Settings loaded from JSON file");
            } else {
                this.currentSettings = this.GetDefaultsFromUnity();
                this.SaveSettingsData();

                Debug.LogWarning("Loading default settings. Is first time execution?");
            }
        }
    }
}

namespace App.Tools.Data {
    [Serializable]
    public class SettingsData {
        public VolumesSnapshot volumes;

        public int languageIndex = 0;
        public int qualityIndex = 0;

        public SettingsData(int lang_i, int quality_i) {
            this.volumes = new VolumesSnapshot(1.0f);

            this.languageIndex = lang_i;
            this.qualityIndex = quality_i;
        }

        public SettingsData(VolumesSnapshot vols_s, int lang_i, int quality_i) {
            this.volumes.masterVolume = vols_s.masterVolume;
            this.volumes.musicVolume = vols_s.musicVolume;
            this.volumes.uiVolume = vols_s.uiVolume;
            this.volumes.sfxVolume = vols_s.sfxVolume;
            this.volumes.atmosphereVolume = vols_s.atmosphereVolume;
            this.volumes.voiceVolume = vols_s.voiceVolume;

            this.languageIndex = lang_i;
            this.qualityIndex = quality_i;
        }

        public void SetSnapshotValues(VolumesSnapshot newVolumes)
        {
            
            this.volumes.masterVolume = newVolumes.masterVolume;
            this.volumes.musicVolume = newVolumes.musicVolume;
            this.volumes.uiVolume = newVolumes.uiVolume;
            this.volumes.sfxVolume = newVolumes.sfxVolume;
            this.volumes.atmosphereVolume = newVolumes.atmosphereVolume;
            this.volumes.voiceVolume = newVolumes.voiceVolume;
        }
    }
    
    /// <summary>
    /// Estructura simple que contiene los volúmenes actuales.
    /// </summary>
    [Serializable]
    public struct VolumesSnapshot {
        public float masterVolume;
        public float musicVolume;
        public float uiVolume;
        public float sfxVolume;
        public float atmosphereVolume;
        public float voiceVolume;

        public VolumesSnapshot(float defaultValue = 1.0f) {
            this.masterVolume = defaultValue;
            this.musicVolume = defaultValue;
            this.uiVolume = defaultValue;
            this.sfxVolume = defaultValue;
            this.atmosphereVolume = defaultValue;
            this.voiceVolume = defaultValue;
        }
    }

    [Serializable]
    public class SavestateData {
        
    }
}