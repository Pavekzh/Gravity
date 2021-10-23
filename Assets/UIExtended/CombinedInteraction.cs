using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BasicTools;

namespace UIExtended
{
    public class CombinedInteraction : StateChanger
    {
        [SerializeField] List<StateChanger> interactors;

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
}