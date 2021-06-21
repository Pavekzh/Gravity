using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Library;
using System.Xml.Serialization;
using System.IO;


public class SaveSystem:MonoBehaviour
{
    [SerializeField] string savesDirectory;
    [SerializeField] string defaulFilePath = "NewScene";
    [SerializeField] FilePathProvider pathProvider;
    [SerializeField] string filePath;
    public string SavesDirectory
    {
        get
        {
#if UNITY_EDITOR
            return Application.dataPath +"/"+ savesDirectory;
#elif UNITY_ANDROID || UNITY_IOS
            return Application.persistentDataPath +"/"+ savesDirectory;
#endif
        }
    }

    public string FilePath { get => filePath; set => filePath = value; }

    private void Start()
    {
        if (pathProvider != null)
            pathProvider.PathChanged += PathChanged;
        else
            ErrorManager.Instance.ShowErrorMessage("PathProvider has not set", this);

        FilePath = SavesDirectory +"/"+ defaulFilePath + ".xml";
    }

    public void SaveToFile()
    {
        try
        {
            if(FilePath != "")
            {
                SceneState state = SceneStateManager.Instance.GetState();
                XmlSerializer serializer = new XmlSerializer(typeof(SceneState));            
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
    public void LoadFromFile()
    {
        try
        {
            if(FilePath != "")
            {
                SceneState state;
                XmlSerializer serializer = new XmlSerializer(typeof(SceneState));
                using (FileStream file = new FileStream(FilePath, FileMode.OpenOrCreate))
                {
                    state = serializer.Deserialize(file) as SceneState;
                    if (state == null)
                    {
                        ErrorManager.Instance.ShowErrorMessage("File have invalid format or doesn't exist", this);
                    }
                    else
                    {
                        SceneStateManager.Instance.RefreshScene(state);
                    }
                }
            }

        }
        catch (System.Exception ex)
        {
            ErrorManager.Instance.ShowErrorMessage(ex.Message, this);
        }
    }
    public void DeleteFile()
    {
        try
        {
            File.Delete(FilePath);
            pathProvider.Init(SavesDirectory, ".xml");
        }
        catch (System.Exception ex)
        {
            ErrorManager.Instance.ShowErrorMessage(ex.Message, this);
        }
    }
    public void InitPathProvider()
    {
        pathProvider.Init(this.SavesDirectory, ".xml");
    }

    private void PathChanged(string path,object sender)
    {
        this.FilePath = path;
    }
}
