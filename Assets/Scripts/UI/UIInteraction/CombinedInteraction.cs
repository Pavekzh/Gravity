using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Library;

public class CombinedInteraction : ElementStateChanger
{
    [SerializeField] List<ElementStateChanger> interactors;
    [SerializeField] State state;

    public override State State { get => state; set => state = value; }

    public override void ChangeState()
    {
        if(State == State.Default)
        {
            foreach(ElementStateChanger stateChanger in interactors)
            {
                stateChanger.ChangeState(State.Changed);

            }
            state = State.Changed;
        }
        else if(State == State.Changed)
        {
            foreach (ElementStateChanger stateChanger in interactors)
            {
                stateChanger.ChangeState(State.Default);

            }
            state = State.Default;
        }
    }
    public override void ChangeState(State state)
    {
        foreach (ElementStateChanger stateChanger in interactors)
        {
            stateChanger.ChangeState(state);

        }
        this.state = state;
    }
}
