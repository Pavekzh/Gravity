using System;
using BasicTools.Validation;
using System.Collections.Generic;

namespace BasicTools
{
    public delegate void ValueChangedHandler<T>(T value,object source);

    public class Binding<T>
    {
        public List<IValidationRule<T>> ValidationRules = new List<IValidationRule<T>>();
        /// <summary>
        /// This value will be used when several validation rules retruned default value
        /// </summary>
        public T ExceptionalValidValue;


        protected ValueChangedHandler<T> OnValueChanged;
        public event ValueChangedHandler<T> ValueChanged
        {
            add { OnValueChanged += value; }
            remove { OnValueChanged -= value; }
        }

        protected T lastValue;

        public void ForceUpdate()
        {
            this.ChangeValue(this.lastValue,this);
        }

        public virtual void ChangeValue(T value, Object source)
        {
            int returnedDefaultValueCount = 0;
            T validValue;

            if (Validate(value, out returnedDefaultValueCount, out validValue))
            {
                OnValueChanged?.Invoke(value, source);
                this.lastValue = value;
            }
            else
            {
                if(returnedDefaultValueCount == 1)
                {
                    OnValueChanged?.Invoke(validValue, this);
                    this.lastValue = validValue;
                }
                else if(returnedDefaultValueCount > 1)
                {
                    OnValueChanged?.Invoke(ExceptionalValidValue, this);
                    this.lastValue = ExceptionalValidValue;
                }
            }
        }

        protected bool Validate(T value,out int returnedDefaultValueCount,out T validValue)
        {
            bool isValid = true;
            returnedDefaultValueCount = 0;
            validValue = default;
            foreach (IValidationRule<T> rule in ValidationRules)
            {
                if (!rule.ValidateValue(value))
                {
                    isValid = false;

                    if (rule.UseDefaultValue)
                    {
                        validValue = rule.DefaultValidValue;
                        returnedDefaultValueCount++;
                    }
                }
            }
            return isValid;
        }
        public bool Validate(T value)
        {
            foreach (IValidationRule<T> rule in ValidationRules)
            {
                if (rule.ValidateValue(value) == false)
                    return false;
            }
            return true;
        }
    }

}
