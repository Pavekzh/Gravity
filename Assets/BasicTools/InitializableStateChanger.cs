using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicTools
{
    public abstract class InitializableStateChanger : StateChanger
    {
        protected bool IsInitialized;

        protected override void Start()
        {
            if(!IsInitialized)
                Initialize(state);
        }

        public virtual void Initialize(State state)
        {
            this.state = state;
            this.IsInitialized = true;
        }
    }
}
