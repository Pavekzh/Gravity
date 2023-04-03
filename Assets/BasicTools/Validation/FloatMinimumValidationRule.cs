using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicTools.Validation
{
    public class FloatMinimumValidationRule : IValidationRule<float>
    {
        public float DefaultValidValue { get; private set; }

        public bool UseDefaultValue { get; private set; }

        public FloatMinimumValidationRule(float minValue,bool useMinValue)
        {
            UseDefaultValue = useMinValue;
            DefaultValidValue = minValue;
        }

        public bool ValidateValue(float value)
        {
            if (value >= DefaultValidValue)
                return true;
            else
                return false;
        }
    }
}
