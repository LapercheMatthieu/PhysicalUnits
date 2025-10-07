using MahApps.Metro.IconPacks;
using System.Globalization;
using System.Windows.Data;

namespace MatthL.PhysicalUnits.UI.Converters
{
    public class ConvertionToKindConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return PackIconPhosphorIconsKind.AtomThin; // Icône par défaut
            else
                return PackIconPhosphorIconsKind.AtomBold;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}