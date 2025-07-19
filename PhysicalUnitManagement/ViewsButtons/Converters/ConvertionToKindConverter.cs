using MahApps.Metro.IconPacks;
using  PhysicalUnitManagement.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace  PhysicalUnitManagement.ViewsButtons.Converters
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
