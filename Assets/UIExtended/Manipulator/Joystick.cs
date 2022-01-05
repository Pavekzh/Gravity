using BasicTools;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UIExtended
{
    [RequireComponent(typeof(RectTransform))]
    public class Joystick : InputManipulator
    {
        private bool isEnabled;
        private RectTransform rectTransform;
        [SerializeField]
        private RectTransform stick;
        [SerializeField]
        private float SpaceRadius;

        public Vector2 Origin { get => rectTransform.position; }

        public override event InputReadingHandler InputReadingStarted;
        public override event InputReadingHandler InputReadingStoped;

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
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

        public virtual void JoystickTouch(BaseEventData eventData)
        {
            if (isEnabled)
            {
                Vector2 pointerPosition = (eventData as PointerEventData).position;
                Vector2 direction = Origin - pointerPosition;
                if (direction.magnitude > SpaceRadius)
                {
                    direction = direction.normalized * SpaceRadius;
                    pointerPosition = Origin - direction;
                }
                stick.position = pointerPosition;
                InputBinding.ChangeValue(direction.GetVector3(), this);
            }

        }
        
        public virtual void JoystickTouchDown()
        {
            if(isEnabled)
                InputReadingStarted?.Invoke();
        }

        public virtual void JoystickTouchUp()
        {
            if(isEnabled)
                InputReadingStoped?.Invoke();
        }
    }
}