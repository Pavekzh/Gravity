using UnityEditor;
using UnityEngine;
using Assets.SceneSimulation;

namespace Assets.Editor
{
    [CustomEditor(typeof(ViewDefinitionModule))]
    public class CustomViewModule : UnityEditor.Editor
    {
        ViewDefinitionModule module;
        UnityEditor.Editor settingsEditor;
        SerializedProperty settingsObjectProperty;
        SerializedProperty autoUpdateProperty;

        private void OnEnable()
        {
            module = target as ViewDefinitionModule;
            settingsObjectProperty = serializedObject.FindProperty("settingsObject");
            autoUpdateProperty = serializedObject.FindProperty("autoUpdate");
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (module.SettingsObject != null)
            {
                module.SettingsFoldout = EditorGUILayout.InspectorTitlebar(module.SettingsFoldout, module.SettingsObject);
                if (module.SettingsFoldout)
                {
                    CreateCachedEditor(module.SettingsObject, null, ref settingsEditor);

                    using (var check = new EditorGUI.ChangeCheckScope())
                    {
                        settingsEditor.OnInspectorGUI();
                        if (check.changed && module.AutoUpdate)
                        {
                            module.UpdateView();
                        }
                    }
                }
            }
            if (GUILayout.Button("Update view"))
            {
                module.UpdateView();
            }

            if(GUILayout.Button("Update material"))
            {
                module.RecreateModuleData();
            }
        }
    }
}