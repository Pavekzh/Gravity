using BasicTools;
using System.Collections;
using UnityEngine;

namespace UIExtended
{
    public class RoundStraightJoystickSystem : InputManipulator
    {
        [SerializeField]
        private bool isEnabled;
        [SerializeField]
        private Joystick roundJoystick;
        [SerializeField]
        private StraightJoystick straightJoystick;
        [SerializeField]
        private float ResultVectorAcceleration;
        [SerializeField]
        private float ResultVectorMagnitudeRange = 1;

        private float straightJoystickInput;
        private Vector2 roundJoystickInput;


        public override event InputReadingHandler InputReadingStarted;
        public override event InputReadingHandler InputReadingStoped;

        protected virtual void Start()
        {
            roundJoystick.InputBinding.ValueChanged += RoundJoystickInputChanged;            
            roundJoystick.InputReadingStoped += RoundJoystickDown;
            roundJoystick.InputReadingStoped += RoundJoystickUp;

            straightJoystick.InputBinding.ValueChanged += StraightJoystickInputChanged;
            straightJoystick.InputReadingStoped += StraightJoystickUp;
            straightJoystick.InputReadingStarted += StraightJoystickDown;
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

        private void StraightJoystickInputChanged(Vector3 value, object source)
        {
            if(isEnabled)
                straightJoystickInput = straightJoystick.GetRangedFloatInput(value.GetVectorXZ());
        }

        private void RoundJoystickInputChanged(Vector3 value, object source)
        {
            if (isEnabled)
            {
                roundJoystickInput = value.GetVectorXZ().normalized * (value.magnitude / roundJoystick.SpaceRadius);
                Vector2 resultVector = roundJoystickInput * ResultVectorMagnitudeRange;
                InputBinding.ChangeValue(resultVector.GetVector3(), this);
            }
        }

        public override void DisableTool()
        {
            InputReadingStoped?.Invoke();
            isEnabled = false;
        }

        public override void EnableTool(Binding<Vector2> originBinding)
        {
            if (isEnabled)
                DisableTool();
            isEnabled = true;
        }

        private bool StraightJoystickInputReading;
        private bool RoundJoystickInputReading;
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