//Libraries
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Video;
using Dev.Compiler;
using UnityEngine;

namespace App.Managers {
    /// <summary>
    /// 
    /// </summary>
    public class NarrativeManager : MonoBehaviour {
    // ? DEBUG======================================================================================================================================
        [Header("Debug")]
        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private readonly static bool DEBUG = false;
    // ? PARAMETERS=================================================================================================================================

        // * REFERENCES
        //[Header("References")]

        
        // * ATTRIBUTES
        [Header("Attributes")]
        
        [SerializeField] private App.Tools.Data.NarrativeDatabase narrativeDB;

        // * INTERNALS
        [Header("Internals")]
        private readonly Queue<App.Tools.Data.NarrativeEntry> queue = new Queue<App.Tools.Data.NarrativeEntry>();
        private int dialogueIntanceId = -1;
        private bool isDisplaying = false;
        private App.Tools.Data.NarrativeEntry currentEntry;

    // ? BASE METHODS===============================================================================================================================
        /// <summary>
        /// 
        /// </summary>
        private void OnEnable() {
            Events.Narrative.OnReproduceSequenceById += this.DisplaySequence;
            Events.Narrative.OnReproduceEntryById += this.DisplayEntryByID;
            Events.Narrative.OnReproduceCustomEntry += this.DisplayEntry;

            Events.Narrative.OnSubtitleDisplayRegistered += this.SetDialogueDisplayReference;
            Events.Narrative.OnSubtitleDisplayUnregistered += this.UnsetDialogueDisplayReference;
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnDisable() {
            Events.Narrative.OnReproduceSequenceById -= this.DisplaySequence;
            Events.Narrative.OnReproduceEntryById -= this.DisplayEntryByID;
            Events.Narrative.OnReproduceCustomEntry -= this.DisplayEntry;
            
            Events.Narrative.OnSubtitleDisplayRegistered -= this.SetDialogueDisplayReference;
            Events.Narrative.OnSubtitleDisplayUnregistered -= this.UnsetDialogueDisplayReference;
        }
        
        /// <summary>
        /// 
        /// </summary>
        private void Awake() {
            narrativeDB?.Initialize();
        }
        
    // ? CUSTOM METHODS=============================================================================================================================
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private App.Tools.Data.NarrativeEntry GetCurrentEntry() => currentEntry;

        /// <summary>
        /// 
        /// </summary>
        private void UnsetDialogueDisplayReference() {
            if (DEBUG) Debug.Log("[NM] Entry unregistered");
            dialogueIntanceId = -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IEnumerator ProcessQueue() {
            isDisplaying = true;

            while (queue.Count > 0) {
                //Process current
                currentEntry = queue.Dequeue();
                if (currentEntry == null) continue;

                //Dialogue sync
                //if (currentEntry.type == App.Tools.Data.NarrativeType.Dialogue) Events.UI.OnSyncElementRequested?.Invoke(App.Tools.Data.SyncCategory.custom, dialogueIntanceId);
                
                //Pre delay handling
                App.Events.Narrative.OnSubtitleShow?.Invoke(currentEntry.subtitle);
                yield return new WaitForSeconds(currentEntry.preDelay);
                
                //Starting WaitCondition corroutine allowing fade skipping when applies
                if (currentEntry.pauseCondition == null) {
                    switch (currentEntry.condition) {
                        case App.Tools.Data.PauseCondition.timeout:
                            currentEntry.pauseCondition = new App.Tools.Data.TimeAwaitCondition();
                        break;
                        // case App.Tools.Data.PauseCondition.timeout_input:
                        //     currentEntry.pauseCondition = new Narrative.InputAwaitCondition();
                        // break;
                        // case App.Tools.Data.PauseCondition.event_:
                        //     currentEntry.pauseCondition = new Narrative.EventAwaitCondition();
                        // break;
                        // case App.Tools.Data.PauseCondition.timeout_event:
                        //     currentEntry.pauseCondition = new Narrative.EventAwaitCondition();
                        // break;
                        default:
                            Debug.LogWarning($"[NE] No valid pause condition found for {currentEntry.id}. Using TimeoutCondition fallback.");
                            currentEntry.pauseCondition = new App.Tools.Data.TimeAwaitCondition();
                        break;
                    }
                }

                //Enter actions managing
                if (currentEntry.actionsOnStart != null) foreach (var action in currentEntry.actionsOnStart) action?.Execute(currentEntry);

                //Fade-in process
                App.Events.Narrative.OnSubtitleFadeIn?.Invoke(currentEntry.fadeInDuration);
                yield return new WaitForSeconds(currentEntry.fadeInDuration);

                //Executing time await time elapsing
                float startTime = Time.time;

                yield return currentEntry.pauseCondition.WaitCoroutine(currentEntry);
                //yield return new WaitUntil(() => {
                //    bool pauseDone = waitCondition != null ? waitCondition.MoveNext() == false : false;
                //    bool timeout = currentEntry.autoSkip ? Time.time - startTime >= currentEntry.duration : false;
                //    return pauseDone || timeout;
                //});

                //Wait time (min 1 frame)
                float delay = Mathf.Max(0f, currentEntry.duration - Time.time - startTime);
                yield return currentEntry.forceSkip || delay <= Mathf.Epsilon ? null : new WaitForSeconds(delay);

                //Fade-out process
                App.Events.Narrative.OnSubtitleFadeOut?.Invoke(currentEntry.fadeOutDuration);
                yield return new WaitForSeconds(currentEntry.fadeOutDuration);

                //Exit actions managing
                if (currentEntry.actionsOnEnd != null) foreach (var actions in currentEntry.actionsOnEnd) actions?.Execute(currentEntry);
                
                //Post delay handling
                App.Events.Narrative.OnSubtitleHide?.Invoke();
                yield return new WaitForSeconds(currentEntry.postDelay);
                
                yield return new WaitForEndOfFrame();
            }

            isDisplaying = false;
        }
    
    // ? EVENT METHODS==============================================================================================================================
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        private void SetDialogueDisplayReference(int id) {
            dialogueIntanceId = id;
            if (DEBUG) Debug.Log($"[NM] Entry registered with ID {id}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void DisplayEntryByID(App.Tools.Data.NarrativeEntryID id) {
            var info = narrativeDB.GetEntryID(id);
            if (info == null) {
                Debug.LogWarning($"Narrative ID '{id}' not found in database.");
                return;
            }
            this.DisplayEntry(info);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        public void DisplayEntry(App.Tools.Data.NarrativeEntry entry) {
            queue.Enqueue(entry);
            if (!isDisplaying) StartCoroutine(ProcessQueue());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sequence"></param>
        public void DisplaySequence(App.Tools.Data.NarrativeSequenceID sequence) {
            var sequenceEntries = narrativeDB.GetSequenceID(sequence);
            if (sequenceEntries == null || sequenceEntries.Count == 0) return;
            
            foreach (var entry in sequenceEntries) queue.Enqueue(entry);
            if (!isDisplaying) StartCoroutine(ProcessQueue());
        }
    }
}

namespace App.Tools.Data {
    /// <summary>
    /// 
    /// </summary>
    public enum PauseCondition {
        timeout,
        timeout_skip,
        timeout_input,
        skip_input,
        timeout_skip_input,
        timeout_event,
        event_,
        event_skip,
        timeout_event_skip,
        event_input,
        timeout_event_input,
    }
}

namespace App.Tools.Data {
    [System.Serializable]
    /// <summary>
    /// 
    /// </summary>
    public sealed class NarrativeDatabase {
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
        [Header("Attributes")]
        // TODO: Probably add Narrative data structures to Game.Narrative instead of Tools.Data since those are only used under Narrative domains
        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private List<NamedSequence> dialogueThread = new List<NamedSequence>();
        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private List<NamedEntry> dialogueLine = new List<NamedEntry>();

        // * INTERNALS
        [Header("Internals")]
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<App.Tools.Data.NarrativeSequenceID, List<NarrativeEntry>> sequenceLookup;
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<App.Tools.Data.NarrativeEntryID, NarrativeEntry> entryLookup;

    // ? BASE METHODS===============================================================================================================================

    // ? CUSTOM METHODS=============================================================================================================================  
        /// <summary>
        /// 
        /// </summary>
        public void Initialize() {
            if (this.sequenceLookup == null) this.sequenceLookup = new Dictionary<App.Tools.Data.NarrativeSequenceID, List<NarrativeEntry>>();
            else this.sequenceLookup.Clear();

            if (this.entryLookup == null) this.entryLookup = new Dictionary<App.Tools.Data.NarrativeEntryID, NarrativeEntry>();
            else this.entryLookup.Clear();

            if (this.dialogueThread != null && this.dialogueThread.Count > 0) {
                foreach (NamedSequence threadSequence in this.dialogueThread) {
                    foreach (SequenceRecord sequenceRecord in threadSequence.SequenceRecords) {
                        if (!this.sequenceLookup.ContainsKey(sequenceRecord.Id)) this.sequenceLookup[sequenceRecord.Id] = sequenceRecord.Sequences;
                    }
                }
            }        

            if (this.dialogueLine != null && this.dialogueLine.Count > 0) {
                foreach (NamedEntry lineEntry in this.dialogueLine) {
                    foreach (EntryRecord entryRecord in lineEntry.entryRecords) {   
                        if (!this.entryLookup.ContainsKey(entryRecord.Id)) this.entryLookup[entryRecord.Id] = entryRecord.Entry;
                    }
                }
            }
        }

    // ? EVENT METHODS==============================================================================================================================
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public NarrativeEntry GetEntryID(App.Tools.Data.NarrativeEntryID id) {
            if (this.entryLookup == null) Initialize();
            return this.entryLookup.TryGetValue(id, out NarrativeEntry entry) ? entry : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<NarrativeEntry> GetSequenceID(App.Tools.Data.NarrativeSequenceID id) {
            if (this.sequenceLookup == null) Initialize();
            return this.sequenceLookup.TryGetValue(id, out List<NarrativeEntry> seq) ? seq : null;
        }   
        
        [System.Serializable]
        /// <summary>
        /// 
        /// </summary>
        private class NamedEntry {
            // * REFERENCES
            //[Header("References")]
            
            // * ATTRIBUTES
            [Header("Attributes")]
            #pragma warning disable CS0414
            [RuntimeLocked, SerializeField] 
            /// <summary>
            ///
            /// </summary>
            private string name = "Unnamed line";
            #pragma warning restore CS0414
            public List<EntryRecord> entryRecords => this.entries;

            // * INTERNALS
            [Header("Internals")]
            [SerializeField] private List<EntryRecord> entries = new List<EntryRecord>();
        }

        [System.Serializable]
        /// <summary>
        /// 
        /// </summary>
        private class NamedSequence {
            // * REFERENCES
            //[Header("References")]
            
            // * ATTRIBUTES
            [Header("Attributes")]
            #pragma warning disable CS0414
            [RuntimeLocked, SerializeField] 
            /// <summary>
            ///
            /// </summary>
            private string name = "Unnamed thread";
            #pragma warning restore CS0414
            public List<SequenceRecord> SequenceRecords => this.sequenceRecords;

            // * INTERNALS
            [Header("Internals")]
            [SerializeField] private List<SequenceRecord> sequenceRecords = new List<SequenceRecord>();
        }

        [System.Serializable]
        /// <summary>
        /// 
        /// </summary>
        private class EntryRecord {
            // * REFERENCES
            //[Header("References")]
            
            // * ATTRIBUTES
            [Header("Attributes")]
            #pragma warning disable CS0414
            [RuntimeLocked, SerializeField] 
            /// <summary>
            ///
            /// </summary>
            private string name = "Unnamed entry record"; //First attribute to display Editor class name properly
            #pragma warning restore CS0414
            public App.Tools.Data.NarrativeEntryID Id => this.id;
            public NarrativeEntry Entry => this.entry;

            // * INTERNALS
            [Header("Internals")]
            [SerializeField] private App.Tools.Data.NarrativeEntryID id;
            [SerializeField] private NarrativeEntry entry;
        }
        
        [System.Serializable]
        /// <summary>
        /// 
        /// </summary>
        private sealed class SequenceRecord {
            // * REFERENCES
            //[Header("References")]
            
            // * ATTRIBUTES
            [Header("Attributes")]
            #pragma warning disable CS0414
            [RuntimeLocked, SerializeField] 
            /// <summary>
            ///
            /// </summary>
            private string name = "Unnamed sequence record";
            #pragma warning restore CS0414
            public App.Tools.Data.NarrativeSequenceID Id => this.id;
            public List<NarrativeEntry> Sequences => this.sequences;
            
            // * INTERNALS
            [Header("Internals")]
            [SerializeField] private App.Tools.Data.NarrativeSequenceID id;
            [SerializeField] private List<NarrativeEntry> sequences = new List<NarrativeEntry>();
        }
    }
    
    [System.Serializable]
    public sealed class BaseSubtitleData {
        // * REFERENCES
        //[Header("References")]
        
        // * ATTRIBUTES
        [Header("Attributes")]
        /// <summary>
        /// 
        /// </summary>
        public Sprite icon = null;
        /// <summary>
        /// 
        /// </summary>
        [TextArea] public string author = "";
        /// <summary>
        /// 
        /// </summary>
        [TextArea] public string message = "";
        /// <summary>
        /// 
        /// </summary>
        public bool displayIcon = true;
        /// <summary>
        /// 
        /// </summary>
        public bool showAuthor = true;

        // * INTERNALS
        //[Header("Internals")]
    }

    [System.Serializable]
    public sealed class NarrativeEntry {
        // * REFERENCES
        //[Header("References")]
        
        // * ATTRIBUTES
        [Header("Attributes")]
        /// <summary>
        /// 
        /// </summary>
        #pragma warning disable CS0414
        [RuntimeLocked, SerializeField] 
        /// <summary>
        ///
        /// </summary>
        private string name = "Unnamed entry";
        #pragma warning restore CS0414
        
        [Header("Identity")]
        /// <summary>
        /// 
        /// </summary>
        public App.Tools.Data.NarrativeEntryID id;
        /// <summary>
        /// 
        /// </summary>
        public App.Tools.Data.NarrativeType type;

        [Header("Content")]
        /// <summary>
        /// 
        /// </summary>
        public App.Tools.Data.SoundClip sound;
        /// <summary>
        /// 
        /// </summary>
        public VideoClip video;
        /// <summary>
        /// 
        /// </summary>
        public BaseSubtitleData subtitle;

        [Header("Presentation")]
        /// <summary>
        /// 
        /// </summary>
        public Color textColor;
        /// <summary>
        /// 
        /// </summary>
        public bool blackScreen = false;

        [Header("Timing")]
        /// <summary>
        /// 
        /// </summary>
        [Min(0.0f)] public float preDelay = 0f;
        /// <summary>
        /// 
        /// </summary>
        public List<App.Tools.Data.NarrativeAction> actionsOnStart = new List<App.Tools.Data.NarrativeAction>();
        /// <summary>
        /// 
        /// </summary>
        [Min(0.0f)] public float fadeInDuration = 0.5f;
        /// <summary>
        /// 
        /// </summary>
        [Range(0.1f, 10.0f)] public float duration = 5.0f;
        /// <summary>
        /// 
        /// </summary>
        [Min(0.0f)] public float fadeOutDuration = 0.5f;
        /// <summary>
        /// 
        /// </summary>
        public List<App.Tools.Data.NarrativeAction> actionsOnEnd = new List<App.Tools.Data.NarrativeAction>();
        /// <summary>
        /// 
        /// </summary>
        [Min(0.0f)] public float postDelay = 0f;

        [Header("Conditions")]
        //TODO: Update to use polymorph and context menu to apply Open/Close principle - [SerializeReference]
        /// <summary>
        /// 
        /// </summary>
        public App.Tools.Data.PauseCondition condition = App.Tools.Data.PauseCondition.timeout;
        /// <summary>
        /// 
        /// </summary>
        public BasePauseCondition pauseCondition;
        /// <summary>
        /// 
        /// </summary>
        public bool evaluateAll = false;
        /// <summary>
        /// 
        /// </summary>
        public bool forceSkip = false;
        /// <summary>
        /// 
        /// </summary>
        public bool pressSkip = false;
        /// <summary>
        /// 
        /// </summary>
        public bool autoSkip = false;
    }

    [System.Serializable]
    /// <summary>
    /// 
    /// </summary>
    public abstract class BasePauseCondition {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public abstract IEnumerator WaitCoroutine(NarrativeEntry entry);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="innerCoroutine"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        protected IEnumerator WaitWithShortcut(IEnumerator innerCoroutine, NarrativeEntry entry) {
            if (PlayerInput.all.Count == 0) yield return innerCoroutine;

            if (entry.pressSkip && PlayerInput.all.Count > 0) {
                PlayerInput playerInput = PlayerInput.all[0];
                InputAction submitAction = playerInput.actions.FindAction("Submit", throwIfNotFound: false);
                if (submitAction != null) {
                    bool submitPressed = false;
                    void OnPerformed(InputAction.CallbackContext ctx) => submitPressed = true;
                    submitAction.performed += OnPerformed;

                    while (!submitPressed && innerCoroutine.MoveNext())
                        yield return innerCoroutine.Current;

                    submitAction.performed -= OnPerformed;
                    yield break;
                }
            }

            yield return innerCoroutine;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="innerCoroutine"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        protected IEnumerator WaitWithTimeout(IEnumerator innerCoroutine, NarrativeEntry entry) {
            float elapsed = 0.0f;
            
            IEnumerator timeoutCoroutine() {
                while (innerCoroutine.MoveNext() && (!entry.autoSkip || elapsed < entry.duration)) {
                    elapsed += Time.deltaTime;
                    yield return innerCoroutine;
                }
            }

            yield return WaitWithShortcut(timeoutCoroutine(), entry);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="forceTime"></param>
        /// <returns></returns>
        protected IEnumerator WaitWithTimeout(NarrativeEntry entry, bool forceTime = false) {
            float elapsed = 0.0f;
            bool autoSkip = forceTime ? true : entry.autoSkip;
            
            IEnumerator timeoutCoroutine() {
                while (!autoSkip || elapsed < entry.duration) {
                    elapsed += Time.deltaTime;
                    yield return null;
                }
            }

            yield return WaitWithShortcut(timeoutCoroutine(), entry);
        }
    }

    [System.Serializable]
    /// <summary>
    /// 
    /// </summary>
    public sealed class TimeAwaitCondition : BasePauseCondition {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public override IEnumerator WaitCoroutine(NarrativeEntry entry) {
            yield return WaitWithTimeout(entry, true);
        }
    }

    [System.Serializable]
    /// <summary>
    /// 
    /// </summary>
    public sealed class InputAwaitCondition : BasePauseCondition {
        /// <summary>
        /// 
        /// </summary>
        public string inputActionName = "Submit";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        public InputAwaitCondition(string i = "Submit") {
            inputActionName = i;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public override IEnumerator WaitCoroutine(NarrativeEntry entry) {
            if (PlayerInput.all.Count == 0) yield break;
            
            //
            IEnumerator waitInput() {
                PlayerInput playerInput = PlayerInput.all[0];
                InputAction action = playerInput.actions.FindAction(this.inputActionName, throwIfNotFound: false);
                if (action == null) yield break;

                bool pressed = false;
                void OnPerformed(InputAction.CallbackContext ctx) => pressed = true;
                action.performed += OnPerformed;

                yield return new WaitUntil(() => pressed);

                action.performed -= OnPerformed;
            }

            yield return this.WaitWithTimeout(waitInput(), entry);
        }
    }

    [System.Serializable]
    /// <summary>
    /// 
    /// </summary>
    public sealed class EventAwaitCondition : BasePauseCondition {
        /// <summary>
        /// 
        /// </summary>
        public System.Action registerEvent;
        /// <summary>
        /// 
        /// </summary>
        public System.Action unregisterEvent;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public override IEnumerator WaitCoroutine(NarrativeEntry entry) {
            bool eventTrigger = false;
            System.Action handler = () => eventTrigger = true;

            this.registerEvent?.Invoke();
            this.registerEvent += handler;

            //
            IEnumerator waitEvent() {
                yield return new WaitUntil(() => eventTrigger);
                this.unregisterEvent?.Invoke();
                this.registerEvent -= handler;
            }

            yield return this.WaitWithShortcut(this.WaitWithTimeout(waitEvent(), entry), entry);
        }
    }

    [System.Serializable]
    /// <summary>
    /// 
    /// </summary>
    public sealed class SoundAwaitCondition : BasePauseCondition {
        /// <summary>
        /// 
        /// </summary>
        public AudioSource audioSource;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public override IEnumerator WaitCoroutine(NarrativeEntry entry) {
            if (this.audioSource == null) yield break;
            
            //
            IEnumerator waitSound() {
                yield return new WaitUntil(() => !this.audioSource.isPlaying);
            }

            yield return this.WaitWithShortcut(this.WaitWithTimeout(waitSound(), entry), entry);
        }
    }

    [System.Serializable]
    /// <summary>
    /// 
    /// </summary>
    public sealed class ClipAwaitCondition : BasePauseCondition {
        /// <summary>
        /// 
        /// </summary>
        public VideoPlayer videoPlayer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public override IEnumerator WaitCoroutine(NarrativeEntry entry) {
            if (this.videoPlayer == null) yield break;

            //
            IEnumerator waitClip() {
                yield return new WaitUntil(() => !this.videoPlayer.isPlaying);
            }

            yield return this.WaitWithShortcut(this.WaitWithTimeout(waitClip(), entry), entry);
        }
    }
}

namespace App.Tools.Data {
    [System.Serializable]
    /// <summary>
    /// 
    /// </summary>
    public sealed class NarrativeAction {
        /// <summary>
        /// 
        /// </summary>
        public NarrativeActionType actionType;
        /// <summary>
        /// 
        /// </summary>
        public string triggerID;
        /// <summary>
        /// 
        /// </summary>
        public bool toggleValue = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void Execute(App.Tools.Data.NarrativeEntry data) {
            switch (actionType) {
                default:
                case NarrativeActionType.DebugLog:
                    Debug.Log($"[NarrativeAction] {data.subtitle.message}");
                break;

                case NarrativeActionType.ReproduceSound:
                    if (data.sound != null) Events.Audio.OnPlayClipUniversally?.Invoke(data.sound);
                    else Debug.LogWarning("No AudioClip assigned to PlaySound action.");
                break;

                case NarrativeActionType.TriggerEvent:
                    TriggerGameEvent(triggerID);
                break;

                case NarrativeActionType.ChangeScene:
                    // Usa tu sistema de router o SceneManager.LoadScene
                break;

                case NarrativeActionType.ToggleInput:
                    ToggleGameplayInput(toggleValue);
                break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enable"></param>
        private void ToggleGameplayInput(bool enable) {
            foreach (var playerInput in PlayerInput.all) {
                var playerMap = playerInput.actions.FindActionMap("Player", throwIfNotFound: false);
                var uiMap = playerInput.actions.FindActionMap("UI", throwIfNotFound: false);

                if (playerMap != null) {
                    if (enable) {
                        playerMap.Enable();
                    } else {
                        playerMap.Disable();
                    }
                }

                if (uiMap != null && !uiMap.enabled) {
                    uiMap.Enable();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        private void TriggerGameEvent(string id) {
            if (id == "wake") Events.Narrative.OnPlayerWakeUp?.Invoke();
            //else if (if == )
        }
    }

    [System.Serializable]
    /// <summary>
    /// 
    /// </summary>
    public enum NarrativeEntryID {
        disconnect,
        reconnect,

        intro_01,
        intro_02,
        intro_03,
        intro_04,
        intro_05,

        mogura_complaint,
        mogura_what,
        mogura_but,
        mogura_shoot,
        mogura_how_bout,
        mogura_i_am,
        mogura_fine,
        mogura_see_you,

        murci_wait,
        murci_who,
        murci_take,
        murci_mission,
        murci_see_you,

        old_scream,
        old_good_luck,

        sister_ill_do_it,
        sister_strong,

        tuto_l1,
        tuto_l2,
        tuto_idk,
        tuto_dig,
    }

    [System.Serializable]
    /// <summary>
    /// 
    /// </summary>
    public enum NarrativeType {
        None,
        Dialogue,
        Instruction,
        Label,
        Title,
        Achievement,
    }

    [System.Serializable]
    /// <summary>
    /// 
    /// </summary>
    public enum NarrativeActionType {
        DebugLog,
        PlayClip,
        ReproduceSound,
        TriggerEvent,
        ChangeScene,
        ToggleInput
    }

    [System.Serializable]
    /// <summary>
    /// 
    /// </summary>
    public enum NarrativeSequenceID {
        None,
        Intro,
        Tutorial,
        Mission
    }

    [System.Serializable]
    /// <summary>
    /// 
    /// </summary>
    public enum WaitConditionType {
        Time,
        Input,
        Event,
        Sound,
        Clip,
    }
}