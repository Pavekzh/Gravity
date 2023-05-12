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
    public class SceneStateLoader:MonoBehaviour
    {
        [SerializeField] SaveSystemFactory saveSystemFactory;
        [SerializeField] string savesDirectory = "Saves/";
        [SerializeField] string defaultName = "NewScene";
        [Header("Start scene")]
        [SerializeField] bool loadStartScene = true;
        [SerializeField] TextAsset startScene;
        [Header("Presets")]
        [SerializeField] string presetsDirectory = "Presets/Scenes/";
        [SerializeField] FileNamesCollectionScriptableObject presetsFileNames;

        private string currentFilePath;
        private SceneInstance sceneInstance;

        public FileNamesCollectionScriptableObject PresetsFileNames {get => presetsFileNames; }
        public string PresetsDirectory { get => BaseDirectory +"Resources/"+ presetsDirectory; }
        public string Directory { get => BaseDirectory + savesDirectory; }
        public string Extension { get => SaveSystem.Extension; }
        public ISaveSystem SaveSystem { get => saveSystemFactory.GetChachedSaveSystem(); }

        public SceneState LocalSave { get; private set; }

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

        private string GetSavePathFromName(string fileName)
        {
            return Directory + fileName + SaveSystem.Extension;
        }

        [Zenject.Inject]
        private void Construct(SceneInstance sceneInstance)
        {
            this.sceneInstance = sceneInstance;
            Initialize();
        }

        private void Initialize()
        {
            if (!System.IO.Directory.Exists(Directory))
                System.IO.Directory.CreateDirectory(Directory);

            currentFilePath = Directory + defaultName + SaveSystem.Extension;

            if (loadStartScene)
            {
                SetScene(LoadStartScene());
            }
        }

        public void SaveState(string fileName)
        {
            string fullPath = GetSavePathFromName(fileName);
            SaveSystem.Save(sceneInstance.CurrentScene, fullPath);
            currentFilePath = fullPath;
        }

        public void SaveState()
        {
            SaveSystem.Save(sceneInstance.CurrentScene, currentFilePath);
        }

        public void SaveLocal()
        {
            LocalSave = sceneInstance.CurrentScene.Clone() as SceneState;
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
            string fullPath = GetSavePathFromName(fileName);
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

        private SceneState LoadStartScene()
        {
            SceneState state;
            using (Stream stream = new MemoryStream(startScene.bytes))
            {
                state = SaveSystem.Load(stream, typeof(SceneState)) as SceneState;
            }

            return state;
        }

        public void LoadPreset(string fileName)
        {
            string resourcePath = presetsDirectory + fileName;
            currentFilePath = GetSavePathFromName(fileName);

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

        public void Delete(string fileName)
        {
            SaveSystem.Delete(GetSavePathFromName(fileName));
        }

        private void SetScene(SceneState sceneState,bool isSceneLocalSaved)
        {
            SceneState clonedState = sceneState.Clone() as SceneState;
            if(clonedState != null)
            {
                sceneInstance.ChangeScene(clonedState,isSceneLocalSaved);                     
            }

            SaveLocal();
        }

        private void SetScene(SceneState sceneState)
        {
            SetScene(sceneState, false);
        }



    }
}
