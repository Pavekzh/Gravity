using System;
using UnityEngine;
using BasicTools;
using UIExtended;

namespace Assets.SceneEditor.Controllers
{
    public abstract class ScalarJoystickSystem:JoystickSystem<float>
    {
        [SerializeField] protected StraightJoystick straightJoystick;
        [SerializeField] protected float dragTouchDistance = 0;
        [SerializeField] protected float scaleSpeed = 0.01f;

        public float ScaleSpeed { get => scaleSpeed; set => scaleSpeed = value; }
        public override bool RestorePanel { get => true; }
        public float DragTouchDistance { get => dragTouchDistance; set => dragTouchDistance = value; }
        public abstract DragInputManipulator<Vector3> DragManipulator { get; }

        public event Action DragInputStarted
        {
            add
            {
                DragManipulator.InputReadingStarted += value;
            }
            remove
            {
                DragManipulator.InputReadingStarted -= value;
            }
        }
        public event Action DragInputEnded
        {
            add
            {
                DragManipulator.InputReadingStoped += value;
            }
            remove
            {
                DragManipulator.InputReadingStoped -= value;
            }
        }
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

        protected float input;
        protected float currentValue;

        private Binding<Vector2> originBinding;
        public Binding<Vector2> OriginBinding
        {
            get => originBinding;
            set
            {
                if (originBinding == null && value != null)
                {
                    originBinding = value;
                    enableDragInput();
                }
                if (originBinding != null)
                {

                    if (value == null)
                        disableDragInput();
                    else
                        DragManipulator.OriginBinding = value;
                    originBinding = value;
                }

            }
        }

        protected override void Start()
        {
            base.Start();
            EditorController.Instance.Camera.ZoomChanged += zoomChanged;
            this.InputBinding.ValueChanged += externalValueChanged;
        }

        protected void FixedUpdate()
        {
            if (isEnabled && !Mathf.Approximately(input, 0))
            {
                currentValue = currentValue + currentValue * input * scaleSpeed;
                externalValueChanged(currentValue, null);
                InputBinding.ChangeValue(currentValue, this);
            }
        }

        private void externalValueChanged(float value, object source)
        {
            if (source != (System.Object)this && OriginBinding != null)
            {
                currentValue = value;
                DragManipulator.InputBinding.ChangeValue(new Vector3(0, 0, value + dragTouchDistance), this);
            }
        }

        private void zoomChanged(float value, object sender)
        {
            if (isEnabled && InputBinding != null)
            {
                DragManipulator.ScaleFactor = EditorController.Instance.Camera.ScaleFactor;
            }
        }

        protected override void DoEnable()
        {
            straightJoystick.InputBinding.ValueChanged += joystickInput;
            straightJoystick.Enable(null);
            enableDragInput();
        }

        protected override void DoDisable()
        {
            straightJoystick.InputBinding.ValueChanged -= joystickInput;
            straightJoystick.Disable();
            disableDragInput();
        }

        protected virtual void joystickInput(float value, object source)
        {
            this.input = value;
        }

        protected virtual void dragInput(Vector3 value, object source)
        {
            if (source != (System.Object)this)
            {
                currentValue = value.magnitude - dragTouchDistance;
                this.InputBinding.ChangeValue(value.magnitude - dragTouchDistance, this);
            }
        }

        protected virtual void enableDragInput()
        {
            if (OriginBinding != null && isEnabled)
            {
                DragManipulator.ScaleFactor = EditorController.Instance.Camera.ScaleFactor;
                DragManipulator.Enable(OriginBinding);
                DragInputStarted += dragInputStarted;
                DragInputEnded += dragInputEnded;
                DragManipulator.InputBinding.ValueChanged += dragInput;
            }

        }

        protected virtual void disableDragInput()
        {
            if (OriginBinding != null)
            {
                DragManipulator.Disable();
                DragInputStarted -= dragInputStarted;
                DragInputEnded -= dragInputEnded;
                DragManipulator.InputBinding.ValueChanged -= dragInput;
            }
        }

        protected void dragInputEnded()
        {
            straightJoystick.Enable(null);
        }

        protected void dragInputStarted()
        {
            straightJoystick.Disable();
        }
    }
}
