﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Assets.Services;
using Assets.SceneEditor.Models;
using Assets.SceneEditor.Controllers;
using Assets.SceneSimulation;


namespace Assets.Editor
{
    public class PlanetSaveWindow : EditorWindow
    {
        private enum SaveSystemEnum
        {
            XmlSaveSystem
        }

        private SaveSystemEnum saveSystemEnum;
        private string savesDirectory = "Planets/";
        private ISaveSystem saveSystem;

        private Module[] selectedPlanetModules;

        private string Directory { get => Services.SceneStateManager.BaseDirectory + savesDirectory; }

        public ISaveSystem SaveSystem
        {
            get
            {
                if (saveSystem == null)
                {
                    switch (saveSystemEnum)
                    {
                        case SaveSystemEnum.XmlSaveSystem:
                            return saveSystem = new XMLSaveSystem();
                        default:
                            return saveSystem = new XMLSaveSystem();
                    }
                }
                else return saveSystem;

            }
        }

        [MenuItem("Window/Planet save")]
        private static void ShowWindow()
        {
            var window = GetWindow<PlanetSaveWindow>();
            window.titleContent.text = "PlanetSave";
            window.autoRepaintOnSceneChange = true;
        }

        private void OnGUI()
        {
            savesDirectory = EditorGUILayout.TextField("Saves directory", savesDirectory);
            saveSystemEnum = (SaveSystemEnum)EditorGUILayout.EnumPopup("Save system",saveSystemEnum);
            if(Selection.activeGameObject != null)
            {
                selectedPlanetModules = Selection.activeGameObject.GetComponents<Module>();
                if(selectedPlanetModules.Length == 0)
                {
                    GUILayout.Label("Selected game object has no modules");
                    return;
                }
            }
            else
            {
                GUILayout.Label("No game object selected");
                return;
            }     
            
            if (GUILayout.Button("Save planet"))
                this.SavePlanet();
        }

        private void SavePlanet()
        {
            Module[] modules = Selection.activeGameObject.GetComponents<Module>();

            if (modules.Length != 0)
            {
                Dictionary<string, ModuleData> modulesData = new Dictionary<string, ModuleData>();

                foreach (Module module in modules)
                {
                    ModuleData moduleData = module.InstatiateModuleData();
                    modulesData.Add(moduleData.Name, moduleData);
                }

                PlanetData planetData = new PlanetData(modulesData, Selection.activeGameObject.name, new DefaultSceneObjectBuilder());
                SaveSystem.Save(planetData, Directory + Selection.activeGameObject.name + saveSystem.Extension);
            }
        }
    }
}