using UnityEngine;
using System.Collections;
using Assets.Library;

public class SelectFilePath : FilePathProvider
{
    [SerializeField] DirectoryPresenter directoryPresenter;

    public override event PathChangedHandler PathChanged;

    public override void Init(string sourceDirectory, string extension)
    {
        directoryPresenter.OpenPanel(sourceDirectory, extension);
    }
    public void SelectPath(string path,object sender)
    {
        PathChanged(path, sender);
    }
}
