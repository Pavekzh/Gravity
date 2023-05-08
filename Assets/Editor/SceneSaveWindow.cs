using UnityEditor;
using UnityEngine;
using Assets.Services;
using Assets.SceneEditor.Models;
using Assets.SceneSimulation;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Editor
{
    [System.Serializable]
    public class SceneSaveWindow : EditorWindow
    {
        private SceneSaveWindowConfig config;
        private GameObject sceneObject;

        private bool configFoldout = true;
        private UnityEditor.Editor configEditor;

        private ISaveSystem saveSystem { get => config.SaveSystemFactory.GetChachedSaveSystem(); }

        [MenuItem("Tools/Saving/SceneSave")]
        static void ShowWindow()
        {
            var window = GetWindow<SceneSaveWindow>();
            window.titleContent.text = "SceneSave";
            window.autoRepaintOnSceneChange = true;
        }

        private void OnGUI()
        {            
            config = EditorGUILayout.ObjectField("Config", config, typeof(SceneSaveWindowConfig),false) as SceneSaveWindowConfig;
            if (config != null)
            {
                configFoldout = EditorGUILayout.InspectorTitlebar(configFoldout, config);
                if (configFoldout)
                {
                    UnityEditor.Editor.CreateCachedEditor(config, null, ref configEditor);
                    configEditor.OnInspectorGUI();
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Saving");
            sceneObject = EditorGUILayout.ObjectField("Scene object", sceneObject, typeof(GameObject), true) as GameObject;

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();

            if(GUILayout.Button("Start Scene"))
            {
                SaveStartScene();
            }

            if(GUILayout.Button("Preset Scene"))
            {
                SavePresetScene();
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            if(GUILayout.Button("Check file names"))
            {
                CheckFileNames();
            }

        }

        private void SaveStartScene()
        {
            Debug.Log("Saved Start scene");

            SceneState state = FormSceneState(config.StartSceneName);
            saveSystem.Save(state, config.StartSceneDirectory + config.StartSceneName + saveSystem.Extension);

        }

        private void SavePresetScene()
        {
            Debug.Log("Saved Preset scene");

            SceneState state = FormSceneState(config.PresetName);
            saveSystem.Save(state, config.PresetsDirectory + config.PresetName + saveSystem.Extension);
            if (!config.PresetFileNames.Collection.Contains(state.Name))
            {
                config.PresetFileNames.Collection.Add(state.Name);
            }
        }

        private void CheckFileNames()
        {
            Debug.Log("Check file names");

            string[] files = System.IO.Directory.GetFiles(config.PresetsDirectory, "*" + saveSystem.Extension);

            IEnumerable<string> names = files.Select(x => System.IO.Path.GetFileNameWithoutExtension(x));
            config.PresetFileNames.Collection = config.PresetFileNames.Collection.Intersect(names).ToList();
            config.PresetFileNames.Collection = config.PresetFileNames.Collection.Union(names).ToList();

        }

        private SceneState FormSceneState(string SceneName)
        {
            SceneState sceneState = new SceneState();
            sceneState.Gravity = FindObjectOfType<GravityComputer>().GravityRatio;
            sceneState.Name = SceneName;
            foreach (Transform planet in sceneObject.transform)
            {
                Module[] modules = planet.GetComponents<Module>();

                if (modules.Length != 0)
                {
                    Dictionary<string, ModuleData> modulesData = new Dictionary<string, ModuleData>();

                    foreach (Module module in modules)
                    {
                        ModuleData moduleData = module.InstatiateModuleData();
                        modulesData.Add(moduleData.Name, moduleData);
                    }

                    PlanetData planetData = new PlanetData(modulesData, planet.name);

                    sceneState.Planets.Add(planetData);
                }
            }
            return sceneState;
        }

        
    }
}