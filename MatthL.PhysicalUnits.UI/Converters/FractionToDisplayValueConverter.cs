using Fractions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MatthL.PhysicalUnits.UI.Converters
{
    public class FractionToDisplayValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            if (value is Fraction fraction)
            {
                // Cas spécial : dénominateur = 1
                if (fraction.Denominator == 1)
                {
                    return FormatScientific((double)fraction.Numerator);
                }

                string numeratorStr = FormatScientific((double)fraction.Numerator);
                string denominatorStr = FormatScientific((double)fraction.Denominator);

                return $"{numeratorStr} / {denominatorStr}";
            }

            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("ConvertBack is not supported for FractionToDisplayValueConverter");
        }

        private string FormatScientific(double number)
        {
            if (number == 0)
                return "0";

            // Calculer l'exposant
            int exponent = (int)Math.Floor(Math.Log10(Math.Abs(number)));

            // Calculer la mantisse
            double mantissa = number / Math.Pow(10, exponent);

            // Arrondir la mantisse à 2 décimales max
            mantissa = Math.Round(mantissa, 2);

            // Si la mantisse est proche de 10, ajuster
            if (Math.Abs(mantissa) >= 10)
            {
                mantissa /= 10;
                exponent++;
            }

            // Formater la mantisse sans zéros inutiles
            string mantissaStr;
            if (mantissa == Math.Floor(mantissa))
            {
                // Pas de décimales si c'est un entier
                mantissaStr = mantissa.ToString("F0");
            }
            else
            {
                // Jusqu'à 2 décimales, sans zéros inutiles
                mantissaStr = mantissa.ToString("0.##");
            }

            // Si l'exposant est 0, pas besoin de notation scientifique
            if (exponent == 0)
            {
                return mantissaStr;
            }

            // Formater l'exposant en superscript
            string exponentStr = FormatExponent(exponent);

            return $"{mantissaStr}×10{exponentStr}";
        }

        private string FormatExponent(int exponent)
        {
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
