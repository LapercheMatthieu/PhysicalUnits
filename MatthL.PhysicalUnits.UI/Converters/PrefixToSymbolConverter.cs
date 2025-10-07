using MatthL.PhysicalUnits.Core.EnumHelpers;
using MatthL.PhysicalUnits.Core.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MatthL.PhysicalUnits.UI.Converters
{
    /// <summary>
    /// Convertit a prefix in its symbol
    /// </summary>
    public class PrefixToSymbolConverter : IValueConverter
    {
        public static readonly PrefixToSymbolConverter Instance = new PrefixToSymbolConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Prefix prefix)
            {
                return prefix.GetSymbol();
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
