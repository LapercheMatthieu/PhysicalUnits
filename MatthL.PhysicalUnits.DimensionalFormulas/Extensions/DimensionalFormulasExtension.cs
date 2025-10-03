using MatthL.PhysicalUnits.Core.Abstractions;
using MatthL.PhysicalUnits.Core.EquationModels;
using MatthL.PhysicalUnits.Core.Models;
using MatthL.PhysicalUnits.DimensionalFormulas.Helpers;

namespace MatthL.PhysicalUnits.DimensionalFormulas.Extensions
{
    public static class DimensionalFormulasExtension
    {
        public static string GetDimensionalFormula(this IBaseUnit baseunit)
        {
            if (baseunit is PhysicalUnit physicalType)
            {
                var simplified = RawUnitsSimplifier.CalculateDimensionalFormula(physicalType);
                return FormulaHelper.CreateFormulaString(RawUnitsOrderer.OrderFormula(simplified));
            }
            else if (baseunit is BaseUnit basetype)
            {
                var simplified = RawUnitsSimplifier.CalculateDimensionalFormula(basetype);
                return FormulaHelper.CreateFormulaString(RawUnitsOrderer.OrderFormula(simplified));
            }
            else return "";
        }

        public static string GetDimensionalFormula(this PhysicalUnitTerm baseunit)
        {
            var simplified = RawUnitsSimplifier.CalculateDimensionalFormula(baseunit);
            return FormulaHelper.CreateFormulaString(RawUnitsOrderer.OrderFormula(simplified));
        }

        public static string GetDimensionalFormula(this EquationTerms baseunit)
        {
            var simplified = RawUnitsSimplifier.CalculateDimensionalFormula(baseunit.Terms.ToArray());
            return FormulaHelper.CreateFormulaString(RawUnitsOrderer.OrderFormula(simplified));
        }

        public static string GetDimensionalFormula(this RawUnit baseunit)
        {
            var simplified = RawUnitsSimplifier.CalculateDimensionalFormula(baseunit);
            return FormulaHelper.CreateFormulaString(RawUnitsOrderer.OrderFormula(simplified));
        }
    }
}