using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicTools.Validation
{
    public class FloatMaximumValidationRule:IValidationRule<float>
    {
        public float DefaultValidValue { get; private set; }

        public bool UseDefaultValue { get; private set; }

        public FloatMaximumValidationRule(float maxValue, bool useMaxValue)
        {
            UseDefaultValue = useMaxValue;
            DefaultValidValue = maxValue;
        }

        public bool ValidateValue(float value)
        {
            if (value <= DefaultValidValue)
                return true;
            else
                return false;
        }
    }
}
