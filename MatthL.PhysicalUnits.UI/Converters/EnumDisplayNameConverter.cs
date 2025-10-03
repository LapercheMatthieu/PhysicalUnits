using MatthL.PhysicalUnits.Core.EnumHelpers;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace MatthL.PhysicalUnits.UI.Converters
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
            var displayNameAttribute = field.GetCustomAttribute<DisplayedNameAttribute>();
            if (displayNameAttribute != null)
                return displayNameAttribute.DisplayedName;

            // Si pas d'attribut, retourner le nom brut
            return name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}