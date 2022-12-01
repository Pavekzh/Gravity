using System;
using UnityEngine;
using BasicTools;
using UIExtended;

namespace Assets.SceneEditor.Controllers
{
    public class RelativeScaleJoystickSystem:ScalarJoystickSystem
    {
        [SerializeField] protected ReturnableDragManipulator dragManipulator;
        [SerializeField] protected float ScalingSpeed = 1;

        public static string DefaultKey => "RelativeScaleJoystickSystem";
        public override string ManipulatorKey => DefaultKey;
        public override DragInputManipulator<Vector3> DragManipulator { get => dragManipulator; }

        protected float savedValue;

        protected override void dragInput(Vector3 value, object source)
        {
            if (source != (System.Object)this)
            {
                float ScalingFactor = value.magnitude / dragManipulator.ManipulatorRadius;
                ScalingFactor = MathF.Pow(ScalingFactor, ScalingSpeed);
                currentValue = ScalingFactor*savedValue;
                this.InputBinding.ChangeValue(currentValue, this);
            }
        }

        protected override void enableDragInput()
        {
            base.enableDragInput();
            if (OriginBinding != null && isEnabled)
            {
                DragInputStarted += SaveCurrentValue;
            }
        }

        protected override void disableDragInput()
        {
            base.disableDragInput();
            if (OriginBinding != null && isEnabled)
            {
                DragInputStarted -= SaveCurrentValue;
            }
        }

        private void SaveCurrentValue()
        {
            savedValue = currentValue;
        }
    }
}
