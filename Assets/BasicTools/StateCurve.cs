using System.Collections.Generic;

namespace BasicTools
{
    public class StateCurve<T> where T : IStateCurvePoint
    {
        private List<T> points = new List<T>();

        /// <summary>
        /// do not use Points.Add(), use StateCurve.AddPoint() instead 
        /// </summary>
        public List<T> Points { get => points; }

        public float Length { get; private set; } = 0;

        public int PointsAmount { get; private set; } = 0;

        public void AddPoint (T point)
        {
            points.Add(point);
            PointsAmount++;
            Length = point.DistanceFromStartPoint;     
        }

        public IStateCurvePoint GetPointAlongCurve(float distanceFromStart)
        {
            int startIndex = 0;
            int endIndex = PointsAmount - 1;
            while (true)
            {
                int middleIndex = startIndex + (endIndex - startIndex) / 2;
                float middleObjectDistance = points[middleIndex].DistanceFromStartPoint;
                if (distanceFromStart > middleObjectDistance)
                {
                    startIndex = middleIndex;

                }
                else
                {
                    endIndex = middleIndex;
                }

                if (endIndex - startIndex <= 1)
                {
                    return points[startIndex].GetPointBetween(points[endIndex], distanceFromStart - points[startIndex].DistanceFromStartPoint);
                }
            }
        }
    }
}
