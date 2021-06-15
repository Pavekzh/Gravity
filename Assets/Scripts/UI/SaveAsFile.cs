using UnityEngine;
using System.Collections;
using TMPro;

public class SaveAsFile : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] SaveSystem saveSytem;
    [SerializeField] string savesDirectory;
    public string FilePath
    {
        get
        {
            string text = inputField.text;
#if UNITY_EDITOR
            return Application.dataPath + savesDirectory+ "/" + text + ".xml";
#elif UNITY_ANDROID || UNITY_IOS
            return Application.persistentDataPath + savesDirectory+ "/" + text + ".xml";
#endif
        }
    }
    bool isPathValid = false;
    private void Start()
    {
        if(inputField == null)
        {
            ErrorManager.Instance.ShowErrorMessage("InputField property not set",this);
        }
        if(saveSytem == null)
        {
            ErrorManager.Instance.ShowErrorMessage("SaveSystem property not set",this);
        }
    }
    public void Save()
    {
        if (isPathValid)
        {
            saveSytem.SaveToFile(FilePath);
        }
    }
    public void ValueChanged()
    {
        if(inputField.text != "")
        {
            isPathValid = true;
        }
        else
        {
            isPathValid = false;
        }
    }
}
