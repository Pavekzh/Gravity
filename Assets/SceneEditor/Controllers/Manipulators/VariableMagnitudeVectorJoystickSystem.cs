using BasicTools;
using System;
using UIExtended;
using UnityEngine;

namespace Assets.SceneEditor.Controllers
{
    class VariableMagnitudeVectorJoystickSystem : JoystickSystem<Vector3>
    {
        [SerializeField]
        private Joystick roundJoystick;
        [SerializeField]
        private StraightJoystick straightJoystick;
        [SerializeField]
        private float ResultVectorAcceleration = 10;
        [SerializeField]
        private float ResultVectorMagnitudeRange = 10;

        private float straightJoystickInput;
        private Vector2 roundJoystickInput;
        private bool StraightJoystickInputReading;
        private bool RoundJoystickInputReading;

        public event Action InputReadingStarted;
        public event Action InputReadingStoped;

        public override bool RestorePanel => true;

        protected override void DoDisable()
        {
            roundJoystick.InputBinding.ValueChanged -= RoundJoystickInputChanged;
            roundJoystick.InputReadingStoped -= RoundJoystickDown;
            roundJoystick.InputReadingStoped -= RoundJoystickUp;


            straightJoystick.InputBinding.ValueChanged -= StraightJoystickInputChanged;
            straightJoystick.InputReadingStoped -= StraightJoystickUp;
            straightJoystick.InputReadingStarted -= StraightJoystickDown;

            InputReadingStoped?.Invoke();
            straightJoystick.Disable();
            roundJoystick.Disable();
        }

        protected override void DoEnable()
        {
            roundJoystick.InputBinding.ValueChanged += RoundJoystickInputChanged;
            roundJoystick.InputReadingStoped += RoundJoystickDown;
            roundJoystick.InputReadingStoped += RoundJoystickUp;
            roundJoystick.ReturnStickToOrigin = false;

            straightJoystick.InputBinding.ValueChanged += StraightJoystickInputChanged;
            straightJoystick.InputReadingStoped += StraightJoystickUp;
            straightJoystick.InputReadingStarted += StraightJoystickDown;

            straightJoystick.Enable(null);
            roundJoystick.Enable(null);
        }


        public static string DefaultKey => "VariableMagintudeVectorJoystickSystem";

        protected void Start()
        {
            if (ManipulatorKey != "")
                EditorController.Instance.ManipulatorsController.Manipulators.Add(ManipulatorKey, this);
            else
                EditorController.Instance.ManipulatorsController.Manipulators.Add(DefaultKey, this);

            InputBinding.ValueChanged += BindingExternalChanged;
        }

        protected virtual void Update()
        {
            if (isEnabled)
            {
                if (straightJoystickInput != 0)
                {
                    ResultVectorMagnitudeRange += straightJoystickInput * Time.deltaTime * ResultVectorAcceleration;
                    Vector2 resultVector = roundJoystickInput * ResultVectorMagnitudeRange;
                    InputBinding.ChangeValue(resultVector.GetVector3(), this);
                }
            }
        }


        private void BindingExternalChanged(Vector3 value, object source)
        {
            if (source != (object)this)
            {
                roundJoystick.InputBinding.ChangeValue(value / ResultVectorMagnitudeRange * roundJoystick.SpaceRadius, this);
                roundJoystickInput = value.GetVectorXZ() / ResultVectorMagnitudeRange;
            }
        }

        private void StraightJoystickInputChanged(float value, object source)
        {
            if (isEnabled)
                straightJoystickInput = value;
        }

        private void RoundJoystickInputChanged(Vector3 value, object source)
        {
            if (isEnabled && source != (object)this)
            {
                roundJoystickInput = value.GetVectorXZ().normalized * (value.magnitude / roundJoystick.SpaceRadius);
                Vector2 resultVector = roundJoystickInput * ResultVectorMagnitudeRange;
                InputBinding.ChangeValue(resultVector.GetVector3(), this);
            }
        }


        //input reading start & stop events invoking
        private void RoundJoystickUp()
        {
            RoundJoystickInputReading = false;
            if (StraightJoystickInputReading == false)
                InputReadingStoped?.Invoke();
        }

        private void RoundJoystickDown()
        {
            RoundJoystickInputReading = true;
            if (StraightJoystickInputReading == false)
                InputReadingStarted?.Invoke();
        }

        private void StraightJoystickUp()
        {
            straightJoystickInput = 0;
            StraightJoystickInputReading = false;
            if (RoundJoystickInputReading == false)
                InputReadingStoped?.Invoke();
        }

        private void StraightJoystickDown()
        {
            StraightJoystickInputReading = true;
            if (RoundJoystickInputReading == false)
                InputReadingStarted?.Invoke();
        }


    }
}
