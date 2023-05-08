﻿using Assets.SceneSimulation;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using UnityEngine;
using BasicTools;
using System.Xml.Schema;
using System.Xml;
using Assets.Services;

namespace Assets.SceneEditor.Models
{
    public class PlanetViewModuleData : ViewModuleData
    {

        private PlanetMeshProvider meshProvider;
        private PlanetMaterialProvider materialProvider;

        [XmlIgnore]
        public Binding<NoiseSettings> NoiseSettsBinding { get; private set; }        

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

        [XmlIgnore]
        public override List<PropertyViewData> Properties { get; } = new List<PropertyViewData>();
        [XmlIgnore]
        public override PlanetData Planet { get; set; }


        public PlanetViewModuleData():base()
        {
            ConvertibleBinding<NoiseSettings,string[]> noiseSettsBinding = new BasicTools.ConvertibleBinding<NoiseSettings, string[]>(new NoiseSetsStringConverter());
            this.NoiseSettsBinding = noiseSettsBinding;
            this.NoiseSettsBinding.ValidationRules.Add(new BasicTools.Validation.ValidationRule<NoiseSettings>(validateNoiseSetts));
            noiseSettsBinding.ValueChanged += setNoiseSettsBinding; 
            CommonPropertyViewData<NoiseSettings> noiseSettingsProperty = new CommonPropertyViewData<NoiseSettings>();
            noiseSettingsProperty.Binding = noiseSettsBinding;
            noiseSettingsProperty.Name = "Relief settings";
            noiseSettingsProperty.Components = new string[] {"Scale","Lacunarity","Persistence","OffsetX","OffsetY","OffsetZ","Octaves","Sea level","Strength" };
            Properties.Add(noiseSettingsProperty);
        }

        private bool validateNoiseSetts(NoiseSettings value)
        {
            if (value.MinValue != 0.5f)
                return false;

            return true;
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
            moduleData.Scale = this.Scale;
            moduleData.UpdateView();
            return moduleData;
        }

        public override void OnDeserialized()
        {
            base.OnDeserialized();
            UpdateView();
        }        
        
        public override void UpdateView()
        {
            MeshBinding.ChangeValue(MeshProvider.GetMesh(), this);
            MaterialProvider.UpdateMinMax(new Vector2(PlanetShapeGenerator.ElevationMinMax.Min, PlanetShapeGenerator.ElevationMinMax.Max));
            MaterialBinding.ChangeValue(MaterialProvider.GetMaterial(), this);

            foreach(GradientColorKey c in materialProvider.LandGradient.colorKeys)
            {
                Debug.Log(c);
            }
        }

        public ModuleData GetModuleData()
        {
            return this;
        }

        protected override float CalculateScale(float volume)
        {
            return Mathf.Pow((3 * volume) / (4 * Mathf.PI), 1f / 3f);
        }

        protected override float CalculateVolume(float scale)
        {
            return (4f / 3f) * Mathf.PI * Mathf.Pow(scale, 3);
        }

        protected override void DoHighlight()
        {
            materialProvider.Highlight();
            MaterialBinding.ChangeValue(materialProvider.GetMaterial(), this);
        }

        protected override void DoLessen()
        {
            materialProvider.Lessen();
            MaterialBinding.ChangeValue(materialProvider.GetMaterial(), this);
        }
    }
}
