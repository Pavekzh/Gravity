using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Library;
using System.Xml.Serialization;
using System.IO;

public class SaveSystem:MonoBehaviour
{
    private SceneState quickSave;
    [SerializeField] string filePath;

    public void QuickSave()
    {
        try
        {
            quickSave = SceneStateManager.Instance.GetState();
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
                SceneStateManager.Instance.RefreshScene(quickSave);
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
            SceneState state = SceneStateManager.Instance.GetState();
            XmlSerializer serializer = new XmlSerializer(typeof(SceneState));
            using (FileStream file = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                serializer.Serialize(file, state);
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
            SceneState state;
            XmlSerializer serializer = new XmlSerializer(typeof(SceneState));
            using (FileStream file = new FileStream(filePath, FileMode.OpenOrCreate))
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
        catch (System.Exception ex)
        {
            ErrorManager.Instance.ShowErrorMessage(ex.Message, this);
        }
    }
    public void SaveToFile(string path)
    {
        try
        {
            this.filePath = path;
            SceneState state = SceneStateManager.Instance.GetState();
            XmlSerializer serializer = new XmlSerializer(typeof(SceneState));
            using (FileStream file = new FileStream(path, FileMode.OpenOrCreate))
            {
                serializer.Serialize(file, state);
            }
        }
        catch (System.Exception ex)
        {
            ErrorManager.Instance.ShowErrorMessage(ex.Message, this);
        }
    }
    public void LoadFromFile(string path)
    {
        try
        {
            this.filePath = path;
            SceneState state;
            XmlSerializer serializer = new XmlSerializer(typeof(SceneState));
            using (FileStream file = new FileStream(path, FileMode.OpenOrCreate))
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
        catch (System.Exception ex)
        {
            ErrorManager.Instance.ShowErrorMessage(ex.Message, this);
        }
    }
}
