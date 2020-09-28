using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Reversi.ValidationAttributes
{
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
