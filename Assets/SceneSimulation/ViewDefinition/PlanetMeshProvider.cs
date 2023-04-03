using System;
using UnityEngine;
using BasicTools;

namespace Assets.SceneSimulation
{
    [Serializable]
    public class PlanetMeshProvider:ICloneable
    {
        public NoiseSettings NoiseSettings 
        {
            get => noiseSettings; 
            set
            {
                noiseSettings = value;
                if (NoiseSettsBinding != null)
                    NoiseSettsBinding.ChangeValue(value,this);
                isSavedMeshValid = false;
            }
        }

        private const float planetRadius = 1;        
        private static int resolution = 100;

        private Mesh savedMesh;
        private NoiseSettings noiseSettings;
        private TerrainFace[] faces = new TerrainFace[6];
        private bool isSavedMeshValid = true;

        [System.Xml.Serialization.XmlIgnore]
        public Binding<NoiseSettings> NoiseSettsBinding { get; set; }

        public PlanetMeshProvider()
        {
            noiseSettings = new NoiseSettings();
            savedMesh = new Mesh();
        }

        public PlanetMeshProvider(Mesh savedMesh,NoiseSettings noiseSettings)
        {
            this.savedMesh = savedMesh;
            this.noiseSettings = noiseSettings;
        }

        public Mesh GetMesh()
        {
                if (savedMesh.vertexCount != 0 && isSavedMeshValid) return savedMesh; 
                else 
                {
                    this.savedMesh = GenerateMesh();
                    return savedMesh;
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
            this.savedMesh = resultMesh;
            this.isSavedMeshValid = true;
            return resultMesh;
        }

        public object Clone()
        {
            PlanetMeshProvider meshProvider = new PlanetMeshProvider(this.savedMesh, this.noiseSettings);
            return meshProvider;
        }
    }
}
