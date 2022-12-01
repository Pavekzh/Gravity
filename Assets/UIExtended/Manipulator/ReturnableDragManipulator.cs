using System;
using UnityEngine;
using BasicTools;

namespace UIExtended
{
    public class ReturnableDragManipulator:CommonDragManipulator
    {
        [SerializeField] float manipulatorRadius;
        public float ManipulatorRadius { get => manipulatorRadius; set => manipulatorRadius = value; }

        private Vector3 lastInput;

        protected override void Awake()
        {
            base.Awake();
            this.InputReadingStoped += DragStoped;
        }

        private void DragStoped()
        {
            UpdateView(lastInput.normalized * manipulatorRadius);
        }

        protected override void WriteInput(Vector3 touchPosition)
        {
            Vector3 input = OriginPosition - touchPosition;
            this.InputBinding.ChangeValue(input, this);
            UpdateView(input);
            lastInput = input;
        }

        protected override void UpdateView(Vector3 input, object sender)
        {
            if (sender != (System.Object)this)
                this.UpdateView(Vector3.forward * manipulatorRadius);          
        }
    }
}
