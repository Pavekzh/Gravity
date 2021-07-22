using UnityEngine;
using System.Collections;
using Assets.Library;

public class SelectedObjectOpener : StateChanger
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

    public void Open()
    {
        objectPresenter.Open(SelectManager.Instance.SelectedObject);
    }
    public void Close()
    {
        objectPresenter.Close();
    }
}
