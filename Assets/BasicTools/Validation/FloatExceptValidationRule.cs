using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicTools.Validation
{
    class FloatExceptValidationRule
    {
        public float DefaultValidValue { get; }

        public bool UseDefaultValue { get => false; }

        private readonly float exceptValue;

        public FloatExceptValidationRule(float exceptValue)
        {
            this.exceptValue = exceptValue;
        }

        public bool ValidateValue(float value)
        {
            if (!UnityEngine.Mathf.Approximately(value, exceptValue))
                return true;
            else
                return false;
        }
    }
}
