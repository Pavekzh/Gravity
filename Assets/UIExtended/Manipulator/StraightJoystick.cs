using BasicTools;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UIExtended
{
    public class StraightJoystick : InputManipulator
    {
        [SerializeField]
        private RectTransform positivePoint;
        [SerializeField]
        private RectTransform negativePoint;
        [SerializeField]
        private RectTransform stick;
        private bool isEnabled;

        public Vector2 PositivePoint { get => positivePoint.position; }
        public Vector2 NegativePoint { get => negativePoint.position; }
        public Vector2 lineDirection { get => PositivePoint - NegativePoint; }

        public override event InputReadingHandler InputReadingStarted;
        public override event InputReadingHandler InputReadingStoped;

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
                Vector2 lineCenter = (PositivePoint + NegativePoint) / 2;
                Vector2 inputVector = pointOverLine - lineCenter;

                InputBinding.ChangeValue(inputVector.GetVector3(), this);
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
        }

        public float GetRangedFloatInput(Vector2 input)
        {
            Vector2 lineCenter = (PositivePoint + NegativePoint) / 2;
            Vector2 positiveAxis = PositivePoint - lineCenter;

            return input.magnitude / positiveAxis.magnitude * Vector2.Dot(input.normalized, positiveAxis.normalized);
        }


        public Vector2 GetRangedVectorInput(Vector2 input)
        {
            Vector2 lineCenter = (PositivePoint + NegativePoint) / 2;
            Vector2 positiveAxis = PositivePoint - lineCenter;

            return input / positiveAxis.magnitude;
        }


    }
}