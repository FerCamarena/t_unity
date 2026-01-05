using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine;
using System;

namespace App.Managers {
    /// <summary>
    /// 
    /// </summary>
    public sealed class AudioManager : MonoBehaviour {
    // ? DEBUG======================================================================================================================================
        //[Header("Debug")]
        /// <summary>
        /// 
        /// </summary>
        private static bool DEBUG => false;

    // ? PARAMETERS=================================================================================================================================
        // * REFERENCES
        [Header("References")]
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private AudioMixer masterMixer;
        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private AudioMixerGroup masterGroup;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private AudioMixerGroup musicGroup;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private AudioMixerGroup uiGroup;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private AudioMixerGroup sfxGroup;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private AudioMixerGroup atmosphereGroup;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private AudioMixerGroup voiceGroup;

        // * ATTRIBUTES
        [Header("Attributes")]
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        private Dictionary<App.Tools.Data.MixerChannel, AudioMixerGroup> mixerGroups;
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private List<AudioSource> sourcesPool = new List<AudioSource>();
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        [SerializeField] private App.Tools.Data.SoundClip menuSound;

        // * INTERNALS
        [Header("Internals")]
        /// <summary>
        /// 
        /// </summary>
        [Tooltip("")]
        private Dictionary<App.Tools.Data.GenericTag, AudioSource> universalRegistry = new Dictionary<App.Tools.Data.GenericTag, AudioSource>();

    // ? BASE METHODS===============================================================================================================================
        /// <summary>
        /// 
        /// </summary>
        private void OnEnable() {
            App.Events.Audio.OnRequestVolumes += this.GetVolumesSnapshot;

            App.Events.Audio.OnApplyVolumesSnapshot += this.SetVolumesSnapshot;
            App.Events.Audio.OnApplyChannelVolume += this.SetChannelVolume;

            App.Events.Audio.OnPlayClipUniversally += this.PlayUniversalClip;
            App.Events.Audio.OnStopUniversalByTag += this.StopUniversalByTag;
            App.Events.Audio.OnStopAllUniversal += this.StopAllUniversal;
            
            App.Events.Application.OnAppOpened += this.LoadVolumeSettings;
        }
        
        /// <summary>
        /// 
        /// </summary>
        private void OnDisable() {
            App.Events.Audio.OnRequestVolumes -= this.GetVolumesSnapshot;

            App.Events.Audio.OnApplyVolumesSnapshot -= this.SetVolumesSnapshot;
            App.Events.Audio.OnApplyChannelVolume -= this.SetChannelVolume;
            
            App.Events.Audio.OnPlayClipUniversally -= this.PlayUniversalClip;
            App.Events.Audio.OnStopUniversalByTag -= this.StopUniversalByTag;
            App.Events.Audio.OnStopAllUniversal -= this.StopAllUniversal;
            
            App.Events.Application.OnAppOpened -= this.LoadVolumeSettings;
        }

        /// <summary>
        /// 
        /// </summary>
        private void Awake() {
            this.mixerGroups = new Dictionary<App.Tools.Data.MixerChannel, AudioMixerGroup> {
                { App.Tools.Data.MixerChannel.Master, masterGroup },
                { App.Tools.Data.MixerChannel.Music, musicGroup },
                { App.Tools.Data.MixerChannel.UI, uiGroup },
                { App.Tools.Data.MixerChannel.SFX, sfxGroup },
                { App.Tools.Data.MixerChannel.Atmosphere, atmosphereGroup },
                { App.Tools.Data.MixerChannel.Voice, voiceGroup },
            };

            this.ValidateReferences();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Start() {
            // TODO: Update to handle calls from local managers to implement its own scene sound or request transitions
            if (this.menuSound != null && SceneManager.GetActiveScene().buildIndex == 0) this.PlayUniversalClip(this.menuSound);
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnValidate() {
            this.ValidateReferences();
        }

    // ? CUSTOM METHODS=============================================================================================================================
        /// <summary>
        /// 
        /// </summary>
        private void ValidateReferences() {
            if (!this.masterMixer) Debug.LogError("Invalid or null required MasterMixer reference!");
            #if UNITY_EDITOR
                else {            
                    foreach (App.Tools.Data.MixerChannel channel in Enum.GetValues(typeof(App.Tools.Data.MixerChannel))) {
                        string paramName = channel.ToString() + "_Volume";
                        float value;
                        if (!this.masterMixer.GetFloat(paramName, out value)) {
                            Debug.LogError($"AudioChannel enum '{channel}' has no exposed parameter named: '{paramName}'!");
                        }
                    }
                }
            #endif
        }
        
        /// <summary>
        /// 
        /// </summary>
        private AudioSource GetFreeSource() {
            if (this.sourcesPool == null) return null;

            foreach (AudioSource src in this.sourcesPool) {
                if (!src.isPlaying) return src;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadVolumeSettings() {
            //Requesting current settings to SavesManager
            App.Tools.Data.SettingsData settings = App.Events.Saves.OnRequestCurrentSettings?.Invoke();

            //Handling no event return
            if (settings == null) {
                Debug.LogError("[AM] No settings found or no listener to request!");
                return;
            }

            //Volumes
            this.SetVolumesSnapshot(settings.volumesSnapshot);
        }

    // ? EVENT METHODS==============================================================================================================================
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private App.Tools.Data.VolumesSnapshot GetVolumesSnapshot() {
            return new App.Tools.Data.VolumesSnapshot {
                masterVolume = this.GetChannelVolume(App.Tools.Data.MixerChannel.Master),
                musicVolume = this.GetChannelVolume(App.Tools.Data.MixerChannel.Music),
                uiVolume = this.GetChannelVolume(App.Tools.Data.MixerChannel.UI),
                sfxVolume = this.GetChannelVolume(App.Tools.Data.MixerChannel.SFX),
                atmosphereVolume = this.GetChannelVolume(App.Tools.Data.MixerChannel.Atmosphere),
                voiceVolume = this.GetChannelVolume(App.Tools.Data.MixerChannel.Voice)
            };
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public float GetChannelVolume(App.Tools.Data.MixerChannel param) {
            if (this.masterMixer.GetFloat($"{param}_Volume", out float db))
                return Dev.Audio.DecibelToLinear(db);
            return Dev.Audio.DecibelToLinear(0.5f);
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <param name="normalized"></param>
        public void SetChannelVolume(App.Tools.Data.MixerChannel param, float normalized) => this.masterMixer.SetFloat($"{param}_Volume", Dev.Audio.LinearToDecibel(normalized));
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="snapshot"></param>
        private void SetVolumesSnapshot(App.Tools.Data.VolumesSnapshot snapshot) {
            this.SetChannelVolume(App.Tools.Data.MixerChannel.Master, snapshot.masterVolume);
            this.SetChannelVolume(App.Tools.Data.MixerChannel.Music, snapshot.musicVolume);
            this.SetChannelVolume(App.Tools.Data.MixerChannel.UI, snapshot.uiVolume);
            this.SetChannelVolume(App.Tools.Data.MixerChannel.SFX, snapshot.sfxVolume);
            this.SetChannelVolume(App.Tools.Data.MixerChannel.Atmosphere, snapshot.atmosphereVolume);
            this.SetChannelVolume(App.Tools.Data.MixerChannel.Voice, snapshot.voiceVolume);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private void PlayUniversalClip(App.Tools.Data.SoundClip data) {
            if (data == null) {
                Debug.LogWarning("[AM] Tried to play null SoundClipData.");
                return;
            }

            AudioClip clip = data.GetClip();
            if (clip == null) {
                Debug.LogWarning($"[AM] SoundClipData {data.Tag} has no AudioClip.");
                return;
            }

            AudioSource source;
            if (data.Scope == App.Tools.Data.PlaybackScope.Universal) {
                if (universalRegistry.TryGetValue(data.Tag, out AudioSource existingSource)) {
                    source = existingSource;

                    if (source.clip != clip) {
                        source.Stop();
                        source.clip = clip;
                    }
                } else {
                    source = gameObject.AddComponent<AudioSource>();
                    universalRegistry[data.Tag] = source;
                }
            }
            else {
                source = gameObject.AddComponent<AudioSource>();
            }

            source.clip = clip;
            source.volume = data.Volume;
            source.loop = data.Loop;
            source.playOnAwake = false;

            if (mixerGroups.TryGetValue(data.Channel, out var group))
                source.outputAudioMixerGroup = group;

            source.Play();

            if (data.Scope != App.Tools.Data.PlaybackScope.Universal && !data.Loop) Destroy(source, clip.length + 0.1f);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="fadeOut"></param>
        /// <param name="fadeTime"></param>
        private void StopUniversalByTag(App.Tools.Data.GenericTag tag, bool fadeOut = false, float fadeTime = 1f) {
            if (!universalRegistry.TryGetValue(tag, out var source) || source == null) return;

            if (fadeOut) StartCoroutine(FadeOutAndStop(source, fadeTime));
            else source.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        private IEnumerator FadeOutAndStop(AudioSource source, float duration) {
            float startVol = source.volume;
            float t = 0f;

            while (t < duration) {
                t += Time.unscaledDeltaTime;
                source.volume = Mathf.Lerp(startVol, 0f, t / duration);
                yield return null;
            }

            source.Stop();
            source.volume = startVol;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="newClip"></param>
        /// <param name="crossfadeTime"></param>
        public void TransitionTo(App.Tools.Data.GenericTag tag, App.Tools.Data.SoundClip newClip, float crossfadeTime = 1.5f) {
            if (!universalRegistry.TryGetValue(tag, out var currentSource) || currentSource == null) {
                PlayUniversalClip(newClip);
                return;
            }
            
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            newSource.clip = newClip.GetClip();
            newSource.volume = 0f;
            newSource.loop = newClip.Loop;
            newSource.playOnAwake = false;

            if (mixerGroups.TryGetValue(newClip.Channel, out var group))
                newSource.outputAudioMixerGroup = group;

            newSource.Play();

            StartCoroutine(CrossfadeTracks(currentSource, newSource, newClip, tag, crossfadeTime));
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldSource"></param>
        /// <param name="newSource"></param>
        /// <param name="data"></param>
        /// <param name="tag"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        private IEnumerator CrossfadeTracks(AudioSource oldSource, AudioSource newSource, App.Tools.Data.SoundClip data, App.Tools.Data.GenericTag tag, float duration) {
            float t = 0f;
            float oldVol = oldSource.volume;

            while (t < duration) {
                t += Time.unscaledDeltaTime;
                float f = t / duration;
                oldSource.volume = Mathf.Lerp(oldVol, 0f, f);
                newSource.volume = Mathf.Lerp(0f, data.Volume, f);
                yield return null;
            }

            oldSource.Stop();
            Destroy(oldSource);
            universalRegistry[tag] = newSource;
        }

        /// <summary>
        /// 
        /// </summary>
        private void StopAllUniversal() {
            foreach (var kvp in this.universalRegistry) kvp.Value.Stop();

            this.universalRegistry.Clear();
        }
    }
}