#if UNITY_EDITOR
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(Dev.Compiler.RuntimeLocked))]
    /// <summary>
    /// 
    /// </summary>
    public class InspectorOnlyDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            bool wasEnabled = GUI.enabled;

            if (Application.isPlaying) GUI.enabled = false;

            EditorGUI.PropertyField(position, property, label, true);
            
            GUI.enabled = wasEnabled;
        }
    }
#endif