using UnityEngine;
using System.Collections;

namespace Assets.Library
{
    public enum State
    {
        Default,
        Changed
    }
    public abstract class ElementStateChanger : MonoBehaviour
    {
        public abstract State State { get; set; }
        public abstract void ChangeState();
        public abstract void ChangeState(State state);

    }
}

