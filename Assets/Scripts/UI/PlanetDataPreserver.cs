using UnityEngine;
using System.Collections;
using Assets.Library;
using UIExtended;

public class PlanetDataPreserver : FilePathProvider
{
    [SerializeField] SaveSystemXML saveSystem;
    [SerializeField] DirectoryPresenter directoryPresenter;
    public override string Directory { get; set; }
    public override string FileExtension { get; set; }
    public override event PathChangedHandler PathChanged;

    public void Awake()
    {
        directoryPresenter.Directory = Directory;
        directoryPresenter.FileExtension = FileExtension;
    }

    public void Start()
    {
        SelectManager.Instance.SelectedPlanetChanged += SelectedPlanetChanged;
    }

    public void SelectedPlanetChanged(Planet value,object sender)
    {
        if(sender != (object)this)
        {
            Debug.Log(value.Name);
            PathChanged?.Invoke(Directory + "/" + value.Name + FileExtension,this);
        }
    }

    public void SavePlanetData()
    {
        if(SelectManager.Instance.SelectedObject != null)
        {
            PlanetData planetData = SelectManager.Instance.SelectedObject.GetPlanetData();
            saveSystem.SaveToFile(planetData);
        }
        //need send some message to player
    }
}
