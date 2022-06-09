using System;
using BasicTools;
using UnityEngine;
using UnityEngine.UI;

namespace UIExtended
{
    public class BindedSlider:MonoBehaviour
    {
        [SerializeField] private Slider slider;
        private bool bindingChanges = false;

        private void Start()
        {
            slider.onValueChanged.AddListener(ValueChanged);
        }

        private Binding<float> binding;
        public Binding<float> Binding 
        {
            get
            {
                return binding;
            }
            set
            {
                if (binding != null)
                    binding.ValueChanged -= BindingChanged;

                this.binding = value;
                this.binding.ValueChanged += BindingChanged;
            }
        }

        private void BindingChanged(float value,object sender)
        {
            if(sender != (System.Object)this)
            {                
                bindingChanges = true;
                slider.value = value;
            }

        }

        private void ValueChanged(float value)
        {
            if (!bindingChanges)
                binding.ChangeValue(value, this);
            else
                bindingChanges = false;
        }
    }
}
