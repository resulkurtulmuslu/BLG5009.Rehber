using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BLG5009.Rehber.WebApp.Methods
{
    public static class EnumExtensionMethods
    {
        public static IEnumerable<KeyValuePair<T, string>> GetDescriptionList<T>()
        {
            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(p => new KeyValuePair<T, string>(p,
                    (p.GetType().GetField(p.ToString())
                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .FirstOrDefault() as DescriptionAttribute)
                    ?.Description ?? p.ToString()))
                    .ToList();
        }

        public static string GetDescription(this Enum enumValue)
        {
            object[] attr = enumValue.GetType().GetField(enumValue.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attr.Length > 0
               ? ((DescriptionAttribute)attr[0]).Description
               : enumValue.ToString();
        }
    }

}
