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
                BasicTools.ErrorManager.Instance.ShowErrorMessage("InputField value not set", this);
            }
            inputField.onValueChanged.AddListener(ValueChanged);
        }

        public void ValueChanged(string text)
        {
            try
            {
                if(text.Length != 0)
                    binding.ChangeValue(float.Parse(text), this);
            }
            catch (Exception ex)
            {
                ErrorManager.Instance.ShowErrorMessage(ex.Message, this);
            }
        }

        public void BindingChanged(float value, object source)
        {
            if (source != (System.Object)this)
            {
                string text = value.ToString();

                if (text.Length > 3)
                    inputField.text = text.Substring(0, 3);
                else
                    inputField.text = text;
            }
        }
    }
}
