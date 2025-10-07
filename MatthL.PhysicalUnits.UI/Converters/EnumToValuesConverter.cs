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
    /// Convert an Enum in a list of its own values
    /// </summary>
    public class EnumToValuesConverter : IValueConverter
    {
        public static readonly EnumToValuesConverter Instance = new EnumToValuesConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return Enum.GetValues(value.GetType());
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
