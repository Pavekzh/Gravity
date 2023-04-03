using UnityEngine;
using System.Collections;

namespace BasicTools
{
    [System.Serializable]
    public enum State
    {
        Default,
        Changed
    }
    public abstract class StateChanger : MonoBehaviour
    {
        [SerializeField] protected State state;

        public abstract State State { get; set; }

        public void ChangeState(int newState)
        {
            State = (State)newState;
        }

        public void ChangeState()
        {
            if (State == State.Default)
                State = State.Changed;
            else if (State == State.Changed)
                State = State.Default;
        }

        protected virtual void Start()
        {
            State = state;
        }
    }
}

