using UnityEngine;
using System.Collections;
using TMPro;
using Assets.Library;

public class FileSelector : ChangeImage
{
    [SerializeField] private string filePath;
    [SerializeField] private SelectFilePath selectSystem;
    public SelectFilePath SelectSystem
    {
        get => selectSystem;
        set
        {
            if (selectSystem != null)
                selectSystem.PathChanged -= SelectedFileChanged;

            selectSystem = value;
            if (selectSystem != null)
                selectSystem.PathChanged += SelectedFileChanged;
        }
    }

    public string FilePath { get => filePath; set => filePath = value; }

    private void Start()
    {
        selectSystem.PathChanged += this.SelectedFileChanged;
    }

    public override void ChangeState()
    {

        if (isDefaultState)
        {
            if (imageField != null)
                imageField.sprite = changedState;

            Select();
            isDefaultState = false;
        }
        else
        {
            if (imageField != null)
                imageField.sprite = defaultState;

            isDefaultState = true;
        }

    }
    public override void ChangeState(bool isDefaultState)
    {
        this.isDefaultState = isDefaultState;

        if (isDefaultState)
        {
            if (imageField != null)
                imageField.sprite = defaultState;
        }
        else
        {
            Select();
            if (imageField != null)
                imageField.sprite = changedState;
        }

    }
    private void Select()
    {
        SelectSystem.SelectPath(FilePath, this);
    }
    public void SelectedFileChanged(string selectedPath, object sender)
    {
        if (sender != (object)this)
        {
            this.ChangeState(true);
        }
    }
}
