using MatthL.PhysicalUnits.Core.Models;
using MatthL.PhysicalUnits.Core.EnumHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatthL.PhysicalUnits.Computation.Converters
{
    public static class ConvertToSIExtensions
    {
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
    }
}
