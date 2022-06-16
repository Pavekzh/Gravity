using System;
using UnityEngine;
using System.Globalization;
using TMPro;
using BasicTools;

namespace UIExtended
{
    public class BindedInputField:MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        private bool bindingChanges = false;

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

        void Start()
        {
            if (inputField == null)
            {
                BasicTools.MessagingSystem.Instance.ShowErrorMessage("InputField value not set", this);
            }
            inputField.onValueChanged.AddListener(ValueChanged);
        }

        public void ValueChanged(string text)
        {
            if (!bindingChanges)
            {
                try
                {
                    if (text.Length != 0)
                        binding.ChangeValue(float.Parse(text), this);
                }
                catch (Exception ex)
                {
                    MessagingSystem.Instance.ShowErrorMessage(ex.Message, this);
                }
            }
            else bindingChanges = false;
        }

        public void BindingChanged(float value, object source)
        {
            if (source != (System.Object)this)
            {
                bindingChanges = true;
                string text = value.ToString();

                if (text.Length > 3)
                    inputField.text = text.Substring(0, 3);
                else
                    inputField.text = text;
            }
        }
    }
}
