using System;
using UnityEngine;
using BasicTools;

namespace UIExtended
{
    public abstract class InputManipulator<T> : MonoBehaviour
    {
        public abstract event Action InputReadingStarted;
        public abstract event Action InputReadingStoped;

        public virtual float ScaleFactor { get; set; }

        public virtual Binding<T> InputBinding { get; protected set; } = new Binding<T>();

        public abstract void Enable(Binding<Vector2> manipulatorTargetPosition);

        public abstract void Disable();        

    }
}