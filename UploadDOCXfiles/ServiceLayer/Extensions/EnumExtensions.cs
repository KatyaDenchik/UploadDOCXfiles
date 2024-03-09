using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Converts the specified string value to the corresponding enumeration value of type TEnum.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enumeration.</typeparam>
        /// <param name="value">The string representation of the enumeration value.</param>
        /// <returns>
        /// The enumeration value of type TEnum that corresponds to the specified string value.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when the string value cannot be converted to the enumeration type TEnum.</exception>
        public static TEnum ToEnum<TEnum>(this string value) where TEnum : struct
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase: true);
        }
    }
}
