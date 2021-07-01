using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Library;
using UnityEngine.UI;

public class ShowElement : ElementStateChanger
{
    [SerializeField] GameObject displayObject;
    [SerializeField] State state = State.Default;

    public override State State { get => state; set => state = value; }

    private void Start()
    {
        if (state == State.Changed)
        {
            displayObject.SetActive(true);
        }
        else if(state == State.Default)
        {
            displayObject.SetActive(false);
        }
    }
    public override void ChangeState()
    {
        if(State == State.Changed)
        {
            this.Hide();
        }
        else if (State == State.Default)
        {
            this.Show();
        }

    }    
    public override void ChangeState(State state)
    {
        this.State = state;
        if(state == State.Default)
        {
            this.Show();

        }
        else if(state == State.Changed)
        {
            this.Hide();
        }
    }
    public void Show()
    {
        displayObject.SetActive(true);
        State = State.Changed;
    }
    public void Hide()
    {
        displayObject.SetActive(false);
        State = State.Default;
    }
}
