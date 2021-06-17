using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Library;
using System.Xml.Serialization;
using System.IO;

public class SaveSystem:MonoBehaviour
{
    private SceneState quickSave;
    [SerializeField] string savesDirectory;
    [SerializeField] string filePath;
    [SerializeField] SceneStateManager sceneStateSystem;

    public string SavesDirectory
    {
        get
        {
#if UNITY_EDITOR
            return Application.dataPath + savesDirectory;
#elif UNITY_ANDROID || UNITY_IOS
            return Application.persistentDataPath + savesDirectory;
#endif
        }
    }
    public string FilePath
    {
        get => SavesDirectory + "/" + filePath + ".xml";
        set => filePath = value;
    }

    public void QuickSave()
    {
        try
        {
            quickSave = sceneStateSystem.GetState();
        }
        catch(System.Exception ex)
        {
            ErrorManager.Instance.ShowErrorMessage(ex.Message, this);
        }

    }
    public void LoadQuickSave()
    {
        try
        {
            if (quickSave != null)
                sceneStateSystem.RefreshScene(quickSave);
        }
        catch (System.Exception ex)
        {
            ErrorManager.Instance.ShowErrorMessage(ex.Message, this);
        }
    }

    public void SaveToFile()
    {
        try
        {
            if(filePath != "")
            {
                SceneState state = sceneStateSystem.GetState();
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
            if(filePath != "")
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
                        sceneStateSystem.RefreshScene(state);
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
        }
        catch (System.Exception ex)
        {
            ErrorManager.Instance.ShowErrorMessage(ex.Message, this);
        }
    }
}
