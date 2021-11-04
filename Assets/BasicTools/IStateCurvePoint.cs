using System;
using UnityEngine;

namespace BasicTools
{
    public interface IStateCurvePoint
    {
        IStateCurvePoint GetPointBetween( IStateCurvePoint nextPoint,float distanceFromThisPoint);

        float DistanceFromStartPoint { get; }

    }
}
