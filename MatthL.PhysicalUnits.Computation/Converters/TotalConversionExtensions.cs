using MatthL.PhysicalUnits.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatthL.PhysicalUnits.Computation.Converters
{
    public static class TotalConversionExtensions
    {
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
