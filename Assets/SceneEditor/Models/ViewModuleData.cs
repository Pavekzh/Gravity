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
        protected bool isHighlighted = false;

        public override int DisplayIndex => 1;

        public float MinVolume { get => CalculateVolume(MinScale); }
        public float MaxVolume { get => CalculateVolume(MaxScale); }
        public const float MinScale = 0.2f;
        public const float MaxScale = 10;

        public static string Key = "ViewModule";

        public float Scale
        {
            get { return objectScale; }
            set
            {
                this.objectScale = value;
                this.ScaleBinding.ChangeValue(value, this);
                this.VolumeBinding.ChangeValue(CalculateVolume(value), this);

                if(isDeserialized)
                    UpdateView();
            }
        }
        [XmlIgnore]
        public float Volume
        {
            get
            {
                return CalculateVolume(this.objectScale);
            }

            set
            {
                float scale = CalculateScale(value);
                this.objectScale = scale;
                this.ScaleBinding.ChangeValue(scale, this);
                this.VolumeBinding.ChangeValue(value, this);

                if (isDeserialized)
                    UpdateView();
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

        public static IValidationRule<float>[] ScaleValidationRules { get; protected set; } = new IValidationRule<float>[] 
        {
            new FloatMinimumValidationRule(MinScale,true),
            new FloatMaximumValidationRule(MaxScale,true)
        };

        public ViewModuleData()
        {
            ConvertibleBinding<float, string[]> scaleBinding = new ConvertibleBinding<float, string[]>(new FloatStringConverter());
            this.ScaleBinding = scaleBinding;
            scaleBinding.ValueChanged += setScale;
            scaleBinding.ValidationRules.AddRange(ScaleValidationRules);

            ConvertibleBinding<float, string[]> volumeBinding = new ConvertibleBinding<float, string[]>(new FloatStringConverter());
            this.VolumeBinding = volumeBinding;
            volumeBinding.ValueChanged += setVolume;
            IValidationRule<float> minVolumeRule = new FloatMinimumValidationRule(MinVolume, true);
            IValidationRule<float> maxVolumeRule = new FloatMaximumValidationRule(MaxVolume, true);
            volumeBinding.ValidationRules.Add(minVolumeRule);
            volumeBinding.ValidationRules.Add(maxVolumeRule);

            CommonPropertyViewData<float> scaleProperty = new CommonPropertyViewData<float>();
            scaleProperty.Binding = scaleBinding;
            scaleProperty.Name = "Radius";
            Properties.Add(scaleProperty);

            CommonPropertyViewData<float> volumeProperty = new CommonPropertyViewData<float>();
            volumeProperty.Binding = volumeBinding;
            volumeProperty.Name = "Volume";
            Properties.Add(volumeProperty);
        }

        public void Highlight()
        {
            if (!isHighlighted)
            {                
                DoHighlight();
                isHighlighted = true;
            }

        }

        public void Lessen()
        {
            if (isHighlighted)
            {                
                DoLessen();
                isHighlighted = false;
            }

        }

        protected abstract void DoHighlight();

        protected abstract void DoLessen();

        protected virtual float CalculateVolume(float scale)
        {
            return MathF.Pow(scale, 3);
        }

        protected virtual float CalculateScale(float volume)
        {
            return MathF.Cbrt(volume);
        }

        protected virtual void setVolume(float value, object source)
        {
            float scale = CalculateScale(value);
            if (source != this && scale != this.objectScale)
            {
                ScaleBinding.ChangeValue(scale, this);
                this.objectScale = scale;
                UpdateView();
            }
        }

        protected virtual void setScale(float value, object source)
        {       
            if (source != this && value != this.objectScale)
            {
                float volume = CalculateVolume(value);
                VolumeBinding.ChangeValue(volume, this);
                this.objectScale = value;
                UpdateView();
            }
        }

        public abstract void DisableView();

        public abstract void EnableView();

        public abstract void UpdateView();
    }
}
