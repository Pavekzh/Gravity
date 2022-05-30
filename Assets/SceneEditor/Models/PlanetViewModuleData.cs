using Assets.SceneSimulation;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using UnityEngine;
using BasicTools;
using System.Xml.Schema;
using System.Xml;

namespace Assets.SceneEditor.Models
{
    public class PlanetViewModuleData : ModuleData, IViewModuleData
    {
        public NoiseSettings NoiseSettings
        {
            get => noiseSettings;
            set
            {
                this.noiseSettings = value;
                this.noiseSetsBinding.ChangeValue(value,this);
            }
        } 
        public float PlanetRadius 
        {
            get { return planetRadius; }
            set
            {
                this.planetRadius = value;
                this.radiusBinding.ChangeValue(value,this);
            }
        }
        public Gradient LandGradient { get; set; } = new Gradient();
        public Gradient WaterGradient { get; set; } = new Gradient();
        public Mesh GeneratedMesh { get; set; } = new Mesh();

        private NoiseSettings noiseSettings = new NoiseSettings();
        private float planetRadius = 1;
        private PlanetMeshProvider meshProvider;
        private PlanetMaterialProvider materialProvider;

        public IMeshProvider MeshProvider 
        { 
            get
            {
                if (meshProvider != null) return meshProvider;
                else
                {
                    materialProvider = new PlanetMaterialProvider(LandGradient,WaterGradient);
                    meshProvider = new PlanetMeshProvider(GeneratedMesh,NoiseSettings,planetRadius);
                    return meshProvider;
                }
                                    
            } 
        }
        public IMaterialProvider MaterialProvider 
        {
            get
            {
                if (materialProvider != null) return materialProvider;
                else
                {
                    materialProvider = new PlanetMaterialProvider(LandGradient, WaterGradient);
                    meshProvider = new PlanetMeshProvider(GeneratedMesh, NoiseSettings,planetRadius);
                    return materialProvider;
                }
            }
        
        }

        public static string Key = "PlanetView";

        private ConvertibleBinding<float, string[]> radiusBinding;
        private ConvertibleBinding<NoiseSettings, string[]> noiseSetsBinding;

        public PlanetViewModuleData()
        {
            noiseSetsBinding = new BasicTools.ConvertibleBinding<NoiseSettings, string[]>(new NoiseSetsStringConverter());
            noiseSetsBinding.ValueChanged += setNoiseSetsBinding; 
            CommonPropertyViewData<NoiseSettings> noiseSettingsProperty = new CommonPropertyViewData<NoiseSettings>();
            noiseSettingsProperty.Binding = noiseSetsBinding;
            noiseSettingsProperty.Name = "Relief settings";
            noiseSettingsProperty.Components = new string[] {"Scale","Lacunarity","Persistence","OffsetX","OffsetY","OffsetZ","Octaves","LowLevel","Strength" };
            Properties.Add(noiseSettingsProperty);

            radiusBinding = new BasicTools.ConvertibleBinding<float, string[]>(new FloatStringConverter());
            radiusBinding.ValueChanged += setRadius;
            CommonPropertyViewData<float> radiusProperty = new CommonPropertyViewData<float>();
            radiusProperty.Binding = radiusBinding;
            radiusProperty.Name = "Radius";
            Properties.Add(radiusProperty);
        }

        private void setNoiseSetsBinding(NoiseSettings value, object source)
        {
            if(source != this && !value.Equals(this.noiseSettings))
            {
                meshProvider.NoiseSettings = value;
                this.noiseSettings = value;
                UpdateView();

            }
        }

        private void setRadius(float value, object source)
        {
            if(source != this && value != this.planetRadius)
            {
                meshProvider.PlanetRadius = value;
                this.planetRadius = value;
                UpdateView();
            }
        }

        [XmlIgnore]
        public override List<PropertyViewData> Properties { get; } = new List<PropertyViewData>();
        public override string Name => Key;
        [XmlIgnore]
        public override PlanetData Planet { get; set; }
        public Binding<Mesh> MeshBinding { get; } = new Binding<Mesh>();
        public Binding<Material> MaterialBinding { get; } = new Binding<Material>();


        public override object Clone()
        {
            PlanetViewModuleData moduleData = (PlanetViewModuleData)this.MemberwiseClone();

            return moduleData;
        }

        public override void CreateModule(GameObject sceneObject)
        {
            ViewDefinitionModule viewModule = sceneObject.AddComponent<ViewDefinitionModule>();
            viewModule.ModuleData = this;
        }

        public override void OnDeserialized()
        {
            UpdateView();
        }        
        
        private void UpdateView()
        {
            MeshBinding.ChangeValue(MeshProvider.GetMesh(), this);
            materialProvider.UpdateMinMax(new Vector2(PlanetShapeGenerator.ElevationMinMax.Min, PlanetShapeGenerator.ElevationMinMax.Max));
            MaterialBinding.ChangeValue(MaterialProvider.GetMaterial(), this);
        }

        public ModuleData GetModuleData()
        {
            return this;
        }
    }
}
