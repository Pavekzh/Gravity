using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using BasicTools;
using System.Collections.Generic;
using Assets.SceneEditor.Models;
using UnityEngine;
using Assets.SceneSimulation;


namespace Assets.Services
{
    public class SceneStateManager : Singleton<SceneStateManager>
    {
        [SerializeField] SaveSystemFactory saveSystemFactory;
        [SerializeField] string savesDirectory = "Saves/";
        [SerializeField] string defaultName = "NewScene";
        [Header("Start scene")]
        [SerializeField] bool loadStartScene = true;
        [SerializeField] string startScenePath = "";
        [SerializeField] TextAsset startScene;
        [Header("Presets")]
        [SerializeField] string presetsDirectory = "Presets/Scenes/";
        [SerializeField] string presetName = "New preset";
        [SerializeField] FileNamesCollectionScriptableObject presetsFileNames;

        public FileNamesCollectionScriptableObject PresetsFileNames {get => presetsFileNames; }
        public string PresetsDirectory { get => BaseDirectory +"Resources/"+ presetsDirectory; }
        public string PresetName { get => presetName; }
        public string Directory { get => BaseDirectory + savesDirectory; }
        public string Extension { get => SaveSystem.Extension; }
        public ISaveSystem SaveSystem { get => saveSystemFactory.GetChachedSaveSystem(); }
        public SceneState CurrentScene { get; private set; }
        public SceneState LocalSave { get; set; }
        public bool IsLoadedSceneLocalSaved { get; private set; }


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

        private string currentFilePath;

        protected override void Awake()
        {
            base.Awake();
            if (!System.IO.Directory.Exists(Directory))
                System.IO.Directory.CreateDirectory(Directory);

            CurrentScene = new SceneState();
            currentFilePath = Directory + defaultName + SaveSystem.Extension;
        }

        private void Start()
        {
            if (loadStartScene)
            {
                SetScene(LoadStartScene());
            }
        }

        public void AddPlanet(PlanetData planet)
        {
            CurrentScene.Planets.Add(planet);
        }

        public void SaveState(string fileName)
        {
            string fullPath = GetSavePath(fileName);
            SaveSystem.Save(CurrentScene, fullPath);
            currentFilePath = fullPath;
        }

        public void SaveState()
        {
            SaveSystem.Save(CurrentScene, currentFilePath);
        }

        public void SaveLocal()
        {
            LocalSave = CurrentScene.Clone() as SceneState;
        }

        public void LoadLocal()
        {
            if(LocalSave != null)
            {
                SetScene(LocalSave,true);
            }
        }

        public void Load(string fileName)
        {
            string fullPath = GetSavePath(fileName);
            currentFilePath = fullPath;

            SceneState state = SaveSystem.Load(fullPath, typeof(SceneState)) as SceneState;
            if (state != null)
            {
                SetScene(state);
            }
            else
                MessagingSystem.Instance.ShowErrorMessage("Saved state didn't load properly ", this);
        }

        public void Load()
        {
            SceneState state = SaveSystem.Load(currentFilePath, typeof(SceneState)) as SceneState;
            if (state != null)
            {
                SetScene(state);
            }
            else
                MessagingSystem.Instance.ShowErrorMessage("Saved state didn't load properly ", this);

        }

        public void Delete(string fileName)
        {
            SaveSystem.Delete(GetSavePath(fileName));
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
            IsLoadedSceneLocalSaved = true;
            SceneChanged?.Invoke();
        }

        public void SaveStartScene(SceneState startScene)
        {
            if (startScenePath != "")
            {
                SaveSystem.Save(startScene, startScenePath);
            }
        }

        private SceneState LoadStartScene()
        {
            SceneState state;
            using (Stream stream = new MemoryStream(startScene.bytes))
            {
                state = SaveSystem.Load(stream, typeof(SceneState)) as SceneState;
            }

            return state;
        }

        public void SavePreset(SceneState preset)
        {
           if(presetsDirectory != "")
            {
                SaveSystem.Save(preset, PresetsDirectory + presetName + SaveSystem.Extension);
                if (!PresetsFileNames.Collection.Contains(preset.Name))
                {
                    PresetsFileNames.Collection.Add(preset.Name);
                }
            }
        }

        public void LoadPreset(string fileName)
        {
            string resourcePath = presetsDirectory + fileName;
            currentFilePath = GetSavePath(fileName);

            try
            {
                TextAsset textFile = Resources.Load<TextAsset>(resourcePath);
                using(MemoryStream stream = new MemoryStream(textFile.bytes))
                {
                    SceneState state = (SceneState)SaveSystem.Load(stream, typeof(SceneState));
                    if(state != null)
                    {
                        SetScene(state);
                    }
                    else
                    {
                        MessagingSystem.Instance.ShowErrorMessage("Saved state didn't load properly ", this);
                    }
                }
            }
            catch(Exception ex)
            {
                BasicTools.MessagingSystem.Instance.ShowErrorMessage(ex.Message, this);
            }


        }

        private void SetScene(SceneState sceneState,bool isSceneLocalSaved)
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
                IsLoadedSceneLocalSaved = isSceneLocalSaved;           
                SceneChanged?.Invoke();
            }

            SaveLocal();
        }

        private void SetScene(SceneState sceneState)
        {
            SetScene(sceneState, false);
        }

        private string GetSavePath(string fileName)
        {
            return Directory + fileName + SaveSystem.Extension;
        }

    }
}
