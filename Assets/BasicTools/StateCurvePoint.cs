using System;
using UnityEngine;

namespace BasicTools
{
    public interface StateCurvePoint
    {
        StateCurvePoint GetPointBetween( StateCurvePoint nextPoint,float distanceFromThisPoint);

        float DistanceFromStartPoint { get; }

    }
}
