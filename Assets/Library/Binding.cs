using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Library
{
    public delegate void ValueChanged<T>(T value,object source);
    public class Binding<U>
    {
        public void ChangeValue(U value, Object source)
        {
            ValueChanged?.Invoke(value, source);
        }
        public event ValueChanged<U> ValueChanged;
    }

}
