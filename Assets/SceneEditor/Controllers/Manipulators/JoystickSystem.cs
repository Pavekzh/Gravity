using System.Collections;
using UnityEngine;
using UIExtended;
using BasicTools;

namespace Assets.SceneEditor.Controllers
{
    public abstract class JoystickSystem<T> : PanelController, IManipulator
    {
        [SerializeField] protected CombinedInteraction visibleStateManager;
        [SerializeField] protected string manipulatorName;

        protected InputSystem inputSystem;
        protected bool isEnabled;

        public string JoystickSystemName { get => manipulatorName; }
        public virtual string ManipulatorKey { get => manipulatorName; }
        public Binding<T> InputBinding { get; } = new Binding<T>();

        protected abstract void DoDisable();
        protected abstract void DoEnable();

        public void DisableManipulator()
        {

            if (isEnabled)
            {         
                isEnabled = false;
                DoDisable();
                Close();
            }
        }

        public void EnableManipulator(InputSystem inputSystem)
        {            
            if (isEnabled)
                DisableManipulator();

            this.inputSystem = inputSystem;
            isEnabled = true;
            Open();
            DoEnable();
        }

        protected sealed override void DoOpen()
        {
            if (isEnabled)
                visibleStateManager.State = BasicTools.State.Changed;
            else
                EnableManipulator(this.inputSystem);
        }        
        
        protected sealed override void DoClose()
        {
            if (isEnabled)
            {
                DisableManipulator();
            }
            else
                visibleStateManager.State = BasicTools.State.Default;
        }

    }
}