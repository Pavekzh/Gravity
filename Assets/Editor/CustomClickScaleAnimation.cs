using UnityEditor;
using UnityEngine;
using Assets.UIComponents;

namespace Assets.Editor
{
    [CustomEditor(typeof(ClickScaleAnimation))]
    public class CustomClickScaleAnimation : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            SerializedProperty useScriptableProperty = serializedObject.FindProperty("useScriptableConfig");
            EditorGUILayout.PropertyField(useScriptableProperty);
            if (useScriptableProperty.boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("animationConfig"));
            }
            else
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("scaleFactor"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("duration"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("easeFunction"));
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}