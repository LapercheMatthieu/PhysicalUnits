
using Fractions;
using MatthL.PhysicalUnits.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MatthL.PhysicalUnits.Infrastructure.Extensions
{
    public static class PhysicalUnitComputationExtension
    {
        public static Func<double, double> GetToSiFunction(this PhysicalUnit Unit)
        {
            List<Func<double, double>> SingleFunctions = new List<Func<double, double>>();

            // Pour chaque BaseUnit dans l'unité physique
            foreach (var baseUnit in Unit.BaseUnits)
            {
                double exponent = baseUnit.Exponent.ToDouble();
                double offset = baseUnit.Offset;
                // Le facteur de conversion de base (déjà une Fraction)
                double factor = baseUnit.ConversionFactor.ToDouble();

                // Le facteur du préfixe en Fraction
                double prefixFactor = (double)baseUnit.Prefix.GetSize();

                // Créer la fonction pour cette BaseUnit
                Func<double, double> baseUnitFunction;

                if (baseUnit.Exponent == 0) // Exposant nul
                {
                    baseUnitFunction = x => 1.0;
                }
                else if (offset == 0) // Pas d'offset (cas simple)
                {
                    baseUnitFunction = x => Math.Pow(prefixFactor * factor * x, exponent);
                }
                else // Avec offset
                {
                    if (exponent > 0) // Exposant positif : on applique l'offset
                    {
                        baseUnitFunction = x => Math.Pow(prefixFactor * (factor * x + offset), exponent);
                    }
                    else // Exposant négatif : pas d'offset (différence de température)
                    {
                        baseUnitFunction = x => Math.Pow(prefixFactor * factor * x, exponent);
                    }
                }

                SingleFunctions.Add(baseUnitFunction);
            }

            // Combiner toutes les fonctions
            return (inputValue) =>
            {
                double result = 1.0;
                for (int i = 0; i < SingleFunctions.Count; i++)
                {
                    result *= SingleFunctions[i](inputValue);
                }
                return result;
            };
        }
        /// <summary>
        /// Convertit une valeur vers l'unité SI équivalente
        /// </summary>
        public static double ConvertToSIValue(this PhysicalUnit unit, double value)
        {
            var function = unit.GetToSiFunction();
            return function(value);
        }

        /// <summary>
        /// Convertit un tableau de valeurs vers l'unité SI équivalente
        /// </summary>
        public static double[] ConvertToSIValues(this PhysicalUnit unit, double[] values)
        {
            var function = unit.GetToSiFunction();
            var newArray = new double[values.Length];
            if (values.Length < 100_000)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    newArray[i] = function(values[i]);
                }
            }
            else
            {
                for (int i = 0; i < values.Length; i++)
                {
                    newArray[i] = function(values[i]);
                }
            }
            return newArray;
        }

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

        public static Func<double, double> GetFromSIFunction(this PhysicalUnit Unit)
        {
            List<Func<double, double>> SingleFunctions = new List<Func<double, double>>();

            // Pour chaque BaseUnit dans l'unité physique
            foreach (var baseUnit in Unit.BaseUnits)
            {
                double exponent = baseUnit.Exponent.ToDouble();
                double offset = baseUnit.Offset;
                double factor = baseUnit.ConversionFactor.ToDouble();
                double prefixFactor = (double)baseUnit.Prefix.GetSize();
                double totalFactor = prefixFactor * factor;

                // Créer la fonction INVERSE pour cette BaseUnit
                Func<double, double> baseUnitFunction;

                if (Math.Abs(exponent) < 1e-10) // Exposant nul
                {
                    baseUnitFunction = x => 1.0;
                }
                else if (offset == 0) // Pas d'offset (cas simple)
                {
                    // Inverse : x' = (x/totalFactor)^(1/exponent)
                    baseUnitFunction = x => Math.Pow(x / totalFactor, exponent);
                }
                else // Avec offset
                {
                    if (exponent > 0) // Exposant positif : on retire l'offset
                    {
                        // Inverse de y = (ax + b)^n est x = (y^(1/n) - b)/a
                        baseUnitFunction = x => Math.Pow((Math.Pow(x, 1.0 / exponent) - offset) / totalFactor, exponent);
                    }
                    else // Exposant négatif : pas d'offset
                    {
                        baseUnitFunction = x => Math.Pow(x / totalFactor, exponent);
                    }
                }

                SingleFunctions.Add(baseUnitFunction);
            }

            // Combiner toutes les fonctions
            return (inputValue) =>
            {
                double result = 1.0;
                for (int i = 0; i < SingleFunctions.Count; i++)
                {
                    result *= SingleFunctions[i](inputValue);
                }
                return result;
            };
        }

        public static Func<double, double> GetFromToSiFunction(this PhysicalUnit FromUnit, PhysicalUnit toUnit)
        {
            var toSiFunctions = FromUnit.GetToSiFunction();
            var fromSIFunctions = toUnit.GetFromSIFunction();
            return (inputValue) =>
            {
                double result = 1.0;
                result *= toSiFunctions(inputValue);
                result *= fromSIFunctions(inputValue);
                return result;
            };
        }

        public static double ConvertFromSIValue(this PhysicalUnit unit, double value)
        {
            var function = unit.GetFromSIFunction();
            return function(value);
        }
        /// <summary>
        /// Convertit un tableau de valeurs vers l'unité SI équivalente
        /// </summary>
        public static double[] ConvertFromSIValues(this PhysicalUnit unit, double[] values)
        {
            var function = unit.GetFromSIFunction();
            var newArray = new double[values.Length];
            if (values.Length < 100_000)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    newArray[i] = function(values[i]);
                }
            }
            else
            {
                for (int i = 0; i < values.Length; i++)
                {
                    newArray[i] = function(values[i]);
                }
            }
            return newArray;
        }

        public static double ConvertValue(this PhysicalUnit unit, PhysicalUnit ToUnit, double value)
        {
            var function = unit.GetFromToSiFunction(ToUnit);
            return function(value);
        }
        /// <summary>
        /// Convertit un tableau de valeurs vers l'unité SI équivalente
        /// </summary>
        public static double[] ConvertValues(this PhysicalUnit unit, PhysicalUnit ToUnit, double[] values)
        {
            var function = unit.GetFromToSiFunction(ToUnit);
            var newArray = new double[values.Length];
            if (values.Length < 100_000)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    newArray[i] = function(values[i]);
                }
            }
            else
            {
                for (int i = 0; i < values.Length; i++)
                {
                    newArray[i] = function(values[i]);
                }
            }
            return newArray;
        }
    }
}
