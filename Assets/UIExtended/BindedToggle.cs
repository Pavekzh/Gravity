using System.Collections;
using BasicTools;
using UnityEngine;
using UnityEngine.UI;

namespace UIExtended
{
    public class BindedToggle : MonoBehaviour
    {
        [SerializeField] Toggle toggle;

        private Binding<bool> binding;
        public Binding<bool> Binding
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

        private void Start()
        {
            toggle.onValueChanged.AddListener(ValueChanged);
        }

        private void BindingChanged(bool value, object sender)
        {
            if (sender != (System.Object)this)
            {
                toggle.SetIsOnWithoutNotify(value);
            }
        }

        private void ValueChanged(bool value)
        {
            binding.ChangeValue(value, this);
        }
    }
}