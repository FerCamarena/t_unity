using UnityEngine.Localization.Settings;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace App.Managers {
    public sealed class SavesManager : MonoBehaviour {
    // ? DEBUG======================================================================================================================================
        //[Header("Debug")]
        /// <summary>
        /// 
        /// </summary>
         private static bool DEBUG => false;

    // ? PARAMETERS=================================================================================================================================
        // * REFERENCES
        //[Header("References")]
        
        // * ATTRIBUTES
        //[Header("Attributes")]

        // * INTERNAL
        [Header("Internals")]
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        private static readonly string settingsDataName = "settings_data";
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private App.Tools.Data.SettingsData storedSettings;

    // ? BASE METHODS===============================================================================================================================
        /// <summary>
        /// 
        /// </summary>
        private void OnEnable() {
            App.Events.Application.OnAppOpened += this.OnLoadSettingsData;

            App.Events.Settings.OnSettingsSaved += this.OnSaveSettingsData;

            App.Events.Saves.OnRequestCurrentSettings += this.OnGetCurrentSettings;
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnDisable() {
            App.Events.Application.OnAppOpened -= this.OnLoadSettingsData;

            App.Events.Settings.OnSettingsSaved -= this.OnSaveSettingsData;

            App.Events.Saves.OnRequestCurrentSettings -= this.OnGetCurrentSettings;
        }

    // ? CUSTOM METHODS==============================================================================================================================

    // ? EVENT METHODS==============================================================================================================================
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public App.Tools.Data.SettingsData OnGetCurrentSettings() => this.storedSettings;

        /// <summary>
        /// 
        /// </summary>
        private void OnSaveSettingsData() {
            if (this.storedSettings == null) return;

            //Volumes from mixer
            App.Tools.Data.VolumesSnapshot snapshot = App.Events.Audio.OnRequestVolumes?.Invoke() ?? App.Tools.Data.VolumesSnapshot.Default();
            
            //Accessibility (Quality & Language)
            this.storedSettings = App.Tools._SystemManager.GetCurrentUnitySettings();

            //Replacing values with actual mixer values or keeping defaults
            if (!App.Tools.Audio.ApproximatelyEqual(this.storedSettings.volumesSnapshot, snapshot)) this.storedSettings.SetSnapshotValues(snapshot);

            //Saving to JSON
            string path = Path.Combine(Application.persistentDataPath, settingsDataName);
            string json = JsonUtility.ToJson(this.storedSettings, true);
            File.WriteAllText(path, json);

            if (DEBUG) Debug.Log("[SM] Game settings data saved and stored in: " + path);
        }
        
        /// <summary>
        /// 
        /// </summary>
        private void OnLoadSettingsData() {
            string path = Path.Combine(Application.persistentDataPath, settingsDataName);

            if (File.Exists(path)){
                string json = File.ReadAllText(path);
                this.storedSettings = JsonUtility.FromJson<App.Tools.Data.SettingsData>(json);

                if (DEBUG) Debug.Log("[SM] Settings data loaded from JSON file");
            } else {
                this.storedSettings = App.Tools._SystemManager.GetCurrentUnitySettings();
                Debug.LogWarning("[SM] Loading default settings data. Is it first time execution?");
            }

            if (this.storedSettings.qualityIndex < 0 || this.storedSettings.qualityIndex >= App.Tools._SystemManager.GetQualityCount()) {
                Debug.LogWarning($"[SM] Stored quality index is out of range. Setting current to default.");

                this.storedSettings.qualityIndex = App.Tools._SystemManager.default_quality_index;
            }

            if (App.Tools._SystemManager.GetAvailableLocales() == null) this.storedSettings.localeIndex = App.Tools._SystemManager.none_locale_index;
            else if (this.storedSettings.localeIndex < 0 || this.storedSettings.localeIndex >= App.Tools._SystemManager.GetAvailableLocales().Count) {
                Debug.LogWarning($"[SM] Stored index locale is out of range. Setting current to default.");

                this.storedSettings.localeIndex = App.Tools._SystemManager.default_locale_index;
            }

            App.Tools._SystemManager.ApplySettingToUnity(this.storedSettings);
        }
    }
}