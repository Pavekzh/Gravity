using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Assets.Services;
using Assets.SceneEditor.Models;
using Assets.SceneSimulation;
using System.Linq;

[CustomEditor(typeof(SceneStateManager))]
public class CustomSceneState:Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        SceneStateManager manager = target as SceneStateManager;
        PlanetData[] planets = new PlanetData[0];

        if (GUILayout.Button("Save start scene"))
        {

            SceneState sceneState = new SceneState();            
            sceneState.Name = "SavedByInspector";
            foreach(Transform planet in PlanetBuildSettings.Instance.PlanetsParent.transform)
            {
                Module[] modules = planet.GetComponents<Module>();

                if(modules.Length != 0)
                {
                    Dictionary<string, ModuleData> modulesData = new Dictionary<string, ModuleData>();

                    foreach(Module module in modules)
                    {
                        ModuleData moduleData = module.InstatiateModuleData();
                        modulesData.Add(moduleData.Name, moduleData);
                    }

                    PlanetData planetData = new PlanetData(modulesData,planet.name, new BasePlanetBuilder());

                    sceneState.Planets.Add(planetData);
                }
            }
            manager.SaveStartScene(sceneState);

        }
    }
}
