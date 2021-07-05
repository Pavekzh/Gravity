using UnityEngine;
using System.Collections;
using TMPro;
using Assets.Library;

public class FileSelector : StateChanger
{
    [SerializeField] private StateChanger StateChanger;
    [SerializeField] private string filePath;
    [SerializeField] private SelectFilePath selectSystem;

    private State state = State.Default;
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
    public override State State
    {
        get { return state; }
        set
        {
            state = value;
            StateChanger.State = value;
            if(value == State.Changed)
            {
                Select();
            }
        }
    }

    private void Start()
    {
        selectSystem.PathChanged += this.SelectedFileChanged;
    }
    private void OnDestroy()
    {
        selectSystem.PathChanged -= this.SelectedFileChanged;
    }

    private void Select()
    {
        SelectSystem.SelectPath(FilePath, this);
    }
    public void SelectedFileChanged(string selectedPath, object sender)
    {
        if (sender != (object)this)
        {
            this.StateChanger.State = State.Default;
            this.state = State.Default;
        }
    }
}
