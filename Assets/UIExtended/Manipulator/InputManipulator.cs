using System.Collections;
using UnityEngine;
using BasicTools;

namespace UIExtended
{
    public abstract class InputManipulator : MonoBehaviour
    {
        public delegate void InputReadingHandler();
        public abstract event InputReadingHandler InputReadingStarted;
        public abstract event InputReadingHandler InputReadingStoped;

        public virtual float ScaleFactor { get; set; }

        public virtual Binding<Vector3> InputBinding { get; protected set; }

        public abstract void EnableTool(Binding<Vector2> originBinding);

        public abstract void DisableTool();
    }
}