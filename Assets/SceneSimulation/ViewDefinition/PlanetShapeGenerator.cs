using System;
using UnityEngine;
using BasicTools;

namespace Assets.SceneSimulation
{
    public static class PlanetShapeGenerator
    {
        public static MinMax ElevationMinMax { get; private set; }

        public static void ResetMinMax()
        {
            ElevationMinMax = new MinMax();
        }

        public static float GetUnscaledElevation(Vector3 pointOnUnitSphere,NoiseSettings settings,float planetRadius)
        {
            float elevation = NoiseFilter.Evaluate(pointOnUnitSphere,settings);
            ElevationMinMax.AddValue(elevation);
            return elevation;
        }

        public static float GetScaledElevation(float unscaledElevation,float planetRadius)
        {
            float elevation = Mathf.Max(0, unscaledElevation);
            elevation = planetRadius * (1 + elevation);
            return elevation;
        }

    }
}
