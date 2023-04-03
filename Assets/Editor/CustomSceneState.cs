using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Assets.Services;
using Assets.SceneEditor.Models;
using Assets.SceneSimulation;
using System.Linq;

namespace Assets.Editor
{
    [CustomEditor(typeof(SceneStateManager))]
    public class CustomSceneState : UnityEditor.Editor
    {
        private enum SaveSystemEnum
        {
            XmlSaveSystem
        }


        public override void OnInspectorGUI()
        {

            DrawDefaultInspector();
            SceneStateManager manager = target as SceneStateManager;
            PlanetData[] planets = new PlanetData[0];

            if (GUILayout.Button("Save start scene"))
            {
                SceneState startScene = FormSceneState("Start scene");
                manager.SaveStartScene(startScene);

            }

            if(GUILayout.Button("Save preset scene"))
            {
                SceneState presetScene = FormSceneState(manager.PresetName);
                manager.SavePreset(presetScene);
            }

            if(GUILayout.Button("Check file names"))
            {
                CheckFileNames(manager);
            }
        }
        private void CheckFileNames(SceneStateManager manager)
        {
            string[] names = System.IO.Directory.GetFiles(manager.PresetsDirectory, "*" + manager.SaveSystem.Extension);
            manager.PresetsFileNames.Collection.Clear();
            foreach (string n in names)
            {
                string name = System.IO.Path.GetFileNameWithoutExtension(n);
                manager.PresetsFileNames.Collection.Add(name);
            }
        }

        private SceneState FormSceneState(string name)
        {
            SceneState sceneState = new SceneState();
            sceneState.Gravity = GravityManager.Instance.GravityRatio;
            sceneState.Name = name;
            foreach (Transform planet in PlanetBuildSettings.Instance.PlanetsParent.transform)
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

                    PlanetData planetData = new PlanetData(modulesData, planet.name, new DefaultSceneObjectBuilder());

                    sceneState.Planets.Add(planetData);
                }
            }
            return sceneState;
        }
    }
}