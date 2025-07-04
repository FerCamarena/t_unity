using UnityEngine.Audio;
using UnityEngine;

namespace App.Game.Managers {
    public class _AudioManager : MonoBehaviour {
    // ? DEBUG======================================================================================================================================
    [SerializeField] private bool DEBUG = false;

    // ? PARAMETERS=================================================================================================================================
        // * REFERENCES
        [SerializeField] private AudioMixer MasterMixer;
    // ? BASE METHODS===============================================================================================================================
        private void OnEnable() {
            Events.Settings.OnMasterVolumeUpdated += UpdateMasterChannelVolume;
            Events.Settings.OnMusicVolumeUpdated += UpdateMusicChannelVolume;
            Events.Settings.OnUIVolumeUpdated += UpdateUIChannelVolume;
            Events.Settings.OnSFXVolumeUpdated += UpdateSFXChannelVolume;
            Events.Settings.OnAtmosphereVolumeUpdated += UpdateAtmosphereChannelVolume;
            Events.Settings.OnVoiceVolumeUpdated += UpdateVoiceChannelVolume;
        }
        
        private void OnDisable() {
            Events.Settings.OnMasterVolumeUpdated -= UpdateMasterChannelVolume;
            Events.Settings.OnMusicVolumeUpdated -= UpdateMusicChannelVolume;
            Events.Settings.OnUIVolumeUpdated -= UpdateUIChannelVolume;
            Events.Settings.OnSFXVolumeUpdated -= UpdateSFXChannelVolume;
            Events.Settings.OnAtmosphereVolumeUpdated -= UpdateAtmosphereChannelVolume;
            Events.Settings.OnVoiceVolumeUpdated -= UpdateVoiceChannelVolume;
        }
    // ? CUSTOM METHODS=============================================================================================================================
        private void UpdateMasterChannelVolume(float newValue) => this.MasterMixer.SetFloat("master_vol", Tools.Audio.LinearToDecibel(newValue));
        private void UpdateMusicChannelVolume(float newValue) => this.MasterMixer.SetFloat("music_vol", Tools.Audio.LinearToDecibel(newValue));
        private void UpdateUIChannelVolume(float newValue) => this.MasterMixer.SetFloat("ui_vol", Tools.Audio.LinearToDecibel(newValue));
        private void UpdateSFXChannelVolume(float newValue) => this.MasterMixer.SetFloat("sfx_vol", Tools.Audio.LinearToDecibel(newValue));
        private void UpdateAtmosphereChannelVolume(float newValue) => this.MasterMixer.SetFloat("atmosphere_vol", Tools.Audio.LinearToDecibel(newValue));
        private void UpdateVoiceChannelVolume(float newValue) => this.MasterMixer.SetFloat("voice_vol", Tools.Audio.LinearToDecibel(newValue));
    // ? EVENT METHODS==============================================================================================================================
    }
}