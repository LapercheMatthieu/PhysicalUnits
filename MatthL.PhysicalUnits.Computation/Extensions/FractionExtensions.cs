using Fractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatthL.PhysicalUnits.Computation.Extensions
{
    /// <summary>
    /// Helpful extensions for FractionClass
    /// 
    /// </summary>
    public static class FractionExtensions
    {
        /// <summary>
        /// Élève une fraction à une puissance fractionnaire
        /// </summary>
        public static Fraction Pow(this Fraction baseValue, Fraction exponent)
        {
            // Si l'exposant est un entier, utiliser Fraction.Pow standard
            if (exponent.Denominator == 1)
            {
                return Fraction.Pow(baseValue, (int)exponent.Numerator);
            }

            // Pour les exposants fractionnaires, on doit passer par double
            // C'est le seul endroit où on perd la précision exacte
            double baseDouble = (double)baseValue.Numerator / (double)baseValue.Denominator;
            double expDouble = (double)exponent.Numerator / (double)exponent.Denominator;
            double resultDouble = Math.Pow(baseDouble, expDouble);

            // Convertir le résultat en fraction
            return Fraction.FromDouble(resultDouble);
        }
    }
}
