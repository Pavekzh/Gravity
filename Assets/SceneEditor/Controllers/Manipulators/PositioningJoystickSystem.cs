using System;
using BasicTools;
using UIExtended;
using UnityEngine;

namespace Assets.SceneEditor.Controllers
{
    public class PositioningJoystickSystem:JoystickSystem<Vector3>
    {
        [SerializeField] Joystick roundJoystick;
        [SerializeField] TouchManipulator touchManipulator;

        public override bool RestorePanel => true;

        private Vector3 input;

        public event Action JoystickInputReadingStarted
        {
            add
            {
                roundJoystick.InputReadingStarted += value;
            }
            remove
            {
                roundJoystick.InputReadingStarted -= value;
            }
        }
        public event Action JoystickInputReadingEnded
        {
            add
            {
                roundJoystick.InputReadingStoped += value;
            }
            remove
            {
                roundJoystick.InputReadingStoped -= value;
            }
        }

        public event Action TouchInputReadingStarted;
        public event Action TouchInputReadingEnded
        {
            add
            {
                touchManipulator.InputReadingEnded += value;
            }
            remove
            {
                touchManipulator.InputReadingEnded -= value;
            }
        }

        public override string ManipulatorKey => DefaultKey;
        public static string DefaultKey => "PositioningJoystickSystem";

        protected void FixedUpdate()
        {
            if(isEnabled && input != Vector3.zero)
            {
                InputBinding.ChangeValue(input, this);
            }
        }

        protected override void DoDisable()
        {
            roundJoystick.InputBinding.ValueChanged -= JoystickTouch;
            roundJoystick.Disable();

            if (touchManipulator.IsEnabled)
                DisableTouchPositioning();
        }

        protected override void DoEnable()
        {
            roundJoystick.InputBinding.ValueChanged += JoystickTouch;
            roundJoystick.Enable(null);
            roundJoystick.ReturnStickToOrigin = true;
        }


        public void EnableTouchPositioning()
        {
            if(touchManipulator.IsEnabled == false)
            {
                roundJoystick.Disable();

                touchManipulator.EnableManipulator(this.inputSystem);
                TouchInputReadingStarted?.Invoke();

                touchManipulator.InputBinding.ValueChanged += TouchChanged;
                TouchInputReadingEnded += DisableTouchPositioning;
            }

        }

        public void DisableTouchPositioning()
        {
            if(touchManipulator.IsEnabled == true)
            {
                roundJoystick.Enable(null);

                touchManipulator.DisableManipulator();

                touchManipulator.InputBinding.ValueChanged -= TouchChanged;
                TouchInputReadingEnded -= DisableTouchPositioning;
            }
        }

        public void DisableUI()
        {
            visibleStateManager.State = State.Default;
        }

        private void TouchChanged(Vector3 value, object source)
        {
            this.InputBinding.ChangeValue(value, source);
        }

        private void JoystickTouch(Vector3 value, object source)
        {
            input = (value.GetVectorXZ().normalized * (value.magnitude / roundJoystick.SpaceRadius)).GetVector3();
        }
    }
}
