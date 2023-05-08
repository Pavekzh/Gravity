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

        public string ManipulatorName { get; }
        public abstract string ManipulatorKey { get; }
        public Binding<T> InputBinding { get; } = new Binding<T>();

        protected override void Construct(EditorController editor)
        {
            base.Construct(editor);

            if (manipulatorName != "")
                editor.ManipulatorsController.Manipulators.Add(manipulatorName, this);
            else
                editor.ManipulatorsController.Manipulators.Add(ManipulatorKey, this);
        }

        protected abstract void DoDisable();
        protected abstract void DoEnable();

        private void OnDestroy()
        {
            DisableManipulator();
        }

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
            visibleStateManager.State = BasicTools.State.Changed;
        }        
        
        protected sealed override void DoClose()
        {
            visibleStateManager.State = BasicTools.State.Default;
        }

    }
}