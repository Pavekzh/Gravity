using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Library;

public class ChangeImage : ElementStateChanger
{
    [SerializeField] protected Image imageField;
    [SerializeField] protected Sprite defaultState;
    [SerializeField] protected Sprite changedState;
    [SerializeField] protected State state = State.Default;

    public override State State { get => state; set => state = value; }
    protected virtual void Start()
    {
        if(imageField != null)
        {
            if (state == State.Default)
            {
                imageField.sprite = defaultState;
            }
            else if(state == State.Changed)
            {
                imageField.sprite = changedState;
            }
        }

    }
    public override void ChangeState()
    {
        if(imageField != null)
        {
            if (state == State.Default)
            {
                imageField.sprite = changedState;
                state = State.Changed;
            }
            else if(state == State.Changed)
            {
                imageField.sprite = defaultState;
                state = State.Default;
            }
        }

    }
    public override void ChangeState(State state)
    { 
        this.state = state;
        if(imageField != null)
        {
            if (state == State.Default)
            {
                imageField.sprite = defaultState;
            }
            else if(state == State.Changed)
            {
                imageField.sprite = changedState;
            }
        }
    }
}
