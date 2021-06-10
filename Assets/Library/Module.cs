using UnityEngine;
using System.Collections;
using System;

namespace Assets.Library
{
    [Serializable]
    public abstract class Module : MonoBehaviour
    {
        public abstract void UpdatePhysicsState(bool State);
        public void Awake()
        {
            TimeManager.Instance.AddObserver(this);
        }
    }
}

