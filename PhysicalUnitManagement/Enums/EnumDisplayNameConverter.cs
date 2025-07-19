using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace  PhysicalUnitManagement.Enums
{
    public class EnumDisplayNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return string.Empty;

            // Obtenir le type d'enum
            Type enumType = value.GetType();
            if (!enumType.IsEnum) return value.ToString();

            // Obtenir le nom du champ de l'enum
            string name = Enum.GetName(enumType, value);
            if (name == null) return value.ToString();

            // Obtenir le FieldInfo pour accéder aux attributs
            FieldInfo field = enumType.GetField(name);
            if (field == null) return value.ToString();

            // Rechercher l'attribut DisplayName
            var displayNameAttribute = field.GetCustomAttribute<DisplayNameAttribute>();
            if (displayNameAttribute != null)
                return displayNameAttribute.DisplayName;

            // Si pas d'attribut, retourner le nom brut
            return name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
