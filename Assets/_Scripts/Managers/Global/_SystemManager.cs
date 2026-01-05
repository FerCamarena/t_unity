//Libraries
using UnityEngine.Localization.Settings;
using System.Collections.Generic;
using UnityEngine.Localization;
using UnityEngine;

namespace App.Tools {
    /// <summary>
    /// 
    /// </summary>
    public static class _SystemManager {
        /// <summary>
        /// 
        /// </summary>
        public readonly static int default_quality_index = 0;
        /// <summary>
        /// 
        /// </summary>
        public readonly static int default_max_fps = 30;
        /// <summary>
        /// 
        /// </summary>
        public readonly static int none_locale_index = -1;
        /// <summary>
        /// 
        /// </summary>
        public readonly static int default_locale_index = 0;
        
        /// <summary>
        /// 
        /// </summary>
        public static List<Locale> Locales => GetAvailableLocales();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int GetQualityCount() => QualitySettings.names.Length;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static App.Tools.Data.SettingsData GetCurrentUnitySettings(){
            //Volumes
            App.Tools.Data.VolumesSnapshot snapshot = App.Tools.Data.VolumesSnapshot.Default();

            //Quality
            int qualityIndex = GetSelectedQualityIndex();

            //Locale
            int localeIndex = GetSelectedLocaleIndex();

            //Returning with default volume values
            return new App.Tools.Data.SettingsData(snapshot, localeIndex, qualityIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string[] GetAvailableQualityLevels() {
            // TODO: Update to quality levels per platform
            
            return QualitySettings.names;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int GetSelectedQualityIndex() => QualitySettings.GetQualityLevel();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public static void SetQuality(int index) {
            if (index < 0 || index >= GetQualityCount()) index = default_quality_index;
            QualitySettings.SetQualityLevel(index, applyExpensiveChanges: true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fps"></param>
        public static void SetFPS(int fps) {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = fps;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Locale> GetAvailableLocales() {
            List<Locale> listedLocales = LocalizationSettings.AvailableLocales?.Locales;
            
            //Localization config are not set yet
            if (listedLocales == null || listedLocales.Count == 0) {
                Debug.LogWarning("No locales available. Are localization settings missing or empty?");
                return null;
            }

            return listedLocales;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int GetSelectedLocaleIndex() {
            //Prevent any modification if no locales are available
            if (Locales == null) return none_locale_index;

            Locale selected = LocalizationSettings.SelectedLocale;

            //Missing references may end with -1 if locale out of range
            int index = Locales.IndexOf(selected);
            if (index < 0) {
                Debug.LogWarning("Locale not found or out of range. Are locale references missing?");
                return none_locale_index;
            }

            return index;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public static void ApplySettingToUnity(App.Tools.Data.SettingsData data) {
            if (data == null) {
                Debug.LogError("[SM] Cannot apply null settings: null SettingsData received from save!");
                return;
            }

            //Quality
            SetQuality(data.qualityIndex);

            //Locale
            SetLocale(data.localeIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool SetLocale(int index) {
            //Prevent any modification if no locales are available
            if (Locales == null) return false;

            //Return without modification if index not valid
            if (index < 0 || index >= Locales.Count) {
                Debug.LogError("Locale not changed: out of range!");
                return false;
            }

            //Impede re assigning same index since Unity may update the entire system but marking as proceeded
            if (GetSelectedLocaleIndex() == index) return true;

            //Applying locale
            LocalizationSettings.SelectedLocale = Locales[index];

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static void CycleLocale() {
            //Prevent any modification if no locales are available
            if (Locales == null) return;

            //Cycling only valid if there are at least 1 locale
            int currentIndex = GetSelectedLocaleIndex();

            //Prevent out of range if references were modified
            if (currentIndex == none_locale_index) currentIndex = default_locale_index;
            int newIndex = (currentIndex + 1) % Locales.Count;

            //Selecting locale
            SetLocale(newIndex);
        }
    }
}