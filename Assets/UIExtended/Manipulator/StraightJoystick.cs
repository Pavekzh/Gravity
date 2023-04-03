using BasicTools;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UIExtended
{
    public class StraightJoystick : InputManipulator<float>
    {
        [SerializeField]
        private RectTransform positivePoint;
        [SerializeField]
        private RectTransform negativePoint;
        [SerializeField]
        private RectTransform stick;
        [SerializeField]
        private bool isEnabled;
        [SerializeField]
        private bool returnStickToOrigin = true;

        public bool ReturnStickToOrigin { get => returnStickToOrigin; set => returnStickToOrigin = value; }
        public Vector2 LineCenter { get => (PositivePoint + NegativePoint) / 2; }
        public Vector2 PositivePoint { get => positivePoint.position; }
        public Vector2 NegativePoint { get => negativePoint.position; }
        public Vector2 lineDirection { get => PositivePoint - NegativePoint; }

        public override event Action InputReadingStarted;
        public override event Action InputReadingStoped;

        public override void Disable()
        {
            InputReadingStoped?.Invoke();
            isEnabled = false;
        }

        public override void Enable(Binding<Vector2> originBinding)
        {
            if (isEnabled)
                Disable();

            isEnabled = true;
        }

        public virtual void StickTouch(BaseEventData eventData)
        {
            if (isEnabled)
            {
                Vector2 pointerPosition = (eventData as PointerEventData).position;
                Vector2 pointOverLine = NegativePoint + (lineDirection.normalized * (Vector2.Dot(lineDirection, (pointerPosition - NegativePoint)) / lineDirection.magnitude));

                if ((pointOverLine - NegativePoint).magnitude - (PositivePoint - NegativePoint).magnitude > 0)
                {
                    pointOverLine = PositivePoint;
                }
                else if ((pointOverLine - PositivePoint).magnitude - (NegativePoint - PositivePoint).magnitude > 0)
                {
                    pointOverLine = NegativePoint;
                }
                stick.position = pointOverLine;
                Vector2 inputVector = pointOverLine - LineCenter;

                InputBinding.ChangeValue(GetRangedFloatInput(inputVector), this);
            }

        }

        public virtual void StickTouchDown()
        {
            if(isEnabled)
                InputReadingStarted?.Invoke();
        }

        public virtual void StickTouchUp()
        {           
            if (isEnabled)
                InputReadingStoped?.Invoke();

            if (returnStickToOrigin)
            {
                stick.position = LineCenter;            
                InputBinding.ChangeValue(0, this);
            }

        }

        public float GetRangedFloatInput(Vector2 input)
        {
            Vector2 positiveAxis = PositivePoint - LineCenter;

            return input.magnitude / positiveAxis.magnitude * Vector2.Dot(input.normalized, positiveAxis.normalized);
        }

        public Vector2 GetRangedVectorInput(Vector2 input)
        {
            Vector2 positiveAxis = PositivePoint - LineCenter;

            return input / positiveAxis.magnitude;
        }


    }
}