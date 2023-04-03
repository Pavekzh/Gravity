using BasicTools;
using UnityEngine;
using UIExtended;
using System;

namespace Assets.SceneEditor.Controllers
{
    public class VectorJoystickSystem : JoystickSystem<Vector2>
    {
        [SerializeField] Joystick roundJoystick;

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

        public static string DefaultKey => "VectorJoystickSystem";
        public override string ManipulatorKey => DefaultKey;

        protected override void DoDisable()
        {
            roundJoystick.InputBinding.ValueChanged -= JoystickTouch;
            roundJoystick.Disable();         
        }

        protected override void DoEnable()
        {
            roundJoystick.InputBinding.ValueChanged += JoystickTouch;
            roundJoystick.Enable(null);
        }

        private void JoystickTouch(Vector3 value, object source)
        {
            InputBinding.ChangeValue(value.GetVectorXZ().normalized * (value.magnitude / roundJoystick.SpaceRadius),this);
        }
    }
}