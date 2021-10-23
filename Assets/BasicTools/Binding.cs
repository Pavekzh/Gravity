using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicTools
{
    public delegate void ValueChangedHandler<T>(T value,object source);
    public delegate bool ValidateValueCallback<T>(T value, object source);
    public class Binding<T>
    {
        private T value;

        public void ForceUpdate()
        {
            this.ChangeValue(this.value,this);
        }

        public virtual void ChangeValue(T value, Object source)
        {
            if(ValidateValue != null)
            {
                if (ValidateValue.Invoke(value, source))
                {
                    ValueChanged?.Invoke(value, source);
                    this.value = value;
                }
            }
            else
            {
                ValueChanged?.Invoke(value, source);
                this.value = value;
            }
        }

        public event ValueChangedHandler<T> ValueChanged;
        public event ValidateValueCallback<T> ValidateValue;

    }

}
