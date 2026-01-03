using System.Collections.Generic;
using UnityEngine.Audio;
using App.Game.Audio;
using App.Tools.Data;
using UnityEngine;
using System;

namespace App.Game.Managers {
    public class _AudioManager : MonoBehaviour {
    // ? DEBUG======================================================================================================================================
    [SerializeField] private static bool DEBUG = false;

    // ? PARAMETERS=================================================================================================================================
        // * REFERENCES
        [SerializeField] private AudioMixer masterMixer;
        [SerializeField] private List<AudioSource> sourcesPool = new List<AudioSource>();

        // * ATTRIBUTES

        // * INTERNAL
        private VolumesSnapshot factoryDefaults;

    // ? BASE METHODS===============================================================================================================================
        private void OnEnable() {
            Events.Audio.OnRequestVolumes += this.GetCurrentVolumes;

            Events.Audio.OnApplyVolumes += this.ApplyVolumes;
            Events.Audio.OnApplyChannelVolume += this.SetChannelVolume;
            

            Events.Audio.OnPlayClipUniversally += this.PlayUniversalSound;
        }
        
        private void OnDisable() {
            Events.Audio.OnRequestVolumes -= this.GetCurrentVolumes;

            Events.Audio.OnApplyVolumes -= this.ApplyVolumes;
            Events.Audio.OnApplyChannelVolume -= this.SetChannelVolume;
            
            Events.Audio.OnPlayClipUniversally -= this.PlayUniversalSound;
        }

        private void Awake() {
            this.ValidateReferences();
        }

        private void Start() {
            this.LoadSettingsOnStart();
        }

        private void OnValidate() {
            this.ValidateReferences();
        }

    // ? CUSTOM METHODS=============================================================================================================================
        private void ValidateReferences() {
            if (!this.masterMixer) Debug.LogError("Invalid or null required MasterMixer reference!");
            else {            
                #if UNITY_EDITOR
                    foreach (Tools.Audio.MixerChannel channel in Enum.GetValues(typeof(Tools.Audio.MixerChannel))) {
                        string paramName = channel.ToString() + "_Volume"; // Convención: MasterVolume, MusicVolume...
                        float value;
                        if (!this.masterMixer.GetFloat(paramName, out value)) {
                            Debug.LogWarning($"AudioChannel enum '{channel}' no tiene un parámetro expuesto en el MasterMixer llamado '{paramName}'");
                        }
                    }
                #endif
            }
        }
        
        private AudioSource GetFreeSource() {
            if (this.sourcesPool == null) return null;

            foreach (AudioSource src in this.sourcesPool) {
                if (!src.isPlaying) return src;
            }
            return null;
        }

        private void LoadSettingsOnStart() {
            SettingsData settings = Events.Saves.OnRequestCurrentSettings?.Invoke();

            this.ApplyVolumes(settings.volumes);
        }

    // ? EVENT METHODS==============================================================================================================================
        private VolumesSnapshot GetFactoryDefaults() => factoryDefaults;
        // TODO: Update to set values from SavesManager SettingsData class instead
        // TODO: Update to handle visual updates from individual UI objects with IUpdatableUI elements by events
        // private void UpdateMasterChannelVolume(float newValue) => this.masterMixer.SetFloat("master_vol", Tools.Audio.LinearToDecibel(newValue));
        // private void UpdateMusicChannelVolume(float newValue) => this.masterMixer.SetFloat("music_vol", Tools.Audio.LinearToDecibel(newValue));
        // private void UpdateUIChannelVolume(float newValue) => this.masterMixer.SetFloat("ui_vol", Tools.Audio.LinearToDecibel(newValue));
        // private void UpdateSFXChannelVolume(float newValue) => this.masterMixer.SetFloat("sfx_vol", Tools.Audio.LinearToDecibel(newValue));
        // private void UpdateAtmosphereChannelVolume(float newValue) => this.masterMixer.SetFloat("atmosphere_vol", Tools.Audio.LinearToDecibel(newValue));
        // private void UpdateVoiceChannelVolume(float newValue) => this.masterMixer.SetFloat("voice_vol", Tools.Audio.LinearToDecibel(newValue));
        public void SetChannelVolume(string param, float normalized) => this.masterMixer.SetFloat(param + "_Volume", Tools.Audio.LinearToDecibel(normalized));
        private void ApplyVolumes(VolumesSnapshot snapshot) {
            this.SetChannelVolume(Tools.Audio.MixerChannel.Master.ToString(), snapshot.masterVolume);
            this.SetChannelVolume(Tools.Audio.MixerChannel.Music.ToString(), snapshot.musicVolume);
            this.SetChannelVolume(Tools.Audio.MixerChannel.UI.ToString(), snapshot.uiVolume);
            this.SetChannelVolume(Tools.Audio.MixerChannel.SFX.ToString(), snapshot.sfxVolume);
            this.SetChannelVolume(Tools.Audio.MixerChannel.Atmosphere.ToString(), snapshot.atmosphereVolume);
            this.SetChannelVolume(Tools.Audio.MixerChannel.Voice.ToString(), snapshot.voiceVolume);
        }

        public float GetChannelVolume(string param) {
            if (masterMixer.GetFloat(param + "_Volume", out float db))
                return Tools.Audio.DecibelToLinear(db);
            return 1f;
        }
        
        private VolumesSnapshot GetCurrentVolumes() {
            VolumesSnapshot snapshot = new VolumesSnapshot();
            
            snapshot.masterVolume = this.GetChannelVolume(Tools.Audio.MixerChannel.Master.ToString());
            snapshot.musicVolume = this.GetChannelVolume(Tools.Audio.MixerChannel.Music.ToString());
            snapshot.uiVolume = this.GetChannelVolume(Tools.Audio.MixerChannel.UI.ToString());
            snapshot.sfxVolume = this.GetChannelVolume(Tools.Audio.MixerChannel.SFX.ToString());
            snapshot.atmosphereVolume = this.GetChannelVolume(Tools.Audio.MixerChannel.Atmosphere.ToString());
            snapshot.voiceVolume = this.GetChannelVolume(Tools.Audio.MixerChannel.Voice.ToString());

            return snapshot;
        }


        // TODO: Update with custom events to receive from local managers
        private void PlayUniversalSound(SoundClipData clipData) {
            AudioSource source = GetFreeSource();

            if (source == null) {
                source = this.gameObject.AddComponent<AudioSource>();
                
                source.playOnAwake = false;
                if (this.sourcesPool == null) this.sourcesPool = new List<AudioSource>();
                this.sourcesPool.Add(source);
            }

            source.volume = clipData.Volume;
            source.clip = clipData.GetClip();
            source.Play();
        }
    }
}