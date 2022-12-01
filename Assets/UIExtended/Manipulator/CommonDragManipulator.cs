using System;
using UnityEngine;
using BasicTools;

namespace UIExtended
{
    public class CommonDragManipulator : DragInputManipulator<Vector3>
    {
        protected override void Awake()
        {
            base.Awake();
            InputBinding.ValueChanged += UpdateView;
        }

        protected override void WriteInput(Vector3 touchPosition)
        {
            Vector3 input = OriginPosition - touchPosition;
            this.InputBinding.ChangeValue(input,this);
        }

        protected virtual void UpdateView(Vector3 input,object sender)
        {
            this.UpdateView(input);
        }
    }
}