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
        public PlanetMeshProvider MeshProvider 
        {
            get
            {
                if (meshProvider == null) 
                {
                    meshProvider = new PlanetMeshProvider(new Mesh(), new NoiseSettings());
                }
                return meshProvider;
            }
            set
            {
                value.NoiseSettsBinding = this.NoiseSettsBinding;                
                meshProvider = value;
                NoiseSettsBinding.ChangeValue(meshProvider.NoiseSettings, this);
            }
        }         
        public PlanetMaterialProvider MaterialProvider 
        {
            get
            {
                if (materialProvider == null)
                {
                    materialProvider = new PlanetMaterialProvider();
                }
                return materialProvider;
            }
            set
            {
                materialProvider = value;
            }
        }

        private PlanetMeshProvider meshProvider;
        private PlanetMaterialProvider materialProvider;
        private ViewDefinitionModule viewModule;



        [XmlIgnore]
        public Binding<NoiseSettings> NoiseSettsBinding { get; private set; }

        [XmlIgnore]
        public override List<PropertyViewData> Properties { get; } = new List<PropertyViewData>();
        [XmlIgnore]
        public override PlanetData Planet { get; set; }

        public PlanetViewModuleData():base()
        {    
            ConvertibleBinding<NoiseSettings,string[]> noiseSettsBinding = new BasicTools.ConvertibleBinding<NoiseSettings, string[]>(new NoiseSetsStringConverter());
            this.NoiseSettsBinding = noiseSettsBinding;
            noiseSettsBinding.ValueChanged += setNoiseSettsBinding; 
            CommonPropertyViewData<NoiseSettings> noiseSettingsProperty = new CommonPropertyViewData<NoiseSettings>();
            noiseSettingsProperty.Binding = noiseSettsBinding;
            noiseSettingsProperty.Name = "Relief settings";
            noiseSettingsProperty.Components = new string[] {"Scale","Lacunarity","Persistence","OffsetX","OffsetY","OffsetZ","Octaves","LowLevel","Strength" };
            Properties.Add(noiseSettingsProperty);
        }

        private void setNoiseSettsBinding(NoiseSettings value, object source)
        {
            if(source != this && !value.Equals(this.MeshProvider.NoiseSettings))
            {
                meshProvider.NoiseSettings = value;
                UpdateView();
            }
        }

        public override object Clone()
        {
            PlanetViewModuleData moduleData = new PlanetViewModuleData();
            moduleData.MeshProvider = this.meshProvider.Clone() as PlanetMeshProvider;
            moduleData.MaterialProvider = this.materialProvider.Clone() as PlanetMaterialProvider;
            moduleData.ObjectScale = this.ObjectScale;
            moduleData.UpdateView();
            return moduleData;
        }

        public override void CreateModule(GameObject sceneObject)
        {
            viewModule = sceneObject.AddComponent<ViewDefinitionModule>();
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

        public override void DisableView()
        {
            viewModule.DisableView();
        }

        public override void EnableView()
        {
            viewModule.EnableView();
        }
    }
}
