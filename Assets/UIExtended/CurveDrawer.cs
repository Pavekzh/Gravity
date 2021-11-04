using UnityEngine;
using System.Collections.Generic;
using BasicTools;

namespace UIExtended
{
    public abstract class CurveDrawer : MonoBehaviour
    {
        public abstract void Draw(StateCurve<StateCurvePoint3D> curve);
    }
}