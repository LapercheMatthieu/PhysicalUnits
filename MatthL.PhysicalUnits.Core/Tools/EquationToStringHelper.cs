using Fractions;
using MatthL.PhysicalUnits.Enums;
using MatthL.PhysicalUnits.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatthL.PhysicalUnits.Tools
{
    /// <summary>
    /// Helper pour convertir les exposants et formater les équations
    /// </summary>
    public static class EquationToStringHelper
    {
        private static readonly Dictionary<char, char> SuperscriptMap = new Dictionary<char, char>
        {
            {'0', '⁰'}, {'1', '¹'}, {'2', '²'}, {'3', '³'}, {'4', '⁴'},
            {'5', '⁵'}, {'6', '⁶'}, {'7', '⁷'}, {'8', '⁸'}, {'9', '⁹'},
            {'+', '⁺'}, {'-', '⁻'}, {'/', 'ᐟ'} 
        };

        /// <summary>
        /// Convertit un nombre en exposant Unicode
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
        /// Convertit une fraction en exposant Unicode
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
        /// Formate une unité avec son exposant entier
        /// </summary>
        public static string FormatWithExponent(string baseSymbol, int exponent)
        {
            if (exponent == 0) return "1";
            if (exponent == 1) return baseSymbol;
            return baseSymbol + ToSuperscript(exponent);
        }

        /// <summary>
        /// Formate une unité avec son exposant fractionnaire
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
        /// Formate une fraction en notation fractionnaire
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

        /// <summary>
        /// Obtient le symbole SI pour une dimension de base
        /// </summary>
        public static string GetBaseSymbol(BaseUnitType type) => type switch
        {
            BaseUnitType.Length => "m",
            BaseUnitType.Mass => "kg",
            BaseUnitType.Time => "s",
            BaseUnitType.ElectricCurrent => "A",
            BaseUnitType.Temperature => "K",
            BaseUnitType.AmountOfSubstance => "mol",
            BaseUnitType.LuminousIntensity => "cd",
            BaseUnitType.Angle => "rad",
            BaseUnitType.Currency => "$",
            BaseUnitType.Information => "bit",
            BaseUnitType.Ratio => "1",
            _ => "?"
        };

    }
}
