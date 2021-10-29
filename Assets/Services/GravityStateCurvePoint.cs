using System;
using BasicTools;
using UnityEngine;

namespace Assets.Services
{
    struct GravityStateCurvePoint : StateCurvePoint
    {
        private float distanceFromStartPoint;
        private GravityInteractor interactorData;

        public GravityInteractor InteractorData { get => interactorData; }     

        float StateCurvePoint.DistanceFromStartPoint => distanceFromStartPoint;

        public GravityStateCurvePoint(GravityInteractor interactorData,float distanceFromStartPoint)
        {
            this.interactorData = interactorData;
            this.distanceFromStartPoint = distanceFromStartPoint;
        }

        public StateCurvePoint GetPointBetween( StateCurvePoint nextPoint,float distanceFromThisPoint)
        {
            if (nextPoint is GravityStateCurvePoint)
            {
                GravityInteractor nextPointData = ((GravityStateCurvePoint)nextPoint).InteractorData;
                float t = distanceFromThisPoint / Vector2.Distance(InteractorData.Position, nextPointData.Position);

                return new GravityStateCurvePoint(GravityInteractor.Lerp(InteractorData, nextPointData, t), distanceFromStartPoint + distanceFromThisPoint);
            }
            else
                throw new Exception("NextPoint was not GravityStateCurvePoint (GravityStateCurvePoint.GetPointBetweenPoints)");
        }
    }
}
