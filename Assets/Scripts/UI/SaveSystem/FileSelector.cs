using UnityEngine;
using System.Collections;
using TMPro;
using Assets.Library;

public class FileSelector : MonoBehaviour
{
    [SerializeField] private ElementStateChanger StateChanger;
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

    private void Start()
    {
        selectSystem.PathChanged += this.SelectedFileChanged;
    }

    public void ChangeState()
    {

        if (state == State.Changed)
        {
            state = State.Default;
            StateChanger.ChangeState(State.Default);
        }
        else if(state == State.Default)
        {
            state = State.Changed;
            StateChanger.ChangeState(State.Changed);
            Select();
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
            this.StateChanger.ChangeState(State.Default);
        }
    }
}
