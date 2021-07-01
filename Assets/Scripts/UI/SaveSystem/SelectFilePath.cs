using UnityEngine;
using System.Collections;
using Assets.Library;

public class SelectFilePath : FilePathProvider
{
    [SerializeField] DirectoryPresenter directoryPresenter;

    public override event PathChangedHandler PathChanged;
    public override string Directory { get; set; }
    public override string FileExtension { get; set; }

    private void Awake()
    {
        directoryPresenter.Directory = Directory;
        directoryPresenter.FileExtension = FileExtension;
    }
    public void SelectPath(string path,object sender)
    {
        PathChanged(path, sender);
    }
}
