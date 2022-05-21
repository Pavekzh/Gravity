using System;
using System.IO;
using System.Xml.Serialization;
using BasicTools;
using System.Collections.Generic;
using Assets.SceneEditor.Models;
using UnityEngine;
using Assets.SceneSimulation;


namespace Assets.Services
{
    public class SceneStateManager:Singleton<SceneStateManager>
    {
        [SerializeField] SaveSystemFactory saveSystemFactory;
        [SerializeField] string savesDirectory = "Saves/";
        [SerializeField] string defaultName = "NewScene";
        [SerializeField] string startScenePath = "";
        [SerializeField] TextAsset startScene;

        public string Directory { get => BaseDirectory + savesDirectory; }
        public string Extension { get => saveSystem.Extension; }
        public delegate void SceneRefreshHandler();
        public event SceneRefreshHandler SceneChanged;
        public static string BaseDirectory
        {
            get
            {
#if UNITY_EDITOR
                return Application.dataPath + "/";
#elif UNITY_ANDROID || UNITY_IOS
            return Application.persistentDataPath +"/";
#endif
            }
        }
        public SceneState CurrentScene { get; private set; }
        public SceneState LocalSave { get; set; }

        private ISaveSystem saveSystem;
        private string currentFilePath;

        protected override void Awake()
        {
            base.Awake();
            if (!System.IO.Directory.Exists(Directory))
                System.IO.Directory.CreateDirectory(Directory);

            CurrentScene = new SceneState();
            saveSystem = saveSystemFactory.GetSaveSystem();
            currentFilePath = Directory + defaultName + saveSystem.Extension;
        }

        private void Start()
        {
            SetScene(LoadStartScene());
            SaveLocal();
        }

        public void AddPlanet(PlanetData planet)
        {
            CurrentScene.Planets.Add(planet);
        }

        public void SaveState(string fileName)
        {
            string fullPath = GetPath(fileName);
            saveSystem.Save(CurrentScene, fullPath);
            currentFilePath = fullPath;
        }

        public void SaveState()
        {
            saveSystem.Save(CurrentScene, currentFilePath);
        }

        public void SaveLocal()
        {
            LocalSave = CurrentScene.Clone() as SceneState;
        }

        public void LoadLocal()
        {
            if(LocalSave != null)
            {
                SetScene(LocalSave);
            }
        }

        public void Load(string fileName)
        {
            string fullPath = GetPath(fileName);
            currentFilePath = fullPath;

            SceneState state = saveSystem.Load(fullPath, typeof(SceneState)) as SceneState;
            if (state != null)
            {
                SetScene(state);
            }
            else
                ErrorManager.Instance.ShowErrorMessage("Saved state didn't load properly ", this);
        }

        public void Load()
        {
            SceneState state = saveSystem.Load(currentFilePath, typeof(SceneState)) as SceneState;
            if (state != null)
            {
                SetScene(state);
            }
            else
                ErrorManager.Instance.ShowErrorMessage("Saved state didn't load properly ", this);

        }

        public void Delete(string fileName)
        {
            saveSystem.Delete(GetPath(fileName));
        }

        public void ClearScene()
        {
            if (PlanetBuildSettings.Instance.PlanetsParent != null)
            {
                foreach(Transform planet in PlanetBuildSettings.Instance.PlanetsParent)
                {
                    planet.gameObject.SetActive(false);
                    GameObject.Destroy(planet.gameObject);
                }
            }

            CurrentScene = new SceneState();            
            SceneChanged?.Invoke();
        }

        public void SaveStartScene(SceneState startScene)
        {
            if (startScenePath != "")
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SceneState));
                File.Delete(startScenePath);
                using (FileStream file = new FileStream(startScenePath, FileMode.OpenOrCreate))
                {
                    serializer.Serialize(file, startScene);
                }
            }
        }

        private SceneState LoadStartScene()
        {
            StringReader stringReader = new System.IO.StringReader(startScene.text);
            XmlSerializer serializer = new XmlSerializer(typeof(SceneState));
            return serializer.Deserialize(stringReader) as SceneState;

        }

        private void SetScene(SceneState sceneState)
        {
            ClearScene();
            SceneState clonedState = sceneState.Clone() as SceneState;
            if(clonedState != null)
            {

                foreach (PlanetData planet in clonedState.Planets)
                {
                    planet.CreateSceneObject();
                }
                CurrentScene = clonedState;                
                SceneChanged?.Invoke();
            }
        }        

        private string GetPath(string fileName)
        {
            return Directory + fileName + saveSystem.Extension;
        }

    }
}
