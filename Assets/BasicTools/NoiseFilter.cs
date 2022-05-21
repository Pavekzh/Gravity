using System;
using UnityEngine;

namespace BasicTools
{
	public delegate float NoiseMethod(Vector3 point, float frequency);

	public static class NoiseFilter
    {

		public static float Evaluate(Vector3 point,NoiseSettings settings)
		{
			float elevation = Sum(Perlin.Perlin3D, point + settings.Center, settings.Frequency, settings.Octaves, settings.Lacunarity, settings.Persistence) * settings.Strength + (0.5f);
			elevation = elevation - settings.MinValue;
			return elevation;
		}


		public static float Sum(NoiseMethod method, Vector3 point, float frequency, int octaves, float lacunarity, float persistence)
		{
			float sum = method(point, frequency);
			float amplitude = 1f;
			float range = 1f;
			for (int o = 1; o < octaves; o++)
			{
				frequency *= lacunarity;
				amplitude *= persistence;
				range += amplitude;
				sum += method(point, frequency) * amplitude;
			}
			return sum / range;
		}
	}
}
