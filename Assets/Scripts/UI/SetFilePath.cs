using UnityEngine;
using System.Collections;
using TMPro;

public class SetFilePath : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] SaveSystem saveSytem;

    public string FilePath
    {
        get
        {
            return inputField.text;
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
    public void SetPath()
    {
        if (isPathValid)
        {
            saveSytem.FilePath = FilePath;
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
