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
    public class PlanetViewModuleData : ViewModuleData
    {
        public NoiseSettings NoiseSettings
        {
            get => noiseSettings;
            set
            {

                this.MeshProvider.NoiseSettings = value;
                this.noiseSettings = value;
                this.noiseSetsBinding.ChangeValue(value,this);
            }
        } 
        public Gradient LandGradient
        {
            get { return landGradient; }
            set
            {
                this.MaterialProvider.LandGradient = value;
                this.landGradient = value;
            } 
        } 
        public Gradient WaterGradient
        {
            get { return waterGradient; }
            set
            {
                this.MaterialProvider.WaterGradient = value;
                this.waterGradient = value;
            }
        }
        public Mesh GeneratedMesh { get; set; } = new Mesh();

        private Gradient landGradient;
        private Gradient waterGradient;
        private NoiseSettings noiseSettings = new NoiseSettings();
        private PlanetMeshProvider meshProvider;
        private PlanetMaterialProvider materialProvider;

        public PlanetMeshProvider MeshProvider 
        { 
            get
            {
                if (meshProvider != null) return meshProvider;
                else
                {
                    meshProvider = new PlanetMeshProvider(GeneratedMesh,NoiseSettings,1);
                    return meshProvider;
                }
                                    
            } 
        }
        public PlanetMaterialProvider MaterialProvider 
        {
            get
            {
                if (materialProvider != null) return materialProvider;
                else
                {
                    materialProvider = new PlanetMaterialProvider(LandGradient, WaterGradient);
                    return materialProvider;
                }
            }
        
        }

        [XmlIgnore]
        public Binding<NoiseSettings> noiseSetsBinding { get; private set; }

        [XmlIgnore]
        public override List<PropertyViewData> Properties { get; } = new List<PropertyViewData>();
        public override string Name => Key;
        [XmlIgnore]
        public override PlanetData Planet { get; set; }

        public PlanetViewModuleData():base()
        {            
            ConvertibleBinding<NoiseSettings,string[]> noiseSetsBinding = new BasicTools.ConvertibleBinding<NoiseSettings, string[]>(new NoiseSetsStringConverter());
            this.noiseSetsBinding = noiseSetsBinding;
            noiseSetsBinding.ValueChanged += setNoiseSetsBinding; 
            CommonPropertyViewData<NoiseSettings> noiseSettingsProperty = new CommonPropertyViewData<NoiseSettings>();
            noiseSettingsProperty.Binding = noiseSetsBinding;
            noiseSettingsProperty.Name = "Relief settings";
            noiseSettingsProperty.Components = new string[] {"Scale","Lacunarity","Persistence","OffsetX","OffsetY","OffsetZ","Octaves","LowLevel","Strength" };
            Properties.Add(noiseSettingsProperty);
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

        public override object Clone()
        {
            PlanetViewModuleData moduleData = new PlanetViewModuleData();
            moduleData.meshProvider = this.meshProvider.Clone() as PlanetMeshProvider;
            moduleData.materialProvider = this.materialProvider.Clone() as PlanetMaterialProvider;
            moduleData.LandGradient = this.LandGradient;
            moduleData.WaterGradient = this.WaterGradient;
            moduleData.NoiseSettings = this.NoiseSettings;
            moduleData.ObjectScale = this.ObjectScale;
            moduleData.UpdateView();
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
        
        public override void UpdateView()
        {
            MeshBinding.ChangeValue(MeshProvider.GetMesh(), this);
            MaterialProvider.UpdateMinMax(new Vector2(PlanetShapeGenerator.ElevationMinMax.Min, PlanetShapeGenerator.ElevationMinMax.Max));
            MaterialBinding.ChangeValue(MaterialProvider.GetMaterial(), this);
        }

        public ModuleData GetModuleData()
        {
            return this;
        }
    }
}
