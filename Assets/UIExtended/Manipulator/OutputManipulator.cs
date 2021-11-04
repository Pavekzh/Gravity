using System.Collections;
using UnityEngine;

namespace UIExtended
{
    public abstract class OutputManipulator : MonoBehaviour
    {
        public abstract bool IsVisible { get; set; }

        public abstract void UpdateManipulatorView(Vector3 origin, Vector3 directPoint, float scaleFactor);
    }
}