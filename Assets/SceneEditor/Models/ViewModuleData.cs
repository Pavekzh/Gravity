using System;
using BasicTools;
using System.Xml.Serialization;
using UnityEngine;

namespace Assets.SceneEditor.Models
{
    public abstract class ViewModuleData:ModuleData
    {
        protected float objectScale = 1;

        public const float MinRadius = 0.2f;
        public static string Key = "ViewModule";

        public float ObjectScale
        {
            get { return objectScale; }
            set
            {
                this.objectScale = value;
                this.ScaleBinding.ChangeValue(value, this);
            }
        }

        public sealed override string Name => Key;

        [XmlIgnore]
        public virtual Binding<Mesh> MeshBinding { get; protected set; } = new Binding<Mesh>();
        [XmlIgnore]
        public virtual Binding<Material> MaterialBinding { get; protected set; } = new Binding<Material>();
        [XmlIgnore]
        public virtual Binding<float> ScaleBinding { get; protected set; }


        public ViewModuleData()
        {
            ConvertibleBinding<float, string[]> radiusBinding = new BasicTools.ConvertibleBinding<float, string[]>(new FloatStringConverter());
            this.ScaleBinding = radiusBinding;
            radiusBinding.ValueChanged += setRadius;
            radiusBinding.ValidateValue += validateRadius;
            CommonPropertyViewData<float> radiusProperty = new CommonPropertyViewData<float>();
            radiusProperty.Binding = radiusBinding;
            radiusProperty.Name = "Radius";
            Properties.Add(radiusProperty);
        }

        private bool validateRadius(float value, object source)
        {

            if(value >= MinRadius)
            {
                return true;
            }
            else
            {
                ScaleBinding.ChangeValue(MinRadius, this);
                return false;
            }
        }

        protected virtual void setRadius(float value, object source)
        {
            
            if (source != this && value != this.objectScale)
            {
                this.objectScale = value;
                UpdateView();
            }
        }

        public abstract void DisableView();

        public abstract void EnableView();

        public abstract void UpdateView();
    }
}
