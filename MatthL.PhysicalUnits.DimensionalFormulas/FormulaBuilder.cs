using MatthL.PhysicalUnits.Core.EquationModels;
using MatthL.PhysicalUnits.DimensionalFormulas.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatthL.PhysicalUnits.DimensionalFormulas
{
    public class FormulaBuilder
    {
        public static string GetDimensionalFormula(params PhysicalUnitTerm[] baseunit)
        {
            var simplified = RawUnitsSimplifier.CalculateDimensionalFormula(baseunit);
            return FormulaHelper.CreateFormulaString(RawUnitsOrderer.OrderFormula(simplified));
        }
    }
}
