using Reversi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Reversi.ValidationAttributes
{
    public class InEnumRangeAttribute : ValidationAttribute
    {
        private Type EnumType { get; set; }

        public InEnumRangeAttribute(Type enumType)
        {
            this.EnumType = enumType;
        }

        public override bool IsValid(object value)
        {
            if (!Int32.TryParse(value.ToString(), out int index))
                throw new Exception("Object value is not `Int32` type.");

            int maxEnumIndex = Enum.GetValues(EnumType).Length;

            return index >= 0 && index < maxEnumIndex;
        }
    }
}
