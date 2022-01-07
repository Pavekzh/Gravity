﻿using BasicTools;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UIExtended
{
    [RequireComponent(typeof(RectTransform))]
    public class Joystick : InputManipulator
    {
        private RectTransform rectTransform;
        [SerializeField]
        private RectTransform stick;
        [SerializeField]
        private float spaceRadius;        
        [SerializeField]
        private bool isEnabled;
        [SerializeField]
        private bool returnStickToOrigin;
        
        public bool ReturnStickToOrigin { get => returnStickToOrigin; set => returnStickToOrigin = value; }
        public bool IsTouched { get; protected set; }
        public float SpaceRadius { get => spaceRadius * rectTransform.lossyScale.x; }
        public Vector2 Origin { get => rectTransform.position; }

        public override event InputReadingHandler InputReadingStarted;
        public override event InputReadingHandler InputReadingStoped;

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            InputBinding.ValueChanged += InputBindingChanged;
        }

        private void InputBindingChanged(Vector3 value, object source)
        {
            if(source != (object)this && IsTouched == false)
            {
                Vector2 pointerPosition;
                if(value.GetVectorXZ().magnitude < spaceRadius * rectTransform.lossyScale.x)
                {
                    pointerPosition = Origin - value.GetVectorXZ();
                }
                else
                {
                    pointerPosition = Origin - value.GetVectorXZ().normalized * spaceRadius * rectTransform.lossyScale.x; ;
                }
                stick.position = pointerPosition;
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

        public virtual void JoystickTouch(BaseEventData eventData)
        {
            if (isEnabled && IsTouched)
            {
                Vector2 pointerPosition = (eventData as PointerEventData).position;
                Vector2 direction = Origin - pointerPosition;
                if (direction.magnitude > spaceRadius * rectTransform.lossyScale.x)
                {
                    direction = direction.normalized * spaceRadius * rectTransform.lossyScale.x;
                    pointerPosition = Origin - (direction);
                }
                stick.position = pointerPosition;
                InputBinding.ChangeValue(direction.GetVector3(), this);
            }
        }
        
        public virtual void JoystickTouchDown()
        {
            if(isEnabled)
                InputReadingStarted?.Invoke();
            IsTouched = true;
        }

        public virtual void JoystickTouchUp()
        {
            if(isEnabled)
                InputReadingStoped?.Invoke();
            if(returnStickToOrigin)
                stick.position = rectTransform.position;

            IsTouched = false;
        }
    }
}