using System;
using BasicTools;
using UnityEngine;

namespace Assets.Services
{
    public struct GravityStateCurvePoint : StateCurvePoint3D
    {
        private float distanceFromStartPoint;
        private GravityInteractor interactorData;

        public GravityInteractor InteractorData { get => interactorData; }     

        float IStateCurvePoint.DistanceFromStartPoint => distanceFromStartPoint;

        public Vector3 Position { get => interactorData.Position.GetVector3(); }

        public GravityStateCurvePoint(GravityInteractor interactorData,float distanceFromStartPoint)
        {
            this.interactorData = interactorData;
            this.distanceFromStartPoint = distanceFromStartPoint;
        }

        public IStateCurvePoint GetPointBetween( IStateCurvePoint nextPoint,float distanceFromThisPoint)
        {
            if (nextPoint is GravityStateCurvePoint)
            {
                GravityInteractor nextPointData = ((GravityStateCurvePoint)nextPoint).InteractorData;
                float t = distanceFromThisPoint / Vector2.Distance(InteractorData.Position, nextPointData.Position);

                return new GravityStateCurvePoint(GravityInteractor.Lerp(InteractorData, nextPointData, t), distanceFromStartPoint + distanceFromThisPoint);
            }
            else
                throw new Exception("NextPoint was not GravityStateCurvePoint (GravityStateCurvePoint.GetPointBetween)");
        }

        public static implicit operator Vector3(GravityStateCurvePoint stateCurvePoint)
        {
            return stateCurvePoint.Position.GetVectorXZ();
        }
    }
}
