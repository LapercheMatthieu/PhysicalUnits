using MatthL.PhysicalUnits.Core.EnumHelpers;
using MatthL.PhysicalUnits.Core.Enums;
using System.Globalization;
using System.Windows.Data;

namespace MatthL.PhysicalUnits.UI.Converters
{
    /// <summary>
    /// Convertit un Prefix en son symbole
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

    /// <summary>
    /// Convertit une enum en liste de ses valeurs
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

    /// <summary>
    /// Inverse un booléen
    /// </summary>
    public class InverseBooleanConverter : IValueConverter
    {
        public static readonly InverseBooleanConverter Instance = new InverseBooleanConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return false;
        }
    }
}