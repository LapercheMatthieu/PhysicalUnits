using MatthL.PhysicalUnits.Core.EnumHelpers;
using MatthL.PhysicalUnits.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatthL.PhysicalUnits.Computation.Converters
{
    public static class ConvertFromSIExtensions
    {
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

    }
}
