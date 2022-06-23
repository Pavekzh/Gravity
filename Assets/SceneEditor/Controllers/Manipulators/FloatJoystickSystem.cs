using UIExtended;
using System;
using UnityEngine;

namespace Assets.SceneEditor.Controllers
{
    public class FloatJoystickSystem : JoystickSystem<float>
    {
        [SerializeField]protected StraightJoystick straightJoystick;

        public override bool RestorePanel { get => true; }
        public static string DefaultKey => "FloatJoystickSystem";

        public event Action JoystickInputReadingStarted
        {
            add
            {
                straightJoystick.InputReadingStarted += value;
            }
            remove
            {
                straightJoystick.InputReadingStarted -= value;
            }
        }
        public event Action JoystickInputReadingStoped
        {
            add
            {
                straightJoystick.InputReadingStoped += value;
            }
            remove
            {
                straightJoystick.InputReadingStoped -= value;
            }
        }

        protected virtual void Start()
        {
            if (manipulatorName != "")
                EditorController.Instance.ManipulatorsController.Manipulators.Add(manipulatorName, this);
            else
                EditorController.Instance.ManipulatorsController.Manipulators.Add(DefaultKey, this);
        }

        protected override void DoDisable()
        {
            straightJoystick.InputBinding.ValueChanged -= joystickInput;
            straightJoystick.Disable();
        }

        protected override void DoEnable()
        {
            straightJoystick.InputBinding.ValueChanged += joystickInput;
            straightJoystick.Enable(null);
        }

        protected virtual void joystickInput(float value, object source)
        {
            this.InputBinding.ChangeValue(value,this);
        }
    }
}