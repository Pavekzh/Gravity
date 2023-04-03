using System;
using UnityEngine;
using BasicTools;

namespace Assets.SceneSimulation
{
    class TerrainFace
    {
        int resolution;
        Vector3 localUp;
        Vector3 axisA;
        Vector3 axisB;
        NoiseSettings noiseSettings;
        float planetRadius;

        public TerrainFace(int resolution, Vector3 localUp,NoiseSettings settings,float planetRadius)
        {
            this.resolution = resolution;
            this.localUp = localUp;
            this.noiseSettings = settings;
            this.planetRadius = planetRadius;

            axisA = new Vector3(localUp.y, localUp.z, localUp.x);
            axisB = Vector3.Cross(localUp, axisA);
        }

        public Mesh ConstructMesh()
        {
            Mesh face = new Mesh();

            Vector3[] vertices = new Vector3[resolution * resolution];
            int[] triangles = new int[(resolution - 1) * (resolution - 1) * 2 * 3];
            int triIndex = 0;
            Vector2[] uv = new Vector2[vertices.Length];

            for (int y = 0; y < resolution; y++)
            {
                for (int x = 0; x < resolution; x++)
                {
                    int i = x + y * resolution;
                    Vector3 percent = new Vector2(x, y) / (resolution - 1);
                    Vector3 pointOnUnitCube = localUp + (percent.x - 0.5f) * 2 * axisA + (percent.y - 0.5f) * 2 * axisB;
                    Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                    float unscaledElevation = PlanetShapeGenerator.GetUnscaledElevation(pointOnUnitSphere,noiseSettings,planetRadius);
                    uv[i].x = unscaledElevation;
                    vertices[i] = pointOnUnitSphere * PlanetShapeGenerator.GetScaledElevation(unscaledElevation,planetRadius);

                    if (x != resolution - 1 && y != resolution - 1)
                    {
                        triangles[triIndex] = i;
                        triangles[triIndex + 1] = i + resolution + 1;
                        triangles[triIndex + 2] = i + resolution;

                        triangles[triIndex + 3] = i;
                        triangles[triIndex + 4] = i + 1;
                        triangles[triIndex + 5] = i + resolution + 1;
                        triIndex += 6;
                    }
                }
            }

            face.Clear();
            face.vertices = vertices;
            face.triangles = triangles;
            face.RecalculateNormals();
            face.uv = uv;
            return face;
        }


    }
}
