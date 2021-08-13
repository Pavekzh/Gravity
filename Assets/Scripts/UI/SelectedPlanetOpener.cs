using UnityEngine;
using System.Collections;
using Assets.Library;
using BasicTools;
using UIExtended;

public class SelectedPlanetOpener : StateChanger
{
    [SerializeField] CustomizableObjectPresenter objectPresenter;
    [SerializeField] State state;

    public override State State
    {
        get => state;
        set
        {
            state = value;
            if(value == State.Default)
            {
                Close();               
            }
            else if(value == State.Changed)
            {
                Open();
            }
        }
    }

    private void Start()
    {
        SelectManager.Instance.SelectedPlanetChanged += SelectedObejctChanged;
    }

    private void SelectedObejctChanged(Planet planet, object sender)
    {
        Open();
    }

    public void Open()
    {
        objectPresenter.Open(SelectManager.Instance.SelectedObject);
    }

    public void Close()
    {
        objectPresenter.Close();
    }
}
