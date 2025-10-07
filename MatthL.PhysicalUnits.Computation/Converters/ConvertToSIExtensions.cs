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

        /// <summary>
        /// Return the function to multiply the value by to get the value in the SI dimension
        /// </summary>
        /// <param name="Unit"></param>
        /// <returns></returns>
        public static Func<double, double> GetToSiFunction(this PhysicalUnit Unit)
        {
            // Check if this is a simple unit with offset (like °C)
            bool hasOffset = Unit.BaseUnits.Any(bu => bu.Offset != 0);
            bool isSimpleUnit = Unit.BaseUnits.Count == 1 && Unit.BaseUnits.First().Exponent.ToDouble() == 1;

            if (hasOffset && isSimpleUnit)
            {
                // Special case: simple unit with offset (e.g., °C → K)
                var baseUnit = Unit.BaseUnits.First();
                double offset = baseUnit.Offset;
                double factor = baseUnit.ConversionFactor.ToDouble();
                double prefixFactor = (double)baseUnit.Prefix.GetSize();

                return (inputValue) => prefixFactor * (factor * inputValue + offset);
            }
            else
            {
                // General case: calculate total conversion factor
                double totalFactor = 1.0;

                foreach (var baseUnit in Unit.BaseUnits)
                {
                    double exponent = baseUnit.Exponent.ToDouble();
                    double factor = baseUnit.ConversionFactor.ToDouble();
                    double prefixFactor = (double)baseUnit.Prefix.GetSize();

                    if (exponent != 0)
                    {
                        totalFactor *= Math.Pow(prefixFactor * factor, exponent);
                    }
                }

                return (inputValue) => inputValue * totalFactor;
            }
        }
    }
}
