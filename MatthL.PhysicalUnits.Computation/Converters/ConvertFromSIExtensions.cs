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
            // Vérifier si c'est une unité simple avec offset
            bool hasOffset = Unit.BaseUnits.Any(bu => bu.Offset != 0);
            bool isSimpleUnit = Unit.BaseUnits.Count == 1 && Math.Abs(Unit.BaseUnits.First().Exponent.ToDouble() - 1.0) < 1e-10;

            if (hasOffset && isSimpleUnit)
            {
                // Cas spécial : unité simple avec offset (ex: K → °C)
                var baseUnit = Unit.BaseUnits.First();
                double offset = baseUnit.Offset;
                double factor = baseUnit.ConversionFactor.ToDouble();
                double prefixFactor = (double)baseUnit.Prefix.GetSize();

                // Inverse de : y = prefixFactor * (factor * x + offset)
                // Donc : x = (y / prefixFactor - offset) / factor
                return (inputValue) => (inputValue / prefixFactor - offset) / factor;
            }
            else
            {
                // Cas général : calculer le facteur de conversion global
                double totalFactor = 1.0;

                foreach (var baseUnit in Unit.BaseUnits)
                {
                    double exponent = baseUnit.Exponent.ToDouble();
                    double factor = baseUnit.ConversionFactor.ToDouble();
                    double prefixFactor = (double)baseUnit.Prefix.GetSize();

                    if (Math.Abs(exponent) > 1e-10)
                    {
                        totalFactor *= Math.Pow(prefixFactor * factor, exponent);
                    }
                }

                // Inverse de : y = x * totalFactor
                // Donc : x = y / totalFactor
                return (inputValue) => inputValue / totalFactor;
            }
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
