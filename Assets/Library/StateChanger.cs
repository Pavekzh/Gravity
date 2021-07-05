using UnityEngine;
using System.Collections;

namespace Assets.Library
{
    public enum State
    {
        Default,
        Changed
    }
    public abstract class StateChanger : MonoBehaviour
    {
        public abstract State State { get; set; }
        public void ChangeState()
        {
            if (State == State.Default)
                State = State.Changed;
            else if (State == State.Changed)
                State = State.Default;
        }

    }
}

