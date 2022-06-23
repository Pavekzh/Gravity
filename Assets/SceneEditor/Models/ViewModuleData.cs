using System;
using BasicTools;
using System.Xml.Serialization;
using UnityEngine;

namespace Assets.SceneEditor.Models
{
    public abstract class ViewModuleData:ModuleData
    {
        protected float objectScale = 1;

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
            CommonPropertyViewData<float> radiusProperty = new CommonPropertyViewData<float>();
            radiusProperty.Binding = radiusBinding;
            radiusProperty.Name = "Radius";
            Properties.Add(radiusProperty);
        }

        protected virtual void setRadius(float value, object source)
        {
            
            if (source != this && value != this.objectScale)
            {
                this.objectScale = value;
                UpdateView();
            }
        }

        public abstract void UpdateView();
    }
}
