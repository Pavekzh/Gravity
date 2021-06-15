using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Library
{
    public delegate void ValueChangedHandler<T>(T value,object source);
    public delegate bool ValidateValueCallback<T>(T value, object source);
    public class Binding<U>
    {
        public void ChangeValue(U value, Object source)
        {
            if(ValidateValue != null)
            {
                if(ValidateValue.Invoke(value,source))
                    ValueChanged?.Invoke(value, source);
            }
            else
            {
                ValueChanged?.Invoke(value, source);
            }
        }
        public event ValueChangedHandler<U> ValueChanged;
        public event ValidateValueCallback<U> ValidateValue;
    }

}
