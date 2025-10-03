using Fractions;

namespace MatthL.PhysicalUnits.Core.Tools
{
    /// <summary>
    /// Helper to build equations in string
    /// </summary>
    public static class EquationToStringHelper
    {
        /// <summary>
        /// The Super script mapping dictionary
        /// </summary>
        private static readonly Dictionary<char, char> SuperscriptMap = new Dictionary<char, char>
        {
            {'0', '⁰'}, {'1', '¹'}, {'2', '²'}, {'3', '³'}, {'4', '⁴'},
            {'5', '⁵'}, {'6', '⁶'}, {'7', '⁷'}, {'8', '⁸'}, {'9', '⁹'},
            {'+', '⁺'}, {'-', '⁻'}, {'/', 'ᐟ'}
        };

        /// <summary>
        /// Convert a number into its superscript power
        /// </summary>
        public static string ToSuperscript(int number)
        {
            if (number == 0) return "⁰";
            if (number == 1) return "";  // Pas d'exposant pour 1

            var result = "";
            var isNegative = number < 0;

            if (isNegative)
            {
                result = "⁻";
                number = -number;
            }

            foreach (var digit in number.ToString())
            {
                if (SuperscriptMap.TryGetValue(digit, out char superscript))
                    result += superscript;
                else
                    result += digit;
            }

            return result;
        }

        /// <summary>
        /// Convert a fraction into its superscript power
        /// </summary>
        public static string ToSuperscript(Fraction fraction)
        {
            if (fraction == 0) return "⁰";
            if (fraction == 1) return "";  // Pas d'exposant pour 1

            // Si c'est un entier
            if (fraction.Denominator == 1)
            {
                return ToSuperscript((int)fraction.Numerator);
            }

            // Si c'est une fraction
            var result = "";

            // Gérer le signe négatif
            if (fraction < 0)
            {
                result = "⁻";
                fraction = -fraction;
            }

            // Convertir numérateur
            foreach (var digit in fraction.Numerator.ToString())
            {
                if (SuperscriptMap.TryGetValue(digit, out char superscript))
                    result += superscript;
            }

            // Ajouter le slash
            result += "ᐟ";

            // Convertir dénominateur
            foreach (var digit in fraction.Denominator.ToString())
            {
                if (SuperscriptMap.TryGetValue(digit, out char superscript))
                    result += superscript;
            }

            return result;
        }

        /// <summary>
        /// combine the symbol and its exponent as int
        /// </summary>
        public static string FormatWithExponent(string baseSymbol, int exponent)
        {
            if (exponent == 0) return "1";
            if (exponent == 1) return baseSymbol;
            return baseSymbol + ToSuperscript(exponent);
        }

        /// <summary>
        ///  combine the symbol and its exponent as Fraction
        /// </summary>
        public static string FormatWithExponent(string baseSymbol, Fraction exponent)
        {
            if (exponent == 0) return "1";
            if (exponent == 1) return baseSymbol;

            // Si l'exposant est un entier simple
            if (exponent.Denominator == 1)
            {
                return baseSymbol + ToSuperscript((int)exponent.Numerator);
            }

            // Si c'est une vraie fraction, on peut choisir entre deux formats :
            // Format 1: m^(1/2) avec parenthèses
            // return $"{baseSymbol}^({exponent.Numerator}/{exponent.Denominator})";

            // Format 2: m¹ᐟ² en exposant complet
            return baseSymbol + ToSuperscript(exponent);
        }

        /// <summary>
        /// Format the factor as fraction to a string
        /// </summary>
        public static string FormatFactor(Fraction factor)
        {
            // Si c'est un entier
            if (factor.Denominator == 1)
            {
                return factor.Numerator.ToString();
            }

            // Si c'est une fraction
            return $"{factor.Numerator}/{factor.Denominator}";
        }
    }
}