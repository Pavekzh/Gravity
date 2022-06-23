using Assets.SceneSimulation;
using System;
using System.Collections.Generic;
using UnityEngine;
using BasicTools;
using System.Xml.Serialization;
using Assets.Services;

namespace Assets.SceneEditor.Models
{
    public class GravityModuleData : ModuleData
    {
        private GravityInteractor data;

        public GravityModuleData()
        {
            //init ui changeble properties
            ConvertibleBinding<Vector2, string[]> positionBinding = new BasicTools.ConvertibleBinding<Vector2, string[]>(new VectorStringConverter());
            positionBinding.ValueChanged += setPosition;
            CommonPropertyViewData<Vector2> positionProperty = new CommonPropertyViewData<Vector2>();
            positionProperty.Binding = positionBinding;
            positionProperty.Name = "Position";
            positionProperty.Components = new string[] { "x", "y" };
            PositionProperty = positionProperty;
            Properties.Add(PositionProperty);

            ConvertibleBinding<float, string[]> massBinding = new BasicTools.ConvertibleBinding<float, string[]>(new FloatStringConverter());
            massBinding.ValueChanged += setMass;
            CommonPropertyViewData<float> massProperty = new CommonPropertyViewData<float>();
            massProperty.Binding = massBinding;
            massProperty.Name = "Mass";
            MassProperty = massProperty;
            Properties.Add(MassProperty);

            ConvertibleBinding<Vector2, string[]> velocityBinding = new BasicTools.ConvertibleBinding<Vector2, string[]>(new VectorStringConverter());
            velocityBinding.ValueChanged += setVelocity;
            CommonPropertyViewData<Vector2> velocityProperty = new CommonPropertyViewData<Vector2>();
            velocityProperty.Binding = velocityBinding;
            velocityProperty.Name = "Velocity";
            velocityProperty.Components = new string[] { "x", "y" };
            VelocityProperty = velocityProperty;
            Properties.Add(VelocityProperty);
        }
        public GravityModuleData(ConvertibleBinding<Vector2, string[]> positionBinding, ConvertibleBinding<Vector2, string[]> velocityBinding, ConvertibleBinding<float, string[]> massBinding)
        {
            //init ui changeble properties
            CommonPropertyViewData<Vector2> positionProperty = new CommonPropertyViewData<Vector2>();
            positionBinding.ValueChanged += setPosition;
            positionProperty.Binding = positionBinding;
            positionProperty.Name = "Position";
            positionProperty.Components = new string[] { "x", "y" };
            PositionProperty = positionProperty;
            Properties.Add(PositionProperty);

            CommonPropertyViewData<Vector2> velocityProperty = new CommonPropertyViewData<Vector2>();
            velocityBinding.ValueChanged += setVelocity;
            velocityProperty.Binding = velocityBinding;
            velocityProperty.Name = "Velocity";
            velocityProperty.Components = new string[] { "x", "y" };
            VelocityProperty = velocityProperty;
            Properties.Add(VelocityProperty);

            CommonPropertyViewData<float> massProperty = new CommonPropertyViewData<float>();
            massBinding.ValueChanged += setMass;
            massProperty.Binding = massBinding;
            massProperty.Name = "Mass";
            MassProperty = massProperty;
            Properties.Add(MassProperty);
        }

        public GravityInteractor Data { get => data; }
        public float Mass
        {
            get => data.Mass;
            set
            {
                MassProperty.Binding.ChangeValue(value, this);
                data.Mass = value;
            }
        }
        public Vector2 Position
        {
            get => data.Position;
            set
            {
                PositionProperty.Binding.ChangeValue(value, this);
                data.Position = value;
            }
        }
        public Vector2 Velocity
        {
            get => data.Velocity;
            set
            {
                VelocityProperty.Binding.ChangeValue(value, this);
                data.Velocity = value;
            }
        }

        public CommonPropertyViewData<float> MassProperty { get; }
        public CommonPropertyViewData<Vector2> PositionProperty { get; }
        public CommonPropertyViewData<Vector2> VelocityProperty { get; }

        [XmlIgnore]
        public override List<PropertyViewData> Properties { get; } = new List<PropertyViewData>();
        public override string Name { get => Key; }
        [XmlIgnore]
        public override PlanetData Planet  { get; set; }

        public static string Key { get => "Gravity"; }

        public override void CreateModule(GameObject planetObject)
        {
            Assets.SceneSimulation.GravityModule gravityModule = planetObject.AddComponent<SceneSimulation.GravityModule>();
            gravityModule.SetModuleData(this);
        }

        public override object Clone()
        {
            GravityModuleData clonedData = new GravityModuleData();
            clonedData.Mass = this.Mass;
            clonedData.Velocity = this.Velocity;
            clonedData.Position = this.Position;

            return clonedData;
        }

        private void setMass(float value,object sender)
        {
            if (sender != this)
                this.data.Mass = value;
        }
        private void setPosition(Vector2 value,object sender)
        {

            if (sender != this)
            {
                this.data.Position = value;
            }
        }
        private void setVelocity(Vector2 value, object sender)
        {
            if (sender != this)
                this.data.Velocity = value;
        }

        public override void OnDeserialized() { }
    }
}
