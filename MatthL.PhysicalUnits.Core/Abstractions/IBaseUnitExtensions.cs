using MatthL.PhysicalUnits.Core.DimensionFormulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatthL.PhysicalUnits.Core.Abstractions
{
    public static class IBaseUnitExtensions
    {
        public static string CalculateDimensionalFormula(this IBaseUnit baseunit)
        {
            return DimensionalFormulaHelper.GetFormulaString(this);
        }
    }
}
