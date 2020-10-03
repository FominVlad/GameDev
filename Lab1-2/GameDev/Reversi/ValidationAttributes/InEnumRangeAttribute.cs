using System;
using System.ComponentModel.DataAnnotations;

namespace Reversi.ValidationAttributes
{
    /// <summary>
    /// Attribute for checking index in enum range.
    /// </summary>
    public class InEnumRangeAttribute : ValidationAttribute
    {
        private Type EnumType { get; set; }

        public InEnumRangeAttribute(Type enumType)
        {
            this.EnumType = enumType;
        }

        public override bool IsValid(object value)
        {
            if (!Int32.TryParse(value.ToString(), out int index) &&
                Enum.Parse(EnumType, value.ToString()) != null)
                return true;

            int maxEnumIndex = Enum.GetValues(EnumType).Length;

            return index >= 0 && index < maxEnumIndex;
        }
    }
}
