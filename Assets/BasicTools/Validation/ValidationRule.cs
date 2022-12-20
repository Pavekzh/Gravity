using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicTools.Validation
{
    public class ValidationRule<T> : IValidationRule<T>
    {
        public T DefaultValidValue { get; private set; }

        public bool UseDefaultValue { get; private set; }

        private readonly ValidateMethod validateMethod;

        public delegate bool ValidateMethod(T value);

        public ValidationRule(ValidateMethod method)
        {
            validateMethod = method;
            UseDefaultValue = false;
        }

        public ValidationRule(T defaultValidValue,ValidateMethod method)
        {
            DefaultValidValue = defaultValidValue;
            validateMethod = method;
            UseDefaultValue = true;
        }

        public bool ValidateValue(T value)
        {
            return validateMethod(value);
        }
    }
}
