using System;
using UnityEngine;
using System.Globalization;

namespace BasicTools
{
    public class ConvertibleBinding<T, U> : Binding<T>
    {
        public IDataPresenterConverter<T,U> Converter { get; set; }

        public ConvertibleBinding(){}

        public ConvertibleBinding(IDataPresenterConverter<T,U> converter)
        {
            Converter = converter;
        }

        public override void ChangeValue(T value, object source)
        {
            T validValue;
            int returnedDefaultValueCount;

            if (this.Validate(value, out returnedDefaultValueCount, out validValue))
            {
                OnValueChanged?.Invoke(value, source);
                OnPresenterChanged?.Invoke(Converter.ConvertDataToPresenter(value), source);
                lastValue = value;
            }
            else
            {
                if (returnedDefaultValueCount == 1)
                {
                    OnValueChanged?.Invoke(validValue, this);
                    OnPresenterChanged?.Invoke(Converter.ConvertDataToPresenter(validValue), this);
                    lastValue = validValue;
                }
                else if (returnedDefaultValueCount > 1)
                {
                    OnValueChanged?.Invoke(ExceptionalValidValue, this);
                    OnPresenterChanged?.Invoke(Converter.ConvertDataToPresenter(ExceptionalValidValue), this);
                    lastValue = ExceptionalValidValue;
                }
            }
        }
        public virtual void ChangePresenter(U presenter, object source)
        {
            T value = Converter.ConvertPresenterToData(presenter);
            T validValue;
            int returnedDefaultValueCount;

            if (this.Validate(value, out returnedDefaultValueCount, out validValue))
            {
                OnValueChanged?.Invoke(value, source);
                OnPresenterChanged?.Invoke(presenter, source);
                lastValue = value;
            }
            else
            {
                if (returnedDefaultValueCount == 1)
                {
                    OnValueChanged?.Invoke(validValue, this);
                    OnPresenterChanged?.Invoke(Converter.ConvertDataToPresenter(validValue), this);
                    lastValue = validValue;
                }
                else if(returnedDefaultValueCount > 1)
                {
                    OnValueChanged?.Invoke(ExceptionalValidValue, this);
                    OnPresenterChanged?.Invoke(Converter.ConvertDataToPresenter(ExceptionalValidValue), this);
                    lastValue = ExceptionalValidValue;
                }
            }
        }

        protected ValueChangedHandler<U> OnPresenterChanged;
        public event ValueChangedHandler<U> PresenterChanged
        {
            add { OnPresenterChanged += value; }
            remove { OnPresenterChanged -= value; }
        }
    }
}
