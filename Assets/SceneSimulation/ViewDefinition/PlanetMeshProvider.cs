using System;
using UnityEngine;
using BasicTools;

namespace Assets.SceneSimulation
{
    [Serializable]
    public class PlanetMeshProvider : IMeshProvider
    {
        public NoiseSettings NoiseSettings 
        {
            get => noiseSettings; 
            set
            {
                noiseSettings = value;
                isSavedMeshValid = false;
            }
        }
        public float PlanetRadius 
        {
            get => planetRadius;
            set
            {
                planetRadius = value;
                isSavedMeshValid = false;
            }
        }

        private NoiseSettings noiseSettings;
        private float planetRadius;
        private TerrainFace[] faces = new TerrainFace[6];
        private static int resolution = 100;
        bool isSavedMeshValid = true;
        Mesh planetMesh;


        public PlanetMeshProvider(Mesh savedMesh,NoiseSettings noiseSettings,float planetRadius)
        {
            this.planetMesh = savedMesh;
            this.noiseSettings = noiseSettings;
            this.planetRadius = planetRadius;
        }

        public Mesh GetMesh()
        {
                if (planetMesh.vertexCount != 0 && isSavedMeshValid) return planetMesh; 
                else 
                {
                    this.planetMesh = GenerateMesh();
                    return planetMesh;
                } 
        }

        public Mesh GenerateMesh()
        {                
            Mesh resultMesh = new Mesh();
            PlanetShapeGenerator.ResetMinMax();
            if(planetRadius != 0)
            {
                Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
                CombineInstance[] combine = new CombineInstance[6];

                for (int i = 0; i < 6; i++)
                {
                    faces[i] = new TerrainFace(resolution, directions[i], NoiseSettings,planetRadius);
                    combine[i].mesh = faces[i].ConstructMesh();
                    combine[i].subMeshIndex = 0;
                }
                resultMesh.CombineMeshes(combine, true, false);
            }
            this.planetMesh = resultMesh;
            this.isSavedMeshValid = true;
            return resultMesh;
        }
    }
}
