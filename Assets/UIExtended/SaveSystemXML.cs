using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using BasicTools;

namespace UIExtended 
{
    public class SaveSystemXML : MonoBehaviour
    {
        [SerializeField] string savesDirectory;
        [SerializeField] string defaultFilePath = "NewFile";
        [SerializeField] FilePathProvider pathProvider;
        [SerializeField] string filePath;

        public string Directory
        {
            get
            {
#if UNITY_EDITOR
                return Application.dataPath + "/" + savesDirectory;
#elif UNITY_ANDROID || UNITY_IOS
            return Application.persistentDataPath +"/"+ savesDirectory;
#endif
            }
        }
        public string FilePath { get => filePath; set => filePath = value; }

        private void Awake()
        {
            if (pathProvider != null)
            {
                pathProvider.Directory = this.Directory;
                pathProvider.FileExtension = ".xml";
                pathProvider.PathChanged += PathChanged;
                if (!System.IO.Directory.Exists(Directory))
                {
                    System.IO.Directory.CreateDirectory(Directory);
                }
            }
            else
                ErrorManager.Instance.ShowWarningMessage("PathProvider has not set", this);


        }

        private void Start()
        {
            FilePath = Directory + "/" + defaultFilePath + ".xml";
        }

        public void SaveToFile(object state)
        {
            try
            {
                if (FilePath != "")
                {
                    XmlSerializer serializer = new XmlSerializer(state.GetType());
                    File.Delete(FilePath);
                    using (FileStream file = new FileStream(FilePath, FileMode.OpenOrCreate))
                    {
                        serializer.Serialize(file, state);
                    }
                }

            }
            catch (System.Exception ex)
            {
                ErrorManager.Instance.ShowErrorMessage(ex.Message, this);
            }

        }

        public object LoadFromFile(System.Type objectType)
        {
            try
            {
                if (FilePath != "")
                {
                    object state;
                    XmlSerializer serializer = new XmlSerializer(objectType);
                    using (FileStream file = new FileStream(FilePath, FileMode.OpenOrCreate))
                    {

                        state = serializer.Deserialize(file);
                        return state;
                    }
                }

            }
            catch (System.Exception ex)
            {
                ErrorManager.Instance.ShowErrorMessage(ex.Message, this);
            }
            return null;
        }

        public void DeleteFile()
        {
            try
            {
                File.Delete(FilePath);
            }
            catch (System.Exception ex)
            {
                ErrorManager.Instance.ShowErrorMessage(ex.Message, this);
            }
        }

        private void PathChanged(string path, object sender)
        {
            this.FilePath = path;
        }
    }

}

