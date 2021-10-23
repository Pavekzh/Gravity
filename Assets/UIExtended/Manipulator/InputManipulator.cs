using System.Collections;
using UnityEngine;
using BasicTools;

namespace Assets.UIExtended
{
    public abstract class InputManipulator : MonoBehaviour
    {
        public bool IsManipulatorActive { get; protected set; }

        public abstract Transform Origin { get; set; }

        public abstract event System.EventHandler OnManipulatorActivates;

        public abstract Vector3 GetInputByTouch(Touch touch, float scale);

        public abstract void UpdateView(Vector3 origin, Vector3 touch, float scaleFactor);

        public abstract void Deactivate(Touch touch);

        public abstract bool IsVisible { get; set; }

    }
}