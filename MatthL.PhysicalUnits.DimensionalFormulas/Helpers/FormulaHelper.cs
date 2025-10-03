using Fractions;
using MatthL.PhysicalUnits.Core.EnumHelpers;
using MatthL.PhysicalUnits.Core.Enums;
using System.Text;
using static MatthL.PhysicalUnits.Core.Tools.EquationToStringHelper;

namespace MatthL.PhysicalUnits.DimensionalFormulas.Helpers
{
    /// <summary>
    /// Simply transform all models into a base unit type dictionary which is the formula
    /// </summary>
    public static class FormulaHelper
    {
        /// <summary>
        /// Create the string from an ordered dictionary
        /// </summary>
        public static string CreateFormulaString(List<(BaseUnitType, Fraction)> OrderedUnits)
        {
            var builder = new StringBuilder();
            bool isinNegative = false;
            bool isFirst = true;
            foreach (var unit in OrderedUnits)
            {
                if (unit.Item2 < 0 && isinNegative == false)
                {
                    if (isFirst)
                    {
                        builder.Append("1/");
                        isinNegative = true;
                        isFirst = false;
                    }
                    else
                    {
                        builder.Append("/");
                        isinNegative = true;
                    }
                }
                else if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    builder.Append("·");
                }
                if (unit.Item2 > 0)
                {
                    builder.Append(FormatWithExponent(unit.Item1.GetBaseSymbol(), unit.Item2));
                }
                else
                {
                    builder.Append(FormatWithExponent(unit.Item1.GetBaseSymbol(), -unit.Item2));
                }
            }
            return builder.ToString();
        }
    }
}