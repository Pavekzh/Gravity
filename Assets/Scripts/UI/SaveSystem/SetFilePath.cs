using UnityEngine;
using System.Collections;
using TMPro;
using Assets.Library;

public class SetFilePath : FilePathProvider
{
    [SerializeField] TMP_InputField inputField;

    public string FilePath
    {
        get
        {
            return directoryPath +"/"+ inputField.text + extension;
        }
    }
    private string directoryPath;
    private string extension;

    public override event PathChangedHandler PathChanged;
    private void Start()
    {
        if(inputField == null)
        {
            ErrorManager.Instance.ShowErrorMessage("InputField property not set",this);
        }
    }
    public void ValueChanged()
    {
        if(inputField.text != "")
        {
            PathChanged?.Invoke(FilePath,this);
        }
    }
    public override void Init(string sourceDirectory,string extension)
    {
        this.directoryPath = sourceDirectory;
        this.extension = extension;
    }
}
