using System;
using BasicTools;
using BasicTools.Validation;
using System.Xml.Serialization;
using UnityEngine;

namespace Assets.SceneEditor.Models
{
    public abstract class ViewModuleData:ModuleData
    {
        protected float objectScale = 1;

        public const float MinScale = 0.2f;
        public const float MaxScale = 10;
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
        [XmlIgnore]
        public virtual Binding<float> VolumeBinding { get; protected set; }

        public static IValidationRule<float>[] ScaleValidationRule { get; protected set; } = new IValidationRule<float>[] 
        {
            new FloatMinimumValidationRule(MinScale,true),
            new FloatMaximumValidationRule(MaxScale,true)
        };

        public ViewModuleData()
        {
            ConvertibleBinding<float, string[]> radiusBinding = new BasicTools.ConvertibleBinding<float, string[]>(new FloatStringConverter());
            this.ScaleBinding = radiusBinding;
            radiusBinding.ValueChanged += setScale;
            radiusBinding.ValidationRules.AddRange(ScaleValidationRule);
            CommonPropertyViewData<float> radiusProperty = new CommonPropertyViewData<float>();
            radiusProperty.Binding = radiusBinding;
            radiusProperty.Name = "Radius";
            Properties.Add(radiusProperty);
        }

        protected virtual float CalculateVolume(float scale)
        {
            return MathF.Pow(scale, 3);
        }

        protected virtual float CalculateScale(float volume)
        {
            return MathF.Cbrt(volume);
        }

        protected virtual void setScale(float value, object source)
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
