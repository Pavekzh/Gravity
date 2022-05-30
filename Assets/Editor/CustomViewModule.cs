using UnityEditor;
using UnityEngine;
using Assets.SceneSimulation;


[CustomEditor(typeof(ViewDefinitionModule))]
public class CustomViewModule : Editor
{
    ViewDefinitionModule module;
    Editor settingsEditor;

    private void OnEnable()
    {
        module = target as ViewDefinitionModule;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if(module.SettingsObject != null)
        {
            module.SettingsFoldout = EditorGUILayout.InspectorTitlebar(module.SettingsFoldout, module.SettingsObject);
            if (module.SettingsFoldout)
            {
                CreateCachedEditor(module.SettingsObject, null, ref settingsEditor);
                settingsEditor.OnInspectorGUI();
            }
        }

    }
}
