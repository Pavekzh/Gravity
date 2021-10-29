using System.Collections.Generic;

namespace BasicTools
{
    public class StateCurve
    {
        private List<StateCurvePoint> points = new List<StateCurvePoint>();

        public IEnumerable<StateCurvePoint> Points { get => points; }

        public float Length { get; private set; } = 0;

        public int PointsAmount { get; private set; } = 0;

        public void AddPoint (StateCurvePoint point)
        {
            points.Add(point);
            PointsAmount++;
            Length = point.DistanceFromStartPoint;     
        }

        public StateCurvePoint GetPointAlongCurve(float distanceFromStart)
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
