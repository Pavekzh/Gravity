﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Library;

public class CombinedInteraction : StateChanger
{
    [SerializeField] List<StateChanger> interactors;
    [SerializeField] State state;

    public override State State
    {
        get { return state; }
        set
        {
            state = value;
            foreach (StateChanger stateChanger in interactors)
            {
                stateChanger.State = value;
            }
        }
    }
}