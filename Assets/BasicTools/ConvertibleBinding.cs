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
            base.ChangeValue(value, source);
            PresenterChanged?.Invoke(Converter.ConvertDataToPresenter(value), source);
        }
        public virtual void ChangePresenter(U presenter, object source)
        {
            PresenterChanged?.Invoke(presenter, source);
            ChangeValue(this.Converter.ConvertPresenterToData(presenter), source);
        } 

        public event ValueChangedHandler<U> PresenterChanged;
    }
}
