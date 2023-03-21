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
        [SerializeField] private int maxBindedTextLength = 3;

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
            try
            {
                if (text.Length != 0 && text != "-")
                    binding.ChangeValue(float.Parse(text), this);
            }
            catch (Exception ex)
            {
                MessagingSystem.Instance.ShowErrorMessage(ex.Message, this);
            }

        }

        public void BindingChanged(float value, object source)
        {
            if (source != (System.Object)this)
            {
                string text = value.ToString();

                if (text.Length > maxBindedTextLength)
                    inputField.SetTextWithoutNotify(text.Substring(0, maxBindedTextLength));
                else
                    inputField.SetTextWithoutNotify(text);
            }
        }
    }
}
