using System;
using System.ComponentModel.DataAnnotations;

namespace Reversi.ValidationAttributes
{
    /// <summary>
    /// Attribute for checking the multiplicity of a number.
    /// </summary>
    public class MultipleAttribute : ValidationAttribute
    {
        private int Divider { get; set; }

        public MultipleAttribute(int divider)
        {
            this.Divider = divider;
        }

        public override bool IsValid(object value)
        {
            if (!Int32.TryParse(value.ToString(), out int dividend))
                throw new Exception("Object value is not `Int32` type.");

            return dividend % Divider == 0;
        }
    }
}
