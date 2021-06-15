using UnityEngine;
using System.Collections;
using System;

namespace Assets.Library
{
    public abstract class Module : MonoBehaviour,IReactPhysicsState
    {
        [SerializeField] Planet planet;
        public Planet Planet
        {
            get => planet;
            set
            {
                planet = value;
                planet.Modules.Add(this);
            }
        }

        public abstract void UpdatePhysicsState(bool State);
        public virtual void Awake()
        {
            TimeManager.Instance.AddObserver(this);
            if(Planet != null)
            {
                Planet.Modules.Add(this);
            }
        }
        public abstract void SetModule(ModuleData module);
        public abstract ModuleData GetModuleData();
    }
}

