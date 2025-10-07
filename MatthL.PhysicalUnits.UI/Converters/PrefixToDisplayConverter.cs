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
    public class PrefixToDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not Prefix prefix)
                return string.Empty;

            string paramStr = parameter as string;

            // Si le paramètre est "Name", retourner le nom pour le tooltip
            if (paramStr == "Name")
            {
                return prefix.GetName();
            }

            // Affichage principal : Symbol + valeur scientifique
            string symbol = prefix.GetSymbol();
            decimal size = prefix.GetSize();

            // Cas spécial pour SI
            if (prefix == Prefix.SI)
            {
                return "SI";
            }

            // Calculer l'exposant (log10)
            int exponent = (int)Math.Log10((double)size);

            // Formatter avec exposant Unicode (superscript)
            string exponentStr = FormatExponent(exponent);

            return $"{symbol} 10{exponentStr}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private string FormatExponent(int exponent)
        {
            // Convertir les chiffres en caractères exposants Unicode
            string expStr = exponent.ToString();
            string result = "";

            foreach (char c in expStr)
            {
                result += c switch
                {
                    '0' => '⁰',
                    '1' => '¹',
                    '2' => '²',
                    '3' => '³',
                    '4' => '⁴',
                    '5' => '⁵',
                    '6' => '⁶',
                    '7' => '⁷',
                    '8' => '⁸',
                    '9' => '⁹',
                    '-' => '⁻',
                    '+' => '⁺',
                    _ => c
                };
            }

            return result;
        }
    }
}
